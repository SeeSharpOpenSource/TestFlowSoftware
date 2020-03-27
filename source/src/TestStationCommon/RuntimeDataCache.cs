using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using SeeSharpTools.JY.File;
using Testflow;
using Testflow.Data.Sequence;

namespace TestStation.Common
{
    public class RuntimeDataCache
    {
        public TestflowRunner TestflowEntity { get; }
        public long Target { get; set; }
        private int _qualifiedCount;
        public int QualifiedCount => _qualifiedCount;
        private int _unqualifiedCount;
        public int UnqualifiedCount => _unqualifiedCount;
        public int QualifiedRatio { get; private set; }
        public double UutProgress { get; set; }
        public string Customer { get; set; }
        public string Model { get; set; }
        public string ProductNumber { get; set; }
        public string TestVersion { get; set; }
        public string Device { get; set; }
        public double ElapsedSeconds { get; set; }
        public string StationName { get; set; }
        public string SequenceName { get; set; }

        public string SerialNumber { get; set; }

        public bool EnableStartTiming { get; set; }
        public bool EnableEndTiming { get; set; }

        public string MainStartStack { get; set; }
        public string MainOverStack { get; set; }
        public string PostStartStack { get; set; }
        public string ScanCodeStack { get; set; }

        public ISequenceGroup SequenceData { get; set; }

        public RunType RunType { get; set; }

        public RuntimeDataCache(TestflowRunner testflowEntity)
        {
            this.TestflowEntity = testflowEntity;
            this.Target = 500;
            this._qualifiedCount = 0;
            this._unqualifiedCount = 0;
            this.QualifiedRatio = 0;
            this.Customer = Constants.NASerialNo;
            this.Model = Constants.NASerialNo;
            this.TestVersion = Constants.NASerialNo;
            this.ElapsedSeconds = 0;
            this.EnableStartTiming = false;
            this.EnableEndTiming = false;
            this.MainStartStack = string.Empty;
            this.MainOverStack = string.Empty;
            this.PostStartStack = string.Empty;
            this.RunType = RunType.Independent;
            this.SerialNumber = Constants.NASerialNo;
            this.StationName = Dns.GetHostName();
        }

        public void Reset()
        {
            this._qualifiedCount = 0;
            this._unqualifiedCount = 0;
            this.QualifiedRatio = 0;
        }

        public void AddQualified()
        {
            Interlocked.Increment(ref _qualifiedCount);
            this.QualifiedRatio = (int)(_qualifiedCount / Target);
        }

        public void AddUnQualified()
        {
            Interlocked.Increment(ref _unqualifiedCount);
        }

        private Dictionary<string, string> ReadSectionData(IniData iniData, string sectionName, params string[] keys)
        {
            const string fileErrorFormat = "Invalid config file: {0} does not exist.";
            if (!iniData.Sections.ContainsSection(sectionName))
            {
                throw new ApplicationException(string.Format(fileErrorFormat, $"Section <{sectionName}>"));
            }
            KeyDataCollection sectionData = iniData.Sections[sectionName];
            Dictionary<string, string> datas = new Dictionary<string, string >(keys.Length);
            foreach (string key in keys)
            {
                if (!sectionData.ContainsKey(key))
                {
                    throw new ApplicationException(string.Format(fileErrorFormat, $"Key <{key}>"));
                }
                datas.Add(key, sectionData[key]);
            }
            return datas;
        }
    }
}