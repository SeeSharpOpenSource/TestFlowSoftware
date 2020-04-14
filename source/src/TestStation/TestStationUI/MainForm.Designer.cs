using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace TestStation
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码 && InitDGV()

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.menuStrip_ActionMenu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveAsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addSequenceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addStepToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.passFailTestToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.numericLimitTestToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stringValueTestToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.actionToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.sequenceCallToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.timingToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.startTimingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.endTimingToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.waitToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.editSequenceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadLibraryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configureToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.configureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectModelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.debugToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.startToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.suspendToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.userToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.managerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reloginToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip_QuickMenu = new System.Windows.Forms.ToolStrip();
            this.toolStripButton_New = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_Open = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_Save = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton_Run = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_Suspend = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_Stop = new System.Windows.Forms.ToolStripButton();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cMS_DgvStep = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.Menu_AddStep = new System.Windows.Forms.ToolStripMenuItem();
            this.actionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.actionToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.conditionBlockToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.conditionStatementToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.conditionLoopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sequenceCallToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.multiThreadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timingToolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.batchBlockToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gotoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tryFinallyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_insertStep = new System.Windows.Forms.ToolStripMenuItem();
            this.testToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.actionToolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.conditionBlockToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.conditionStatementToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.conditionLoopToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.sequenceCallToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.multiThreadToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.timerBlockToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.batchBlockToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.timingToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.gotoToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.tryFinallyBlockToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_DeleteStep = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl_settings = new System.Windows.Forms.TabControl();
            this.tabpage_Properties = new System.Windows.Forms.TabPage();
            this.flowLayoutPanel_stepProperties = new System.Windows.Forms.FlowLayoutPanel();
            this.groupBox_stepBehavior = new System.Windows.Forms.GroupBox();
            this.label_invokeFailedLabel = new System.Windows.Forms.Label();
            this.label_runType = new System.Windows.Forms.Label();
            this.comboBox_invokeFailedAction = new System.Windows.Forms.ComboBox();
            this.comboBox_runType = new System.Windows.Forms.ComboBox();
            this.label_assertFailedAction = new System.Windows.Forms.Label();
            this.checkBox_RecordStatus = new System.Windows.Forms.CheckBox();
            this.comboBox_asserFailedAction = new System.Windows.Forms.ComboBox();
            this.groupBox_loopProperties = new System.Windows.Forms.GroupBox();
            this.numericUpDown_passTimes = new System.Windows.Forms.NumericUpDown();
            this.label_passTimes = new System.Windows.Forms.Label();
            this.numericUpDown_retryTime = new System.Windows.Forms.NumericUpDown();
            this.label_retryTime = new System.Windows.Forms.Label();
            this.label_loopTypeConfig = new System.Windows.Forms.Label();
            this.LoopTimesnumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.LoopTypecomboBox = new System.Windows.Forms.ComboBox();
            this.label_loopTimesConfig = new System.Windows.Forms.Label();
            this.tabpage_Module = new System.Windows.Forms.TabPage();
            this.button_selectAssembly = new System.Windows.Forms.Button();
            this.comboBox_assembly = new System.Windows.Forms.ComboBox();
            this.comboBox_Constructor = new System.Windows.Forms.ComboBox();
            this.label_InstanceConfig = new System.Windows.Forms.Label();
            this.Parameter_panel = new System.Windows.Forms.Panel();
            this.comboBox_Method = new System.Windows.Forms.ComboBox();
            this.comboBox_RootClass = new System.Windows.Forms.ComboBox();
            this.label_methodConfig = new System.Windows.Forms.Label();
            this.label_ClassConfig = new System.Windows.Forms.Label();
            this.label_assemblyConfig = new System.Windows.Forms.Label();
            this.tabPage_runtimeInfo = new System.Windows.Forms.TabPage();
            this.splitContainer_runtime = new System.Windows.Forms.SplitContainer();
            this.button_deleteWatch = new System.Windows.Forms.Button();
            this.button_addWatch = new System.Windows.Forms.Button();
            this.dataGridView_variableValues = new System.Windows.Forms.DataGridView();
            this.Column_VariableName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_VariableValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label_variableValues = new System.Windows.Forms.Label();
            this.label_ouotput = new System.Windows.Forms.Label();
            this.button_clearOutput = new System.Windows.Forms.Button();
            this.button_copyOutput = new System.Windows.Forms.Button();
            this.textBox_output = new System.Windows.Forms.TextBox();
            this.tabCon_Step = new System.Windows.Forms.TabControl();
            this.tabPage_stepData = new System.Windows.Forms.TabPage();
            this.treeView_stepView = new System.Windows.Forms.TreeView();
            this.RuntimeStatusTab = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.textBoxReport = new System.Windows.Forms.TextBox();
            this.panel_buttonPanel = new System.Windows.Forms.Panel();
            this.button_openReportDir = new System.Windows.Forms.Button();
            this.buttonOpenReport = new System.Windows.Forms.Button();
            this.statusStripButton = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusUseValue = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel_userGroupLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel_userGroup = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusMsg = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusDataL = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusDate = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusTimeL = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel_stateLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel_stateValue = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar_progress = new System.Windows.Forms.ToolStripProgressBar();
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.labelTestRunning = new System.Windows.Forms.Label();
            this.labelTestGen = new System.Windows.Forms.Label();
            this.labelProject = new System.Windows.Forms.Label();
            this.splitContainer_MainFrame = new System.Windows.Forms.SplitContainer();
            this.splitContainer_sequenceAndVar = new System.Windows.Forms.SplitContainer();
            this.tabCon_Seq = new System.Windows.Forms.TabControl();
            this.tabPage_mainSequence = new System.Windows.Forms.TabPage();
            this.treeView_sequenceTree = new System.Windows.Forms.TreeView();
            this.tabCon_Variable = new System.Windows.Forms.TabControl();
            this.globalVariableTab = new System.Windows.Forms.TabPage();
            this.splitContainer_StepConfig = new System.Windows.Forms.SplitContainer();
            this.cMS_DgvVariable = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.saveFileDialog_sequence = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog_sequence = new System.Windows.Forms.OpenFileDialog();
            this.viewController_Main = new SeeSharpTools.JY.GUI.ViewController(this.components);
            this.contextMenuStrip_varList = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openFileDialog_assembly = new System.Windows.Forms.OpenFileDialog();
            this.mainFormBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.contextMenuStrip_sequence = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addSequenceGroupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.insertSequenceGroupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteSequenceGroupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addSequenceToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.insertSequenceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteSequenceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameSequenceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip_ActionMenu.SuspendLayout();
            this.toolStrip_QuickMenu.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.cMS_DgvStep.SuspendLayout();
            this.tabControl_settings.SuspendLayout();
            this.tabpage_Properties.SuspendLayout();
            this.flowLayoutPanel_stepProperties.SuspendLayout();
            this.groupBox_stepBehavior.SuspendLayout();
            this.groupBox_loopProperties.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_passTimes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_retryTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LoopTimesnumericUpDown)).BeginInit();
            this.tabpage_Module.SuspendLayout();
            this.tabPage_runtimeInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_runtime)).BeginInit();
            this.splitContainer_runtime.Panel1.SuspendLayout();
            this.splitContainer_runtime.Panel2.SuspendLayout();
            this.splitContainer_runtime.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_variableValues)).BeginInit();
            this.tabCon_Step.SuspendLayout();
            this.tabPage_stepData.SuspendLayout();
            this.RuntimeStatusTab.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel_buttonPanel.SuspendLayout();
            this.statusStripButton.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_MainFrame)).BeginInit();
            this.splitContainer_MainFrame.Panel1.SuspendLayout();
            this.splitContainer_MainFrame.Panel2.SuspendLayout();
            this.splitContainer_MainFrame.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_sequenceAndVar)).BeginInit();
            this.splitContainer_sequenceAndVar.Panel1.SuspendLayout();
            this.splitContainer_sequenceAndVar.Panel2.SuspendLayout();
            this.splitContainer_sequenceAndVar.SuspendLayout();
            this.tabCon_Seq.SuspendLayout();
            this.tabPage_mainSequence.SuspendLayout();
            this.tabCon_Variable.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_StepConfig)).BeginInit();
            this.splitContainer_StepConfig.Panel1.SuspendLayout();
            this.splitContainer_StepConfig.Panel2.SuspendLayout();
            this.splitContainer_StepConfig.SuspendLayout();
            this.cMS_DgvVariable.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainFormBindingSource)).BeginInit();
            this.contextMenuStrip_sequence.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip_ActionMenu
            // 
            this.menuStrip_ActionMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip_ActionMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem1,
            this.editToolStripMenuItem,
            this.configureToolStripMenuItem1,
            this.debugToolStripMenuItem1,
            this.userToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.menuStrip_ActionMenu.Location = new System.Drawing.Point(0, 0);
            this.menuStrip_ActionMenu.Name = "menuStrip_ActionMenu";
            this.menuStrip_ActionMenu.Size = new System.Drawing.Size(1284, 25);
            this.menuStrip_ActionMenu.TabIndex = 1;
            this.menuStrip_ActionMenu.Text = "menuStrip2";
            // 
            // fileToolStripMenuItem1
            // 
            this.fileToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem1,
            this.openToolStripMenuItem1,
            this.saveToolStripMenuItem1,
            this.SaveAsToolStripMenuItem1,
            this.toolStripSeparator2,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem1.Name = "fileToolStripMenuItem1";
            this.fileToolStripMenuItem1.Size = new System.Drawing.Size(39, 21);
            this.fileToolStripMenuItem1.Text = "File";
            // 
            // newToolStripMenuItem1
            // 
            this.newToolStripMenuItem1.Name = "newToolStripMenuItem1";
            this.newToolStripMenuItem1.Size = new System.Drawing.Size(121, 22);
            this.newToolStripMenuItem1.Text = "New";
            this.newToolStripMenuItem1.Click += new System.EventHandler(this.newToolStripMenuItem1_Click);
            // 
            // openToolStripMenuItem1
            // 
            this.openToolStripMenuItem1.Name = "openToolStripMenuItem1";
            this.openToolStripMenuItem1.Size = new System.Drawing.Size(121, 22);
            this.openToolStripMenuItem1.Text = "Open";
            this.openToolStripMenuItem1.Click += new System.EventHandler(this.openToolStripMenuItem1_Click);
            // 
            // saveToolStripMenuItem1
            // 
            this.saveToolStripMenuItem1.Name = "saveToolStripMenuItem1";
            this.saveToolStripMenuItem1.Size = new System.Drawing.Size(121, 22);
            this.saveToolStripMenuItem1.Text = "Save";
            this.saveToolStripMenuItem1.Click += new System.EventHandler(this.saveToolStripMenuItem1_Click);
            // 
            // SaveAsToolStripMenuItem1
            // 
            this.SaveAsToolStripMenuItem1.Name = "SaveAsToolStripMenuItem1";
            this.SaveAsToolStripMenuItem1.Size = new System.Drawing.Size(121, 22);
            this.SaveAsToolStripMenuItem1.Text = "Save As";
            this.SaveAsToolStripMenuItem1.Click += new System.EventHandler(this.SaveAsToolStripMenuItem1_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(118, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addSequenceToolStripMenuItem,
            this.addStepToolStripMenuItem,
            this.editSequenceToolStripMenuItem,
            this.loadLibraryToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(42, 21);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // addSequenceToolStripMenuItem
            // 
            this.addSequenceToolStripMenuItem.Name = "addSequenceToolStripMenuItem";
            this.addSequenceToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.addSequenceToolStripMenuItem.Text = "Add Subsequence";
            this.addSequenceToolStripMenuItem.Click += new System.EventHandler(this.addSequenceToolStripMenuItem_Click);
            // 
            // addStepToolStripMenuItem
            // 
            this.addStepToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.testToolStripMenuItem,
            this.actionToolStripMenuItem2,
            this.sequenceCallToolStripMenuItem1,
            this.timingToolStripMenuItem2});
            this.addStepToolStripMenuItem.Name = "addStepToolStripMenuItem";
            this.addStepToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.addStepToolStripMenuItem.Text = "Add Step";
            // 
            // testToolStripMenuItem
            // 
            this.testToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.passFailTestToolStripMenuItem,
            this.numericLimitTestToolStripMenuItem,
            this.stringValueTestToolStripMenuItem});
            this.testToolStripMenuItem.Name = "testToolStripMenuItem";
            this.testToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.testToolStripMenuItem.Text = "Test";
            // 
            // passFailTestToolStripMenuItem
            // 
            this.passFailTestToolStripMenuItem.Name = "passFailTestToolStripMenuItem";
            this.passFailTestToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.passFailTestToolStripMenuItem.Text = "Boolean Test";
            this.passFailTestToolStripMenuItem.Click += new System.EventHandler(this.booleanTestToolStripMenuItem_Click);
            // 
            // numericLimitTestToolStripMenuItem
            // 
            this.numericLimitTestToolStripMenuItem.Name = "numericLimitTestToolStripMenuItem";
            this.numericLimitTestToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.numericLimitTestToolStripMenuItem.Text = "Numeric Limit Test";
            this.numericLimitTestToolStripMenuItem.Click += new System.EventHandler(this.numericLimitTestToolStripMenuItem1_Click);
            // 
            // stringValueTestToolStripMenuItem
            // 
            this.stringValueTestToolStripMenuItem.Name = "stringValueTestToolStripMenuItem";
            this.stringValueTestToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.stringValueTestToolStripMenuItem.Text = "String Value Test";
            this.stringValueTestToolStripMenuItem.Click += new System.EventHandler(this.stringValueTestToolStripMenuItem1_Click);
            // 
            // actionToolStripMenuItem2
            // 
            this.actionToolStripMenuItem2.Name = "actionToolStripMenuItem2";
            this.actionToolStripMenuItem2.Size = new System.Drawing.Size(157, 22);
            this.actionToolStripMenuItem2.Text = "Action";
            this.actionToolStripMenuItem2.Click += new System.EventHandler(this.actionToolStripMenuItem2_Click);
            // 
            // sequenceCallToolStripMenuItem1
            // 
            this.sequenceCallToolStripMenuItem1.Name = "sequenceCallToolStripMenuItem1";
            this.sequenceCallToolStripMenuItem1.Size = new System.Drawing.Size(157, 22);
            this.sequenceCallToolStripMenuItem1.Text = "Sequence Call";
            this.sequenceCallToolStripMenuItem1.Click += new System.EventHandler(this.sequenceCallToolStripMenuItem1_Click);
            // 
            // timingToolStripMenuItem2
            // 
            this.timingToolStripMenuItem2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startTimingToolStripMenuItem,
            this.endTimingToolStripMenuItem2,
            this.waitToolStripMenuItem2});
            this.timingToolStripMenuItem2.Name = "timingToolStripMenuItem2";
            this.timingToolStripMenuItem2.Size = new System.Drawing.Size(157, 22);
            this.timingToolStripMenuItem2.Text = "Timing";
            // 
            // startTimingToolStripMenuItem
            // 
            this.startTimingToolStripMenuItem.Name = "startTimingToolStripMenuItem";
            this.startTimingToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.startTimingToolStripMenuItem.Text = "Start Timing";
            this.startTimingToolStripMenuItem.Click += new System.EventHandler(this.startTimingToolStripMenuItem_Click);
            // 
            // endTimingToolStripMenuItem2
            // 
            this.endTimingToolStripMenuItem2.Name = "endTimingToolStripMenuItem2";
            this.endTimingToolStripMenuItem2.Size = new System.Drawing.Size(146, 22);
            this.endTimingToolStripMenuItem2.Text = "End Timing";
            this.endTimingToolStripMenuItem2.Click += new System.EventHandler(this.endTimingToolStripMenuItem_Click);
            // 
            // waitToolStripMenuItem2
            // 
            this.waitToolStripMenuItem2.Name = "waitToolStripMenuItem2";
            this.waitToolStripMenuItem2.Size = new System.Drawing.Size(146, 22);
            this.waitToolStripMenuItem2.Text = "Wait";
            this.waitToolStripMenuItem2.Click += new System.EventHandler(this.waitToolStripMenuItem_Click);
            // 
            // editSequenceToolStripMenuItem
            // 
            this.editSequenceToolStripMenuItem.Name = "editSequenceToolStripMenuItem";
            this.editSequenceToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.editSequenceToolStripMenuItem.Text = "Edit Sequence";
            this.editSequenceToolStripMenuItem.Click += new System.EventHandler(this.editSequenceToolStripMenuItem_Click);
            // 
            // loadLibraryToolStripMenuItem
            // 
            this.loadLibraryToolStripMenuItem.Name = "loadLibraryToolStripMenuItem";
            this.loadLibraryToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.loadLibraryToolStripMenuItem.Text = "Load Library";
            this.loadLibraryToolStripMenuItem.Click += new System.EventHandler(this.loadLibraryToolStripMenuItem_Click);
            // 
            // configureToolStripMenuItem1
            // 
            this.configureToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.configureToolStripMenuItem,
            this.selectModelToolStripMenuItem});
            this.configureToolStripMenuItem1.Name = "configureToolStripMenuItem1";
            this.configureToolStripMenuItem1.Size = new System.Drawing.Size(77, 21);
            this.configureToolStripMenuItem1.Text = "Configure";
            // 
            // configureToolStripMenuItem
            // 
            this.configureToolStripMenuItem.Name = "configureToolStripMenuItem";
            this.configureToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.configureToolStripMenuItem.Text = "Configure";
            this.configureToolStripMenuItem.Click += new System.EventHandler(this.configureToolStripMenuItem_Click);
            // 
            // selectModelToolStripMenuItem
            // 
            this.selectModelToolStripMenuItem.Name = "selectModelToolStripMenuItem";
            this.selectModelToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.selectModelToolStripMenuItem.Text = "Select Model";
            this.selectModelToolStripMenuItem.Click += new System.EventHandler(this.selectModelToolStripMenuItem_Click);
            // 
            // debugToolStripMenuItem1
            // 
            this.debugToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startToolStripMenuItem1,
            this.suspendToolStripMenuItem,
            this.stopToolStripMenuItem2});
            this.debugToolStripMenuItem1.Name = "debugToolStripMenuItem1";
            this.debugToolStripMenuItem1.Size = new System.Drawing.Size(42, 21);
            this.debugToolStripMenuItem1.Text = "Run";
            // 
            // startToolStripMenuItem1
            // 
            this.startToolStripMenuItem1.Image = global::TestStation.Properties.Resources.start;
            this.startToolStripMenuItem1.Name = "startToolStripMenuItem1";
            this.startToolStripMenuItem1.Size = new System.Drawing.Size(126, 22);
            this.startToolStripMenuItem1.Text = "Start";
            this.startToolStripMenuItem1.Click += new System.EventHandler(this.startToolStripMenuItem1_Click);
            // 
            // suspendToolStripMenuItem
            // 
            this.suspendToolStripMenuItem.Image = global::TestStation.Properties.Resources.suspend;
            this.suspendToolStripMenuItem.Name = "suspendToolStripMenuItem";
            this.suspendToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.suspendToolStripMenuItem.Text = "Suspend";
            this.suspendToolStripMenuItem.Click += new System.EventHandler(this.suspendToolStripMenuItem_Click);
            // 
            // stopToolStripMenuItem2
            // 
            this.stopToolStripMenuItem2.Image = global::TestStation.Properties.Resources.stop;
            this.stopToolStripMenuItem2.Name = "stopToolStripMenuItem2";
            this.stopToolStripMenuItem2.Size = new System.Drawing.Size(126, 22);
            this.stopToolStripMenuItem2.Text = "Stop";
            this.stopToolStripMenuItem2.Click += new System.EventHandler(this.stopToolStripMenuItem2_Click);
            // 
            // userToolStripMenuItem
            // 
            this.userToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.managerToolStripMenuItem,
            this.reloginToolStripMenuItem});
            this.userToolStripMenuItem.Name = "userToolStripMenuItem";
            this.userToolStripMenuItem.Size = new System.Drawing.Size(47, 21);
            this.userToolStripMenuItem.Text = "User";
            this.userToolStripMenuItem.Visible = false;
            // 
            // managerToolStripMenuItem
            // 
            this.managerToolStripMenuItem.Name = "managerToolStripMenuItem";
            this.managerToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.managerToolStripMenuItem.Text = "Manage";
            this.managerToolStripMenuItem.Click += new System.EventHandler(this.managerToolStripMenuItem_Click);
            // 
            // reloginToolStripMenuItem
            // 
            this.reloginToolStripMenuItem.Name = "reloginToolStripMenuItem";
            this.reloginToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.reloginToolStripMenuItem.Text = "Relogin";
            this.reloginToolStripMenuItem.Click += new System.EventHandler(this.reloginToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem1});
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(47, 21);
            this.aboutToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem1
            // 
            this.aboutToolStripMenuItem1.Name = "aboutToolStripMenuItem1";
            this.aboutToolStripMenuItem1.Size = new System.Drawing.Size(111, 22);
            this.aboutToolStripMenuItem1.Text = "About";
            this.aboutToolStripMenuItem1.Click += new System.EventHandler(this.aboutToolStripMenuItem1_Click);
            // 
            // toolStrip_QuickMenu
            // 
            this.toolStrip_QuickMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip_QuickMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_New,
            this.toolStripButton_Open,
            this.toolStripButton_Save,
            this.toolStripSeparator1,
            this.toolStripButton_Run,
            this.toolStripButton_Suspend,
            this.toolStripButton_Stop});
            this.toolStrip_QuickMenu.Location = new System.Drawing.Point(0, 25);
            this.toolStrip_QuickMenu.Name = "toolStrip_QuickMenu";
            this.toolStrip_QuickMenu.Size = new System.Drawing.Size(1284, 27);
            this.toolStrip_QuickMenu.TabIndex = 2;
            this.toolStrip_QuickMenu.Text = "toolStrip1";
            // 
            // toolStripButton_New
            // 
            this.toolStripButton_New.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_New.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_New.Image")));
            this.toolStripButton_New.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_New.Name = "toolStripButton_New";
            this.toolStripButton_New.Size = new System.Drawing.Size(24, 24);
            this.toolStripButton_New.Text = "New";
            this.toolStripButton_New.Click += new System.EventHandler(this.newToolStripMenuItem1_Click);
            // 
            // toolStripButton_Open
            // 
            this.toolStripButton_Open.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_Open.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_Open.Image")));
            this.toolStripButton_Open.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Open.Name = "toolStripButton_Open";
            this.toolStripButton_Open.Size = new System.Drawing.Size(24, 24);
            this.toolStripButton_Open.Text = "Open";
            this.toolStripButton_Open.Click += new System.EventHandler(this.openToolStripMenuItem1_Click);
            // 
            // toolStripButton_Save
            // 
            this.toolStripButton_Save.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_Save.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_Save.Image")));
            this.toolStripButton_Save.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Save.Name = "toolStripButton_Save";
            this.toolStripButton_Save.Size = new System.Drawing.Size(24, 24);
            this.toolStripButton_Save.Text = "Save";
            this.toolStripButton_Save.Click += new System.EventHandler(this.saveToolStripMenuItem1_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
            // 
            // toolStripButton_Run
            // 
            this.toolStripButton_Run.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_Run.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_Run.Image")));
            this.toolStripButton_Run.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Run.Name = "toolStripButton_Run";
            this.toolStripButton_Run.Size = new System.Drawing.Size(24, 24);
            this.toolStripButton_Run.Text = "Run";
            this.toolStripButton_Run.Click += new System.EventHandler(this.startButton_Click);
            // 
            // toolStripButton_Suspend
            // 
            this.toolStripButton_Suspend.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_Suspend.Image = global::TestStation.Properties.Resources.suspend;
            this.toolStripButton_Suspend.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Suspend.Name = "toolStripButton_Suspend";
            this.toolStripButton_Suspend.Size = new System.Drawing.Size(24, 24);
            this.toolStripButton_Suspend.Text = "Suspend";
            this.toolStripButton_Suspend.Click += new System.EventHandler(this.toolStripButton_Suspend_Click);
            // 
            // toolStripButton_Stop
            // 
            this.toolStripButton_Stop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_Stop.Image = global::TestStation.Properties.Resources.stop;
            this.toolStripButton_Stop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Stop.Name = "toolStripButton_Stop";
            this.toolStripButton_Stop.Size = new System.Drawing.Size(24, 24);
            this.toolStripButton_Stop.Text = "Stop";
            this.toolStripButton_Stop.Click += new System.EventHandler(this.toolStripButton6_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.closeToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(109, 26);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(108, 22);
            this.closeToolStripMenuItem.Text = "Close";
            // 
            // cMS_DgvStep
            // 
            this.cMS_DgvStep.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cMS_DgvStep.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_AddStep,
            this.ToolStripMenuItem_insertStep,
            this.Menu_DeleteStep,
            this.toolStripMenuItem3});
            this.cMS_DgvStep.Name = "cMS_DgvStep";
            this.cMS_DgvStep.Size = new System.Drawing.Size(154, 92);
            // 
            // Menu_AddStep
            // 
            this.Menu_AddStep.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.actionToolStripMenuItem,
            this.actionToolStripMenuItem1,
            this.sequenceCallToolStripMenuItem,
            this.timingToolStripMenuItem});
            this.Menu_AddStep.Name = "Menu_AddStep";
            this.Menu_AddStep.Size = new System.Drawing.Size(153, 22);
            this.Menu_AddStep.Text = "Add Step";
            // 
            // actionToolStripMenuItem
            // 
            this.actionToolStripMenuItem.Name = "actionToolStripMenuItem";
            this.actionToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.actionToolStripMenuItem.Tag = "Execution";
            this.actionToolStripMenuItem.Text = "Common";
            this.actionToolStripMenuItem.Click += new System.EventHandler(this.AddSequenceStep);
            // 
            // actionToolStripMenuItem1
            // 
            this.actionToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.conditionBlockToolStripMenuItem,
            this.conditionStatementToolStripMenuItem,
            this.conditionLoopToolStripMenuItem});
            this.actionToolStripMenuItem1.Name = "actionToolStripMenuItem1";
            this.actionToolStripMenuItem1.Size = new System.Drawing.Size(149, 22);
            this.actionToolStripMenuItem1.Text = "Condition";
            // 
            // conditionBlockToolStripMenuItem
            // 
            this.conditionBlockToolStripMenuItem.Name = "conditionBlockToolStripMenuItem";
            this.conditionBlockToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.conditionBlockToolStripMenuItem.Tag = "ConditionBlock";
            this.conditionBlockToolStripMenuItem.Text = "Condition Block";
            this.conditionBlockToolStripMenuItem.Click += new System.EventHandler(this.AddSequenceStep);
            // 
            // conditionStatementToolStripMenuItem
            // 
            this.conditionStatementToolStripMenuItem.Name = "conditionStatementToolStripMenuItem";
            this.conditionStatementToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.conditionStatementToolStripMenuItem.Tag = "ConditionStatement";
            this.conditionStatementToolStripMenuItem.Text = "Condition Statement";
            this.conditionStatementToolStripMenuItem.Click += new System.EventHandler(this.AddSequenceStep);
            // 
            // conditionLoopToolStripMenuItem
            // 
            this.conditionLoopToolStripMenuItem.Name = "conditionLoopToolStripMenuItem";
            this.conditionLoopToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.conditionLoopToolStripMenuItem.Tag = "ConditionLoop";
            this.conditionLoopToolStripMenuItem.Text = "Condition Loop";
            this.conditionLoopToolStripMenuItem.Click += new System.EventHandler(this.AddSequenceStep);
            // 
            // sequenceCallToolStripMenuItem
            // 
            this.sequenceCallToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.multiThreadToolStripMenuItem,
            this.timingToolStripMenuItem3,
            this.batchBlockToolStripMenuItem});
            this.sequenceCallToolStripMenuItem.Name = "sequenceCallToolStripMenuItem";
            this.sequenceCallToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.sequenceCallToolStripMenuItem.Text = "Parallel";
            // 
            // multiThreadToolStripMenuItem
            // 
            this.multiThreadToolStripMenuItem.Name = "multiThreadToolStripMenuItem";
            this.multiThreadToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.multiThreadToolStripMenuItem.Tag = "MultiThreadBlock";
            this.multiThreadToolStripMenuItem.Text = "Multi-Thread";
            this.multiThreadToolStripMenuItem.Click += new System.EventHandler(this.AddSequenceStep);
            // 
            // timingToolStripMenuItem3
            // 
            this.timingToolStripMenuItem3.Name = "timingToolStripMenuItem3";
            this.timingToolStripMenuItem3.Size = new System.Drawing.Size(151, 22);
            this.timingToolStripMenuItem3.Tag = "TimerBlock";
            this.timingToolStripMenuItem3.Text = "Timer Block";
            this.timingToolStripMenuItem3.Click += new System.EventHandler(this.AddSequenceStep);
            // 
            // batchBlockToolStripMenuItem
            // 
            this.batchBlockToolStripMenuItem.Name = "batchBlockToolStripMenuItem";
            this.batchBlockToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.batchBlockToolStripMenuItem.Tag = "BatchBlock";
            this.batchBlockToolStripMenuItem.Text = "Batch Block";
            this.batchBlockToolStripMenuItem.Click += new System.EventHandler(this.AddSequenceStep);
            // 
            // timingToolStripMenuItem
            // 
            this.timingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.gotoToolStripMenuItem,
            this.tryFinallyToolStripMenuItem});
            this.timingToolStripMenuItem.Name = "timingToolStripMenuItem";
            this.timingToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.timingToolStripMenuItem.Text = "Flow Control";
            // 
            // gotoToolStripMenuItem
            // 
            this.gotoToolStripMenuItem.Name = "gotoToolStripMenuItem";
            this.gotoToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.gotoToolStripMenuItem.Tag = "Goto";
            this.gotoToolStripMenuItem.Text = "Goto";
            this.gotoToolStripMenuItem.Click += new System.EventHandler(this.AddSequenceStep);
            // 
            // tryFinallyToolStripMenuItem
            // 
            this.tryFinallyToolStripMenuItem.Name = "tryFinallyToolStripMenuItem";
            this.tryFinallyToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.tryFinallyToolStripMenuItem.Tag = "TryFinallyBlock";
            this.tryFinallyToolStripMenuItem.Text = "Try-Finally";
            this.tryFinallyToolStripMenuItem.Click += new System.EventHandler(this.AddSequenceStep);
            // 
            // ToolStripMenuItem_insertStep
            // 
            this.ToolStripMenuItem_insertStep.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.testToolStripMenuItem1,
            this.actionToolStripMenuItem3,
            this.sequenceCallToolStripMenuItem2,
            this.timingToolStripMenuItem1});
            this.ToolStripMenuItem_insertStep.Name = "ToolStripMenuItem_insertStep";
            this.ToolStripMenuItem_insertStep.Size = new System.Drawing.Size(153, 22);
            this.ToolStripMenuItem_insertStep.Text = "Insert Step";
            // 
            // testToolStripMenuItem1
            // 
            this.testToolStripMenuItem1.Name = "testToolStripMenuItem1";
            this.testToolStripMenuItem1.Size = new System.Drawing.Size(149, 22);
            this.testToolStripMenuItem1.Tag = "Execution";
            this.testToolStripMenuItem1.Text = "Common";
            this.testToolStripMenuItem1.Click += new System.EventHandler(this.InsertSequenceStep);
            // 
            // actionToolStripMenuItem3
            // 
            this.actionToolStripMenuItem3.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.conditionBlockToolStripMenuItem1,
            this.conditionStatementToolStripMenuItem1,
            this.conditionLoopToolStripMenuItem1});
            this.actionToolStripMenuItem3.Name = "actionToolStripMenuItem3";
            this.actionToolStripMenuItem3.Size = new System.Drawing.Size(149, 22);
            this.actionToolStripMenuItem3.Text = "Condition";
            // 
            // conditionBlockToolStripMenuItem1
            // 
            this.conditionBlockToolStripMenuItem1.Name = "conditionBlockToolStripMenuItem1";
            this.conditionBlockToolStripMenuItem1.Size = new System.Drawing.Size(194, 22);
            this.conditionBlockToolStripMenuItem1.Tag = "ConditionBlock";
            this.conditionBlockToolStripMenuItem1.Text = "Condition Block";
            this.conditionBlockToolStripMenuItem1.Click += new System.EventHandler(this.InsertSequenceStep);
            // 
            // conditionStatementToolStripMenuItem1
            // 
            this.conditionStatementToolStripMenuItem1.Name = "conditionStatementToolStripMenuItem1";
            this.conditionStatementToolStripMenuItem1.Size = new System.Drawing.Size(194, 22);
            this.conditionStatementToolStripMenuItem1.Tag = "ConditionStatement";
            this.conditionStatementToolStripMenuItem1.Text = "Condition Statement";
            this.conditionStatementToolStripMenuItem1.Click += new System.EventHandler(this.InsertSequenceStep);
            // 
            // conditionLoopToolStripMenuItem1
            // 
            this.conditionLoopToolStripMenuItem1.Name = "conditionLoopToolStripMenuItem1";
            this.conditionLoopToolStripMenuItem1.Size = new System.Drawing.Size(194, 22);
            this.conditionLoopToolStripMenuItem1.Tag = "ConditionLoop";
            this.conditionLoopToolStripMenuItem1.Text = "Condition Loop";
            this.conditionLoopToolStripMenuItem1.Click += new System.EventHandler(this.InsertSequenceStep);
            // 
            // sequenceCallToolStripMenuItem2
            // 
            this.sequenceCallToolStripMenuItem2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.multiThreadToolStripMenuItem1,
            this.timerBlockToolStripMenuItem,
            this.batchBlockToolStripMenuItem1});
            this.sequenceCallToolStripMenuItem2.Name = "sequenceCallToolStripMenuItem2";
            this.sequenceCallToolStripMenuItem2.Size = new System.Drawing.Size(149, 22);
            this.sequenceCallToolStripMenuItem2.Text = "Parallel";
            this.sequenceCallToolStripMenuItem2.Visible = false;
            // 
            // multiThreadToolStripMenuItem1
            // 
            this.multiThreadToolStripMenuItem1.Name = "multiThreadToolStripMenuItem1";
            this.multiThreadToolStripMenuItem1.Size = new System.Drawing.Size(151, 22);
            this.multiThreadToolStripMenuItem1.Tag = "MultiThreadBlock";
            this.multiThreadToolStripMenuItem1.Text = "Multi-Thread";
            this.multiThreadToolStripMenuItem1.Click += new System.EventHandler(this.InsertSequenceStep);
            // 
            // timerBlockToolStripMenuItem
            // 
            this.timerBlockToolStripMenuItem.Name = "timerBlockToolStripMenuItem";
            this.timerBlockToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.timerBlockToolStripMenuItem.Tag = "TimerBlock";
            this.timerBlockToolStripMenuItem.Text = "Timer Block";
            this.timerBlockToolStripMenuItem.Click += new System.EventHandler(this.InsertSequenceStep);
            // 
            // batchBlockToolStripMenuItem1
            // 
            this.batchBlockToolStripMenuItem1.Name = "batchBlockToolStripMenuItem1";
            this.batchBlockToolStripMenuItem1.Size = new System.Drawing.Size(151, 22);
            this.batchBlockToolStripMenuItem1.Tag = "BatchBlock";
            this.batchBlockToolStripMenuItem1.Text = "Batch Block";
            this.batchBlockToolStripMenuItem1.Click += new System.EventHandler(this.InsertSequenceStep);
            // 
            // timingToolStripMenuItem1
            // 
            this.timingToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.gotoToolStripMenuItem1,
            this.tryFinallyBlockToolStripMenuItem});
            this.timingToolStripMenuItem1.Name = "timingToolStripMenuItem1";
            this.timingToolStripMenuItem1.Size = new System.Drawing.Size(149, 22);
            this.timingToolStripMenuItem1.Text = "Flow Control";
            // 
            // gotoToolStripMenuItem1
            // 
            this.gotoToolStripMenuItem1.Name = "gotoToolStripMenuItem1";
            this.gotoToolStripMenuItem1.Size = new System.Drawing.Size(134, 22);
            this.gotoToolStripMenuItem1.Tag = "Goto";
            this.gotoToolStripMenuItem1.Text = "Goto";
            this.gotoToolStripMenuItem1.Visible = false;
            this.gotoToolStripMenuItem1.Click += new System.EventHandler(this.InsertSequenceStep);
            // 
            // tryFinallyBlockToolStripMenuItem
            // 
            this.tryFinallyBlockToolStripMenuItem.Name = "tryFinallyBlockToolStripMenuItem";
            this.tryFinallyBlockToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.tryFinallyBlockToolStripMenuItem.Tag = "TryFinallyBlock";
            this.tryFinallyBlockToolStripMenuItem.Text = "Try-Finally";
            this.tryFinallyBlockToolStripMenuItem.Click += new System.EventHandler(this.InsertSequenceStep);
            // 
            // Menu_DeleteStep
            // 
            this.Menu_DeleteStep.Name = "Menu_DeleteStep";
            this.Menu_DeleteStep.Size = new System.Drawing.Size(153, 22);
            this.Menu_DeleteStep.Text = "Delete Step";
            this.Menu_DeleteStep.Click += new System.EventHandler(this.DeleteSequenceStep);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(153, 22);
            this.toolStripMenuItem3.Text = "Rename Step";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.toolStripMenuItem3_Click);
            // 
            // tabControl_settings
            // 
            this.tabControl_settings.Controls.Add(this.tabpage_Properties);
            this.tabControl_settings.Controls.Add(this.tabpage_Module);
            this.tabControl_settings.Controls.Add(this.tabPage_runtimeInfo);
            this.tabControl_settings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl_settings.Location = new System.Drawing.Point(0, 0);
            this.tabControl_settings.Name = "tabControl_settings";
            this.tabControl_settings.SelectedIndex = 0;
            this.tabControl_settings.Size = new System.Drawing.Size(916, 386);
            this.tabControl_settings.TabIndex = 5;
            this.tabControl_settings.SelectedIndexChanged += new System.EventHandler(this.tabControl_settings_SelectedIndexChanged);
            // 
            // tabpage_Properties
            // 
            this.tabpage_Properties.AutoScroll = true;
            this.tabpage_Properties.Controls.Add(this.flowLayoutPanel_stepProperties);
            this.tabpage_Properties.Location = new System.Drawing.Point(4, 22);
            this.tabpage_Properties.Name = "tabpage_Properties";
            this.tabpage_Properties.Padding = new System.Windows.Forms.Padding(3);
            this.tabpage_Properties.Size = new System.Drawing.Size(908, 360);
            this.tabpage_Properties.TabIndex = 0;
            this.tabpage_Properties.Text = "Properties";
            this.tabpage_Properties.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanel_stepProperties
            // 
            this.flowLayoutPanel_stepProperties.AutoScroll = true;
            this.flowLayoutPanel_stepProperties.Controls.Add(this.groupBox_stepBehavior);
            this.flowLayoutPanel_stepProperties.Controls.Add(this.groupBox_loopProperties);
            this.flowLayoutPanel_stepProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel_stepProperties.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel_stepProperties.Name = "flowLayoutPanel_stepProperties";
            this.flowLayoutPanel_stepProperties.Size = new System.Drawing.Size(902, 354);
            this.flowLayoutPanel_stepProperties.TabIndex = 14;
            // 
            // groupBox_stepBehavior
            // 
            this.groupBox_stepBehavior.Controls.Add(this.label_invokeFailedLabel);
            this.groupBox_stepBehavior.Controls.Add(this.label_runType);
            this.groupBox_stepBehavior.Controls.Add(this.comboBox_invokeFailedAction);
            this.groupBox_stepBehavior.Controls.Add(this.comboBox_runType);
            this.groupBox_stepBehavior.Controls.Add(this.label_assertFailedAction);
            this.groupBox_stepBehavior.Controls.Add(this.checkBox_RecordStatus);
            this.groupBox_stepBehavior.Controls.Add(this.comboBox_asserFailedAction);
            this.groupBox_stepBehavior.Location = new System.Drawing.Point(3, 3);
            this.groupBox_stepBehavior.Name = "groupBox_stepBehavior";
            this.groupBox_stepBehavior.Size = new System.Drawing.Size(387, 150);
            this.groupBox_stepBehavior.TabIndex = 13;
            this.groupBox_stepBehavior.TabStop = false;
            this.groupBox_stepBehavior.Text = "Behavior";
            // 
            // label_invokeFailedLabel
            // 
            this.label_invokeFailedLabel.AutoSize = true;
            this.label_invokeFailedLabel.Location = new System.Drawing.Point(201, 87);
            this.label_invokeFailedLabel.Name = "label_invokeFailedLabel";
            this.label_invokeFailedLabel.Size = new System.Drawing.Size(125, 12);
            this.label_invokeFailedLabel.TabIndex = 7;
            this.label_invokeFailedLabel.Text = "Invoke Error Action:";
            // 
            // label_runType
            // 
            this.label_runType.AutoSize = true;
            this.label_runType.Location = new System.Drawing.Point(25, 22);
            this.label_runType.Name = "label_runType";
            this.label_runType.Size = new System.Drawing.Size(59, 12);
            this.label_runType.TabIndex = 10;
            this.label_runType.Text = "Run Type:";
            // 
            // comboBox_invokeFailedAction
            // 
            this.comboBox_invokeFailedAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_invokeFailedAction.FormattingEnabled = true;
            this.comboBox_invokeFailedAction.Items.AddRange(new object[] {
            "Terminate",
            "Continue",
            "BreakLoop",
            "NextLoop"});
            this.comboBox_invokeFailedAction.Location = new System.Drawing.Point(203, 102);
            this.comboBox_invokeFailedAction.Name = "comboBox_invokeFailedAction";
            this.comboBox_invokeFailedAction.Size = new System.Drawing.Size(148, 20);
            this.comboBox_invokeFailedAction.TabIndex = 6;
            // 
            // comboBox_runType
            // 
            this.comboBox_runType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_runType.FormattingEnabled = true;
            this.comboBox_runType.Items.AddRange(new object[] {
            "Normal",
            "Skip",
            "ForceSuccess",
            "ForceFailed"});
            this.comboBox_runType.Location = new System.Drawing.Point(27, 41);
            this.comboBox_runType.Name = "comboBox_runType";
            this.comboBox_runType.Size = new System.Drawing.Size(121, 20);
            this.comboBox_runType.TabIndex = 7;
            this.comboBox_runType.SelectedIndexChanged += new System.EventHandler(this.StepTypecomboBox_SelectedIndexChanged);
            // 
            // label_assertFailedAction
            // 
            this.label_assertFailedAction.AutoSize = true;
            this.label_assertFailedAction.Location = new System.Drawing.Point(201, 26);
            this.label_assertFailedAction.Name = "label_assertFailedAction";
            this.label_assertFailedAction.Size = new System.Drawing.Size(131, 12);
            this.label_assertFailedAction.TabIndex = 5;
            this.label_assertFailedAction.Text = "Assert Failed Action:";
            // 
            // checkBox_RecordStatus
            // 
            this.checkBox_RecordStatus.AutoSize = true;
            this.checkBox_RecordStatus.Checked = true;
            this.checkBox_RecordStatus.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_RecordStatus.Location = new System.Drawing.Point(27, 106);
            this.checkBox_RecordStatus.Name = "checkBox_RecordStatus";
            this.checkBox_RecordStatus.Size = new System.Drawing.Size(102, 16);
            this.checkBox_RecordStatus.TabIndex = 3;
            this.checkBox_RecordStatus.Text = "Record Result";
            this.checkBox_RecordStatus.UseVisualStyleBackColor = true;
            this.checkBox_RecordStatus.CheckedChanged += new System.EventHandler(this.checkBox_RecordResult_CheckedChanged);
            // 
            // comboBox_asserFailedAction
            // 
            this.comboBox_asserFailedAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_asserFailedAction.FormattingEnabled = true;
            this.comboBox_asserFailedAction.Items.AddRange(new object[] {
            "Terminate",
            "Continue",
            "BreakLoop",
            "NextLoop"});
            this.comboBox_asserFailedAction.Location = new System.Drawing.Point(203, 41);
            this.comboBox_asserFailedAction.Name = "comboBox_asserFailedAction";
            this.comboBox_asserFailedAction.Size = new System.Drawing.Size(148, 20);
            this.comboBox_asserFailedAction.TabIndex = 0;
            this.comboBox_asserFailedAction.SelectedIndexChanged += new System.EventHandler(this.comboBox_conditionType_SelectedIndexChanged);
            // 
            // groupBox_loopProperties
            // 
            this.groupBox_loopProperties.Controls.Add(this.numericUpDown_passTimes);
            this.groupBox_loopProperties.Controls.Add(this.label_passTimes);
            this.groupBox_loopProperties.Controls.Add(this.numericUpDown_retryTime);
            this.groupBox_loopProperties.Controls.Add(this.label_retryTime);
            this.groupBox_loopProperties.Controls.Add(this.label_loopTypeConfig);
            this.groupBox_loopProperties.Controls.Add(this.LoopTimesnumericUpDown);
            this.groupBox_loopProperties.Controls.Add(this.LoopTypecomboBox);
            this.groupBox_loopProperties.Controls.Add(this.label_loopTimesConfig);
            this.groupBox_loopProperties.Location = new System.Drawing.Point(396, 3);
            this.groupBox_loopProperties.Name = "groupBox_loopProperties";
            this.groupBox_loopProperties.Size = new System.Drawing.Size(325, 150);
            this.groupBox_loopProperties.TabIndex = 11;
            this.groupBox_loopProperties.TabStop = false;
            this.groupBox_loopProperties.Text = "Loop Configuration";
            // 
            // numericUpDown_passTimes
            // 
            this.numericUpDown_passTimes.BackColor = System.Drawing.Color.LightGray;
            this.numericUpDown_passTimes.Cursor = System.Windows.Forms.Cursors.Default;
            this.numericUpDown_passTimes.Location = new System.Drawing.Point(175, 101);
            this.numericUpDown_passTimes.Name = "numericUpDown_passTimes";
            this.numericUpDown_passTimes.Size = new System.Drawing.Size(120, 21);
            this.numericUpDown_passTimes.TabIndex = 9;
            this.numericUpDown_passTimes.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_passTimes.ValueChanged += new System.EventHandler(this.numericUpDown_passTimes_ValueChanged);
            // 
            // label_passTimes
            // 
            this.label_passTimes.AutoSize = true;
            this.label_passTimes.Location = new System.Drawing.Point(173, 86);
            this.label_passTimes.Name = "label_passTimes";
            this.label_passTimes.Size = new System.Drawing.Size(71, 12);
            this.label_passTimes.TabIndex = 10;
            this.label_passTimes.Text = "Pass Times:";
            // 
            // numericUpDown_retryTime
            // 
            this.numericUpDown_retryTime.BackColor = System.Drawing.Color.LightGray;
            this.numericUpDown_retryTime.Cursor = System.Windows.Forms.Cursors.Default;
            this.numericUpDown_retryTime.Location = new System.Drawing.Point(175, 44);
            this.numericUpDown_retryTime.Name = "numericUpDown_retryTime";
            this.numericUpDown_retryTime.Size = new System.Drawing.Size(120, 21);
            this.numericUpDown_retryTime.TabIndex = 7;
            this.numericUpDown_retryTime.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numericUpDown_retryTime.ValueChanged += new System.EventHandler(this.numericUpDown_retryTime_ValueChanged);
            // 
            // label_retryTime
            // 
            this.label_retryTime.AutoSize = true;
            this.label_retryTime.Location = new System.Drawing.Point(173, 29);
            this.label_retryTime.Name = "label_retryTime";
            this.label_retryTime.Size = new System.Drawing.Size(77, 12);
            this.label_retryTime.TabIndex = 8;
            this.label_retryTime.Text = "Retry Times:";
            // 
            // label_loopTypeConfig
            // 
            this.label_loopTypeConfig.AutoSize = true;
            this.label_loopTypeConfig.Location = new System.Drawing.Point(26, 29);
            this.label_loopTypeConfig.Name = "label_loopTypeConfig";
            this.label_loopTypeConfig.Size = new System.Drawing.Size(65, 12);
            this.label_loopTypeConfig.TabIndex = 5;
            this.label_loopTypeConfig.Text = "Loop Type:";
            // 
            // LoopTimesnumericUpDown
            // 
            this.LoopTimesnumericUpDown.BackColor = System.Drawing.Color.LightGray;
            this.LoopTimesnumericUpDown.Cursor = System.Windows.Forms.Cursors.Default;
            this.LoopTimesnumericUpDown.Location = new System.Drawing.Point(29, 101);
            this.LoopTimesnumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.LoopTimesnumericUpDown.Name = "LoopTimesnumericUpDown";
            this.LoopTimesnumericUpDown.Size = new System.Drawing.Size(120, 21);
            this.LoopTimesnumericUpDown.TabIndex = 4;
            this.LoopTimesnumericUpDown.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.LoopTimesnumericUpDown.ValueChanged += new System.EventHandler(this.LoopTimesnumericUpDown_ValueChanged);
            // 
            // LoopTypecomboBox
            // 
            this.LoopTypecomboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.LoopTypecomboBox.FormattingEnabled = true;
            this.LoopTypecomboBox.Items.AddRange(new object[] {
            "None",
            "FixedTimes",
            "PassTimes"});
            this.LoopTypecomboBox.Location = new System.Drawing.Point(28, 44);
            this.LoopTypecomboBox.Name = "LoopTypecomboBox";
            this.LoopTypecomboBox.Size = new System.Drawing.Size(121, 20);
            this.LoopTypecomboBox.TabIndex = 0;
            this.LoopTypecomboBox.SelectedIndexChanged += new System.EventHandler(this.LoopTypecomboBox_SelectedIndexChanged);
            // 
            // label_loopTimesConfig
            // 
            this.label_loopTimesConfig.AutoSize = true;
            this.label_loopTimesConfig.Location = new System.Drawing.Point(27, 86);
            this.label_loopTimesConfig.Name = "label_loopTimesConfig";
            this.label_loopTimesConfig.Size = new System.Drawing.Size(41, 12);
            this.label_loopTimesConfig.TabIndex = 6;
            this.label_loopTimesConfig.Text = "Times:";
            // 
            // tabpage_Module
            // 
            this.tabpage_Module.Controls.Add(this.button_selectAssembly);
            this.tabpage_Module.Controls.Add(this.comboBox_assembly);
            this.tabpage_Module.Controls.Add(this.comboBox_Constructor);
            this.tabpage_Module.Controls.Add(this.label_InstanceConfig);
            this.tabpage_Module.Controls.Add(this.Parameter_panel);
            this.tabpage_Module.Controls.Add(this.comboBox_Method);
            this.tabpage_Module.Controls.Add(this.comboBox_RootClass);
            this.tabpage_Module.Controls.Add(this.label_methodConfig);
            this.tabpage_Module.Controls.Add(this.label_ClassConfig);
            this.tabpage_Module.Controls.Add(this.label_assemblyConfig);
            this.tabpage_Module.Location = new System.Drawing.Point(4, 22);
            this.tabpage_Module.Name = "tabpage_Module";
            this.tabpage_Module.Padding = new System.Windows.Forms.Padding(3);
            this.tabpage_Module.Size = new System.Drawing.Size(908, 360);
            this.tabpage_Module.TabIndex = 1;
            this.tabpage_Module.Text = "Module";
            this.tabpage_Module.UseVisualStyleBackColor = true;
            // 
            // button_selectAssembly
            // 
            this.button_selectAssembly.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_selectAssembly.Location = new System.Drawing.Point(870, 6);
            this.button_selectAssembly.Name = "button_selectAssembly";
            this.button_selectAssembly.Size = new System.Drawing.Size(33, 24);
            this.button_selectAssembly.TabIndex = 14;
            this.button_selectAssembly.Text = "...";
            this.button_selectAssembly.UseVisualStyleBackColor = true;
            this.button_selectAssembly.Click += new System.EventHandler(this.button_selectAssembly_Click);
            // 
            // comboBox_assembly
            // 
            this.comboBox_assembly.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox_assembly.FormattingEnabled = true;
            this.comboBox_assembly.Location = new System.Drawing.Point(86, 8);
            this.comboBox_assembly.Name = "comboBox_assembly";
            this.comboBox_assembly.Size = new System.Drawing.Size(778, 20);
            this.comboBox_assembly.TabIndex = 13;
            this.comboBox_assembly.TextChanged += new System.EventHandler(this.comboBox_assembly_TextChanged);
            // 
            // comboBox_Constructor
            // 
            this.comboBox_Constructor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_Constructor.FormattingEnabled = true;
            this.comboBox_Constructor.Location = new System.Drawing.Point(85, 62);
            this.comboBox_Constructor.Name = "comboBox_Constructor";
            this.comboBox_Constructor.Size = new System.Drawing.Size(321, 20);
            this.comboBox_Constructor.TabIndex = 12;
            this.comboBox_Constructor.TextChanged += new System.EventHandler(this.comboBox_Constructor_Validated);
            // 
            // label_InstanceConfig
            // 
            this.label_InstanceConfig.AutoSize = true;
            this.label_InstanceConfig.Location = new System.Drawing.Point(18, 65);
            this.label_InstanceConfig.Name = "label_InstanceConfig";
            this.label_InstanceConfig.Size = new System.Drawing.Size(59, 12);
            this.label_InstanceConfig.TabIndex = 11;
            this.label_InstanceConfig.Text = "Instance:";
            // 
            // Parameter_panel
            // 
            this.Parameter_panel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Parameter_panel.Location = new System.Drawing.Point(86, 87);
            this.Parameter_panel.Name = "Parameter_panel";
            this.Parameter_panel.Size = new System.Drawing.Size(819, 276);
            this.Parameter_panel.TabIndex = 10;
            // 
            // comboBox_Method
            // 
            this.comboBox_Method.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox_Method.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_Method.FormattingEnabled = true;
            this.comboBox_Method.Location = new System.Drawing.Point(583, 62);
            this.comboBox_Method.Name = "comboBox_Method";
            this.comboBox_Method.Size = new System.Drawing.Size(320, 20);
            this.comboBox_Method.TabIndex = 9;
            this.comboBox_Method.TextChanged += new System.EventHandler(this.comboBox_Method_Validated);
            // 
            // comboBox_RootClass
            // 
            this.comboBox_RootClass.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox_RootClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_RootClass.FormattingEnabled = true;
            this.comboBox_RootClass.Location = new System.Drawing.Point(85, 36);
            this.comboBox_RootClass.Name = "comboBox_RootClass";
            this.comboBox_RootClass.Size = new System.Drawing.Size(818, 20);
            this.comboBox_RootClass.TabIndex = 9;
            this.comboBox_RootClass.TextChanged += new System.EventHandler(this.comboBox_RootClass_Validated);
            // 
            // label_methodConfig
            // 
            this.label_methodConfig.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label_methodConfig.AutoSize = true;
            this.label_methodConfig.Location = new System.Drawing.Point(530, 65);
            this.label_methodConfig.Name = "label_methodConfig";
            this.label_methodConfig.Size = new System.Drawing.Size(47, 12);
            this.label_methodConfig.TabIndex = 7;
            this.label_methodConfig.Text = "Method:";
            // 
            // label_ClassConfig
            // 
            this.label_ClassConfig.AutoSize = true;
            this.label_ClassConfig.Location = new System.Drawing.Point(8, 40);
            this.label_ClassConfig.Name = "label_ClassConfig";
            this.label_ClassConfig.Size = new System.Drawing.Size(71, 12);
            this.label_ClassConfig.TabIndex = 7;
            this.label_ClassConfig.Text = "Root Class:";
            // 
            // label_assemblyConfig
            // 
            this.label_assemblyConfig.AutoSize = true;
            this.label_assemblyConfig.Location = new System.Drawing.Point(19, 11);
            this.label_assemblyConfig.Name = "label_assemblyConfig";
            this.label_assemblyConfig.Size = new System.Drawing.Size(59, 12);
            this.label_assemblyConfig.TabIndex = 7;
            this.label_assemblyConfig.Text = "Assemble:";
            // 
            // tabPage_runtimeInfo
            // 
            this.tabPage_runtimeInfo.Controls.Add(this.splitContainer_runtime);
            this.tabPage_runtimeInfo.Location = new System.Drawing.Point(4, 22);
            this.tabPage_runtimeInfo.Name = "tabPage_runtimeInfo";
            this.tabPage_runtimeInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_runtimeInfo.Size = new System.Drawing.Size(908, 360);
            this.tabPage_runtimeInfo.TabIndex = 4;
            this.tabPage_runtimeInfo.Text = "RuntimeInformation";
            this.tabPage_runtimeInfo.UseVisualStyleBackColor = true;
            // 
            // splitContainer_runtime
            // 
            this.splitContainer_runtime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer_runtime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_runtime.Location = new System.Drawing.Point(3, 3);
            this.splitContainer_runtime.Name = "splitContainer_runtime";
            // 
            // splitContainer_runtime.Panel1
            // 
            this.splitContainer_runtime.Panel1.Controls.Add(this.button_deleteWatch);
            this.splitContainer_runtime.Panel1.Controls.Add(this.button_addWatch);
            this.splitContainer_runtime.Panel1.Controls.Add(this.dataGridView_variableValues);
            this.splitContainer_runtime.Panel1.Controls.Add(this.label_variableValues);
            // 
            // splitContainer_runtime.Panel2
            // 
            this.splitContainer_runtime.Panel2.Controls.Add(this.label_ouotput);
            this.splitContainer_runtime.Panel2.Controls.Add(this.button_clearOutput);
            this.splitContainer_runtime.Panel2.Controls.Add(this.button_copyOutput);
            this.splitContainer_runtime.Panel2.Controls.Add(this.textBox_output);
            this.splitContainer_runtime.Size = new System.Drawing.Size(902, 354);
            this.splitContainer_runtime.SplitterDistance = 423;
            this.splitContainer_runtime.TabIndex = 0;
            // 
            // button_deleteWatch
            // 
            this.button_deleteWatch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_deleteWatch.Location = new System.Drawing.Point(266, 319);
            this.button_deleteWatch.Name = "button_deleteWatch";
            this.button_deleteWatch.Size = new System.Drawing.Size(100, 28);
            this.button_deleteWatch.TabIndex = 7;
            this.button_deleteWatch.Text = "Delete Watch";
            this.button_deleteWatch.UseVisualStyleBackColor = true;
            this.button_deleteWatch.Click += new System.EventHandler(this.button_deleteWatch_Click);
            // 
            // button_addWatch
            // 
            this.button_addWatch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_addWatch.Location = new System.Drawing.Point(47, 319);
            this.button_addWatch.Name = "button_addWatch";
            this.button_addWatch.Size = new System.Drawing.Size(100, 28);
            this.button_addWatch.TabIndex = 6;
            this.button_addWatch.Text = "Add Watch";
            this.button_addWatch.UseVisualStyleBackColor = true;
            this.button_addWatch.Click += new System.EventHandler(this.button_addWatch_Click);
            // 
            // dataGridView_variableValues
            // 
            this.dataGridView_variableValues.AllowUserToAddRows = false;
            this.dataGridView_variableValues.AllowUserToDeleteRows = false;
            this.dataGridView_variableValues.AllowUserToResizeRows = false;
            this.dataGridView_variableValues.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView_variableValues.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView_variableValues.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_variableValues.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column_VariableName,
            this.Column_VariableValue});
            this.dataGridView_variableValues.Location = new System.Drawing.Point(1, 22);
            this.dataGridView_variableValues.Name = "dataGridView_variableValues";
            this.dataGridView_variableValues.ReadOnly = true;
            this.dataGridView_variableValues.RowHeadersVisible = false;
            this.dataGridView_variableValues.RowTemplate.Height = 23;
            this.dataGridView_variableValues.Size = new System.Drawing.Size(419, 291);
            this.dataGridView_variableValues.TabIndex = 5;
            // 
            // Column_VariableName
            // 
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Column_VariableName.DefaultCellStyle = dataGridViewCellStyle2;
            this.Column_VariableName.HeaderText = "VariableName";
            this.Column_VariableName.Name = "Column_VariableName";
            this.Column_VariableName.ReadOnly = true;
            // 
            // Column_VariableValue
            // 
            this.Column_VariableValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Column_VariableValue.DefaultCellStyle = dataGridViewCellStyle3;
            this.Column_VariableValue.HeaderText = "Value";
            this.Column_VariableValue.Name = "Column_VariableValue";
            this.Column_VariableValue.ReadOnly = true;
            // 
            // label_variableValues
            // 
            this.label_variableValues.AutoSize = true;
            this.label_variableValues.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_variableValues.Location = new System.Drawing.Point(2, 4);
            this.label_variableValues.Name = "label_variableValues";
            this.label_variableValues.Size = new System.Drawing.Size(101, 12);
            this.label_variableValues.TabIndex = 4;
            this.label_variableValues.Text = "Variable Values:";
            // 
            // label_ouotput
            // 
            this.label_ouotput.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_ouotput.Location = new System.Drawing.Point(4, 3);
            this.label_ouotput.Name = "label_ouotput";
            this.label_ouotput.Size = new System.Drawing.Size(51, 15);
            this.label_ouotput.TabIndex = 3;
            this.label_ouotput.Text = "Output";
            // 
            // button_clearOutput
            // 
            this.button_clearOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_clearOutput.FlatAppearance.BorderSize = 0;
            this.button_clearOutput.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.button_clearOutput.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_clearOutput.Font = new System.Drawing.Font("SimSun", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_clearOutput.Location = new System.Drawing.Point(426, -1);
            this.button_clearOutput.Name = "button_clearOutput";
            this.button_clearOutput.Size = new System.Drawing.Size(45, 22);
            this.button_clearOutput.TabIndex = 2;
            this.button_clearOutput.Text = "Clear";
            this.button_clearOutput.UseVisualStyleBackColor = true;
            this.button_clearOutput.Click += new System.EventHandler(this.button_clearOutput_Click);
            // 
            // button_copyOutput
            // 
            this.button_copyOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_copyOutput.FlatAppearance.BorderSize = 0;
            this.button_copyOutput.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.button_copyOutput.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_copyOutput.Font = new System.Drawing.Font("SimSun", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_copyOutput.Location = new System.Drawing.Point(381, -1);
            this.button_copyOutput.Name = "button_copyOutput";
            this.button_copyOutput.Size = new System.Drawing.Size(45, 22);
            this.button_copyOutput.TabIndex = 1;
            this.button_copyOutput.Text = "Copy";
            this.button_copyOutput.UseVisualStyleBackColor = true;
            this.button_copyOutput.Click += new System.EventHandler(this.button_copyOutput_Click);
            // 
            // textBox_output
            // 
            this.textBox_output.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_output.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_output.Location = new System.Drawing.Point(0, 21);
            this.textBox_output.MaxLength = 65535;
            this.textBox_output.Multiline = true;
            this.textBox_output.Name = "textBox_output";
            this.textBox_output.ReadOnly = true;
            this.textBox_output.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_output.Size = new System.Drawing.Size(470, 331);
            this.textBox_output.TabIndex = 0;
            // 
            // tabCon_Step
            // 
            this.tabCon_Step.Controls.Add(this.tabPage_stepData);
            this.tabCon_Step.Controls.Add(this.RuntimeStatusTab);
            this.tabCon_Step.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabCon_Step.Location = new System.Drawing.Point(0, 0);
            this.tabCon_Step.Name = "tabCon_Step";
            this.tabCon_Step.SelectedIndex = 0;
            this.tabCon_Step.Size = new System.Drawing.Size(916, 232);
            this.tabCon_Step.TabIndex = 0;
            // 
            // tabPage_stepData
            // 
            this.tabPage_stepData.Controls.Add(this.treeView_stepView);
            this.tabPage_stepData.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPage_stepData.Location = new System.Drawing.Point(4, 22);
            this.tabPage_stepData.Name = "tabPage_stepData";
            this.tabPage_stepData.Size = new System.Drawing.Size(908, 206);
            this.tabPage_stepData.TabIndex = 2;
            this.tabPage_stepData.Text = "Step";
            this.tabPage_stepData.UseVisualStyleBackColor = true;
            // 
            // treeView_stepView
            // 
            this.treeView_stepView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView_stepView.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeView_stepView.Location = new System.Drawing.Point(0, 0);
            this.treeView_stepView.Name = "treeView_stepView";
            this.treeView_stepView.Size = new System.Drawing.Size(908, 206);
            this.treeView_stepView.TabIndex = 0;
            this.treeView_stepView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_stepView_AfterSelect);
            // 
            // RuntimeStatusTab
            // 
            this.RuntimeStatusTab.Controls.Add(this.tableLayoutPanel1);
            this.RuntimeStatusTab.Location = new System.Drawing.Point(4, 22);
            this.RuntimeStatusTab.Name = "RuntimeStatusTab";
            this.RuntimeStatusTab.Padding = new System.Windows.Forms.Padding(3);
            this.RuntimeStatusTab.Size = new System.Drawing.Size(904, 204);
            this.RuntimeStatusTab.TabIndex = 1;
            this.RuntimeStatusTab.Text = "Runtime Status";
            this.RuntimeStatusTab.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.textBoxReport, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel_buttonPanel, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(898, 198);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // textBoxReport
            // 
            this.textBoxReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxReport.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxReport.Location = new System.Drawing.Point(3, 33);
            this.textBoxReport.Multiline = true;
            this.textBoxReport.Name = "textBoxReport";
            this.textBoxReport.ReadOnly = true;
            this.textBoxReport.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxReport.Size = new System.Drawing.Size(892, 184);
            this.textBoxReport.TabIndex = 0;
            // 
            // panel_buttonPanel
            // 
            this.panel_buttonPanel.Controls.Add(this.button_openReportDir);
            this.panel_buttonPanel.Controls.Add(this.buttonOpenReport);
            this.panel_buttonPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_buttonPanel.Location = new System.Drawing.Point(3, 3);
            this.panel_buttonPanel.Name = "panel_buttonPanel";
            this.panel_buttonPanel.Size = new System.Drawing.Size(892, 24);
            this.panel_buttonPanel.TabIndex = 15;
            // 
            // button_openReportDir
            // 
            this.button_openReportDir.Location = new System.Drawing.Point(164, 2);
            this.button_openReportDir.Margin = new System.Windows.Forms.Padding(5, 3, 3, 3);
            this.button_openReportDir.Name = "button_openReportDir";
            this.button_openReportDir.Size = new System.Drawing.Size(160, 21);
            this.button_openReportDir.TabIndex = 3;
            this.button_openReportDir.Text = "Open Report Directory";
            this.button_openReportDir.UseVisualStyleBackColor = true;
            this.button_openReportDir.Click += new System.EventHandler(this.button_openReportDir_Click);
            // 
            // buttonOpenReport
            // 
            this.buttonOpenReport.Location = new System.Drawing.Point(0, 2);
            this.buttonOpenReport.Name = "buttonOpenReport";
            this.buttonOpenReport.Size = new System.Drawing.Size(160, 21);
            this.buttonOpenReport.TabIndex = 2;
            this.buttonOpenReport.Text = "Open Report";
            this.buttonOpenReport.UseVisualStyleBackColor = true;
            this.buttonOpenReport.Click += new System.EventHandler(this.buttonOpenReport_Click);
            // 
            // statusStripButton
            // 
            this.statusStripButton.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStripButton.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.StatusUseValue,
            this.toolStripStatusLabel_userGroupLabel,
            this.toolStripStatusLabel_userGroup,
            this.StatusMsg,
            this.StatusDataL,
            this.StatusDate,
            this.StatusTimeL,
            this.StatusTime,
            this.toolStripStatusLabel_stateLabel,
            this.toolStripStatusLabel_stateValue,
            this.toolStripProgressBar_progress});
            this.statusStripButton.Location = new System.Drawing.Point(0, 711);
            this.statusStripButton.Name = "statusStripButton";
            this.statusStripButton.Size = new System.Drawing.Size(1284, 31);
            this.statusStripButton.TabIndex = 7;
            this.statusStripButton.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.toolStripStatusLabel1.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(51, 26);
            this.toolStripStatusLabel1.Text = "User：";
            // 
            // StatusUseValue
            // 
            this.StatusUseValue.AutoSize = false;
            this.StatusUseValue.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.StatusUseValue.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.StatusUseValue.Name = "StatusUseValue";
            this.StatusUseValue.Size = new System.Drawing.Size(150, 26);
            this.StatusUseValue.Text = "UseValue";
            // 
            // toolStripStatusLabel_userGroupLabel
            // 
            this.toolStripStatusLabel_userGroupLabel.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.toolStripStatusLabel_userGroupLabel.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.toolStripStatusLabel_userGroupLabel.Name = "toolStripStatusLabel_userGroupLabel";
            this.toolStripStatusLabel_userGroupLabel.Size = new System.Drawing.Size(92, 26);
            this.toolStripStatusLabel_userGroupLabel.Text = "User Group：";
            // 
            // toolStripStatusLabel_userGroup
            // 
            this.toolStripStatusLabel_userGroup.AutoSize = false;
            this.toolStripStatusLabel_userGroup.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.toolStripStatusLabel_userGroup.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.toolStripStatusLabel_userGroup.Name = "toolStripStatusLabel_userGroup";
            this.toolStripStatusLabel_userGroup.Size = new System.Drawing.Size(150, 26);
            this.toolStripStatusLabel_userGroup.Text = "UseValue";
            // 
            // StatusMsg
            // 
            this.StatusMsg.Name = "StatusMsg";
            this.StatusMsg.Size = new System.Drawing.Size(600, 26);
            this.StatusMsg.Text = "                                                                                 " +
    "                                                                   ";
            // 
            // StatusDataL
            // 
            this.StatusDataL.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.StatusDataL.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.StatusDataL.Name = "StatusDataL";
            this.StatusDataL.Size = new System.Drawing.Size(51, 26);
            this.StatusDataL.Spring = true;
            this.StatusDataL.Text = "Date:";
            // 
            // StatusDate
            // 
            this.StatusDate.AutoSize = false;
            this.StatusDate.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.StatusDate.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.StatusDate.Name = "StatusDate";
            this.StatusDate.Size = new System.Drawing.Size(71, 26);
            this.StatusDate.Text = "2019-07-16";
            // 
            // StatusTimeL
            // 
            this.StatusTimeL.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.StatusTimeL.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.StatusTimeL.Name = "StatusTimeL";
            this.StatusTimeL.Size = new System.Drawing.Size(51, 26);
            this.StatusTimeL.Spring = true;
            this.StatusTimeL.Text = "Time:";
            // 
            // StatusTime
            // 
            this.StatusTime.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.StatusTime.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.StatusTime.Name = "StatusTime";
            this.StatusTime.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.StatusTime.Size = new System.Drawing.Size(51, 26);
            this.StatusTime.Spring = true;
            this.StatusTime.Text = "08:36:12";
            // 
            // toolStripStatusLabel_stateLabel
            // 
            this.toolStripStatusLabel_stateLabel.AutoSize = false;
            this.toolStripStatusLabel_stateLabel.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.toolStripStatusLabel_stateLabel.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.toolStripStatusLabel_stateLabel.Name = "toolStripStatusLabel_stateLabel";
            this.toolStripStatusLabel_stateLabel.Size = new System.Drawing.Size(77, 26);
            this.toolStripStatusLabel_stateLabel.Text = "State:";
            // 
            // toolStripStatusLabel_stateValue
            // 
            this.toolStripStatusLabel_stateValue.AutoSize = false;
            this.toolStripStatusLabel_stateValue.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.toolStripStatusLabel_stateValue.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.toolStripStatusLabel_stateValue.Name = "toolStripStatusLabel_stateValue";
            this.toolStripStatusLabel_stateValue.Size = new System.Drawing.Size(80, 26);
            // 
            // toolStripProgressBar_progress
            // 
            this.toolStripProgressBar_progress.Name = "toolStripProgressBar_progress";
            this.toolStripProgressBar_progress.Size = new System.Drawing.Size(100, 25);
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMain.Location = new System.Drawing.Point(0, 52);
            this.splitContainerMain.Name = "splitContainerMain";
            this.splitContainerMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.labelTestRunning);
            this.splitContainerMain.Panel1.Controls.Add(this.labelTestGen);
            this.splitContainerMain.Panel1.Controls.Add(this.labelProject);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.splitContainer_MainFrame);
            this.splitContainerMain.Size = new System.Drawing.Size(1284, 659);
            this.splitContainerMain.SplitterDistance = 25;
            this.splitContainerMain.TabIndex = 8;
            // 
            // labelTestRunning
            // 
            this.labelTestRunning.AutoSize = true;
            this.labelTestRunning.Location = new System.Drawing.Point(1211, 6);
            this.labelTestRunning.Name = "labelTestRunning";
            this.labelTestRunning.Size = new System.Drawing.Size(47, 12);
            this.labelTestRunning.TabIndex = 2;
            this.labelTestRunning.Text = "Running";
            this.labelTestRunning.Visible = false;
            // 
            // labelTestGen
            // 
            this.labelTestGen.AutoSize = true;
            this.labelTestGen.Location = new System.Drawing.Point(1065, 6);
            this.labelTestGen.Name = "labelTestGen";
            this.labelTestGen.Size = new System.Drawing.Size(65, 12);
            this.labelTestGen.TabIndex = 1;
            this.labelTestGen.Text = "Generating";
            this.labelTestGen.Visible = false;
            // 
            // labelProject
            // 
            this.labelProject.AutoSize = true;
            this.labelProject.Location = new System.Drawing.Point(7, 6);
            this.labelProject.Name = "labelProject";
            this.labelProject.Size = new System.Drawing.Size(77, 12);
            this.labelProject.TabIndex = 0;
            this.labelProject.Text = "Project Name";
            // 
            // splitContainer_MainFrame
            // 
            this.splitContainer_MainFrame.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer_MainFrame.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_MainFrame.Location = new System.Drawing.Point(0, 0);
            this.splitContainer_MainFrame.Name = "splitContainer_MainFrame";
            // 
            // splitContainer_MainFrame.Panel1
            // 
            this.splitContainer_MainFrame.Panel1.Controls.Add(this.splitContainer_sequenceAndVar);
            // 
            // splitContainer_MainFrame.Panel2
            // 
            this.splitContainer_MainFrame.Panel2.Controls.Add(this.splitContainer_StepConfig);
            this.splitContainer_MainFrame.Size = new System.Drawing.Size(1284, 630);
            this.splitContainer_MainFrame.SplitterDistance = 360;
            this.splitContainer_MainFrame.TabIndex = 0;
            // 
            // splitContainer_sequenceAndVar
            // 
            this.splitContainer_sequenceAndVar.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer_sequenceAndVar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_sequenceAndVar.Location = new System.Drawing.Point(0, 0);
            this.splitContainer_sequenceAndVar.Name = "splitContainer_sequenceAndVar";
            this.splitContainer_sequenceAndVar.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer_sequenceAndVar.Panel1
            // 
            this.splitContainer_sequenceAndVar.Panel1.Controls.Add(this.tabCon_Seq);
            // 
            // splitContainer_sequenceAndVar.Panel2
            // 
            this.splitContainer_sequenceAndVar.Panel2.Controls.Add(this.tabCon_Variable);
            this.splitContainer_sequenceAndVar.Size = new System.Drawing.Size(360, 630);
            this.splitContainer_sequenceAndVar.SplitterDistance = 310;
            this.splitContainer_sequenceAndVar.TabIndex = 1;
            // 
            // tabCon_Seq
            // 
            this.tabCon_Seq.Controls.Add(this.tabPage_mainSequence);
            this.tabCon_Seq.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabCon_Seq.Enabled = false;
            this.tabCon_Seq.Location = new System.Drawing.Point(0, 0);
            this.tabCon_Seq.Name = "tabCon_Seq";
            this.tabCon_Seq.SelectedIndex = 0;
            this.tabCon_Seq.Size = new System.Drawing.Size(356, 306);
            this.tabCon_Seq.TabIndex = 0;
            this.tabCon_Seq.SelectedIndexChanged += new System.EventHandler(this.tabCon_Seq_SelectedIndexChanged);
            // 
            // tabPage_mainSequence
            // 
            this.tabPage_mainSequence.Controls.Add(this.treeView_sequenceTree);
            this.tabPage_mainSequence.Location = new System.Drawing.Point(4, 22);
            this.tabPage_mainSequence.Name = "tabPage_mainSequence";
            this.tabPage_mainSequence.Size = new System.Drawing.Size(348, 280);
            this.tabPage_mainSequence.TabIndex = 0;
            this.tabPage_mainSequence.Text = "Default Sequences";
            this.tabPage_mainSequence.UseVisualStyleBackColor = true;
            // 
            // treeView_sequenceTree
            // 
            this.treeView_sequenceTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView_sequenceTree.Font = new System.Drawing.Font("Courier New", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeView_sequenceTree.Location = new System.Drawing.Point(0, 0);
            this.treeView_sequenceTree.Name = "treeView_sequenceTree";
            this.treeView_sequenceTree.Size = new System.Drawing.Size(348, 280);
            this.treeView_sequenceTree.TabIndex = 0;
            this.treeView_sequenceTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_sequenceTree_AfterSelect);
            // 
            // tabCon_Variable
            // 
            this.tabCon_Variable.Controls.Add(this.globalVariableTab);
            this.tabCon_Variable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabCon_Variable.Enabled = false;
            this.tabCon_Variable.Location = new System.Drawing.Point(0, 0);
            this.tabCon_Variable.Name = "tabCon_Variable";
            this.tabCon_Variable.SelectedIndex = 0;
            this.tabCon_Variable.Size = new System.Drawing.Size(356, 312);
            this.tabCon_Variable.TabIndex = 0;
            // 
            // globalVariableTab
            // 
            this.globalVariableTab.Location = new System.Drawing.Point(4, 22);
            this.globalVariableTab.Name = "globalVariableTab";
            this.globalVariableTab.Padding = new System.Windows.Forms.Padding(3);
            this.globalVariableTab.Size = new System.Drawing.Size(348, 286);
            this.globalVariableTab.TabIndex = 0;
            this.globalVariableTab.Text = "Global Variables";
            this.globalVariableTab.UseVisualStyleBackColor = true;
            // 
            // splitContainer_StepConfig
            // 
            this.splitContainer_StepConfig.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer_StepConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_StepConfig.Location = new System.Drawing.Point(0, 0);
            this.splitContainer_StepConfig.Name = "splitContainer_StepConfig";
            this.splitContainer_StepConfig.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer_StepConfig.Panel1
            // 
            this.splitContainer_StepConfig.Panel1.Controls.Add(this.tabCon_Step);
            // 
            // splitContainer_StepConfig.Panel2
            // 
            this.splitContainer_StepConfig.Panel2.Controls.Add(this.tabControl_settings);
            this.splitContainer_StepConfig.Size = new System.Drawing.Size(920, 630);
            this.splitContainer_StepConfig.SplitterDistance = 236;
            this.splitContainer_StepConfig.TabIndex = 0;
            // 
            // cMS_DgvVariable
            // 
            this.cMS_DgvVariable.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cMS_DgvVariable.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripMenuItem2});
            this.cMS_DgvVariable.Name = "cMS_DgvStep";
            this.cMS_DgvVariable.Size = new System.Drawing.Size(166, 48);
            this.cMS_DgvVariable.Opening += new System.ComponentModel.CancelEventHandler(this.cMS_DgvVariable_Opening);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(165, 22);
            this.toolStripMenuItem1.Text = "Add Variable";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.AddVariable_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(165, 22);
            this.toolStripMenuItem2.Text = "Delete Variable";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.DeleteVariable_Click);
            // 
            // saveFileDialog_sequence
            // 
            this.saveFileDialog_sequence.Filter = "Testflow序列|*.tfseq";
            // 
            // openFileDialog_sequence
            // 
            this.openFileDialog_sequence.FileName = "openFileDialog";
            this.openFileDialog_sequence.Filter = "Testflow序列|*.tfseq";
            // 
            // viewController_Main
            // 
            this.viewController_Main.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("viewController_Main.BackgroundImage")));
            this.viewController_Main.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.viewController_Main.ControlInfos = new string[] {
        resources.GetString("viewController_Main.ControlInfos"),
        resources.GetString("viewController_Main.ControlInfos1"),
        resources.GetString("viewController_Main.ControlInfos2"),
        resources.GetString("viewController_Main.ControlInfos3"),
        resources.GetString("viewController_Main.ControlInfos4"),
        resources.GetString("viewController_Main.ControlInfos5"),
        resources.GetString("viewController_Main.ControlInfos6"),
        resources.GetString("viewController_Main.ControlInfos7"),
        resources.GetString("viewController_Main.ControlInfos8")};
            this.viewController_Main.Location = new System.Drawing.Point(1252, 0);
            this.viewController_Main.MaximumSize = new System.Drawing.Size(30, 30);
            this.viewController_Main.MinimumSize = new System.Drawing.Size(30, 30);
            this.viewController_Main.Name = "viewController_Main";
            this.viewController_Main.Size = new System.Drawing.Size(30, 30);
            this.viewController_Main.State = "";
            this.viewController_Main.StateNames = new string[] {
        "EditIdle",
        "EditProcess",
        "RunIdle",
        "RunBlock",
        "RunProcessing",
        "Running",
        "RunOver"};
            this.viewController_Main.StateValue = -1;
            this.viewController_Main.TabIndex = 9;
            this.viewController_Main.PostListeners += new System.Action<int, int>(this.viewController_Main_PostListeners);
            this.viewController_Main.PreListeners += new System.Action<int, int>(this.viewController_Main_PreListeners);
            // 
            // contextMenuStrip_varList
            // 
            this.contextMenuStrip_varList.Font = new System.Drawing.Font("Microsoft YaHei", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.contextMenuStrip_varList.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip_varList.Name = "contextMenuStrip_varList";
            this.contextMenuStrip_varList.Size = new System.Drawing.Size(61, 4);
            this.contextMenuStrip_varList.VisibleChanged += new System.EventHandler(this.contextMenuStrip_varList_VisibleChanged);
            // 
            // openFileDialog_assembly
            // 
            this.openFileDialog_assembly.FileName = "Open assembly file";
            this.openFileDialog_assembly.Filter = "dll library|*.dll";
            // 
            // contextMenuStrip_sequence
            // 
            this.contextMenuStrip_sequence.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip_sequence.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addSequenceGroupToolStripMenuItem,
            this.insertSequenceGroupToolStripMenuItem,
            this.deleteSequenceGroupToolStripMenuItem,
            this.addSequenceToolStripMenuItem1,
            this.insertSequenceToolStripMenuItem,
            this.deleteSequenceToolStripMenuItem,
            this.renameSequenceToolStripMenuItem});
            this.contextMenuStrip_sequence.Name = "contextMenuStrip_sequence";
            this.contextMenuStrip_sequence.Size = new System.Drawing.Size(211, 158);
            // 
            // addSequenceGroupToolStripMenuItem
            // 
            this.addSequenceGroupToolStripMenuItem.Name = "addSequenceGroupToolStripMenuItem";
            this.addSequenceGroupToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.addSequenceGroupToolStripMenuItem.Text = "Add SequenceGroup";
            this.addSequenceGroupToolStripMenuItem.Visible = false;
            // 
            // insertSequenceGroupToolStripMenuItem
            // 
            this.insertSequenceGroupToolStripMenuItem.Name = "insertSequenceGroupToolStripMenuItem";
            this.insertSequenceGroupToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.insertSequenceGroupToolStripMenuItem.Text = "Insert SequenceGroup";
            this.insertSequenceGroupToolStripMenuItem.Visible = false;
            // 
            // deleteSequenceGroupToolStripMenuItem
            // 
            this.deleteSequenceGroupToolStripMenuItem.Name = "deleteSequenceGroupToolStripMenuItem";
            this.deleteSequenceGroupToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.deleteSequenceGroupToolStripMenuItem.Text = "Delete SequenceGroup";
            this.deleteSequenceGroupToolStripMenuItem.Visible = false;
            // 
            // addSequenceToolStripMenuItem1
            // 
            this.addSequenceToolStripMenuItem1.Name = "addSequenceToolStripMenuItem1";
            this.addSequenceToolStripMenuItem1.Size = new System.Drawing.Size(210, 22);
            this.addSequenceToolStripMenuItem1.Text = "Add Sequence";
            this.addSequenceToolStripMenuItem1.Click += new System.EventHandler(this.addSequenceToolStripMenuItem1_Click);
            // 
            // insertSequenceToolStripMenuItem
            // 
            this.insertSequenceToolStripMenuItem.Name = "insertSequenceToolStripMenuItem";
            this.insertSequenceToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.insertSequenceToolStripMenuItem.Text = "Insert Sequence";
            this.insertSequenceToolStripMenuItem.Click += new System.EventHandler(this.insertSequenceToolStripMenuItem_Click);
            // 
            // deleteSequenceToolStripMenuItem
            // 
            this.deleteSequenceToolStripMenuItem.Name = "deleteSequenceToolStripMenuItem";
            this.deleteSequenceToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.deleteSequenceToolStripMenuItem.Text = "Delete Sequence";
            this.deleteSequenceToolStripMenuItem.Click += new System.EventHandler(this.deleteSequenceToolStripMenuItem_Click);
            // 
            // renameSequenceToolStripMenuItem
            // 
            this.renameSequenceToolStripMenuItem.Name = "renameSequenceToolStripMenuItem";
            this.renameSequenceToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.renameSequenceToolStripMenuItem.Text = "Rename Sequence";
            this.renameSequenceToolStripMenuItem.Click += new System.EventHandler(this.renameSequenceToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1284, 742);
            this.Controls.Add(this.viewController_Main);
            this.Controls.Add(this.splitContainerMain);
            this.Controls.Add(this.statusStripButton);
            this.Controls.Add(this.toolStrip_QuickMenu);
            this.Controls.Add(this.menuStrip_ActionMenu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Easy Test";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.menuStrip_ActionMenu.ResumeLayout(false);
            this.menuStrip_ActionMenu.PerformLayout();
            this.toolStrip_QuickMenu.ResumeLayout(false);
            this.toolStrip_QuickMenu.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.cMS_DgvStep.ResumeLayout(false);
            this.tabControl_settings.ResumeLayout(false);
            this.tabpage_Properties.ResumeLayout(false);
            this.flowLayoutPanel_stepProperties.ResumeLayout(false);
            this.groupBox_stepBehavior.ResumeLayout(false);
            this.groupBox_stepBehavior.PerformLayout();
            this.groupBox_loopProperties.ResumeLayout(false);
            this.groupBox_loopProperties.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_passTimes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_retryTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LoopTimesnumericUpDown)).EndInit();
            this.tabpage_Module.ResumeLayout(false);
            this.tabpage_Module.PerformLayout();
            this.tabPage_runtimeInfo.ResumeLayout(false);
            this.splitContainer_runtime.Panel1.ResumeLayout(false);
            this.splitContainer_runtime.Panel1.PerformLayout();
            this.splitContainer_runtime.Panel2.ResumeLayout(false);
            this.splitContainer_runtime.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_runtime)).EndInit();
            this.splitContainer_runtime.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_variableValues)).EndInit();
            this.tabCon_Step.ResumeLayout(false);
            this.tabPage_stepData.ResumeLayout(false);
            this.RuntimeStatusTab.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel_buttonPanel.ResumeLayout(false);
            this.statusStripButton.ResumeLayout(false);
            this.statusStripButton.PerformLayout();
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel1.PerformLayout();
            this.splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
            this.splitContainerMain.ResumeLayout(false);
            this.splitContainer_MainFrame.Panel1.ResumeLayout(false);
            this.splitContainer_MainFrame.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_MainFrame)).EndInit();
            this.splitContainer_MainFrame.ResumeLayout(false);
            this.splitContainer_sequenceAndVar.Panel1.ResumeLayout(false);
            this.splitContainer_sequenceAndVar.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_sequenceAndVar)).EndInit();
            this.splitContainer_sequenceAndVar.ResumeLayout(false);
            this.tabCon_Seq.ResumeLayout(false);
            this.tabPage_mainSequence.ResumeLayout(false);
            this.tabCon_Variable.ResumeLayout(false);
            this.splitContainer_StepConfig.Panel1.ResumeLayout(false);
            this.splitContainer_StepConfig.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_StepConfig)).EndInit();
            this.splitContainer_StepConfig.ResumeLayout(false);
            this.cMS_DgvVariable.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mainFormBindingSource)).EndInit();
            this.contextMenuStrip_sequence.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        #region 所有控件变量
        private System.Windows.Forms.MenuStrip menuStrip_ActionMenu;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem SaveAsToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem configureToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem debugToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem startToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem suspendToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stopToolStripMenuItem2;
        private System.Windows.Forms.ToolStrip toolStrip_QuickMenu;
        private System.Windows.Forms.ToolStripButton toolStripButton_New;
        private System.Windows.Forms.ToolStripButton toolStripButton_Open;
        private System.Windows.Forms.ToolStripButton toolStripButton_Save;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButton_Run;
        private System.Windows.Forms.ToolStripButton toolStripButton_Suspend;
        private System.Windows.Forms.ToolStripButton toolStripButton_Stop;
        private System.Windows.Forms.ToolStripMenuItem userToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem managerToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip cMS_DgvStep;
        private System.Windows.Forms.ToolStripMenuItem Menu_AddStep;
        private System.Windows.Forms.TabControl tabControl_settings;
        private System.Windows.Forms.TabPage tabpage_Properties;
        private System.Windows.Forms.TabPage tabpage_Module;
        private System.Windows.Forms.ComboBox comboBox_Method;
        private System.Windows.Forms.ComboBox comboBox_RootClass;
        private System.Windows.Forms.Label label_methodConfig;
        private System.Windows.Forms.Label label_ClassConfig;
        private System.Windows.Forms.Label label_assemblyConfig;
        private System.Windows.Forms.ToolStripMenuItem Menu_DeleteStep;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TabControl tabCon_Step;
        private System.Windows.Forms.TabPage RuntimeStatusTab;
        private System.Windows.Forms.ComboBox LoopTypecomboBox;
        private System.Windows.Forms.CheckBox checkBox_RecordStatus;
        private System.Windows.Forms.NumericUpDown LoopTimesnumericUpDown;
        private System.Windows.Forms.StatusStrip statusStripButton;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel StatusUseValue;
        private System.Windows.Forms.TextBox textBoxReport;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ToolStripStatusLabel StatusDataL;
        private System.Windows.Forms.ToolStripStatusLabel StatusDate;
        private System.Windows.Forms.ToolStripStatusLabel StatusTimeL;
        private System.Windows.Forms.ToolStripStatusLabel StatusTime;
        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.SplitContainer splitContainer_MainFrame;
        private System.Windows.Forms.SplitContainer splitContainer_sequenceAndVar;
        private System.Windows.Forms.SplitContainer splitContainer_StepConfig;
        private System.Windows.Forms.Label labelProject;
        private System.Windows.Forms.TabControl tabCon_Variable;
        private System.Windows.Forms.TabPage globalVariableTab;
        private System.Windows.Forms.ToolStripStatusLabel StatusMsg;
        private System.Windows.Forms.Label label_loopTimesConfig;
        private System.Windows.Forms.Label label_loopTypeConfig;
       #endregion

        private TabControl tabCon_Seq;
        private TabPage tabPage_mainSequence;
        private ContextMenuStrip cMS_DgvVariable;
        private ToolStripMenuItem toolStripMenuItem1;
        private ToolStripMenuItem toolStripMenuItem2;
        private Panel Parameter_panel;
        private ComboBox comboBox_Constructor;
        private Label label_InstanceConfig;
        private SaveFileDialog saveFileDialog_sequence;
        private OpenFileDialog openFileDialog_sequence;
        private ComboBox comboBox_runType;
        private ToolStripMenuItem actionToolStripMenuItem;
        private ToolStripMenuItem actionToolStripMenuItem1;
        private ToolStripMenuItem sequenceCallToolStripMenuItem;
        private BindingSource mainFormBindingSource;
        private Label labelTestRunning;
        private Label labelTestGen;
        private ToolStripMenuItem ToolStripMenuItem_insertStep;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem1;
        private ToolStripMenuItem reloginToolStripMenuItem;
        private TabPage tabPage_runtimeInfo;
        private SplitContainer splitContainer_runtime;
        private TextBox textBox_output;
        private Button button_copyOutput;
        private Button button_clearOutput;
        private Label label_ouotput;
        private SeeSharpTools.JY.GUI.ViewController viewController_Main;
        private GroupBox groupBox_loopProperties;
        private GroupBox groupBox_stepBehavior;
        private Label label_runType;
        private FlowLayoutPanel flowLayoutPanel_stepProperties;
        private ToolStripProgressBar toolStripProgressBar_progress;
        private ToolStripMenuItem testToolStripMenuItem1;
        private ToolStripMenuItem actionToolStripMenuItem3;
        private ToolStripMenuItem sequenceCallToolStripMenuItem2;
        private Label label_variableValues;
        private DataGridView dataGridView_variableValues;
        private TabPage tabPage_stepData;
        private ContextMenuStrip contextMenuStrip_varList;
        private ToolStripMenuItem configureToolStripMenuItem;
        private ToolStripStatusLabel toolStripStatusLabel_stateLabel;
        private ToolStripStatusLabel toolStripStatusLabel_stateValue;
        private ComboBox comboBox_assembly;
        private Button button_selectAssembly;
        private OpenFileDialog openFileDialog_assembly;
        private Button button_deleteWatch;
        private Button button_addWatch;
        private ToolStripMenuItem timingToolStripMenuItem;
        private ToolStripMenuItem timingToolStripMenuItem1;
        private NumericUpDown numericUpDown_retryTime;
        private Label label_retryTime;
        private NumericUpDown numericUpDown_passTimes;
        private Label label_passTimes;
        private DataGridViewTextBoxColumn Column_VariableName;
        private DataGridViewTextBoxColumn Column_VariableValue;
        private ToolStripStatusLabel toolStripStatusLabel_userGroupLabel;
        private ToolStripStatusLabel toolStripStatusLabel_userGroup;
        private ToolStripMenuItem selectModelToolStripMenuItem;
        private Panel panel_buttonPanel;
        private Button button_openReportDir;
        private Button buttonOpenReport;
        private TreeView treeView_sequenceTree;
        private TreeView treeView_stepView;
        private ContextMenuStrip contextMenuStrip_sequence;
        private ToolStripMenuItem addSequenceGroupToolStripMenuItem;
        private ToolStripMenuItem addSequenceToolStripMenuItem1;
        private ToolStripMenuItem deleteSequenceGroupToolStripMenuItem;
        private ToolStripMenuItem deleteSequenceToolStripMenuItem;
        private ToolStripMenuItem insertSequenceGroupToolStripMenuItem;
        private ToolStripMenuItem insertSequenceToolStripMenuItem;
        private Label label_invokeFailedLabel;
        private ComboBox comboBox_invokeFailedAction;
        private Label label_assertFailedAction;
        private ComboBox comboBox_asserFailedAction;
        private ToolStripMenuItem renameSequenceToolStripMenuItem;
        private ToolStripMenuItem toolStripMenuItem3;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripMenuItem addSequenceToolStripMenuItem;
        private ToolStripMenuItem addStepToolStripMenuItem;
        private ToolStripMenuItem testToolStripMenuItem;
        private ToolStripMenuItem passFailTestToolStripMenuItem;
        private ToolStripMenuItem numericLimitTestToolStripMenuItem;
        private ToolStripMenuItem stringValueTestToolStripMenuItem;
        private ToolStripMenuItem actionToolStripMenuItem2;
        private ToolStripMenuItem sequenceCallToolStripMenuItem1;
        private ToolStripMenuItem timingToolStripMenuItem2;
        private ToolStripMenuItem startTimingToolStripMenuItem;
        private ToolStripMenuItem endTimingToolStripMenuItem2;
        private ToolStripMenuItem waitToolStripMenuItem2;
        private ToolStripMenuItem editSequenceToolStripMenuItem;
        private ToolStripMenuItem loadLibraryToolStripMenuItem;
        private ToolStripMenuItem conditionBlockToolStripMenuItem;
        private ToolStripMenuItem conditionStatementToolStripMenuItem;
        private ToolStripMenuItem conditionLoopToolStripMenuItem;
        private ToolStripMenuItem multiThreadToolStripMenuItem;
        private ToolStripMenuItem timingToolStripMenuItem3;
        private ToolStripMenuItem batchBlockToolStripMenuItem;
        private ToolStripMenuItem conditionBlockToolStripMenuItem1;
        private ToolStripMenuItem conditionStatementToolStripMenuItem1;
        private ToolStripMenuItem conditionLoopToolStripMenuItem1;
        private ToolStripMenuItem multiThreadToolStripMenuItem1;
        private ToolStripMenuItem timerBlockToolStripMenuItem;
        private ToolStripMenuItem batchBlockToolStripMenuItem1;
        private ToolStripMenuItem gotoToolStripMenuItem1;
        private ToolStripMenuItem tryFinallyBlockToolStripMenuItem;
        private ToolStripMenuItem gotoToolStripMenuItem;
        private ToolStripMenuItem tryFinallyToolStripMenuItem;
    }
}

