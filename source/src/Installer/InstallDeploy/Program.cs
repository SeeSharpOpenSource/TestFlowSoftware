using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace EasyTestDeploy
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("注册文件类型信息...");
            string filePath = args[0];
            if (filePath.Contains("\""))
            {
                filePath = filePath.Replace("\"", "");
            }
            if (filePath.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                filePath = filePath.Substring(0, filePath.Length - 1);
            }
            RegistryFile(filePath);
        }

        /// <summary>
        /// 注册文件信息
        /// </summary>
        private static void RegistryFile(string installPath)
        {
            //后缀文件信息
            FileTypeRegInfo ftri = new FileTypeRegInfo();
            ftri.ExtendName = ".tfseq";
            ftri.Description = "Test Sequence File项目文件";
            ftri.IcoPath = installPath + "\\jytek.ico";
            ftri.ExePath = installPath + "\\Workspace\\JYProductTestPanel\\JYProductTestPanel.exe";

            try
            {
                //注册文件类型
                FileTypeRegister.RegisterFileType(ftri);
                Console.WriteLine("注册文件类型成功");
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"注册文件类型信息失败：{ex.Message}");
                Log.LogEnable = true;
                Log.Print(LogLevel.ERROR, $"Failed to register file type:{ex.Message}");
            }
        }
    }

    /// <summary>
    /// 注册信息
    /// </summary>
    public class FileTypeRegInfo
    {
        // <summary>
        /// 目标类型文件的扩展名
        /// </summary>
        public string ExtendName;  //".xcf"

        /// <summary>
        /// 目标文件类型说明
        /// </summary>
        public string Description; //"XCodeFactory项目文件"

        /// <summary>
        /// 目标类型文件关联的图标
        /// </summary>
        public string IcoPath;

        /// <summary>
        /// 打开目标类型文件的应用程序
        /// </summary>
        public string ExePath;

        public FileTypeRegInfo()
        {
        }

        public FileTypeRegInfo(string extendName)
        {
            this.ExtendName = extendName;
        }
    }

    /// <summary>
    /// FileTypeRegister 用于注册自定义的文件类型。
    /// </summary>
    public class FileTypeRegister
    {
        #region RegisterFileType
        /// <summary>
        /// RegisterFileType 使文件类型与对应的图标及应用程序关联起来。
        /// </summary>        
        public static void RegisterFileType(FileTypeRegInfo regInfo)
        {
            RemoveIfRegistered(regInfo.ExtendName);
            string openValueFormat = "\"{0}\" \"%1\"";
            string openValue = string.Format(openValueFormat, regInfo.ExePath);
            string relationName = regInfo.ExtendName.Substring(1, regInfo.ExtendName.Length - 1).ToUpper() + "_FileType";
            RegistryKey registryKey = Registry.ClassesRoot.CreateSubKey(regInfo.ExtendName);
            if (registryKey != null && registryKey.OpenSubKey("shell") != null &&
                registryKey.OpenSubKey("shell").OpenSubKey("open") != null &&
                registryKey.OpenSubKey("shell").OpenSubKey("open").OpenSubKey("command") != null)
            {
                var varSub = registryKey.OpenSubKey("shell").OpenSubKey("open").OpenSubKey("command");
                var varValue = varSub.GetValue("");

                Log.LogEnable = true;
                Log.Print(LogLevel.ERROR, "1");
                if (Object.Equals(varValue, openValue))
                {
                    Log.LogEnable = true;
                    Log.Print(LogLevel.ERROR, "2");
                    return;
                }
            }

            //删除
            Registry.ClassesRoot.DeleteSubKeyTree(regInfo.ExtendName, false);

            //文件注册
            registryKey = Microsoft.Win32.Registry.ClassesRoot.CreateSubKey(regInfo.ExtendName);
            registryKey.SetValue("文件类型", "Test Sequence File");
            registryKey.SetValue("Content Type", regInfo.Description);

            // 设置默认图标
            RegistryKey iconKey = registryKey.CreateSubKey("DefaultIcon");
            iconKey.SetValue("", regInfo.IcoPath);
            //设置默认打开程序路径
            registryKey = registryKey.CreateSubKey("shell\\open\\command");
            //其中" %1"表示将被双击的文件的路径传给目标应用程序，这样在双击a.xcf文件时，XCodeFactory才知道要打开哪个文件
            Log.Print(LogLevel.DEBUG, "DoubleClick " + regInfo.ExePath);
            registryKey.SetValue("", openValue);
            Log.LogEnable = true;
            Log.Print(LogLevel.ERROR, "3");
            registryKey.Close();

            //RegistryKey relationKey = Registry.ClassesRoot.CreateSubKey(relationName);
            //relationKey.SetValue("", regInfo.Description);

            //RegistryKey iconKey = relationKey.CreateSubKey("DefaultIcon");
            //iconKey.SetValue("", regInfo.IcoPath);

            //RegistryKey shellKey = relationKey.CreateSubKey("Shell");
            //RegistryKey openKey = shellKey.CreateSubKey("Open");
            //RegistryKey commandKey = openKey.CreateSubKey("Command");
            //commandKey.SetValue("", regInfo.ExePath + " %1");

            //relationKey.Close();
        }

        public static void DeleteRegistry()
        {

        }

        /// <summary>
        /// GetFileTypeRegInfo 得到指定文件类型关联信息
        /// </summary>        
        public static FileTypeRegInfo GetFileTypeRegInfo(string extendName)
        {
            RemoveIfRegistered(extendName);

            FileTypeRegInfo regInfo = new FileTypeRegInfo(extendName);

            string relationName = extendName.Substring(1, extendName.Length - 1).ToUpper() + "_FileType";
            RegistryKey relationKey = Registry.ClassesRoot.OpenSubKey(relationName);
            regInfo.Description = relationKey.GetValue("").ToString();

            RegistryKey iconKey = relationKey.OpenSubKey("DefaultIcon");
            regInfo.IcoPath = iconKey.GetValue("").ToString();

            RegistryKey shellKey = relationKey.OpenSubKey("Shell");
            RegistryKey openKey = shellKey.OpenSubKey("Open");
            RegistryKey commandKey = openKey.OpenSubKey("Command");
            string temp = commandKey.GetValue("").ToString();
            regInfo.ExePath = temp.Substring(0, temp.Length - 3);

            return regInfo;
        }

        /// <summary>
        /// UpdateFileTypeRegInfo 更新指定文件类型关联信息
        /// </summary>    
        public static bool UpdateFileTypeRegInfo(FileTypeRegInfo regInfo)
        {
            RemoveIfRegistered(regInfo.ExtendName);

            string extendName = regInfo.ExtendName;
            string relationName = extendName.Substring(1, extendName.Length - 1).ToUpper() + "_FileType";
            RegistryKey relationKey = Registry.ClassesRoot.OpenSubKey(relationName, true);
            relationKey.SetValue("", regInfo.Description);

            RegistryKey iconKey = relationKey.OpenSubKey("DefaultIcon", true);
            iconKey.SetValue("", regInfo.IcoPath);

            RegistryKey shellKey = relationKey.OpenSubKey("Shell");
            RegistryKey openKey = shellKey.OpenSubKey("Open");
            RegistryKey commandKey = openKey.OpenSubKey("Command", true);
            commandKey.SetValue("", regInfo.ExePath + " %1");

            relationKey.Close();

            return true;
        }

        /// <summary>
        /// FileTypeRegistered 指定文件类型是否已经注册
        /// </summary>        
        private static void RemoveIfRegistered(string extendName)
        {
            RegistryKey softwareKey = Registry.ClassesRoot.OpenSubKey(extendName);
            if (softwareKey != null)
            {
                //删除
                Registry.ClassesRoot.DeleteSubKeyTree(extendName, false);
            }
        }
        #endregion
    }
}
