using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestStation.Authentication.Data;

namespace TestFlow.DevSoftware.Authentication
{
    public partial class NewUserForm : Form
    {
        private AuthenticationSession _session;
        private UserGroup _group;

        internal NewUserForm(AuthenticationSession session, UserGroup userGroup)
        {
            InitializeComponent();
            _session = session;
            _group = userGroup;
        }

        private void button_confirm_Click(object sender, EventArgs e)
        {
            if (textBox_userName.Text.Contains(" "))
            {
                MessageBox.Show("用户名中包含非法字符。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (textBox_passwd.Text.Contains(" "))
            {
                MessageBox.Show("密码中包含非法字符。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!textBox_passwd.Text.Equals(textBox_confirmPassword.Text))
            {
                MessageBox.Show("两次输入的密码不相同。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                AuthenticationManage.DataAccessor.AddUser(_session, textBox_userName.Text, _group, textBox_passwd.Text);
                MessageBox.Show("用户创建成功。", "创建用户", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (AuthenticationException ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void NewUserForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button_confirm_Click(null, null);
            }
        }
    }
}
