namespace TestStation.Common
{
    public static class Constants
    {
        public const string ContinueVariable = "Continue";
        public const string TimingEnableVar = "EnableUserTiming";
        public const string StartTimeVar = "StartTime";
        public const string EndTimeVar = "EndTime";
        public const string TimeFormat = "yyyy-MM-ddTHH:mm:ss.fffffffK";
        public const string StartTimingMethod = "StartTiming";
        public const string EndTimingMethod = "StartTiming";
        public const string SerialNoVarName = "SerialNumber";
        public const string DutIndexVarName = "DUTIndex";
        public const string UutIndexVar = "UutIndex";
        public const string NASerialNo = "N/A";
        public const string NAModelNo = "N/A";
        public const string EmptySn = "EmptySN";
        public const string NullValue = "NULL";
        public const string DefaultWaitTime = "100";
        public const string ProjectNamePostfix = ".tfseq";

        public const string StartTimingStepName = "Start Timing";
        public const string EndTimingStepName = "End Timing";
        public const string WaitStepName = "Wait";

        public const string LocalVarPrefix = "Local";
        public const string GlobalVarPrefix = "Global";
        public const string VaraibleDelim = ".";

        public const string StringLimit = "String";
        public const string BoolLimit = "Boolean";
        public const string NumericLimit = "Numeric";
        public const string ActionType = "Action";
        public const string TestType = "Test";
        public const string SeqCallType = "Sequence Call";
        public const string LimitStepPrefix = "Limit";

        public const string MethodStepName = "Method";
        public const string ReservedStep = "ReservedStep";
        public const string ConditionStepName = "Condition";

        public const string ConditionNone = "None";
        public const string ConditionTrue = "IsTrue";
        public const string ConditionFalse = "IsFalse";

        public const bool DefaultTestRecord = true;
        public const bool DefaultActionRecord = false;
        public const bool DefaultSeqCallRecord = false;

        public const bool DefaultBreakIfFailed = true;

        public const string UnavailableValue = "/";

        public const int SequenceGroupIndex = -3;
    }
}