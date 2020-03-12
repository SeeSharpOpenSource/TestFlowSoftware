using System;
using System.Net.NetworkInformation;
using SeeSharpTools.JY.File;

namespace EasyTest.ModelManage.Data
{
    public class TestModelData
    {
        public string A2C { get; }
        public string FixtureCode { get; set; }
        public string ModelName { get; set; }
        public string ScanPrefix { get; set; }
        public int PrefixPostion { get; set; }
        public string ScanSuffix { get; set; }
        public int SuffixPosition { get; set; }
        public string SequenceFile { get; set; }
        public string Customer { get; set; }
        public string TestVersion { get; set; }

        public TestModelData(IniData testModels, string a2C)
        {
            this.A2C = a2C.Replace(" ", "");
            KeyDataCollection testModelData = Utility.CheckAndGetSection(testModels, a2C);
            this.ModelName = Utility.CheckAndGetKey(testModelData, "Model");
            this.FixtureCode = Utility.CheckAndGetKey(testModelData, "Fixture Code");
            this.ScanPrefix = Utility.CheckAndGetKey(testModelData, "Scan prefix");
            string prefixPositionStr = Utility.CheckAndGetKey(testModelData, "prefix position");
            this.PrefixPostion = Utility.TryParseInt(prefixPositionStr);
            this.ScanSuffix = Utility.CheckAndGetKey(testModelData, "Scan suffix");
            string suffixPositionStr = Utility.CheckAndGetKey(testModelData, "suffix position");
            this.SuffixPosition = Utility.TryParseInt(suffixPositionStr);
            this.SequenceFile = Utility.CheckAndGetKey(testModelData, "Path");
            this.Customer = Utility.CheckAndGetKey(testModelData, "Customer");
            this.TestVersion = Utility.CheckAndGetKey(testModelData, "Test Version");
        }

        public bool IsFitPattern(string productCode)
        {
            if (PrefixPostion >= 0 && !productCode.StartsWith(ScanPrefix, StringComparison.CurrentCulture))
            {
                return false;
            }
            if (SuffixPosition >= 0 && !productCode.EndsWith(ScanSuffix, StringComparison.CurrentCulture))
            {
                return false;
            }
            return true;
        }
    }
}