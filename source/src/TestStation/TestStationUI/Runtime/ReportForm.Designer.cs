namespace TestStation.Runtime
{
    partial class ReportForm
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
            this.button_openReportDIr = new System.Windows.Forms.Button();
            this.textBox_reportValue = new System.Windows.Forms.TextBox();
            this.label_reportPathTitle = new System.Windows.Forms.Label();
            this.label_reportPath = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button_openReportDIr
            // 
            this.button_openReportDIr.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_openReportDIr.Location = new System.Drawing.Point(734, 12);
            this.button_openReportDIr.Name = "button_openReportDIr";
            this.button_openReportDIr.Size = new System.Drawing.Size(136, 23);
            this.button_openReportDIr.TabIndex = 0;
            this.button_openReportDIr.Text = "打开报表所在文件夹";
            this.button_openReportDIr.UseVisualStyleBackColor = true;
            this.button_openReportDIr.Click += new System.EventHandler(this.button_openReportDIr_Click);
            // 
            // textBox_reportValue
            // 
            this.textBox_reportValue.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_reportValue.Font = new System.Drawing.Font("Courier New", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_reportValue.Location = new System.Drawing.Point(12, 42);
            this.textBox_reportValue.MaxLength = 65535;
            this.textBox_reportValue.Multiline = true;
            this.textBox_reportValue.Name = "textBox_reportValue";
            this.textBox_reportValue.ReadOnly = true;
            this.textBox_reportValue.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_reportValue.Size = new System.Drawing.Size(858, 710);
            this.textBox_reportValue.TabIndex = 1;
            this.textBox_reportValue.WordWrap = false;
            // 
            // label_reportPathTitle
            // 
            this.label_reportPathTitle.AutoSize = true;
            this.label_reportPathTitle.Location = new System.Drawing.Point(12, 17);
            this.label_reportPathTitle.Name = "label_reportPathTitle";
            this.label_reportPathTitle.Size = new System.Drawing.Size(65, 12);
            this.label_reportPathTitle.TabIndex = 2;
            this.label_reportPathTitle.Text = "报表路径：";
            // 
            // label_reportPath
            // 
            this.label_reportPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label_reportPath.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label_reportPath.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_reportPath.Location = new System.Drawing.Point(72, 13);
            this.label_reportPath.Name = "label_reportPath";
            this.label_reportPath.Size = new System.Drawing.Size(596, 22);
            this.label_reportPath.TabIndex = 3;
            this.label_reportPath.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ReportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(882, 764);
            this.Controls.Add(this.label_reportPath);
            this.Controls.Add(this.label_reportPathTitle);
            this.Controls.Add(this.textBox_reportValue);
            this.Controls.Add(this.button_openReportDIr);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "ReportForm";
            this.Text = "运行结果";
            this.Load += new System.EventHandler(this.ReportForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_openReportDIr;
        private System.Windows.Forms.TextBox textBox_reportValue;
        private System.Windows.Forms.Label label_reportPathTitle;
        private System.Windows.Forms.Label label_reportPath;
    }
}