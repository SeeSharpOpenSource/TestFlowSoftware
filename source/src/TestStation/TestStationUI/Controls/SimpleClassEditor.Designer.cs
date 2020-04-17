namespace TestFlow.DevSoftware.Controls
{
    partial class SimpleClassEditor
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
            this.splitContainer_main = new System.Windows.Forms.SplitContainer();
            this.label_properties = new System.Windows.Forms.Label();
            this.dataGridView_properties = new System.Windows.Forms.DataGridView();
            this.Column_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label_fields = new System.Windows.Forms.Label();
            this.dataGridView_fields = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button_confirm = new System.Windows.Forms.Button();
            this.button_cancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_main)).BeginInit();
            this.splitContainer_main.Panel1.SuspendLayout();
            this.splitContainer_main.Panel2.SuspendLayout();
            this.splitContainer_main.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_fields)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer_main
            // 
            this.splitContainer_main.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer_main.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer_main.Location = new System.Drawing.Point(12, 12);
            this.splitContainer_main.Name = "splitContainer_main";
            this.splitContainer_main.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer_main.Panel1
            // 
            this.splitContainer_main.Panel1.Controls.Add(this.label_properties);
            this.splitContainer_main.Panel1.Controls.Add(this.dataGridView_properties);
            // 
            // splitContainer_main.Panel2
            // 
            this.splitContainer_main.Panel2.Controls.Add(this.label_fields);
            this.splitContainer_main.Panel2.Controls.Add(this.dataGridView_fields);
            this.splitContainer_main.Size = new System.Drawing.Size(288, 295);
            this.splitContainer_main.SplitterDistance = 147;
            this.splitContainer_main.TabIndex = 0;
            // 
            // label_properties
            // 
            this.label_properties.AutoSize = true;
            this.label_properties.Location = new System.Drawing.Point(7, 6);
            this.label_properties.Name = "label_properties";
            this.label_properties.Size = new System.Drawing.Size(65, 12);
            this.label_properties.TabIndex = 1;
            this.label_properties.Text = "Properties";
            // 
            // dataGridView_properties
            // 
            this.dataGridView_properties.AllowUserToAddRows = false;
            this.dataGridView_properties.AllowUserToDeleteRows = false;
            this.dataGridView_properties.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_properties.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column_name,
            this.Column_type,
            this.Column_value});
            this.dataGridView_properties.Location = new System.Drawing.Point(3, 24);
            this.dataGridView_properties.Name = "dataGridView_properties";
            this.dataGridView_properties.RowHeadersVisible = false;
            this.dataGridView_properties.RowTemplate.Height = 23;
            this.dataGridView_properties.Size = new System.Drawing.Size(278, 118);
            this.dataGridView_properties.TabIndex = 0;
            this.dataGridView_properties.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridView_properties_CellBeginEdit);
            this.dataGridView_properties.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_properties_CellValueChanged);
            // 
            // Column_name
            // 
            this.Column_name.HeaderText = "Name";
            this.Column_name.Name = "Column_name";
            this.Column_name.ReadOnly = true;
            this.Column_name.Width = 80;
            // 
            // Column_type
            // 
            this.Column_type.HeaderText = "Type";
            this.Column_type.Name = "Column_type";
            this.Column_type.ReadOnly = true;
            this.Column_type.Width = 80;
            // 
            // Column_value
            // 
            this.Column_value.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column_value.HeaderText = "Value";
            this.Column_value.Name = "Column_value";
            // 
            // label_fields
            // 
            this.label_fields.AutoSize = true;
            this.label_fields.Location = new System.Drawing.Point(7, 5);
            this.label_fields.Name = "label_fields";
            this.label_fields.Size = new System.Drawing.Size(41, 12);
            this.label_fields.TabIndex = 2;
            this.label_fields.Text = "Fields";
            // 
            // dataGridView_fields
            // 
            this.dataGridView_fields.AllowUserToAddRows = false;
            this.dataGridView_fields.AllowUserToDeleteRows = false;
            this.dataGridView_fields.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView_fields.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_fields.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3});
            this.dataGridView_fields.Location = new System.Drawing.Point(3, 21);
            this.dataGridView_fields.Name = "dataGridView_fields";
            this.dataGridView_fields.RowHeadersVisible = false;
            this.dataGridView_fields.RowTemplate.Height = 23;
            this.dataGridView_fields.Size = new System.Drawing.Size(278, 118);
            this.dataGridView_fields.TabIndex = 1;
            this.dataGridView_fields.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridView_fields_CellBeginEdit);
            this.dataGridView_fields.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_fields_CellValueChanged);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "Name";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 80;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "Type";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Width = 80;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn3.HeaderText = "Value";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            // 
            // button_confirm
            // 
            this.button_confirm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_confirm.Location = new System.Drawing.Point(36, 314);
            this.button_confirm.Name = "button_confirm";
            this.button_confirm.Size = new System.Drawing.Size(75, 23);
            this.button_confirm.TabIndex = 1;
            this.button_confirm.Text = "Confirm";
            this.button_confirm.UseVisualStyleBackColor = true;
            this.button_confirm.Click += new System.EventHandler(this.button_confirm_Click);
            // 
            // button_cancel
            // 
            this.button_cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_cancel.Location = new System.Drawing.Point(200, 313);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(75, 23);
            this.button_cancel.TabIndex = 2;
            this.button_cancel.Text = "Cancel";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // SimpleClassEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(312, 347);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_confirm);
            this.Controls.Add(this.splitContainer_main);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "SimpleClassEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Simple Class Editor";
            this.Load += new System.EventHandler(this.SimpleClassEditor_Load);
            this.splitContainer_main.Panel1.ResumeLayout(false);
            this.splitContainer_main.Panel1.PerformLayout();
            this.splitContainer_main.Panel2.ResumeLayout(false);
            this.splitContainer_main.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_main)).EndInit();
            this.splitContainer_main.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_fields)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer_main;
        private System.Windows.Forms.Button button_confirm;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.Label label_properties;
        private System.Windows.Forms.DataGridView dataGridView_properties;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_type;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_value;
        private System.Windows.Forms.Label label_fields;
        private System.Windows.Forms.DataGridView dataGridView_fields;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
    }
}