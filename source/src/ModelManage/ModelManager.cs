using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using EasyTest.ModelManage.Data;
using SeeSharpTools.JY.File;

namespace EasyTest.ModelManage
{
    public static class ModelManager
    {
        internal static EquipmentData GetEquipmentData(string configFilePath)
        {
            string hostName = Dns.GetHostName();
            IniData equipmentDatas = Utility.GetIniData(configFilePath);
            string stationSection = GetStationSection(equipmentDatas, hostName);
            if (string.IsNullOrWhiteSpace(stationSection))
            {
                throw new ApplicationException(
                    $"The information of host '{hostName}' cannot be found in configuration file.");
            }
            return new EquipmentData(equipmentDatas, stationSection);
        }

        internal static IList<TestModelData> GetFittedModels(EquipmentData equipmentData, string productString)
        {
            List<TestModelData> fittedModels = new List<TestModelData>(20);
            fittedModels.AddRange(equipmentData.TestModels.Where(testModelData => testModelData.IsFitPattern(productString)));
            return fittedModels;
        }

        public static EquipmentData ShowModelSelctionForm(string configFilePath)
        {
            TestModelSelectionForm modelSelectionForm = new TestModelSelectionForm(configFilePath);
            Application.Run(modelSelectionForm);
            return modelSelectionForm.EquipmentData;
        }

        public static EquipmentData ShowModelSelectionDialog(string configFilePath)
        {
            TestModelSelectionForm modelSelectionForm = new TestModelSelectionForm(configFilePath);
            modelSelectionForm.ShowDialog();
            return modelSelectionForm.EquipmentData;
        }

        private static string GetStationSection(IniData iniData, string stationName)
        {
            foreach (SectionData sectionData in iniData.Sections)
            {
                if (sectionData.Keys.ContainsKey("StaionID") && stationName.Equals(sectionData.Keys["StaionID"]))
                {
                    return sectionData.SectionName;
                }
            }
            return null;
        }
    }
}