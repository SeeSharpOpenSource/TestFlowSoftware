namespace JYProductOperationPanel
{
    partial class ProductionTestOIConfigForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProductionTestOIConfigForm));
            this.openFileDialog_panelSelector = new System.Windows.Forms.OpenFileDialog();
            this.label_classNameLabel = new System.Windows.Forms.Label();
            this.label_assemblyPathLabel = new System.Windows.Forms.Label();
            this.textBox_assemblyPath = new System.Windows.Forms.TextBox();
            this.button_assemblyFileSelect = new System.Windows.Forms.Button();
            this.comboBox_classes = new System.Windows.Forms.ComboBox();
            this.label_oiMessageVariable = new System.Windows.Forms.Label();
            this.comboBox_variables = new System.Windows.Forms.ComboBox();
            this.button_confirm = new System.Windows.Forms.Button();
            this.button_cancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // openFileDialog_panelSelector
            // 
            this.openFileDialog_panelSelector.Filter = "dll files|*.dll";
            // 
            // label_classNameLabel
            // 
            this.label_classNameLabel.AutoSize = true;
            this.label_classNameLabel.Location = new System.Drawing.Point(34, 85);
            this.label_classNameLabel.Name = "label_classNameLabel";
            this.label_classNameLabel.Size = new System.Drawing.Size(137, 12);
            this.label_classNameLabel.TabIndex = 0;
            this.label_classNameLabel.Text = "Operation Panel Class:";
            // 
            // label_assemblyPathLabel
            // 
            this.label_assemblyPathLabel.AutoSize = true;
            this.label_assemblyPathLabel.Location = new System.Drawing.Point(34, 38);
            this.label_assemblyPathLabel.Name = "label_assemblyPathLabel";
            this.label_assemblyPathLabel.Size = new System.Drawing.Size(89, 12);
            this.label_assemblyPathLabel.TabIndex = 1;
            this.label_assemblyPathLabel.Text = "Assembly Path:";
            // 
            // textBox_assemblyPath
            // 
            this.textBox_assemblyPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_assemblyPath.Location = new System.Drawing.Point(174, 34);
            this.textBox_assemblyPath.Name = "textBox_assemblyPath";
            this.textBox_assemblyPath.ReadOnly = true;
            this.textBox_assemblyPath.Size = new System.Drawing.Size(400, 21);
            this.textBox_assemblyPath.TabIndex = 2;
            this.textBox_assemblyPath.TextChanged += new System.EventHandler(this.textBox_assemblyPath_TextChanged);
            // 
            // button_assemblyFileSelect
            // 
            this.button_assemblyFileSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_assemblyFileSelect.Location = new System.Drawing.Point(580, 33);
            this.button_assemblyFileSelect.Name = "button_assemblyFileSelect";
            this.button_assemblyFileSelect.Size = new System.Drawing.Size(35, 23);
            this.button_assemblyFileSelect.TabIndex = 3;
            this.button_assemblyFileSelect.Text = "...";
            this.button_assemblyFileSelect.UseVisualStyleBackColor = true;
            this.button_assemblyFileSelect.Click += new System.EventHandler(this.button_assemblyFileSelect_Click);
            // 
            // comboBox_classes
            // 
            this.comboBox_classes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox_classes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_classes.FormattingEnabled = true;
            this.comboBox_classes.Items.AddRange(new object[] {
            ""});
            this.comboBox_classes.Location = new System.Drawing.Point(174, 81);
            this.comboBox_classes.Name = "comboBox_classes";
            this.comboBox_classes.Size = new System.Drawing.Size(441, 20);
            this.comboBox_classes.TabIndex = 4;
            // 
            // label_oiMessageVariable
            // 
            this.label_oiMessageVariable.AutoSize = true;
            this.label_oiMessageVariable.Location = new System.Drawing.Point(34, 132);
            this.label_oiMessageVariable.Name = "label_oiMessageVariable";
            this.label_oiMessageVariable.Size = new System.Drawing.Size(125, 12);
            this.label_oiMessageVariable.TabIndex = 5;
            this.label_oiMessageVariable.Text = "OI Message Variable:";
            // 
            // comboBox_variables
            // 
            this.comboBox_variables.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox_variables.FormattingEnabled = true;
            this.comboBox_variables.Location = new System.Drawing.Point(174, 128);
            this.comboBox_variables.Name = "comboBox_variables";
            this.comboBox_variables.Size = new System.Drawing.Size(441, 20);
            this.comboBox_variables.TabIndex = 6;
            // 
            // button_confirm
            // 
            this.button_confirm.Location = new System.Drawing.Point(96, 174);
            this.button_confirm.Name = "button_confirm";
            this.button_confirm.Size = new System.Drawing.Size(75, 23);
            this.button_confirm.TabIndex = 7;
            this.button_confirm.Text = "Confirm";
            this.button_confirm.UseVisualStyleBackColor = true;
            this.button_confirm.Click += new System.EventHandler(this.button_confirm_Click);
            // 
            // button_cancel
            // 
            this.button_cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.button_cancel.Location = new System.Drawing.Point(481, 174);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(75, 23);
            this.button_cancel.TabIndex = 8;
            this.button_cancel.Text = "Cancel";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // ProductionTestOIConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(652, 217);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_confirm);
            this.Controls.Add(this.comboBox_variables);
            this.Controls.Add(this.label_oiMessageVariable);
            this.Controls.Add(this.comboBox_classes);
            this.Controls.Add(this.button_assemblyFileSelect);
            this.Controls.Add(this.textBox_assemblyPath);
            this.Controls.Add(this.label_assemblyPathLabel);
            this.Controls.Add(this.label_classNameLabel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "ProductionTestOIConfigForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Production Test OI Configure";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog_panelSelector;
        private System.Windows.Forms.Label label_classNameLabel;
        private System.Windows.Forms.Label label_assemblyPathLabel;
        private System.Windows.Forms.TextBox textBox_assemblyPath;
        private System.Windows.Forms.Button button_assemblyFileSelect;
        private System.Windows.Forms.ComboBox comboBox_classes;
        private System.Windows.Forms.Label label_oiMessageVariable;
        private System.Windows.Forms.ComboBox comboBox_variables;
        private System.Windows.Forms.Button button_confirm;
        private System.Windows.Forms.Button button_cancel;
    }
}