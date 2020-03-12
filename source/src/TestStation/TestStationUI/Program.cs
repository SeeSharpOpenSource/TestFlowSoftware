using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using TestStation.Authentication;
using TestStation.Common;

namespace TestStation
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
            AuthenticationSession session = AuthenticationManage.GetLoginSession(null);
            if (null == session)
            {
                return;
            }
            try
            {
                Application.Run(new MainForm(session, filePath));
            }
            finally
            {
                AuthenticationManage.Logout(session);
            }
        }
    }
}
