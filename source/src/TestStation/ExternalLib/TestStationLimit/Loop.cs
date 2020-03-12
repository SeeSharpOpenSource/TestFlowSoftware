using Testflow.FlowControl;

namespace TestStationLimit
{
    public static class Loop
    {
        public static bool AlwaysTrue()
        {
            return true;
        }

        public static bool SetBoolValue(bool value)
        {
            return value;
        }

        // 判断Continue变量以决定是否继续执行的方法，如果为false，则跳出执行
        public static void IsContinue(bool value)
        {
            if (value)
            {
                return;
            }
            // 如果是false，则直接跳出当前循环
            throw new TestflowLoopBreakException(true, null);
        }

        public static bool IsTrue(bool value)
        {
            return value;
        }

        public static bool IsFalse(bool value)
        {
            return !value;
        }
    }
}