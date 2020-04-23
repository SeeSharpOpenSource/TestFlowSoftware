namespace TestFlow.Software.WinformCommonOi.ValueInputOi
{
    partial class OiConfigForm
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
            this.dataGridView_paramconfig = new System.Windows.Forms.DataGridView();
            this.button_confirm = new System.Windows.Forms.Button();
            this.button_cancel = new System.Windows.Forms.Button();
            this.button_delete = new System.Windows.Forms.Button();
            this.button_add = new System.Windows.Forms.Button();
            this.Column_paramName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_variableName = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Column_constantValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_paramconfig)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView_paramconfig
            // 
            this.dataGridView_paramconfig.AllowUserToAddRows = false;
            this.dataGridView_paramconfig.AllowUserToDeleteRows = false;
            this.dataGridView_paramconfig.AllowUserToResizeRows = false;
            this.dataGridView_paramconfig.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_paramconfig.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column_paramName,
            this.Column_variableName,
            this.Column_constantValue});
            this.dataGridView_paramconfig.Location = new System.Drawing.Point(12, 79);
            this.dataGridView_paramconfig.Name = "dataGridView_paramconfig";
            this.dataGridView_paramconfig.RowHeadersVisible = false;
            this.dataGridView_paramconfig.RowTemplate.Height = 23;
            this.dataGridView_paramconfig.Size = new System.Drawing.Size(463, 206);
            this.dataGridView_paramconfig.TabIndex = 0;
            // 
            // button_confirm
            // 
            this.button_confirm.Location = new System.Drawing.Point(78, 321);
            this.button_confirm.Name = "button_confirm";
            this.button_confirm.Size = new System.Drawing.Size(75, 23);
            this.button_confirm.TabIndex = 1;
            this.button_confirm.Text = "Confirm";
            this.button_confirm.UseVisualStyleBackColor = true;
            this.button_confirm.Click += new System.EventHandler(this.button_confirm_Click);
            // 
            // button_cancel
            // 
            this.button_cancel.Location = new System.Drawing.Point(308, 321);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(75, 23);
            this.button_cancel.TabIndex = 2;
            this.button_cancel.Text = "Cancel";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // button_delete
            // 
            this.button_delete.Location = new System.Drawing.Point(308, 292);
            this.button_delete.Name = "button_delete";
            this.button_delete.Size = new System.Drawing.Size(75, 23);
            this.button_delete.TabIndex = 4;
            this.button_delete.Text = "Delete";
            this.button_delete.UseVisualStyleBackColor = true;
            this.button_delete.Click += new System.EventHandler(this.button_delete_Click);
            // 
            // button_add
            // 
            this.button_add.Location = new System.Drawing.Point(78, 292);
            this.button_add.Name = "button_add";
            this.button_add.Size = new System.Drawing.Size(75, 23);
            this.button_add.TabIndex = 3;
            this.button_add.Text = "Add";
            this.button_add.UseVisualStyleBackColor = true;
            this.button_add.Click += new System.EventHandler(this.button_add_Click);
            // 
            // Column_paramName
            // 
            this.Column_paramName.HeaderText = "Parameter Name";
            this.Column_paramName.Name = "Column_paramName";
            this.Column_paramName.Width = 150;
            // 
            // Column_variableName
            // 
            this.Column_variableName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column_variableName.HeaderText = "Variable Name";
            this.Column_variableName.Name = "Column_variableName";
            // 
            // Column_constantValue
            // 
            this.Column_constantValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column_constantValue.HeaderText = "ConstantValue";
            this.Column_constantValue.Name = "Column_constantValue";
            // 
            // OiConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(487, 351);
            this.Controls.Add(this.button_delete);
            this.Controls.Add(this.button_add);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_confirm);
            this.Controls.Add(this.dataGridView_paramconfig);
            this.Name = "OiConfigForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Operation Panel Configuration";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_paramconfig)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView_paramconfig;
        private System.Windows.Forms.Button button_confirm;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.Button button_delete;
        private System.Windows.Forms.Button button_add;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_paramName;
        private System.Windows.Forms.DataGridViewComboBoxColumn Column_variableName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_constantValue;
    }
}