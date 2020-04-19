using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using Testflow.Data.Sequence;
using Testflow.ExtensionBase.OperationPanel;
using Testflow.Modules;
using Testflow.Runtime;
using Testflow.Runtime.Data;
using Testflow.Utility.Utils;
using TestFlow.SoftDevCommon;
using TestFlow.SoftDSevCommon;

namespace TestFlow.Software.OperationPanel
{
    public class OperationPanelInvoker : IDisposable
    {
        private OperationPanelBase _operationPanel;
        private readonly IOperationPanelInfo _operationPanelInfo;
        private readonly GlobalInfo _globalInfo;
        private readonly ISequenceGroup _sequenceData;
        private readonly List<Delegate> _eventActions;
        private Thread _operationPanelThd;

        public OperationPanelInvoker(GlobalInfo globalInfo, ISequenceGroup sequenceData)
        {
            _globalInfo = globalInfo;
            this._sequenceData = sequenceData;
            this._operationPanelInfo = sequenceData.Info.OperationPanelInfo;
            this._eventActions = new List<Delegate>(10);
        }

        public void Initialize()
        {
            try
            {
                bool oiInitSuccess = InitOperationPanel();
                if (!oiInitSuccess)
                {
                    return;
                }
                RegisterEvent();
                _operationPanelThd = new Thread((state) =>
                {
                    _operationPanel.ShowPanel(_sequenceData);
                });
            }
            catch (ApplicationException ex)
            {
                throw new ApplicationException($"Initialize operation panel failed:{ex.Message}");
            }
        }

        private bool InitOperationPanel()
        {
            if (null == _operationPanelInfo)
            {
                return false;
            }
            _operationPanel = null;
            Assembly assembly = Assembly.LoadFrom(_operationPanelInfo.Assembly.Path);
            Type operationPanelType = assembly.GetType(
                $"{_operationPanelInfo.OperationPanelClass.Namespace}.{_operationPanelInfo.OperationPanelClass.Name}");
            _operationPanel = (OperationPanelBase)Activator.CreateInstance(operationPanelType);
            
            if (null == _operationPanel)
            {
                throw new ApplicationException("Operation panel class is not derived from <OperationPanelBase>");
            }
            return true;
        }

        private void RegisterEvent()
        {
            _eventActions.Clear();
            _eventActions.Add(new RuntimeDelegate.TestGenerationAction(TestGenStart));
            _eventActions.Add(new RuntimeDelegate.TestGenerationAction(TestGenOver));
            _eventActions.Add(new RuntimeDelegate.TestInstanceStatusAction(TestInstanceStart));
            _eventActions.Add(new RuntimeDelegate.SessionStatusAction(SessionStart));
            _eventActions.Add(new RuntimeDelegate.SequenceStatusAction(SequenceStarted));
            _eventActions.Add(new RuntimeDelegate.StatusReceivedAction(StatusReceived));
            _eventActions.Add(new RuntimeDelegate.SequenceStatusAction(SequenceOver));
            _eventActions.Add(new RuntimeDelegate.SessionStatusAction(SessionOver));
            _eventActions.Add(new RuntimeDelegate.TestInstanceStatusAction(TestInstanceOver));
            IEngineController engineController = _globalInfo.TestflowEntity.EngineController;
            engineController.RegisterRuntimeEvent(_eventActions[0], "TestGenerationStart", 0);
            engineController.RegisterRuntimeEvent(_eventActions[1], "TestGenerationEnd", 0);
            engineController.RegisterRuntimeEvent(_eventActions[2], "TestInstanceStart", 0);
            engineController.RegisterRuntimeEvent(_eventActions[3], "SessionStart", 0);
            engineController.RegisterRuntimeEvent(_eventActions[4], "SequenceStarted", 0);
            engineController.RegisterRuntimeEvent(_eventActions[5], "StatusReceived", 0);
            engineController.RegisterRuntimeEvent(_eventActions[6], "SequenceOver", 0);
            engineController.RegisterRuntimeEvent(_eventActions[7], "SessionOver", 0);
            engineController.RegisterRuntimeEvent(_eventActions[8], "TestInstanceOver", 0);
        }

        private void SessionOver(ITestResultCollection statistics)
        {
            _operationPanel.SessionOver(statistics);
        }

        private void SessionStart(ITestResultCollection statistics)
        {
            _operationPanel.SessionStart(statistics.Session, DateTime.Now);
        }

        private void TestInstanceStart(IList<ITestResultCollection> statistics)
        {
            _operationPanel.TestStart(DateTime.Now);
        }

        private void TestGenStart(ITestGenerationInfo generationInfo)
        {
            _operationPanel.TestGenerationStart(generationInfo);
        }

        private void TestGenOver(ITestGenerationInfo generationInfo)
        {
            if (generationInfo.GenerationInfos[0].Status == GenerationStatus.Success)
            {
                _operationPanel.TestGenerationOver(generationInfo);
            }
            else
            {
                _operationPanel.TestGenerationFailed(generationInfo);
            }
        }

        private void SequenceStarted(ISequenceTestResult statistics)
        {
            ISequence sequence = SequenceUtils.GetSequence(_sequenceData, 0, statistics.SequenceIndex);
            _operationPanel.SequenceStart(sequence, statistics.StartTime);
        }

        private void StatusReceived(IRuntimeStatusInfo statusinfo)
        {
            ICallStack stack = GetCurrentStack(statusinfo);
            ISequenceStep currentStep = SequenceUtils.GetStepFromStack(_sequenceData, stack);
            if (null == currentStep)
            {
                return;
            }
            _operationPanel.StatusReceived(currentStep, statusinfo);
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


        private void TestInstanceOver(IList<ITestResultCollection> statistics)
        {
            _operationPanel.TestOver(statistics);
        }


        private void SequenceOver(ISequenceTestResult statistics)
        {
            _operationPanel.SequenceOver(statistics);
        }

        public void Dispose()
        {
            IEngineController engineController = _globalInfo.TestflowEntity.EngineController;
            engineController.UnregisterRuntimeEvent(_eventActions[0], "TestGenerationStart", 0);
            engineController.UnregisterRuntimeEvent(_eventActions[1], "TestGenerationEnd", 0);
            engineController.UnregisterRuntimeEvent(_eventActions[2], "TestInstanceStart", 0);
            engineController.UnregisterRuntimeEvent(_eventActions[3], "SessionStart", 0);
            engineController.UnregisterRuntimeEvent(_eventActions[4], "SequenceStarted", 0);
            engineController.UnregisterRuntimeEvent(_eventActions[5], "StatusReceived", 0);
            engineController.UnregisterRuntimeEvent(_eventActions[6], "SequenceOver", 0);
            engineController.UnregisterRuntimeEvent(_eventActions[7], "SessionOver", 0);
            engineController.UnregisterRuntimeEvent(_eventActions[8], "TestInstanceOver", 0);
            _eventActions.Clear();
        }
    }
}