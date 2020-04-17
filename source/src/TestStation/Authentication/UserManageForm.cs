using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestFlow.DevSoftware.Authentication.Data;
using TestStation.Authentication.Data;

namespace TestFlow.DevSoftware.Authentication
{
    public partial class UserManageForm : Form
    {
        private AuthenticationSession _session;
        private IList<UserInfo> _userInfos;
        private List<UserGroupInfo> _userGroupInfos;
        private UserInfo _currentUser;
        const int IconCol = 0;
        const int UserNameCol = 1;

        public AuthenticationSession Session => _session;

        public UserManageForm(AuthenticationSession session)
        {
            InitializeComponent();
            this._session = session;
        }

        private void dataGridView_users_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == IconCol)
            {
                dataGridView_users.CurrentCell = dataGridView_users.Rows[e.RowIndex].Cells[UserNameCol];
            }
        }

        private void UserManageForm_Load(object sender, EventArgs e)
        {
            RefreshUserDatas();
            ToolStripMenuItem_createUser.Visible = Session.HasAuthority(AuthorityDefinition.AddUser);
            ToolStripMenuItem_deleteCurrentUser.Visible = Session.HasAuthority(AuthorityDefinition.DeleteUser);
        }

        private void RefreshUserDatas()
        {
            DataBaseProxy dataAccessor = AuthenticationManage.DataAccessor;
            _userInfos = dataAccessor.GetUserInfos(_session);
            HashSet<string> groups = new HashSet<string>();
            foreach (UserInfo userInfo in _userInfos)
            {
                groups.Add(userInfo.UserGroup.ToString());
            }
            _userGroupInfos = new List<UserGroupInfo>(groups.Count);
            listBox_userGroup.Items.Clear();
            foreach (string groupName in groups)
            {
                UserGroupInfo userGroupInfo = dataAccessor.GetUserGroupInfo(groupName);
                _userGroupInfos.Add(userGroupInfo);
                listBox_userGroup.Items.Add(userGroupInfo.Description);
            }
            listBox_userGroup.SelectedIndex = 0;
        }

        private void dataGridView_users_CurrentCellChanged(object sender, EventArgs e)
        {
            if (null == dataGridView_users.CurrentCell)
            {
                return;
            }
            int rowIndex = dataGridView_users.CurrentCell.RowIndex;
            string userName = dataGridView_users.Rows[rowIndex].Cells[1].Value.ToString();
            FillUserInfoPage(userName);
        }

        private void listBox_userGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            UserGroupInfo selectedGroup = _userGroupInfos[listBox_userGroup.SelectedIndex];
            IEnumerable<string> userNames = from userInfo in _userInfos
                where userInfo.UserGroup.ToString().Equals(selectedGroup.GroupName)
                select userInfo.Name;
            dataGridView_users.Rows.Clear();
            Image image = selectedGroup.GroupName.Equals(UserGroup.Administrator.ToString()) || 
                selectedGroup.GroupName.Equals(UserGroup.SuperAdmin.ToString()) ? 
                Properties.Resources.管理员 : Properties.Resources.用户;
            foreach (string userName in userNames)
            {
                dataGridView_users.Rows.Add(image, userName);
            }
        }

        private void FillUserInfoPage(string userName)
        {
            bool isSelf = userName.Equals(_session.UserName);
            _currentUser = _userInfos.First(item => item.Name.Equals(userName));
            UserGroupInfo userGroupInfo =
                _userGroupInfos.First(item => item.GroupName.Equals(_currentUser.UserGroup.ToString()));
            label_userGroup.Text = userGroupInfo.Description;
            label_userName.Text = _currentUser.Name;

            button_fixPasswd.Enabled = isSelf;
            button_resetPassword.Enabled = _session.Authorities.Contains(AuthorityDefinition.ResetPassword) && 
                _session.UserGroup < _currentUser.UserGroup;

            DataBaseProxy dataAccessor = AuthenticationManage.DataAccessor;
            IList<string> authorities = dataAccessor.GetAuthorities(_currentUser.UserGroup);
            dataGridView_authority.Rows.Clear();
            foreach (string authority in authorities)
            {
                dataGridView_authority.Rows.Add(authority, true);
            }

            IList<LoginInfo> loginInfos = dataAccessor.GetLoginInfos(userName);
            dataGridView_loginInfo.Rows.Clear();
            foreach (LoginInfo loginInfo in loginInfos)
            {
                dataGridView_loginInfo.Rows.Add(loginInfo.LoginTime, loginInfo.LogoutTime);
            }
        }

        private void button_fixPasswd_Click(object sender, EventArgs e)
        {
            FixPasswordForm passwordForm = new FixPasswordForm(_session);
            passwordForm.ShowDialog(this);
        }
        
        private void button_resetPassword_Click(object sender, EventArgs e)
        {
            if (null == _currentUser)
            {
                return;
            }
            try
            {
                AuthenticationManage.DataAccessor.ResetUserPassword(_session, _currentUser.Name);
                MessageBox.Show("密码重置成功。", "重置密码", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (AuthenticationException ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ToolStripMenuItem_createAdmin_Click(object sender, EventArgs e)
        {
            NewUserForm newUserForm = new NewUserForm(_session, UserGroup.Administrator);
            newUserForm.ShowDialog(this);
            RefreshUserDatas();
        }

        private void ToolStripMenuItem_createConfigurator_Click(object sender, EventArgs e)
        {
            NewUserForm newUserForm = new NewUserForm(_session, UserGroup.Configurator);
            newUserForm.ShowDialog(this);
            RefreshUserDatas();
        }

        private void ToolStripMenuItem_createDebugger_Click(object sender, EventArgs e)
        {
            NewUserForm newUserForm = new NewUserForm(_session, UserGroup.Adjustor);
            newUserForm.ShowDialog(this);
            RefreshUserDatas();
        }

        private void ToolStripMenuItem_createOperator_Click(object sender, EventArgs e)
        {
            NewUserForm newUserForm = new NewUserForm(_session, UserGroup.Operator);
            newUserForm.ShowDialog(this);
            RefreshUserDatas();
        }

        private void ToolStripMenuItem_deleteCurrentUser_Click(object sender, EventArgs e)
        {
            if (null == _currentUser)
            {
                return;
            }
            DialogResult dialogResult = MessageBox.Show("确定要删除当前账户？", "删除账户", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            if (dialogResult != DialogResult.Yes)
            {
                return;
            }
            try
            {
                AuthenticationManage.DataAccessor.DeleteUser(_session, _currentUser);
                MessageBox.Show("删除账户成功", "删除账户", MessageBoxButtons.OK, MessageBoxIcon.Information);
                RefreshUserDatas();
            }
            catch (AuthenticationException ex)
            {
                MessageBox.Show(ex.Message, "删除账户", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ToolStripMenuItem_logoutAndExit_Click(object sender, EventArgs e)
        {
            try
            {
                AuthenticationManage.DataAccessor.LogOut(_session);
            }
            catch (AuthenticationException ex)
            {
                MessageBox.Show("登出失败。", "登录", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            this._session = null;
            this.Close();
        }

        private void ToolStripMenuItem_switchToOtherSession_Click(object sender, EventArgs e)
        {
            try
            {
                AuthenticationManage.DataAccessor.LogOut(_session);
                ScanLoginForm loginForm = new ScanLoginForm(null);
                loginForm.ShowDialog(this);
                _session = loginForm.Session;
                RefreshUserDatas();
            }
            catch (AuthenticationException ex)
            {
                MessageBox.Show("切换用户失败。", "登录", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
