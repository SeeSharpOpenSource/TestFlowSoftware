namespace TestFlow.Software.OperationPanel
{
    partial class OiSelectionForm
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
            this.button_selectAssembly = new System.Windows.Forms.Button();
            this.label_assemblyName = new System.Windows.Forms.Label();
            this.label_assemblyPath = new System.Windows.Forms.Label();
            this.label_oiClass = new System.Windows.Forms.Label();
            this.button_confirm = new System.Windows.Forms.Button();
            this.button_cancel = new System.Windows.Forms.Button();
            this.button_configOi = new System.Windows.Forms.Button();
            this.comboBox_classes = new System.Windows.Forms.ComboBox();
            this.openFileDialog_assembly = new System.Windows.Forms.OpenFileDialog();
            this.button_removeOi = new System.Windows.Forms.Button();
            this.groupBox_currentOi = new System.Windows.Forms.GroupBox();
            this.label_currentOiClass = new System.Windows.Forms.Label();
            this.label_currentOiClassLabel = new System.Windows.Forms.Label();
            this.label_currentoiAssembly = new System.Windows.Forms.Label();
            this.label_currentoiAssemblyLabel = new System.Windows.Forms.Label();
            this.groupBox_currentOi.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_selectAssembly
            // 
            this.button_selectAssembly.Location = new System.Drawing.Point(479, 132);
            this.button_selectAssembly.Name = "button_selectAssembly";
            this.button_selectAssembly.Size = new System.Drawing.Size(75, 23);
            this.button_selectAssembly.TabIndex = 0;
            this.button_selectAssembly.Text = "Select";
            this.button_selectAssembly.UseVisualStyleBackColor = true;
            this.button_selectAssembly.Click += new System.EventHandler(this.button_selectAssembly_Click);
            // 
            // label_assemblyName
            // 
            this.label_assemblyName.AutoSize = true;
            this.label_assemblyName.Location = new System.Drawing.Point(25, 137);
            this.label_assemblyName.Name = "label_assemblyName";
            this.label_assemblyName.Size = new System.Drawing.Size(59, 12);
            this.label_assemblyName.TabIndex = 1;
            this.label_assemblyName.Text = "Assembly:";
            // 
            // label_assemblyPath
            // 
            this.label_assemblyPath.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label_assemblyPath.Location = new System.Drawing.Point(91, 132);
            this.label_assemblyPath.Name = "label_assemblyPath";
            this.label_assemblyPath.Size = new System.Drawing.Size(382, 23);
            this.label_assemblyPath.TabIndex = 2;
            this.label_assemblyPath.Text = "AssemblyPath";
            this.label_assemblyPath.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label_oiClass
            // 
            this.label_oiClass.AutoSize = true;
            this.label_oiClass.Location = new System.Drawing.Point(25, 185);
            this.label_oiClass.Name = "label_oiClass";
            this.label_oiClass.Size = new System.Drawing.Size(59, 12);
            this.label_oiClass.TabIndex = 4;
            this.label_oiClass.Text = "OI Class:";
            // 
            // button_confirm
            // 
            this.button_confirm.Location = new System.Drawing.Point(329, 220);
            this.button_confirm.Name = "button_confirm";
            this.button_confirm.Size = new System.Drawing.Size(75, 23);
            this.button_confirm.TabIndex = 6;
            this.button_confirm.Text = "Confirm";
            this.button_confirm.UseVisualStyleBackColor = true;
            this.button_confirm.Click += new System.EventHandler(this.button_confirm_Click);
            // 
            // button_cancel
            // 
            this.button_cancel.Location = new System.Drawing.Point(448, 220);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(75, 23);
            this.button_cancel.TabIndex = 7;
            this.button_cancel.Text = "Cancel";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // button_configOi
            // 
            this.button_configOi.Location = new System.Drawing.Point(210, 220);
            this.button_configOi.Name = "button_configOi";
            this.button_configOi.Size = new System.Drawing.Size(75, 23);
            this.button_configOi.TabIndex = 8;
            this.button_configOi.Text = "OI Config";
            this.button_configOi.UseVisualStyleBackColor = true;
            this.button_configOi.Click += new System.EventHandler(this.button_configOi_Click);
            // 
            // comboBox_classes
            // 
            this.comboBox_classes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_classes.FormattingEnabled = true;
            this.comboBox_classes.Location = new System.Drawing.Point(91, 181);
            this.comboBox_classes.Name = "comboBox_classes";
            this.comboBox_classes.Size = new System.Drawing.Size(463, 20);
            this.comboBox_classes.TabIndex = 9;
            this.comboBox_classes.SelectedIndexChanged += new System.EventHandler(this.comboBox_classes_SelectedIndexChanged);
            // 
            // openFileDialog_assembly
            // 
            this.openFileDialog_assembly.FileName = "openFileDialog1";
            this.openFileDialog_assembly.Filter = "dll Files|*.dll";
            // 
            // button_removeOi
            // 
            this.button_removeOi.Location = new System.Drawing.Point(91, 220);
            this.button_removeOi.Name = "button_removeOi";
            this.button_removeOi.Size = new System.Drawing.Size(75, 23);
            this.button_removeOi.TabIndex = 10;
            this.button_removeOi.Text = "Remove OI";
            this.button_removeOi.UseVisualStyleBackColor = true;
            this.button_removeOi.Click += new System.EventHandler(this.button_removeOi_Click);
            // 
            // groupBox_currentOi
            // 
            this.groupBox_currentOi.Controls.Add(this.label_currentOiClass);
            this.groupBox_currentOi.Controls.Add(this.label_currentOiClassLabel);
            this.groupBox_currentOi.Controls.Add(this.label_currentoiAssembly);
            this.groupBox_currentOi.Controls.Add(this.label_currentoiAssemblyLabel);
            this.groupBox_currentOi.Location = new System.Drawing.Point(27, 12);
            this.groupBox_currentOi.Name = "groupBox_currentOi";
            this.groupBox_currentOi.Size = new System.Drawing.Size(527, 100);
            this.groupBox_currentOi.TabIndex = 11;
            this.groupBox_currentOi.TabStop = false;
            this.groupBox_currentOi.Text = "Current Operation panel";
            // 
            // label_currentOiClass
            // 
            this.label_currentOiClass.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label_currentOiClass.Location = new System.Drawing.Point(104, 63);
            this.label_currentOiClass.Name = "label_currentOiClass";
            this.label_currentOiClass.Size = new System.Drawing.Size(382, 23);
            this.label_currentOiClass.TabIndex = 13;
            this.label_currentOiClass.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label_currentOiClassLabel
            // 
            this.label_currentOiClassLabel.AutoSize = true;
            this.label_currentOiClassLabel.Location = new System.Drawing.Point(38, 68);
            this.label_currentOiClassLabel.Name = "label_currentOiClassLabel";
            this.label_currentOiClassLabel.Size = new System.Drawing.Size(59, 12);
            this.label_currentOiClassLabel.TabIndex = 12;
            this.label_currentOiClassLabel.Text = "OI Class:";
            // 
            // label_currentoiAssembly
            // 
            this.label_currentoiAssembly.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label_currentoiAssembly.Location = new System.Drawing.Point(104, 22);
            this.label_currentoiAssembly.Name = "label_currentoiAssembly";
            this.label_currentoiAssembly.Size = new System.Drawing.Size(382, 23);
            this.label_currentoiAssembly.TabIndex = 4;
            this.label_currentoiAssembly.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label_currentoiAssemblyLabel
            // 
            this.label_currentoiAssemblyLabel.AutoSize = true;
            this.label_currentoiAssemblyLabel.Location = new System.Drawing.Point(38, 27);
            this.label_currentoiAssemblyLabel.Name = "label_currentoiAssemblyLabel";
            this.label_currentoiAssemblyLabel.Size = new System.Drawing.Size(59, 12);
            this.label_currentoiAssemblyLabel.TabIndex = 3;
            this.label_currentoiAssemblyLabel.Text = "Assembly:";
            // 
            // OiSelectionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(577, 261);
            this.Controls.Add(this.groupBox_currentOi);
            this.Controls.Add(this.button_removeOi);
            this.Controls.Add(this.comboBox_classes);
            this.Controls.Add(this.button_configOi);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_confirm);
            this.Controls.Add(this.label_oiClass);
            this.Controls.Add(this.label_assemblyPath);
            this.Controls.Add(this.label_assemblyName);
            this.Controls.Add(this.button_selectAssembly);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "OiSelectionForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Operation panel selection";
            this.groupBox_currentOi.ResumeLayout(false);
            this.groupBox_currentOi.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_selectAssembly;
        private System.Windows.Forms.Label label_assemblyName;
        private System.Windows.Forms.Label label_assemblyPath;
        private System.Windows.Forms.Label label_oiClass;
        private System.Windows.Forms.Button button_confirm;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.Button button_configOi;
        private System.Windows.Forms.ComboBox comboBox_classes;
        private System.Windows.Forms.OpenFileDialog openFileDialog_assembly;
        private System.Windows.Forms.Button button_removeOi;
        private System.Windows.Forms.GroupBox groupBox_currentOi;
        private System.Windows.Forms.Label label_currentOiClass;
        private System.Windows.Forms.Label label_currentOiClassLabel;
        private System.Windows.Forms.Label label_currentoiAssembly;
        private System.Windows.Forms.Label label_currentoiAssemblyLabel;
    }
}