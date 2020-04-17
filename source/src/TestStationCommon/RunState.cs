namespace TestFlow.SoftDevCommon
{
    public enum RunState
    {
        NotAvailable = -1,
        
        /// <summary>
        /// 编辑空闲
        /// </summary>
        EditIdle = 0,

        /// <summary>
        /// 编辑处理
        /// </summary>
        EditProcess = 1,

        /// <summary>
        /// 运行空闲
        /// </summary>
        RunIdle = 2,

        /// <summary>
        /// 运行阻塞
        /// </summary>
        RunBlock = 3,

        /// <summary>
        /// 运行操作处理
        /// </summary>
        RunProcessing = 4,

        /// <summary>
        /// 运行
        /// </summary>
        Running = 5,

        /// <summary>
        /// 运行结束
        /// </summary>
        RunOver = 6
    }
}