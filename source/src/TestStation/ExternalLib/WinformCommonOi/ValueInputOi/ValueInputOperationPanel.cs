using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;
using Testflow.Data.Sequence;
using Testflow.Runtime;
using Testflow.Runtime.Data;
using Testflow.Runtime.OperationPanel;
using Testflow.Utility.Utils;

namespace TestFlow.Software.WinformCommonOi.ValueInputOi
{
    public class ValueInputOperationPanel : WinformOperationPanelBase
    {
        
        public ISequenceFlowContainer SequenceData { get; private set; }
        public OiSequenceType SupportSequence => OiSequenceType.SequenceGroup;

        public override void Dispose()
        {
            _valueInputOiForm?.Dispose();
            _runtimeStatusForm?.Dispose();
        }

        private RuntimeStatusForm _runtimeStatusForm;
        private ValueInputOiForm _valueInputOiForm;

        public ValueInputOperationPanel()
        {
            this.ConfigFormType = typeof (OiConfigForm);
        }

        /// <summary>
        /// 显示OI面板
        /// </summary>
        /// <param name="sequenceData">待执行的序列数据</param>
        /// <param name="extraParams">扩展参数，默认0为OI参数, 1为在OI上显示的消息</param>
        public override void ShowPanel(ISequenceFlowContainer sequenceData, params object[] extraParams)
        {
            this.SequenceData = sequenceData;
            if (!(this.SequenceData is ISequenceGroup))
            {
                OnOiReady(false, "Invalid sequence type.");
                return;
            }
            if (extraParams.Length < 1 || string.IsNullOrWhiteSpace(extraParams[0]?.ToString()))
            {
                OnOiReady(false, "Invalid operation Panel parameter.");
                return;
            }
            Dictionary<string, string> parameters;
            try
            {
                 parameters = JsonConvert.DeserializeObject<Dictionary<string, string>>((string)extraParams[0]);
            }
            catch (JsonException)
            {
                OnOiReady(false, "Invalid operation Panel parameter.");
                return;
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            _valueInputOiForm = new ValueInputOiForm((ISequenceGroup)sequenceData, parameters);
            Application.Run(_valueInputOiForm);
            OnOiReady(_valueInputOiForm.IsConfirmed, _valueInputOiForm.ErrorMessage);
            if (!_valueInputOiForm.IsConfirmed)
            {
                _valueInputOiForm.Dispose();
                _valueInputOiForm = null;
                return;
            }
            _valueInputOiForm.Dispose();
            _valueInputOiForm = null;
            this._runtimeStatusForm = new RuntimeStatusForm((ISequenceGroup) sequenceData);
            Application.Run(_runtimeStatusForm);
        }

        public override void TestGenerationStart(ITestGenerationInfo generationInfo)
        {
            _runtimeStatusForm?.ShowAllSequenceState(RuntimeState.TestGen);
            _runtimeStatusForm?.ShowStatus(RuntimeState.TestGen);
        }

        public override void TestGenerationOver(ITestGenerationInfo generationInfo)
        {
            if (generationInfo.GenerationInfos[0].Status == GenerationStatus.Success)
            {
                _runtimeStatusForm.ShowAllSequenceState(RuntimeState.StartIdle);
            }
            else
            {
                ISessionGenerationInfo sessionGenerationInfo = generationInfo.GenerationInfos[0];
                _runtimeStatusForm.ShowAllSequenceState(RuntimeState.Error);
                _runtimeStatusForm.PrintInformation("Test generation failed.");
                if (null != sessionGenerationInfo.ErrorStack &&
                    !string.IsNullOrWhiteSpace(sessionGenerationInfo.ErrorInfo))
                {
                    ISequenceStep step = SequenceUtils.GetStepFromStack((ISequenceGroup)SequenceData, sessionGenerationInfo.ErrorStack);
                    _runtimeStatusForm.PrintInformation($"<{step?.Name ?? string.Empty}>:{sessionGenerationInfo.ErrorInfo}");
                }
            }
        }

        public override void TestStart(IList<ITestResultCollection> testResults)
        {
        }

        public override void SessionStart(ITestResultCollection results)
        {
            _runtimeStatusForm.ShowStatus(RuntimeState.Running);
        }

        public override void SequenceStart(ISequenceTestResult result)
        {
            _runtimeStatusForm?.ShowSequenceState(result.SequenceIndex, RuntimeState.Running);
            _runtimeStatusForm?.ShowStartTime(result.SequenceIndex, result.StartTime.ToString("yyyy-MM-dd HH:mm:ss.fff"));
        }

        public override void StatusReceived(IRuntimeStatusInfo runtimeInfo)
        {
            ICallStack currentStack = GetCurrentStack(runtimeInfo);
            ISequenceStep currentStep = SequenceUtils.GetStepFromStack((ISequenceGroup)SequenceData, currentStack);
            if (null == currentStep)
            {
                return;
            }
            if (runtimeInfo.StepResults.ContainsKey(currentStack))
            {
                StepResult stepResult = runtimeInfo.StepResults[currentStack];
                if (stepResult != StepResult.NotAvailable)
                {
                    _runtimeStatusForm.PrintInformation($"{currentStep.Name}        {stepResult}");
                }
            }
            if (runtimeInfo.FailedInfos.ContainsKey(0) && !string.IsNullOrWhiteSpace(runtimeInfo.FailedInfos[0]?.Message))
            {
                _runtimeStatusForm.PrintInformation($"<{currentStep.Name}>:{runtimeInfo.FailedInfos[0].Message}");
            }
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

        public override void SequenceOver(ISequenceTestResult sequenceResult)
        {
            _runtimeStatusForm.ShowSequenceState(sequenceResult.SequenceIndex, sequenceResult.ResultState);
            _runtimeStatusForm.ShowEndTime(sequenceResult.SequenceIndex, sequenceResult.EndTime.ToString("yyyy-MM-dd HH:mm:ss.fff"));
        }

        public override void SessionOver(ITestResultCollection sessionResult)
        {
            bool allSequencePassed = sessionResult.All( item => item.Value.ResultState == RuntimeState.Success || item.Value.ResultState == RuntimeState.Over);
            RuntimeState state = allSequencePassed ? RuntimeState.Success : RuntimeState.Failed;
            _runtimeStatusForm.ShowStatus(state);
        }

        public override void TestOver(IList<ITestResultCollection> testResults)
        {
        }

    }
}