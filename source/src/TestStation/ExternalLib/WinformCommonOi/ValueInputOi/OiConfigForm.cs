using System;
using System.Collections.Generic;
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

        public OiConfigForm()
        {
            InitializeComponent();
            Column_variableNmae.Items.AddRange(_variables);
            _configData = null;
        }

        public void Initialize(ISequenceFlowContainer sequenceData)
        {
            IVariableCollection variableCollection;
            if (sequenceData is ISequenceGroup)
            {
                variableCollection = ((ISequenceGroup) sequenceData).Variables;
            }
            else if (sequenceData is ITestProject)
            {
                variableCollection = ((ITestProject) sequenceData).Variables;
            }
            else
            {
                throw new ApplicationException("Invalid sequence type.");
            }
            _variables = new List<string>(variableCollection.Count);
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
            this.ShowDialog();
        }

        private void button_confirm_Click(object sender, EventArgs e)
        {
            _configData = new Dictionary<string, string>(dataGridView_paramconfig.RowCount);
            foreach (DataGridViewRow rowData in dataGridView_paramconfig.Rows)
            {
                string paramName = rowData.Cells[0].Value?.ToString();
                string variableName = rowData.Cells[1].Value?.ToString();
                if (null != paramName && null != variableName)
                {
                    _configData.Add(paramName, variableName);
                }
            }
            this.Close();
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {

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
    }
}
