using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Newtonsoft.Json;
using Testflow.Data.Sequence;
using Testflow.Runtime.Data;
using TestStation.Common;
using TestStation.Report.ReportCreator;

namespace TestStation.Report
{
    public class TestTraceCreator : IDisposable
    {
        private readonly GlobalInfo _globalInfo;
        private readonly SequenceMaintainer _seqMaintainer;
        private ReportCreatorBase _reportCreator;
        private string _runtimeHash;
        private ReportGlobalInfo _reportGlobalInfo;

        public string ReportDir { get; private set; }
        public string CurrentReport => _reportCreator?.CurrentReport ?? string.Empty;
        public event Action PrintOver;
        public event Action<string> SingleUutOver;

        public IList<ProductTestResult> PruductTestResults { get; }

        private volatile bool _over;
        public bool Over => _over;
        private readonly CancellationTokenSource _cancellation;

        public TestTraceCreator(GlobalInfo globalInfo, SequenceMaintainer seqMaintainer)
        {
            _globalInfo = globalInfo; 
            _seqMaintainer = seqMaintainer;
            PruductTestResults = new List<ProductTestResult>(100);
            _over = false;
            _cancellation = new CancellationTokenSource();
        }

        public void Start(RuntimeDataCache dataCache)
        {
            _over = false;
            string reportNameFormat = _globalInfo.ConfigManager.GetConfig<string>("ReportNameFormat");
            reportNameFormat = reportNameFormat.Replace("{BaseName}", _globalInfo.ConfigManager.GetConfig<string>("BaseName"));
            _reportGlobalInfo = new ReportGlobalInfo()
            {
                Operator = "UserName",
                SocketIndex = "0",
                StationId = dataCache.StationName,
                SequencePath = _seqMaintainer.GetSequencePath(),
                SequenceName = dataCache.SequenceName,
                ReportNameFormat = reportNameFormat,
                Model = dataCache.Model
            };
            _runtimeHash = _globalInfo.TestflowEntity.EngineController.GetRuntimeInfo<string>("RuntimeHash");
            TestInstanceData testInstanceData = _globalInfo.TestflowEntity.DataMaintainer.GetTestInstance(_runtimeHash);
            string reportPath = _globalInfo.ConfigManager.GetConfig<string>("ReportPath");
            if (string.IsNullOrWhiteSpace(reportPath) || !Directory.Exists(reportPath))
            {
                reportPath = GetSequenceDirectory(_reportGlobalInfo.SequencePath);
            }
            string reportDir = Utility.GetReportDir(reportPath,
                dataCache.SequenceName, testInstanceData.StartTime);
            ReportDir = reportDir;
            _reportCreator = ReportCreatorBase.GetReportCreator(ReportType.Txt, _reportGlobalInfo, reportDir);
            ThreadPool.QueueUserWorkItem((state) => { UpdateProductTestResults(); });
        }

        private string GetSequenceDirectory(string sequencePath)
        {
            int index = sequencePath.LastIndexOf(Path.DirectorySeparatorChar);
            return sequencePath.Substring(0, index + 1);
        }

        public void Stop()
        {
            _cancellation.Cancel();
        }

        /// <summary>
        /// 获取产品测试结果信息，并打印报表
        /// </summary>
        private void UpdateProductTestResults()
        {
            Action<string> printInfo = _globalInfo.PrintInfo;

            try
            {
                GetUutResultAndPrintReport();
            }
            catch (ApplicationException ex)
            {
                Log.Print(LogLevel.ERROR, ex.Message);
                printInfo.Invoke(ex.Message);
            }
            catch (Exception ex)
            {
                Log.Print(LogLevel.FATAL, ex.Message);
                printInfo.Invoke(ex.Message);
            }
            finally
            {
                OnPrintOver();
            }
        }

        private void GetUutResultAndPrintReport()
        {
            long startIndex = 0;
            long count = 500;
            string mainOverStack = _seqMaintainer.PostStartStack;
            string mainStartStack = _seqMaintainer.MainStartStack;

            ProductTestResult currentProduct = null;
            IList<ProductTestResult> productTestResults = PruductTestResults;
            bool currentUutStart = false;
            int currentUutIndex = -1;
            bool uutPassed = true;

            Action<string> printUutResult = _globalInfo.PrintUutResult;
            while (!_cancellation.IsCancellationRequested)
            {
                IList<RuntimeStatusData> runtimeStatusDatas =
                    _globalInfo.TestflowEntity.DataMaintainer.GetRuntimeStatusInRange(_runtimeHash, 0, startIndex, count);
                if (runtimeStatusDatas.Count == 0)
                {
                    Thread.Sleep(50);
                    continue;
                }
                foreach (RuntimeStatusData runtimeStatusData in runtimeStatusDatas)
                {
                    if (runtimeStatusData.Sequence == -1 ||
                        (runtimeStatusData.Result == StepResult.Over && runtimeStatusData.Sequence != -2))
                    {
                        continue;
                    }
                    Dictionary<string, string> watchData = GetWatchData(runtimeStatusData);
                    int uutIndex = GetIntVaraible(watchData, Constants.UutIndexVar);
                    // 如果UUT更新，并且当前堆栈等于uut的入口，则UUT开始执行

                    if (uutIndex > currentUutIndex && mainStartStack.Equals(runtimeStatusData.Stack))
                    {
                        currentProduct = new ProductTestResult();
                        currentProduct.Model = _reportGlobalInfo.Model;
                        UpdateKeyVariables(Constants.SerialNoVarName, currentProduct, watchData, runtimeStatusData);
                        currentUutIndex = uutIndex;
                        uutPassed = true;
                        _reportCreator.BeginPrint(runtimeStatusData.ExecutionTime);
                        currentUutStart = true;
                    }
                    if (currentUutStart)
                    {
                        bool stepPassed = ProcessRuntimeInfo(Constants.SerialNoVarName, currentProduct,
                            runtimeStatusData, watchData);
                        uutPassed &= stepPassed;
                        // 用户编辑的序列Step
                        ISequenceStep rawStep = _seqMaintainer.GetStepByRawStack(runtimeStatusData.Stack);
                        // 真正运行的序列Step
                        ISequenceStep runStep = _seqMaintainer.GetRuntimeStep(runtimeStatusData.Stack);
                        _reportCreator.PrintSingleInfo(runtimeStatusData, rawStep, runStep);
                    }
                    if (currentUutStart && runtimeStatusData.Stack.Equals(mainOverStack))
                    {
                        if (IsValidSerialNumber(currentProduct.SerialNo))
                        {
                            currentProduct.Result = uutPassed ? ResultState.Pass : ResultState.Fail;
                            WriteProductTestOverData(currentProduct, runtimeStatusData);
                            productTestResults.Add(currentProduct);
                            _reportCreator.SetStartTime(currentProduct.StartTime);
                            _reportCreator.SetEndTime(currentProduct.EndTime);
                            _reportCreator.EndPrint();
                            OnSingleUutOver();
                            PrintUutResult(printUutResult, CurrentReport);
                            currentProduct = null;
                        }
                        else
                        {
                            _reportCreator.DeleteCurrent();
                        }
                        currentUutStart = false;
                    }
                }
                startIndex = runtimeStatusDatas[runtimeStatusDatas.Count - 1].StatusIndex + 1;
            }
        }

        private void OnPrintOver()
        {
            PrintOver?.Invoke();
        }

        private void OnSingleUutOver()
        {
            SingleUutOver?.Invoke(CurrentReport);
        }

        private static void PrintUutResult(Action<string> printUutResult, string reportPath)
        {
            string reportData = File.ReadAllText(reportPath);
            printUutResult.Invoke(reportData);
        }

        private bool IsValidSerialNumber(string serialiNumber)
        {
            return null != serialiNumber && !Constants.NullValue.Equals(serialiNumber);
        }

        private void WriteProductTestOverData(ProductTestResult currentProduct, RuntimeStatusData runtimeStatus)
        {
            if (!_seqMaintainer.UserEndTiming || currentProduct.EndTime == DateTime.MinValue)
            {
                currentProduct.EndTime = runtimeStatus.ExecutionTime;
            }
            currentProduct.ElapsedTime = (currentProduct.EndTime - currentProduct.StartTime).TotalMilliseconds/1000;
            currentProduct.Result = currentProduct.TestResult.Values.Any(item => item != ResultState.Pass && item != ResultState.Skip)
                ? ResultState.Fail
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
                    result = ResultState.Fail;
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
            if (runtimeStatus.Stack.Equals(_seqMaintainer.PostStartStack) && currentProduct.EndTime == DateTime.MinValue)
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

        public void Dispose()
        {
            _reportCreator.Dispose();
        }
    }
}