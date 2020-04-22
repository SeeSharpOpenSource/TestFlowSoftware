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
    public partial class ConfigItem : Form
    {
        private IVariable _variable;

        public ConfigItem(string paramName, IVariable variable, string constValue)
        {
            InitializeComponent();
            label_paramName.Text = paramName;
            if (null == variable && !string.IsNullOrWhiteSpace(constValue))
            {
                textBox_value.Text = constValue;
                textBox_value.ReadOnly = true;
            }
            this.TopLevel = false;
            this.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
        }

        public void WriteValue()
        {
            if (null != _variable && !string.IsNullOrWhiteSpace(textBox_value.Text))
            {
                _variable.Value = textBox_value.Text;
            }
        }
    }
}
