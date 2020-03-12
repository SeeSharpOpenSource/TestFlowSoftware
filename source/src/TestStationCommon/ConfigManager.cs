using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SeeSharpTools.JY.File;

namespace TestStation.Common
{
    public class ConfigManager
    {
        const string ConfigFilePath = "easytestconfig.ini";
        private string _configFileName;

        private readonly Dictionary<string, string> _configData;

        public ConfigManager()
        {
            this._configData = new Dictionary<string, string>(100);
            string testflowHome = Environment.GetEnvironmentVariable("TESTFLOW_HOME");
            if (string.IsNullOrWhiteSpace(testflowHome) || !Directory.Exists(testflowHome))
            {
                Log.Print(LogLevel.FATAL, "Invalid testflow home.");
                throw new ApplicationException("Invalid testflow home.");
            }
            if (!testflowHome.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                testflowHome += Path.DirectorySeparatorChar;
            }
            _configFileName = testflowHome + ConfigFilePath;
        }

        public void LoadConfigData()
        {
            
            IniData configData = IniHandler.ReadIniFile(_configFileName, Encoding.UTF8);
            _configData.Clear();
            foreach (SectionData sectionData in configData.Sections)
            {
                foreach (KeyData keyData in sectionData.Keys)
                {
                    _configData.Add(keyData.KeyName, keyData.Value);
                }
            }
//            if (!_configData.ContainsKey("ReportPath") || string.IsNullOrWhiteSpace(_configData["ReportPath"]) ||
//                !Directory.Exists(_configData["ReportPath"]))
//            {
//                string workSpaceDir = Environment.GetEnvironmentVariable("TESTFLOW_WORKSPACE");
//                if (string.IsNullOrWhiteSpace(workSpaceDir) || !Directory.Exists(workSpaceDir))
//                {
//                    Log.Print(LogLevel.FATAL, "Invalid workspace directory.");
//                    throw new ApplicationException("Invalid workspace directory.");
//                }
//                if (!workSpaceDir.EndsWith(Path.DirectorySeparatorChar.ToString()))
//                {
//                    workSpaceDir += Path.DirectorySeparatorChar;
//                }
//                workSpaceDir += $"Report{Path.DirectorySeparatorChar}";
//                if (Directory.Exists(workSpaceDir))
//                {
//                    Directory.CreateDirectory(workSpaceDir);
//                }
//                _configData["ReportPath"] = workSpaceDir;
//            }
        }

        public void ApplyConfig(string property, string value)
        {
            if (_configData.ContainsKey(property))
            {
                _configData[property] = value;
            }
            else
            {
                _configData.Add(property, value);
            }
        }

        public void WriteConfigData()
        {
            IniData configData = IniHandler.ReadIniFile(_configFileName, Encoding.UTF8);
            foreach (SectionData sectionData in configData.Sections)
            {
                foreach (KeyData keyData in sectionData.Keys)
                {
                    if (_configData.ContainsKey(keyData.KeyName))
                    {
                        keyData.Value = _configData[keyData.KeyName];
                    }
                }
            }
            IniHandler.WriteIniFile(_configFileName, configData, Encoding.UTF8);
        }

        public TDataType GetConfig<TDataType>(string configName)
        {
            string valueStr = _configData.ContainsKey(configName) ? _configData[configName] : null;
            Type dataType = typeof(TDataType);
            object dataValue;
            if (dataType == typeof (string))
            {
                dataValue =  valueStr ?? string.Empty;
            }
            else if (dataType == typeof (int))
            {
                dataValue = null != valueStr ? int.Parse(valueStr) : 0;
            }
            else if (dataType == typeof(long))
            {
                dataValue = null != valueStr ? long.Parse(valueStr) : 0L;
            }
            else if (dataType == typeof (bool))
            {
                dataValue = null != valueStr && bool.Parse(valueStr);
            }
            else if (dataType.IsEnum)
            {
                dataValue = null != valueStr
                    ? Enum.Parse(dataType, valueStr)
                    : Enum.Parse(dataType, Enum.GetNames(dataType)[0]);
            }
            else
            {
                throw new ApplicationException($"Unable to cast to type {dataType.Name}");
            }
            return (TDataType) dataValue;
        }
    }
}