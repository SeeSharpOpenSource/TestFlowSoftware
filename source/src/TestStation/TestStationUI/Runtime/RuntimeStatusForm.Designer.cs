namespace TestFlow.DevSoftware.Runtime
{
    partial class RuntimeStatusForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridView_status = new System.Windows.Forms.DataGridView();
            this.groupBox_runstatus = new System.Windows.Forms.GroupBox();
            this.groupBox_sequenceInfo = new System.Windows.Forms.GroupBox();
            this.textBox_testInstanceName = new System.Windows.Forms.TextBox();
            this.label_runtimeName = new System.Windows.Forms.Label();
            this.label_nameValue = new System.Windows.Forms.Label();
            this.label_name = new System.Windows.Forms.Label();
            this.Column_session = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_sequence = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_sequenceName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_nowState = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_startTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_endTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_elapsedTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_result = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_status)).BeginInit();
            this.groupBox_runstatus.SuspendLayout();
            this.groupBox_sequenceInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView_status
            // 
            this.dataGridView_status.AllowUserToAddRows = false;
            this.dataGridView_status.AllowUserToDeleteRows = false;
            this.dataGridView_status.AllowUserToResizeRows = false;
            this.dataGridView_status.BackgroundColor = System.Drawing.SystemColors.ControlDark;
            this.dataGridView_status.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView_status.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView_status.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_status.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column_session,
            this.Column_sequence,
            this.Column_sequenceName,
            this.Column_nowState,
            this.Column_startTime,
            this.Column_endTime,
            this.Column_elapsedTime,
            this.Column_result});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView_status.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView_status.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_status.Location = new System.Drawing.Point(3, 17);
            this.dataGridView_status.Name = "dataGridView_status";
            this.dataGridView_status.RowHeadersVisible = false;
            this.dataGridView_status.RowTemplate.Height = 23;
            this.dataGridView_status.Size = new System.Drawing.Size(853, 414);
            this.dataGridView_status.TabIndex = 0;
            // 
            // groupBox_runstatus
            // 
            this.groupBox_runstatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_runstatus.Controls.Add(this.dataGridView_status);
            this.groupBox_runstatus.Location = new System.Drawing.Point(12, 77);
            this.groupBox_runstatus.Name = "groupBox_runstatus";
            this.groupBox_runstatus.Size = new System.Drawing.Size(859, 434);
            this.groupBox_runstatus.TabIndex = 1;
            this.groupBox_runstatus.TabStop = false;
            this.groupBox_runstatus.Text = "运行时状态";
            // 
            // groupBox_sequenceInfo
            // 
            this.groupBox_sequenceInfo.Controls.Add(this.textBox_testInstanceName);
            this.groupBox_sequenceInfo.Controls.Add(this.label_runtimeName);
            this.groupBox_sequenceInfo.Controls.Add(this.label_nameValue);
            this.groupBox_sequenceInfo.Controls.Add(this.label_name);
            this.groupBox_sequenceInfo.Location = new System.Drawing.Point(12, 13);
            this.groupBox_sequenceInfo.Name = "groupBox_sequenceInfo";
            this.groupBox_sequenceInfo.Size = new System.Drawing.Size(892, 58);
            this.groupBox_sequenceInfo.TabIndex = 3;
            this.groupBox_sequenceInfo.TabStop = false;
            this.groupBox_sequenceInfo.Text = "序列信息";
            // 
            // textBox_testInstanceName
            // 
            this.textBox_testInstanceName.Location = new System.Drawing.Point(416, 20);
            this.textBox_testInstanceName.Name = "textBox_testInstanceName";
            this.textBox_testInstanceName.Size = new System.Drawing.Size(283, 21);
            this.textBox_testInstanceName.TabIndex = 3;
            // 
            // label_runtimeName
            // 
            this.label_runtimeName.Location = new System.Drawing.Point(306, 19);
            this.label_runtimeName.Name = "label_runtimeName";
            this.label_runtimeName.Size = new System.Drawing.Size(94, 23);
            this.label_runtimeName.TabIndex = 2;
            this.label_runtimeName.Text = "运行实例名称：";
            this.label_runtimeName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label_nameValue
            // 
            this.label_nameValue.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label_nameValue.Location = new System.Drawing.Point(96, 22);
            this.label_nameValue.Name = "label_nameValue";
            this.label_nameValue.Size = new System.Drawing.Size(195, 23);
            this.label_nameValue.TabIndex = 1;
            this.label_nameValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label_name
            // 
            this.label_name.Location = new System.Drawing.Point(23, 22);
            this.label_name.Name = "label_name";
            this.label_name.Size = new System.Drawing.Size(76, 23);
            this.label_name.TabIndex = 0;
            this.label_name.Text = "序列名称：";
            this.label_name.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Column_session
            // 
            this.Column_session.Frozen = true;
            this.Column_session.HeaderText = "会话";
            this.Column_session.Name = "Column_session";
            this.Column_session.ReadOnly = true;
            this.Column_session.Width = 60;
            // 
            // Column_sequence
            // 
            this.Column_sequence.Frozen = true;
            this.Column_sequence.HeaderText = "序列号";
            this.Column_sequence.Name = "Column_sequence";
            this.Column_sequence.ReadOnly = true;
            this.Column_sequence.Width = 74;
            // 
            // Column_sequenceName
            // 
            this.Column_sequenceName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column_sequenceName.HeaderText = "序列名称";
            this.Column_sequenceName.Name = "Column_sequenceName";
            this.Column_sequenceName.ReadOnly = true;
            // 
            // Column_nowState
            // 
            this.Column_nowState.HeaderText = "状态";
            this.Column_nowState.Name = "Column_nowState";
            this.Column_nowState.ReadOnly = true;
            this.Column_nowState.Width = 80;
            // 
            // Column_startTime
            // 
            this.Column_startTime.HeaderText = "开始时间";
            this.Column_startTime.Name = "Column_startTime";
            this.Column_startTime.ReadOnly = true;
            this.Column_startTime.Width = 110;
            // 
            // Column_endTime
            // 
            this.Column_endTime.HeaderText = "结束时间";
            this.Column_endTime.Name = "Column_endTime";
            this.Column_endTime.ReadOnly = true;
            this.Column_endTime.Width = 110;
            // 
            // Column_elapsedTime
            // 
            this.Column_elapsedTime.HeaderText = "运行时间/ms";
            this.Column_elapsedTime.Name = "Column_elapsedTime";
            this.Column_elapsedTime.ReadOnly = true;
            this.Column_elapsedTime.Width = 109;
            // 
            // Column_result
            // 
            this.Column_result.HeaderText = "运行结果";
            this.Column_result.Name = "Column_result";
            this.Column_result.ReadOnly = true;
            this.Column_result.Width = 120;
            // 
            // RuntimeStatusForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(883, 523);
            this.Controls.Add(this.groupBox_sequenceInfo);
            this.Controls.Add(this.groupBox_runstatus);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "RuntimeStatusForm";
            this.Text = "RuntimeStatusForm";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_status)).EndInit();
            this.groupBox_runstatus.ResumeLayout(false);
            this.groupBox_sequenceInfo.ResumeLayout(false);
            this.groupBox_sequenceInfo.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView_status;
        private System.Windows.Forms.GroupBox groupBox_runstatus;
        private System.Windows.Forms.GroupBox groupBox_sequenceInfo;
        private System.Windows.Forms.Label label_name;
        private System.Windows.Forms.Label label_nameValue;
        private System.Windows.Forms.Label label_runtimeName;
        private System.Windows.Forms.TextBox textBox_testInstanceName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_session;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_sequence;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_sequenceName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_nowState;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_startTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_endTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_elapsedTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_result;
    }
}