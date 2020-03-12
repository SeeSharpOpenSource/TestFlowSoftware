namespace TestStation.OperationPanel
{
    /// <summary>
    /// OI的运行方式
    /// </summary>
    public enum RunType
    {
        /// <summary>
        /// OI作为TestStation的结果显示界面运行
        /// </summary>
        Slave = 0,

        /// <summary>
        /// OI独立运行
        /// </summary>
        Independent = 1
    }
}