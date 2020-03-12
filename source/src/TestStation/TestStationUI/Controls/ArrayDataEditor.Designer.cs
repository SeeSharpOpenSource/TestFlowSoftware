namespace TestStation.Controls
{
    partial class ArrayDataEditor
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
            this.dataGridView_elements = new System.Windows.Forms.DataGridView();
            this.Column_index = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button_confirm = new System.Windows.Forms.Button();
            this.button_cancel = new System.Windows.Forms.Button();
            this.button_remove = new System.Windows.Forms.Button();
            this.button_add = new System.Windows.Forms.Button();
            this.label_elementTypeLabel = new System.Windows.Forms.Label();
            this.label_elementType = new System.Windows.Forms.Label();
            this.button_insert = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_elements)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView_elements
            // 
            this.dataGridView_elements.AllowUserToAddRows = false;
            this.dataGridView_elements.AllowUserToDeleteRows = false;
            this.dataGridView_elements.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView_elements.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_elements.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column_index,
            this.Column_value});
            this.dataGridView_elements.Location = new System.Drawing.Point(12, 51);
            this.dataGridView_elements.Name = "dataGridView_elements";
            this.dataGridView_elements.RowHeadersVisible = false;
            this.dataGridView_elements.RowTemplate.Height = 23;
            this.dataGridView_elements.Size = new System.Drawing.Size(280, 268);
            this.dataGridView_elements.TabIndex = 0;
            this.dataGridView_elements.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridView_elements_CellBeginEdit);
            this.dataGridView_elements.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_elements_CellValueChanged);
            // 
            // Column_index
            // 
            this.Column_index.Frozen = true;
            this.Column_index.HeaderText = "Index";
            this.Column_index.Name = "Column_index";
            this.Column_index.ReadOnly = true;
            this.Column_index.Width = 80;
            // 
            // Column_value
            // 
            this.Column_value.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column_value.HeaderText = "Value";
            this.Column_value.Name = "Column_value";
            // 
            // button_confirm
            // 
            this.button_confirm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_confirm.Location = new System.Drawing.Point(52, 355);
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
            this.button_cancel.Location = new System.Drawing.Point(176, 355);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(75, 23);
            this.button_cancel.TabIndex = 2;
            this.button_cancel.Text = "Cancel";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // button_remove
            // 
            this.button_remove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_remove.Location = new System.Drawing.Point(217, 326);
            this.button_remove.Name = "button_remove";
            this.button_remove.Size = new System.Drawing.Size(75, 23);
            this.button_remove.TabIndex = 4;
            this.button_remove.Text = "Remove";
            this.button_remove.UseVisualStyleBackColor = true;
            this.button_remove.Click += new System.EventHandler(this.button_remove_Click);
            // 
            // button_add
            // 
            this.button_add.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_add.Location = new System.Drawing.Point(20, 326);
            this.button_add.Name = "button_add";
            this.button_add.Size = new System.Drawing.Size(75, 23);
            this.button_add.TabIndex = 3;
            this.button_add.Text = "Add";
            this.button_add.UseVisualStyleBackColor = true;
            this.button_add.Click += new System.EventHandler(this.button_add_Click);
            // 
            // label_elementTypeLabel
            // 
            this.label_elementTypeLabel.AutoSize = true;
            this.label_elementTypeLabel.Location = new System.Drawing.Point(12, 19);
            this.label_elementTypeLabel.Name = "label_elementTypeLabel";
            this.label_elementTypeLabel.Size = new System.Drawing.Size(83, 12);
            this.label_elementTypeLabel.TabIndex = 5;
            this.label_elementTypeLabel.Text = "Element Type:";
            // 
            // label_elementType
            // 
            this.label_elementType.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label_elementType.Location = new System.Drawing.Point(101, 15);
            this.label_elementType.Name = "label_elementType";
            this.label_elementType.Size = new System.Drawing.Size(191, 20);
            this.label_elementType.TabIndex = 6;
            this.label_elementType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button_insert
            // 
            this.button_insert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_insert.Location = new System.Drawing.Point(118, 326);
            this.button_insert.Name = "button_insert";
            this.button_insert.Size = new System.Drawing.Size(75, 23);
            this.button_insert.TabIndex = 7;
            this.button_insert.Text = "Insert";
            this.button_insert.UseVisualStyleBackColor = true;
            this.button_insert.Click += new System.EventHandler(this.button_insert_Click);
            // 
            // ArrayDataEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(304, 387);
            this.Controls.Add(this.button_insert);
            this.Controls.Add(this.label_elementType);
            this.Controls.Add(this.label_elementTypeLabel);
            this.Controls.Add(this.button_remove);
            this.Controls.Add(this.button_add);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_confirm);
            this.Controls.Add(this.dataGridView_elements);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ArrayDataEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Array Data Editor";
            this.Load += new System.EventHandler(this.ArrayDataEditor_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_elements)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView_elements;
        private System.Windows.Forms.Button button_confirm;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_index;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_value;
        private System.Windows.Forms.Button button_remove;
        private System.Windows.Forms.Button button_add;
        private System.Windows.Forms.Label label_elementTypeLabel;
        private System.Windows.Forms.Label label_elementType;
        private System.Windows.Forms.Button button_insert;
    }
}