using Testflow.Data.Sequence;
using TestFlow.SoftDevCommon;

namespace TestFlow.DevSoftware.Common
{
    public class StepResultInfo
    {
        public string Stack { get; }
        public string LimitVariable { get; }
        public ResultState Result { get; set; }
        public string LimitValue { get; set; }
        public int SequenceIndex { get; }

        public StepResultInfo(string stack, string limitVariable, int sequenceIndex)
        {
            this.Stack = stack;
            this.LimitVariable = limitVariable;
            this.SequenceIndex = sequenceIndex;
            Reset();
        }

        public void Reset()
        {
            this.Result = ResultState.NA;
            this.LimitValue = null;
        }
    }
}