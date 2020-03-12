using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestStation.OperationPanel
{
    public partial class TargetSetForm : Form
    {
        private readonly OperationPanelForm _parent;
        public TargetSetForm(OperationPanelForm parentForm, long targetNumber)
        {
            InitializeComponent();
            this._parent = parentForm;
            numericUpDown_target.Value = targetNumber;
            this.IsCancelled = true;
        }

        public bool IsCancelled { get; private set; }

        public long TargetNumber { get; private set; }

        private void button_confirm_Click(object sender, EventArgs e)
        {
            this.TargetNumber = (long) numericUpDown_target.Value;
            this.IsCancelled = false;
            _parent.SetTarget(TargetNumber);
            this.Close();
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.IsCancelled = true;
            this.Close();
        }
    }
}
