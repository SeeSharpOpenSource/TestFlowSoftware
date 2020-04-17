namespace TestFlow.DevSoftware.Authentication
{
    partial class LoginForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            this.label_name = new System.Windows.Forms.Label();
            this.label_passwd = new System.Windows.Forms.Label();
            this.textBox_passwd = new System.Windows.Forms.TextBox();
            this.button_login = new System.Windows.Forms.Button();
            this.button_cancel = new System.Windows.Forms.Button();
            this.comboBox_names = new System.Windows.Forms.ComboBox();
            this.button_clearList = new System.Windows.Forms.Button();
            this.button_resetPassword = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label_name
            // 
            this.label_name.AutoSize = true;
            this.label_name.Location = new System.Drawing.Point(48, 51);
            this.label_name.Name = "label_name";
            this.label_name.Size = new System.Drawing.Size(56, 17);
            this.label_name.TabIndex = 100;
            this.label_name.Text = "用户名：";
            // 
            // label_passwd
            // 
            this.label_passwd.AutoSize = true;
            this.label_passwd.Location = new System.Drawing.Point(48, 91);
            this.label_passwd.Name = "label_passwd";
            this.label_passwd.Size = new System.Drawing.Size(56, 17);
            this.label_passwd.TabIndex = 100;
            this.label_passwd.Text = "密   码：";
            // 
            // textBox_passwd
            // 
            this.textBox_passwd.Location = new System.Drawing.Point(112, 88);
            this.textBox_passwd.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBox_passwd.MaxLength = 24;
            this.textBox_passwd.Name = "textBox_passwd";
            this.textBox_passwd.PasswordChar = '*';
            this.textBox_passwd.Size = new System.Drawing.Size(161, 23);
            this.textBox_passwd.TabIndex = 2;
            this.textBox_passwd.KeyDown += new System.Windows.Forms.KeyEventHandler(this.LoginForm_KeyDown);
            // 
            // button_login
            // 
            this.button_login.Location = new System.Drawing.Point(75, 133);
            this.button_login.Name = "button_login";
            this.button_login.Size = new System.Drawing.Size(75, 23);
            this.button_login.TabIndex = 4;
            this.button_login.Text = "登录";
            this.button_login.UseVisualStyleBackColor = true;
            this.button_login.Click += new System.EventHandler(this.button_login_Click);
            // 
            // button_cancel
            // 
            this.button_cancel.Location = new System.Drawing.Point(196, 133);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(75, 23);
            this.button_cancel.TabIndex = 5;
            this.button_cancel.Text = "取消";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // comboBox_names
            // 
            this.comboBox_names.FormattingEnabled = true;
            this.comboBox_names.Location = new System.Drawing.Point(111, 47);
            this.comboBox_names.MaxLength = 24;
            this.comboBox_names.Name = "comboBox_names";
            this.comboBox_names.Size = new System.Drawing.Size(162, 25);
            this.comboBox_names.TabIndex = 0;
            this.comboBox_names.KeyDown += new System.Windows.Forms.KeyEventHandler(this.LoginForm_KeyDown);
            // 
            // button_clearList
            // 
            this.button_clearList.Location = new System.Drawing.Point(280, 48);
            this.button_clearList.Name = "button_clearList";
            this.button_clearList.Size = new System.Drawing.Size(51, 23);
            this.button_clearList.TabIndex = 1;
            this.button_clearList.Text = "清空";
            this.button_clearList.UseVisualStyleBackColor = true;
            this.button_clearList.Click += new System.EventHandler(this.button_clearList_Click);
            // 
            // button_resetPassword
            // 
            this.button_resetPassword.Location = new System.Drawing.Point(279, 88);
            this.button_resetPassword.Name = "button_resetPassword";
            this.button_resetPassword.Size = new System.Drawing.Size(51, 23);
            this.button_resetPassword.TabIndex = 3;
            this.button_resetPassword.Text = "重置";
            this.button_resetPassword.UseVisualStyleBackColor = true;
            this.button_resetPassword.Click += new System.EventHandler(this.button_resetPassword_Click);
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(351, 172);
            this.Controls.Add(this.button_resetPassword);
            this.Controls.Add(this.button_clearList);
            this.Controls.Add(this.comboBox_names);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_login);
            this.Controls.Add(this.label_passwd);
            this.Controls.Add(this.textBox_passwd);
            this.Controls.Add(this.label_name);
            this.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "用户登录";
            this.Load += new System.EventHandler(this.LoginForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.LoginForm_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label_name;
        private System.Windows.Forms.Label label_passwd;
        private System.Windows.Forms.TextBox textBox_passwd;
        private System.Windows.Forms.Button button_login;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.ComboBox comboBox_names;
        private System.Windows.Forms.Button button_clearList;
        private System.Windows.Forms.Button button_resetPassword;
    }
}