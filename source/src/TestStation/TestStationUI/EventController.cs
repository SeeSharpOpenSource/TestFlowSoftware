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
using TestStation.Common;
using TestStation.OperationPanel;
using TestStation.Report;

namespace TestStation
{
    internal class EventController : IDisposable
    {
        private readonly GlobalInfo _globalInfo;
        private readonly MainForm _mainform;
        private readonly SequenceMaintainer _seqMaintainer;
        private TestTraceCreator _testTraceCreator;
        private volatile string _serialNumber;
        private ResultMaintainer _resultMaintainer;
        private ISequenceGroup rawSequence;
        private int _uutIndex;

        public EventController(GlobalInfo globalInfo, ISequenceGroup sequenceData, 
            MainForm mainform, SequenceMaintainer seqMaintainer)
        {
            _globalInfo = globalInfo;
            this._mainform = mainform;
            this._seqMaintainer = seqMaintainer;
            _currentSequence = null;
            DataCache = null;
            _testTraceCreator = new TestTraceCreator(_globalInfo, _seqMaintainer);
            _testTraceCreator.PrintOver += ReportPrintOver;
            _serialNumber = string.Empty;
            rawSequence = sequenceData;
            _resultMaintainer = new ResultMaintainer(sequenceData, _seqMaintainer);
            _uutIndex = -1;
        }

        public RuntimeDataCache DataCache { get; set; }

        public string ReportDir { get; private set; }

        public string CurrentReport => _testTraceCreator.CurrentReport;

        private volatile ISequence _currentSequence;
        public ISequence CurrentSequence => _currentSequence;

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
                int sequenceIndex = ((ISequence) variable.Parent)?.Index ?? Constants.SequenceGroupIndex;
                string variableName = _seqMaintainer.GetRawVariable(sequenceIndex, variable.Name);
                watchDatas.Add(variableName, value);
            }

            ISequenceStep step = _seqMaintainer.GetStepByRawStack(information.BreakPoint.ToString());
            if (null == step)
            {
                return;
            }
            ISequence sequence = (ISequence)step.Parent;
            ResultState result = ResultState.NA;
            IList<ResultState> results;
            IList<string> variables;
            _resultMaintainer.GetResultsAndVariables(sequence.Index, out results, out variables);
            _mainform.Invoke(new Action(() =>
            {
                _mainform.BreakPointHittedAction(step, results, variables, watchDatas);
            }));
        }

        public void TestGenStart(ITestGenerationInfo generationInfo)
        {
            _mainform.Invoke(new Action(() =>
            {
                _mainform.AppendOutput("Test generation start...");
            }));
        }

        public void TestGenOver(ITestGenerationInfo generationInfo)
        {
            _mainform.Invoke(new Action(() =>
            {
                if (generationInfo.GenerationInfos[0].Status == GenerationStatus.Success)
                {
                    _mainform.AppendOutput("Test generation success.");
                }
                else
                {
                    _mainform.AppendOutput("Test generation failed.");
                    _mainform.RunningOver();
                }
            }));
        }
        
        private void SequenceStarted(ISequenceTestResult statistics)
        {
            string printInfo;
            if (statistics.SequenceIndex == -1)
            {
                printInfo = "ProcessSetUp started.";
            }
            else if (statistics.SequenceIndex == -2)
            {
                printInfo = "ProcessCleanUp started.";
            }
            else
            {
                printInfo = "MainSequence started.";
            }
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
            ICallStack currentStack = GetCurrentStack(statusinfo);
            string stackStr = currentStack.ToString();
            if (stackStr.Equals(_seqMaintainer.MainStartStack))
            {
                _resultMaintainer.ResetUutResult();
            }
            ISequenceStep step = _seqMaintainer.GetStepByRawStack(stackStr);
            ISequence currentSequence = null != step ? Utility.GetParentSequence(step) : null;
            _currentSequence = currentSequence;
            if (null == currentSequence)
            {
                return;
            }
            ISequenceStep rawStep = _seqMaintainer.GetStepByStack(currentStack);
            bool isLimitStep = rawStep.Function?.ClassType.Name.Equals("Limit") ?? false;
            IDictionary<string, string> watchDatas = _resultMaintainer.UpdateResult(statusinfo,currentStack, step, isLimitStep);
            string serialNumber;
            int uutIndex;
            _resultMaintainer.GetSerialNumber(out serialNumber, out uutIndex);
            if (!serialNumber.Equals(_serialNumber))
            {
                _serialNumber = serialNumber;
                printInfos.Add($"Serial Number:{_serialNumber}");
            }
            if (uutIndex > _uutIndex)
            {
                Thread.VolatileWrite(ref _uutIndex, uutIndex);
            }
            bool isPreUutStack = Utility.IsPreUutStack(stackStr, _seqMaintainer.MainStartStack);
            bool isPostUutStack = Utility.IsPostUutStack(stackStr, _seqMaintainer.PostStartStack);
            bool isMainStack = !isPostUutStack && !isPreUutStack && currentStack.Sequence == 0;
            ISequenceStepCollection mainSeqSteps = rawSequence.Sequences[1].Steps;
            int sequenceIndex = currentSequence.Index;
            if (isPreUutStack)
            {
                step = mainSeqSteps.Count > 0 ? mainSeqSteps[0] : null;
                sequenceIndex = 1;
            }
            else if (isPostUutStack)
            {
                step = mainSeqSteps.Count > 0 ? mainSeqSteps[mainSeqSteps.Count - 1] : null;
                sequenceIndex = 1;
            }
            else if (currentSequence.Index < 0)
            {
                step = null;
                sequenceIndex = 1;
            }
            IList<ResultState> stepResults;
            IList<string> sequenceVariableValues;
            _resultMaintainer.GetResultsAndVariables(sequenceIndex, out stepResults, out sequenceVariableValues);
            _mainform.Invoke(new Action(() =>
            {
                if (printInfos.Count > 0)
                {
                    _mainform.AppendOutput(string.Join(Environment.NewLine, printInfos));
                }
                _mainform.RefreshVariableValues(watchDatas);
                if (null != step)
                {
                    _mainform.RefreshStepResult(step, stepResults, sequenceVariableValues);
                }
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
                if (statistics.ResultState == RuntimeState.Over || statistics.ResultState == RuntimeState.Success)
                {
                    _testTraceCreator.Start(DataCache);
                    this.ReportDir = _testTraceCreator.ReportDir;
                }
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
            _testTraceCreator.Stop();
            UnRegisterEvent();
            _mainform.Invoke(new Action(() =>
            {
                _mainform.RunningOver();
            }));
        }

        private void ReportPrintOver()
        {
            _mainform.Invoke(new Action(() =>
            {
                _mainform.PrintUutResults(_testTraceCreator.PruductTestResults);
            }));
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
            while (!_testTraceCreator.Over)
            {
                Thread.Yield();
            }
            _testTraceCreator.Dispose();
        }
    }
}