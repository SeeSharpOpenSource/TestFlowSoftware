using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Testflow.Data.Sequence;
using Testflow.Modules;
using Testflow.Runtime;
using Testflow.Runtime.Data;
using Testflow.Usr;
using TestFlow.SoftDevCommon;
using TestFlow.SoftDSevCommon;

namespace TestFlow.DevSoftware.OperationPanel
{
//    internal class EventController : IDisposable
//    {
//        private readonly OperationPanelForm _operationPanel;
//        private readonly RuntimeDataCache _dataCache;
//        private readonly IEngineController _engineController;
//        private volatile int _currentUutIndex;
//        private volatile bool _currentTestPassed;
//        private volatile bool _currentUutError;
//
//        private IVariable _uutVariable;
//        private IVariable _serialNoVar;
//        private IVariable _startTimeVariable;
//        private IVariable _endTimeVariable;
//        private ReaderWriterLockSlim _dataCacheLock;
//        private volatile UutStatus _uutState = UutStatus.Waitting;
//
//        public UutStatus UutStatus
//        {
//            get { return _uutState; }
//            set
//            {
//                // 只有在手动重置Waiting或者新的状态比原始状态大时才可以配置成功
//                if (value == UutStatus.Waitting || value > _uutState)
//                {
//                    _uutState = value;
//                }
//            }
//        }
//
//        #region 进度计算相关参数
//
//        private int _stepCount = 2;
//        // Step执行进度计数的级别
//        private const int StepCountLevel = 3;
//        // Stack中执行进度计数的级别
//        private const int StackCountLevel = 5;
//        private int _startStepIndex = 0;
//        private int _endStepIndex = 1;
//
//        #endregion
//
//        private DateTime _mainStartTime;
//        private volatile bool _currentUutStart;
//
//        public EventController(OperationPanelForm operationPanel, RuntimeDataCache dataCache)
//        {
//            this._dataCache = dataCache;
//            this._operationPanel = operationPanel;
//            this._engineController = dataCache.TestflowEntity.EngineController;
//            ISequenceGroup sequenceData = _dataCache.SequenceData;
//            _currentUutIndex = -1;
//            _uutVariable = sequenceData.Variables.FirstOrDefault(item => item.Name.Equals(Constants.UutIndexVar));
//            _serialNoVar = sequenceData.Variables.FirstOrDefault(item => item.Name.Equals(Constants.SerialNoVarName));
//            _startTimeVariable = _dataCache.EnableStartTiming
//                ? sequenceData.Variables.FirstOrDefault(item => item.Name.Equals(Constants.StartTimeVar))
//                : null;
//            _endTimeVariable = _dataCache.EnableEndTiming
//                ? sequenceData.Variables.FirstOrDefault(item => item.Name.Equals(Constants.EndTimeVar))
//                : null;
//            if (null == _uutVariable || null == _serialNoVar)
//            {
//                throw new ApplicationException("Sequence data does not contain the definition of serial number and uut index.");
//            }
//            _dataCacheLock = new ReaderWriterLockSlim();
//            _currentTestPassed = true;
//        }
//
//        public void Initialize()
//        {
//            RegisterEvent();
//            InitProgressCalcParam();
//        }
//
//        private void RegisterEvent()
//        {
//            IEngineController engineController = _engineController;
//            engineController.RegisterRuntimeEvent(new RuntimeDelegate.TestGenerationAction(TestGenStart),
//                "TestGenerationStart",
//                0);
//            engineController.RegisterRuntimeEvent(new RuntimeDelegate.TestGenerationAction(TestGenOver),
//                "TestGenerationEnd", 0);
//
//
//            engineController.RegisterRuntimeEvent(new RuntimeDelegate.SequenceStatusAction(SequenceStarted),
//                "SequenceStarted",
//                0);
//            engineController.RegisterRuntimeEvent(new RuntimeDelegate.StatusReceivedAction(StatusReceived),
//                "StatusReceived", 0);
//            engineController.RegisterRuntimeEvent(new RuntimeDelegate.SequenceStatusAction(SequenceOver), "SequenceOver",
//                0);
//            engineController.RegisterRuntimeEvent(new RuntimeDelegate.TestInstanceStatusAction(TestInstanceOver),
//                "TestInstanceOver", 0);
//            engineController.RegisterRuntimeEvent(new RuntimeDelegate.BreakPointHittedAction(BreakPointHitted),
//                "BreakPointHitted", 0);
//        }
//
//        private void InitProgressCalcParam()
//        {
//            string[] startStackElems = _dataCache.MainStartStack.Split('_');
//            string[] endStackElems = _dataCache.MainOverStack.Split('_');
//            if (startStackElems.Length > StackCountLevel && endStackElems.Length > StackCountLevel)
//            {
//                _startStepIndex = int.Parse(startStackElems[StackCountLevel]);
//                _endStepIndex = int.Parse(endStackElems[StackCountLevel]);
//                _stepCount = _endStepIndex - _startStepIndex + 1;
//            }
//            else
//            {
//
//                _startStepIndex = 0;
//                _endStepIndex = 0;
//                _stepCount = 1;
//            }
//        }
//
//        private void BreakPointHitted(IDebuggerHandle debuggerhandle, IDebugInformation information)
//        {
//            if (_operationPanel.FormUnavaiable)
//            {
//                return;
//            }
//            Thread.MemoryBarrier();
//            _operationPanel.Invoke(new Action(() =>
//            {
//                _operationPanel.SetViewControllerState(RunState.RunBlock);
//            }));
//        }
//
//        public void TestGenStart(ITestGenerationInfo generationInfo)
//        {
//            if (_operationPanel.FormUnavaiable)
//            {
//                return;
//            }
//            Thread.MemoryBarrier();
//            _operationPanel.Invoke(new Action(() =>
//            {
//                _operationPanel.ShowUutStatus(UutStatus.Waitting);
//            }));
//        }
//
//        public void TestGenOver(ITestGenerationInfo generationInfo)
//        {
//            if (_operationPanel.FormUnavaiable)
//            {
//                return;
//            }
//            Thread.MemoryBarrier();
//            _operationPanel.Invoke(new Action(() =>
//            {
//                if (generationInfo.GenerationInfos[0].Status != GenerationStatus.Success)
//                {
//                    
//                    _operationPanel.ShowUutStatus(UutStatus.Error);
//                    _operationPanel.SetViewControllerState(RunState.RunOver);
//                }
//                else
//                {
//                    _operationPanel.ShowUutStatus(UutStatus.Waitting);
//                }
//            }));
//        }
//
//        private void SequenceStarted(ISequenceTestResult statistics)
//        {
//            // ignore
//        }
//
//        private void StatusReceived(IRuntimeStatusInfo statusinfo)
//        {
//            int uutIndex;
//            IDictionary<IVariable, string> watchDatas = statusinfo.WatchDatas;
//            bool uutOver = false;
//            bool serialNumberChanged = false;
//            if (!watchDatas.ContainsKey(_uutVariable) || !int.TryParse(watchDatas[_uutVariable], out uutIndex) ||
//                uutIndex < 0)
//            {
//                return;
//            }
//            ICallStack mainStack = statusinfo.CallStacks.FirstOrDefault(item => item.Sequence == 0);
//            int infoIndex = statusinfo.CallStacks.IndexOf(mainStack);
//            _dataCacheLock.EnterWriteLock();
//            // 更新了uutIndex，进入下次循环
//            if (uutIndex > _currentUutIndex && _dataCache.MainStartStack.Equals(mainStack?.ToString()))
//            {
//                StartNewUutTest(statusinfo);
//                _currentUutIndex = uutIndex;
//                serialNumberChanged = true;
//                UutStatus = UutStatus.Waitting;
//                _mainStartTime = statusinfo.CurrentTime;
//            }
//            bool serialNumberReceived;
//            ProcessStatusInfo(statusinfo, mainStack, out uutOver, out serialNumberReceived);
//            _dataCacheLock.ExitWriteLock();
//
//            UpdateProgressAndStatus(mainStack, uutOver);
//            if (null != statusinfo.StepResults && statusinfo.StepResults.Values.Any(item => item == StepResult.Abort))
//            {
//                UutStatus = UutStatus.Terminal;
//            }
//
//            serialNumberChanged |= serialNumberReceived;
//            if (_operationPanel.FormUnavaiable)
//            {
//                return;
//            }
//            Thread.MemoryBarrier();
//            _operationPanel.Invoke(new Action(() =>
//            {
//                _dataCacheLock.EnterReadLock();
//                if (uutOver || serialNumberChanged)
//                {
//                    _operationPanel.RefreshProductTestInformation(UutStatus);
//                }
//                else
//                {
//                    _operationPanel.RefreshCommonRunningInformation(UutStatus);
//                }
//                _dataCacheLock.ExitReadLock();
//            }));
//        }
//
//
//        private void TestInstanceOver(IList<ITestResultCollection> statistics)
//        {
//            
//        }
//
//        private void UpdateProgressAndStatus(ICallStack mainStack, bool uutOver)
//        {
//            // UUT结束
//            if (uutOver)
//            {
//                _dataCache.UutProgress = 1;
//                if (_currentTestPassed)
//                {
//                    UutStatus = UutStatus.Pass;
//                }
//                else if (_currentUutError)
//                {
//                    UutStatus = UutStatus.Error;
//                }
//                else
//                {
//                    UutStatus = UutStatus.Fail;
//                }
//            }
//            // UUT执行过程中
//            else if (null != mainStack && mainStack.StepStack.Count > StepCountLevel &&
//                     mainStack.StepStack[StepCountLevel] >= _startStepIndex &&
//                     mainStack.StepStack[StepCountLevel] <= _endStepIndex)
//            {
//                _dataCache.UutProgress = (double) (mainStack.StepStack[StepCountLevel] - _startStepIndex + 1)/_stepCount;
//                UutStatus = UutStatus.Running;
//            }
//            else
//            {
//                UutStatus = UutStatus.Waitting;
//                _dataCache.UutProgress = 0;
//            }
//        }
//
//        private void StartNewUutTest(IRuntimeStatusInfo statusinfo)
//        {
//            _dataCache.SerialNumber = Constants.SerialNoVarName;
//            if (statusinfo.WatchDatas.ContainsKey(_serialNoVar) &&
//                string.IsNullOrWhiteSpace(statusinfo.WatchDatas[_serialNoVar]))
//            {
//                _dataCache.SerialNumber = statusinfo.WatchDatas[_serialNoVar];
//            }
//            _dataCache.ElapsedSeconds = 0;
//            UutStatus = UutStatus.Waitting;
//            _currentTestPassed = true;
//            _currentUutError = false;
//            _currentUutStart = true;
//        }
//
//        private void ProcessStatusInfo(IRuntimeStatusInfo statusinfo, ICallStack cuurentStack, out bool uutOver, 
//            out bool serialNumberChanged)
//        {
//            uutOver = false;
//            serialNumberChanged = false;
//            StepResult result = statusinfo.StepResults.ContainsKey(cuurentStack)
//                ? statusinfo.StepResults[cuurentStack]
//                : StepResult.Pass;
//            // 如果结果信息为Pass/RetryFailed/NotAvailable/Over/Skip，则认定返回信息的step执行成功
//            bool stepPass = result == StepResult.Pass || result == StepResult.RetryFailed || result == StepResult.NotAvailable ||
//                        result == StepResult.Over || result == StepResult.Skip;
//            _currentTestPassed &= stepPass;
//            if (!stepPass && null != statusinfo.FailedInfos && statusinfo.FailedInfos.ContainsKey(0) &&
//                null != statusinfo.FailedInfos[0])
//            {
//                FailedType failedType = statusinfo.FailedInfos[0].Type;
//                bool isError = failedType == FailedType.RuntimeError || failedType == FailedType.TargetError ||
//                               failedType == FailedType.TimeOut;
//                _currentUutError |= isError;
//            }
//            IDictionary<IVariable, string> watchDatas = statusinfo.WatchDatas;
//            if (watchDatas.ContainsKey(_serialNoVar) && !string.IsNullOrWhiteSpace(watchDatas[_serialNoVar]) &&
//                !_dataCache.SerialNumber.Equals(watchDatas[_serialNoVar]))
//            {
//                _dataCache.SerialNumber = watchDatas[_serialNoVar];
//                serialNumberChanged = true;
//            }
//            ICallStack mainStack = statusinfo.CallStacks.FirstOrDefault(item => item.Sequence == 0);
//            _dataCache.ElapsedSeconds = _mainStartTime != DateTime.MinValue
//                ? (statusinfo.CurrentTime - _mainStartTime).TotalMilliseconds/1000
//                : 0;
//            if (null != mainStack && mainStack.ToString().Equals(_dataCache.PostStartStack) && _currentUutStart && IsValidSerialNumber())
//            {
//                uutOver = true;
//                _mainStartTime = DateTime.MinValue;
//                if (_currentTestPassed)
//                {
//                    _dataCache.AddQualified();
//                }
//                else
//                {
//                    _dataCache.AddUnQualified();
//                }
//                _dataCache.SerialNumber = Constants.NASerialNo;
//                _currentUutStart = false;
//            }
//        }
//
//        private void SequenceOver(ISequenceTestResult statistics)
//        {
//            if (statistics.SequenceIndex != -2 || statistics.ResultState < RuntimeState.Success)
//            {
//                return;
//            }
//            if (_operationPanel.FormUnavaiable)
//            {
//                return;
//            }
//            Thread.MemoryBarrier();
//            _operationPanel.Invoke(new Action(() =>
//            {
//                _operationPanel.SetViewControllerState(RunState.RunOver);
//            }));
//            this.Dispose();
//        }
//
//        public void Dispose()
//        {
//            IEngineController engineController = _dataCache.TestflowEntity.EngineController;
//            engineController.UnregisterRuntimeEvent(new RuntimeDelegate.TestGenerationAction(TestGenStart),
//                "TestGenerationStart", 0);
//            engineController.UnregisterRuntimeEvent(new RuntimeDelegate.TestGenerationAction(TestGenOver),
//                "TestGenerationEnd", 0);
//            engineController.UnregisterRuntimeEvent(new RuntimeDelegate.SequenceStatusAction(SequenceStarted),
//                "SequenceStarted", 0);
//            engineController.UnregisterRuntimeEvent(new RuntimeDelegate.StatusReceivedAction(StatusReceived),
//                "StatusReceived", 0);
//            engineController.UnregisterRuntimeEvent(new RuntimeDelegate.SequenceStatusAction(SequenceOver),
//                "SequenceOver", 0);
//            engineController.UnregisterRuntimeEvent(new RuntimeDelegate.TestInstanceStatusAction(TestInstanceOver),
//                "TestInstanceOver", 0);
//            Thread.Sleep(200);
//            Application.DoEvents();
//            Thread.Sleep(200);
//            _dataCacheLock.Dispose();
//        }
//
//        private bool IsValidSerialNumber()
//        {
//            return null != _dataCache.SerialNumber &&
//                   !Constants.NASerialNo.Equals(_dataCache.SerialNumber) &&
//                   !Constants.NullValue.Equals(_dataCache.SerialNumber);
//        }
//    }
}