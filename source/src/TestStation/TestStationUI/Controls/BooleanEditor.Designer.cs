﻿namespace TestStation
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
            this.TypeLabel.Size = new System.Drawing.Size(83, 12);
            this.TypeLabel.Text = "Type: Boolean";
            // 
            // ValuecomboBox
            // 
            this.ValuecomboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ValuecomboBox.FormattingEnabled = true;
            this.ValuecomboBox.Items.AddRange(new object[] {
            "True",
            "False"});
            this.ValuecomboBox.Location = new System.Drawing.Point(12, 35);
            this.ValuecomboBox.Name = "ValuecomboBox";
            this.ValuecomboBox.Size = new System.Drawing.Size(121, 20);
            this.ValuecomboBox.TabIndex = 22;
            // 
            // BooleanEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(283, 124);
            this.Controls.Add(this.ValuecomboBox);
            this.Name = "BooleanEditor";
            this.Text = "BooleanEditor";
            this.Controls.SetChildIndex(this.TypeLabel, 0);
            this.Controls.SetChildIndex(this.OkButton, 0);
            this.Controls.SetChildIndex(this.CancelButton, 0);
            this.Controls.SetChildIndex(this.ValuecomboBox, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox ValuecomboBox;
    }
}
