using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Testflow.Data.Sequence;
using Testflow.Runtime;
using Testflow.Runtime.Data;
using Testflow.Runtime.OperationPanel;
using Testflow.Usr;
using Testflow.Utility.Utils;

namespace JYProductOperationPanel
{
    
    /// <summary>
    /// 简仪生产测试OI
    /// </summary>
    public class ProductionTestOI : IOperationPanel
    {
        // private Thread _oiThread;
        private Form _panelForm;
        private volatile Exception _internalException;

        public ProductionTestOI()
        {
            _internalException = null;
        }
        
        private int _disposedFlag = 0;
        private Action<string> _showMessage;
        private Action _testStart;
        private Action<bool, string> _showError;
        // 设置状态的枚举，参数分别为是否正在运行、是否出现错误、是否出现失败
        private Action<bool, bool, bool> _setStatus;
        private string _assemblyPath;
        private string _classFullName;
        private string _messageVariableName;

        public Type ConfigPanelType => ConfigPanel;
        public ISequenceFlowContainer SequenceData { get; private set; }
        public OiSequenceType SupportSequence => OiSequenceType.SequenceGroup;
        public event Action<bool, Dictionary<string, object>> OiReady;

        public static Type ConfigPanel => typeof(ProductionTestOIConfigPanel);

        public void Dispose()
        {
            if (this._disposedFlag != 0)
            {
                return;
            }
            Thread.VolatileWrite(ref this._disposedFlag, 1);
            Thread.MemoryBarrier();
            if ((!this._panelForm?.IsDisposed) ?? false)
            {
                this._panelForm.Close();
                this._panelForm = null;
            }
        }

        // 异步事件
        private void OiStartSequenceConfirmed(bool isStartConfirmed, Dictionary<string, object> parameters)
        {
            OiReady?.Invoke(isStartConfirmed, parameters);
        }

        private EventInfo GetOIEventInfo(Type oiClassType, string eventName)
        {
            EventInfo startSequenceEvent = oiClassType.GetEvent(eventName,
                BindingFlags.Public | BindingFlags.Instance);
            if (null == startSequenceEvent)
            {
                throw new ApplicationException($"Event {eventName} is not found in target class.");
            }
            return startSequenceEvent;
        }

        private Delegate GetOIMethodInfo<TMethodType>(object instance, Type oiClassType, string methodName,
            params Type[] paramTypes)
        {
            MethodInfo showPanelMethod = oiClassType.GetMethod(methodName,
                BindingFlags.Instance | BindingFlags.Public, null, paramTypes, null);
            if (null == showPanelMethod)
            {
                throw new ApplicationException($"Method {methodName} is not found in target class.");
            }
            return showPanelMethod.CreateDelegate(typeof(TMethodType), instance);
        }

        public void ShowPanel(ISequenceFlowContainer sequenceData, params object[] extraParams)
        {
            this.SequenceData = sequenceData;
            IOperationPanelInfo panelInfo = ((ISequenceGroup) sequenceData).Info.OperationPanelInfo;
            if (string.IsNullOrWhiteSpace(panelInfo.Parameters))
            {
                throw new ApplicationException("The panel form of operation panel is not configured.");
            }
            string[] parameterElements = panelInfo.Parameters.Split('$');
            if (parameterElements.Length != 3)
            {
                throw new ApplicationException("Invalid operation panel parameters.");
            }
            this._assemblyPath = Utility.GetAbsolutePath(parameterElements[0], extraParams[0] as string[]);
            this._classFullName = parameterElements[1];
            this._messageVariableName = parameterElements[2];
            if (string.IsNullOrWhiteSpace(this._messageVariableName))
            {
                this._messageVariableName = "message";
            }
            ShowOperationPanel();
        }

        private void ShowOperationPanel()
        {
            this._panelForm = null;

            try
            {
                // TODO 待优化。因为单个AppDomain中加载的类无法被卸载的，可能会导致执行过程中同名的但不同实例的程序集不能加载
                Assembly oiAssembly = Assembly.LoadFrom(this._assemblyPath);
                Type oiClassType = oiAssembly.GetType(this._classFullName);
                if (null == oiClassType || !typeof(IDisposable).IsAssignableFrom(oiClassType))
                {
                    throw new ApplicationException(
                        $"Operation panel class {this._classFullName} cannot be found.");
                }
                ConstructorInfo oiClassConstructor = oiClassType.GetConstructor(
                    BindingFlags.Instance | BindingFlags.Public, null,
                    new Type[] { }, null);

                if (null == oiClassConstructor)
                {
                    throw new ApplicationException(
                        $"The default constructor of operation panel class {oiClassType.Name} cannot be found.");
                }
                this._panelForm = Activator.CreateInstance(oiClassType) as Form;

                this._showMessage = (Action<string>) GetOIMethodInfo<Action<string>>(_panelForm, oiClassType, "ShowMessage",
                        typeof(string));
                this._showError = (Action<bool, string>) GetOIMethodInfo<Action<bool, string>>(_panelForm, oiClassType,
                    "ShowError", typeof(bool), typeof(string));
                this._setStatus = (Action<bool, bool, bool>)GetOIMethodInfo<Action<bool, bool, bool>>(_panelForm, oiClassType,
                    "SetStatus", typeof(bool), typeof(bool), typeof(bool));
                EventInfo startSequenceEvent = GetOIEventInfo(oiClassType, "StartSequence");
                Delegate startSequenceCallBack = Delegate.CreateDelegate(typeof(Action<bool, Dictionary<string, object>>), this,
                    nameof(OiStartSequenceConfirmed));
                startSequenceEvent.AddEventHandler(this._panelForm, startSequenceCallBack);

                Application.Run(this._panelForm);
            }
            catch (TestflowException ex)
            {
                this._internalException = ex;
            }
            catch (TargetException ex)
            {
                this._internalException = ex.InnerException ?? ex;
            }
        }

        public void TestGenerationStart(ITestGenerationInfo generationInfo)
        {
            if (!this._panelForm.IsDisposed)
            {
                this._setStatus(true, false, false);
            }
        }

        public void TestGenerationOver(ITestGenerationInfo generationInfo)
        {
            ISessionGenerationInfo failedGeneration =
                generationInfo.GenerationInfos.FirstOrDefault(item => item.Status != GenerationStatus.Success);
            if (null != failedGeneration && !this._panelForm.IsDisposed)
            {
                this._setStatus(false, true, false);
                this._showError(false, $"Test generation failed. {failedGeneration.ErrorInfo}");
            }
        }

        public void TestStart(IList<ITestResultCollection> testResults)
        {

        }

        public void SessionStart(ITestResultCollection sessionResult)
        {

        }

        public void SequenceStart(ISequenceTestResult sequenceResult)
        {
            ISequence sequence = SequenceUtils.GetSequence((ISequenceGroup) SequenceData, sequenceResult.SessionId,
                sequenceResult.SequenceIndex);
            if (null == sequence)
            {
                return;
            }
            if (!this._panelForm.IsDisposed)
            {
                this._showMessage($"{sequence.Name} start...");
            }
        }

        public void StatusReceived(IRuntimeStatusInfo runtimeInfo)
        {
            ICallStack callStack = GetCurrentStack(runtimeInfo);
            if (null == callStack || callStack.Sequence == -1 || callStack.Sequence == -2 || null == runtimeInfo.StepResults ||
                (runtimeInfo.StepResults[callStack] != StepResult.Pass && runtimeInfo.StepResults[callStack] != StepResult.Failed))
            {
                return;
            }
            ISequence sequence = SequenceUtils.GetSequence(SequenceData, runtimeInfo.SessionId, callStack.Sequence);
            string oiMessage = GetVariableValue(runtimeInfo, sequence, this._messageVariableName);
            if (!string.IsNullOrEmpty(oiMessage) && !this._panelForm.IsDisposed)
            {
                this._showMessage(oiMessage);
            }
        }

        private ICallStack GetCurrentStack(IRuntimeStatusInfo statusInfo)
        {
            for (int i = 0; i < statusInfo.SequenceState.Count; i++)
            {
                if (statusInfo.SequenceState[i] == RuntimeState.Running)
                {
                    return statusInfo.CallStacks[i];
                }
            }
            return null;
        }

        private string GetVariableValue(IRuntimeStatusInfo statusInfo, ISequence sequence, string variableName)
        {
            IVariable variable = SequenceUtils.GetVariable(variableName, sequence);
            if (null == variable)
            {
                return string.Empty;
            }
            foreach (var item in statusInfo.WatchDatas)
            {
                if (ReferenceEquals(item.Key, variable))
                {
                    return item.Value;
                }
            }
            return string.Empty;
        }


        public void SequenceOver(ISequenceTestResult sequenceResult)
        {
            ISequence sequence = SequenceUtils.GetSequence((ISequenceGroup)SequenceData, sequenceResult.SessionId,
                sequenceResult.SequenceIndex);
            if (null == sequence)
            {
                return;
            }

            string result = sequenceResult.ResultState == RuntimeState.Success ? "Passed" : "Failed";
            string failedInfo = null != sequenceResult.FailedInfo ? sequenceResult.FailedInfo.Message : string.Empty;
            if (!this._panelForm.IsDisposed)
            {
                this._showMessage($"{sequence.Name} {result}. {failedInfo}");
            }
        }

        public void SessionOver(ITestResultCollection sessionResult)
        {
        }

        public void TestOver(IList<ITestResultCollection> testResults)
        {
            bool isFailed = testResults.Any(item => item.FailedCount > 0);
            bool isError = testResults.Any(item => item.AbortCount > 0 || item.TimeOutCount > 0);
            if (!this._panelForm.IsDisposed)
            {
                this._setStatus(false, isError, isFailed);
            }
        }

        public void PrintMessage(string message)
        {
            if (!this._panelForm.IsDisposed)
            {
                this._showMessage(message);
            }
        }

        public void ShowErrorMessage(bool isContinue, string message)
        {
            if (!this._panelForm.IsDisposed)
            {
                this._showError(isContinue, message);
            }
        }

        
    }
}
