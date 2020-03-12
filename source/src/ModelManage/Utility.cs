using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using EasyTest.ModelManage.Data;
using SeeSharpTools.JY.File;

namespace EasyTest.ModelManage
{
    internal static class Utility
    {
        public static IniData GetIniData(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new ApplicationException($"Config file '{filePath}' does not exist.");
            }
            return IniHandler.ReadIniFile(filePath, Encoding.UTF8);
        }


        public static KeyDataCollection CheckAndGetSection(IniData iniData, string sectionName)
        {
            if (!iniData.Sections.ContainsSection(sectionName))
            {
                throw new ApplicationException($"Section '{sectionName}' cannot be found in config file.");
            }
            return iniData.Sections[sectionName];
        }

        public static string CheckAndGetKey(KeyDataCollection sectionData, string keyName)
        {
            if (!sectionData.ContainsKey(keyName))
            {
                throw new ApplicationException($"Key '{keyName}' cannot be found in config file.");
            }
            string keyValue = sectionData[keyName].Trim();
            // 删除路径上的引号
            if (keyValue.StartsWith("\"") && keyValue.EndsWith("\"") && keyValue.Length >= 2)
            {
                keyValue = keyValue.Substring(1, keyValue.Length - 2).Trim();
            }
            return keyValue;
        }

        public static int TryParseInt(string valueStr)
        {
            int value = -1;
            if (string.IsNullOrWhiteSpace(valueStr))
            {
                return value;
            }
            if (!int.TryParse(valueStr, out value))
            {
                throw new ApplicationException($"Value {valueStr} is invalid value for integer.");
            }
            return value;
        }
    }
}