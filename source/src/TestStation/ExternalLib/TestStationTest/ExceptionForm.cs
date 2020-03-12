using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Testflow.Usr;

namespace DemoTest
{
    internal partial class ExceptionForm : Form
    {
        public ExceptionForm()
        {
            InitializeComponent();
        }

        private void button_no_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button_yes_Click(object sender, EventArgs e)
        {
            throw new TestflowAssertException("这是一个用于测试的异常。");
//            throw new ApplicationException("这是一个用于测试的异常。");
        }
    }
}
