namespace TestStation
{
    partial class ConfigureForm
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
            this.label_testRecordResult = new System.Windows.Forms.Label();
            this.groupBox_sequenceDefaultProperties = new System.Windows.Forms.GroupBox();
            this.tabControl_SequencedefaultSettings = new System.Windows.Forms.TabControl();
            this.tabPage_test = new System.Windows.Forms.TabPage();
            this.comboBox_testBreakIfFailed = new System.Windows.Forms.ComboBox();
            this.label_testBreakIfFailed = new System.Windows.Forms.Label();
            this.comboBox_testRecordResult = new System.Windows.Forms.ComboBox();
            this.tabPage_action = new System.Windows.Forms.TabPage();
            this.comboBox_actionBreakIfFailed = new System.Windows.Forms.ComboBox();
            this.label_actionBreakIfFailed = new System.Windows.Forms.Label();
            this.comboBox_actionRecordResult = new System.Windows.Forms.ComboBox();
            this.label_actionRecordResult = new System.Windows.Forms.Label();
            this.tabPage_sequenceCall = new System.Windows.Forms.TabPage();
            this.comboBox_seqCallBreakIfFailed = new System.Windows.Forms.ComboBox();
            this.label_seqCallBreakIfFailed = new System.Windows.Forms.Label();
            this.comboBox_seqCallRecordResult = new System.Windows.Forms.ComboBox();
            this.label_seqCallRecordResult = new System.Windows.Forms.Label();
            this.button_apply = new System.Windows.Forms.Button();
            this.button_cancel = new System.Windows.Forms.Button();
            this.tabControl_settings = new System.Windows.Forms.TabControl();
            this.tabPage_sequenceSettings = new System.Windows.Forms.TabPage();
            this.tabPage_Report = new System.Windows.Forms.TabPage();
            this.button_selectReportPath = new System.Windows.Forms.Button();
            this.label_reportPath = new System.Windows.Forms.Label();
            this.groupBox_reportName = new System.Windows.Forms.GroupBox();
            this.label_baseName = new System.Windows.Forms.Label();
            this.textBox_baseName = new System.Windows.Forms.TextBox();
            this.textbox_reportNamePreview = new System.Windows.Forms.TextBox();
            this.button_removeNameElem = new System.Windows.Forms.Button();
            this.button_addNameElem = new System.Windows.Forms.Button();
            this.listBox_nameElement = new System.Windows.Forms.ListBox();
            this.textBox_reportNameFormat = new System.Windows.Forms.TextBox();
            this.label_reportNameFormatLabel = new System.Windows.Forms.Label();
            this.label_reportNamePreviewLabel = new System.Windows.Forms.Label();
            this.folderBrowserDialog_reportPath = new System.Windows.Forms.FolderBrowserDialog();
            this.comboBox_reportPath = new System.Windows.Forms.ComboBox();
            this.groupBox_sequenceDefaultProperties.SuspendLayout();
            this.tabControl_SequencedefaultSettings.SuspendLayout();
            this.tabPage_test.SuspendLayout();
            this.tabPage_action.SuspendLayout();
            this.tabPage_sequenceCall.SuspendLayout();
            this.tabControl_settings.SuspendLayout();
            this.tabPage_sequenceSettings.SuspendLayout();
            this.tabPage_Report.SuspendLayout();
            this.groupBox_reportName.SuspendLayout();
            this.SuspendLayout();
            // 
            // label_testRecordResult
            // 
            this.label_testRecordResult.AutoSize = true;
            this.label_testRecordResult.Location = new System.Drawing.Point(21, 29);
            this.label_testRecordResult.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_testRecordResult.Name = "label_testRecordResult";
            this.label_testRecordResult.Size = new System.Drawing.Size(119, 15);
            this.label_testRecordResult.TabIndex = 0;
            this.label_testRecordResult.Text = "Record Result:";
            // 
            // groupBox_sequenceDefaultProperties
            // 
            this.groupBox_sequenceDefaultProperties.Controls.Add(this.tabControl_SequencedefaultSettings);
            this.groupBox_sequenceDefaultProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_sequenceDefaultProperties.Location = new System.Drawing.Point(4, 4);
            this.groupBox_sequenceDefaultProperties.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox_sequenceDefaultProperties.Name = "groupBox_sequenceDefaultProperties";
            this.groupBox_sequenceDefaultProperties.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox_sequenceDefaultProperties.Size = new System.Drawing.Size(716, 348);
            this.groupBox_sequenceDefaultProperties.TabIndex = 1;
            this.groupBox_sequenceDefaultProperties.TabStop = false;
            // 
            // tabControl_SequencedefaultSettings
            // 
            this.tabControl_SequencedefaultSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl_SequencedefaultSettings.Controls.Add(this.tabPage_test);
            this.tabControl_SequencedefaultSettings.Controls.Add(this.tabPage_action);
            this.tabControl_SequencedefaultSettings.Controls.Add(this.tabPage_sequenceCall);
            this.tabControl_SequencedefaultSettings.Location = new System.Drawing.Point(17, 25);
            this.tabControl_SequencedefaultSettings.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabControl_SequencedefaultSettings.Name = "tabControl_SequencedefaultSettings";
            this.tabControl_SequencedefaultSettings.SelectedIndex = 0;
            this.tabControl_SequencedefaultSettings.Size = new System.Drawing.Size(682, 315);
            this.tabControl_SequencedefaultSettings.TabIndex = 0;
            // 
            // tabPage_test
            // 
            this.tabPage_test.Controls.Add(this.comboBox_testBreakIfFailed);
            this.tabPage_test.Controls.Add(this.label_testBreakIfFailed);
            this.tabPage_test.Controls.Add(this.comboBox_testRecordResult);
            this.tabPage_test.Controls.Add(this.label_testRecordResult);
            this.tabPage_test.Location = new System.Drawing.Point(4, 25);
            this.tabPage_test.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage_test.Name = "tabPage_test";
            this.tabPage_test.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage_test.Size = new System.Drawing.Size(674, 286);
            this.tabPage_test.TabIndex = 1;
            this.tabPage_test.Text = "Test";
            this.tabPage_test.UseVisualStyleBackColor = true;
            // 
            // comboBox_testBreakIfFailed
            // 
            this.comboBox_testBreakIfFailed.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_testBreakIfFailed.FormattingEnabled = true;
            this.comboBox_testBreakIfFailed.Items.AddRange(new object[] {
            "True",
            "False"});
            this.comboBox_testBreakIfFailed.Location = new System.Drawing.Point(480, 25);
            this.comboBox_testBreakIfFailed.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.comboBox_testBreakIfFailed.Name = "comboBox_testBreakIfFailed";
            this.comboBox_testBreakIfFailed.Size = new System.Drawing.Size(143, 23);
            this.comboBox_testBreakIfFailed.TabIndex = 3;
            // 
            // label_testBreakIfFailed
            // 
            this.label_testBreakIfFailed.AutoSize = true;
            this.label_testBreakIfFailed.Location = new System.Drawing.Point(337, 29);
            this.label_testBreakIfFailed.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_testBreakIfFailed.Name = "label_testBreakIfFailed";
            this.label_testBreakIfFailed.Size = new System.Drawing.Size(135, 15);
            this.label_testBreakIfFailed.TabIndex = 2;
            this.label_testBreakIfFailed.Text = "Break If Failed:";
            // 
            // comboBox_testRecordResult
            // 
            this.comboBox_testRecordResult.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_testRecordResult.FormattingEnabled = true;
            this.comboBox_testRecordResult.Items.AddRange(new object[] {
            "True",
            "False"});
            this.comboBox_testRecordResult.Location = new System.Drawing.Point(148, 25);
            this.comboBox_testRecordResult.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.comboBox_testRecordResult.Name = "comboBox_testRecordResult";
            this.comboBox_testRecordResult.Size = new System.Drawing.Size(143, 23);
            this.comboBox_testRecordResult.TabIndex = 1;
            // 
            // tabPage_action
            // 
            this.tabPage_action.Controls.Add(this.comboBox_actionBreakIfFailed);
            this.tabPage_action.Controls.Add(this.label_actionBreakIfFailed);
            this.tabPage_action.Controls.Add(this.comboBox_actionRecordResult);
            this.tabPage_action.Controls.Add(this.label_actionRecordResult);
            this.tabPage_action.Location = new System.Drawing.Point(4, 25);
            this.tabPage_action.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage_action.Name = "tabPage_action";
            this.tabPage_action.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage_action.Size = new System.Drawing.Size(671, 283);
            this.tabPage_action.TabIndex = 0;
            this.tabPage_action.Text = "Action";
            this.tabPage_action.UseVisualStyleBackColor = true;
            // 
            // comboBox_actionBreakIfFailed
            // 
            this.comboBox_actionBreakIfFailed.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_actionBreakIfFailed.FormattingEnabled = true;
            this.comboBox_actionBreakIfFailed.Items.AddRange(new object[] {
            "True",
            "False"});
            this.comboBox_actionBreakIfFailed.Location = new System.Drawing.Point(480, 25);
            this.comboBox_actionBreakIfFailed.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.comboBox_actionBreakIfFailed.Name = "comboBox_actionBreakIfFailed";
            this.comboBox_actionBreakIfFailed.Size = new System.Drawing.Size(143, 23);
            this.comboBox_actionBreakIfFailed.TabIndex = 11;
            // 
            // label_actionBreakIfFailed
            // 
            this.label_actionBreakIfFailed.AutoSize = true;
            this.label_actionBreakIfFailed.Location = new System.Drawing.Point(337, 29);
            this.label_actionBreakIfFailed.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_actionBreakIfFailed.Name = "label_actionBreakIfFailed";
            this.label_actionBreakIfFailed.Size = new System.Drawing.Size(135, 15);
            this.label_actionBreakIfFailed.TabIndex = 10;
            this.label_actionBreakIfFailed.Text = "Break If Failed:";
            // 
            // comboBox_actionRecordResult
            // 
            this.comboBox_actionRecordResult.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_actionRecordResult.FormattingEnabled = true;
            this.comboBox_actionRecordResult.Items.AddRange(new object[] {
            "True",
            "False"});
            this.comboBox_actionRecordResult.Location = new System.Drawing.Point(148, 25);
            this.comboBox_actionRecordResult.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.comboBox_actionRecordResult.Name = "comboBox_actionRecordResult";
            this.comboBox_actionRecordResult.Size = new System.Drawing.Size(143, 23);
            this.comboBox_actionRecordResult.TabIndex = 9;
            // 
            // label_actionRecordResult
            // 
            this.label_actionRecordResult.AutoSize = true;
            this.label_actionRecordResult.Location = new System.Drawing.Point(21, 29);
            this.label_actionRecordResult.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_actionRecordResult.Name = "label_actionRecordResult";
            this.label_actionRecordResult.Size = new System.Drawing.Size(119, 15);
            this.label_actionRecordResult.TabIndex = 8;
            this.label_actionRecordResult.Text = "Record Result:";
            // 
            // tabPage_sequenceCall
            // 
            this.tabPage_sequenceCall.Controls.Add(this.comboBox_seqCallBreakIfFailed);
            this.tabPage_sequenceCall.Controls.Add(this.label_seqCallBreakIfFailed);
            this.tabPage_sequenceCall.Controls.Add(this.comboBox_seqCallRecordResult);
            this.tabPage_sequenceCall.Controls.Add(this.label_seqCallRecordResult);
            this.tabPage_sequenceCall.Location = new System.Drawing.Point(4, 25);
            this.tabPage_sequenceCall.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage_sequenceCall.Name = "tabPage_sequenceCall";
            this.tabPage_sequenceCall.Size = new System.Drawing.Size(671, 283);
            this.tabPage_sequenceCall.TabIndex = 2;
            this.tabPage_sequenceCall.Text = "Sequence Call";
            this.tabPage_sequenceCall.UseVisualStyleBackColor = true;
            // 
            // comboBox_seqCallBreakIfFailed
            // 
            this.comboBox_seqCallBreakIfFailed.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_seqCallBreakIfFailed.FormattingEnabled = true;
            this.comboBox_seqCallBreakIfFailed.Items.AddRange(new object[] {
            "True",
            "False"});
            this.comboBox_seqCallBreakIfFailed.Location = new System.Drawing.Point(480, 25);
            this.comboBox_seqCallBreakIfFailed.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.comboBox_seqCallBreakIfFailed.Name = "comboBox_seqCallBreakIfFailed";
            this.comboBox_seqCallBreakIfFailed.Size = new System.Drawing.Size(143, 23);
            this.comboBox_seqCallBreakIfFailed.TabIndex = 11;
            // 
            // label_seqCallBreakIfFailed
            // 
            this.label_seqCallBreakIfFailed.AutoSize = true;
            this.label_seqCallBreakIfFailed.Location = new System.Drawing.Point(337, 29);
            this.label_seqCallBreakIfFailed.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_seqCallBreakIfFailed.Name = "label_seqCallBreakIfFailed";
            this.label_seqCallBreakIfFailed.Size = new System.Drawing.Size(135, 15);
            this.label_seqCallBreakIfFailed.TabIndex = 10;
            this.label_seqCallBreakIfFailed.Text = "Break If Failed:";
            // 
            // comboBox_seqCallRecordResult
            // 
            this.comboBox_seqCallRecordResult.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_seqCallRecordResult.FormattingEnabled = true;
            this.comboBox_seqCallRecordResult.Items.AddRange(new object[] {
            "True",
            "False"});
            this.comboBox_seqCallRecordResult.Location = new System.Drawing.Point(148, 25);
            this.comboBox_seqCallRecordResult.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.comboBox_seqCallRecordResult.Name = "comboBox_seqCallRecordResult";
            this.comboBox_seqCallRecordResult.Size = new System.Drawing.Size(143, 23);
            this.comboBox_seqCallRecordResult.TabIndex = 9;
            // 
            // label_seqCallRecordResult
            // 
            this.label_seqCallRecordResult.AutoSize = true;
            this.label_seqCallRecordResult.Location = new System.Drawing.Point(21, 29);
            this.label_seqCallRecordResult.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_seqCallRecordResult.Name = "label_seqCallRecordResult";
            this.label_seqCallRecordResult.Size = new System.Drawing.Size(119, 15);
            this.label_seqCallRecordResult.TabIndex = 8;
            this.label_seqCallRecordResult.Text = "Record Result:";
            // 
            // button_apply
            // 
            this.button_apply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_apply.Location = new System.Drawing.Point(476, 398);
            this.button_apply.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button_apply.Name = "button_apply";
            this.button_apply.Size = new System.Drawing.Size(100, 29);
            this.button_apply.TabIndex = 2;
            this.button_apply.Text = "Apply";
            this.button_apply.UseVisualStyleBackColor = true;
            this.button_apply.Click += new System.EventHandler(this.button_apply_Click);
            // 
            // button_cancel
            // 
            this.button_cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_cancel.Location = new System.Drawing.Point(613, 398);
            this.button_cancel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(100, 29);
            this.button_cancel.TabIndex = 3;
            this.button_cancel.Text = "Cancel";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // tabControl_settings
            // 
            this.tabControl_settings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl_settings.Controls.Add(this.tabPage_sequenceSettings);
            this.tabControl_settings.Controls.Add(this.tabPage_Report);
            this.tabControl_settings.Location = new System.Drawing.Point(7, 5);
            this.tabControl_settings.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabControl_settings.Name = "tabControl_settings";
            this.tabControl_settings.SelectedIndex = 0;
            this.tabControl_settings.Size = new System.Drawing.Size(732, 385);
            this.tabControl_settings.TabIndex = 4;
            // 
            // tabPage_sequenceSettings
            // 
            this.tabPage_sequenceSettings.Controls.Add(this.groupBox_sequenceDefaultProperties);
            this.tabPage_sequenceSettings.Location = new System.Drawing.Point(4, 25);
            this.tabPage_sequenceSettings.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage_sequenceSettings.Name = "tabPage_sequenceSettings";
            this.tabPage_sequenceSettings.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage_sequenceSettings.Size = new System.Drawing.Size(724, 356);
            this.tabPage_sequenceSettings.TabIndex = 0;
            this.tabPage_sequenceSettings.Text = "Sequence Settings";
            this.tabPage_sequenceSettings.UseVisualStyleBackColor = true;
            // 
            // tabPage_Report
            // 
            this.tabPage_Report.Controls.Add(this.comboBox_reportPath);
            this.tabPage_Report.Controls.Add(this.button_selectReportPath);
            this.tabPage_Report.Controls.Add(this.label_reportPath);
            this.tabPage_Report.Controls.Add(this.groupBox_reportName);
            this.tabPage_Report.Location = new System.Drawing.Point(4, 25);
            this.tabPage_Report.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage_Report.Name = "tabPage_Report";
            this.tabPage_Report.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage_Report.Size = new System.Drawing.Size(724, 356);
            this.tabPage_Report.TabIndex = 1;
            this.tabPage_Report.Text = "Report";
            this.tabPage_Report.UseVisualStyleBackColor = true;
            // 
            // button_selectReportPath
            // 
            this.button_selectReportPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_selectReportPath.Location = new System.Drawing.Point(617, 306);
            this.button_selectReportPath.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button_selectReportPath.Name = "button_selectReportPath";
            this.button_selectReportPath.Size = new System.Drawing.Size(84, 29);
            this.button_selectReportPath.TabIndex = 3;
            this.button_selectReportPath.Text = "Select";
            this.button_selectReportPath.UseVisualStyleBackColor = true;
            this.button_selectReportPath.Click += new System.EventHandler(this.button_selectReportPath_Click);
            // 
            // label_reportPath
            // 
            this.label_reportPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label_reportPath.AutoSize = true;
            this.label_reportPath.Location = new System.Drawing.Point(24, 312);
            this.label_reportPath.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_reportPath.Name = "label_reportPath";
            this.label_reportPath.Size = new System.Drawing.Size(103, 15);
            this.label_reportPath.TabIndex = 1;
            this.label_reportPath.Text = "Report Path:";
            // 
            // groupBox_reportName
            // 
            this.groupBox_reportName.Controls.Add(this.label_baseName);
            this.groupBox_reportName.Controls.Add(this.textBox_baseName);
            this.groupBox_reportName.Controls.Add(this.textbox_reportNamePreview);
            this.groupBox_reportName.Controls.Add(this.button_removeNameElem);
            this.groupBox_reportName.Controls.Add(this.button_addNameElem);
            this.groupBox_reportName.Controls.Add(this.listBox_nameElement);
            this.groupBox_reportName.Controls.Add(this.textBox_reportNameFormat);
            this.groupBox_reportName.Controls.Add(this.label_reportNameFormatLabel);
            this.groupBox_reportName.Controls.Add(this.label_reportNamePreviewLabel);
            this.groupBox_reportName.Location = new System.Drawing.Point(24, 20);
            this.groupBox_reportName.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox_reportName.Name = "groupBox_reportName";
            this.groupBox_reportName.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox_reportName.Size = new System.Drawing.Size(677, 279);
            this.groupBox_reportName.TabIndex = 0;
            this.groupBox_reportName.TabStop = false;
            this.groupBox_reportName.Text = "Report Name";
            // 
            // label_baseName
            // 
            this.label_baseName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label_baseName.AutoSize = true;
            this.label_baseName.Location = new System.Drawing.Point(8, 164);
            this.label_baseName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_baseName.Name = "label_baseName";
            this.label_baseName.Size = new System.Drawing.Size(87, 15);
            this.label_baseName.TabIndex = 11;
            this.label_baseName.Text = "Base Name:";
            // 
            // textBox_baseName
            // 
            this.textBox_baseName.Location = new System.Drawing.Point(184, 160);
            this.textBox_baseName.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBox_baseName.Name = "textBox_baseName";
            this.textBox_baseName.Size = new System.Drawing.Size(465, 25);
            this.textBox_baseName.TabIndex = 10;
            this.textBox_baseName.TextChanged += new System.EventHandler(this.textBox_reportNameFormat_TextChanged);
            // 
            // textbox_reportNamePreview
            // 
            this.textbox_reportNamePreview.BackColor = System.Drawing.SystemColors.Window;
            this.textbox_reportNamePreview.Location = new System.Drawing.Point(184, 236);
            this.textbox_reportNamePreview.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textbox_reportNamePreview.Name = "textbox_reportNamePreview";
            this.textbox_reportNamePreview.ReadOnly = true;
            this.textbox_reportNamePreview.Size = new System.Drawing.Size(465, 25);
            this.textbox_reportNamePreview.TabIndex = 9;
            // 
            // button_removeNameElem
            // 
            this.button_removeNameElem.Location = new System.Drawing.Point(479, 106);
            this.button_removeNameElem.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button_removeNameElem.Name = "button_removeNameElem";
            this.button_removeNameElem.Size = new System.Drawing.Size(172, 29);
            this.button_removeNameElem.TabIndex = 8;
            this.button_removeNameElem.Text = "Remove From Name";
            this.button_removeNameElem.UseVisualStyleBackColor = true;
            this.button_removeNameElem.Click += new System.EventHandler(this.button_removeNameElem_Click);
            // 
            // button_addNameElem
            // 
            this.button_addNameElem.Location = new System.Drawing.Point(479, 44);
            this.button_addNameElem.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button_addNameElem.Name = "button_addNameElem";
            this.button_addNameElem.Size = new System.Drawing.Size(172, 29);
            this.button_addNameElem.TabIndex = 5;
            this.button_addNameElem.Text = "Add To Name";
            this.button_addNameElem.UseVisualStyleBackColor = true;
            this.button_addNameElem.Click += new System.EventHandler(this.button_addNameElem_Click);
            // 
            // listBox_nameElement
            // 
            this.listBox_nameElement.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.listBox_nameElement.FormattingEnabled = true;
            this.listBox_nameElement.ItemHeight = 12;
            this.listBox_nameElement.Location = new System.Drawing.Point(29, 26);
            this.listBox_nameElement.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.listBox_nameElement.Name = "listBox_nameElement";
            this.listBox_nameElement.Size = new System.Drawing.Size(440, 124);
            this.listBox_nameElement.TabIndex = 7;
            this.listBox_nameElement.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.listBox_nameElement_DrawItem);
            this.listBox_nameElement.DoubleClick += new System.EventHandler(this.listBox_nameElement_DoubleClick);
            // 
            // textBox_reportNameFormat
            // 
            this.textBox_reportNameFormat.Location = new System.Drawing.Point(184, 196);
            this.textBox_reportNameFormat.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBox_reportNameFormat.Name = "textBox_reportNameFormat";
            this.textBox_reportNameFormat.Size = new System.Drawing.Size(465, 25);
            this.textBox_reportNameFormat.TabIndex = 6;
            this.textBox_reportNameFormat.TextChanged += new System.EventHandler(this.textBox_reportNameFormat_TextChanged);
            // 
            // label_reportNameFormatLabel
            // 
            this.label_reportNameFormatLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label_reportNameFormatLabel.AutoSize = true;
            this.label_reportNameFormatLabel.Location = new System.Drawing.Point(8, 201);
            this.label_reportNameFormatLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_reportNameFormatLabel.Name = "label_reportNameFormatLabel";
            this.label_reportNameFormatLabel.Size = new System.Drawing.Size(159, 15);
            this.label_reportNameFormatLabel.TabIndex = 5;
            this.label_reportNameFormatLabel.Text = "Report Name Format:";
            // 
            // label_reportNamePreviewLabel
            // 
            this.label_reportNamePreviewLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label_reportNamePreviewLabel.AutoSize = true;
            this.label_reportNamePreviewLabel.Location = new System.Drawing.Point(8, 241);
            this.label_reportNamePreviewLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_reportNamePreviewLabel.Name = "label_reportNamePreviewLabel";
            this.label_reportNamePreviewLabel.Size = new System.Drawing.Size(167, 15);
            this.label_reportNamePreviewLabel.TabIndex = 3;
            this.label_reportNamePreviewLabel.Text = "Report Name Preview:";
            // 
            // comboBox_reportPath
            // 
            this.comboBox_reportPath.FormattingEnabled = true;
            this.comboBox_reportPath.Location = new System.Drawing.Point(135, 308);
            this.comboBox_reportPath.Name = "comboBox_reportPath";
            this.comboBox_reportPath.Size = new System.Drawing.Size(475, 23);
            this.comboBox_reportPath.TabIndex = 4;
            // 
            // ConfigureForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(740, 441);
            this.Controls.Add(this.tabControl_settings);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_apply);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "ConfigureForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Configuration";
            this.Load += new System.EventHandler(this.ConfigureForm_Load);
            this.groupBox_sequenceDefaultProperties.ResumeLayout(false);
            this.tabControl_SequencedefaultSettings.ResumeLayout(false);
            this.tabPage_test.ResumeLayout(false);
            this.tabPage_test.PerformLayout();
            this.tabPage_action.ResumeLayout(false);
            this.tabPage_action.PerformLayout();
            this.tabPage_sequenceCall.ResumeLayout(false);
            this.tabPage_sequenceCall.PerformLayout();
            this.tabControl_settings.ResumeLayout(false);
            this.tabPage_sequenceSettings.ResumeLayout(false);
            this.tabPage_Report.ResumeLayout(false);
            this.tabPage_Report.PerformLayout();
            this.groupBox_reportName.ResumeLayout(false);
            this.groupBox_reportName.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label_testRecordResult;
        private System.Windows.Forms.GroupBox groupBox_sequenceDefaultProperties;
        private System.Windows.Forms.ComboBox comboBox_testRecordResult;
        private System.Windows.Forms.TabControl tabControl_SequencedefaultSettings;
        private System.Windows.Forms.TabPage tabPage_action;
        private System.Windows.Forms.TabPage tabPage_test;
        private System.Windows.Forms.TabPage tabPage_sequenceCall;
        private System.Windows.Forms.ComboBox comboBox_testBreakIfFailed;
        private System.Windows.Forms.Label label_testBreakIfFailed;
        private System.Windows.Forms.ComboBox comboBox_actionBreakIfFailed;
        private System.Windows.Forms.Label label_actionBreakIfFailed;
        private System.Windows.Forms.ComboBox comboBox_actionRecordResult;
        private System.Windows.Forms.Label label_actionRecordResult;
        private System.Windows.Forms.ComboBox comboBox_seqCallBreakIfFailed;
        private System.Windows.Forms.Label label_seqCallBreakIfFailed;
        private System.Windows.Forms.ComboBox comboBox_seqCallRecordResult;
        private System.Windows.Forms.Label label_seqCallRecordResult;
        private System.Windows.Forms.Button button_apply;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.TabControl tabControl_settings;
        private System.Windows.Forms.TabPage tabPage_sequenceSettings;
        private System.Windows.Forms.TabPage tabPage_Report;
        private System.Windows.Forms.GroupBox groupBox_reportName;
        private System.Windows.Forms.Label label_reportPath;
        private System.Windows.Forms.Button button_selectReportPath;
        private System.Windows.Forms.Label label_reportNamePreviewLabel;
        private System.Windows.Forms.Label label_reportNameFormatLabel;
        private System.Windows.Forms.TextBox textBox_reportNameFormat;
        private System.Windows.Forms.ListBox listBox_nameElement;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog_reportPath;
        private System.Windows.Forms.Button button_addNameElem;
        private System.Windows.Forms.Button button_removeNameElem;
        private System.Windows.Forms.TextBox textbox_reportNamePreview;
        private System.Windows.Forms.Label label_baseName;
        private System.Windows.Forms.TextBox textBox_baseName;
        private System.Windows.Forms.ComboBox comboBox_reportPath;
    }
}