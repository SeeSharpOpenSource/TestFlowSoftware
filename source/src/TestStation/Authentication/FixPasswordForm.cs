using System;
using System.Windows.Forms;

namespace TestFlow.DevSoftware.Authentication
{
    public partial class FixPasswordForm : Form
    {
        private AuthenticationSession _session;

        public FixPasswordForm(AuthenticationSession session)
        {
            InitializeComponent();
            _session = session;
        }

        public string Password { get; private set; }

        private void button_confirm_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox_inputPasswd.Text) || textBox_inputPasswd.Text.Contains(" "))
            {
                MessageBox.Show("输入的密码无效或包含非法字符。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!textBox_inputPasswd.Text.Equals(textBox_confirmPasswd.Text))
            {
                MessageBox.Show("两次输入的密码不一致。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                AuthenticationManage.DataAccessor.FixPassword(_session, textBox_oldPassword.Text,
                    textBox_inputPasswd.Text);
                MessageBox.Show("密码修改成功。", "修改密码", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (AuthenticationException ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.Password = string.Empty;
            this.Close();
        }

        private void button_clear_Click(object sender, EventArgs e)
        {
            textBox_confirmPasswd.Text = string.Empty;
            textBox_inputPasswd.Text = string.Empty;
        }

        private void FixPasswordForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button_confirm_Click(null, null);
            }
        }

        private void textBox_inputPasswd_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                textBox_confirmPasswd.Focus();
            }
        }
    }
}
