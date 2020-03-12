using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Drawing;
using TestStation.Common;

namespace TestStation
{
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
            if (FileTypeRegistered(regInfo.ExtendName))
            {
                return;
            }

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
                if (Object.Equals(varValue, regInfo.ExePath + " %1"))
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
            Log.Print(LogLevel.DEBUG, "DoubleClick "+regInfo.ExePath);
            registryKey.SetValue("", regInfo.ExePath + " %1");
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

        /// <summary>
        /// GetFileTypeRegInfo 得到指定文件类型关联信息
        /// </summary>        
        public static FileTypeRegInfo GetFileTypeRegInfo(string extendName)
        {
            if (!FileTypeRegistered(extendName))
            {
                return null;
            }

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
            if (!FileTypeRegistered(regInfo.ExtendName))
            {
                return false;
            }


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
        public static bool FileTypeRegistered(string extendName)
        {
            RegistryKey softwareKey = Registry.ClassesRoot.OpenSubKey(extendName);
            if (softwareKey != null)
            {
                return true;
            }

            return false;
        }
        #endregion
    }
}

