namespace TestStation.Authentication
{
    partial class NewUserForm
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
            this.button_cancel = new System.Windows.Forms.Button();
            this.button_confirm = new System.Windows.Forms.Button();
            this.label_passwd = new System.Windows.Forms.Label();
            this.textBox_passwd = new System.Windows.Forms.TextBox();
            this.label_name = new System.Windows.Forms.Label();
            this.textBox_userName = new System.Windows.Forms.TextBox();
            this.label_confirmPassword = new System.Windows.Forms.Label();
            this.textBox_confirmPassword = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // button_cancel
            // 
            this.button_cancel.Location = new System.Drawing.Point(183, 128);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(75, 23);
            this.button_cancel.TabIndex = 4;
            this.button_cancel.Text = "取消";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // button_confirm
            // 
            this.button_confirm.Location = new System.Drawing.Point(62, 128);
            this.button_confirm.Name = "button_confirm";
            this.button_confirm.Size = new System.Drawing.Size(75, 23);
            this.button_confirm.TabIndex = 3;
            this.button_confirm.Text = "确定";
            this.button_confirm.UseVisualStyleBackColor = true;
            this.button_confirm.Click += new System.EventHandler(this.button_confirm_Click);
            // 
            // label_passwd
            // 
            this.label_passwd.AutoSize = true;
            this.label_passwd.Location = new System.Drawing.Point(35, 59);
            this.label_passwd.Name = "label_passwd";
            this.label_passwd.Size = new System.Drawing.Size(59, 12);
            this.label_passwd.TabIndex = 107;
            this.label_passwd.Text = "密   码：";
            // 
            // textBox_passwd
            // 
            this.textBox_passwd.Location = new System.Drawing.Point(99, 56);
            this.textBox_passwd.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBox_passwd.MaxLength = 24;
            this.textBox_passwd.Name = "textBox_passwd";
            this.textBox_passwd.PasswordChar = '*';
            this.textBox_passwd.Size = new System.Drawing.Size(161, 21);
            this.textBox_passwd.TabIndex = 1;
            // 
            // label_name
            // 
            this.label_name.AutoSize = true;
            this.label_name.Location = new System.Drawing.Point(35, 24);
            this.label_name.Name = "label_name";
            this.label_name.Size = new System.Drawing.Size(53, 12);
            this.label_name.TabIndex = 108;
            this.label_name.Text = "用户名：";
            // 
            // textBox_userName
            // 
            this.textBox_userName.Location = new System.Drawing.Point(99, 20);
            this.textBox_userName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBox_userName.MaxLength = 24;
            this.textBox_userName.Name = "textBox_userName";
            this.textBox_userName.Size = new System.Drawing.Size(161, 21);
            this.textBox_userName.TabIndex = 0;
            // 
            // label_confirmPassword
            // 
            this.label_confirmPassword.AutoSize = true;
            this.label_confirmPassword.Location = new System.Drawing.Point(35, 93);
            this.label_confirmPassword.Name = "label_confirmPassword";
            this.label_confirmPassword.Size = new System.Drawing.Size(65, 12);
            this.label_confirmPassword.TabIndex = 111;
            this.label_confirmPassword.Text = "确认密码：";
            // 
            // textBox_confirmPassword
            // 
            this.textBox_confirmPassword.Location = new System.Drawing.Point(99, 90);
            this.textBox_confirmPassword.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBox_confirmPassword.MaxLength = 24;
            this.textBox_confirmPassword.Name = "textBox_confirmPassword";
            this.textBox_confirmPassword.PasswordChar = '*';
            this.textBox_confirmPassword.Size = new System.Drawing.Size(161, 21);
            this.textBox_confirmPassword.TabIndex = 2;
            // 
            // NewUserForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(316, 163);
            this.Controls.Add(this.label_confirmPassword);
            this.Controls.Add(this.textBox_confirmPassword);
            this.Controls.Add(this.textBox_userName);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_confirm);
            this.Controls.Add(this.label_passwd);
            this.Controls.Add(this.textBox_passwd);
            this.Controls.Add(this.label_name);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "NewUserForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "创建用户";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.NewUserForm_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.Button button_confirm;
        private System.Windows.Forms.Label label_passwd;
        private System.Windows.Forms.TextBox textBox_passwd;
        private System.Windows.Forms.Label label_name;
        private System.Windows.Forms.TextBox textBox_userName;
        private System.Windows.Forms.Label label_confirmPassword;
        private System.Windows.Forms.TextBox textBox_confirmPassword;
    }
}