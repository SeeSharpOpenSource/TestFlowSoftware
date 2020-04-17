namespace TestFlow.DevSoftware
{
    partial class ErrorInfoForm
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
            this.button_continue = new System.Windows.Forms.Button();
            this.button_cancel = new System.Windows.Forms.Button();
            this.textBox_errorInfo = new System.Windows.Forms.TextBox();
            this.label_tip = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button_continue
            // 
            this.button_continue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_continue.Location = new System.Drawing.Point(134, 295);
            this.button_continue.Name = "button_continue";
            this.button_continue.Size = new System.Drawing.Size(75, 23);
            this.button_continue.TabIndex = 0;
            this.button_continue.Text = "Continue";
            this.button_continue.UseVisualStyleBackColor = true;
            this.button_continue.Click += new System.EventHandler(this.button_continue_Click);
            // 
            // button_cancel
            // 
            this.button_cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_cancel.Location = new System.Drawing.Point(354, 295);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(75, 23);
            this.button_cancel.TabIndex = 1;
            this.button_cancel.Text = "Cancel";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // textBox_errorInfo
            // 
            this.textBox_errorInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_errorInfo.Location = new System.Drawing.Point(42, 29);
            this.textBox_errorInfo.MaxLength = 327670;
            this.textBox_errorInfo.Multiline = true;
            this.textBox_errorInfo.Name = "textBox_errorInfo";
            this.textBox_errorInfo.ReadOnly = true;
            this.textBox_errorInfo.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_errorInfo.Size = new System.Drawing.Size(478, 252);
            this.textBox_errorInfo.TabIndex = 2;
            // 
            // label_tip
            // 
            this.label_tip.AutoSize = true;
            this.label_tip.Location = new System.Drawing.Point(42, 11);
            this.label_tip.Name = "label_tip";
            this.label_tip.Size = new System.Drawing.Size(131, 12);
            this.label_tip.TabIndex = 3;
            this.label_tip.Text = "Configuration errors:";
            // 
            // ErrorInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(562, 330);
            this.Controls.Add(this.label_tip);
            this.Controls.Add(this.textBox_errorInfo);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_continue);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "ErrorInfoForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Error Information";
            this.Load += new System.EventHandler(this.ErrorInfoForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_continue;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.TextBox textBox_errorInfo;
        private System.Windows.Forms.Label label_tip;
    }
}