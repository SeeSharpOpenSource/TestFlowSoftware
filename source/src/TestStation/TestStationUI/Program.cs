using System;
using System.IO;
using System.Windows.Forms;
using SeeSharpTools.JY.Report;
using TestFlow.SoftDevCommon;

namespace TestFlow.DevSoftware
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            InitializeLogger();
            GlobalInfo globalInfo = null;
            try
            {
                globalInfo = GlobalInfo.GetInstance();
                string filePath = string.Empty;
                if (args.Length == 1 && !string.IsNullOrWhiteSpace(args[0]) && File.Exists(args[0]))
                {
                    filePath = args[0];
                }
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm(filePath, globalInfo));
            }
            finally
            {
                globalInfo?.TestflowEntity.Dispose();
                Logger.Close();
            }
        }

        private static void InitializeLogger()
        {
            string testflowHome = Environment.GetEnvironmentVariable("TESTFLOW_HOME");
            if (string.IsNullOrWhiteSpace(testflowHome))
            {
                throw new ApplicationException("TestFlow home does not configured.");
            }
            if (!testflowHome.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                testflowHome += Path.DirectorySeparatorChar;
            }
            string loggerPath = $"{testflowHome}Log{Path.DirectorySeparatorChar}{DateTime.Now.ToString("yyyy-MM-dd")}-software.log";
            LogConfig logConfig = new LogConfig()
            {
                Header = string.Empty
            };
            logConfig.FileLog.Path = loggerPath;
            Logger.Initialize(logConfig);
        }
    }
}
