namespace TestStation
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
            this.SuspendLayout();
            // 
            // CancelButton
            // 
            this.CancelButton.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.CancelButton.Location = new System.Drawing.Point(213, 91);
            // 
            // OkButton
            // 
            this.OkButton.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.OkButton.Location = new System.Drawing.Point(149, 91);
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
            // StringEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(283, 124);
            this.Controls.Add(this.ValuetextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "StringEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "StringEditor";
            this.Controls.SetChildIndex(this.TypeLabel, 0);
            this.Controls.SetChildIndex(this.OkButton, 0);
            this.Controls.SetChildIndex(this.CancelButton, 0);
            this.Controls.SetChildIndex(this.ValuetextBox, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected System.Windows.Forms.TextBox ValuetextBox;
    }
}
