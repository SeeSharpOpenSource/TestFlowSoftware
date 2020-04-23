namespace TestFlow.Software.WinformCommonOi.ValueInputOi
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
            this.label_sequenceName = new System.Windows.Forms.Label();
            this.dataGridView_sequenceView = new System.Windows.Forms.DataGridView();
            this.Column_sequenceName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_startTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_endTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.textBox_status = new System.Windows.Forms.TextBox();
            this.splitContainer_contents = new System.Windows.Forms.SplitContainer();
            this.label_statusValue = new System.Windows.Forms.Label();
            this.label_status = new System.Windows.Forms.Label();
            this.led_result = new SeeSharpTools.JY.GUI.LED();
            this.label_result = new System.Windows.Forms.Label();
            this.splitContainer_status = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_sequenceView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_contents)).BeginInit();
            this.splitContainer_contents.Panel1.SuspendLayout();
            this.splitContainer_contents.Panel2.SuspendLayout();
            this.splitContainer_contents.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_status)).BeginInit();
            this.splitContainer_status.Panel1.SuspendLayout();
            this.splitContainer_status.Panel2.SuspendLayout();
            this.splitContainer_status.SuspendLayout();
            this.SuspendLayout();
            // 
            // label_sequenceName
            // 
            this.label_sequenceName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label_sequenceName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(35)))), ((int)(((byte)(25)))));
            this.label_sequenceName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label_sequenceName.Font = new System.Drawing.Font("SimHei", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_sequenceName.ForeColor = System.Drawing.Color.White;
            this.label_sequenceName.Location = new System.Drawing.Point(26, 23);
            this.label_sequenceName.Name = "label_sequenceName";
            this.label_sequenceName.Size = new System.Drawing.Size(739, 58);
            this.label_sequenceName.TabIndex = 0;
            this.label_sequenceName.Text = "sequenceName";
            this.label_sequenceName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dataGridView_sequenceView
            // 
            this.dataGridView_sequenceView.AllowUserToAddRows = false;
            this.dataGridView_sequenceView.AllowUserToDeleteRows = false;
            this.dataGridView_sequenceView.AllowUserToResizeRows = false;
            this.dataGridView_sequenceView.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dataGridView_sequenceView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_sequenceView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column_sequenceName,
            this.Column_startTime,
            this.Column_endTime,
            this.Column_status});
            this.dataGridView_sequenceView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_sequenceView.Location = new System.Drawing.Point(0, 0);
            this.dataGridView_sequenceView.Name = "dataGridView_sequenceView";
            this.dataGridView_sequenceView.RowHeadersVisible = false;
            this.dataGridView_sequenceView.RowTemplate.Height = 23;
            this.dataGridView_sequenceView.Size = new System.Drawing.Size(536, 133);
            this.dataGridView_sequenceView.TabIndex = 1;
            // 
            // Column_sequenceName
            // 
            this.Column_sequenceName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column_sequenceName.HeaderText = "Sequence";
            this.Column_sequenceName.Name = "Column_sequenceName";
            this.Column_sequenceName.ReadOnly = true;
            // 
            // Column_startTime
            // 
            this.Column_startTime.HeaderText = "Start Time";
            this.Column_startTime.Name = "Column_startTime";
            this.Column_startTime.ReadOnly = true;
            this.Column_startTime.Width = 150;
            // 
            // Column_endTime
            // 
            this.Column_endTime.HeaderText = "End Time";
            this.Column_endTime.Name = "Column_endTime";
            this.Column_endTime.ReadOnly = true;
            this.Column_endTime.Width = 150;
            // 
            // Column_status
            // 
            this.Column_status.HeaderText = "Status";
            this.Column_status.Name = "Column_status";
            this.Column_status.ReadOnly = true;
            this.Column_status.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // textBox_status
            // 
            this.textBox_status.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_status.Location = new System.Drawing.Point(0, 0);
            this.textBox_status.MaxLength = 65534;
            this.textBox_status.Multiline = true;
            this.textBox_status.Name = "textBox_status";
            this.textBox_status.ReadOnly = true;
            this.textBox_status.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_status.Size = new System.Drawing.Size(536, 211);
            this.textBox_status.TabIndex = 2;
            // 
            // splitContainer_contents
            // 
            this.splitContainer_contents.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer_contents.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer_contents.Location = new System.Drawing.Point(26, 95);
            this.splitContainer_contents.Name = "splitContainer_contents";
            // 
            // splitContainer_contents.Panel1
            // 
            this.splitContainer_contents.Panel1.Controls.Add(this.label_statusValue);
            this.splitContainer_contents.Panel1.Controls.Add(this.label_status);
            this.splitContainer_contents.Panel1.Controls.Add(this.led_result);
            this.splitContainer_contents.Panel1.Controls.Add(this.label_result);
            // 
            // splitContainer_contents.Panel2
            // 
            this.splitContainer_contents.Panel2.Controls.Add(this.splitContainer_status);
            this.splitContainer_contents.Size = new System.Drawing.Size(739, 348);
            this.splitContainer_contents.SplitterDistance = 199;
            this.splitContainer_contents.TabIndex = 3;
            // 
            // label_statusValue
            // 
            this.label_statusValue.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label_statusValue.Font = new System.Drawing.Font("SimSun", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_statusValue.Location = new System.Drawing.Point(26, 89);
            this.label_statusValue.Name = "label_statusValue";
            this.label_statusValue.Size = new System.Drawing.Size(133, 42);
            this.label_statusValue.TabIndex = 3;
            this.label_statusValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_status
            // 
            this.label_status.AutoSize = true;
            this.label_status.Font = new System.Drawing.Font("SimSun", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_status.Location = new System.Drawing.Point(53, 43);
            this.label_status.Name = "label_status";
            this.label_status.Size = new System.Drawing.Size(79, 19);
            this.label_status.TabIndex = 2;
            this.label_status.Text = "Status:";
            // 
            // led_result
            // 
            this.led_result.BlinkColor = System.Drawing.Color.Orange;
            this.led_result.BlinkInterval = 1000;
            this.led_result.BlinkOn = false;
            this.led_result.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.led_result.Interacton = SeeSharpTools.JY.GUI.LED.InteractionStyle.Indicator;
            this.led_result.Location = new System.Drawing.Point(62, 241);
            this.led_result.Name = "led_result";
            this.led_result.OffColor = System.Drawing.Color.Gray;
            this.led_result.OnColor = System.Drawing.Color.Lime;
            this.led_result.Size = new System.Drawing.Size(60, 60);
            this.led_result.Style = SeeSharpTools.JY.GUI.LED.LedStyle.Circular3D;
            this.led_result.TabIndex = 1;
            this.led_result.Value = false;
            // 
            // label_result
            // 
            this.label_result.AutoSize = true;
            this.label_result.Font = new System.Drawing.Font("SimSun", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_result.Location = new System.Drawing.Point(53, 202);
            this.label_result.Name = "label_result";
            this.label_result.Size = new System.Drawing.Size(79, 19);
            this.label_result.TabIndex = 0;
            this.label_result.Text = "Result:";
            // 
            // splitContainer_status
            // 
            this.splitContainer_status.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_status.Location = new System.Drawing.Point(0, 0);
            this.splitContainer_status.Name = "splitContainer_status";
            this.splitContainer_status.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer_status.Panel1
            // 
            this.splitContainer_status.Panel1.Controls.Add(this.dataGridView_sequenceView);
            // 
            // splitContainer_status.Panel2
            // 
            this.splitContainer_status.Panel2.Controls.Add(this.textBox_status);
            this.splitContainer_status.Size = new System.Drawing.Size(536, 348);
            this.splitContainer_status.SplitterDistance = 133;
            this.splitContainer_status.TabIndex = 0;
            // 
            // RuntimeStatusForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(778, 455);
            this.Controls.Add(this.splitContainer_contents);
            this.Controls.Add(this.label_sequenceName);
            this.MinimumSize = new System.Drawing.Size(794, 493);
            this.Name = "RuntimeStatusForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Runtime Status";
            this.Load += new System.EventHandler(this.RuntimeStatusForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_sequenceView)).EndInit();
            this.splitContainer_contents.Panel1.ResumeLayout(false);
            this.splitContainer_contents.Panel1.PerformLayout();
            this.splitContainer_contents.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_contents)).EndInit();
            this.splitContainer_contents.ResumeLayout(false);
            this.splitContainer_status.Panel1.ResumeLayout(false);
            this.splitContainer_status.Panel2.ResumeLayout(false);
            this.splitContainer_status.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_status)).EndInit();
            this.splitContainer_status.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label_sequenceName;
        private System.Windows.Forms.DataGridView dataGridView_sequenceView;
        private System.Windows.Forms.TextBox textBox_status;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_sequenceName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_startTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_endTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_status;
        private System.Windows.Forms.SplitContainer splitContainer_contents;
        private System.Windows.Forms.SplitContainer splitContainer_status;
        private System.Windows.Forms.Label label_result;
        private SeeSharpTools.JY.GUI.LED led_result;
        private System.Windows.Forms.Label label_status;
        private System.Windows.Forms.Label label_statusValue;
    }
}