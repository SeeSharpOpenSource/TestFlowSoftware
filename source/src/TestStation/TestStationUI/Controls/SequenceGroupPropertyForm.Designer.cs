namespace TestFlow.DevSoftware.Controls
{
    partial class SequenceGroupPropertyForm
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
            this.label_execution = new System.Windows.Forms.Label();
            this.comboBox_execution = new System.Windows.Forms.ComboBox();
            this.label_versionLabel = new System.Windows.Forms.Label();
            this.label_version = new System.Windows.Forms.Label();
            this.label_createTime = new System.Windows.Forms.Label();
            this.label_createTimeLabel = new System.Windows.Forms.Label();
            this.label_modifiedTime = new System.Windows.Forms.Label();
            this.label_modifiedTimeLabel = new System.Windows.Forms.Label();
            this.comboBox_platform = new System.Windows.Forms.ComboBox();
            this.label_platform = new System.Windows.Forms.Label();
            this.button_confirm = new System.Windows.Forms.Button();
            this.button_cancel = new System.Windows.Forms.Button();
            this.label_name = new System.Windows.Forms.Label();
            this.label_sequenceName = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label_execution
            // 
            this.label_execution.AutoSize = true;
            this.label_execution.Location = new System.Drawing.Point(15, 200);
            this.label_execution.Name = "label_execution";
            this.label_execution.Size = new System.Drawing.Size(65, 12);
            this.label_execution.TabIndex = 0;
            this.label_execution.Text = "Execution:";
            // 
            // comboBox_execution
            // 
            this.comboBox_execution.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_execution.FormattingEnabled = true;
            this.comboBox_execution.Location = new System.Drawing.Point(109, 196);
            this.comboBox_execution.Name = "comboBox_execution";
            this.comboBox_execution.Size = new System.Drawing.Size(136, 20);
            this.comboBox_execution.TabIndex = 1;
            // 
            // label_versionLabel
            // 
            this.label_versionLabel.AutoSize = true;
            this.label_versionLabel.Location = new System.Drawing.Point(15, 68);
            this.label_versionLabel.Name = "label_versionLabel";
            this.label_versionLabel.Size = new System.Drawing.Size(53, 12);
            this.label_versionLabel.TabIndex = 2;
            this.label_versionLabel.Text = "Version:";
            // 
            // label_version
            // 
            this.label_version.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label_version.Location = new System.Drawing.Point(109, 64);
            this.label_version.Name = "label_version";
            this.label_version.Size = new System.Drawing.Size(136, 20);
            this.label_version.TabIndex = 3;
            this.label_version.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label_createTime
            // 
            this.label_createTime.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label_createTime.Location = new System.Drawing.Point(109, 108);
            this.label_createTime.Name = "label_createTime";
            this.label_createTime.Size = new System.Drawing.Size(136, 20);
            this.label_createTime.TabIndex = 5;
            this.label_createTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label_createTimeLabel
            // 
            this.label_createTimeLabel.AutoSize = true;
            this.label_createTimeLabel.Location = new System.Drawing.Point(15, 112);
            this.label_createTimeLabel.Name = "label_createTimeLabel";
            this.label_createTimeLabel.Size = new System.Drawing.Size(77, 12);
            this.label_createTimeLabel.TabIndex = 4;
            this.label_createTimeLabel.Text = "Create Time:";
            // 
            // label_modifiedTime
            // 
            this.label_modifiedTime.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label_modifiedTime.Location = new System.Drawing.Point(109, 152);
            this.label_modifiedTime.Name = "label_modifiedTime";
            this.label_modifiedTime.Size = new System.Drawing.Size(136, 20);
            this.label_modifiedTime.TabIndex = 7;
            this.label_modifiedTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label_modifiedTimeLabel
            // 
            this.label_modifiedTimeLabel.AutoSize = true;
            this.label_modifiedTimeLabel.Location = new System.Drawing.Point(15, 156);
            this.label_modifiedTimeLabel.Name = "label_modifiedTimeLabel";
            this.label_modifiedTimeLabel.Size = new System.Drawing.Size(89, 12);
            this.label_modifiedTimeLabel.TabIndex = 6;
            this.label_modifiedTimeLabel.Text = "Modified Time:";
            // 
            // comboBox_platform
            // 
            this.comboBox_platform.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_platform.FormattingEnabled = true;
            this.comboBox_platform.Location = new System.Drawing.Point(109, 240);
            this.comboBox_platform.Name = "comboBox_platform";
            this.comboBox_platform.Size = new System.Drawing.Size(136, 20);
            this.comboBox_platform.TabIndex = 9;
            // 
            // label_platform
            // 
            this.label_platform.AutoSize = true;
            this.label_platform.Location = new System.Drawing.Point(15, 244);
            this.label_platform.Name = "label_platform";
            this.label_platform.Size = new System.Drawing.Size(59, 12);
            this.label_platform.TabIndex = 8;
            this.label_platform.Text = "Platform:";
            // 
            // button_confirm
            // 
            this.button_confirm.Location = new System.Drawing.Point(33, 286);
            this.button_confirm.Name = "button_confirm";
            this.button_confirm.Size = new System.Drawing.Size(75, 23);
            this.button_confirm.TabIndex = 10;
            this.button_confirm.Text = "Confirm";
            this.button_confirm.UseVisualStyleBackColor = true;
            this.button_confirm.Click += new System.EventHandler(this.button_confirm_Click);
            // 
            // button_cancel
            // 
            this.button_cancel.Location = new System.Drawing.Point(144, 286);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(75, 23);
            this.button_cancel.TabIndex = 11;
            this.button_cancel.Text = "Cancel";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // label_name
            // 
            this.label_name.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label_name.Location = new System.Drawing.Point(109, 20);
            this.label_name.Name = "label_name";
            this.label_name.Size = new System.Drawing.Size(136, 20);
            this.label_name.TabIndex = 13;
            this.label_name.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label_sequenceName
            // 
            this.label_sequenceName.AutoSize = true;
            this.label_sequenceName.Location = new System.Drawing.Point(15, 24);
            this.label_sequenceName.Name = "label_sequenceName";
            this.label_sequenceName.Size = new System.Drawing.Size(89, 12);
            this.label_sequenceName.TabIndex = 12;
            this.label_sequenceName.Text = "Sequence Name:";
            // 
            // SequenceGroupPropertyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(261, 326);
            this.Controls.Add(this.label_name);
            this.Controls.Add(this.label_sequenceName);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_confirm);
            this.Controls.Add(this.comboBox_platform);
            this.Controls.Add(this.label_platform);
            this.Controls.Add(this.label_modifiedTime);
            this.Controls.Add(this.label_modifiedTimeLabel);
            this.Controls.Add(this.label_createTime);
            this.Controls.Add(this.label_createTimeLabel);
            this.Controls.Add(this.label_version);
            this.Controls.Add(this.label_versionLabel);
            this.Controls.Add(this.comboBox_execution);
            this.Controls.Add(this.label_execution);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "SequenceGroupPropertyForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Sequence Property";
            this.Load += new System.EventHandler(this.SequencePropertyForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_execution;
        private System.Windows.Forms.ComboBox comboBox_execution;
        private System.Windows.Forms.Label label_versionLabel;
        private System.Windows.Forms.Label label_version;
        private System.Windows.Forms.Label label_createTime;
        private System.Windows.Forms.Label label_createTimeLabel;
        private System.Windows.Forms.Label label_modifiedTime;
        private System.Windows.Forms.Label label_modifiedTimeLabel;
        private System.Windows.Forms.ComboBox comboBox_platform;
        private System.Windows.Forms.Label label_platform;
        private System.Windows.Forms.Button button_confirm;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.Label label_name;
        private System.Windows.Forms.Label label_sequenceName;
    }
}