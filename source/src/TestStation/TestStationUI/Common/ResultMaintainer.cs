using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using Testflow.Data.Sequence;
using Testflow.Runtime;
using Testflow.Runtime.Data;
using TestFlow.SoftDevCommon;
using TestFlow.SoftDSevCommon;

namespace TestFlow.DevSoftware.Common
{
    public class ResultMaintainer : IDisposable
    {
        private ReaderWriterLockSlim _writerLock;

        private readonly Dictionary<string, string> _variableValue;
        private readonly Dictionary<int, IList<StepResultInfo>> _stepResults;

        public ResultMaintainer(ISequenceGroup sequenceData)
        {
            _variableValue = new Dictionary<string, string>(100);
            _stepResults = new Dictionary<int, IList<StepResultInfo>>(10);
            _writerLock = new ReaderWriterLockSlim();
            GenerateResultStructure(sequenceData);
        }

        private void GenerateResultStructure(ISequenceGroup sequenceData)
        {
            AddVariable(sequenceData.Variables);
            AddVariable(sequenceData.SetUp.Variables);
            AddVariable(sequenceData.TearDown.Variables);
            foreach (ISequence sequence in sequenceData.Sequences)
            {
                AddVariable(sequence.Variables);
            }
            AddStepInfo(sequenceData.SetUp);
            AddStepInfo(sequenceData.TearDown);
            foreach (ISequence sequence in sequenceData.Sequences)
            {
                AddStepInfo(sequence);
            }
        }

        private void AddVariable(IVariableCollection variables)
        {
            foreach (IVariable variable in variables)
            {
                string runVariableName = variable.Name;
                string value = variable.Value ?? string.Empty;
                if (!_variableValue.ContainsKey(runVariableName))
                {
                    _variableValue.Add(runVariableName, value);
                }
            }
        }

        private void AddStepInfo(ISequence sequence)
        {
            List<StepResultInfo> stepResultInfos = new List<StepResultInfo>(sequence.Steps.Count);
            _stepResults.Add(sequence.Index, stepResultInfos);
            foreach (ISequenceStep step in sequence.Steps)
            {
                string limitVariable = string.Empty;
                ISequenceStep limitStep;
                if (step.HasSubSteps &&
                    null != (limitStep = step.SubSteps.FirstOrDefault(item => item.Name.StartsWith(Constants.LimitStepPrefix))))
                {
                    limitVariable = limitStep.Function.Parameters[3].Value;
                }
                stepResultInfos.Add(new StepResultInfo(string.Empty, limitVariable, sequence.Index));
            }
        }

        public IDictionary<string, string> UpdateResult(IRuntimeStatusInfo statusinfo, ICallStack currentStack, ISequenceStep step, bool isLimitStep)
        {
            _writerLock.EnterWriteLock();
            Dictionary<string, string> watchDatas = null;
            if (null != statusinfo.WatchDatas)
            {
                watchDatas = new Dictionary<string, string>(statusinfo.WatchDatas.Count);
                foreach (KeyValuePair<IVariable, string> keyValuePair in statusinfo.WatchDatas)
                {
                    IVariable variable = keyValuePair.Key;
                    int seqIndex = variable.Parent is ISequenceGroup
                        ? Constants.SequenceGroupIndex
                        : ((ISequence) variable.Parent).Index;
                    string value = keyValuePair.Value;
//                    string rawVarName = _seqMaintainer.GetRawVariable(seqIndex, variable.Name);
                    // 返回原始变量名到变量值的映射
//                    watchDatas.Add(rawVarName, value);
                    _variableValue[variable.Name] = value;
                }
            }
            if (null != step)
            {
                ResultState result = GetStepResult(statusinfo, currentStack, step);
                ISequence sequence = (ISequence) step.Parent;
                ResultState oldResult = _stepResults[sequence.Index][step.Index].Result;
                if (result > oldResult)
                {
                    _stepResults[sequence.Index][step.Index].Result = result;
                }
                // 将已运行，单状态为更新的Step标记为Pass
                IList<StepResultInfo> resultInfos = _stepResults[sequence.Index];
                for (int i = step.Index - 1; i >= 0; i--)
                {
                    StepResultInfo resultInfo = resultInfos[i];
                    if (resultInfo.Result < ResultState.Skip)
                    {
                        resultInfo.Result = ResultState.Pass;
                        if (!string.IsNullOrWhiteSpace(resultInfo.LimitVariable) && null == resultInfo.LimitValue)
                        {
                            resultInfo.LimitValue = _variableValue[resultInfo.LimitVariable];
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                StepResultInfo currentResult = _stepResults[sequence.Index][step.Index];
                if (!string.IsNullOrWhiteSpace(currentResult.LimitVariable) && null == currentResult.LimitValue && isLimitStep)
                {
                    currentResult.LimitValue = _variableValue[currentResult.LimitVariable];
                }
            }
            _writerLock.ExitWriteLock();
            return watchDatas;
        }

        public void GetSerialNumber(out string serialNumber, out int uutIndex)
        {
            try
            {
                _writerLock.EnterReadLock();

                const string serialNoVarName = "0$" + Constants.SerialNoVarName;
                const string uutIndexVarName = "0$" + Constants.UutIndexVar;
                serialNumber =  _variableValue.ContainsKey(serialNoVarName) && !string.IsNullOrWhiteSpace(_variableValue[serialNoVarName])
                    ? _variableValue[serialNoVarName]
                    : Constants.NASerialNo;
                if (_variableValue.ContainsKey(uutIndexVarName))
                {
                    if (!int.TryParse(_variableValue[uutIndexVarName], out uutIndex))
                    {
                        uutIndex = -1;
                    }
                }
                else
                {
                    uutIndex = -1;
                }
            }
            finally
            {
                _writerLock.ExitReadLock();
            }
        }

        public void GetResultsAndVariables(int sequenceIndex, out IList<ResultState> results, out IList<string> variableValues)
        {
            try
            {
                _writerLock.EnterReadLock();

                if (!_stepResults.ContainsKey(sequenceIndex))
                {
                    results = new List<ResultState>(0);
                    variableValues = new List<string>(0);
                    return;
                }
                IList<StepResultInfo> sequenceResults = _stepResults[sequenceIndex];
                results = new List<ResultState>(sequenceResults.Count);
                variableValues = new List<string>(sequenceResults.Count);
                foreach (StepResultInfo sequenceResult in sequenceResults)
                {
                    results.Add(sequenceResult.Result);
                    variableValues.Add(sequenceResult.LimitValue ?? string.Empty);
                }
            }
            finally
            {
                _writerLock.ExitReadLock();
            }
        }

        public void Reset()
        {
            _writerLock.EnterWriteLock();

            foreach (IList<StepResultInfo> stepResults in _stepResults.Values)
            {
                foreach (StepResultInfo stepResult in stepResults)
                {
                    stepResult.Reset();
                }
            }
            foreach (string key in _variableValue.Keys)
            {
                _variableValue[key] = string.Empty;
            }

            _writerLock.ExitWriteLock();
        }

        public void Reset(int sequenceIndex)
        {
            _writerLock.EnterWriteLock();
            if (!_stepResults.ContainsKey(sequenceIndex))
            {
                return;
            }
            foreach (StepResultInfo stepResultInfo in _stepResults[sequenceIndex])
            {
                stepResultInfo.Reset();
            }
            _writerLock.ExitWriteLock();
        }

        public void ResetUutResult()
        {
            _writerLock.EnterWriteLock();
            foreach (int key in _stepResults.Keys)
            {
                if (key >= 0)
                {
                    foreach (StepResultInfo stepResultInfo in _stepResults[key])
                    {
                        stepResultInfo.Reset();
                    }
                }
            }
            _writerLock.ExitWriteLock();
        }


        private static ResultState GetStepResult(IRuntimeStatusInfo statusinfo, ICallStack currentStack, ISequenceStep step)
        {
            ResultState result = ResultState.NA;
            if (null == statusinfo.StepResults || !statusinfo.StepResults.ContainsKey(currentStack))
            {
                result = ResultState.NA;
            }
            else
            {
                switch (statusinfo.StepResults[currentStack])
                {
                    case StepResult.NotAvailable:
                        result = ResultState.Running;
                        break;
                    case StepResult.Skip:
                        result = ResultState.Skip;
                        break;
                    case StepResult.Pass:
                        result = Utility.IsActionStep(step) ? ResultState.Done : ResultState.Pass;
                        break;
                    case StepResult.RetryFailed:
                        result = ResultState.Running;
                        break;
                    case StepResult.Failed:
                    case StepResult.Abort:
                    case StepResult.Timeout:
                    case StepResult.Error:
                        result = ResultState.Fail;
                        break;
                    case StepResult.Over:
                        result = ResultState.NA;
                        break;
                }
            }
            return result;
        }

        public void Dispose()
        {
            this._writerLock?.Dispose();
        }
    }
}