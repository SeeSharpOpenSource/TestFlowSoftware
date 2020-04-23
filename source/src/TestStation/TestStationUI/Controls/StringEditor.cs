using System;
using Testflow.Data;
using Testflow.Data.Sequence;

namespace TestFlow.DevSoftware.Controls
{
    public partial class StringEditor : ValueEditor
    {
        public StringEditor() : base(null)
        {
            InitializeComponent();
        }

        public StringEditor(IVariable variable) : base(variable)
        {
            InitializeComponent();
            ValuetextBox.Text = variable.Value;
        }

        protected override void OkButton_Click(object sender, EventArgs e)
        {
            string newValue = ValuetextBox.Text;
            if (!newValue.Equals(base._value))
            {
                base._value = ValuetextBox.Text;
                base._valueChanged = true;
                Variable.Value = newValue;
            }
            Variable.ReportRecordLevel = (RecordLevel)Enum.Parse(typeof(RecordLevel), comboBox_recordLevel.Text);
            base.OkButton_Click(sender, e);
        }

        private void StringEditor_Load(object sender, EventArgs e)
        {
            comboBox_recordLevel.Items.AddRange(Enum.GetNames(typeof(RecordLevel)));
            comboBox_recordLevel.Text = Variable.ReportRecordLevel.ToString();
        }
    }
}
 