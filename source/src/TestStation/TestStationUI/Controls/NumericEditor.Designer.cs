namespace TestFlow.DevSoftware
{
    partial class NumericEditor
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
            this.ValuetextBox = new System.Windows.Forms.TextBox();
            this.TypecomboBox = new System.Windows.Forms.ComboBox();
            this.label_recordLevel = new System.Windows.Forms.Label();
            this.comboBox_recordLevel = new System.Windows.Forms.ComboBox();
            this.label_logRecordLevel = new System.Windows.Forms.Label();
            this.comboBox_logRecordLevel = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // CancelButton
            // 
            this.CancelButton.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.CancelButton.Location = new System.Drawing.Point(277, 94);
            // 
            // OkButton
            // 
            this.OkButton.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.OkButton.Location = new System.Drawing.Point(213, 94);
            // 
            // ValuetextBox
            // 
            this.ValuetextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ValuetextBox.Location = new System.Drawing.Point(12, 31);
            this.ValuetextBox.Name = "ValuetextBox";
            this.ValuetextBox.Size = new System.Drawing.Size(181, 21);
            this.ValuetextBox.TabIndex = 40;
            this.ValuetextBox.TextChanged += new System.EventHandler(this.ValuetextBox_TextChanged);
            // 
            // TypecomboBox
            // 
            this.TypecomboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TypecomboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.TypecomboBox.FormattingEnabled = true;
            this.TypecomboBox.Items.AddRange(new object[] {
            "Double",
            "Float",
            "Int",
            "UInt",
            "Short",
            "UShort",
            "Byte",
            "Char"});
            this.TypecomboBox.Location = new System.Drawing.Point(12, 82);
            this.TypecomboBox.Name = "TypecomboBox";
            this.TypecomboBox.Size = new System.Drawing.Size(182, 20);
            this.TypecomboBox.TabIndex = 37;
            // 
            // label_recordLevel
            // 
            this.label_recordLevel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label_recordLevel.AutoSize = true;
            this.label_recordLevel.Location = new System.Drawing.Point(211, 9);
            this.label_recordLevel.Name = "label_recordLevel";
            this.label_recordLevel.Size = new System.Drawing.Size(83, 12);
            this.label_recordLevel.TabIndex = 42;
            this.label_recordLevel.Text = "Record Level:";
            // 
            // comboBox_recordLevel
            // 
            this.comboBox_recordLevel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox_recordLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_recordLevel.FormattingEnabled = true;
            this.comboBox_recordLevel.Location = new System.Drawing.Point(213, 26);
            this.comboBox_recordLevel.Name = "comboBox_recordLevel";
            this.comboBox_recordLevel.Size = new System.Drawing.Size(120, 20);
            this.comboBox_recordLevel.TabIndex = 41;
            // 
            // label_logRecordLevel
            // 
            this.label_logRecordLevel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label_logRecordLevel.AutoSize = true;
            this.label_logRecordLevel.Location = new System.Drawing.Point(211, 52);
            this.label_logRecordLevel.Name = "label_logRecordLevel";
            this.label_logRecordLevel.Size = new System.Drawing.Size(107, 12);
            this.label_logRecordLevel.TabIndex = 44;
            this.label_logRecordLevel.Text = "Log Record Level:";
            // 
            // comboBox_logRecordLevel
            // 
            this.comboBox_logRecordLevel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox_logRecordLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_logRecordLevel.FormattingEnabled = true;
            this.comboBox_logRecordLevel.Location = new System.Drawing.Point(213, 68);
            this.comboBox_logRecordLevel.Name = "comboBox_logRecordLevel";
            this.comboBox_logRecordLevel.Size = new System.Drawing.Size(117, 20);
            this.comboBox_logRecordLevel.TabIndex = 43;
            // 
            // NumericEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(347, 124);
            this.Controls.Add(this.label_logRecordLevel);
            this.Controls.Add(this.comboBox_logRecordLevel);
            this.Controls.Add(this.label_recordLevel);
            this.Controls.Add(this.comboBox_recordLevel);
            this.Controls.Add(this.ValuetextBox);
            this.Controls.Add(this.TypecomboBox);
            this.Name = "NumericEditor";
            this.Text = "NumericEditor";
            this.Load += new System.EventHandler(this.NumericEditor_Load);
            this.Controls.SetChildIndex(this.TypeLabel, 0);
            this.Controls.SetChildIndex(this.OkButton, 0);
            this.Controls.SetChildIndex(this.CancelButton, 0);
            this.Controls.SetChildIndex(this.TypecomboBox, 0);
            this.Controls.SetChildIndex(this.ValuetextBox, 0);
            this.Controls.SetChildIndex(this.comboBox_recordLevel, 0);
            this.Controls.SetChildIndex(this.label_recordLevel, 0);
            this.Controls.SetChildIndex(this.comboBox_logRecordLevel, 0);
            this.Controls.SetChildIndex(this.label_logRecordLevel, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected System.Windows.Forms.TextBox ValuetextBox;
        private System.Windows.Forms.ComboBox TypecomboBox;
        private System.Windows.Forms.Label label_recordLevel;
        private System.Windows.Forms.ComboBox comboBox_recordLevel;
        private System.Windows.Forms.Label label_logRecordLevel;
        private System.Windows.Forms.ComboBox comboBox_logRecordLevel;
    }
}
