namespace TestFlow.DevSoftware
{
    partial class BooleanEditor
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
            this.ValuecomboBox = new System.Windows.Forms.ComboBox();
            this.label_recordLevel = new System.Windows.Forms.Label();
            this.comboBox_recordLevel = new System.Windows.Forms.ComboBox();
            this.label_logRecordLevel = new System.Windows.Forms.Label();
            this.comboBox_logRecordLevel = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // CancelButton
            // 
            this.CancelButton.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.CancelButton.Location = new System.Drawing.Point(245, 91);
            // 
            // OkButton
            // 
            this.OkButton.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.OkButton.Location = new System.Drawing.Point(181, 91);
            // 
            // TypeLabel
            // 
            this.TypeLabel.Size = new System.Drawing.Size(83, 12);
            this.TypeLabel.Text = "Type: Boolean";
            // 
            // ValuecomboBox
            // 
            this.ValuecomboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ValuecomboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ValuecomboBox.FormattingEnabled = true;
            this.ValuecomboBox.Items.AddRange(new object[] {
            "True",
            "False"});
            this.ValuecomboBox.Location = new System.Drawing.Point(12, 35);
            this.ValuecomboBox.Name = "ValuecomboBox";
            this.ValuecomboBox.Size = new System.Drawing.Size(148, 20);
            this.ValuecomboBox.TabIndex = 22;
            // 
            // label_recordLevel
            // 
            this.label_recordLevel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label_recordLevel.AutoSize = true;
            this.label_recordLevel.Location = new System.Drawing.Point(182, 9);
            this.label_recordLevel.Name = "label_recordLevel";
            this.label_recordLevel.Size = new System.Drawing.Size(83, 12);
            this.label_recordLevel.TabIndex = 34;
            this.label_recordLevel.Text = "Record Level:";
            // 
            // comboBox_recordLevel
            // 
            this.comboBox_recordLevel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox_recordLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_recordLevel.FormattingEnabled = true;
            this.comboBox_recordLevel.Location = new System.Drawing.Point(184, 24);
            this.comboBox_recordLevel.Name = "comboBox_recordLevel";
            this.comboBox_recordLevel.Size = new System.Drawing.Size(117, 20);
            this.comboBox_recordLevel.TabIndex = 33;
            // 
            // label_logRecordLevel
            // 
            this.label_logRecordLevel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label_logRecordLevel.AutoSize = true;
            this.label_logRecordLevel.Location = new System.Drawing.Point(182, 49);
            this.label_logRecordLevel.Name = "label_logRecordLevel";
            this.label_logRecordLevel.Size = new System.Drawing.Size(107, 12);
            this.label_logRecordLevel.TabIndex = 41;
            this.label_logRecordLevel.Text = "Log Record Level:";
            // 
            // comboBox_logRecordLevel
            // 
            this.comboBox_logRecordLevel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox_logRecordLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_logRecordLevel.FormattingEnabled = true;
            this.comboBox_logRecordLevel.Location = new System.Drawing.Point(184, 65);
            this.comboBox_logRecordLevel.Name = "comboBox_logRecordLevel";
            this.comboBox_logRecordLevel.Size = new System.Drawing.Size(117, 20);
            this.comboBox_logRecordLevel.TabIndex = 40;
            // 
            // BooleanEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(315, 124);
            this.Controls.Add(this.label_logRecordLevel);
            this.Controls.Add(this.comboBox_logRecordLevel);
            this.Controls.Add(this.label_recordLevel);
            this.Controls.Add(this.comboBox_recordLevel);
            this.Controls.Add(this.ValuecomboBox);
            this.Name = "BooleanEditor";
            this.Text = "BooleanEditor";
            this.Load += new System.EventHandler(this.BooleanEditor_Load);
            this.Controls.SetChildIndex(this.TypeLabel, 0);
            this.Controls.SetChildIndex(this.OkButton, 0);
            this.Controls.SetChildIndex(this.CancelButton, 0);
            this.Controls.SetChildIndex(this.ValuecomboBox, 0);
            this.Controls.SetChildIndex(this.comboBox_recordLevel, 0);
            this.Controls.SetChildIndex(this.label_recordLevel, 0);
            this.Controls.SetChildIndex(this.comboBox_logRecordLevel, 0);
            this.Controls.SetChildIndex(this.label_logRecordLevel, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox ValuecomboBox;
        private System.Windows.Forms.Label label_recordLevel;
        private System.Windows.Forms.ComboBox comboBox_recordLevel;
        private System.Windows.Forms.Label label_logRecordLevel;
        private System.Windows.Forms.ComboBox comboBox_logRecordLevel;
    }
}
