using System;
using System.Windows.Forms;

namespace TestStationLimit
{
    internal partial class TextInputForm : Form
    {
        public TextInputForm()
        {
            InitializeComponent();
            this.Continue = false;
            this.TopMost = true;
            textBox_barCode.Text = string.Empty;
        }

        public string BarCode { get; private set; }
        public bool Continue { get; private set; }

        private void button_confirm_Click(object sender, EventArgs e)
        {
            if (null == textBox_barCode.Text)
            {
                MessageBox.Show("Invalid Serial Number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            this.BarCode = textBox_barCode.Text;
            this.Continue = true;
            this.Close();
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.Continue = false;
            this.Close();
        }

        private void TextInputForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button_confirm_Click(null, null);
            }
        }

        private void TextInputForm_Shown(object sender, EventArgs e)
        {
            textBox_barCode.Focus();
        }

        private void textBox_barCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button_confirm_Click(null, null);
            }
        }
    }
}
