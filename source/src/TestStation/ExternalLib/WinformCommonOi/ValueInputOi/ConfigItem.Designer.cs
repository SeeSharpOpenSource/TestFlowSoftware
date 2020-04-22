namespace TestFlow.Software.WinformCommonOi.ValueInputOi
{
    partial class ConfigItem
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
            this.label_paramName = new System.Windows.Forms.Label();
            this.textBox_value = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label_paramName
            // 
            this.label_paramName.Font = new System.Drawing.Font("SimHei", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_paramName.Location = new System.Drawing.Point(12, 13);
            this.label_paramName.Name = "label_paramName";
            this.label_paramName.Size = new System.Drawing.Size(162, 23);
            this.label_paramName.TabIndex = 0;
            this.label_paramName.Text = "paramName";
            this.label_paramName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBox_value
            // 
            this.textBox_value.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_value.Font = new System.Drawing.Font("SimHei", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox_value.Location = new System.Drawing.Point(180, 9);
            this.textBox_value.Name = "textBox_value";
            this.textBox_value.Size = new System.Drawing.Size(188, 30);
            this.textBox_value.TabIndex = 1;
            // 
            // ConfigItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(380, 47);
            this.Controls.Add(this.textBox_value);
            this.Controls.Add(this.label_paramName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ConfigItem";
            this.Text = "ConfigItem";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_paramName;
        private System.Windows.Forms.TextBox textBox_value;
    }
}