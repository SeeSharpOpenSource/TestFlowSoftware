using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestStation.Authentication
{
    public partial class ScanLoginForm : Form
    {
        public ScanLoginForm(AuthenticationSession session)
        {
            InitializeComponent();
            this.Session = session;
        }

        public AuthenticationSession Session { get; private set; }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            this.textBox_loginName.Focus();
        }

        private void LoginForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string userName = textBox_loginName.Text;
                try
                {
                    AuthenticationSession session = AuthenticationManage.DataAccessor.ScanLoginIn(userName);
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
        }

        private void button_userPasswordLogin_Click(object sender, EventArgs e)
        {
            LoginForm loginForm = new LoginForm(Session);
            loginForm.ShowDialog(this);
            this.Session = loginForm.Session;
            this.Close();
        }
    }
}
