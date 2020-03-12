namespace DemoTest
{
    partial class TextInputForm
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
            this.button1 = new System.Windows.Forms.Button();
            this.textBox_barCode = new System.Windows.Forms.TextBox();
            this.label_serialNumber = new System.Windows.Forms.Label();
            this.button_cancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(43, 62);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Confirm";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button_confirm_Click);
            // 
            // textBox_barCode
            // 
            this.textBox_barCode.Location = new System.Drawing.Point(112, 23);
            this.textBox_barCode.Name = "textBox_barCode";
            this.textBox_barCode.Size = new System.Drawing.Size(159, 21);
            this.textBox_barCode.TabIndex = 0;
            this.textBox_barCode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_barCode_KeyDown);
            // 
            // label_serialNumber
            // 
            this.label_serialNumber.AutoSize = true;
            this.label_serialNumber.Location = new System.Drawing.Point(20, 27);
            this.label_serialNumber.Name = "label_serialNumber";
            this.label_serialNumber.Size = new System.Drawing.Size(89, 12);
            this.label_serialNumber.TabIndex = 3;
            this.label_serialNumber.Text = "Serial Number:";
            // 
            // button_cancel
            // 
            this.button_cancel.Location = new System.Drawing.Point(163, 62);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(75, 23);
            this.button_cancel.TabIndex = 2;
            this.button_cancel.Text = "Cancel";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // TextInputForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(283, 97);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.label_serialNumber);
            this.Controls.Add(this.textBox_barCode);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "TextInputForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Input Serial Number";
            this.Shown += new System.EventHandler(this.TextInputForm_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextInputForm_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox_barCode;
        private System.Windows.Forms.Label label_serialNumber;
        private System.Windows.Forms.Button button_cancel;
    }
}