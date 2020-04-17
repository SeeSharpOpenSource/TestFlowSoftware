using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestFlow.DevSoftware.Authentication
{
    public partial class LoginForm : Form
    {
        public LoginForm(AuthenticationSession session)
        {
            InitializeComponent();
            this.Session = session;
        }

        public AuthenticationSession Session { get; private set; }

        private void button_login_Click(object sender, EventArgs e)
        {
            string password = textBox_passwd.Text;
            string userName = comboBox_names.Text;
            try
            {
                AuthenticationSession session = AuthenticationManage.DataAccessor.LoginIn(userName, password);
                if (Session != null)
                {
                    AuthenticationManage.DataAccessor.LogOut(Session);
                }
                this.Session = session;
                AuthenticationManage.DataAccessor.AddToLoginList(session.UserName);
                this.Close();
            }
            catch (AuthenticationException ex)
            {
                MessageBox.Show(ex.Message, "登录失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            IList<string> loginList = AuthenticationManage.DataAccessor.GetLoginList();
            comboBox_names.Items.Clear();
            comboBox_names.Items.AddRange(loginList.ToArray());
        }

        private void LoginForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button_login_Click(null, null);
            }
        }

        private void button_clearList_Click(object sender, EventArgs e)
        {
            AuthenticationManage.DataAccessor.ClearLoginList();
            comboBox_names.Items.Clear();
        }

        private void button_resetPassword_Click(object sender, EventArgs e)
        {
            textBox_passwd.Text = string.Empty;
        }
    }
}
