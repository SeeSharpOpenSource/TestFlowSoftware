using System.Collections.Generic;
using Testflow.Data.Sequence;

namespace TestFlow.Software.WinformCommonOi
{
    public interface IOiConfigForm
    {
        void Initialize(ISequenceFlowContainer sequenceData);

        string GetOiConfigData();

        void ShowDialog();
    }
}