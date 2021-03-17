using System;
using System.Collections.Generic;
using Testflow.Data.Sequence;
using Testflow.Runtime.Data;
using Testflow.Runtime.OperationPanel;

namespace TestFlow.Software.WinformCommonOi
{
    public abstract class WinformOperationPanelBase : IOperationPanel
    {
        public Type ConfigFormType { get; protected set; }

        /// <summary>
        /// OI面板可以开始运行的事件
        /// </summary>
        public event Action<bool, Dictionary<string, object>> OiReady;

        public abstract void ShowPanel(ISequenceFlowContainer sequenceData, params object[] extraParams);

        public abstract void TestGenerationStart(ITestGenerationInfo generationInfo);

        public abstract void TestGenerationOver(ITestGenerationInfo generationInfo);

        public abstract void TestStart(IList<ITestResultCollection> testResults);

        public abstract void SessionStart(ITestResultCollection sessionResult);

        public abstract void SequenceStart(ISequenceTestResult sequenceResult);

        public abstract void StatusReceived(IRuntimeStatusInfo runtimeInfo);

        public abstract void SequenceOver(ISequenceTestResult sequenceResult);

        public abstract void SessionOver(ITestResultCollection sessionResult);

        public abstract void TestOver(IList<ITestResultCollection> testResults);
        public void PrintMessage(string message)
        {
            throw new NotImplementedException();
        }

        public void ShowErrorMessage(bool isContinue, string message)
        {
            throw new NotImplementedException();
        }

        public Type ConfigPanelType { get; }

        public ISequenceFlowContainer SequenceData { get; }
        public OiSequenceType SupportSequence { get; }

        public void OnOiReady(bool oiInitSuccess, string message)
        {
            this.OiReady?.Invoke(oiInitSuccess, null);
        }

        public abstract void Dispose();
    }
}