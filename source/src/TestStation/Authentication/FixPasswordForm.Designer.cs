namespace TestFlow.DevSoftware.Authentication
{
    partial class FixPasswordForm
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
            this.label_inputPasswd = new System.Windows.Forms.Label();
            this.textBox_inputPasswd = new System.Windows.Forms.TextBox();
            this.textBox_confirmPasswd = new System.Windows.Forms.TextBox();
            this.label_confirmPasswd = new System.Windows.Forms.Label();
            this.button_confirm = new System.Windows.Forms.Button();
            this.button_cancel = new System.Windows.Forms.Button();
            this.button_clear = new System.Windows.Forms.Button();
            this.textBox_oldPassword = new System.Windows.Forms.TextBox();
            this.label_oldPassword = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label_inputPasswd
            // 
            this.label_inputPasswd.AutoSize = true;
            this.label_inputPasswd.Location = new System.Drawing.Point(26, 58);
            this.label_inputPasswd.Name = "label_inputPasswd";
            this.label_inputPasswd.Size = new System.Drawing.Size(68, 17);
            this.label_inputPasswd.TabIndex = 100;
            this.label_inputPasswd.Text = "输入密码：";
            // 
            // textBox_inputPasswd
            // 
            this.textBox_inputPasswd.Location = new System.Drawing.Point(100, 55);
            this.textBox_inputPasswd.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBox_inputPasswd.MaxLength = 15;
            this.textBox_inputPasswd.Name = "textBox_inputPasswd";
            this.textBox_inputPasswd.PasswordChar = '*';
            this.textBox_inputPasswd.Size = new System.Drawing.Size(158, 23);
            this.textBox_inputPasswd.TabIndex = 0;
            this.textBox_inputPasswd.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_inputPasswd_KeyDown);
            // 
            // textBox_confirmPasswd
            // 
            this.textBox_confirmPasswd.Location = new System.Drawing.Point(100, 97);
            this.textBox_confirmPasswd.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBox_confirmPasswd.MaxLength = 15;
            this.textBox_confirmPasswd.Name = "textBox_confirmPasswd";
            this.textBox_confirmPasswd.PasswordChar = '*';
            this.textBox_confirmPasswd.Size = new System.Drawing.Size(158, 23);
            this.textBox_confirmPasswd.TabIndex = 1;
            this.textBox_confirmPasswd.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FixPasswordForm_KeyDown);
            // 
            // label_confirmPasswd
            // 
            this.label_confirmPasswd.AutoSize = true;
            this.label_confirmPasswd.Location = new System.Drawing.Point(26, 100);
            this.label_confirmPasswd.Name = "label_confirmPasswd";
            this.label_confirmPasswd.Size = new System.Drawing.Size(68, 17);
            this.label_confirmPasswd.TabIndex = 100;
            this.label_confirmPasswd.Text = "确认密码：";
            // 
            // button_confirm
            // 
            this.button_confirm.Location = new System.Drawing.Point(103, 143);
            this.button_confirm.Name = "button_confirm";
            this.button_confirm.Size = new System.Drawing.Size(75, 23);
            this.button_confirm.TabIndex = 3;
            this.button_confirm.Text = "确认";
            this.button_confirm.UseVisualStyleBackColor = true;
            this.button_confirm.Click += new System.EventHandler(this.button_confirm_Click);
            // 
            // button_cancel
            // 
            this.button_cancel.Location = new System.Drawing.Point(188, 143);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(75, 23);
            this.button_cancel.TabIndex = 4;
            this.button_cancel.Text = "取消";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // button_clear
            // 
            this.button_clear.Location = new System.Drawing.Point(18, 143);
            this.button_clear.Name = "button_clear";
            this.button_clear.Size = new System.Drawing.Size(75, 23);
            this.button_clear.TabIndex = 2;
            this.button_clear.Text = "清空";
            this.button_clear.UseVisualStyleBackColor = true;
            this.button_clear.Click += new System.EventHandler(this.button_clear_Click);
            // 
            // textBox_oldPassword
            // 
            this.textBox_oldPassword.Location = new System.Drawing.Point(100, 13);
            this.textBox_oldPassword.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBox_oldPassword.MaxLength = 15;
            this.textBox_oldPassword.Name = "textBox_oldPassword";
            this.textBox_oldPassword.PasswordChar = '*';
            this.textBox_oldPassword.Size = new System.Drawing.Size(158, 23);
            this.textBox_oldPassword.TabIndex = 101;
            // 
            // label_oldPassword
            // 
            this.label_oldPassword.AutoSize = true;
            this.label_oldPassword.Location = new System.Drawing.Point(26, 16);
            this.label_oldPassword.Name = "label_oldPassword";
            this.label_oldPassword.Size = new System.Drawing.Size(68, 17);
            this.label_oldPassword.TabIndex = 102;
            this.label_oldPassword.Text = "原始密码：";
            // 
            // FixPasswordForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(281, 176);
            this.Controls.Add(this.textBox_oldPassword);
            this.Controls.Add(this.label_oldPassword);
            this.Controls.Add(this.button_clear);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_confirm);
            this.Controls.Add(this.textBox_confirmPasswd);
            this.Controls.Add(this.label_confirmPasswd);
            this.Controls.Add(this.textBox_inputPasswd);
            this.Controls.Add(this.label_inputPasswd);
            this.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FixPasswordForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "修改密码";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FixPasswordForm_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_inputPasswd;
        private System.Windows.Forms.TextBox textBox_inputPasswd;
        private System.Windows.Forms.TextBox textBox_confirmPasswd;
        private System.Windows.Forms.Label label_confirmPasswd;
        private System.Windows.Forms.Button button_confirm;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.Button button_clear;
        private System.Windows.Forms.TextBox textBox_oldPassword;
        private System.Windows.Forms.Label label_oldPassword;
    }
}