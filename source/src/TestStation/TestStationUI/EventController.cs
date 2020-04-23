using System;
using System.Collections.Generic;
using System.IO;
using Testflow.Data;
using Testflow.Data.Sequence;
using Testflow.Modules;
using Testflow.Runtime;
using Testflow.Runtime.Data;
using Testflow.Utility.Utils;
using TestFlow.DevSoftware.Runtime;
using TestFlow.SoftDevCommon;

namespace TestFlow.DevSoftware
{
    internal class EventController : IDisposable
    {
        private readonly GlobalInfo _globalInfo;
        private readonly MainForm _mainform;
        private volatile string _serialNumber;
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
            Dictionary<string, string> watchDatas = GetWatchData(information.WatchDatas);
            ISequenceStep step = SequenceUtils.GetStepFromStack(_sequenceData, information.BreakPoint);
            if (null == step)
            {
                return;
            }
            _mainform.Invoke(new Action(() =>
            {
                _mainform.BreakPointHittedAction(information.BreakPoint, StepResult.NotAvailable, watchDatas);
            }));
        }

        private static Dictionary<string, string> GetWatchData(IDictionary<IVariable, string> watchData)
        {
            Dictionary<string, string> watchDatas = null;
            if (watchData?.Count > 0)
            {
                watchDatas = new Dictionary<string, string>(watchData.Count);
                foreach (KeyValuePair<IVariable, string> keyValuePair in watchData)
                {
                    watchDatas.Add(keyValuePair.Key.Name, keyValuePair.Value);
                }
            }
            return watchDatas;
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
            if (statusinfo.FailedInfos?.Count > 0)
            {
                foreach (IFailedInfo failedInfo in statusinfo.FailedInfos.Values)
                {
                    ISequenceStep step = SequenceUtils.GetStepFromStack(_sequenceData, failedInfo.StackTrace);
                    printInfos.Add($"Step <{step.Name}> failed: {failedInfo.Message}");
                }
            }
            ICallStack currentStack = GetCurrentStack(statusinfo);
            StepResult result = statusinfo.StepResults[currentStack];
            Dictionary<string, string> watchData = GetWatchData(statusinfo.WatchDatas);
            _mainform.Invoke(new Action(() =>
            {
                if (printInfos.Count > 0)
                {
                    _mainform.AppendOutput(string.Join(Environment.NewLine, printInfos));
                }
                _mainform.ShowStepResults(currentStack, result, watchData);
            }));
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
            string runtimeHash = _globalInfo.TestflowEntity.EngineController.GetRuntimeInfo<string>("RuntimeHash");
            string reportPath = GetReportPath();
            _globalInfo.TestflowEntity.ResultManager.PrintReport(reportPath, runtimeHash, ReportType.txt, _sequenceData);
            _mainform.Invoke(new Action(() =>
            {
                _mainform.RunningOver();
                _mainform.PrintReport(reportPath);
            }));
        }

        private string GetReportPath()
        {
            string workspaceDir = _globalInfo.TestflowEntity.ConfigurationManager.ConfigData.GetProperty<string[]>("WorkspaceDir")[0];
            string testName = _globalInfo.TestflowEntity.EngineController.GetRuntimeInfo<string>("TestName");
            string filePath = $"{workspaceDir}{testName}.txt";
            int index = 1;
            while (File.Exists(filePath))
            {
                filePath = $"{workspaceDir}{testName}({index++}).txt";
            }
            return filePath;
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