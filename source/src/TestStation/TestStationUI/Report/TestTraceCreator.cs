using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Testflow.Data.Sequence;
using Testflow.Runtime.Data;
using TestStation.Common;
using TestStation.OperationPanel;
using TestStation.Report.ReportCreator;

namespace TestStation.Report
{
    internal class TestTraceCreator
    {
        private readonly GlobalInfo _globalInfo;
        private readonly SequenceMaintainer _seqMaintainer;

        public string ReportDir { get; private set; }

        public TestTraceCreator(GlobalInfo globalInfo, SequenceMaintainer seqMaintainer)
        {
            _globalInfo = globalInfo;
            _seqMaintainer = seqMaintainer;
        }

        public IList<ProductTestResult> GetProductTestResults(OiDataCache dataCache)
        {
            ReportGlobalInfo reportGlobalInfo = new ReportGlobalInfo()
            {
                Operator = _globalInfo.Session.UserName,
                SocketIndex = "0",
                StationId = "Station Id",
                SequencePath = _seqMaintainer.GetSequencePath()
            };
            ReportCreatorBase reportCreator = null;
            try
            {
                string runtimeHash = _globalInfo.TestflowEntity.EngineController.GetRuntimeInfo<string>("RuntimeHash");

                TestInstanceData testInstanceData = _globalInfo.TestflowEntity.DataMaintainer.GetTestInstance(runtimeHash);
                string reportDir = Utility.GetReportDir(testInstanceData.StartTime);
                ReportDir = reportDir;
                reportCreator = ReportCreatorBase.GetReportCreator(ReportType.Txt, reportGlobalInfo, reportDir);

                IList<RuntimeStatusData> runtimeStatusDatas =
                    _globalInfo.TestflowEntity.DataMaintainer.GetRuntimeStatus(runtimeHash, 0);

                string mainOverStack = _seqMaintainer.MainOverStack;
                string mainStartStack = _seqMaintainer.MainStartStack;

                ProductTestResult currentProduct = null;
                List<ProductTestResult> productTestResults = new List<ProductTestResult>(100);
                bool currentUutStart = false;
                int currentUutIndex = -1;
                bool uutPassed = true;
                foreach (RuntimeStatusData runtimeStatusData in runtimeStatusDatas)
                {
                    if (runtimeStatusData.Sequence == -1 || (runtimeStatusData.Result == StepResult.Over && runtimeStatusData.Sequence != -2))
                    {
                        continue;
                    }
                    Dictionary<string, string> watchData = GetWatchData(runtimeStatusData);
                    int uutIndex = GetIntVaraible(watchData, Constants.UutIndexVar);
                    // 如果UUT更新，并且当前堆栈等于uut的入口，则UUT开始执行
                    
                    if (uutIndex > currentUutIndex && mainStartStack.Equals(runtimeStatusData.Stack))
                    {
                        currentProduct = new ProductTestResult();
                        currentProduct.Model = dataCache.Model;
                        UpdateKeyVariables(Constants.SerialNoVarName, currentProduct, watchData, runtimeStatusData);
                        
                        uutPassed = true;
                        reportCreator.BeginPrint(runtimeStatusData.ExecutionTime);
                        currentUutStart = true;
                    }
                    if (currentUutStart)
                    {
                        bool stepPassed = ProcessRuntimeInfo(Constants.SerialNoVarName, currentProduct,
                            runtimeStatusData, watchData);
                        uutPassed &= stepPassed;
                        ISequenceStep rawStep = _seqMaintainer.GetStepByRawStack(runtimeStatusData.Stack);
                        ISequenceStep runStep = _seqMaintainer.GetRuntimeStep(runtimeStatusData.Stack);
                        reportCreator.PrintSingleInfo(runtimeStatusData, rawStep, runStep);
                    }
                    if (currentUutStart && runtimeStatusData.Stack.Equals(mainOverStack))
                    {
                        if (IsValidSerialNumber(currentProduct.SerialNo))
                        {
                            currentProduct.Result = uutPassed ? ResultState.Pass : ResultState.Failed;
                            WriteProductTestOverData(currentProduct, runtimeStatusData);
                            productTestResults.Add(currentProduct);
                            reportCreator.SetStartTime(currentProduct.StartTime);
                            reportCreator.SetEndTime(currentProduct.EndTime);
                            reportCreator.EndPrint();
                            currentProduct = null;
                            currentUutStart = false;
                        }
                        else
                        {
                            reportCreator.DeleteCurrent();
                        }
                    }
                }
                if (productTestResults.Count > 0 && productTestResults[productTestResults.Count - 1].SerialNo.Equals(Constants.NASerialNo))
                {
                    productTestResults.RemoveAt(productTestResults.Count - 1);
                }

                return productTestResults;
            }
            finally
            {
                reportCreator?.Dispose();
            }
        }

        private bool IsValidSerialNumber(string serialiNumber)
        {
            return !string.IsNullOrWhiteSpace(serialiNumber) &&
                   !Constants.NASerialNo.Equals(serialiNumber);
        }

        private void WriteProductTestOverData(ProductTestResult currentProduct, RuntimeStatusData runtimeStatus)
        {
            if (!_seqMaintainer.UserEndTiming || currentProduct.EndTime == DateTime.MinValue)
            {
                currentProduct.EndTime = runtimeStatus.ExecutionTime;
            }
            currentProduct.ElapsedTime = (currentProduct.EndTime - currentProduct.StartTime).TotalMilliseconds/1000;
            currentProduct.Result = currentProduct.TestResult.Values.Any(item => item != ResultState.Pass && item != ResultState.Skip)
                ? ResultState.Failed
                : ResultState.Pass;
        }

        private bool ProcessRuntimeInfo(string serialNoVar, ProductTestResult currentProduct, 
            RuntimeStatusData runtimeStatusData, Dictionary<string, string> watchDatas)
        {
            UpdateKeyVariables(serialNoVar, currentProduct, watchDatas, runtimeStatusData);
            ISequenceStep step = _seqMaintainer.GetStepByRawStack(runtimeStatusData.Stack);
            if (null == step || !step.RecordStatus)
            {
                return true;
            }
            bool stepPassed = false;
            ResultState result;
            switch (runtimeStatusData.Result)
            {
                case StepResult.Pass:
                case StepResult.RetryFailed:
                case StepResult.Over:
                    result = ResultState.Pass;
                    stepPassed = true;
                    break;
                case StepResult.Skip:
                    result = ResultState.Skip;
                    stepPassed = true;
                    break;
                default:
                    result = ResultState.Failed;
                    break;
            }
            // 如果不包含当前step的结果，则直接添加
            if (!currentProduct.TestResult.ContainsKey(step.Name))
            {
                currentProduct.TestResult.Add(step.Name, result);
            }
            // 如果已经包含当前步骤的结果，则如果新的结果更大(Failed > Pass)，则记录最新的结果
            else if (result > currentProduct.TestResult[step.Name])
            {
                currentProduct.TestResult[step.Name] = result;
            }
            if (null != runtimeStatusData.FailedInfo)
            {
                if (currentProduct.ErrorInfo.ContainsKey(step.Name))
                {
                    currentProduct.ErrorInfo[step.Name] = runtimeStatusData.FailedInfo.Message;
                }
                else
                {
                    currentProduct.ErrorInfo.Add(step.Name, runtimeStatusData.FailedInfo.Message);
                }
            }
            return stepPassed;
        }

        private void UpdateKeyVariables(string serialNoVar, ProductTestResult currentProduct, 
            Dictionary<string, string> watchDatas, RuntimeStatusData runtimeStatus)
        {
            string variableValue = GetVariableValue(serialNoVar, watchDatas);
            if (!string.IsNullOrWhiteSpace(variableValue))
            {
                currentProduct.SerialNo = variableValue;
            }
            // 开始相关
            if (_seqMaintainer.UserStartTiming)
            {
                string startTimeStr = GetVariableValue(Constants.StartTimeVar, watchDatas);
                DateTime time;
                if (!string.IsNullOrWhiteSpace(startTimeStr) && DateTime.TryParse(startTimeStr, out time) &&
                    time != DateTime.MinValue)
                {
                    currentProduct.StartTime = DateTime.Parse(startTimeStr);
                }
            }
            if (runtimeStatus.Stack.Equals(_seqMaintainer.MainStartStack) && currentProduct.StartTime == DateTime.MinValue)
            {
                currentProduct.StartTime = runtimeStatus.ExecutionTime;
            }
            // 结束计时
            if (_seqMaintainer.UserEndTiming)
            {
                string endTimeStr = GetVariableValue(Constants.EndTimeVar, watchDatas);
                DateTime time;
                if (!string.IsNullOrWhiteSpace(endTimeStr) && DateTime.TryParse(endTimeStr, out time) &&
                    time != DateTime.MinValue)
                {
                    currentProduct.EndTime = DateTime.Parse(endTimeStr);
                }
            }
            if (runtimeStatus.Stack.Equals(_seqMaintainer.MainOverStack) && currentProduct.EndTime == DateTime.MinValue)
            {
                currentProduct.EndTime = runtimeStatus.ExecutionTime;
            }
        }

        private int GetIntVaraible(Dictionary<string, string> watchDatas, string uutIndexVar)
        {
            int value;
            if (null == watchDatas || !watchDatas.ContainsKey(uutIndexVar) ||
                !int.TryParse(watchDatas[uutIndexVar], out value))
            {
                return -1;
            }
            return value;
        }

        private string GetVariableValue(string variableName, Dictionary<string, string> watchDatas)
        {
            if (null == watchDatas || !watchDatas.ContainsKey(variableName))
            {
                return null;
            }
            return watchDatas[variableName];
        }

        private Dictionary<string, string> GetWatchData(RuntimeStatusData runtimeStatus)
        {
            if (string.IsNullOrWhiteSpace(runtimeStatus.WatchData))
            {
                return null;
            }
            Dictionary<string, string> variableValues = new Dictionary<string, string>(10);
            Dictionary<string, string> variables =
                JsonConvert.DeserializeObject<Dictionary<string, string>>(runtimeStatus.WatchData);
            foreach (string variableName in variables.Keys)
            {
                string realVariable = GetShortenVariable(variableName);
                string nextValue = variables[variableName];
                if (variableValues.ContainsKey(realVariable))
                {
                    // 只有更新的值合法，且原始值不合法时才更新值
                    if (IsValidVariableValue(nextValue) && !IsValidVariableValue(variableValues[realVariable]))
                    {
                        variableValues[realVariable] = nextValue;
                    }
                }
                else
                {
                    variableValues.Add(realVariable, nextValue);
                }
            }
            return variableValues;
        }

        private string GetShortenVariable(string runtimeVariableName)
        {
            const string runtimeVarDelim = "$";
            int startIndex = runtimeVariableName.LastIndexOf(runtimeVarDelim) + 1;
            if (startIndex <= 0)
            {
                return runtimeVariableName;
            }
            return runtimeVariableName.Substring(startIndex, runtimeVariableName.Length - startIndex);
        }

        private bool IsValidVariableValue(string value)
        {
            return !string.IsNullOrWhiteSpace(value) && !Constants.NASerialNo.Equals(value);
        }
    }
}