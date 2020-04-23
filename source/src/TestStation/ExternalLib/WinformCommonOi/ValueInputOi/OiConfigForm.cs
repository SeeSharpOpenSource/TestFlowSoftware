using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;
using Testflow.Data.Sequence;
using Testflow.Usr;

namespace TestFlow.Software.WinformCommonOi.ValueInputOi
{
    public partial class OiConfigForm : Form, IOiConfigForm
    {
        private List<string> _variables;
        private Dictionary<string, string> _configData;
        private IOperationPanelInfo _oiInfo;

        public OiConfigForm()
        {
            InitializeComponent();
        }

        public void Initialize(ISequenceFlowContainer sequenceData)
        {
            IVariableCollection variableCollection;
            if (sequenceData is ISequenceGroup)
            {
                variableCollection = ((ISequenceGroup) sequenceData).Variables;
                _oiInfo = ((ISequenceGroup) sequenceData).Info.OperationPanelInfo;
            }
            else if (sequenceData is ITestProject)
            {
                variableCollection = ((ITestProject) sequenceData).Variables;
            }
            else
            {
                throw new ApplicationException("Invalid sequence type.");
            }
            _variables = new List<string>(from item in variableCollection select item.Name);
            if (_variables?.Count > 0)
            {
                Column_variableName.Items.Add(string.Empty);
                foreach (string variable in _variables)
                {
                    Column_variableName.Items.Add(variable);
                }
            }
            _configData = null;
            ShowParameter();
        }

        public string GetOiConfigData()
        {
            string configDataStr = string.Empty;
            if (null == _configData)
            {
                return configDataStr;
            }
            try
            {
                configDataStr = JsonConvert.SerializeObject(_configData);
            }
            catch (JsonException ex)
            {
                throw new TestflowDataException(0, ex.Message);
            }
            return configDataStr;
        }

        public void ShowDialog()
        {
            base.ShowDialog();
        }

        private void button_confirm_Click(object sender, EventArgs e)
        {
            _configData = new Dictionary<string, string>(dataGridView_paramconfig.RowCount);
            foreach (DataGridViewRow rowData in dataGridView_paramconfig.Rows)
            {
                string paramName = rowData.Cells[0].Value?.ToString();
                string variableName = rowData.Cells[1].Value?.ToString();
                string constantValue = rowData.Cells[2].Value?.ToString();
                if (null != paramName && !string.IsNullOrWhiteSpace(variableName))
                {
                    _configData.Add(paramName, variableName);
                }
                else if (null != paramName && !string.IsNullOrWhiteSpace(constantValue))
                {
                    _configData.Add(paramName, constantValue);
                }
            }
            this.Close();
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            _configData = null;
            this.Close();
        }

        private void button_add_Click(object sender, EventArgs e)
        {
            dataGridView_paramconfig.Rows.Add("ParamName", "");
        }

        private void button_delete_Click(object sender, EventArgs e)
        {
            int selectedRowIndex = dataGridView_paramconfig.CurrentCell?.RowIndex ?? -1;
            if (selectedRowIndex < 0 || selectedRowIndex >= dataGridView_paramconfig.RowCount)
            {
                return;
            }
            dataGridView_paramconfig.Rows.RemoveAt(selectedRowIndex);
        }

        private void ShowParameter()
        {
            dataGridView_paramconfig.Rows.Clear();
            if (string.IsNullOrWhiteSpace(_oiInfo?.Parameters))
            {
                return;
            }
            try
            {
                Dictionary<string, string> parameters =
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(_oiInfo.Parameters);
                foreach (KeyValuePair<string, string> keyValuePair in parameters)
                {
                    // 配置项为变量
                    if (_variables.Contains(keyValuePair.Value))
                    {
                        dataGridView_paramconfig.Rows.Add(keyValuePair.Key, keyValuePair.Value, "");
                    }
                    // 配置项为常量
                    else
                    {
                        dataGridView_paramconfig.Rows.Add(keyValuePair.Key, "", keyValuePair.Value);
                    }
                }
            }
            catch (JsonException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
