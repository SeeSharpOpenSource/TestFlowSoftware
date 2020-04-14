namespace TestStation
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
            System.Windows.Forms.Label DigitsLabel;
            System.Windows.Forms.Label FormattedNumberLabel;
            this.ValuetextBox = new System.Windows.Forms.TextBox();
            this.DigitNumber = new System.Windows.Forms.NumericUpDown();
            this.TypecomboBox = new System.Windows.Forms.ComboBox();
            this.FormattedNumbertextBox = new System.Windows.Forms.TextBox();
            DigitsLabel = new System.Windows.Forms.Label();
            FormattedNumberLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.DigitNumber)).BeginInit();
            this.SuspendLayout();
            // 
            // CancelButton
            // 
            this.CancelButton.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            // 
            // OkButton
            // 
            this.OkButton.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            // 
            // DigitsLabel
            // 
            DigitsLabel.AutoSize = true;
            DigitsLabel.Location = new System.Drawing.Point(12, 115);
            DigitsLabel.Name = "DigitsLabel";
            DigitsLabel.Size = new System.Drawing.Size(173, 12);
            DigitsLabel.TabIndex = 38;
            DigitsLabel.Text = "Number of Fractional Digits:";
            // 
            // FormattedNumberLabel
            // 
            FormattedNumberLabel.AutoSize = true;
            FormattedNumberLabel.Location = new System.Drawing.Point(150, 11);
            FormattedNumberLabel.Name = "FormattedNumberLabel";
            FormattedNumberLabel.Size = new System.Drawing.Size(107, 12);
            FormattedNumberLabel.TabIndex = 35;
            FormattedNumberLabel.Text = "Formatted Number:";
            // 
            // ValuetextBox
            // 
            this.ValuetextBox.Location = new System.Drawing.Point(12, 31);
            this.ValuetextBox.Name = "ValuetextBox";
            this.ValuetextBox.Size = new System.Drawing.Size(117, 21);
            this.ValuetextBox.TabIndex = 40;
            this.ValuetextBox.TextChanged += new System.EventHandler(this.ValuetextBox_TextChanged);
            // 
            // DigitNumber
            // 
            this.DigitNumber.Location = new System.Drawing.Point(205, 114);
            this.DigitNumber.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.DigitNumber.Name = "DigitNumber";
            this.DigitNumber.Size = new System.Drawing.Size(62, 21);
            this.DigitNumber.TabIndex = 39;
            this.DigitNumber.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.DigitNumber.ValueChanged += new System.EventHandler(this.DigitNumber_ValueChanged);
            // 
            // TypecomboBox
            // 
            this.TypecomboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.TypecomboBox.FormattingEnabled = true;
            this.TypecomboBox.Items.AddRange(new object[] {
            "Real",
            "Integer"});
            this.TypecomboBox.Location = new System.Drawing.Point(151, 64);
            this.TypecomboBox.Name = "TypecomboBox";
            this.TypecomboBox.Size = new System.Drawing.Size(118, 20);
            this.TypecomboBox.TabIndex = 37;
            this.TypecomboBox.SelectedIndexChanged += new System.EventHandler(this.TypecomboBox_SelectedIndexChanged);
            // 
            // FormattedNumbertextBox
            // 
            this.FormattedNumbertextBox.Location = new System.Drawing.Point(150, 31);
            this.FormattedNumbertextBox.Name = "FormattedNumbertextBox";
            this.FormattedNumbertextBox.ReadOnly = true;
            this.FormattedNumbertextBox.Size = new System.Drawing.Size(117, 21);
            this.FormattedNumbertextBox.TabIndex = 36;
            this.FormattedNumbertextBox.Text = "10.0000";
            // 
            // NumericEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(283, 262);
            this.Controls.Add(this.ValuetextBox);
            this.Controls.Add(this.DigitNumber);
            this.Controls.Add(DigitsLabel);
            this.Controls.Add(this.TypecomboBox);
            this.Controls.Add(this.FormattedNumbertextBox);
            this.Controls.Add(FormattedNumberLabel);
            this.Name = "NumericEditor";
            this.Text = "NumericEditor";
            this.Controls.SetChildIndex(this.TypeLabel, 0);
            this.Controls.SetChildIndex(this.OkButton, 0);
            this.Controls.SetChildIndex(this.CancelButton, 0);
            this.Controls.SetChildIndex(FormattedNumberLabel, 0);
            this.Controls.SetChildIndex(this.FormattedNumbertextBox, 0);
            this.Controls.SetChildIndex(this.TypecomboBox, 0);
            this.Controls.SetChildIndex(DigitsLabel, 0);
            this.Controls.SetChildIndex(this.DigitNumber, 0);
            this.Controls.SetChildIndex(this.ValuetextBox, 0);
            ((System.ComponentModel.ISupportInitialize)(this.DigitNumber)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected System.Windows.Forms.TextBox ValuetextBox;
        private System.Windows.Forms.NumericUpDown DigitNumber;
        private System.Windows.Forms.ComboBox TypecomboBox;
        private System.Windows.Forms.TextBox FormattedNumbertextBox;
    }
}
