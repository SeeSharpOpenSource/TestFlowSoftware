using System.Collections.Generic;
using System.Linq;
using Testflow.Data.Sequence;

namespace TestStation.ParameterChecker
{
    public static class SequenceGroupChecker
    {
        public static IList<string> Check(ISequenceGroup sequenceGroup)
        {
            List<string> errorInfos = new List<string>(200);

            SequenceChecker setUpChecker = new SequenceChecker(sequenceGroup, sequenceGroup.SetUp);
            setUpChecker.Check(errorInfos);

            foreach (SequenceChecker sequenceChecker in sequenceGroup.Sequences.Select(sequence => new SequenceChecker(sequenceGroup, sequence)))
            {
                sequenceChecker.Check(errorInfos);
            }
            SequenceChecker teardownChecker = new SequenceChecker(sequenceGroup, sequenceGroup.TearDown);
            teardownChecker.Check(errorInfos);
            return errorInfos;
        }
    }
}
