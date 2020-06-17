namespace TestFlow.DevSoftware
{
    partial class AssemblyConfigForm
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
            this.dataGridView_assemblies = new System.Windows.Forms.DataGridView();
            this.Column_available = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Column_assemblyName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_specifiedVersion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_currentVersion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_assemblyPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_selectPath = new System.Windows.Forms.DataGridViewButtonColumn();
            this.button_confirm = new System.Windows.Forms.Button();
            this.button_cancel = new System.Windows.Forms.Button();
            this.label_assembliesTitle = new System.Windows.Forms.Label();
            this.openFileDialog_assembly = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_assemblies)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView_assemblies
            // 
            this.dataGridView_assemblies.AllowUserToAddRows = false;
            this.dataGridView_assemblies.AllowUserToDeleteRows = false;
            this.dataGridView_assemblies.AllowUserToResizeRows = false;
            this.dataGridView_assemblies.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView_assemblies.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_assemblies.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column_available,
            this.Column_assemblyName,
            this.Column_specifiedVersion,
            this.Column_currentVersion,
            this.Column_assemblyPath,
            this.Column_selectPath});
            this.dataGridView_assemblies.Location = new System.Drawing.Point(12, 36);
            this.dataGridView_assemblies.Name = "dataGridView_assemblies";
            this.dataGridView_assemblies.RowHeadersVisible = false;
            this.dataGridView_assemblies.RowTemplate.Height = 23;
            this.dataGridView_assemblies.Size = new System.Drawing.Size(761, 326);
            this.dataGridView_assemblies.TabIndex = 0;
            this.dataGridView_assemblies.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_assemblies_CellContentClick);
            this.dataGridView_assemblies.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_assemblies_CellValueChanged);
            // 
            // Column_available
            // 
            this.Column_available.Frozen = true;
            this.Column_available.HeaderText = "Available";
            this.Column_available.Name = "Column_available";
            this.Column_available.ReadOnly = true;
            this.Column_available.Width = 65;
            // 
            // Column_assemblyName
            // 
            this.Column_assemblyName.HeaderText = "Assembly Name";
            this.Column_assemblyName.Name = "Column_assemblyName";
            this.Column_assemblyName.ReadOnly = true;
            this.Column_assemblyName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column_assemblyName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column_specifiedVersion
            // 
            this.Column_specifiedVersion.FillWeight = 120F;
            this.Column_specifiedVersion.HeaderText = "Used Version";
            this.Column_specifiedVersion.Name = "Column_specifiedVersion";
            this.Column_specifiedVersion.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column_specifiedVersion.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column_currentVersion
            // 
            this.Column_currentVersion.FillWeight = 120F;
            this.Column_currentVersion.HeaderText = "Dll Version";
            this.Column_currentVersion.Name = "Column_currentVersion";
            this.Column_currentVersion.ReadOnly = true;
            this.Column_currentVersion.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column_currentVersion.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column_assemblyPath
            // 
            this.Column_assemblyPath.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column_assemblyPath.HeaderText = "AssemblyPath";
            this.Column_assemblyPath.Name = "Column_assemblyPath";
            this.Column_assemblyPath.ReadOnly = true;
            this.Column_assemblyPath.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column_assemblyPath.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column_selectPath
            // 
            this.Column_selectPath.HeaderText = "Select Dll";
            this.Column_selectPath.Name = "Column_selectPath";
            this.Column_selectPath.ReadOnly = true;
            this.Column_selectPath.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column_selectPath.Text = "...";
            this.Column_selectPath.Width = 80;
            // 
            // button_confirm
            // 
            this.button_confirm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_confirm.Location = new System.Drawing.Point(145, 372);
            this.button_confirm.Name = "button_confirm";
            this.button_confirm.Size = new System.Drawing.Size(75, 23);
            this.button_confirm.TabIndex = 1;
            this.button_confirm.Text = "Confirm";
            this.button_confirm.UseVisualStyleBackColor = true;
            this.button_confirm.Click += new System.EventHandler(this.button_confirm_Click);
            // 
            // button_cancel
            // 
            this.button_cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_cancel.Location = new System.Drawing.Point(521, 372);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(75, 23);
            this.button_cancel.TabIndex = 2;
            this.button_cancel.Text = "Cancel";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // label_assembliesTitle
            // 
            this.label_assembliesTitle.AutoSize = true;
            this.label_assembliesTitle.Location = new System.Drawing.Point(13, 14);
            this.label_assembliesTitle.Name = "label_assembliesTitle";
            this.label_assembliesTitle.Size = new System.Drawing.Size(167, 12);
            this.label_assembliesTitle.TabIndex = 3;
            this.label_assembliesTitle.Text = "Used Assemblies in Sequence";
            // 
            // openFileDialog_assembly
            // 
            this.openFileDialog_assembly.FileName = "openFileDialog1";
            this.openFileDialog_assembly.Filter = "Dll files|*.dll";
            // 
            // AssemblyConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(783, 403);
            this.Controls.Add(this.label_assembliesTitle);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_confirm);
            this.Controls.Add(this.dataGridView_assemblies);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "AssemblyConfigForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Assembly Configuration";
            this.Load += new System.EventHandler(this.AssemblyConfigForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_assemblies)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView_assemblies;
        private System.Windows.Forms.Button button_confirm;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.Label label_assembliesTitle;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column_available;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_assemblyName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_specifiedVersion;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_currentVersion;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_assemblyPath;
        private System.Windows.Forms.DataGridViewButtonColumn Column_selectPath;
        private System.Windows.Forms.OpenFileDialog openFileDialog_assembly;
    }
}