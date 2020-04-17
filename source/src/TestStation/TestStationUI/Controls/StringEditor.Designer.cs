namespace TestFlow.DevSoftware.Controls
{
    partial class StringEditor
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
            this.label_recordLevel = new System.Windows.Forms.Label();
            this.comboBox_recordLevel = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // CancelButton
            // 
            this.CancelButton.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.CancelButton.Location = new System.Drawing.Point(208, 101);
            // 
            // OkButton
            // 
            this.OkButton.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.OkButton.Location = new System.Drawing.Point(144, 101);
            // 
            // TypeLabel
            // 
            this.TypeLabel.Size = new System.Drawing.Size(77, 12);
            this.TypeLabel.Text = "Type: String";
            // 
            // ValuetextBox
            // 
            this.ValuetextBox.Location = new System.Drawing.Point(12, 34);
            this.ValuetextBox.Name = "ValuetextBox";
            this.ValuetextBox.Size = new System.Drawing.Size(117, 21);
            this.ValuetextBox.TabIndex = 35;
            // 
            // label_recordLevel
            // 
            this.label_recordLevel.AutoSize = true;
            this.label_recordLevel.Location = new System.Drawing.Point(147, 8);
            this.label_recordLevel.Name = "label_recordLevel";
            this.label_recordLevel.Size = new System.Drawing.Size(83, 12);
            this.label_recordLevel.TabIndex = 37;
            this.label_recordLevel.Text = "Record Level:";
            // 
            // comboBox_recordLevel
            // 
            this.comboBox_recordLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_recordLevel.FormattingEnabled = true;
            this.comboBox_recordLevel.Location = new System.Drawing.Point(149, 34);
            this.comboBox_recordLevel.Name = "comboBox_recordLevel";
            this.comboBox_recordLevel.Size = new System.Drawing.Size(117, 20);
            this.comboBox_recordLevel.TabIndex = 36;
            // 
            // StringEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(283, 144);
            this.Controls.Add(this.label_recordLevel);
            this.Controls.Add(this.comboBox_recordLevel);
            this.Controls.Add(this.ValuetextBox);
            this.Name = "StringEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "StringEditor";
            this.Load += new System.EventHandler(this.StringEditor_Load);
            this.Controls.SetChildIndex(this.ValuetextBox, 0);
            this.Controls.SetChildIndex(this.TypeLabel, 0);
            this.Controls.SetChildIndex(this.OkButton, 0);
            this.Controls.SetChildIndex(this.CancelButton, 0);
            this.Controls.SetChildIndex(this.comboBox_recordLevel, 0);
            this.Controls.SetChildIndex(this.label_recordLevel, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected System.Windows.Forms.TextBox ValuetextBox;
        private System.Windows.Forms.Label label_recordLevel;
        private System.Windows.Forms.ComboBox comboBox_recordLevel;
    }
}
