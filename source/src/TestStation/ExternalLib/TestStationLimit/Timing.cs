using System;
using System.Threading;

namespace TestStationLimit
{
    public static class Timing
    {
        public static DateTime StartTiming()
        {
            return DateTime.Now;
        }

        public static void Sleep(int milliseconds)
        {
            if (milliseconds <= 0)
            {
                return;
            }
            using (ManualResetEventSlim blockEvent = new ManualResetEventSlim(false))
            {
                blockEvent.Wait(milliseconds);
            }
        }
    }
}