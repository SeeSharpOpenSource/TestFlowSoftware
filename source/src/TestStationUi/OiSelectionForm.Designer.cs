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
            this.SuspendLayout();
            // 
            // button_selectAssembly
            // 
            this.button_selectAssembly.Location = new System.Drawing.Point(479, 35);
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
            this.label_assemblyName.Location = new System.Drawing.Point(25, 40);
            this.label_assemblyName.Name = "label_assemblyName";
            this.label_assemblyName.Size = new System.Drawing.Size(59, 12);
            this.label_assemblyName.TabIndex = 1;
            this.label_assemblyName.Text = "Assembly:";
            // 
            // label_assemblyPath
            // 
            this.label_assemblyPath.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label_assemblyPath.Location = new System.Drawing.Point(91, 35);
            this.label_assemblyPath.Name = "label_assemblyPath";
            this.label_assemblyPath.Size = new System.Drawing.Size(382, 23);
            this.label_assemblyPath.TabIndex = 2;
            this.label_assemblyPath.Text = "AssemblyPath";
            this.label_assemblyPath.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label_oiClass
            // 
            this.label_oiClass.AutoSize = true;
            this.label_oiClass.Location = new System.Drawing.Point(25, 88);
            this.label_oiClass.Name = "label_oiClass";
            this.label_oiClass.Size = new System.Drawing.Size(59, 12);
            this.label_oiClass.TabIndex = 4;
            this.label_oiClass.Text = "OI Class:";
            // 
            // button_confirm
            // 
            this.button_confirm.Location = new System.Drawing.Point(329, 123);
            this.button_confirm.Name = "button_confirm";
            this.button_confirm.Size = new System.Drawing.Size(75, 23);
            this.button_confirm.TabIndex = 6;
            this.button_confirm.Text = "Confirm";
            this.button_confirm.UseVisualStyleBackColor = true;
            this.button_confirm.Click += new System.EventHandler(this.button_confirm_Click);
            // 
            // button_cancel
            // 
            this.button_cancel.Location = new System.Drawing.Point(448, 123);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(75, 23);
            this.button_cancel.TabIndex = 7;
            this.button_cancel.Text = "Cancel";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // button_configOi
            // 
            this.button_configOi.Location = new System.Drawing.Point(210, 123);
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
            this.comboBox_classes.Location = new System.Drawing.Point(91, 84);
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
            this.button_removeOi.Location = new System.Drawing.Point(91, 123);
            this.button_removeOi.Name = "button_removeOi";
            this.button_removeOi.Size = new System.Drawing.Size(75, 23);
            this.button_removeOi.TabIndex = 10;
            this.button_removeOi.Text = "Remove OI";
            this.button_removeOi.UseVisualStyleBackColor = true;
            this.button_removeOi.Click += new System.EventHandler(this.button_removeOi_Click);
            // 
            // OiSelectionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(577, 172);
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
    }
}