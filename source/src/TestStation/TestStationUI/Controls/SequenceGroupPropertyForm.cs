using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Testflow.Data;
using Testflow.Data.Sequence;
using Testflow.Runtime;

namespace TestFlow.DevSoftware.Controls
{
    public partial class SequenceGroupPropertyForm : Form
    {
        private ISequenceGroup _sequenceGroup;

        public SequenceGroupPropertyForm(ISequenceGroup sequenceGroup)
        {
            InitializeComponent();
            this._sequenceGroup = sequenceGroup;
        }

        private void button_confirm_Click(object sender, EventArgs e)
        {
            this._sequenceGroup.ExecutionModel = (ExecutionModel) Enum.Parse(typeof (ExecutionModel), comboBox_execution.Text);
            this._sequenceGroup.Info.Platform = (RunnerPlatform) Enum.Parse(typeof (RunnerPlatform), comboBox_platform.Text);
            this.Close();
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SequencePropertyForm_Load(object sender, EventArgs e)
        {
            label_name.Text = _sequenceGroup.Name;
            label_createTime.Text = _sequenceGroup.Info.CreationTime.ToString("yyyy-MM-dd HH:mm:ss");
            label_modifiedTime.Text = _sequenceGroup.Info.ModifiedTime.ToString("yyyy-MM-dd HH:mm:ss");
            label_version.Text = _sequenceGroup.Info.Version;
            comboBox_execution.Items.AddRange(Enum.GetNames(typeof(ExecutionModel)));
            comboBox_execution.Text = _sequenceGroup.ExecutionModel.ToString();
            comboBox_platform.Items.AddRange(Enum.GetNames(typeof(RunnerPlatform)));
            comboBox_platform.Text = _sequenceGroup.Info.Platform.ToString();
        }
    }
}
