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

namespace TestStation
{
    public partial class RenameForm : Form
    {
        private ISequenceFlowContainer _target;
        public string Name { get; private set; }
        public RenameForm(ISequenceFlowContainer target)
        {
            InitializeComponent();
            textBox_name.Text = target.Name;
        }

        private void button_confirm_Click(object sender, EventArgs e)
        {
            this.Name = textBox_name.Text;
            this.Close();
        }

        private void button_cancell_Click(object sender, EventArgs e)
        {
            this.Name = _target.Name;
            this.Close();
        }
    }
}
