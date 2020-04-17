using System;
using System.IO;
using System.Windows.Forms;

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
            string filePath = string.Empty;
           
            if (args.Length == 1 && !string.IsNullOrWhiteSpace(args[0]) && File.Exists(args[0]))
            {
                filePath = args[0];
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm(filePath));
        }
    }
}
