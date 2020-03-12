using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestStation
{
    /// <summary>
    /// 日志打印类，用于在程序逻辑中打印一些日记记录到文件，方便调试程序，
    /// 同时客户使用时如果遇到问题，也可以使能日志打印的功能，方便定位错误的原因
    /// </summary>
    public static class Log
    {       
        /// <summary>
        /// 使能日志打印功能
        /// </summary>
        public static bool LogEnable
        {
            get
            {
                return _logEnable;
            }
            set
            {
                _logEnable = _writting = value;
                if (value)
                {
                    if (_tskWriteFile == null)
                    {
                        _tskWriteFile = new Task(FuncWriteLog, TaskCreationOptions.LongRunning);
                        _tskWriteFile.Start();
                    }
                }
                else
                {
                    if (_tskWriteFile != null && _tskWriteFile.Status == TaskStatus.Running)
                    {
                        _tskWriteFile.Wait(100);
                        _tskWriteFile.Dispose();
                    }
                    _tskWriteFile = null;
                }
            }  
        }
        private static bool _logEnable;

        /// <summary>
        /// 日志等级，默认为ERROR
        /// </summary>
        public static LogLevel LogLevel
        {
            get
            {
                return _logLevel;
            }
            set
            {
                _logLevel = value;
            }
        }
        private static LogLevel _logLevel;
       
        private static Queue _logMsgQ;//用于日志消息缓存的队列，首次调用时初始化
        private static Task _tskWriteFile;//异步写日志
        private static bool _writting;//写日志标志位

        /// <summary>
        /// 构造函数
        /// </summary>
        static Log()
        {
            _logEnable = false;//初始化为不打印日志  
            _logLevel = LogLevel.ERROR;//初始化日志打印等级
            Queue q = new Queue(1024);//初始化大小为1024
            _logMsgQ = Queue.Synchronized(q);//获取Queue.Synchronized方法包装的Queue q
        }

        /// <summary>
        ///  写入日志文件,需要定义宏LOGENABLE 或 DEBUG
        /// </summary>
        public static void Print(LogLevel logLevel, string logMsg, params object[] args)
        {
            // 这段代码会像往常那样编译，但读取debug配置文件包含在#if子句内。这行代码只有在前面的#define命令定义了符号DEBUG后才执行。
            //当编译器遇到#if语句后，将先检查相关的符号是否存在，如果符号存在，就只编译#if块中的代码。
            //否则，编译器会忽略所有的代码，直到遇到匹配的#endif指令为止。一般是在调试时定义符号DEBUG，把不同的调试相关代码放在#if子句中。
            //在完成了调试后，就把#define语句注释掉，所有的调试代码会奇迹般地消失，可执行文件也会变小，
            //最终用户不会被这些调试信息弄糊涂(显然，要做更多的测试，确保代码在没有定义DEBUG的情况下也能工作)。这项技术在C和C++编程中非常普通，称为条件编译(conditional compilation)。
#if (LOGENABLE || DEBUG) //对于Release版本可以选择关闭日志功能，对于Debug版本默认开启
            if (_logEnable == false || ((int)logLevel < (int)_logLevel))
            {
                return;
            }
            DateTime t = DateTime.Now;
            var msg = string.Format(logMsg, args); //2016-04-26 10:59:10.6679687
            var s_t = $"[{t.Year:D4}-{t.Month:D2}-{t.Day:D2} {t.TimeOfDay.ToString()}]\t{msg}";
            _logMsgQ.Enqueue(s_t);
#endif
        }


        /// <summary>
        /// 写入日志文件,需要定义宏LOGENABLE 或 DEBUG，此方法兼容旧版
        /// </summary>
        /// <param name="logMsg">要打印的消息内容</param>
        /// <param name="args">参数</param>
        public static void Print(string logMsg, params object[] args)
        {
#if (LOGENABLE || DEBUG)
            if (_logEnable == false)
            {
                return;
            }
            DateTime t = DateTime.Now;
            var msg = string.Format(logMsg, args); //2016-04-26 10:59:10.6679687
            var s_t = $"[{t.Year:D4}-{t.Month:D2}-{t.Day:D2} {t.TimeOfDay.ToString()}]\t{msg}";
            _logMsgQ.Enqueue(s_t);
#endif
        }

        /// <summary>
        /// 轮询日志队列的定时器回调函数
        /// </summary>
        /// <param name="state"></param>
        private static void FuncWriteLog()
        {
            while (_writting)
            {
                //如果之前的回调正在进行或日志队列为空，都直接返回
                if (_logMsgQ.Count <= 0)
                {
                    Thread.Sleep(10);
                    continue;
                }

                FileInfo finfo = null;
                FileStream fs = null;
                StreamWriter sw = null;
                try
                {
                    DateTime t = DateTime.Now;
                    //指定日志文件的目录
                    string fname = "";
                    if (Environment.OSVersion.ToString().Contains("Unix"))
                    {
                        fname = Directory.GetCurrentDirectory() + "/" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";
                    }
                    else
                    {
                        fname = Directory.GetCurrentDirectory() + "\\" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";
                    }
                    //string fname = Directory.GetCurrentDirectory() + "\\"
                    //               + t.Year.ToString("D04") + t.Month.ToString("D02") + t.Day.ToString("D02") + ".log";

                    //定义文件信息对象
                    finfo = new FileInfo(fname);
                    fs = null;
                    if (!finfo.Exists)
                    {
                        fs = File.Create(fname);
                        fs.Close();
                        finfo = new FileInfo(fname);
                    }

                    //判断文件是否存在以及是否大于10M
                    if (finfo.Length > 1024 * 1024 * 10)
                    {
                        //文件超过10MB则重命名
                        if (Environment.OSVersion.ToString().Contains("Unix"))
                        {
                            File.Move(
                           Directory.GetCurrentDirectory() + "/" + DateTime.Now.ToString("yyyy-MM-dd") + ".log",
                           Directory.GetCurrentDirectory() + "/" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".log");
                        }
                        else
                        {
                            File.Move(
                           Directory.GetCurrentDirectory() + "\\" + DateTime.Now.ToString("yyyy-MM-dd") + ".log",
                           Directory.GetCurrentDirectory() + "\\" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".log");
                        }
                      
                        //File.Move(
                        //   Directory.GetCurrentDirectory() + "\\" + t.Year.ToString("D04") + t.Month.ToString("D02") +
                        //   t.Day.ToString("D02") + ".log",
                        //   Directory.GetCurrentDirectory() + "\\" + t.Year.ToString("D04") + t.Month.ToString("D02") +
                        //   t.Day.ToString("D02") + t.Hour.ToString("D02") + t.Minute.ToString("D02") +
                        //   t.Second.ToString("D02") + ".log");
                    }

                    fs = finfo.OpenWrite();
                    sw = new StreamWriter(fs);

                    while (_logMsgQ.Count > 0)
                    {
                        //设置写数据流的起始位置为文件流的末尾
                        sw.BaseStream.Seek(0, SeekOrigin.End);
                        var s_t = _logMsgQ.Dequeue().ToString();
                        System.Diagnostics.Debug.Print(s_t);//打印到输出窗口
                        //写入日志内容并换行
                        sw.Write(s_t + "\r\n");
                    }
                }
                catch 
                {

                }
                finally
                {
                    //清空缓冲区内容，并把缓冲区内容写入基础流
                    sw?.Flush();
                    //关闭写数据流
                    sw?.Close();
                    fs?.Close();
                }
                Thread.Sleep(10);
            }
        }

    }

    /// <summary>
    /// 日志级别定义
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// DEBUG Level指出细粒度信息事件对调试应用程序是非常有帮助的
        /// </summary>
        DEBUG,

        /// <summary>
        /// INFO level表明 消息在粗粒度级别上突出强调应用程序的运行过程
        /// </summary>
        INFO,

        /// <summary>
        /// WARN level表明会出现潜在错误的情形。
        /// </summary>
        WARN,

        /// <summary>
        /// ERROR level指出虽然发生错误事件，但仍然不影响系统的继续运行。
        /// </summary>
        ERROR,

        /// <summary>
        /// FATAL level指出每个严重的错误事件将会导致应用程序的退出。
        /// </summary>
        FATAL
    }

}
