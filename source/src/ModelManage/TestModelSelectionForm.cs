using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EasyTest.ModelManage.Data;

namespace EasyTest.ModelManage
{
    internal partial class TestModelSelectionForm : Form
    {
        private readonly string _configFilePath;
        public EquipmentData EquipmentData { get; private set; }

        public TestModelSelectionForm(string configFile)
        {
            InitializeComponent();
            _configFilePath = configFile;
            try
            {
                EquipmentData = ModelManager.GetEquipmentData(_configFilePath);
                label_lineName.Text = EquipmentData.Line;
                label_stationType.Text = EquipmentData.StationID;
            }
            catch (ApplicationException)
            {
                this.EquipmentData = null;
                throw;
            }
            ShowModelList(string.Empty);
        }

        private void TestModelSelectionForm_Load(object sender, EventArgs e)
        {
        }

        private void ShowModelList(string productNumber)
        {
            dataGridView_matchedModels.Rows.Clear();
            IList<TestModelData> testModelDatas;
            if (string.IsNullOrWhiteSpace(productNumber))
            {
                testModelDatas = EquipmentData.TestModels;
            }
            else
            {
                testModelDatas = ModelManager.GetFittedModels(EquipmentData, productNumber);
            }
            foreach (TestModelData modelData in testModelDatas)
            {
                int rowIndex = dataGridView_matchedModels.Rows.Add(modelData.ModelName, modelData.A2C,
                    EquipmentData.StationID,EquipmentData.Line);
                // 将每个row对应的ModelData配置到该行的Tag里
                dataGridView_matchedModels.Rows[rowIndex].Tag = modelData;
            }
            dataGridView_matchedModels.CurrentCell = testModelDatas.Count > 0
                ? dataGridView_matchedModels.Rows[0].Cells[0]
                : null;
            if (testModelDatas.Count == 1)
            {
                EquipmentData.SetSelectedModel(testModelDatas[0]);
                this.Close();
            }
        }

        private void ShowModelValue(TestModelData modelData)
        {
            if (null == modelData)
            {
                textBox_modelA2C.Text = string.Empty;
                textBox_modelName.Text = string.Empty;
                textBox_modelStationType.Text = string.Empty;
                textBox_modelLine.Text = string.Empty;
                textBox_modelScanPrefix.Text = string.Empty;
                textBox_modelPrefixPosition.Text = string.Empty;
                textBox_modelScanSuffix.Text = string.Empty;
                textBox_modelSuffixPosition.Text = string.Empty;
                textBox_modelSeqPath.Text = string.Empty;
                return;
            }
            textBox_modelA2C.Text = modelData.A2C;
            textBox_modelName.Text = modelData.ModelName;
            textBox_modelStationType.Text = EquipmentData.StationID;
            textBox_modelLine.Text = EquipmentData.Line;
            textBox_modelScanPrefix.Text = modelData.ScanPrefix;
            textBox_modelPrefixPosition.Text = modelData.PrefixPostion.ToString();
            textBox_modelScanSuffix.Text = modelData.ScanSuffix;
            textBox_modelSuffixPosition.Text = modelData.SuffixPosition.ToString();
            textBox_modelSeqPath.Text = modelData.SequenceFile;
        }

        private void ShowMessage(string message, string caption, MessageBoxIcon showIcon)
        {
            MessageBox.Show(message, caption, MessageBoxButtons.OK, showIcon);
        }

        private void dataGridView_matchedModels_CurrentCellChanged(object sender, EventArgs e)
        {
            DataGridViewCell currentCell = dataGridView_matchedModels.CurrentCell;
            if (null != currentCell && currentCell.RowIndex >= 0 &&
                currentCell.RowIndex < dataGridView_matchedModels.RowCount)
            {
                ShowModelValue((TestModelData) currentCell.OwningRow.Tag);
            }
        }

        private void button_search_Click(object sender, EventArgs e)
        {
            ShowModelList(textBox_searchValue.Text);
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            DataGridViewCell currentCell = dataGridView_matchedModels.CurrentCell;
            if (null == currentCell || currentCell.RowIndex < 0 ||
                currentCell.RowIndex >= dataGridView_matchedModels.RowCount)
            {
                ShowMessage("Please select a valid model.", "Model Selection", MessageBoxIcon.Warning);
                return;
            }
            EquipmentData.SetSelectedModel((TestModelData)currentCell.OwningRow.Tag);
            this.Close();
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            EquipmentData.SetSelectedModel(null);
            this.Close();
        }
    }
}
