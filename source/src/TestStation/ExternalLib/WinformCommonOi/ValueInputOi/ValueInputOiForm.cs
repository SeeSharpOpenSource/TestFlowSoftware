using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Testflow.Data.Sequence;

namespace TestFlow.Software.WinformCommonOi.ValueInputOi
{
    public partial class ValueInputOiForm : Form
    {
        public bool IsConfirmed { get; set; }

        public string ErrorMessage { get; set; }

        private ISequenceGroup _sequenceData;
        private Dictionary<string, string> _parameters;

        public ValueInputOiForm(ISequenceGroup sequenceData, Dictionary<string, string> parameters)
        {
            InitializeComponent();
            this._sequenceData = sequenceData;
            this._parameters = parameters;
            this.IsConfirmed = false;
        }

        private void button_start_Click(object sender, EventArgs e)
        {
            foreach (Control control in tableLayoutPanel_configItems.Controls)
            {
                ((ConfigItem)control).WriteValue();
            }
            IsConfirmed = true;
            this.Close();
        }

        private void ValueInputOiForm_Load(object sender, EventArgs e)
        {
            label_sequenceName.Text = _sequenceData.Name;
            this.Text = _sequenceData.Name;
            int rowCount = (_parameters.Count + 1)/2;
            if (rowCount > 1)
            {
                this.Height += 50*(rowCount - 1);
                tableLayoutPanel_configItems.RowCount = rowCount;
            }
            int rowIndex = 0;
            int colIndex = 0;
            int index = 0;
            foreach (KeyValuePair<string, string> paramKeyValue in _parameters)
            {
                rowIndex = rowIndex/2;
                colIndex = rowIndex%2;
                index++;
                IVariable variable = GetVariable(paramKeyValue.Value);
                ConfigItem configItem = new ConfigItem(paramKeyValue.Key, variable,
                    null != variable ? string.Empty : paramKeyValue.Value);
                configItem.Show();
                tableLayoutPanel_configItems.Controls.Add(configItem, colIndex, rowIndex);
            }
        }

        private IVariable GetVariable(string variableName)
        {
            return _sequenceData.Variables.FirstOrDefault(item => item.Name.Equals(variableName));
        }
    }
}
