namespace TestStation.Authentication
{
    partial class ScanLoginForm
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
            this.label_name = new System.Windows.Forms.Label();
            this.textBox_loginName = new System.Windows.Forms.TextBox();
            this.button_userPasswordLogin = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label_name
            // 
            this.label_name.AutoSize = true;
            this.label_name.Location = new System.Drawing.Point(38, 25);
            this.label_name.Name = "label_name";
            this.label_name.Size = new System.Drawing.Size(56, 17);
            this.label_name.TabIndex = 100;
            this.label_name.Text = "用户名：";
            // 
            // textBox_loginName
            // 
            this.textBox_loginName.Location = new System.Drawing.Point(100, 22);
            this.textBox_loginName.Name = "textBox_loginName";
            this.textBox_loginName.Size = new System.Drawing.Size(186, 23);
            this.textBox_loginName.TabIndex = 0;
            this.textBox_loginName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.LoginForm_KeyDown);
            // 
            // button_userPasswordLogin
            // 
            this.button_userPasswordLogin.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.button_userPasswordLogin.Location = new System.Drawing.Point(100, 64);
            this.button_userPasswordLogin.Name = "button_userPasswordLogin";
            this.button_userPasswordLogin.Size = new System.Drawing.Size(113, 29);
            this.button_userPasswordLogin.TabIndex = 1;
            this.button_userPasswordLogin.Text = "账户密码登录";
            this.button_userPasswordLogin.UseVisualStyleBackColor = true;
            this.button_userPasswordLogin.Click += new System.EventHandler(this.button_userPasswordLogin_Click);
            // 
            // ScanLoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(315, 105);
            this.Controls.Add(this.button_userPasswordLogin);
            this.Controls.Add(this.textBox_loginName);
            this.Controls.Add(this.label_name);
            this.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "ScanLoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "用户登录";
            this.Load += new System.EventHandler(this.LoginForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.LoginForm_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label_name;
        private System.Windows.Forms.TextBox textBox_loginName;
        private System.Windows.Forms.Button button_userPasswordLogin;
    }
}