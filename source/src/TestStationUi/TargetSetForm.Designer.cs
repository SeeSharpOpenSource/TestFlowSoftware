namespace TestStation.OperationPanel
{
    partial class TargetSetForm
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
            this.numericUpDown_target = new System.Windows.Forms.NumericUpDown();
            this.label_target = new System.Windows.Forms.Label();
            this.button_confirm = new System.Windows.Forms.Button();
            this.button_cancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_target)).BeginInit();
            this.SuspendLayout();
            // 
            // numericUpDown_target
            // 
            this.numericUpDown_target.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDown_target.Location = new System.Drawing.Point(133, 20);
            this.numericUpDown_target.Maximum = new decimal(new int[] {
            50000,
            0,
            0,
            0});
            this.numericUpDown_target.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_target.Name = "numericUpDown_target";
            this.numericUpDown_target.Size = new System.Drawing.Size(99, 26);
            this.numericUpDown_target.TabIndex = 11;
            this.numericUpDown_target.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDown_target.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
            // 
            // label_target
            // 
            this.label_target.AutoSize = true;
            this.label_target.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_target.Location = new System.Drawing.Point(15, 23);
            this.label_target.Name = "label_target";
            this.label_target.Size = new System.Drawing.Size(94, 19);
            this.label_target.TabIndex = 12;
            this.label_target.Text = "Target Value:";
            // 
            // button_confirm
            // 
            this.button_confirm.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_confirm.Location = new System.Drawing.Point(20, 72);
            this.button_confirm.Name = "button_confirm";
            this.button_confirm.Size = new System.Drawing.Size(89, 28);
            this.button_confirm.TabIndex = 13;
            this.button_confirm.Text = "Confirm";
            this.button_confirm.UseVisualStyleBackColor = true;
            this.button_confirm.Click += new System.EventHandler(this.button_confirm_Click);
            // 
            // button_cancel
            // 
            this.button_cancel.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_cancel.Location = new System.Drawing.Point(140, 72);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(89, 28);
            this.button_cancel.TabIndex = 14;
            this.button_cancel.Text = "Cancel";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // TargetSetForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(258, 120);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_confirm);
            this.Controls.Add(this.label_target);
            this.Controls.Add(this.numericUpDown_target);
            this.Font = new System.Drawing.Font("SimHei", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "TargetSetForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Set Target";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_target)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown numericUpDown_target;
        private System.Windows.Forms.Label label_target;
        private System.Windows.Forms.Button button_confirm;
        private System.Windows.Forms.Button button_cancel;
    }
}