using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using Testflow.Data.Sequence;
using Testflow.Modules;
using Testflow.Runtime;
using Testflow.Runtime.Data;
using TestFlow.SoftDevCommon;
using TestFlow.Software.WinformCommonOi;

namespace TestFlow.Software.OperationPanel
{
    public class OperationPanelInvoker : IDisposable
    {
        private WinformOperationPanelBase _operationPanel;
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

        public bool Initialize()
        {
            try
            {
                bool oiInitSuccess = InitOperationPanel();
                if (!oiInitSuccess)
                {
                    return false;
                }
                RegisterEvent();
                _operationPanelThd = new Thread((state) =>
                {
                    _operationPanel.ShowPanel(_sequenceData, _operationPanelInfo.Parameters);
                });
                _operationPanelThd.Start();
                return true;
            }
            catch (ApplicationException ex)
            {
                throw new ApplicationException($"Initialize operation panel failed:{ex.Message}");
            }
        }

        public void RegeisterOiReadyEventAndStart(Action<bool, string> eventDelegate)
        {
            _operationPanel.OiReady += eventDelegate;
        }


        private bool InitOperationPanel()
        {
            if (string.IsNullOrWhiteSpace(_operationPanelInfo?.Assembly?.Path))
            {
                return false;
            }
            _operationPanel = null;
            Assembly assembly = Assembly.LoadFrom(_operationPanelInfo.Assembly.Path);
            Type operationPanelType = assembly.GetType(
                $"{_operationPanelInfo.OperationPanelClass.Namespace}.{_operationPanelInfo.OperationPanelClass.Name}");
            _operationPanel = Activator.CreateInstance(operationPanelType) as WinformOperationPanelBase;
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
            _operationPanel.SessionStart(statistics);
        }

        private void TestInstanceStart(IList<ITestResultCollection> statistics)
        {
            _operationPanel.TestStart(statistics);
        }

        private void TestGenStart(ITestGenerationInfo generationInfo)
        {
            _operationPanel.TestGenerationStart(generationInfo);
        }

        private void TestGenOver(ITestGenerationInfo generationInfo)
        {
            _operationPanel.TestGenerationOver(generationInfo);
        }

        private void SequenceStarted(ISequenceTestResult statistics)
        {
            _operationPanel.SequenceStart(statistics);
        }

        private void StatusReceived(IRuntimeStatusInfo statusinfo)
        {
            _operationPanel.StatusReceived(statusinfo);
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

            _operationPanel?.Dispose();
            if (_operationPanelThd.Join(500))
            {
                _operationPanelThd.Abort();
            }
        }
    }
}