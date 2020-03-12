using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestStation
{
    public partial class ErrorInfoForm : Form
    {
        public ErrorInfoForm(IList<string> errorInfos)
        {
            InitializeComponent();
            this.Continue = false;
            foreach (string errorInfo in errorInfos)
            {
                textBox_errorInfo.AppendText(errorInfo);
                textBox_errorInfo.AppendText(Environment.NewLine);
            }
        }

        public bool Continue { get; private set; }

        private void ErrorInfoForm_Load(object sender, EventArgs e)
        {

        }

        private void button_continue_Click(object sender, EventArgs e)
        {
            this.Continue = true;
            this.Close();
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.Continue = false;
            this.Close();
        }
    }
}
