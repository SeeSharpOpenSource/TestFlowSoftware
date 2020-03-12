using System;
using System.Collections.Generic;
using SeeSharpTools.JY.File;

namespace EasyTest.ModelManage.Data
{
    public class EquipmentData
    {
        public string EquipmentName { get; }
        public string EquipmentCode { get; private set; }
        public string StationID { get; private set; }
        public string Line { get; private set; }
        public string ProductConfigFile { get; private set; }
        public TestModelData SelectedTestModel { get; private set; }
        public List<TestModelData> TestModels { get; private set; }

        public EquipmentData(IniData configFileData, string equipmentName)
        {
            this.EquipmentName = equipmentName;
            KeyDataCollection equipmentInfo = Utility.CheckAndGetSection(configFileData, equipmentName);
            this.EquipmentCode = Utility.CheckAndGetKey(equipmentInfo, "EquipmentCode");
            this.StationID = Utility.CheckAndGetKey(equipmentInfo, "StaionID");
            this.Line = Utility.CheckAndGetKey(equipmentInfo, "Line");
            this.ProductConfigFile = Utility.CheckAndGetKey(equipmentInfo, "Path");
            ReloadTestModels();
        }

        public void SetSelectedModel(TestModelData testModel)
        {
            this.SelectedTestModel = testModel;
            this.TestModels.Clear();
        }

        public void ReloadTestModels()
        {
            IniData testModelDatas = Utility.GetIniData(ProductConfigFile);
            if (null == TestModels)
            {
                TestModels = new List<TestModelData>(testModelDatas.Sections.Count);
            }
            else
            {
                TestModels.Clear();
            }
            foreach (SectionData sectionData in testModelDatas.Sections)
            {
                TestModels.Add(new TestModelData(testModelDatas, sectionData.SectionName));
            }
        }
    }
}