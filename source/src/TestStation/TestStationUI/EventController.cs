using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Testflow.Data;
using Testflow.Data.Sequence;
using Testflow.Modules;
using Testflow.Runtime;
using Testflow.Runtime.Data;
using Testflow.Utility.Utils;
using TestFlow.DevSoftware.Common;
using TestFlow.SoftDevCommon;
using TestFlow.SoftDSevCommon;

namespace TestFlow.DevSoftware
{
    internal class EventController : IDisposable
    {
        private readonly GlobalInfo _globalInfo;
        private readonly MainForm _mainform;
        private volatile string _serialNumber;
        private ResultMaintainer _resultMaintainer;
        private ISequenceGroup _sequenceData;
        private int _uutIndex;

        public EventController(GlobalInfo globalInfo, ISequenceGroup sequenceDataData, 
            MainForm mainform)
        {
            _globalInfo = globalInfo;
            this._mainform = mainform;
            DataCache = null;
            _serialNumber = string.Empty;
            _sequenceData = sequenceDataData;
            _resultMaintainer = new ResultMaintainer(sequenceDataData);
            _uutIndex = -1;
        }

        public RuntimeDataCache DataCache { get; set; }

        public void RegisterEvents()
        {
            IEngineController engineController = _globalInfo.TestflowEntity.EngineController;
            engineController.RegisterRuntimeEvent(new RuntimeDelegate.TestGenerationAction(TestGenStart), "TestGenerationStart",
                0);
            engineController.RegisterRuntimeEvent(new RuntimeDelegate.TestGenerationAction(TestGenOver), "TestGenerationEnd", 0);


            engineController.RegisterRuntimeEvent(new RuntimeDelegate.SequenceStatusAction(SequenceStarted), "SequenceStarted",
                0);
            engineController.RegisterRuntimeEvent(new RuntimeDelegate.StatusReceivedAction(StatusReceived), "StatusReceived", 0);
            engineController.RegisterRuntimeEvent(new RuntimeDelegate.SequenceStatusAction(SequenceOver), "SequenceOver", 0);
            engineController.RegisterRuntimeEvent(new RuntimeDelegate.TestInstanceStatusAction(TestInstanceOver),
                "TestInstanceOver", 0);
            engineController.RegisterRuntimeEvent(new RuntimeDelegate.BreakPointHittedAction(BreakPointHitted), "BreakPointHitted", 0);
        }

        private void BreakPointHitted(IDebuggerHandle debuggerhandle, IDebugInformation information)
        {

            Dictionary<string, string> watchDatas = new Dictionary<string, string>(information.WatchDatas.Count);
            foreach (KeyValuePair<IVariable, string> keyValuePair in information.WatchDatas)
            {
                IVariable variable = keyValuePair.Key;
                string value = keyValuePair.Value;
                string variableName = variable.Name;
                watchDatas.Add(variableName, value);
            }

            ISequenceStep step = SequenceUtils.GetStepFromStack(_sequenceData, information.BreakPoint);
            if (null == step)
            {
                return;
            }
            ISequence sequence = (ISequence)step.Parent;
            IList<ResultState> results;
            IList<string> variables;
            _resultMaintainer.GetResultsAndVariables(sequence.Index, out results, out variables);
            _mainform.Invoke(new Action(() =>
            {
                _mainform.BreakPointHittedAction(step, results, variables, watchDatas);
            }));
        }

        private void TestGenStart(ITestGenerationInfo generationInfo)
        {
            _mainform.Invoke(new Action(() =>
            {
                _mainform.AppendOutput("Test generation start...");
            }));
        }

        private void TestGenOver(ITestGenerationInfo generationInfo)
        {
            _mainform.Invoke(new Action(() =>
            {
                ISessionGenerationInfo sessionGenerationInfo = generationInfo.GenerationInfos[0];
                if (sessionGenerationInfo.Status == GenerationStatus.Success)
                {
                    _mainform.AppendOutput("Test generation success.");
                }
                else
                {
                    _mainform.AppendOutput("Test generation failed.");
                    if (string.IsNullOrWhiteSpace(sessionGenerationInfo.ErrorInfo) &&
                        null != sessionGenerationInfo.ErrorStack)
                    {
                        ISequenceStep errorStep = SequenceUtils.GetStepFromStack(_sequenceData,
                            sessionGenerationInfo.ErrorStack);
                        _mainform.AppendOutput($"ErrorStep:{errorStep.Name}  ErrorInfo:{sessionGenerationInfo.ErrorInfo}");
                    }
                    _mainform.RunningOver();
                }
            }));
        }
        
        private void SequenceStarted(ISequenceTestResult statistics)
        {
            ISequence sequence = SequenceUtils.GetSequence(_sequenceData, 0, statistics.SequenceIndex);
            string printInfo = $"{sequence.Name} started.";
            _mainform.Invoke(new Action(() =>
            {
                _mainform.AppendOutput(printInfo);
            }));
        }

        private void StatusReceived(IRuntimeStatusInfo statusinfo)
        {
            List<string> printInfos = new List<string>(5);
            if (statusinfo.FailedInfos.Count > 0)
            {
                foreach (IFailedInfo failedInfo in statusinfo.FailedInfos.Values)
                {
                    printInfos.Add(failedInfo.Message);
                }
            }
//            ICallStack currentStack = GetCurrentStack(statusinfo);
//            string stackStr = currentStack.ToString();
//            if (stackStr.Equals(_seqMaintainer.MainStartStack))
//            {
//                _resultMaintainer.ResetUutResult();
//            }
//            ISequenceStep step = _seqMaintainer.GetStepByRawStack(stackStr);
//            ISequence currentSequence = null != step ? Utility.GetParentSequence(step) : null;
//            _currentSequence = currentSequence;
//            if (null == currentSequence)
//            {
//                return;
//            }
//            ISequenceStep rawStep = _seqMaintainer.GetStepByStack(currentStack);
//            bool isLimitStep = rawStep.Function?.ClassType.Name.Equals("Limit") ?? false;
//            IDictionary<string, string> watchDatas = _resultMaintainer.UpdateResult(statusinfo,currentStack, step, isLimitStep);
//            string serialNumber;
//            int uutIndex;
//            _resultMaintainer.GetSerialNumber(out serialNumber, out uutIndex);
//            if (uutIndex > _uutIndex)
//            {
//                Thread.VolatileWrite(ref _uutIndex, uutIndex);
//            }
//            bool isPreUutStack = Utility.IsPreUutStack(stackStr, _seqMaintainer.MainStartStack);
//            bool isPostUutStack = Utility.IsPostUutStack(stackStr, _seqMaintainer.PostStartStack);
//            bool isMainStack = !isPostUutStack && !isPreUutStack && currentStack.Sequence == 0;
//            ISequenceStepCollection mainSeqSteps = _sequenceData.Sequences[1].Steps;
//            int sequenceIndex = currentSequence.Index;
//            if (isPreUutStack)
//            {
//                step = mainSeqSteps.Count > 0 ? mainSeqSteps[0] : null;
//                sequenceIndex = 1;
//            }
//            else if (isPostUutStack)
//            {
//                step = mainSeqSteps.Count > 0 ? mainSeqSteps[mainSeqSteps.Count - 1] : null;
//                sequenceIndex = 1;
//            }
//            else if (currentSequence.Index < 0)
//            {
//                step = null;
//                sequenceIndex = 1;
//            }
//            IList<ResultState> stepResults;
//            IList<string> sequenceVariableValues;
//            _resultMaintainer.GetResultsAndVariables(sequenceIndex, out stepResults, out sequenceVariableValues);
//            _mainform.Invoke(new Action(() =>
//            {
//                if (printInfos.Count > 0)
//                {
//                    _mainform.AppendOutput(string.Join(Environment.NewLine, printInfos));
//                }
//                _mainform.RefreshVariableValues(watchDatas);
//                if (null != step)
//                {
//                    _mainform.RefreshStepResult(step, stepResults, sequenceVariableValues);
//                }
//            }));
        }

        private ICallStack GetCurrentStack(IRuntimeStatusInfo statusinfo)
        {
            if (statusinfo.StepResults?.Count > 0)
            {
                foreach (ICallStack callStack in statusinfo.StepResults.Keys)
                {
                    return callStack;
                }
            }
            for (int i = statusinfo.CallStacks.Count - 1; i >= 0; i--)
            {
                if (statusinfo.SequenceState[i] > RuntimeState.StartIdle && statusinfo.SequenceState[i] < RuntimeState.Success)
                {
                    return statusinfo.CallStacks[i];
                }
            }
            return statusinfo.CallStacks[0];
        }


        private void SequenceOver(ISequenceTestResult statistics)
        {
            string printInfo;
            if (statistics.SequenceIndex == -1)
            {
                printInfo = "ProcessSetUp over.";
            }
            else if (statistics.SequenceIndex == -2)
            {
                printInfo = "ProcessCleanUp over.";
            }
            else
            {
                printInfo = "MainSequence over.";
            }
            _mainform.Invoke(new Action(() => { _mainform.AppendOutput(printInfo); }));
        }


        private void TestInstanceOver(IList<ITestResultCollection> statistics)
        {
            UnRegisterEvent();
            _mainform.Invoke(new Action(() =>
            {
                _mainform.RunningOver();
            }));
        }

        private void ReportPrintOver()
        {
        }

        public void UnRegisterEvent()
        {
            IEngineController engineController = _globalInfo.TestflowEntity.EngineController;
            engineController.UnregisterRuntimeEvent(new RuntimeDelegate.TestGenerationAction(TestGenStart), "TestGenerationStart", 0);
            engineController.UnregisterRuntimeEvent(new RuntimeDelegate.TestGenerationAction(TestGenOver), "TestGenerationEnd", 0);


            engineController.UnregisterRuntimeEvent(new RuntimeDelegate.SequenceStatusAction(SequenceStarted), "SequenceStarted", 0);
            engineController.UnregisterRuntimeEvent(new RuntimeDelegate.StatusReceivedAction(StatusReceived), "StatusReceived", 0);
            engineController.UnregisterRuntimeEvent(new RuntimeDelegate.SequenceStatusAction(SequenceOver), "SequenceOver", 0);
            engineController.UnregisterRuntimeEvent(new RuntimeDelegate.TestInstanceStatusAction(TestInstanceOver),
                "TestInstanceOver", 0);
        }

        public void Dispose()
        {
        }
    }
}