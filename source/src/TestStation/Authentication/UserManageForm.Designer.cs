namespace TestStation.Authentication
{
    partial class UserManageForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.splitContainer_main = new System.Windows.Forms.SplitContainer();
            this.splitContainer_user = new System.Windows.Forms.SplitContainer();
            this.label_userGroups = new System.Windows.Forms.Label();
            this.listBox_userGroup = new System.Windows.Forms.ListBox();
            this.dataGridView_users = new System.Windows.Forms.DataGridView();
            this.Column_picture = new System.Windows.Forms.DataGridViewImageColumn();
            this.Column_userName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label_users = new System.Windows.Forms.Label();
            this.button_resetPassword = new System.Windows.Forms.Button();
            this.splitContainer_userDetail = new System.Windows.Forms.SplitContainer();
            this.dataGridView_authority = new System.Windows.Forms.DataGridView();
            this.dataGridViewImageColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridView_loginInfo = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label_userGroup = new System.Windows.Forms.Label();
            this.label_userGroupTitle = new System.Windows.Forms.Label();
            this.button_fixPasswd = new System.Windows.Forms.Button();
            this.label_userName = new System.Windows.Forms.Label();
            this.label_userNameTitle = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.ToolStripMenuItem_login = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_switchToOtherSession = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_logoutAndExit = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_userManage = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_createUser = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_createAdmin = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_createConfigurator = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_createOperator = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_createDebugger = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_deleteCurrentUser = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_main)).BeginInit();
            this.splitContainer_main.Panel1.SuspendLayout();
            this.splitContainer_main.Panel2.SuspendLayout();
            this.splitContainer_main.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_user)).BeginInit();
            this.splitContainer_user.Panel1.SuspendLayout();
            this.splitContainer_user.Panel2.SuspendLayout();
            this.splitContainer_user.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_users)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_userDetail)).BeginInit();
            this.splitContainer_userDetail.Panel1.SuspendLayout();
            this.splitContainer_userDetail.Panel2.SuspendLayout();
            this.splitContainer_userDetail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_authority)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_loginInfo)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer_main
            // 
            this.splitContainer_main.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer_main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_main.Location = new System.Drawing.Point(0, 0);
            this.splitContainer_main.Name = "splitContainer_main";
            // 
            // splitContainer_main.Panel1
            // 
            this.splitContainer_main.Panel1.Controls.Add(this.splitContainer_user);
            // 
            // splitContainer_main.Panel2
            // 
            this.splitContainer_main.Panel2.Controls.Add(this.button_resetPassword);
            this.splitContainer_main.Panel2.Controls.Add(this.splitContainer_userDetail);
            this.splitContainer_main.Panel2.Controls.Add(this.label_userGroup);
            this.splitContainer_main.Panel2.Controls.Add(this.label_userGroupTitle);
            this.splitContainer_main.Panel2.Controls.Add(this.button_fixPasswd);
            this.splitContainer_main.Panel2.Controls.Add(this.label_userName);
            this.splitContainer_main.Panel2.Controls.Add(this.label_userNameTitle);
            this.splitContainer_main.Panel2.Controls.Add(this.menuStrip1);
            this.splitContainer_main.Size = new System.Drawing.Size(831, 541);
            this.splitContainer_main.SplitterDistance = 277;
            this.splitContainer_main.TabIndex = 3;
            // 
            // splitContainer_user
            // 
            this.splitContainer_user.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer_user.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_user.Location = new System.Drawing.Point(0, 0);
            this.splitContainer_user.Name = "splitContainer_user";
            this.splitContainer_user.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer_user.Panel1
            // 
            this.splitContainer_user.Panel1.Controls.Add(this.label_userGroups);
            this.splitContainer_user.Panel1.Controls.Add(this.listBox_userGroup);
            // 
            // splitContainer_user.Panel2
            // 
            this.splitContainer_user.Panel2.Controls.Add(this.dataGridView_users);
            this.splitContainer_user.Panel2.Controls.Add(this.label_users);
            this.splitContainer_user.Size = new System.Drawing.Size(277, 541);
            this.splitContainer_user.SplitterDistance = 178;
            this.splitContainer_user.TabIndex = 0;
            // 
            // label_userGroups
            // 
            this.label_userGroups.AutoSize = true;
            this.label_userGroups.Location = new System.Drawing.Point(5, 9);
            this.label_userGroups.Name = "label_userGroups";
            this.label_userGroups.Size = new System.Drawing.Size(56, 17);
            this.label_userGroups.TabIndex = 2;
            this.label_userGroups.Text = "用户组：";
            // 
            // listBox_userGroup
            // 
            this.listBox_userGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox_userGroup.BackColor = System.Drawing.SystemColors.Control;
            this.listBox_userGroup.ItemHeight = 17;
            this.listBox_userGroup.Location = new System.Drawing.Point(2, 33);
            this.listBox_userGroup.Name = "listBox_userGroup";
            this.listBox_userGroup.Size = new System.Drawing.Size(269, 140);
            this.listBox_userGroup.TabIndex = 1;
            this.listBox_userGroup.SelectedIndexChanged += new System.EventHandler(this.listBox_userGroup_SelectedIndexChanged);
            // 
            // dataGridView_users
            // 
            this.dataGridView_users.AllowUserToAddRows = false;
            this.dataGridView_users.AllowUserToDeleteRows = false;
            this.dataGridView_users.AllowUserToResizeColumns = false;
            this.dataGridView_users.AllowUserToResizeRows = false;
            this.dataGridView_users.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView_users.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dataGridView_users.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataGridView_users.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dataGridView_users.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_users.ColumnHeadersVisible = false;
            this.dataGridView_users.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column_picture,
            this.Column_userName});
            this.dataGridView_users.Location = new System.Drawing.Point(2, 28);
            this.dataGridView_users.MultiSelect = false;
            this.dataGridView_users.Name = "dataGridView_users";
            this.dataGridView_users.RowHeadersVisible = false;
            this.dataGridView_users.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridView_users.RowTemplate.Height = 23;
            this.dataGridView_users.Size = new System.Drawing.Size(268, 326);
            this.dataGridView_users.TabIndex = 4;
            this.dataGridView_users.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView_users_CellMouseDown);
            this.dataGridView_users.CurrentCellChanged += new System.EventHandler(this.dataGridView_users_CurrentCellChanged);
            // 
            // Column_picture
            // 
            this.Column_picture.Frozen = true;
            this.Column_picture.HeaderText = "用户头像";
            this.Column_picture.Image = global::TestStation.Authentication.Properties.Resources.管理员;
            this.Column_picture.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Stretch;
            this.Column_picture.Name = "Column_picture";
            this.Column_picture.ReadOnly = true;
            this.Column_picture.Width = 30;
            // 
            // Column_userName
            // 
            this.Column_userName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column_userName.HeaderText = "用户名";
            this.Column_userName.Name = "Column_userName";
            this.Column_userName.ReadOnly = true;
            // 
            // label_users
            // 
            this.label_users.AutoSize = true;
            this.label_users.Location = new System.Drawing.Point(5, 6);
            this.label_users.Name = "label_users";
            this.label_users.Size = new System.Drawing.Size(56, 17);
            this.label_users.TabIndex = 3;
            this.label_users.Text = "用户列表";
            // 
            // button_resetPassword
            // 
            this.button_resetPassword.Location = new System.Drawing.Point(379, 34);
            this.button_resetPassword.Name = "button_resetPassword";
            this.button_resetPassword.Size = new System.Drawing.Size(75, 23);
            this.button_resetPassword.TabIndex = 9;
            this.button_resetPassword.Text = "重置密码";
            this.button_resetPassword.UseVisualStyleBackColor = true;
            this.button_resetPassword.Click += new System.EventHandler(this.button_resetPassword_Click);
            // 
            // splitContainer_userDetail
            // 
            this.splitContainer_userDetail.Location = new System.Drawing.Point(3, 107);
            this.splitContainer_userDetail.Name = "splitContainer_userDetail";
            this.splitContainer_userDetail.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer_userDetail.Panel1
            // 
            this.splitContainer_userDetail.Panel1.Controls.Add(this.dataGridView_authority);
            // 
            // splitContainer_userDetail.Panel2
            // 
            this.splitContainer_userDetail.Panel2.Controls.Add(this.dataGridView_loginInfo);
            this.splitContainer_userDetail.Size = new System.Drawing.Size(540, 427);
            this.splitContainer_userDetail.SplitterDistance = 213;
            this.splitContainer_userDetail.TabIndex = 8;
            // 
            // dataGridView_authority
            // 
            this.dataGridView_authority.AllowUserToAddRows = false;
            this.dataGridView_authority.AllowUserToDeleteRows = false;
            this.dataGridView_authority.AllowUserToResizeColumns = false;
            this.dataGridView_authority.AllowUserToResizeRows = false;
            this.dataGridView_authority.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dataGridView_authority.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataGridView_authority.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView_authority.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView_authority.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_authority.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewImageColumn1,
            this.dataGridViewTextBoxColumn1});
            this.dataGridView_authority.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_authority.Location = new System.Drawing.Point(0, 0);
            this.dataGridView_authority.MultiSelect = false;
            this.dataGridView_authority.Name = "dataGridView_authority";
            this.dataGridView_authority.RowHeadersVisible = false;
            this.dataGridView_authority.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridView_authority.RowTemplate.Height = 23;
            this.dataGridView_authority.Size = new System.Drawing.Size(540, 213);
            this.dataGridView_authority.TabIndex = 6;
            // 
            // dataGridViewImageColumn1
            // 
            this.dataGridViewImageColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewImageColumn1.Frozen = true;
            this.dataGridViewImageColumn1.HeaderText = "权限名称";
            this.dataGridViewImageColumn1.Name = "dataGridViewImageColumn1";
            this.dataGridViewImageColumn1.ReadOnly = true;
            this.dataGridViewImageColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewImageColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewImageColumn1.Width = 438;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn1.Frozen = true;
            this.dataGridViewTextBoxColumn1.HeaderText = "是否可用";
            this.dataGridViewTextBoxColumn1.MinimumWidth = 100;
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // dataGridView_loginInfo
            // 
            this.dataGridView_loginInfo.AllowUserToAddRows = false;
            this.dataGridView_loginInfo.AllowUserToDeleteRows = false;
            this.dataGridView_loginInfo.AllowUserToResizeColumns = false;
            this.dataGridView_loginInfo.AllowUserToResizeRows = false;
            this.dataGridView_loginInfo.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dataGridView_loginInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataGridView_loginInfo.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView_loginInfo.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView_loginInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_loginInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewCheckBoxColumn1});
            this.dataGridView_loginInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_loginInfo.Location = new System.Drawing.Point(0, 0);
            this.dataGridView_loginInfo.MultiSelect = false;
            this.dataGridView_loginInfo.Name = "dataGridView_loginInfo";
            this.dataGridView_loginInfo.RowHeadersVisible = false;
            this.dataGridView_loginInfo.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridView_loginInfo.RowTemplate.Height = 23;
            this.dataGridView_loginInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridView_loginInfo.Size = new System.Drawing.Size(540, 210);
            this.dataGridView_loginInfo.TabIndex = 7;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.FillWeight = 50F;
            this.dataGridViewTextBoxColumn2.HeaderText = "登入时间";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewCheckBoxColumn1
            // 
            this.dataGridViewCheckBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewCheckBoxColumn1.FillWeight = 50F;
            this.dataGridViewCheckBoxColumn1.HeaderText = "登出时间";
            this.dataGridViewCheckBoxColumn1.MinimumWidth = 50;
            this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
            this.dataGridViewCheckBoxColumn1.ReadOnly = true;
            this.dataGridViewCheckBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // label_userGroup
            // 
            this.label_userGroup.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label_userGroup.Location = new System.Drawing.Point(95, 75);
            this.label_userGroup.Name = "label_userGroup";
            this.label_userGroup.Size = new System.Drawing.Size(153, 22);
            this.label_userGroup.TabIndex = 5;
            this.label_userGroup.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label_userGroupTitle
            // 
            this.label_userGroupTitle.AutoSize = true;
            this.label_userGroupTitle.Location = new System.Drawing.Point(33, 78);
            this.label_userGroupTitle.Name = "label_userGroupTitle";
            this.label_userGroupTitle.Size = new System.Drawing.Size(56, 17);
            this.label_userGroupTitle.TabIndex = 4;
            this.label_userGroupTitle.Text = "用户组：";
            // 
            // button_fixPasswd
            // 
            this.button_fixPasswd.Location = new System.Drawing.Point(277, 34);
            this.button_fixPasswd.Name = "button_fixPasswd";
            this.button_fixPasswd.Size = new System.Drawing.Size(75, 23);
            this.button_fixPasswd.TabIndex = 2;
            this.button_fixPasswd.Text = "修改密码";
            this.button_fixPasswd.UseVisualStyleBackColor = true;
            this.button_fixPasswd.Click += new System.EventHandler(this.button_fixPasswd_Click);
            // 
            // label_userName
            // 
            this.label_userName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label_userName.Location = new System.Drawing.Point(95, 35);
            this.label_userName.Name = "label_userName";
            this.label_userName.Size = new System.Drawing.Size(153, 22);
            this.label_userName.TabIndex = 1;
            this.label_userName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label_userNameTitle
            // 
            this.label_userNameTitle.AutoSize = true;
            this.label_userNameTitle.Location = new System.Drawing.Point(33, 38);
            this.label_userNameTitle.Name = "label_userNameTitle";
            this.label_userNameTitle.Size = new System.Drawing.Size(56, 17);
            this.label_userNameTitle.TabIndex = 0;
            this.label_userNameTitle.Text = "用户名：";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItem_login,
            this.ToolStripMenuItem_userManage});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(546, 25);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // ToolStripMenuItem_login
            // 
            this.ToolStripMenuItem_login.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItem_switchToOtherSession,
            this.ToolStripMenuItem_logoutAndExit});
            this.ToolStripMenuItem_login.Name = "ToolStripMenuItem_login";
            this.ToolStripMenuItem_login.Size = new System.Drawing.Size(44, 21);
            this.ToolStripMenuItem_login.Text = "登录";
            // 
            // ToolStripMenuItem_switchToOtherSession
            // 
            this.ToolStripMenuItem_switchToOtherSession.Name = "ToolStripMenuItem_switchToOtherSession";
            this.ToolStripMenuItem_switchToOtherSession.Size = new System.Drawing.Size(148, 22);
            this.ToolStripMenuItem_switchToOtherSession.Text = "切换其他用户";
            this.ToolStripMenuItem_switchToOtherSession.Click += new System.EventHandler(this.ToolStripMenuItem_switchToOtherSession_Click);
            // 
            // ToolStripMenuItem_logoutAndExit
            // 
            this.ToolStripMenuItem_logoutAndExit.Name = "ToolStripMenuItem_logoutAndExit";
            this.ToolStripMenuItem_logoutAndExit.Size = new System.Drawing.Size(148, 22);
            this.ToolStripMenuItem_logoutAndExit.Text = "注销并退出";
            this.ToolStripMenuItem_logoutAndExit.Click += new System.EventHandler(this.ToolStripMenuItem_logoutAndExit_Click);
            // 
            // ToolStripMenuItem_userManage
            // 
            this.ToolStripMenuItem_userManage.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItem_createUser,
            this.ToolStripMenuItem_deleteCurrentUser});
            this.ToolStripMenuItem_userManage.Name = "ToolStripMenuItem_userManage";
            this.ToolStripMenuItem_userManage.Size = new System.Drawing.Size(68, 21);
            this.ToolStripMenuItem_userManage.Text = "用户管理";
            // 
            // ToolStripMenuItem_createUser
            // 
            this.ToolStripMenuItem_createUser.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItem_createAdmin,
            this.ToolStripMenuItem_createConfigurator,
            this.ToolStripMenuItem_createOperator,
            this.ToolStripMenuItem_createDebugger});
            this.ToolStripMenuItem_createUser.Name = "ToolStripMenuItem_createUser";
            this.ToolStripMenuItem_createUser.Size = new System.Drawing.Size(152, 22);
            this.ToolStripMenuItem_createUser.Text = "创建用户";
            // 
            // ToolStripMenuItem_createAdmin
            // 
            this.ToolStripMenuItem_createAdmin.Name = "ToolStripMenuItem_createAdmin";
            this.ToolStripMenuItem_createAdmin.Size = new System.Drawing.Size(136, 22);
            this.ToolStripMenuItem_createAdmin.Text = "创建管理员";
            this.ToolStripMenuItem_createAdmin.Visible = false;
            this.ToolStripMenuItem_createAdmin.Click += new System.EventHandler(this.ToolStripMenuItem_createAdmin_Click);
            // 
            // ToolStripMenuItem_createConfigurator
            // 
            this.ToolStripMenuItem_createConfigurator.Name = "ToolStripMenuItem_createConfigurator";
            this.ToolStripMenuItem_createConfigurator.Size = new System.Drawing.Size(136, 22);
            this.ToolStripMenuItem_createConfigurator.Text = "创建配置员";
            this.ToolStripMenuItem_createConfigurator.Click += new System.EventHandler(this.ToolStripMenuItem_createConfigurator_Click);
            // 
            // ToolStripMenuItem_createOperator
            // 
            this.ToolStripMenuItem_createOperator.Name = "ToolStripMenuItem_createOperator";
            this.ToolStripMenuItem_createOperator.Size = new System.Drawing.Size(136, 22);
            this.ToolStripMenuItem_createOperator.Text = "创建操作员";
            this.ToolStripMenuItem_createOperator.Click += new System.EventHandler(this.ToolStripMenuItem_createOperator_Click);
            // 
            // ToolStripMenuItem_createDebugger
            // 
            this.ToolStripMenuItem_createDebugger.Name = "ToolStripMenuItem_createDebugger";
            this.ToolStripMenuItem_createDebugger.Size = new System.Drawing.Size(136, 22);
            this.ToolStripMenuItem_createDebugger.Text = "创建调试员";
            this.ToolStripMenuItem_createDebugger.Click += new System.EventHandler(this.ToolStripMenuItem_createDebugger_Click);
            // 
            // ToolStripMenuItem_deleteCurrentUser
            // 
            this.ToolStripMenuItem_deleteCurrentUser.Name = "ToolStripMenuItem_deleteCurrentUser";
            this.ToolStripMenuItem_deleteCurrentUser.Size = new System.Drawing.Size(152, 22);
            this.ToolStripMenuItem_deleteCurrentUser.Text = "删除当前用户";
            this.ToolStripMenuItem_deleteCurrentUser.Click += new System.EventHandler(this.ToolStripMenuItem_deleteCurrentUser_Click);
            // 
            // UserManageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(831, 541);
            this.Controls.Add(this.splitContainer_main);
            this.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "UserManageForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "用户管理";
            this.Load += new System.EventHandler(this.UserManageForm_Load);
            this.splitContainer_main.Panel1.ResumeLayout(false);
            this.splitContainer_main.Panel2.ResumeLayout(false);
            this.splitContainer_main.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_main)).EndInit();
            this.splitContainer_main.ResumeLayout(false);
            this.splitContainer_user.Panel1.ResumeLayout(false);
            this.splitContainer_user.Panel1.PerformLayout();
            this.splitContainer_user.Panel2.ResumeLayout(false);
            this.splitContainer_user.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_user)).EndInit();
            this.splitContainer_user.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_users)).EndInit();
            this.splitContainer_userDetail.Panel1.ResumeLayout(false);
            this.splitContainer_userDetail.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_userDetail)).EndInit();
            this.splitContainer_userDetail.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_authority)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_loginInfo)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer_main;
        private System.Windows.Forms.SplitContainer splitContainer_user;
        private System.Windows.Forms.Label label_userGroups;
        private System.Windows.Forms.ListBox listBox_userGroup;
        private System.Windows.Forms.Label label_users;
        private System.Windows.Forms.DataGridView dataGridView_users;
        private System.Windows.Forms.Label label_userNameTitle;
        private System.Windows.Forms.Label label_userName;
        private System.Windows.Forms.Button button_fixPasswd;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_userManage;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_createUser;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_deleteCurrentUser;
        private System.Windows.Forms.Label label_userGroup;
        private System.Windows.Forms.Label label_userGroupTitle;
        private System.Windows.Forms.DataGridView dataGridView_authority;
        private System.Windows.Forms.SplitContainer splitContainer_userDetail;
        private System.Windows.Forms.DataGridView dataGridView_loginInfo;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewImageColumn1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewImageColumn Column_picture;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_userName;
        private System.Windows.Forms.Button button_resetPassword;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_createAdmin;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_createOperator;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_login;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_switchToOtherSession;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_logoutAndExit;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_createConfigurator;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_createDebugger;
    }
}