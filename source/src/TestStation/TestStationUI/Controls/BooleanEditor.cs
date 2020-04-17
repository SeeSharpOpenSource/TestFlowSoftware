using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Testflow.Data;
using Testflow.Data.Sequence;
using TestFlow.DevSoftware.Controls;

namespace TestFlow.DevSoftware
{
    public partial class BooleanEditor : ValueEditor
    {
        public BooleanEditor(IVariable variable) : base(variable)
        {
            InitializeComponent();

            ValuecomboBox.Text = variable.Value ?? string.Empty;
        }

        protected override void OkButton_Click(object sender, EventArgs e)
        {
            string newValue = ValuecomboBox.Text;
            if (!newValue.Equals(base._value))
            {
                base._value = ValuecomboBox.Text;
                base._valueChanged = true;
                Variable.Value = newValue;
            }

            base.OkButton_Click(sender, e);
        }

        private void BooleanEditor_Load(object sender, EventArgs e)
        {
            comboBox_recordLevel.Items.AddRange(Enum.GetNames(typeof(RecordLevel)));
            comboBox_recordLevel.Text = Variable.ReportRecordLevel.ToString();
        }
    }
}
