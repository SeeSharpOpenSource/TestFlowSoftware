namespace TestFlow.DevSoftware.Controls
{
    partial class ObjectEditor
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
            this.components = new System.ComponentModel.Container();
            this.comboBox_type = new System.Windows.Forms.ComboBox();
            this.label_typeValue = new System.Windows.Forms.Label();
            this.label_objType = new System.Windows.Forms.Label();
            this.button_OK = new System.Windows.Forms.Button();
            this.button_cancel = new System.Windows.Forms.Button();
            this.dataGridView_element = new System.Windows.Forms.DataGridView();
            this.Column_value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip_delete = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_element)).BeginInit();
            this.contextMenuStrip_delete.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboBox_type
            // 
            this.comboBox_type.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_type.FormattingEnabled = true;
            this.comboBox_type.Items.AddRange(new object[] {
            "Object",
            "Array of String",
            "Array of Boolean",
            "Array of Decimal(Double)",
            "Array of Decimal(Float)",
            "Array of Decimal(Int)",
            "Array of Decimal(UInt)",
            "Array of Decimal(Short)",
            "Array of Decimal(UShort)",
            "Array of Decimal(Byte)"});
            this.comboBox_type.Location = new System.Drawing.Point(52, 9);
            this.comboBox_type.Name = "comboBox_type";
            this.comboBox_type.Size = new System.Drawing.Size(219, 20);
            this.comboBox_type.TabIndex = 22;
            this.comboBox_type.SelectedIndexChanged += new System.EventHandler(this.ValuecomboBox_SelectedIndexChanged);
            // 
            // label_typeValue
            // 
            this.label_typeValue.Location = new System.Drawing.Point(50, 64);
            this.label_typeValue.Name = "label_typeValue";
            this.label_typeValue.Size = new System.Drawing.Size(74, 20);
            this.label_typeValue.TabIndex = 24;
            this.label_typeValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label_objType
            // 
            this.label_objType.AutoSize = true;
            this.label_objType.Location = new System.Drawing.Point(11, 12);
            this.label_objType.Name = "label_objType";
            this.label_objType.Size = new System.Drawing.Size(35, 12);
            this.label_objType.TabIndex = 27;
            this.label_objType.Text = "Type:";
            // 
            // button_OK
            // 
            this.button_OK.Location = new System.Drawing.Point(35, 322);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new System.Drawing.Size(75, 23);
            this.button_OK.TabIndex = 28;
            this.button_OK.Text = "OK";
            this.button_OK.UseVisualStyleBackColor = true;
            this.button_OK.Click += new System.EventHandler(this.button_OK_Click);
            // 
            // button_cancel
            // 
            this.button_cancel.Location = new System.Drawing.Point(175, 321);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(75, 23);
            this.button_cancel.TabIndex = 29;
            this.button_cancel.Text = "Cancel";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // dataGridView_element
            // 
            this.dataGridView_element.AllowUserToResizeRows = false;
            this.dataGridView_element.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_element.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column_value});
            this.dataGridView_element.ContextMenuStrip = this.contextMenuStrip_delete;
            this.dataGridView_element.Location = new System.Drawing.Point(13, 35);
            this.dataGridView_element.Name = "dataGridView_element";
            this.dataGridView_element.RowTemplate.Height = 23;
            this.dataGridView_element.Size = new System.Drawing.Size(258, 280);
            this.dataGridView_element.TabIndex = 30;
            // 
            // Column_value
            // 
            this.Column_value.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column_value.HeaderText = "Value";
            this.Column_value.Name = "Column_value";
            // 
            // contextMenuStrip_delete
            // 
            this.contextMenuStrip_delete.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteToolStripMenuItem});
            this.contextMenuStrip_delete.Name = "contextMenuStrip_delete";
            this.contextMenuStrip_delete.Size = new System.Drawing.Size(153, 48);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // ObjectEditor
            // 
            this.ClientSize = new System.Drawing.Size(283, 348);
            this.Controls.Add(this.dataGridView_element);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_OK);
            this.Controls.Add(this.label_objType);
            this.Controls.Add(this.label_typeValue);
            this.Controls.Add(this.comboBox_type);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ObjectEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Object Editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ObjectEditor_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_element)).EndInit();
            this.contextMenuStrip_delete.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox_type;
        private System.Windows.Forms.Label label_typeValue;
        private System.Windows.Forms.Label label_objType;
        private System.Windows.Forms.Button button_OK;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.DataGridView dataGridView_element;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_value;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_delete;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
    }
}
