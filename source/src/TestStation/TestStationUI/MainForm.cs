using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Testflow.Data.Description;
using Testflow.Data.Sequence;
using Testflow.DesignTime;
using Testflow.Runtime;
using Testflow.Usr;
using Testflow.Data;
using Testflow.Runtime.Data;
using System.IO;
using Testflow.Modules;
using TestStation.Controls;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using EasyTest.ModelManage;
using TestStation.Authentication;
using TestStation.Common;
using TestStation.OperationPanel;
using TestStation.ParameterChecker;
using TestStation.Properties;
using TestStation.Report;
using TestStationLimit;
using LogLevel = TestStation.Common.LogLevel;

namespace TestStation
{
    public partial class MainForm : Form
    {
        private const string ExistingObjName = "<Existing Object>";
        private const string ExistingObjParent = "Existing Object";

        #region Id
        private int _currentTestProjectId = 0;   //TestProject ID
        private int _currentSequenceId = 0;      //Sequence ID
        private int _currentStepId = 0;         //Step ID
        private int _currentVariableId = 0;     //Variable ID
        private int _currentLimitId = 0;        //Limit ID
        #endregion

        #region Selected Current Objects
        private ISequenceGroup SequenceGroup => TestflowDesigntimeSession?.Context.SequenceGroup;

        private ISequence CurrentSeq { get; set; }

        private ISequenceStep CurrentStep { get; set; }

        #endregion

        #region 图片

        private string _expandImagePath;
        private string _NexpandImagePath;
        private SequenceMaintainer _sequenceMaintainer;
        #endregion

        #region 其他字段

        private bool stepSelection = false; //step选择
        string _filePath = string.Empty;
        string _reportFilePath = "";
        public string TsfFilePath { get; private set; }

        #endregion

        #region 列号常量定义

        const int SeqTableNameCol = 0;

        const int StepTableIconCol = 0;
        const int StepTableNameCol = 1;
        const int StepTableDescCol = 2;
        const int StepTableSettingCol = 3;
        const int StepTableUnitCol = 4;
        const int StepTableDataCol = 5;
        const int StepTableResultCol = 6;

        const int LimitTableNameCol = 1;
        const int LimitTableTypeCol = 1;
        const int LimitTableLowCol = 2;
        const int LimitTableHighCol = 3;
        const int LimitTableUnitCol = 4;
        const int LimitTableCompTypeCol = 5;
        const int LimitTableVariableCol = 6;

        const int ParamTableNameCol = 1;
        const int ParamTableTypeCol = 2;
        const int ParamTableModifierCol = 3;
        const int ParamTableValueCol = 4;
        const int ParamTableFxCol = 5;
        const int ParamTableCheckCol = 6;

        #endregion

        #region 背景颜色定义定义

        private Color NAColor = Control.DefaultBackColor;
        private Color PassColor = Color.FromArgb(102, 255, 51);
        private Color FailedColor = Color.Red;
        private Color RunningColor = Color.Coral;
        private Color SkipColor = Color.Gray;

        #endregion

        #region 私有字段-各种标志位

        private bool _stopSign = false;
        private bool _firstNew = true;
        private bool _internalOperation = false;
        private volatile bool _start = false;
        bool openSign = false;

        #endregion

        #region 私有字段-Testflow模块与服务

        private readonly GlobalInfo _globalInfo; //GlobalInfo是Runner的包装层
        private readonly IDesignTimeService _testflowDesigntimeService;

        private IDesignTimeSession TestflowDesigntimeSession
        {
            get
            {
                if (!_testflowDesigntimeService.SequenceSessions.ContainsKey(0))
                {
                    return null;
                }
                return _testflowDesigntimeService?.SequenceSessions[0];
            }
        } 
        private readonly IRuntimeService _testflowRuntimeService;
        private readonly IComInterfaceManager _interfaceManger;
        private EventController _eventController;
        // 表达式匹配模式，第1组为数组的源数据，第二组为数组的变量名称
        private readonly Regex _expRegex;

        #endregion

        #region 私有字段-被选中序列相关的标识

        private ISequenceStep CurrentTypeStep
            => (null != CurrentStep && CurrentStep.HasSubSteps) ? CurrentStep.SubSteps[0] : null;

        private ISequenceStep CurrentConstructorStep
            => CurrentStep.SubSteps.FirstOrDefault(item => item.Description.Equals("Constructor"));

        private ISequenceStep CurrentFunctionStep
            => CurrentStep.SubSteps.FirstOrDefault(item => item.Description.Equals("Method"));

        private string _oldVariableType;
        private string _oldVariableValue;
        private IComInterfaceDescription _currentComDescription;
        private IClassInterfaceDescription _currentClassDescription;
        private volatile OperationPanelForm _operationPanel;

        #endregion

        #region 私有字段-序列控件相关

        //当前的sequence列表
        private DataGridView SequenceTable => (DataGridView) (tabCon_Seq.SelectedTab.Controls[0]);
        // step列表
        private DataGridView _stepTable;
        // 当前的变量列表
        private DataGridView VaraibleTable => (DataGridView) (tabCon_Variable.SelectedTab.Controls[0]);
        // 参数列表
        private TreeDataGridView _paramTable;

        #endregion

        #region 界面初始化
        
        public MainForm(AuthenticationSession session, string sequencePath)
        {
            #region Testflow：模块与服务初始化
            _globalInfo = GlobalInfo.GetInstance();
            _globalInfo.Session = session;
            _globalInfo.PrintInfo = this.PrintInfo;
            _globalInfo.PrintUutResult = PrintUutResult;
            _testflowDesigntimeService = _globalInfo.TestflowEntity.DesignTimeService;
            _testflowRuntimeService = _globalInfo.TestflowEntity.RuntimeService;
            _interfaceManger = _globalInfo.TestflowEntity.ComInterfaceManager;
            _testflowDesigntimeService.Initialize();
            #endregion
            _expandImagePath = _globalInfo.TestflowHome + "expand.png";
            _NexpandImagePath = _globalInfo.TestflowHome + "Nexpand.png";
            this._expRegex = new Regex("^(([^\\.]+)(?:\\.[^\\.]+)*)\\[\\d+\\]$");

            InitializeComponent();
            this.TsfFilePath = sequencePath;

            InitControls();

            toolStripButton_New.Enabled = true;
            toolStripButton_Open.Enabled = true;
            toolStripButton_Save.Enabled = false;
            toolStripButton_Run.Enabled = false;
            toolStripButton_Suspend.Enabled = false;
            toolStripButton_Stop.Enabled = false;

            CheckForIllegalCrossThreadCalls = false;//多线程check使能

            

            // TestStationLimit.dll加载
            _interfaceManger.GetComponentInterface(Environment.GetEnvironmentVariable("TESTFLOW_WORKSPACE") + @"\TestStationLimit.dll");
            
            // 暂时移除Parameters页
            tabControl_settings.TabPages.Remove(tabpage_parameters);
        }

        private void InitControls()
        {
            // tdgv_Parameter初始化
            CreateTDGVParameter();

            // dgv_GlobalVariable初始化
            CreateDGVVariable(0);
        }

        private void CreateDGVVariable(int tabNumber)
        {
            DataGridView dgv_variable = new DataGridView();

            // 属性
            dgv_variable.Location = new Point();
            dgv_variable.Dock = DockStyle.Fill;

            // Columns
            dgv_variable.Columns.Add("VariableName", "Name");

            DataGridViewComboBoxColumn TypeColumn = new DataGridViewComboBoxColumn();   //注意type有三种选项所以是ComboBoxColumn
            TypeColumn.Name = "VariableType";
            TypeColumn.HeaderText = "Type";
            TypeColumn.DataSource = Enum.GetNames(typeof(JudgeType));
            dgv_variable.Columns.Add(TypeColumn);

            dgv_variable.Columns.Add("VariableValue", "Value");
            dgv_variable.Columns[2].ReadOnly = true;

            dgv_variable.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;//固定表头
            dgv_variable.RowHeadersVisible = false;//hid first columns
            dgv_variable.BackgroundColor = Color.White;
            dgv_variable.AllowUserToResizeRows = false;//固定行间距
            dgv_variable.AllowUserToAddRows = false;//删除最后一行空白行
            dgv_variable.SelectionMode = DataGridViewSelectionMode.FullRowSelect;//选中整行
            dgv_variable.MultiSelect = false;
            dgv_variable.CellBorderStyle = DataGridViewCellBorderStyle.None;

            foreach (DataGridViewColumn column in dgv_variable.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill; //自动拉长
                column.SortMode = DataGridViewColumnSortMode.NotSortable; //不能排序
            }

            #region Global Variables
            if (tabNumber == 0)
            {
                dgv_variable.Name = "GlobalVariableList";
                globalVariableTab.Controls.Clear();
                globalVariableTab.Controls.Add(dgv_variable);
            }
            #endregion

            #region Local Variables
            else //tabNumber == 1
            {
                #region TabPages[1]
                if (tabCon_Variable.TabPages.Count == 1)
                {
                    tabCon_Variable.TabPages.Add(CurrentSeq.Name, "Var:" + CurrentSeq.Name);
                }
                else
                {
                    tabCon_Variable.TabPages[1].Name = CurrentSeq.Name;
                    tabCon_Variable.TabPages[1].Text = "Var:" + CurrentSeq.Name;
                }
                #endregion

                dgv_variable.Name = CurrentSeq.Name + "VariableList";
                tabCon_Variable.TabPages[1].Controls.Clear();
                tabCon_Variable.TabPages[1].Controls.Add(dgv_variable);
                ShowVariables(1);
            }
            #endregion

            #region 事件
            dgv_variable.MouseClick += Dgv_Variable_MouseClick;
            dgv_variable.MouseDown += Dgv_Variable_MouseDown;
            dgv_variable.CellBeginEdit += Dgv_Variable_CellBeginEdit;
            dgv_variable.CellEndEdit += Dgv_Variable_CellEndEdit;
            dgv_variable.CellDoubleClick += Dgv_Variable_CellDoubleClick;
            #endregion

            tabCon_Variable.SelectedIndex = tabNumber;
        }

        private void CreateTDGVParameter()
        {
            #region 属性
            _paramTable = new TreeDataGridView(Image.FromFile(_expandImagePath), Image.FromFile(_NexpandImagePath));
            _paramTable.Name = "ParameterList";
            _paramTable.RowHeadersVisible = false;
            _paramTable.AllowUserToResizeRows = false;//固定行间距
            _paramTable.AllowUserToAddRows = false;//删除最后一行空白行
            _paramTable.Location = new Point();
            _paramTable.Dock = DockStyle.Fill;
            Parameter_panel.Controls.Add(_paramTable);

            #region Columns
            DataGridViewTextBoxColumn NameColumn = new DataGridViewTextBoxColumn();
            NameColumn.Name = "ParameterName";
            NameColumn.ReadOnly = true;
            NameColumn.HeaderText = "Parameter Name";
            _paramTable.Columns.Add(NameColumn);

            DataGridViewTextBoxColumn TypeColumn = new DataGridViewTextBoxColumn();
            TypeColumn.Name = "ParameterType";
            TypeColumn.ReadOnly = true;
            TypeColumn.HeaderText = "Type";
            _paramTable.Columns.Add(TypeColumn);

            DataGridViewTextBoxColumn ModifierColumn = new DataGridViewTextBoxColumn();
            ModifierColumn.Name = "ParameterModifier";
            ModifierColumn.ReadOnly = true;
            ModifierColumn.HeaderText = "In/Out/Ref";
            _paramTable.Columns.Add(ModifierColumn);

            DataGridViewTextBoxColumn ValueColumn = new DataGridViewTextBoxColumn();
            ValueColumn.Name = "ParameterValue";
            ValueColumn.ReadOnly = false;
            ValueColumn.HeaderText = "Value";
            _paramTable.Columns.Add(ValueColumn);

            DataGridViewButtonColumn FxColumn = new DataGridViewButtonColumn();
            FxColumn.Name = "Parameterfx";
            FxColumn.ReadOnly = false;
            FxColumn.HeaderText = "f(x)";
            FxColumn.Text = "f(x)";
            FxColumn.UseColumnTextForButtonValue = true;
            _paramTable.Columns.Add(FxColumn);

            DataGridViewButtonColumn CheckColumn = new DataGridViewButtonColumn();
            CheckColumn.Name = "ParameterCheck";
            CheckColumn.ReadOnly = false;
            CheckColumn.HeaderText = "";
            CheckColumn.Text = "√";
            CheckColumn.UseColumnTextForButtonValue = true;
            _paramTable.Columns.Add(CheckColumn);

            foreach (DataGridViewColumn column in _paramTable.Columns)
            {
                if (column.Index != 0)
                {
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill; //自动伸缩
                }
                column.SortMode = DataGridViewColumnSortMode.NotSortable; //不能排序
            }
            #endregion

            _paramTable.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;//固定表头
            _paramTable.BackgroundColor = Color.White;

            #endregion

            // 事件
            _paramTable.CellValueChanged += TdgvParamCellValueChanged;
            _paramTable.CellContentClick += TdgvParamCellContentClick;
            _paramTable.CellBeginEdit += TdgvParamCellEnterEdit;
        }

        #endregion

        #region 窗体事件

        private void Form1_Load(object sender, EventArgs e)
        {
            SessionChangedAction();
            if (viewController_Main.State == RunState.EditIdle.ToString())
            {
                // 隐藏运行时变量值窗体
                splitContainer_runtime.Panel1Collapsed = true;
            }
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TsfFilePath))
            {
                SelectTestModel();
            }
            else
            {
                try
                {
                    LoadSequence(TsfFilePath);
                    ShowValidSettingPages(GetCurrentStepType());
                }
                catch (ApplicationException ex)
                {
                    ShowMessage(ex.Message, "Load Sequence", MessageBoxIcon.Error);
                }
            }
            UpdateToolStripButtonsState();
        }

        private void SelectTestModel()
        {
            string configFile = _globalInfo.ConfigManager.GetConfig<string>("ModelConfigFilePath");
            if (string.IsNullOrWhiteSpace(configFile) || !File.Exists(configFile))
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Title = "Load Configuration File";
                openFileDialog.Filter = "Config file|*.ini";
                string workspaceDir = Environment.GetEnvironmentVariable("TESTFLOW_WORKSPACE");
                if (!string.IsNullOrWhiteSpace(workspaceDir) && Directory.Exists(workspaceDir))
                {
                    openFileDialog.InitialDirectory = workspaceDir;
                }
                DialogResult dialogResult = openFileDialog.ShowDialog(this);
                if (dialogResult != DialogResult.OK)
                {
                    ShowMessage("Configuration file not selected.", "Load Configuration", MessageBoxIcon.Warning);
                    return;
                }
                configFile = openFileDialog.FileName;
            }
            try
            {
                // 弹出模型选择窗口
                _globalInfo.Equipment = ModelManager.ShowModelSelectionDialog(configFile);
                // 写入最新的配置文件路径
                _globalInfo.ConfigManager.ApplyConfig("ModelConfigFilePath", configFile);
                _globalInfo.ConfigManager.WriteConfigData();

                string sequenceFile = _globalInfo.Equipment?.SelectedTestModel?.SequenceFile;
                if (!string.IsNullOrWhiteSpace(sequenceFile) && File.Exists(sequenceFile))
                {
                    LoadSequence(sequenceFile);
                }
                else
                {
                    ShowMessage("Sequence file not exist.", "Load Configuration", MessageBoxIcon.Warning);
                }
            }
            catch (ApplicationException ex)
            {
                ShowMessage(ex.Message, "Load Configuration", MessageBoxIcon.Error);
            }
        }

        private void SessionChangedAction()
        {
            AuthenticationSession session = _globalInfo.Session;
            StatusUseValue.Text = session.UserName;
            toolStripStatusLabel_userGroup.Text = session.UserGroup.ToString();
            if (session.HasAuthority(AuthorityDefinition.CreateSequence) &&
                session.HasAuthority(AuthorityDefinition.EditSequence))
            {
                viewController_Main.State = RunState.EditIdle.ToString();
            }
            else if (session.HasAuthority(AuthorityDefinition.RunSequence))
            {
                viewController_Main.State = RunState.RunIdle.ToString();
            }
            else
            {
                ShowMessage("Current session cannot operation sequence", "Authentication", MessageBoxIcon.Error);
                this.Dispose();
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !CheckIfCanClose();
        }
        
        #endregion

        #region 主菜单事件

        #region File子菜单项事件

        private void newToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            CreateNewSequence();
            if (tabCon_Step.TabCount > 0)
            {
                tabCon_Step.SelectedIndex = 0;
            }
        }

        private void openToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = openFileDialog_sequence.ShowDialog(this);
            if (dialogResult != DialogResult.OK)
            {
                return;
            }
            try
            {
                LoadSequence(openFileDialog_sequence.FileName);
                ShowValidSettingPages(GetCurrentStepType());
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, "Load Sequence", MessageBoxIcon.Error);
            }
        }
        
        private void saveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (null == SequenceGroup)
            {
                return;
            }
            try
            {
                _globalInfo.Session.CheckAuthority(AuthorityDefinition.EditSequence);
                SaveSequence(SequenceGroup.Info.SequenceGroupFile);
            }
            catch (ApplicationException ex)
            {
               ShowMessage(ex.Message, "Save Sequence", MessageBoxIcon.Error);
            }
        }

        private void SaveAsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (null == SequenceGroup)
            {
                return;
            }
            SaveAsSequence();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CheckIfCanClose())
            {
                this.Close();
            }
        }

        private bool CheckIfCanClose()
        {
            string currentState = viewController_Main.State;
            if (currentState != RunState.EditIdle.ToString() && currentState != RunState.RunIdle.ToString() &&
                currentState != RunState.RunOver.ToString())
            {
                ShowMessage("The application cannot be closed when a test is running.", "Close", MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        #endregion

        #region Edit子菜单项事件

        private void addSequenceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (null == CurrentSeq)
            {
                return;
            }
            string sequenceName;
            int index = 1;
            do
            {
                sequenceName = $"Sequence{index++}";
            } while (SequenceGroup.Sequences.Any(item => item.Name.Equals(sequenceName)));
            TestflowDesigntimeSession.AddSequence(sequenceName, "", SequenceGroup.Sequences.Count);
            TreeNode currentNode = FindTreeNode(CurrentSeq);
            currentNode.Parent.Nodes.Add(sequenceName);
        }

        private void actionToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            AddAction_Click(null, null);
        }

        private void sequenceCallToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AddSequenceCall_Click(null, null);
        }

        private void editSequenceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                viewController_Main.State = "EditIdle";
            }
            catch (ApplicationException ex)
            {
                ShowMessage(ex.Message, "Error", MessageBoxIcon.Error);
            }
        }

        private void startTimingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddTimingStep(Constants.ActionType, Constants.StartTimeVar, Constants.StartTimingStepName);
        }


        private void endTimingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddTimingStep(Constants.ActionType, Constants.EndTimeVar, Constants.EndTimingStepName);
        }

        private void waitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddWaitStep(Constants.ActionType, Constants.WaitStepName);
        }

        private void loadLibraryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = openFileDialog_assembly.ShowDialog(this);
            if (dialogResult != DialogResult.OK)
            {
                return;
            }
            try
            {
                _globalInfo.TestflowEntity.ComInterfaceManager.GetComponentInterface(openFileDialog_assembly.FileName);
            }
            catch (ApplicationException ex)
            {
                ShowMessage(ex.Message, "Load Assembly", MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Configure子菜单项事件

        private void reportToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void breakIfFailedToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void configureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConfigureForm configureForm = new ConfigureForm(_globalInfo);
            configureForm.ShowDialog(this);
        }


        private void selectModelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectTestModel();
        }

        #endregion

        #region Run子菜单项事件

        private void startToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            startButton_Click(null, null);
        }

        private void suspendToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SuspendSequence();
        }

        private void stopToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            StopRunning();
            viewController_Main.State = RunState.RunProcessing.ToString();
        }

        #endregion
        
        #region User子菜单项事件

        private void managerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AuthenticationSession session = AuthenticationManage.ShowUserManageForm(_globalInfo.Session, this);
            if (null == session)
            {
                MessageBox.Show("已退出用户会话。", "用户", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            this._globalInfo.Session = session;
            SessionChangedAction();
        }

        private void reloginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AuthenticationSession session = _globalInfo.Session;
            _globalInfo.Session = AuthenticationManage.Relogin(session);
            if (null == _globalInfo.Session)
            {
                ShowMessage("Relogin failed.", "Login", MessageBoxIcon.Error);
                this.Close();
            }
            SessionChangedAction();
        }

        #endregion

        #endregion

        #region 运行时信息窗口事件

        private void button_copyOutput_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox_output.Text))
            {
                Clipboard.SetText(textBox_output.Text);
            }
        }

        private void button_clearOutput_Click(object sender, EventArgs e)
        {
            textBox_output.Text = string.Empty;
        }

        #endregion

        #region 快捷菜单事件

        #region 运行快捷菜单事件

        private void toolStripButton_Suspend_Click(object sender, EventArgs e)
        {
            SuspendSequence();
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            if (null == SequenceGroup)
            {
                return;
            }
            // 如果是运行时暂停阻塞状态则继续执行
            if (viewController_Main.State == RunState.RunBlock.ToString())
            {
                IDebuggerHandle debuggerHandle =
                        _globalInfo.TestflowEntity.EngineController.GetRuntimeInfo<IDebuggerHandle>("DebugHandle");
                debuggerHandle.Continue(0);
                viewController_Main.State = RunState.Running.ToString();
            }
            else
            {
                viewController_Main.State = RunState.RunIdle.ToString();
                try
                {
                    _globalInfo.Session.CheckAuthority(AuthorityDefinition.RunSequence);
                    // 更新显示当前页面的所有step的unit
                    ShowSteps();
                    RunSequence();
                }
                catch (AuthenticationException ex)
                {
                    Log.Print(LogLevel.ERROR, ex.Message);
                    ShowMessage(ex.Message, "Run Sequence", MessageBoxIcon.Error);
                }
            }
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            StopRunning();
            viewController_Main.State = RunState.RunProcessing.ToString();
        }

        #endregion

        #endregion

        #region 右键菜单事件

        #region 子序列列表右键菜单事件

        private void AddSeq_Click(object sender, EventArgs e)
        {
            AddSubSequence();
        }

        private void DeleteSeq_Click(object sender, EventArgs e)
        {
            DeleteSubSequence();
        }

        #endregion

        #region 步骤列表右键菜单事件

        private void AddAction_Click(object sender, EventArgs e)
        {
            AddStep(Constants.ActionType);
        }

        private void AddSequenceCall_Click(object sender, EventArgs e)
        {
            AddStep(Constants.SeqCallType);
        }

        private void stringValueTestToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AddStep(Constants.TestType, Constants.StringLimit);
        }

        private void numericLimitTestToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AddStep(Constants.TestType, Constants.NumericLimit);
        }

        private void booleanTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddStep(Constants.TestType, Constants.BoolLimit);
        }

        private void ToolStripMenuItem_commonTest_Click(object sender, EventArgs e)
        {
            AddStep("Test");
        }

        private void booleanTestToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (null == CurrentStep)
            {
                return;
            }
            int insertIndex = CurrentStep.Index;
            InsertStep(Constants.TestType, insertIndex, Constants.BoolLimit);
        }

        private void numericLimitTestToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (null == CurrentStep)
            {
                return;
            }
            int insertIndex = CurrentStep.Index;
            InsertStep(Constants.TestType, insertIndex, Constants.NumericLimit);
        }

        private void stringValueTestToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (null == CurrentStep)
            {
                return;
            }
            int insertIndex = CurrentStep.Index;
            InsertStep(Constants.TestType, insertIndex, Constants.StringLimit);
        }

        private void commonTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (null == CurrentStep)
            {
                return;
            }
            int insertIndex = CurrentStep.Index;
            InsertStep(Constants.TestType, insertIndex);
        }

        private void actionToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            if (null == CurrentStep)
            {
                return;
            }
            int insertIndex = CurrentStep.Index;
            InsertStep(Constants.ActionType, insertIndex);
        }

        private void sequenceCallToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (null == CurrentStep)
            {
                return;
            }
            int insertIndex = CurrentStep.Index;
            InsertStep(Constants.SeqCallType, insertIndex);
        }


        private void startTimingToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (null == CurrentStep)
            {
                return;
            }
            int insertIndex = CurrentStep.Index;
            InsertTimingStep(Constants.ActionType, insertIndex, Constants.StartTimeVar, Constants.StartTimingStepName);
        }


        private void endTimingToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (null == CurrentStep)
            {
                return;
            }
            int insertIndex = CurrentStep.Index;
            InsertTimingStep(Constants.ActionType, insertIndex, Constants.EndTimeVar, Constants.EndTimingStepName);
        }


        private void waitToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (null == CurrentStep)
            {
                return;
            }
            int insertIndex = CurrentStep.Index;
            InsertWaitStep(Constants.ActionType, insertIndex, Constants.WaitStepName);
        }

        #endregion

        #region 变量窗体右键菜单事件


        private void cMS_DgvVariable_Opening(object sender, CancelEventArgs e)
        {
            string varName = VaraibleTable.CurrentRow?.Cells["VariableName"].Value.ToString() ?? null;
            bool isGlobalVariable = VaraibleTable.Name.Equals("GlobalVariableList");
            // 系统变量不能删除
            if (string.IsNullOrWhiteSpace(varName) || IsSystemVariable(varName))
            {
                toolStripMenuItem2.Enabled = false;
            }
            else
            {
                toolStripMenuItem2.Enabled = true;
            }
        }

        private void AddVariable_Click(object sender, EventArgs e)
        {
            if (null == TestflowDesigntimeSession)
            {
                return;
            }
            IVariable variable = null;
            string VariableName;

            #region VariableName
            do
            {
                _currentVariableId++;
                VariableName = "Variable" + _currentVariableId.ToString();
                variable = TestflowDesigntimeSession.FindVariableInParent(VariableName, VaraibleTable.Name.Equals("GlobalVariableList") ?
                                                                              (ISequenceFlowContainer)SequenceGroup : CurrentSeq);
            }
            while (variable != null);
            #endregion

            AddVariable(VaraibleTable.Name.Equals("GlobalVariableList") ? (ISequenceFlowContainer)SequenceGroup : CurrentSeq,
                        VaraibleTable.Name.Equals("GlobalVariableList") ? 0 : 1,
                        VariableName, false);

        }

        private void DeleteVariable_Click(object sender, EventArgs e)
        {
            string varName = VaraibleTable.CurrentRow.Cells["VariableName"].Value.ToString();
            VaraibleTable.Rows.Remove(VaraibleTable.CurrentRow);
            TestflowDesigntimeSession.RemoveVariable(VaraibleTable.Name.Equals("GlobalVariableList") ?
                                                               (ISequenceFlowContainer)SequenceGroup : CurrentSeq,
                                                      varName);

        }

        #endregion

        #endregion
        
        private void buttonOpenReport_Click(object sender, EventArgs e)
        {
            if (_eventController?.CurrentReport == null || !File.Exists(_eventController.CurrentReport))
            {
                return;
            }
            try
            {
                Process.Start("notepad.exe", _eventController.CurrentReport);
            }
            catch (Exception ex)
            {
                Log.Print(LogLevel.ERROR, ex.Message);
            }
        }

        private void button_openReportDir_Click(object sender, EventArgs e)
        {
            if (_eventController?.ReportDir == null || !Directory.Exists(_eventController.ReportDir))
            {
                return;
            }
            try
            {
                Process.Start(_eventController.ReportDir);
            }
            catch (Exception ex)
            {
                Log.Print(LogLevel.ERROR, ex.Message);
            }
        }

        #region Variable相关事件

        private void Dgv_Variable_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ((DataGridView)sender).ContextMenuStrip = this.cMS_DgvVariable;
            }
        }

        private void Dgv_Variable_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
            {
                return;
            }
            DataGridView varTable = sender as DataGridView;
            int selectedIndex = -1;
            for (int i = 0; i < varTable.RowCount; i++)
            {
                Rectangle displayRectangle = varTable.GetRowDisplayRectangle(i, true);
                if (displayRectangle.Top <= e.Y && displayRectangle.Bottom >= e.Y)
                {
                    selectedIndex = i;
                    break;
                }
            }
            if (selectedIndex == -1)
            {
                if (varTable.RowCount == 0)
                {
                    return;
                }
                selectedIndex = varTable.RowCount - 1;
            }
            varTable.CurrentCell = varTable.Rows[selectedIndex].Cells[StepTableNameCol];
        }

        private void SetVariableIntoInstance(int tabIndex, IVariable variable)
        {
            #region 选中Tab
            tabCon_Variable.SelectedIndex = tabIndex;
            #endregion

            #region 寻找该variable的所在的行
            foreach (DataGridViewRow row in VaraibleTable.Rows)
            {
                if (row.Cells["VariableName"].Value.Equals(variable.Name))
                {
                    row.Cells["VariableType"].ReadOnly = true;
                    row.Cells["VariableValue"].Value = _currentClassDescription.ClassType.Name;

                    TestflowDesigntimeSession.SetVariableType(variable, _currentClassDescription.ClassType);

                    break;
                }
            }
            #endregion
        }

        private IVariable AddVariable(ISequenceFlowContainer parent, int tabIndex, string variableName, bool isObject = false)
        {
            #region 选中Tab
            tabCon_Variable.SelectedIndex = tabIndex;
            #endregion

            #region 添加行

            int rowIndex = VaraibleTable.Rows.Add(variableName, isObject ? $"Object" : "Numeric", isObject? "" : "0");
            VaraibleTable.Rows[rowIndex].Tag = variableName;

            #endregion

            // 选中行
            VaraibleTable.Rows[VaraibleTable.RowCount - 1].Selected = true;

            // Testflow: AddVariable
            IVariable variable = TestflowDesigntimeSession.AddVariable(parent, variableName, "", VaraibleTable.RowCount - 1);
            variable.VariableType = (isObject) ? VariableType.Class : VariableType.Undefined;

            // 判断Object
            if (isObject)
            {
                SetVariableIntoInstance(1, variable);
            }
            return variable;
        }

        
        private void Dgv_Variable_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            DataGridView variableTable = VaraibleTable;
            string varName = variableTable.Rows[e.RowIndex].Cells["VariableName"].Value.ToString();
            // 如果是系统变量则退出编辑
            if (IsSystemVariable(varName))
            {
                e.Cancel = true;
                return;
            }
            _oldVariableType = variableTable.Rows[e.RowIndex].Cells["VariableType"].Value.ToString();
            _oldVariableValue = variableTable.Rows[e.RowIndex].Cells["VariableValue"].Value.ToString();
        }
        
        private void Dgv_Variable_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            string oldName = (string) VaraibleTable.Rows[e.RowIndex].Tag;
            string name = VaraibleTable.Rows[e.RowIndex].Cells["VariableName"].Value?.ToString();
            string type = VaraibleTable.Rows[e.RowIndex].Cells["VariableType"].Value.ToString();
            string value = VaraibleTable.Rows[e.RowIndex].Cells["VariableValue"].Value?.ToString();
            
            #region 啥都没变
            if (name.Equals(oldName) && type.Equals(_oldVariableType) && value.Equals(_oldVariableValue))
            {
                return;
            }
            #endregion

            ISequenceFlowContainer variableParent = VaraibleTable.Name.Equals("GlobalVariableList") ?
                (ISequenceFlowContainer)SequenceGroup : CurrentSeq;
            

            #region 改名
            if (VaraibleTable.Columns[e.ColumnIndex].Name.Equals("VariableName"))
            {
                IVariable variable = TestflowDesigntimeSession.FindVariableInParent(oldName, variableParent);
                IVariableCollection variables = variableParent is ISequence
                    ? ((ISequence) variableParent).Variables
                    : ((ISequenceGroup) variableParent).Variables;
                //todo 改名字的判断
                if (IsValidVariableName(name, variables))
                {
                    variable.Name = name;
                    VaraibleTable.Rows[e.RowIndex].Tag = name;
                }
                else
                {
                    ShowMessage("Invalid name.", "Rename", MessageBoxIcon.Error);
                    VaraibleTable[0, e.RowIndex].Value = oldName;
                }
            }
            #endregion

            #region 改类型
            else if (VaraibleTable.Columns[e.ColumnIndex].Name.Equals("VariableType"))
            {
                IVariable variable = TestflowDesigntimeSession.FindVariableInParent(name, variableParent);
                ITypeData typeData = null;
                string defaultValue = "";
                //判断新类型，从service.compoents寻找对应typeData,并且重置值
                switch (type)
                {
                    case "":
                        break;
                    case "Numeric":
                        //todo 做个numeric的东西
                        typeData = _testflowDesigntimeService.Components["mscorlib"].VariableTypes.FirstOrDefault(item => item.Name.Equals("Double"));
                        defaultValue = "0";
                        variable.ReportRecordLevel = RecordLevel.Trace;
                        break;
                    case "String":
                        typeData = _testflowDesigntimeService.Components["mscorlib"].VariableTypes.FirstOrDefault(item => item.Name.Equals("String"));
                        defaultValue = "";
                        variable.ReportRecordLevel = RecordLevel.Trace;
                        break;
                    case "Boolean":
                        typeData = _testflowDesigntimeService.Components["mscorlib"].VariableTypes.FirstOrDefault(item => item.Name.Equals("Boolean"));
                        defaultValue = false.ToString();
                        variable.ReportRecordLevel = RecordLevel.Trace;
                        break;
                    case "Object":
                        variable.VariableType = VariableType.Class;
                        variable.ReportRecordLevel = RecordLevel.None;
                        break;
                }

                #region 改值
                VaraibleTable.Rows[e.RowIndex].Cells["VariableValue"].Value = defaultValue;
                TestflowDesigntimeSession.SetVariableValue(variable, defaultValue);
                #endregion

                #region 改类型
                TestflowDesigntimeSession.SetVariableType(variable, typeData);
                #endregion
            }
            #endregion

            #region 改值
            else
            {
                //如果类型为空，则值无效
                if (type.Equals(""))
                {
                    //todo "Enter Type first." I18n
                    MessageBox.Show("Enter Type first");
                    VaraibleTable.Rows[e.RowIndex].Cells["VariableValue"].Value = "";
                    TestflowDesigntimeSession.SetVariableValue(name, "");
                    return;
                }

                TestflowDesigntimeSession.SetVariableValue(name, value);

                IWarningInfo warningInfo = null;
                if (VaraibleTable.Name.Equals("GlobalVariableList"))
                {
                    warningInfo = _globalInfo.TestflowEntity.ParameterChecker.CheckVariableValue(SequenceGroup, name);
                }
                else
                {
                    warningInfo = _globalInfo.TestflowEntity.ParameterChecker.CheckVariableValue(CurrentSeq, name);
                }

                //todo WarningInfo
                if (warningInfo != null)
                {
                    MessageBox.Show(warningInfo.Infomation);
                }
            }
            #endregion
        }

        //todo 问一下这个variable的editor怎么弹出来
        private void Dgv_Variable_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) { return; }
            DataGridView varaibleTable = VaraibleTable;
            string name = varaibleTable.Rows[e.RowIndex].Cells["VariableName"].Value.ToString();
            string type = varaibleTable.Rows[e.RowIndex].Cells["VariableType"].Value.ToString();
            string value = varaibleTable.Rows[e.RowIndex].Cells["VariableValue"].Value.ToString();

            if (varaibleTable.Columns[e.ColumnIndex].Name.Equals("VariableValue"))
            {
                IVariable variable = TestflowDesigntimeSession.FindVariableInParent(name, varaibleTable.Name.Equals("GlobalVariableList") ?
                                                                                        (ISequenceFlowContainer)SequenceGroup : CurrentSeq);
                ValueEditor editor = null;
                ObjectEditor objEditor = null;
                switch (type)
                {
                    case "":
                        return;
                    case "Numeric":
                        editor = new NumericEditor(value);
                        break;
                    case "String":
                        editor = new StringEditor(value);
                        break;
                    case "Boolean":
                        editor = new BooleanEditor(value);
                        break;
                    case "Object":
                        if (!IsSystemVariable(variable.Name))
                        {
                            objEditor = new ObjectEditor(variable);
                        }
                        break;
                }
                if (null != editor)
                {
                    //显示用户编辑Numeric Form
                    editor.ShowDialog(this);
                    if (editor._valueChanged)
                    {
                        varaibleTable.Rows[e.RowIndex].Cells["VariableValue"].Value = editor._value;
                        TestflowDesigntimeSession.SetVariableValue(variable, editor._value);
                    }
                    editor.Dispose();
                }
                else
                {
                    objEditor?.ShowDialog(this);
                    varaibleTable.Rows[e.RowIndex].Cells["VariableValue"].Value = variable.Type?.Name ?? string.Empty;
                }
            }
        }
        #endregion

        // Sequence
        private void tabCon_Seq_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 判断空dgv
            if (SequenceTable.Rows.Count == 0)
            {
                if (tabCon_Variable.TabPages.Count > 1)
                {
                    tabCon_Variable.TabPages.RemoveAt(1);
                }
                if (tabCon_Step.TabPages.Count > 0)
                {
                    tabCon_Step.TabPages.RemoveAt(0);
                }
                tabControl_settings.Visible = false;
            }
            else
            {
                // Show step
                ShowSteps();

                // CreateDGVvariable
                CreateDGVVariable(1);
            }
        }

        #region Sequence表格事件

        //sequence改名字
        private void Dgv_Seq_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // 如果不是名称的列变更，则返回
            if (e.ColumnIndex != SeqTableNameCol)
            {
                return;
            }
            string newName = SequenceTable[e.ColumnIndex, e.RowIndex].Value.ToString();
            string lastSeqName = CurrentSeq.Name;
            if (!IsValidSequenceName(newName))
            {
                ShowMessage("Invalid sequence name", "Rename", MessageBoxIcon.Error);
                SequenceTable[e.ColumnIndex, e.RowIndex].Value = lastSeqName;
                return;
            }

            // 如果TabPages里面有此sequence的键名，替换成新的键名
            tabPage_stepData.Name = newName;
            tabPage_stepData.Text = "Steps:" + newName;

            string lastSeqCallName = $"{Constants.SeqCallType}:{lastSeqName}";
            string newSeqCallName = $"{Constants.SeqCallType}:{newName}";
            ModifySequenceCallName(SequenceGroup.SetUp, lastSeqCallName, newSeqCallName);
            ModifySequenceCallName(SequenceGroup.TearDown, lastSeqCallName, newSeqCallName);
            foreach (ISequence sequence in SequenceGroup.Sequences)
            {
                ModifySequenceCallName(sequence, lastSeqCallName, newSeqCallName);
            }
            CurrentSeq.Name = newName;
            UpdateSequenceCallList();
            ISequenceStep currentStep = CurrentStep;
            if (null != currentStep && Utility.IsSequenceCall(currentStep))
            {
                comboBox_SequenceCall.Text = Utility.GetSequenceCallName(currentStep.SubSteps[0].Name);
            }
        }

        private void ModifySequenceCallName(ISequence sequence, string lastSeqCallName, string newSeqCallName)
        {
            foreach (ISequenceStep step in sequence.Steps)
            {
                if (null != step.SubSteps && step.SubSteps.Count > 0 && step.SubSteps[0].Name.Equals(lastSeqCallName))
                {
                    step.SubSteps[0].Name = newSeqCallName;
                    // 连带修改Description
                    StepInfoCreator.SetStepDescriptionInfo(step, Constants.SeqCallType);
                }
            }
        }

        private bool IsValidSequenceName(string name)
        {
            return !string.IsNullOrWhiteSpace(name) && !SequenceGroup.Sequences.Any(item => item.Name.Equals(name));
        }

        private bool IsValidVariableName(string name, IVariableCollection variables)
        {
            return !string.IsNullOrWhiteSpace(name) && !IsSystemVariable(name);
        }

        private bool IsSystemVariable(string name)
        {
            return name.Equals(Constants.ContinueVariable) ||
                   name.Equals(Constants.TimingEnableVar) ||
                   name.Equals(Constants.StartTimeVar) || name.Equals(Constants.EndTimeVar) ||
                   name.Equals(Constants.SerialNoVarName) || name.Equals(Constants.DutIndexVarName) ||
                   name.Equals(Constants.UutIndexVar);
        }

        //sequence列表选择改变
        private void Dgv_Seq_CurrentCellChanged(object sender, EventArgs e)
        {
            // 删除Sequence以后CurrentRow为null
            if (SequenceTable.CurrentRow == null)
            {
                return;
            }
            ShowSteps();
            if (_stepTable.RowCount > 0)
            {
                _stepTable.CurrentCell = _stepTable.Rows[0].Cells[StepTableNameCol];
            }
            else
            {
                _stepTable.CurrentCell = null;
            }
            
        }

        private void Dgv_Seq_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                SequenceTable.ContextMenuStrip = this.cMS_DgvSeq;
            }
        }

        #endregion

        #region Step表格关联事件

        private bool _stepRowChange = false;

        private void Dgv_Step_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right || tabCon_Step.TabCount < 2)
            {
                return;
            }
            DataGridView stepTable = _stepTable;
            int selectedIndex = -1;
            for (int i = 0; i < stepTable.RowCount; i++)
            {
                Rectangle displayRectangle = stepTable.GetRowDisplayRectangle(i, true);
                if (displayRectangle.Top <= e.Y && displayRectangle.Bottom >= e.Y)
                {
                    selectedIndex = i;
                    break;
                }
            }
            if (selectedIndex == -1)
            {
                if (stepTable.RowCount == 0)
                {
                    return;
                }
                selectedIndex = stepTable.RowCount - 1;
            }
            stepTable.CurrentCell = stepTable.Rows[selectedIndex].Cells[StepTableNameCol];
        }
        
        private void Dgv_Step_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            
            //改name
            if (_stepTable.Columns[e.ColumnIndex].Name.Equals("StepName"))
            {
                CurrentSeq.Steps[e.RowIndex].Name = _stepTable.Rows[e.RowIndex].Cells["StepName"].Value.ToString();
            }
            //改description
            else if (_stepTable.Columns[e.ColumnIndex].Name.Equals("StepDescription"))
            {
                CurrentStep.Description = _stepTable.Rows[e.RowIndex].Cells["StepDescription"].Value.ToString();
            }
        }

        private void cMS_DgvStep_Opening(object sender, CancelEventArgs e)
        {
            ISequenceStep step = CurrentStep;
            bool isSequenceCall = (null != step && step.SubSteps[0].Name.Contains(Constants.SeqCallType));
            ToolStripMenuItem_gotoSubSequence.Visible = isSequenceCall;
            ToolStripMenuItem_gotoSubSequence.Enabled = isSequenceCall;
        }

        //step行改变
        private void Dgv_Step_SelectionChanged(object sender, EventArgs e)
        {
            if (_stepTable.CurrentRow == null || _stepTable.CurrentRow.Index < 0 || _internalOperation)
            {
                return;
            }
            ShowValidSettingPages(GetCurrentStepType());
            _stepRowChange = true;
            UpdateSettings();
            _stepRowChange = false;
        }
        
        //添加步骤
        private void AddStep(string stepType, string limitType = null)
        {
            if (!CheckAuthority(AuthorityDefinition.EditSequence))
            {
                return;
            }

            _internalOperation = true;

            string stepName;
            // 获取可用的step名称
            do
            {
                _currentStepId ++;
                stepName = "Step" + _currentStepId;
            }
            while (CurrentSeq.Steps.Any(item => item.Name.Equals(stepName)));

            // 添加新的行，记录新行的位置
            int index = _stepTable.Rows.Add(GetImage(stepType), GetShowStepName(stepName), "", "", "", "", "");
            _stepTable.Rows[index].Selected = true;//选择新添加的行
            _stepTable.CurrentCell = _stepTable.Rows[index].Cells[0];//移动箭头

            // Testflow: 添加step
            ISequenceStep currentStep = TestflowDesigntimeSession.AddSequenceStep(CurrentSeq, stepName, "", index);
            currentStep.RecordStatus = true;
            TestflowDesigntimeSession.AddSequenceStep(currentStep, stepType, "", 0);
            SetStepProperty(currentStep, stepType);

            // 清空_currentDescriptions
            _currentComDescription = null;
            _currentClassDescription = null;

            // 展示空的settings
            ShowValidSettingPages(GetCurrentStepType());
            UpdateSettings();

            _internalOperation = false;

            if (null != limitType)
            {
                AddLimitFunction(currentStep, limitType);
            }

            currentStep.AssertFailedAction = FailedAction.Continue;
            currentStep.InvokeErrorAction = FailedAction.BreakLoop;
        }

        private void SetStepProperty(ISequenceStep step, string stepType)
        {
            bool breakIfFailed = false;
            bool recordStatus = false;
            switch (stepType)
            {
                case Constants.ActionType:
                    breakIfFailed = _globalInfo.ConfigManager.GetConfig<bool>("ActionBreakIfFailed");
                    recordStatus = _globalInfo.ConfigManager.GetConfig<bool>("ActionRecordStatus");
                    break;
                case Constants.TestType:
                    breakIfFailed = _globalInfo.ConfigManager.GetConfig<bool>("TestBreakIfFailed");
                    recordStatus = _globalInfo.ConfigManager.GetConfig<bool>("TestRecordStatus");
                    break;
                case Constants.SeqCallType:
                    breakIfFailed = _globalInfo.ConfigManager.GetConfig<bool>("SeqCallBreakIfFailed");
                    recordStatus = _globalInfo.ConfigManager.GetConfig<bool>("SeqCallRecordStatus");
                    break;
                default:
                    break;
            }
            step.BreakIfFailed = breakIfFailed;
            step.RecordStatus = recordStatus;
            if (null != step.SubSteps)
            {
                foreach (ISequenceStep subStep in step.SubSteps)
                {
                    subStep.BreakIfFailed = breakIfFailed;
                    subStep.RecordStatus = recordStatus;
                }
            }
        }

        //添加步骤
        private void AddTimingStep(string stepType, string timingVariable, string defaultStepName)
        {
            if (!CheckAuthority(AuthorityDefinition.EditSequence))
            {
                return;
            }

            _internalOperation = true;

            // 获取可用的step名称
            string stepName = defaultStepName;
            while (CurrentSeq.Steps.Any(item => item.Name.Equals(stepName)))
            {
                stepName = defaultStepName + _currentStepId;
                _currentStepId++;
            }

            // 添加新的行，记录新行的位置
            int index = _stepTable.Rows.Add(GetImage(stepType), stepName, "", "", "", "", "");
            _stepTable.Rows[index].Selected = true;//选择新添加的行
            _stepTable.CurrentCell = _stepTable.Rows[index].Cells[0];//移动箭头

            // Testflow: 添加step
            ISequenceStep currentStep = TestflowDesigntimeSession.AddSequenceStep(CurrentSeq, stepName, "", index);
            currentStep.RecordStatus = true;
            TestflowDesigntimeSession.AddSequenceStep(currentStep, stepType, "", 0);
            IFunctionData timingFunction = GetStartTimingFunction();
            ISequenceStep timingStep = TestflowDesigntimeSession.AddSequenceStep(currentStep, timingFunction, Constants.MethodStepName, 
                Constants.MethodStepName, 1);
            TestflowDesigntimeSession.SetReturn(timingVariable, timingStep);
            // 更新Settings和Description
            StepInfoCreator.SetStepDescriptionInfo(currentStep, stepType);
            StepInfoCreator.SetStepSettingInfo(currentStep, stepType);
            _stepTable.Rows[index].Cells[StepTableDescCol].Value = currentStep.Description;
            _stepTable.Rows[index].Cells[StepTableSettingCol].Value = currentStep.SubSteps[0].Description;

            SetStepProperty(currentStep, stepType);

            // 如果变量不存在则创建
            CreateDefaultVariable();

            // 清空_currentDescriptions
            _currentComDescription = null;
            _currentClassDescription = null;

            // 展示空的settings
            ShowValidSettingPages(GetCurrentStepType());
            UpdateSettings();

            _internalOperation = false;

            // 默认断言失败后继续执行，调用出现异常，则进入下次loop
            currentStep.AssertFailedAction = FailedAction.Continue;
            currentStep.InvokeErrorAction = FailedAction.BreakLoop;
        }

        //添加等待步骤
        private void AddWaitStep(string stepType, string defaultStepName)
        {
            if (!CheckAuthority(AuthorityDefinition.EditSequence))
            {
                return;
            }

            _internalOperation = true;

            // 获取可用的step名称
            string stepName = defaultStepName;
            ISequence parentSequence = CurrentSeq;
            while (parentSequence.Steps.Any(item => item.Name.Equals(stepName)))
            {
                stepName = defaultStepName + _currentStepId;
                _currentStepId++;
            }

            // 添加新的行，记录新行的位置
            int index = _stepTable.Rows.Add(GetImage(stepType), stepName, "", "", "", "", "");
            _stepTable.Rows[index].Selected = true;//选择新添加的行
            _stepTable.CurrentCell = _stepTable.Rows[index].Cells[0];//移动箭头

            // Testflow: 添加step
            ISequenceStep currentStep = TestflowDesigntimeSession.AddSequenceStep(parentSequence, stepName, "", index);
            currentStep.RecordStatus = true;
            TestflowDesigntimeSession.AddSequenceStep(currentStep, stepType, "", 0);
            IFunctionData timingFunction = GetWaitFunction();
            ISequenceStep timingStep = TestflowDesigntimeSession.AddSequenceStep(currentStep, timingFunction, Constants.MethodStepName,
                Constants.MethodStepName, 1);
            timingStep.Function.Parameters[0].Value = Constants.DefaultWaitTime;
            timingStep.Function.Parameters[0].ParameterType = ParameterType.Value;
            // 更新Settings和Description
            StepInfoCreator.SetStepDescriptionInfo(currentStep, stepType);
            StepInfoCreator.SetStepSettingInfo(currentStep, stepType);
            _stepTable.Rows[index].Cells[StepTableDescCol].Value = currentStep.Description;
            _stepTable.Rows[index].Cells[StepTableSettingCol].Value = currentStep.SubSteps[0].Description;

            SetStepProperty(currentStep, stepType);

            // 清空_currentDescriptions
            _currentComDescription = null;
            _currentClassDescription = null;

            // 展示空的settings
            ShowValidSettingPages(GetCurrentStepType());
            UpdateSettings();

            _internalOperation = false;

            // 默认断言失败后继续执行，调用出现异常，则进入下次loop
            currentStep.AssertFailedAction = FailedAction.Continue;
            currentStep.InvokeErrorAction = FailedAction.BreakLoop;
        }

        private void GetDefaultProperty(string stepType, out FailedAction failedAction, out bool recordStatus)
        {
            bool breakIfFailed = true;
            if (stepType.Equals(Constants.ActionType))
            {
                breakIfFailed = _globalInfo.ConfigManager.GetConfig<bool>("ActionBreakIfFailed");
                recordStatus = _globalInfo.ConfigManager.GetConfig<bool>("ActionRecordStatus");
            }
            else if (stepType.Equals(Constants.TestType))
            {
                breakIfFailed = _globalInfo.ConfigManager.GetConfig<bool>("TestBreakIfFailed");
                recordStatus = _globalInfo.ConfigManager.GetConfig<bool>("TestRecordStatus");
            }
            else
            {
                breakIfFailed = _globalInfo.ConfigManager.GetConfig<bool>("SeqCallBreakIfFailed");
                recordStatus = _globalInfo.ConfigManager.GetConfig<bool>("SeqCallRecordStatus");
            }
            failedAction = breakIfFailed ? FailedAction.NextLoop : FailedAction.Continue;
        }

        private IFunctionData GetStartTimingFunction()
        {
            IList<IFuncInterfaceDescription> funcList =
                _testflowDesigntimeService.Components["TestStationLimit"].Classes.First(
                    item => item.Name.Equals("Timing")).Functions;

            return _globalInfo.TestflowEntity.SequenceManager.CreateFunctionData(
                    funcList.FirstOrDefault(item => item.Name.Equals("StartTiming")));
        }

        private IFunctionData GetWaitFunction()
        {
            IList<IFuncInterfaceDescription> funcList =
                _testflowDesigntimeService.Components["TestStationLimit"].Classes.First(
                    item => item.Name.Equals("Timing")).Functions;

            return _globalInfo.TestflowEntity.SequenceManager.CreateFunctionData(
                    funcList.FirstOrDefault(item => item.Name.Equals("Sleep")));
        }

        //添加步骤
        private void InsertStep(string stepType, int insertIndex, string limitType = null)
        {
            if (!CheckAuthority(AuthorityDefinition.EditSequence))
            {
                return;
            }

            _internalOperation = true;

            string stepName;
            // 获取可用的step名称
            do
            {
                _currentStepId++;
                stepName = "Step" + _currentStepId;
            }
            while (CurrentSeq.Steps.Any(item => item.Name.Equals(stepName)));

            // 添加新的行，记录新行的位置
            _stepTable.Rows.Insert(insertIndex, GetImage(stepType), GetShowStepName(stepName), "", "", "", "", "");
            _stepTable.Rows[insertIndex].Selected = true;//选择新添加的行
            _stepTable.CurrentCell = _stepTable.Rows[insertIndex].Cells[0];//移动箭头

            // Testflow: 添加step
            ISequenceStep currentStep = TestflowDesigntimeSession.AddSequenceStep(CurrentSeq, stepName, "", insertIndex);
            currentStep.RecordStatus = true;
            TestflowDesigntimeSession.AddSequenceStep(currentStep, stepType, "", 0);

            SetStepProperty(currentStep, stepType);

            // 清空_currentDescriptions
            _currentComDescription = null;
            _currentClassDescription = null;

            // 展示空的settings
            ShowValidSettingPages(GetCurrentStepType());
            UpdateSettings();
            _internalOperation = false;

            if (null != limitType)
            {
                AddLimitFunction(currentStep, limitType);
            }
        }

        //添加步骤
        private void InsertTimingStep(string stepType, int insertIndex, string timingVariable, string defaultStepName)
        {
            if (!CheckAuthority(AuthorityDefinition.EditSequence))
            {
                return;
            }

            _internalOperation = true;

            // 获取可用的step名称
            string stepName = defaultStepName;
            while (CurrentSeq.Steps.Any(item => item.Name.Equals(stepName)))
            {
                stepName = defaultStepName + _currentStepId;
                _currentStepId++;
            }

            // 添加新的行，记录新行的位置
            _stepTable.Rows.Insert(insertIndex, GetImage(stepType), stepName, "", "", "", "", "");
            _stepTable.Rows[insertIndex].Selected = true;//选择新添加的行
            _stepTable.CurrentCell = _stepTable.Rows[insertIndex].Cells[0];//移动箭头

            // Testflow: 添加step
            ISequenceStep currentStep = TestflowDesigntimeSession.AddSequenceStep(CurrentSeq, stepName, "", insertIndex);
            currentStep.RecordStatus = true;
            TestflowDesigntimeSession.AddSequenceStep(currentStep, stepType, "", 0);
            IFunctionData timingFunction = GetStartTimingFunction();
            ISequenceStep timingStep = TestflowDesigntimeSession.AddSequenceStep(currentStep, timingFunction, Constants.MethodStepName,
                Constants.MethodStepName, 1);
            TestflowDesigntimeSession.SetReturn(timingVariable, timingStep);
            // 更新Settings和Description
            StepInfoCreator.SetStepDescriptionInfo(currentStep, stepType);
            StepInfoCreator.SetStepSettingInfo(currentStep, stepType);
            _stepTable.Rows[insertIndex].Cells[StepTableDescCol].Value = currentStep.Description;
            _stepTable.Rows[insertIndex].Cells[StepTableSettingCol].Value = currentStep.SubSteps[0].Description;

            SetStepProperty(currentStep, stepType);

            // 如果变量不存在则创建
            CreateDefaultVariable();

            // 清空_currentDescriptions
            _currentComDescription = null;
            _currentClassDescription = null;

            // 展示空的settings
            ShowValidSettingPages(GetCurrentStepType());
            UpdateSettings();

            _internalOperation = false;

            // 默认断言失败后继续执行，调用出现异常，则进入下次loop
            currentStep.AssertFailedAction = FailedAction.Continue;
            currentStep.InvokeErrorAction = FailedAction.BreakLoop;
        }

        //添加等待步骤
        private void InsertWaitStep(string stepType, int insertIndex, string defaultStepName)
        {
            if (!CheckAuthority(AuthorityDefinition.EditSequence))
            {
                return;
            }

            _internalOperation = true;

            // 获取可用的step名称
            string stepName = defaultStepName;
            ISequence parentSequence = CurrentSeq;
            while (parentSequence.Steps.Any(item => item.Name.Equals(stepName)))
            {
                stepName = defaultStepName + _currentStepId;
                _currentStepId++;
            }

            // 添加新的行，记录新行的位置
            _stepTable.Rows.Insert(insertIndex, GetImage(stepType), stepName, "", "", "", "", "");
            _stepTable.Rows[insertIndex].Selected = true;//选择新添加的行
            _stepTable.CurrentCell = _stepTable.Rows[insertIndex].Cells[0];//移动箭头

            // Testflow: 添加step
            ISequenceStep currentStep = TestflowDesigntimeSession.AddSequenceStep(parentSequence, stepName, "", insertIndex);
            currentStep.RecordStatus = true;
            TestflowDesigntimeSession.AddSequenceStep(currentStep, stepType, "", 0);
            IFunctionData timingFunction = GetWaitFunction();
            ISequenceStep timingStep = TestflowDesigntimeSession.AddSequenceStep(currentStep, timingFunction, Constants.MethodStepName,
                Constants.MethodStepName, 1);
            timingStep.Function.Parameters[0].Value = Constants.DefaultWaitTime;
            timingStep.Function.Parameters[0].ParameterType = ParameterType.Value;
            // 更新Settings和Description
            StepInfoCreator.SetStepDescriptionInfo(currentStep, stepType);
            StepInfoCreator.SetStepSettingInfo(currentStep, stepType);
            _stepTable.Rows[insertIndex].Cells[StepTableDescCol].Value = currentStep.Description;
            _stepTable.Rows[insertIndex].Cells[StepTableSettingCol].Value = currentStep.SubSteps[0].Description;

            SetStepProperty(currentStep, stepType);

            // 清空_currentDescriptions
            _currentComDescription = null;
            _currentClassDescription = null;

            // 展示空的settings
            ShowValidSettingPages(GetCurrentStepType());
            UpdateSettings();

            _internalOperation = false;

            // 默认断言失败后继续执行，调用出现异常，则进入下次loop
            currentStep.AssertFailedAction = FailedAction.Continue;
            currentStep.InvokeErrorAction = FailedAction.BreakLoop;
        }

        private void AddLimitFunction(ISequenceStep currentStep, string limitType)
        {
            AddDefaultLimitSubStep(currentStep);
            _internalOperation = true;
            dGV_Limit.Rows[0].Cells[LimitTableTypeCol].Value = limitType;
            dGV_Limit.Rows[0].Selected = true;
            dGV_Limit.CurrentCell = dGV_Limit.Rows[0].Cells[LimitTableTypeCol];
            _internalOperation = false;
            ValidateLimitTableContent(0, LimitTableTypeCol);
        }

        //删除步骤
        private void DeleteStep_Click(object sender, EventArgs e)
        {
            // 没有行
            if (_stepTable.CurrentRow == null)
            {
                return;
            }

            int index = _stepTable.CurrentRow.Index;
            TestflowDesigntimeSession.RemoveSequenceStep(CurrentSeq, index);
            _stepTable.Rows.RemoveAt(index);

            if (_stepTable.Rows.Count == 0)
            {
                tabControl_settings.Visible = false;
            }
        }

        private void ToolStripMenuItem_insertStep_Click(object sender, EventArgs e)
        {

        }

        #endregion

        #region Step详情-Properties页面控件事件

        private void tabControl_settings_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSettings();
        }

        private void StepTypecomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string stepType = GetCurrentStepType();
            string newStepType = comboBox_runType.SelectedItem.ToString();
            if (stepType.Equals(newStepType))
            {
                return;
            }
            ISequenceStep currentStep = CurrentStep;
            //if (start) { return; }
            
            ShowValidSettingPages(newStepType);

            // 修改step第一列的图标
            DataGridView stepTable;
            if (tabCon_Step.TabPages.Count >= 2 && null != (stepTable = tabCon_Step.TabPages[0].Controls[0] as DataGridView))
            {
                Image stepImage = GetImage(newStepType);
                stepTable.Rows[currentStep.Index].Cells[StepTableIconCol].Value = stepImage;
            }

            if (_internalOperation || _stepRowChange) { return; }

            #region 根据StepType修改_currentStep
            
            TestflowDesigntimeSession.RemoveSequenceStep(currentStep, 0);

            #region Sequence Call 特殊处理
            if (newStepType.Equals("Sequence Call"))
            {
                currentStep.SubSteps.Clear();
            }
            #endregion

            #region Test 到 action 特殊处理
            else
            {
                for (int n = 0; n < currentStep.SubSteps.Count; n++) if (currentStep.SubSteps[n].Name.Contains("Limit"))
                    {
                        TestflowDesigntimeSession.RemoveSequenceStep(currentStep, n);
                        n--;
                    }
            }
            #endregion

            TestflowDesigntimeSession.AddSequenceStep(currentStep, newStepType, "", 0);
            #endregion

            RefreshCurrentStepInfo(currentStep);

        }

        private void checkBox_RecordResult_CheckedChanged(object sender, EventArgs e)
        {
            if (null == CurrentStep || _internalOperation)
            {
                return;
            }
            bool recordResult = checkBox_RecordStatus.Checked;
            bool breakIfFailed = checkBox_breakIfFailed.Checked;
            bool skipStep = checkBox_skipStep.Checked;
            ModifyStepExecutionProperties(CurrentStep, recordResult, breakIfFailed, skipStep);
        }

        private void checkBox_breakIfFailed_CheckedChanged(object sender, EventArgs e)
        {
            if (null == CurrentStep || _internalOperation)
            {
                return;
            }
            bool recordResult = checkBox_RecordStatus.Checked;
            bool breakIfFailed = checkBox_breakIfFailed.Checked;
            bool skipStep = checkBox_skipStep.Checked;
            ModifyStepExecutionProperties(CurrentStep, recordResult, breakIfFailed, skipStep);
        }

        private void ModifyStepExecutionProperties(ISequenceStep step, bool recodeStatus, bool breakIfFailed, bool skipStep)
        {
            step.RecordStatus = recodeStatus;
            step.BreakIfFailed = breakIfFailed;
            step.Behavior = skipStep ? RunBehavior.Skip : RunBehavior.Normal;
            if (null != step.SubSteps)
            {
                foreach (ISequenceStep subStep in step.SubSteps)
                {
                    ModifyStepExecutionProperties(subStep, recodeStatus, breakIfFailed, skipStep);
                }
                RefreshCurrentStepInfo(CurrentStep);
            }
        }

        private void ShowValidSettingPages(string stepType)
        {
            switch (stepType)
            {
                case Constants.TestType:
                    // TODO 目前不支持序列参数，暂时封闭
//                    if (tabControl_settings.TabPages.ContainsKey("SeqParametersTab"))
//                    {
//                        tabControl_settings.TabPages.Remove(tabpage_parameters);
//                    }
                    if (!tabControl_settings.TabPages.Contains(tabpage_Module))
                    {
                        tabControl_settings.TabPages.Insert(1, tabpage_Module);
                    }
                    if (!tabControl_settings.TabPages.Contains(tabpage_limit))
                    {
                        tabControl_settings.TabPages.Insert(2, tabpage_limit);
                    }
                    panel_SequenceCallControls.Visible = false;
                    break;
                case Constants.ActionType:
                    // TODO 目前不支持序列参数，暂时封闭
//                    if (tabControl_settings.TabPages.ContainsKey("SeqParametersTab"))
//                    {
//                        tabControl_settings.TabPages.Remove(tabpage_parameters);
//                    }
                    if (!tabControl_settings.TabPages.Contains(tabpage_Module))
                    {
                        tabControl_settings.TabPages.Insert(1, tabpage_Module);
                    }
                    if (tabControl_settings.TabPages.Contains(tabpage_limit))
                    {
                        tabControl_settings.TabPages.Remove(tabpage_limit);
                    }
                    panel_SequenceCallControls.Visible = false;
                    break;
                case Constants.SeqCallType:
// TODO 目前不支持序列参数，暂时封闭
//                    if (!tabControl_settings.TabPages.ContainsKey("SeqParametersTab"))
//                    {
//                        tabControl_settings.TabPages.Add(tabpage_parameters);
//                    }
                    if (tabControl_settings.TabPages.Contains(tabpage_Module))
                    {
                        tabControl_settings.TabPages.Remove(tabpage_Module);
                    }
                    if (tabControl_settings.TabPages.Contains(tabpage_limit))
                    {
                        tabControl_settings.TabPages.Remove(tabpage_limit);
                    }
                    panel_SequenceCallControls.Visible = true;
                    break;
                default:
                    throw new Exception("Not supposed to get here.");
            }
        }

        private void LoopTypecomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            _internalOperation = true;
            ISequenceStep currentStep = CurrentStep;
            if (null == currentStep)
            {
                return;
            }
            ISequenceManager sequenceManager = _globalInfo.TestflowEntity.SequenceManager;
            switch (LoopTypecomboBox.Text)
            {
                case "None":
                    currentStep.LoopCounter = null;
                    LoopTimesnumericUpDown.Enabled = false;
                    numericUpDown_passTimes.Enabled = false;
                    numericUpDown_retryTime.Enabled = false;
                    currentStep.RetryCounter = null;
                    currentStep.LoopCounter = null;
                    break;
                case "FixedTimes":
                    LoopTimesnumericUpDown.Enabled = true;
                    numericUpDown_passTimes.Enabled = false;
                    numericUpDown_retryTime.Enabled = false;
                    if (!_stepRowChange)
                    {
                        currentStep.RetryCounter = null;
                        if (currentStep.LoopCounter == null)
                        {
                            currentStep.LoopCounter = sequenceManager.CreateLoopCounter();
                        }
                        currentStep.LoopCounter.CounterEnabled = true;
                        currentStep.LoopCounter.CounterEnabled = true;
                        currentStep.LoopCounter.MaxValue = (int)LoopTimesnumericUpDown.Value;
                    }
                    break;
                case "PassTimes":
                    LoopTimesnumericUpDown.Enabled = false;
                    numericUpDown_passTimes.Enabled = true;
                    numericUpDown_retryTime.Enabled = true;
                    if (!_stepRowChange)
                    {
                        currentStep.LoopCounter = null;
                        if (currentStep.RetryCounter == null)
                        {
                            currentStep.RetryCounter = sequenceManager.CreateRetryCounter();
                        }
                        currentStep.RetryCounter.RetryEnabled = true;
                        currentStep.RetryCounter.MaxRetryTimes = (int) numericUpDown_retryTime.Value;
                        currentStep.RetryCounter.PassTimes = (int) numericUpDown_passTimes.Value;
                    }
                    break;
            }
            RefreshCurrentStepInfo(currentStep);
            _internalOperation = false;
        }

        private void LoopTimesnumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (_internalOperation || _stepRowChange)
            {
                return;
            }
            CurrentStep.LoopCounter.MaxValue = Convert.ToInt32(LoopTimesnumericUpDown.Value);
        }
        
        private void numericUpDown_retryTime_ValueChanged(object sender, EventArgs e)
        {
            if (_internalOperation || _stepRowChange || null == CurrentStep?.RetryCounter)
            {
                return;
            }
            CurrentStep.RetryCounter.MaxRetryTimes = Convert.ToInt32(numericUpDown_retryTime.Value);
        }

        private void numericUpDown_passTimes_ValueChanged(object sender, EventArgs e)
        {
            if (_internalOperation || _stepRowChange || null == CurrentStep?.RetryCounter)
            {
                return;
            }
            CurrentStep.RetryCounter.PassTimes = Convert.ToInt32(numericUpDown_passTimes.Value);
        }

        private void SequencecomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_internalOperation || _stepRowChange)
            {
                return;
            }
            CurrentTypeStep.Name = $"Sequence Call:{comboBox_SequenceCall.SelectedItem}";
            RefreshCurrentStepInfo(CurrentStep);
        }

        private void textBox_conditionVar_TextChanged(object sender, EventArgs e)
        {
            if (_internalOperation)
            {
                return;
            }
            ISequenceStep currentStep = CurrentStep;
            ISequenceStep conditionStep =
                currentStep.SubSteps.FirstOrDefault(item => item.Name.Equals(Constants.ConditionStepName));
            if (null == conditionStep)
            {
                return;
            }
            bool isVariable;
            string variableName = Utility.GetParamValue(textBox_conditionVar.Text, out isVariable);
            if (!isVariable)
            {
                ShowMessage($"{variableName} is not a valid variable.", "Condition", MessageBoxIcon.Warning);
                return;
            }
            conditionStep.Description = Utility.GetConditionStepDescription(comboBox_asserFailedAction.Text, variableName);
        }

        private void button_conditionVarSelect_Click(object sender, EventArgs e)
        {
            VariableForm variableForm = new VariableForm(SequenceGroup.Variables, CurrentSeq.Variables, string.Empty);
            variableForm.ShowDialog(this);
            if (variableForm.IsCancelled)
            {
                return;
            }
            string showVariableName = Utility.GetShowVariableName(variableForm.IsGlobalVariable,
                variableForm.Value);
            textBox_conditionVar.Text = showVariableName;
        }

        private void comboBox_conditionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_internalOperation)
            {
                return;
            }
            ISequenceStep currentStep = CurrentStep;
            bool conditionEnabled = comboBox_asserFailedAction.SelectedIndex != 0;
            button_conditionVarSelect.Enabled = conditionEnabled;
            textBox_conditionVar.Enabled = conditionEnabled;
            if (_internalOperation || null == currentStep)
            {
                return;
            }
            ISequenceStep conditionStep =
                currentStep.SubSteps.FirstOrDefault(item => item.Name.Equals(Constants.ConditionStepName));

            if (conditionEnabled)
            {
                string description = Utility.GetConditionStepDescription(comboBox_asserFailedAction.Text, textBox_conditionVar.Text);
                if (null == conditionStep)
                {
                    TestflowDesigntimeSession.AddSequenceStep(currentStep, Constants.ConditionStepName, description,
                        currentStep.SubSteps.Count);
                }
                else
                {
                    conditionStep.Description = description;
                }
            }
            else
            {
                if (null != conditionStep)
                {
                    currentStep.SubSteps.Remove(conditionStep);
                }
            }
        }

        #endregion

        // Step详情-Module页面控件事件
        //路径改变：加载程序集并展示类名
        private void comboBox_assembly_TextChanged(object sender, EventArgs e)
        {
            if (_stopSign)
            {
                _stopSign = false;
                return;
            }

            if (_stepRowChange || openSign)
            {
                return;
            }

            #region 清空Parameters, comboBox_RootClass, comboBox_Method, _currentClassDescription, _currentFuncDescription
            _paramTable.Clear();
            comboBox_RootClass.DataSource = null;
            comboBox_Method.DataSource = null;
            comboBox_Constructor.DataSource = null;
            _currentClassDescription = null;
            //_currentFuncDescription = null;
            //_currentConstructorDescription = null;
            #endregion

            #region 判断空选项
            if (comboBox_assembly.Text.Equals(""))
            {
                _currentComDescription = null;
                return;
            }
            #endregion

            try
            {
                // Testflow:加载组件
                //1.GetComponentInterface(path)取得IComInterfaceDescription并放进InterfaceManager里
                //2.AddComponent(IComInterfaceDescription)添加到service的components里面方便管理
                _currentComDescription = _interfaceManger.GetComponentInterface(comboBox_assembly.Text);
                if (!comboBox_assembly.Items.Contains(comboBox_assembly.Text))
                {
                    comboBox_assembly.Items.Add(comboBox_assembly.Text);
                }
                _testflowDesigntimeService.AddComponent(_currentComDescription);

                // 展示类名
                ShowClasses();
            }
            catch (TestflowException ex)
            {
                ShowMessage(ex.Message, "Error", MessageBoxIcon.Error);
            }
        }

        //类选择改变：展示方法名
        private void comboBox_RootClass_Validated(object sender, EventArgs e)
        {
            if (_stepRowChange || _internalOperation || stepSelection)
            {
                return;
            }

            #region 清空Parameters、Method、Constructor
            comboBox_Method.DataSource = null;
            comboBox_Constructor.DataSource = null;
            //_currentFuncDescription = null;
            //_currentConstructorDescription = null;

            _paramTable.Clear();
            #endregion

            #region 判断空选项
            if (comboBox_RootClass.SelectedValue == null || comboBox_RootClass.SelectedValue.Equals(""))
            {
                _currentClassDescription = null;
                return;
            }
            #endregion

            #region Testflow:获得类description
            //获取当前选择的类的description：comboBox_RootClass.SelectedIndex 对应 Classes的Index
            _currentClassDescription = _currentComDescription.Classes[comboBox_RootClass.SelectedIndex];
            #endregion

            #region 展示方法名
            ShowConstructorsAndMethods();
            #endregion
        }

        private void comboBox_Constructor_Validated(object sender, EventArgs e)
        {
            if (_stepRowChange || _internalOperation || string.IsNullOrWhiteSpace(comboBox_Constructor.Text) ||
                !comboBox_Constructor.Enabled)
            {
                return;
            }

            // 清空Parameter
            _paramTable.Clear();

            InitializeConstructorStep();

            // 添加Parameter
            UpdateTDGVParameter();
        }

        private void InitializeConstructorStep()
        {
            // 判断空选项 , Existing Object
            ISequenceStep currentConstructorStep = CurrentConstructorStep;
            if (string.IsNullOrWhiteSpace(comboBox_Constructor.Text) || comboBox_Constructor.Text.Equals(ExistingObjName))
            {
                //_currentConstructorDescription = null;
                if (currentConstructorStep != null)
                {
                    TestflowDesigntimeSession.RemoveSequenceStep(CurrentStep, currentConstructorStep);
                }
                return;
            }
            // Testflow: 获得构造函数description
            IFuncInterfaceDescription constructorDescription =
                _currentClassDescription.Functions.FirstOrDefault(
                    item => GetConstructorSignature(item).Equals(comboBox_Constructor.Text));

            // 创建构造函数SubStep
            if (currentConstructorStep == null)
            {
                currentConstructorStep = TestflowDesigntimeSession.AddSequenceStep(CurrentStep, $"Constructor", "Constructor", 1);
            }

            // 判断选择的构造函数有没有变
            if (currentConstructorStep.Function == null ||
                !IsFunctionCreatedFromDescription(currentConstructorStep.Function, constructorDescription))
            {
                // 创建functionData， 并改变_currentFunctionStep的functionData
                IFunctionData constructorData =
                    _globalInfo.TestflowEntity.SequenceManager.CreateFunctionData(constructorDescription);
                currentConstructorStep.Function = constructorData;

                // 删除functionStep里的instance
                if (CurrentFunctionStep != null)
                {
                    TestflowDesigntimeSession.SetInstance("", CurrentFunctionStep);
                }
            }
        }

        private void comboBox_Method_Validated(object sender, EventArgs e)
        {
            if (_stepRowChange || _internalOperation)
            {
                return;
            }
            // 清空Parameter
            _paramTable.Clear();

            // 根据用户选择的方法，初始化FunctionStep
            InitializeFunctionStep();
            // 如果是实例方法且未配置Constructor，则修改Constructor为ExistingObject
            if (null != CurrentStep && null != CurrentFunctionStep && IsInstanceFunction(CurrentFunctionStep.Function) &&
                string.IsNullOrWhiteSpace(comboBox_Constructor.Text))
            {
                _internalOperation = true;
                comboBox_Constructor.Text = ExistingObjName;
                _internalOperation = false;
            }
            // 添加Parameter
            UpdateTDGVParameter();

            RefreshCurrentStepInfo(CurrentStep);
        }

        private void InitializeFunctionStep()
        {
            ISequenceStep currentStep = CurrentStep;
            if (null == currentStep)
            {
                return;
            }
            ISequenceStep currentFunctionStep = CurrentFunctionStep;
            ISequenceStep currentConstructorStep = CurrentConstructorStep;
            // Static Class, remove Constructor
            if (_currentClassDescription.IsStatic && currentConstructorStep != null)
            {
                TestflowDesigntimeSession.RemoveSequenceStep(currentStep, currentConstructorStep);
            }

            // 判断空选项
            if (string.IsNullOrWhiteSpace(comboBox_Method.Text))
            {
                if (currentFunctionStep != null)
                {
                    TestflowDesigntimeSession.RemoveSequenceStep(currentStep, currentFunctionStep);
                }
                return;
            }
            // 如果FunctionStep不存在则创建
            if (currentFunctionStep == null)
            {
                //添加在constructorStep后面
                currentFunctionStep = TestflowDesigntimeSession.AddSequenceStep(currentStep, Constants.MethodStepName,
                    "Method", (currentConstructorStep == null) ? 1 : 2);
            }

            IFuncInterfaceDescription funcDescription =
                    _currentClassDescription.Functions.FirstOrDefault(
                        item => GetMethodSignature(item).Equals(comboBox_Method.Text));
            //_currentFunctionStep没有方法 或 新的funcDescription
            if (currentFunctionStep.Function == null ||
                !IsFunctionCreatedFromDescription(currentFunctionStep.Function, funcDescription))
            {
                //创建functionData， 并改变_currentFunctionStep的functionData
                IFunctionData functionData = _globalInfo.TestflowEntity.SequenceManager.CreateFunctionData(funcDescription);
                currentFunctionStep.Function = functionData;
            }
            //Instance是constructorStep的instance
            if (!string.IsNullOrWhiteSpace(currentConstructorStep?.Function.Instance))
            {
                currentFunctionStep.Function.Instance = currentConstructorStep.Function.Instance;
            }
        }

        private bool IsInstanceFunction(IFunctionData function)
        {
            return null != function && function.Type != FunctionType.StaticFieldSetter &&
                   function.Type != FunctionType.StaticFunction && function.Type != FunctionType.StaticPropertySetter;
        }

        private void RefreshCurrentStepInfo(ISequenceStep currentStep)
        {
            int rowIndex = currentStep.Index;
            // 运行时该方法不执行
            if (viewController_Main.StateValue > 1 || _stepTable.Rows.Count <= rowIndex)
            {
                return;
            }
            _internalOperation = true;
            string stepType = GetCurrentStepType();
            StepInfoCreator.SetStepDescriptionInfo(currentStep, stepType);
            _stepTable.Rows[rowIndex].Cells[StepTableDescCol].Value = currentStep.Description;
            StepInfoCreator.SetStepSettingInfo(currentStep, stepType);
            _stepTable.Rows[rowIndex].Cells[StepTableSettingCol].Value = currentStep.SubSteps[0].Description;
            _internalOperation = false;
        }

        private void TdgvParamCellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            #region ColumnHeaders不作为
            if (e.RowIndex < 0)
            {
                return;
            }
            #endregion

            #region 标题行不作为
            if (_paramTable.IsParent(e.RowIndex))
            {
                return;
            }
            #endregion

            string name = _paramTable.Rows[e.RowIndex].Cells["ParameterName"].Value.ToString();
            string value = _paramTable.Rows[e.RowIndex].Cells["ParameterValue"].Value?.ToString();
            string group = _paramTable.FindNodeGroup(e.RowIndex);

            #region f(x) button

            int columnIndex = e.ColumnIndex;
            if (_paramTable.Columns[columnIndex].Name.Equals("Parameterfx"))
            {
                VariableForm variableForm = new VariableForm(SequenceGroup.Variables, CurrentSeq.Variables, group, value,
                    true);
                variableForm.ShowDialog(this);
                if (!variableForm.IsCancelled)
                {
                    _paramTable.Rows[e.RowIndex].Cells["ParameterValue"].Value =
                        Utility.GetShowVariableName(variableForm.IsGlobalVariable, variableForm.Value);
                }
                variableForm.Dispose();
            }
            #endregion
            #region check button
            else if (_paramTable.Columns[columnIndex].Name.Equals("ParameterCheck"))
            {
                IWarningInfo warningInfo = null;
                ISequenceFlowContainer[] arr = new ISequenceFlowContainer[] { SequenceGroup, CurrentSeq };

                #region Constructor
                if (group.Equals("Constructor"))
                {
                    #region 检查 Existing Object
                    if (name.Equals(ExistingObjParent))
                    {
                        if (CurrentFunctionStep != null)
                        {
                            warningInfo = _globalInfo.TestflowEntity.ParameterChecker.CheckInstance(CurrentFunctionStep, arr, false);
                        }
                        else
                        {
                            if ((warningInfo = _globalInfo.TestflowEntity.ParameterChecker.CheckPropertyType(CurrentSeq, value, _currentClassDescription.ClassType, false)).WarnCode == 1025)
                            {
                                warningInfo = _globalInfo.TestflowEntity.ParameterChecker.CheckPropertyType(SequenceGroup, value, _currentClassDescription.ClassType, false);
                            }
                        }
                    }
                    #endregion

                    #region 检查构造函数
                    else
                    {
                        #region 空值
                        //todo 如果值为空表示，实例不为变量，在运行开始前再创造变量
                        if (string.IsNullOrEmpty(value) || !name.Equals("Return Value")) { }
                        #endregion

                        else
                        {
                            warningInfo = _globalInfo.TestflowEntity.ParameterChecker.CheckInstance(CurrentConstructorStep, arr, false);
                        }

                    }
                    #endregion
                }
                #endregion

                #region Method
                else
                {
                    #region 检查返回值
                    if (name.Equals("Return Value"))
                    {
                        warningInfo = _globalInfo.TestflowEntity.ParameterChecker.CheckReturn(CurrentFunctionStep, arr, false);
                    }
                    #endregion

                    #region 检查参数
                    else
                    {
                        warningInfo = _globalInfo.TestflowEntity.ParameterChecker.CheckParameterData(CurrentFunctionStep.Function,
                                                                                                    (CurrentFunctionStep.Function.ReturnType == null) ? _paramTable.IndexInGroup(e.RowIndex) : _paramTable.IndexInGroup(e.RowIndex - 1),
                                                                                                    arr, false);
                    }
                    #endregion
                }
                #endregion

                #region 报成功或错误信息
                if (warningInfo != null)
                {
                    MessageBox.Show(warningInfo.Infomation);
                }
                else
                {
                    MessageBox.Show("Success");
                }
                #endregion
            }
            #endregion
        }


        private void TdgvParamCellEnterEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            int rowIndex = e.RowIndex;
            if (rowIndex < 0 || _paramTable.IsParent(rowIndex) || e.ColumnIndex != ParamTableValueCol)
            {
                return;
            }
            string paramName = _paramTable.Rows[rowIndex].Cells["ParameterName"].Value.ToString();
            string value = _paramTable.Rows[rowIndex].Cells["ParameterValue"].Value?.ToString();
            string group = _paramTable.FindNodeGroup(rowIndex);
            IFunctionData function = null;
            ISequenceStep functionStep = CurrentFunctionStep;
            ISequenceStep constructorStep = CurrentConstructorStep;
            if (group.Equals(Constants.MethodStepName) && null != functionStep && null != functionStep.Function)
            {
                function = functionStep.Function;
            }
            else if (null != constructorStep && null != constructorStep.Function)
            {
                function = constructorStep.Function;
            }
            IArgument paramInfo = null;
            // 只有ParameterInfo为Struct或Class时才可以使用字符串初始化对象
            if (null == function ||
                (null == (paramInfo = function.ParameterType.FirstOrDefault(item => item.Name.Equals(paramName)))) ||
                (paramInfo.VariableType != VariableType.Struct && paramInfo.VariableType != VariableType.Class))
            {
                return;
            }
            try
            {
                int index = function.ParameterType.IndexOf(paramInfo);
                // 如果原来配置的是变量，则配置原始值为空
                if (function.Parameters[index].ParameterType == ParameterType.Variable)
                {
                    value = string.Empty;
                }
                string changedValue = ShowComplexDataEditor(paramInfo, value);
                if (!string.IsNullOrWhiteSpace(changedValue))
                {
                    _paramTable.Rows[rowIndex].Cells["ParameterValue"].Value = changedValue;
                }
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, "Parameter", MessageBoxIcon.Error);
            }
            finally
            {
                _paramTable.EndEdit();
            }
        }

        private string ShowComplexDataEditor(IArgument argument, string valueStr)
        {
            if (argument.VariableType != VariableType.Struct && argument.VariableType != VariableType.Class)
            {
                return null;
            }
            ITypeData typeData = argument.Type;
            IComInterfaceManager interfaceManager = _globalInfo.TestflowEntity.ComInterfaceManager;
            bool isArrayType = Utility.IsArrayType(typeData);
            IClassInterfaceDescription classInterface = null;
            if (isArrayType)
            {
                classInterface = Utility.GetElementInterface(interfaceManager, typeData);
            }
            else
            {
                IComInterfaceDescription comInterface = interfaceManager.GetComInterfaceByName(typeData.AssemblyName);
                classInterface = comInterface?.Classes.FirstOrDefault(item => item.Name.Equals(typeData.Name));
            }
            if (classInterface == null)
            {
                return null;
            }
            
            if (isArrayType)
            {
                IClassInterfaceDescription elementInterface = Utility.GetElementInterface(interfaceManager, typeData);
                if (null == elementInterface)
                {
                    return null;
                }
                ArrayDataEditor arrayDataEditor = new ArrayDataEditor(elementInterface, interfaceManager, valueStr);
                arrayDataEditor.ShowDialog(this);
                return arrayDataEditor.IsCancelled ? null : arrayDataEditor.GetValue();

            }
            else
            {
                SimpleClassEditor classEditor = new SimpleClassEditor(classInterface, interfaceManager, valueStr);
                classEditor.ShowDialog(this);
                return classEditor.IsCancelled ? null : classEditor.GetValue();
            }
        }

        private DataGridViewCell _currentStepParamCell = null;
        private void TdgvParamCellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //清除的时候不触发或者标题行直接返回
            if (e.RowIndex < 0 || _internalOperation || _paramTable.IsParent(e.RowIndex))
            {
                return;
            }
            _internalOperation = true;

            try
            {
                SetParamValue(e);
                _internalOperation = false;
            }
            catch (ApplicationException ex)
            {
                ShowMessage(ex.Message, "Set Parameter", MessageBoxIcon.Error);
                _internalOperation = false;
                // 将默认值配置为空，连带更新值的变更
                _paramTable.Rows[e.RowIndex].Cells["ParameterValue"].Value = string.Empty;
            }
        }

        private void SetParamValue(DataGridViewCellEventArgs e)
        {
            //注：只有value的值会改变
            string paramName = _paramTable.Rows[e.RowIndex].Cells["ParameterName"].Value?.ToString();
            DataGridViewCell currentCell = _paramTable.Rows[e.RowIndex].Cells["ParameterValue"];
            string tableValue = currentCell.Value?.ToString() ?? string.Empty;
            // 如果是在编辑第三列的数据，并且值等于local.或者global.，则弹出变量选择列表
            if (e.ColumnIndex == 4 &&
                (tableValue.Equals(Constants.GlobalVarPrefix + Constants.VaraibleDelim,
                    StringComparison.CurrentCultureIgnoreCase) ||
                 tableValue.Equals(Constants.LocalVarPrefix + Constants.VaraibleDelim, StringComparison.CurrentCultureIgnoreCase)))
            {
                Rectangle rectangle = _paramTable.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                ShowVariableList(tableValue, e.RowIndex, rectangle);
                return;
            }

            bool isVariable;
            string value = Utility.GetParamValue(tableValue, out isVariable);

            ISequenceStep functionStep = CurrentFunctionStep;
            ISequenceStep constructorStep = CurrentConstructorStep;
            ISequenceStep currentStep = CurrentStep;
            // 修改构造方法的参数
            if (_paramTable.FindNodeGroup(e.RowIndex).Equals("Constructor"))
            {
                string instance = value;
                // 实例为已存在的变量
                if (paramName.Equals(ExistingObjParent))
                {
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        TestflowDesigntimeSession.SetInstance(string.Empty, functionStep);
                        return;
                    }
                    IVariable variable = GetAvailableVariable(currentStep, value, functionStep.Function.ClassType);
                    TestflowDesigntimeSession.SetInstance(variable?.Name ?? string.Empty, functionStep);
                    SetVariableType(functionStep.Function.ClassType, variable);
                    currentCell.Value = (null != variable) ? Utility.GetShowVariableName(variable) : string.Empty;
                }
                else
                {
                    // 
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        if (null != functionStep)
                        {
                            TestflowDesigntimeSession.SetInstance(string.Empty, functionStep);
                        }
                        if (null != constructorStep)
                        {
                            TestflowDesigntimeSession.SetInstance(string.Empty, constructorStep);
                        }
                        return;
                    }
                    if (paramName.Equals("Return Value"))
                    {
                        IVariable variable = GetAvailableVariable(currentStep, value, constructorStep.Function.ClassType);
                        TestflowDesigntimeSession.SetInstance(variable?.Name ?? string.Empty, constructorStep);
                        SetVariableType(constructorStep.Function.ClassType, variable);
                        if (null != functionStep)
                        {
                            TestflowDesigntimeSession.SetInstance(variable?.Name ?? string.Empty, functionStep);
                        }
                        currentCell.Value = (null != variable) ? Utility.GetShowVariableName(variable) : string.Empty;
                    }
                    else
                    {
                        SetStepParamValue(constructorStep, paramName, isVariable, value, currentCell);
                    }
                }
            }
            // 修改Method的参数
            else
            {
                if (paramName.Equals("Return Value"))
                {
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        TestflowDesigntimeSession.SetInstance(string.Empty, functionStep);
                        return;
                    }
                    IVariable variable = GetAvailableVariable(currentStep, value, functionStep.Function.ClassType);
                    TestflowDesigntimeSession.SetReturn(variable?.Name ?? string.Empty, functionStep);
                    SetVariableType(functionStep.Function.ReturnType.Type, variable);
                    currentCell.Value = (null != variable) ? Utility.GetShowVariableName(variable) : string.Empty;
                }
                else
                {
                    SetStepParamValue(functionStep, paramName, isVariable, value, currentCell);
                }
            }
        }

        private void SetVariableType(ITypeData paramType, IVariable variable)
        {
            if (null != variable.Type)
            {
                return;
            }
            variable.Type = paramType;
        }

        private void SetStepParamValue(ISequenceStep step, string paramName, bool isVariable, string value, 
            DataGridViewCell currentCell)
        {
            IArgument argument = step.Function.ParameterType.FirstOrDefault(item => item.Name.Equals(paramName));
            if (null == argument)
            {
                return;
            }
            // 如果确认为变量，则直接写入参数
            if (isVariable)
            {
                string variableName = value;
                ParameterType paramType = ParameterType.Variable;
                if (_expRegex.IsMatch(value))
                {
                    paramType = ParameterType.Expression;
                    variableName = _expRegex.Match(value).Groups[2].Value;
                }
                // 检查变量是否存在
                IVariable variable = GetAvailableVariable(step, variableName, null);
                TestflowDesigntimeSession.SetParameterValue(paramName, value, paramType, step);
                SetVariableType(argument.Type, variable);
            }
            // 如果参数为类类型或者有ref或者out的参数且不是json字符串，则需要使用变量传递
            else if ((argument.VariableType == VariableType.Class || argument.VariableType == VariableType.Struct ||
                      argument.Modifier != ArgumentModifier.None) && !Utility.IsJsonValue(value) && !string.IsNullOrWhiteSpace(value))
            {
                IVariable variable = GetAvailableVariable(step, value, argument.Type);
                TestflowDesigntimeSession.SetParameterValue(paramName, variable?.Name ?? string.Empty,
                    ParameterType.Value, step);
                SetVariableType(argument.Type, variable);
                currentCell.Value = (null != variable) ? Utility.GetShowVariableName(variable) : string.Empty;
            }
            else
            {
                // 如果是bool类型，且值既不是True也不是False，则判断后重新赋值
                if (argument.Type.Name.Equals("Boolean") && !true.ToString().Equals(value) &&
                    !false.ToString().Equals(value) && !string.IsNullOrEmpty(value))
                {
                    bool boolValue;
                    if (!bool.TryParse(value, out boolValue))
                    {
                        throw new ApplicationException("Invalid bool value.");
                    }
                    value = boolValue.ToString();
                    currentCell.Value = value;
                }
                // 不是string类型，并且值为空，则修改为未配置状态
                if (!argument.Type.Name.Equals("String") && string.IsNullOrWhiteSpace(value))
                {
                    // 值类型直接写入参数
                    TestflowDesigntimeSession.SetParameterValue(paramName, string.Empty, ParameterType.NotAvailable, step);
                }
                else
                {
                    // 值类型直接写入参数
                    TestflowDesigntimeSession.SetParameterValue(paramName, value, ParameterType.Value, step);
                }
            }
        }

        private IVariable GetAvailableVariable(ISequenceStep step, string variableName, ITypeData type)
        {
            IVariable variable = Utility.GetVariable(variableName, step);
            if (null != variable)
            {
                return variable;
            }
            throw new ApplicationException($"Variable {variableName} not exist.");
        }

        private IVariable ShowCreateVariableForm(string variableName, ITypeData typeData)
        {
            DialogResult dialogResult = MessageBox.Show(string.Format(Resources.AddVarMessage, variableName), "Parameter", 
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Cancel)
            {
                return null;
            }
            ISequenceFlowContainer parent = dialogResult == DialogResult.Yes
                ? (ISequenceFlowContainer)SequenceGroup
                :(ISequenceFlowContainer) CurrentSeq;
            IVariable varialbe = TestflowDesigntimeSession.AddVariable(parent, variableName, string.Empty, 0);
            varialbe.LogRecordLevel = RecordLevel.None;
            varialbe.ReportRecordLevel = RecordLevel.None;
            varialbe.Type = typeData;
            ShowVariables(parent is ISequenceGroup ?　0 : 1);
            return varialbe;
        }

        private void ShowVariableList(string value, int rowIndex, Rectangle cellRectangle)
        {
            contextMenuStrip_varList.Items.Clear();
            _currentStepParamCell = _paramTable.Rows[rowIndex].Cells[4];
            IVariableCollection variables = value.StartsWith(Constants.GlobalVarPrefix, true, CultureInfo.CurrentCulture)
                ? SequenceGroup.Variables
                : CurrentSeq?.Variables;
            if (null == variables || variables.Count == 0)
            {
                _currentStepParamCell.Value = string.Empty;
                _currentStepParamCell = null;
                return;
            }
            int index = 0;
            foreach (IVariable variable in variables)
            {
                contextMenuStrip_varList.Items.Add(Utility.GetShowVariableName(variable));
                contextMenuStrip_varList.Items[index].MouseDown += SetCellVariableValue;
                contextMenuStrip_varList.Items[index].Click += SetCellVariableValue;
                index++;
            }
            // 暂时不考虑下面显示不够的问题
            Point showLocation = new Point(cellRectangle.X, cellRectangle.Bottom);
            contextMenuStrip_varList.Show(_paramTable, showLocation);
        }


        private void contextMenuStrip_varList_VisibleChanged(object sender, EventArgs e)
        {
            if (null == _currentStepParamCell || contextMenuStrip_varList.Visible)
            {
                return;
            }
            // 如果结束后没有选择正确的变量，则直接配置对应参数为空值
            string cellValue = _currentStepParamCell.Value.ToString();
            if (cellValue.Equals(Constants.GlobalVarPrefix + Constants.VaraibleDelim, StringComparison.CurrentCultureIgnoreCase) ||
                cellValue.Equals(Constants.LocalVarPrefix + Constants.VaraibleDelim, StringComparison.CurrentCultureIgnoreCase))
            {
                _currentStepParamCell.Value = "";
            }
        }

        private void SetCellVariableValue(object sender, EventArgs eventArgs)
        {
            if (null == _currentStepParamCell)
            {
                return;
            }
            _currentStepParamCell.Value = (sender as ToolStripItem).Text;
            _currentStepParamCell = null;
        }

        private void button_selectAssembly_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = openFileDialog_assembly.ShowDialog(this);
            if (dialogResult != DialogResult.OK) return;
            if (!comboBox_assembly.Items.Contains(openFileDialog_assembly.FileName))
            {
                comboBox_assembly.Items.Add(openFileDialog_assembly.FileName);
            }
            comboBox_assembly.Text = openFileDialog_assembly.FileName;
        }

        private void ResetPathComboBox()
        {
            comboBox_assembly.Items.Clear();
            if (null != SequenceGroup)
            {
                foreach (IAssemblyInfo assemblyInfo in SequenceGroup.Assemblies)
                {
                    comboBox_assembly.Items.Add(assemblyInfo.Path);
                }
            }
        }

        #region Limit相关事件和处理

        private void SetEmptyLimitStep(string testType)
        {
            IList<IFuncInterfaceDescription> funcList =
                _testflowDesigntimeService.Components["TestStationLimit"].Classes.First(
                    item => item.Name.Equals("Limit")).Functions;

            IFunctionData function =
                _globalInfo.TestflowEntity.SequenceManager.CreateFunctionData(
                    funcList.FirstOrDefault(item => item.Name.Equals($"Assert{testType}")));

            CurrentLimitStep.Function = function;
        }

        private void dGV_Limit_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (openSign || _internalOperation)
            {
                return;
            }

            int rowIndex = e.RowIndex;
            int columnIndex = e.ColumnIndex;
            if (rowIndex < 0)
            {
                return;
            }
            ValidateLimitTableContent(rowIndex, columnIndex);
        }

        // 根据变更的行号和列号，有效化对应行的数据信息
        private void ValidateLimitTableContent(int rowIndex, int columnIndex)
        {
            string testType = dGV_Limit.Rows[rowIndex].Cells["LimitTestType"].Value.ToString();
            string comparisonType = dGV_Limit.Rows[rowIndex].Cells["LimitComparisonType"].Value?.ToString();
            string lowValue = dGV_Limit.Rows[rowIndex].Cells["LimitLow"].Value?.ToString();
            string highValue = dGV_Limit.Rows[rowIndex].Cells["LimitHigh"].Value?.ToString();
            string unit = dGV_Limit.Rows[rowIndex].Cells["LimitUnit"].Value?.ToString();
            string expression = dGV_Limit.Rows[rowIndex].Cells["LimitExpression"].Value?.ToString();
            string limitName = dGV_Limit.Rows[rowIndex].Cells["Column_limitName"].Value?.ToString();
            ISequenceStep currentLimitStep = CurrentLimitStep;
            switch (dGV_Limit.Columns[columnIndex].Name)
            {
                case "Column_limitName":
                    if (null != currentLimitStep)
                    {
                        currentLimitStep.Description = limitName;
                    }
                    break;
                case "LimitTestType":
                    SetEmptyLimitStep(testType);
                    string[] compareTypes = Limit.GetCompareTypes(testType);
                    if (testType.Equals("Boolean"))
                    {
                        dGV_Limit.Rows[rowIndex].Cells["LimitLow"] = new DataGridViewComboBoxCell();
                        ((DataGridViewComboBoxCell)dGV_Limit.Rows[rowIndex].Cells["LimitLow"]).DataSource = new string
                            []
                        {"True", "False"};
                    }
                    else
                    {
                        dGV_Limit.Rows[rowIndex].Cells["LimitLow"] = new DataGridViewTextBoxCell();
                    }
                    // ComparisonType
                    ((DataGridViewComboBoxCell) dGV_Limit.Rows[rowIndex].Cells["LimitComparisonType"]).DataSource =
                        compareTypes;
                    dGV_Limit.Rows[rowIndex].Cells["LimitComparisonType"].Value = compareTypes[0];
                        //触发cellvaluechanged事件改compareType
                    dGV_Limit.Rows[rowIndex].Cells["LimitComparisonType"].ReadOnly = false;
                    break;
                case "LimitComparisonType":
                    bool highAvailable, unitAvailable;
                    Limit.GetHighLowAndUnitEnable(testType, comparisonType, out highAvailable, out unitAvailable);

                    dGV_Limit.Rows[rowIndex].Cells["LimitLow"].ReadOnly = false;
                    if (!Limit.IsValidValue(testType, lowValue))
                    {
                        dGV_Limit.Rows[rowIndex].Cells["LimitLow"].Value = Limit.GetDefaultLowValue(testType);
                    }

                    dGV_Limit.Rows[rowIndex].Cells["LimitHigh"].ReadOnly = !highAvailable;
                    if (highAvailable && !Limit.IsValidValue(testType, highValue))
                    {
                        dGV_Limit.Rows[rowIndex].Cells["LimitHigh"].Value = Limit.GetDefaultHighValue(testType);
                    }
                    else if (!highAvailable)
                    {
                        dGV_Limit.Rows[rowIndex].Cells["LimitHigh"].Value = Constants.UnavailableValue;
                    }
                    else
                    {
                        TestflowDesigntimeSession.SetParameterValue("high", highValue, ParameterType.Value,
                        currentLimitStep);
                    }

                    dGV_Limit.Rows[rowIndex].Cells["LimitUnit"].ReadOnly = !unitAvailable;
                    // 当前unit可用，并且原来unit的值是非法的，修改unit默认为空
                    if (unitAvailable && Constants.UnavailableValue.Equals(unit))
                    {
                        dGV_Limit.Rows[rowIndex].Cells["LimitUnit"].Value = string.Empty;
                    }
                    else if (!unitAvailable)
                    {
                        dGV_Limit.Rows[rowIndex].Cells["LimitUnit"].Value = Constants.UnavailableValue;
                    }
                    else
                    {
                        TestflowDesigntimeSession.SetParameterValue("unit", unit ?? "", ParameterType.Value,
                        currentLimitStep);
                    }

                    TestflowDesigntimeSession.SetParameterValue("comparisonType", comparisonType, ParameterType.Value,
                        currentLimitStep);
                    break;

                    #region 改low

                case "LimitLow":
                    // 非String比较时，如果Low配置为空则认为该参数未配置
                    ParameterType parameterType = string.IsNullOrWhiteSpace(lowValue) && !testType.Equals("String")
                        ? ParameterType.NotAvailable
                        : ParameterType.Value;
                    TestflowDesigntimeSession.SetParameterValue("low", lowValue, parameterType, currentLimitStep);
                    break;

                    #endregion

                    #region 改high

                case "LimitHigh":
                    // 非String比较时，如果Low配置为空则认为该参数未配置
                    ParameterType hignParamType = string.IsNullOrWhiteSpace(highValue) && !testType.Equals("String")
                        ? ParameterType.NotAvailable
                        : ParameterType.Value;
                    TestflowDesigntimeSession.SetParameterValue("high", highValue, hignParamType,
                        currentLimitStep);
                    break;

                    #endregion

                    #region 改unit

                case "LimitUnit":
                    TestflowDesigntimeSession.SetParameterValue("unit", (unit == null) ? "" : unit, ParameterType.Value,
                        currentLimitStep);
                    break;

                    #endregion

                    #region  改expression

                case "LimitExpression":
                    if (string.IsNullOrWhiteSpace(expression))
                    {
                        TestflowDesigntimeSession.SetParameterValue("expression", string.Empty, ParameterType.Value,
                            currentLimitStep);
                    }
                    else
                    {
                        bool isVariable;
                        expression = Utility.GetParamValue(expression, out isVariable);
                        // 如果不是变量，则弹出提醒，并配置expression为空
                        if (!isVariable)
                        {
                            ShowMessage("Expression should be a variable name.", "Limit", MessageBoxIcon.Error);
                            dGV_Limit.Rows[rowIndex].Cells["LimitExpression"].Value = string.Empty;
                            return;
                        }
                        string variableName = expression;
                        ParameterType paramType = ParameterType.Variable;
                        if (_expRegex.IsMatch(expression))
                        {
                            paramType = ParameterType.Expression;
                            variableName = _expRegex.Match(expression).Groups[1].Value;
                        }
                        // 检查变量是否存在
                        try
                        {
                            GetAvailableVariable(currentLimitStep, variableName, null);
                            TestflowDesigntimeSession.SetParameterValue("expression", expression, paramType,
                                currentLimitStep);
                        }
                        catch (ApplicationException ex)
                        {
                            ShowMessage(ex.Message, "Limit", MessageBoxIcon.Error);
                            dGV_Limit.Rows[rowIndex].Cells["LimitExpression"].Value = string.Empty;
                        }
                    }
                    break;

                    #endregion

                default:
                    throw new Exception("Should not get to this point.");
            }
        }

        private void dGV_Limit_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string columnName = dGV_Limit.Columns[e.ColumnIndex].Name;
            ISequence currentSequence = CurrentSeq;
            if (columnName.Equals("Limitfx"))
            {
                VariableForm variableForm = new VariableForm(SequenceGroup.Variables, currentSequence.Variables, "Method",
                    dGV_Limit.Rows[e.RowIndex].Cells["LimitExpression"].Value?.ToString() ?? string.Empty, true);
                variableForm.ShowDialog(this);
                if (!variableForm.IsCancelled)
                {
                    IVariableCollection variableCollection = variableForm.IsGlobalVariable
                        ? SequenceGroup.Variables
                        : currentSequence.Variables;
                    IVariable variable =
                        variableCollection.FirstOrDefault(item => item.Name.Equals(variableForm.Value));
                    if (null != variable)
                    {
                        variable.ReportRecordLevel = RecordLevel.Trace;
                    }
                    dGV_Limit.Rows[e.RowIndex].Cells["LimitExpression"].Value =
                        Utility.GetShowVariableName(variableForm.IsGlobalVariable, variableForm.Value);
                }
                variableForm.Dispose();
            }
            else if (columnName.Equals("LimitCheck"))
            {
                IWarningInfo warningInfo = null;
                ISequenceFlowContainer[] arr = new ISequenceFlowContainer[] {SequenceGroup, currentSequence};

                warningInfo = _globalInfo.TestflowEntity.ParameterChecker.CheckParameterData(
                    CurrentLimitStep.Function, 3, arr, false);

                // 报成功或错误信息

                if (warningInfo != null)
                {
                    MessageBox.Show(warningInfo.Infomation);
                }
                else
                {
                    MessageBox.Show("Success");
                }
            }
        }

        private void AddLimit_Click(object sender, EventArgs e)
        {
            // method为空返回
            if (CurrentFunctionStep == null)
            {
                MessageBox.Show("Please choose Method first.");
                tabControl_settings.SelectedIndex = 1;
                return;
            }
            AddDefaultLimitSubStep(CurrentStep);
        }

        private void AddDefaultLimitSubStep(ISequenceStep functionStep)
        {
            ISequenceStep limitStep = null;
            string limitName;
            // 获取当前Limit的名称
            do
            {
                _currentLimitId++;
                limitName = "Limit" + _currentLimitId;
                limitStep = functionStep.SubSteps.FirstOrDefault(item => item.Name.Equals(limitName));
            } while (limitStep != null);

            //  testflow: 添加subStep
            //  testflow: 添加subStep
            limitStep = TestflowDesigntimeSession.AddSequenceStep(functionStep, limitName,
                limitName, functionStep.SubSteps.Count);
            // 同步修改LimitStep的步骤属性
            limitStep.RecordStatus = functionStep.RecordStatus;
            limitStep.BreakIfFailed = functionStep.BreakIfFailed;

            dGV_Limit.Rows.Add(limitName, "", "", "", null, "", "", "");
        }

        private void DeleteLimit_Click(object sender, EventArgs e)
        {
            int limitIndex = (CurrentStep.SubSteps.Count - dGV_Limit.Rows.Count) + dGV_Limit.CurrentRow.Index;

            #region testflow: RemoveLimitSubStep

            TestflowDesigntimeSession.RemoveSequenceStep(CurrentStep, limitIndex);

            #endregion

            dGV_Limit.Rows.RemoveAt(dGV_Limit.CurrentRow.Index);
        }

        #endregion

        #region UI上展示数据 Show/Update

        private void ShowSequences(int tabNumber)
        {
            if (tabNumber == 0)
            {
                DataGridView seqTable = (DataGridView)tabPage_mainSequence.Controls[0];
                seqTable.Rows.Clear();
                if (viewController_Main.StateValue <= (int) RunState.EditProcess)
                {
                    seqTable.Rows.Add(SequenceGroup.SetUp.Name);
                    seqTable.Rows.Add(SequenceGroup.Sequences[0].Name);
                }
                
                seqTable.Rows.Add(SequenceGroup.Sequences[1].Name);
                if(viewController_Main.StateValue <= (int)RunState.EditProcess)
                {
                    seqTable.Rows.Add(SequenceGroup.Sequences[2].Name);
                    seqTable.Rows.Add(SequenceGroup.TearDown.Name);
                }
            }
            else  //tabNumber == 1
            {
                foreach (string sequence in _userSequences)
                {
                    ((DataGridView)tabPage_userSequence.Controls[0]).Rows.Add(sequence);
                }
            }
        }

        private void ShowVariables(int tabNumber)
        {
            DataGridView dgv;
            IVariableCollection variableCollection;
            if (tabNumber == 0)
            {
                variableCollection = SequenceGroup.Variables;
                dgv = (DataGridView)globalVariableTab.Controls[0];
            }

            else
            {
                variableCollection = CurrentSeq.Variables;
                dgv = (DataGridView)tabCon_Variable.TabPages[1].Controls[0];
            }
            dgv.Rows.Clear();
            foreach (IVariable variable in variableCollection)
            {
                int rowIndex;
                if (variable.VariableType == VariableType.Undefined)
                {
                    rowIndex = dgv.Rows.Add(variable.Name, "", variable.Value);
                }
                else if (variable.VariableType == VariableType.Class && variable.Type == null)
                {
                    rowIndex = dgv.Rows.Add(variable.Name, "Object", variable.Value);
                }
                else
                {
                    switch (variable.Type.Name)
                    {
                        case "":
                            rowIndex = dgv.Rows.Add(variable.Name, "", variable.Value);
                            break;
                        case "String":
                            rowIndex = dgv.Rows.Add(variable.Name, "String", variable.Value);
                            break;
                        case "Boolean":
                            rowIndex = dgv.Rows.Add(variable.Name, "Boolean", variable.Value);
                            break;
                        case "Double":
                            rowIndex = dgv.Rows.Add(variable.Name, "Numeric", variable.Value);
                            break;

                        //Object
                        default:
                            rowIndex = dgv.Rows.Add(variable.Name, "Object", variable.Type.Name);
                            dgv.Rows[rowIndex].Cells["VariableType"].ReadOnly = true;
                            break;
                    }
                }
                dgv.Rows[rowIndex].Tag = variable.Name;
            }
        }

        private void UpdateModule()
        {
            
            if (CurrentFunctionStep == null && CurrentConstructorStep == null)
            {
                return;
            }
            _internalOperation = true;
            IFunctionData function = (CurrentFunctionStep == null) ? (CurrentConstructorStep.Function)
                                                                       : (CurrentFunctionStep.Function);
            //IFuncInterfaceDescription funcDescription = function.Description;
            //#region 加载进来的function，Description值为null，这时候从components里寻找
            //if(funcDescription == null)
            //{
            //    LoadfuncDescription(function);
            //}
            //#endregion
            _currentComDescription = _testflowDesigntimeService.Components[function.ClassType.AssemblyName];
            if (!comboBox_assembly.Items.Contains(_currentComDescription.Assembly.Path))
            {
                comboBox_assembly.Items.Add(_currentComDescription.Assembly.Path);
            }
            comboBox_assembly.Text = _currentComDescription.Assembly.Path;

            ShowClasses(function.ClassType);

            ShowConstructorsAndMethods(CurrentFunctionStep?.Function, CurrentConstructorStep?.Function);

            UpdateTDGVParameter();
            _internalOperation = false;
        }

        private string GetCurrentStepType()
        {
            if (null == CurrentTypeStep)
            {
                return Constants.ActionType;
            }
            return CurrentTypeStep.Name.Contains(Constants.SeqCallType) ? Constants.SeqCallType : CurrentTypeStep.Name;
        }

        private void UpdateSettings()
        {
            ISequenceStep step = FindSelectedStep(treeView_stepView.SelectedNode);
            if (null == step)
            {
                tabControl_settings.Enabled = false;
                tabControl_settings.SelectedIndex = 0;
                return;
            }
            TabPage currentTab = tabControl_settings.SelectedTab;
            if (currentTab != tabPage_runtimeInfo && null != CurrentStep)
            {
                ClearSettings();

                if (currentTab == tabpage_Properties)
                {
                    ShowStepInfo(step);
                }
                else if (currentTab == tabpage_Module)
                {
                    UpdateModule();
                }
            }
            if (null != SequenceGroup && !tabControl_settings.Visible)
            {
                tabControl_settings.Visible = true;
            }
        }

        private void ClearSettings()
        {
            _internalOperation = true;
            // 清空Properties
            //注：StepTypeComboBox与LoopTypeComboBox没有空值，也就是不用把SelectedIndex变成-1.
            LoopTimesnumericUpDown.Value = 2;

            // 清空module
            comboBox_assembly.Text = "";
            comboBox_RootClass.DataSource = null;
            comboBox_RootClass.Text = "";
            comboBox_Method.DataSource = null;
            comboBox_Method.Text = "";
            comboBox_Constructor.DataSource = null;
            comboBox_Method.Text = "";
            _paramTable.Clear();

            _internalOperation = false;
        }

        private void ShowClasses(ITypeData selectedClass = null)
        {
            List<string> classNames = new List<string>();
            string className = null;
            //int selectedIndex = -1;

            foreach (IClassInterfaceDescription Class in _currentComDescription.Classes)
            {
                classNames.Add(Class.Name);

                #region 选中已有的Class
                if (selectedClass != null && selectedClass.Equals(Class.ClassType))
                {
                    _currentClassDescription = Class;
                    className = Class.Name;
                    //selectedIndex = classNames.Count - 1;
                }
                #endregion
            }
            comboBox_RootClass.DataSource = classNames;
            comboBox_RootClass.SelectedItem = className;
            //comboBox_RootClass.SelectedIndex = selectedIndex;
        }

        // 比较function和FunctinDescription，判断当前function是不是从FunctionDescrition创建出来的
        private bool IsFunctionCreatedFromDescription(IFunctionData function, IFuncInterfaceDescription funcDescription)
        {
            if (function == null || !function.MethodName.Equals(funcDescription.Name) ||
                function.ParameterType.Count != funcDescription.Arguments.Count)
            {
                return false;
            }
            for (int n = 0; n < function.ParameterType.Count; n++)
            {
                if (!function.ParameterType[n].Type.Equals(funcDescription.Arguments[n].Type) ||
                    function.ParameterType[n].Modifier != funcDescription.Arguments[n].Modifier)
                {
                    return false;
                }
            }
            return true;
        }

        //获取图片
        private Image GetImage(string type)
        {
            Image image;
            if (type.Contains(Constants.SeqCallType))
            {
                image = Properties.Resources.SequenceCall;
            }
            else if (type == Constants.TestType)
            {
                image = Properties.Resources.Test;
            }
            else if (type == Constants.ActionType)
            {
                image = Properties.Resources.Action;
            }
            else
            {
                image = Properties.Resources.JXI;
            }
            return image;
        }

        private string GetConstructorSignature(IFuncInterfaceDescription func)
        {
            string className = func.Signature.Split('.')[0];
            string parameters = func.Signature.Substring(func.Signature.LastIndexOf('('));
            return className + parameters;
        }

        private string GetMethodSignature(IFuncInterfaceDescription func)
        {
            string[] str = func.Signature.Split('.');
            return str[str.Length - 1];
        }

        private void ShowConstructorsAndMethods(IFunctionData selectedMethod = null, IFunctionData selectedConstructor = null)
        {
            List<string> methodNames = new List<string>();
            List<string> constructorNames = new List<string>();
            constructorNames.Add("");
            
            constructorNames.Add(ExistingObjName);
            int methodIndex = -1;
            int constructorIndex = -1;
            foreach (IFuncInterfaceDescription function in _currentClassDescription.Functions)
            {
                //构造函数
                if (function.FuncType == FunctionType.Constructor || function.FuncType == FunctionType.StructConstructor)
                {
                    constructorNames.Add(GetConstructorSignature(function));
                }
                else
                {
                    methodNames.Add(GetMethodSignature(function));
                }

                #region 选中已有的method和constructor
                if (IsFunctionCreatedFromDescription(selectedMethod, function))
                {
                    methodIndex = methodNames.Count - 1;
                }
                if (IsFunctionCreatedFromDescription(selectedConstructor, function))
                {
                    constructorIndex = constructorNames.Count - 1;
                }
                #endregion
            }

            #region 判断 <Existing Object>
            if (CurrentConstructorStep == null && CurrentFunctionStep != null && CurrentFunctionStep.Function.Instance != "")
            {
                constructorIndex = constructorNames.IndexOf(ExistingObjName);
            }
            #endregion

            comboBox_Method.DataSource = methodNames;
            if (methodNames.Count > 0 && methodIndex != -1)
            {
                comboBox_Method.SelectedIndex = methodIndex;
            }
            else
            {
                comboBox_Method.Text = "";
                comboBox_Method.SelectedIndex = -1;     //SelectedValue = null
            }

            comboBox_Constructor.DataSource = constructorNames;
            if (constructorNames.Count > 0 && methodIndex != -1)
            {
                comboBox_Constructor.SelectedIndex = constructorIndex;
            }
            else
            {
                comboBox_Constructor.Text = "";
                comboBox_Constructor.SelectedIndex = -1;    //SelectedValue = null
            }


            // Static Constructor
            if (_currentClassDescription.IsStatic)
            {
                comboBox_Constructor.BackColor = Color.Gray;
                comboBox_Constructor.Enabled = false;
            }
            else
            {
                comboBox_Constructor.BackColor = Color.White;
                comboBox_Constructor.Enabled = true;
                comboBox_Constructor.DataSource = constructorNames;
            }
        }

        private void ShowSteps()
        {
            if (null == CurrentSeq)
            {
                if (tabCon_Step.TabPages.Count > 1)
                {
                    tabCon_Step.TabPages.RemoveAt(0);
                }
                return;
            }
            _stepTable.Rows.Clear();
            tabPage_stepData.Name = CurrentSeq.Name;
            tabPage_stepData.Text = "Steps:" + CurrentSeq.Name;

            _stepTable.Name = CurrentSeq.Name;
            if (tabCon_Step.TabPages.Count == 1)
            {
                tabCon_Step.TabPages.Insert(0, tabPage_stepData);
            }

            // Sequence无Step就取消tabControl_Setting
            if (CurrentSeq.Steps.Count == 0)
            {
                if (tabControl_settings.Visible)
                {
                    tabControl_settings.Visible = false;
                }
                return;
            }
            else if (!tabControl_settings.Visible)
            {
                tabControl_settings.Visible = true;
            }

            // 输入steps
            foreach (ISequenceStep step in CurrentSeq.Steps)
            {
                // 如果step的描述信息为空，则更新step的描述信息
                if (string.IsNullOrWhiteSpace(step.Description) ||
                    string.IsNullOrWhiteSpace(step.SubSteps[0].Description))
                {
                    RefreshCurrentStepInfo(step);
                }
                string stepType = Utility.GetStepType(step);
                string unit = "";
                if (stepType.Equals(Constants.TestType))
                {
                    ISequenceStep limitStep = Utility.GetLimitStep(step);
                    unit = limitStep?.Function.Parameters[4].Value ?? "";
                }
                _stepTable.Rows.Add(GetImage(step.SubSteps[0].Name), GetShowStepName(step.Name), step.Description,
                    step.SubSteps[0].Description, unit, "", "");
            }
        }

        private static string GetShowStepName(string stepName)
        {
            return Regex.IsMatch(stepName, "^Step\\d+$") ? "Step" : stepName;
        }

        private void ShowReturnAndParameters(ISequenceStep step, IFunctionData functionData)
        {
            #region 构造函数instance
            if (functionData.Type == FunctionType.Constructor || functionData.Type == FunctionType.StructConstructor)
            {
                string instance = Utility.GetShowVariableName(functionData.Instance, step);
                _paramTable.Rows.Add(new object[] { null, "Return Value", $"Object({ functionData.ClassType.Namespace}.{functionData.ClassType.Name})", "Out", instance });
            }

            #endregion

            #region 返回值
            if (functionData.ReturnType != null)
            {
                string returnValue = Utility.GetShowVariableName(functionData.Return, step);
                _paramTable.Rows.Add(new object[] { null, "Return Value", functionData.ReturnType.Type.Name, "Out", returnValue });
            }

            #endregion

            #region 参数
            for (int n = 0; n < ((functionData.Parameters == null) ? 0 : (functionData.Parameters.Count)); n++)
            {
                IArgument parameterType = functionData.ParameterType[n];
                IParameterData parameterData = functionData.Parameters[n];
                string modifier = parameterType.Modifier.ToString();
                // 如果参数值为变量类型，则处理该变量显示值
                string paramValue = parameterData.ParameterType == ParameterType.Variable
                    ? Utility.GetShowVariableName(parameterData.Value, step)
                    : parameterData.Value;
                int rowIndex = _paramTable.Rows.Add(new object[] { null, parameterType.Name, parameterType.Type.Name, modifier.Equals("None") ? "In" : modifier, paramValue });

                // 该步骤是为了添加参数的Assembly到接口加载模块
                IAssemblyInfo assemblyInfo;
                _globalInfo.TestflowEntity.ComInterfaceManager.GetClassDescriptionByType(parameterType.Type,
                    out assemblyInfo);

                // 枚举特殊处理
                if (parameterType.VariableType == VariableType.Enumeration)
                {
                    string[] enumItems = null;
                    try
                    {
                        enumItems = _globalInfo.TestflowEntity.ComInterfaceManager.GetEnumItems(parameterType.Type);
                    }
                    catch (ApplicationException ex)
                    {
                        Log.Print(LogLevel.WARN, ex.Message);
                    }
                    // 如果当前程序集包含该枚举的定义，则显示为下拉框，否则不作为
                    if (null != enumItems && enumItems.Length > 0)
                    {
                        DataGridViewComboBoxCell enumCell = new DataGridViewComboBoxCell();
                        enumCell.DataSource = enumItems;
                        _paramTable.Rows[rowIndex].Cells["ParameterValue"] = enumCell;
                        if (!string.IsNullOrEmpty(parameterData.Value))
                        {
                            enumCell.Value = parameterData.Value;
                        }
                    }
                }
            }
            #endregion
        }

        private void UpdateTDGVParameter()
        {
            ISequenceStep currentFunctionStep = CurrentFunctionStep;
            ISequenceStep constructorStep = CurrentConstructorStep;
            #region 构造函数
            if (!string.IsNullOrWhiteSpace(comboBox_Constructor.Text))
            {
                // 包含ConstructorStep
                if (!comboBox_Constructor.Text.Equals(ExistingObjName) && null != constructorStep?.Function)
                {
                    _paramTable.AddParent($"{ _currentClassDescription.ClassType.Namespace}.{ _currentClassDescription.ClassType.Name}", "Constructor");
                    ShowReturnAndParameters(constructorStep, constructorStep.Function);
                }
                else if (comboBox_Constructor.SelectedValue.Equals(ExistingObjName) && null != currentFunctionStep)
                {
                    if (null != currentFunctionStep)
                        _paramTable.AddParent(ExistingObjParent, "Constructor");
                    string instanceValue = Utility.GetShowVariableName(currentFunctionStep.Function.Instance,
                        currentFunctionStep);
                    _paramTable.Rows.Add(new object[] { null, ExistingObjParent,
                    $"Object({_currentClassDescription.ClassType.Namespace}.{_currentClassDescription.ClassType.Name})", "In",
                                                        instanceValue});
                }
            }
            #endregion

            // 方法显示
            if (!string.IsNullOrWhiteSpace(comboBox_Method.Text) && null != currentFunctionStep)
            {
                _paramTable.AddParent($"{comboBox_Method.SelectedValue}", "Method");
                ShowReturnAndParameters(currentFunctionStep, currentFunctionStep.Function);
            }
        }

        private void UpdateSequenceCallList()
        {
            _userSequences.Clear();
            for (int i = 3; i < SequenceGroup.Sequences.Count; i++)
            {
                _userSequences.Add(SequenceGroup.Sequences[i].Name);
            }
        }

        #endregion

        #region 状态更新事件及界面联动

        private void viewController_Main_PreListeners(int oldState, int newState)
        {
            if (newState > 1)
            {
                _globalInfo.Session.CheckAuthority(AuthorityDefinition.RunSequence);
            }
            else if (newState <= 1)
            {
                _globalInfo.Session.CheckAuthority(AuthorityDefinition.EditSequence);
            }
        }

        private void viewController_Main_PostListeners(int oldState, int newState)
        {
            if (newState >= 0)
            {
                toolStripStatusLabel_stateValue.Text = viewController_Main.StateNames[newState];
            }
            // 状态为运行时
            if (newState > 1)
            {
                if (!toolStripProgressBar_progress.Enabled)
                {
                    toolStripProgressBar_progress.Enabled = true;
                    toolStripProgressBar_progress.Value = 100;
                    toolStripProgressBar_progress.Style = ProgressBarStyle.Marquee;
                }
                if (newState == 6)
                {
                    toolStripProgressBar_progress.Enabled = false;
                    toolStripProgressBar_progress.Value = 100;
                    toolStripProgressBar_progress.Style = ProgressBarStyle.Continuous;
                }
                // 序列创建、关闭按钮不使能
                toolStripButton_New.Enabled = false;
                newToolStripMenuItem1.Enabled = false;
                toolStripButton_Open.Enabled = true;
                openToolStripMenuItem1.Enabled = true;
                toolStripButton_Save.Enabled = false;
                saveToolStripMenuItem1.Enabled = false;
                SaveAsToolStripMenuItem1.Enabled = false;
                // 运行菜单栏
                toolStripButton_Run.Enabled = newState == (int) RunState.RunIdle || newState == (int) RunState.RunOver || newState == (int) RunState.RunBlock;
                startToolStripMenuItem1.Enabled = toolStripButton_Run.Enabled;
                toolStripButton_Suspend.Enabled = newState == (int)RunState.Running;
                suspendToolStripMenuItem.Enabled = toolStripButton_Suspend.Enabled;
                toolStripButton_Stop.Enabled = newState == (int)RunState.Running || newState == (int)RunState.RunProcessing;
                stopToolStripMenuItem2.Enabled = toolStripButton_Stop.Enabled;
                bool uiNotBusy = newState != (int) RunState.EditProcess &&
                               newState != (int) RunState.Running &&
                               newState != (int)RunState.RunProcessing;
                configureToolStripMenuItem.Enabled = uiNotBusy;
                selectModelToolStripMenuItem.Enabled = uiNotBusy;

                button_addWatch.Enabled = newState == (int) RunState.RunBlock;
                button_deleteWatch.Enabled = newState == (int) RunState.RunBlock;

                if (oldState <= 1)
                {
                    // 如果原始状态是编辑状态，执行切换到运行模式的相关配置
                    RunModeAction();
                }
            }
            // 状态为编辑时
            else
            {
                if (toolStripProgressBar_progress.Enabled)
                {
                    toolStripProgressBar_progress.Enabled = false;
                    toolStripProgressBar_progress.Value = 0;
                    toolStripProgressBar_progress.Style = ProgressBarStyle.Blocks;
                }
                // 序列创建、打开、关闭按钮使能
                toolStripButton_New.Enabled = true;
                newToolStripMenuItem1.Enabled = true;
                toolStripButton_Open.Enabled = true;
                openToolStripMenuItem1.Enabled = true;
                toolStripButton_Save.Enabled = true;
                saveToolStripMenuItem1.Enabled = true;
                SaveAsToolStripMenuItem1.Enabled = true;
                // 运行菜单栏
                toolStripButton_Run.Enabled = true;
                startToolStripMenuItem1.Enabled = true;
                toolStripButton_Suspend.Enabled = false;
                suspendToolStripMenuItem.Enabled = false;
                toolStripButton_Stop.Enabled = false;
                stopToolStripMenuItem2.Enabled = false;
                if (oldState > 1)
                {
                    // 如果原始状态是运行状态，执行切换到编辑模式的相关配置
                    EditModeAction();
                }
            }
        }

        private void EditModeAction()
        {
            _paramTable.CellValueChanged += TdgvParamCellValueChanged;
            _paramTable.CellContentClick += TdgvParamCellContentClick;
            ((DataGridView)tabCon_Seq.TabPages[1].Controls[0]).ReadOnly = false;
            // 隐藏运行时变量值窗体
            splitContainer_runtime.Panel1Collapsed = true;
            // step表格只读，且不响应值变更事件
            _stepTable.CellValueChanged += Dgv_Step_CellValueChanged;
            _stepTable.ReadOnly = false;
            // 子序列名称可修改
            ((DataGridView)tabCon_Seq.TabPages[1].Controls[0]).ReadOnly = false;

            // 隐藏运行时信息列
            _stepTable.Columns[StepTableDataCol].Visible = false;
            _stepTable.Columns[StepTableUnitCol].Visible = false;
            _stepTable.Columns[StepTableResultCol].Visible = false;
            // 隐藏返回编辑状态的菜单
            editSequenceToolStripMenuItem.Visible = false;
            // 显示编辑序列的菜单项
            addSequenceToolStripMenuItem.Visible = true;
            addStepToolStripMenuItem.Visible = true;
            // 添加step表格的右键菜单
            _stepTable.ContextMenuStrip = this.cMS_DgvStep;

            _eventController?.UnRegisterEvent();
            if (null != SequenceGroup)
            {
                ShowSequences(0);
            }
        }

        private void RunModeAction()
        {
            _paramTable.CellValueChanged -= TdgvParamCellValueChanged;
            _paramTable.CellContentClick -= TdgvParamCellContentClick;
            // 显示运行时变量值窗体
            splitContainer_runtime.Panel1Collapsed = false;
            ((DataGridView)tabCon_Seq.TabPages[1].Controls[0]).ReadOnly = true;
            // step表格只读，且不响应值变更事件
            _stepTable.CellValueChanged -= Dgv_Step_CellValueChanged;
            _stepTable.ReadOnly = true;
            // 子序列名称只读
            ((DataGridView) tabCon_Seq.TabPages[1].Controls[0]).ReadOnly = true;

            // 显示运行时信息列
            _stepTable.Columns[StepTableDataCol].Visible = true;
            _stepTable.Columns[StepTableUnitCol].Visible = true;
            _stepTable.Columns[StepTableResultCol].Visible = true;
            // 显示返回编辑状态的菜单
            editSequenceToolStripMenuItem.Visible = true;
            // 隐藏编辑序列的菜单项
            addSequenceToolStripMenuItem.Visible = false;
            addStepToolStripMenuItem.Visible = false;
            // 删除step表格的右键菜单
            _stepTable.ContextMenuStrip = null;
            if (null != SequenceGroup)
            {
                ShowSequences(0);
            }
        }

        private void ClearAll()
        {
            // Sequence
            ((DataGridView)tabPage_mainSequence.Controls[0]).Rows.Clear();
            ((DataGridView)tabPage_userSequence.Controls[0]).Rows.Clear();

            // Step
            tabCon_Step.TabPages.Clear();
            tabCon_Step.TabPages.Add(ReportTab);

            // Variable
            if (tabCon_Variable.TabPages.Count > 1)
            {
                tabCon_Variable.TabPages.RemoveAt(1);
            }
            ((DataGridView)globalVariableTab.Controls[0]).Rows.Clear();
            // Module
            tabControl_settings.Visible = false;
            comboBox_assembly.Text = "";
            comboBox_RootClass.DataSource = null;
            comboBox_RootClass.Text = "";
            comboBox_Method.DataSource = null;
            comboBox_Method.Text = "";
            _paramTable?.Clear();
            // Limit
            dGV_Limit.Rows.Clear();
        }

        #endregion

        #region 序列组管理

        // 创建新的序列
        private void CreateNewSequence()
        {
            if (!CheckAuthority(AuthorityDefinition.CreateSequence))
            {
                return;
            }
            if (null != SequenceGroup && SequenceGroup.Info.Modified)
            {
                DialogResult result = MessageBox.Show("Do you want to save current sequence?", "Create Sequence",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    SaveSequence(SequenceGroup.Info.SequenceGroupFile);
                }
            }

            // 清空UI
            ClearAll();

            _filePath = string.Empty;
            if (_firstNew)
            {
                TsfFilePath = string.Empty;
                _firstNew = false;
            }

            #region _currentId

            _currentTestProjectId++;
            _currentSequenceId = 0;
            _currentVariableId = 0;

            #endregion

            string projectName = "Test Project " + _currentTestProjectId.ToString();
            labelProject.Text = projectName + Constants.ProjectNamePostfix;

            #region 清空_userSequences

            _userSequences.Clear();

            #endregion

            // Testflow: 创建新的TestProject => SequenceGroup => Setup/Cleanup, MainSequence
            _testflowDesigntimeService.Unload();
            _testflowDesigntimeService.Load("Test Project", "");
            _testflowDesigntimeService.AddComponent(_interfaceManger.GetComponentDescriptions()[0]); //加载System.mscorlib
            IComInterfaceDescription limitDLLDescription = _testflowDesigntimeService.AddComponent(
                    _interfaceManger.GetComponentDescriptions()
                        .First(item => item.Assembly.AssemblyName.Equals("TestStationLimit")));
            //加载TestStationLimit.dll

            _testflowDesigntimeService.AddSequenceGroup(projectName, "");
            TestflowDesigntimeSession.AddSequence("ProcessSetup", "", -1);
            TestflowDesigntimeSession.AddSequence("ProcessCleanup", "", -2);

            #region PreUUT: ScanCode Step

            IList<IFuncInterfaceDescription> funcList =
                limitDLLDescription.Classes.First(item => item.Name.Equals("UIUtils")).Functions;
            IFuncInterfaceDescription scanCodeFunc = funcList.FirstOrDefault(item => item.Name.Equals("GetInput"));
            if (null != scanCodeFunc)
            {
                IFunctionData function = _globalInfo.TestflowEntity.SequenceManager.CreateFunctionData(scanCodeFunc);
                function.Parameters[0].Value = Constants.SerialNoVarName;
                function.Parameters[0].ParameterType = ParameterType.Variable;
                function.Return = Constants.ContinueVariable;
                ISequence preUUTSequence = TestflowDesigntimeSession.AddSequence("PreUUT", "", 0);
                ISequenceStep scanCodeStep = TestflowDesigntimeSession.AddSequenceStep(preUUTSequence, "Scan Code", "", 0);
                scanCodeStep.RecordStatus = true;
                TestflowDesigntimeSession.AddSequenceStep(scanCodeStep, "Action", "", 0);
                TestflowDesigntimeSession.AddSequenceStep(scanCodeStep, function, Constants.MethodStepName, Constants.MethodStepName, 1);
            }

            #endregion

            TestflowDesigntimeSession.AddSequence("MainSequence", "", 1);
            TestflowDesigntimeSession.AddSequence("PostUUT", "", 2);

            // Sequence UI
            tabCon_Seq.SelectedIndex = 0;
            ShowSequences(0);
            ShowSequences(1);

            // 创建默认添加的变量
            CreateDefaultVariable();

            EnableAllEditControl();
        }

        private void CreateDefaultVariable()
        {
            // 循环控制变量
            if (!SequenceGroup.Sequences[0].Variables.Any(item => item.Name.Equals(Constants.ContinueVariable)))
            {
                Type boolType = typeof(bool);
                IVariable continueVar = TestflowDesigntimeSession.AddVariable(SequenceGroup.Sequences[0],
                    Constants.ContinueVariable, true.ToString(), 0);
                TestflowDesigntimeSession.SetVariableValue(continueVar, true.ToString());
                continueVar.Type = _globalInfo.TestflowEntity.ComInterfaceManager.GetTypeByName(boolType.Name,
                    boolType.Namespace);
                continueVar.ReportRecordLevel = RecordLevel.None;
            }
            // 时钟使能变量
            if (!SequenceGroup.Variables.Any(item => item.Name.Equals(Constants.TimingEnableVar)))
            {
                Type boolType = typeof(bool);
                IVariable timingEnableVar = TestflowDesigntimeSession.AddVariable(SequenceGroup,
                    Constants.TimingEnableVar, false.ToString(), 0);
                TestflowDesigntimeSession.SetVariableValue(timingEnableVar, false.ToString());
                timingEnableVar.Type = _globalInfo.TestflowEntity.ComInterfaceManager.GetTypeByName(boolType.Name,
                    boolType.Namespace);
                timingEnableVar.ReportRecordLevel = RecordLevel.None;
            }
            // 开始时间变量
            if (!SequenceGroup.Variables.Any(item => item.Name.Equals(Constants.StartTimeVar)))
            {
                Type timeType = typeof(DateTime);
                IVariable startTimeVar = TestflowDesigntimeSession.AddVariable(SequenceGroup, Constants.StartTimeVar,
                    DateTime.MinValue.ToString(Constants.TimeFormat), 0);
                startTimeVar.Type = _globalInfo.TestflowEntity.ComInterfaceManager.GetTypeByName(timeType.Name,
                    timeType.Namespace);
                startTimeVar.ReportRecordLevel = RecordLevel.FinalResult;
            }
            // 结束时间变量
            if (!SequenceGroup.Variables.Any(item => item.Name.Equals(Constants.EndTimeVar)))
            {
                Type timeType = typeof(DateTime);
                IVariable endTimeVar = TestflowDesigntimeSession.AddVariable(SequenceGroup, Constants.EndTimeVar,
                    DateTime.MinValue.ToString(Constants.TimeFormat), 0);
                endTimeVar.Type = _globalInfo.TestflowEntity.ComInterfaceManager.GetTypeByName(timeType.Name,
                    timeType.Namespace);
                endTimeVar.ReportRecordLevel = RecordLevel.FinalResult;
            }
            // DUT索引号变量
            if (!SequenceGroup.Variables.Any(item => item.Name.Equals(Constants.DutIndexVarName)))
            {
                Type intType = typeof(double);
                IVariable dutIndexVar = TestflowDesigntimeSession.AddVariable(SequenceGroup,
                    Constants.DutIndexVarName, "0", 0);
                TestflowDesigntimeSession.SetVariableValue(dutIndexVar, "0");
                dutIndexVar.Type = _globalInfo.TestflowEntity.ComInterfaceManager.GetTypeByName(intType.Name,
                    intType.Namespace);
                dutIndexVar.ReportRecordLevel = RecordLevel.FullTrace;
            }
            // 硬件序列号变量
            if (!SequenceGroup.Variables.Any(item => item.Name.Equals(Constants.SerialNoVarName)))
            {
                Type strType = typeof(string);
                IVariable seriaNoVar = TestflowDesigntimeSession.AddVariable(SequenceGroup,
                    Constants.SerialNoVarName, false.ToString(), 0);
                TestflowDesigntimeSession.SetVariableValue(seriaNoVar, Constants.NASerialNo);
                seriaNoVar.Type = _globalInfo.TestflowEntity.ComInterfaceManager.GetTypeByName(strType.Name,
                    strType.Namespace);
                seriaNoVar.ReportRecordLevel = RecordLevel.FullTrace;
            }
            ShowVariables(0);
        }

        // 载入序列
        private void LoadSequence(string sequenceFilePath)
        {
            _filePath = sequenceFilePath;
            ISequenceGroup loadedSequenceGroup = _globalInfo.TestflowEntity.SequenceManager.LoadSequenceGroup(
                SerializationTarget.File, _filePath);

            _testflowDesigntimeService.Unload();
            _testflowDesigntimeService.Load(loadedSequenceGroup.Name, loadedSequenceGroup.Description, loadedSequenceGroup);
            labelProject.Text = SequenceGroup.Name + Constants.ProjectNamePostfix;

            #region _userSequences清空并装填

            _userSequences.Clear();

            for (int n = 3; n < SequenceGroup.Sequences.Count; n++)
            {
                _userSequences.Add(loadedSequenceGroup.Sequences[n].Name);
            }

            #endregion

            #region 清空UI

            ClearAll();

            #endregion

            #region Variables UI

            ShowVariables(0);

            #endregion

            ResetPathComboBox();
            // Sequence UI
            ShowSequences(1);
            ShowSequences(0);
            tabCon_Seq.SelectedIndex = 0;
            UpdateSettings();
            EnableAllEditControl();
            tabCon_Step.SelectedTab = tabPage_stepData;
            UpdateToolStripButtonsState();
        }

        private void UpdateToolStripButtonsState()
        {
            bool sequenceLoaded = null != SequenceGroup;
            toolStripButton_Run.Enabled = sequenceLoaded;
            toolStripButton_Save.Enabled = sequenceLoaded;
        }

        private void EnableAllEditControl()
        {
            tabCon_Seq.Enabled = true;
            tabCon_Variable.Enabled = true;
            toolStripButton_New.Enabled = true;
            toolStripButton_Open.Enabled = true;
            toolStripButton_Save.Enabled = true;
            toolStripButton_Run.Enabled = true;
            toolStripButton_Suspend.Enabled = false;
            toolStripButton_Stop.Enabled = false;
        }

        // 保存序列
        private void SaveSequence(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                if (!string.IsNullOrWhiteSpace(SequenceGroup.Info.SequenceGroupFile))
                {
                    saveFileDialog_sequence.FileName = SequenceGroup.Info.SequenceGroupFile;
                }
                DialogResult dialogResult = saveFileDialog_sequence.ShowDialog(this);
                if (dialogResult != DialogResult.OK)
                {
                    return;
                }
                filePath = saveFileDialog_sequence.FileName;
            }
            SequenceGroup.Info.SequenceGroupFile = filePath;
            SequenceGroup.Name = Utility.GetSequenceName(filePath);
            SequenceGroup.Info.ModifiedTime = DateTime.Now;
            SequenceGroup.Info.Modified = true;
            try
            {
                _globalInfo.TestflowEntity.SequenceManager.Serialize(SequenceGroup, SerializationTarget.File,
                    filePath);
                labelProject.Text = SequenceGroup.Name + Constants.ProjectNamePostfix;
            }
            catch (ApplicationException ex)
            {
                ShowMessage(ex.Message, "Save", MessageBoxIcon.Error);
            }
        }

        // 序列另存为
        private bool SaveAsSequence()
        {
            if (string.IsNullOrWhiteSpace(SequenceGroup.Info.SequenceGroupFile))
            {
                openFileDialog_sequence.FileName = SequenceGroup.Info.SequenceGroupFile;
            }
            DialogResult dialogResult = saveFileDialog_sequence.ShowDialog(this);
            if (dialogResult != DialogResult.OK)
            {
                return false;
            }
            SequenceGroup.Info.ModifiedTime = DateTime.Now;
            SequenceGroup.Info.Modified = true;
            string fileName = saveFileDialog_sequence.FileName;
            SequenceGroup.Name = Utility.GetSequenceName(fileName);
            try
            {
                _globalInfo.TestflowEntity.SequenceManager.Serialize(SequenceGroup, SerializationTarget.File,
                    fileName);
                labelProject.Text = SequenceGroup.Name + Constants.ProjectNamePostfix;
            }
            catch (ApplicationException ex)
            {
                ShowMessage(ex.Message, "Save", MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        #endregion

        #region 序列编辑


        private void AddSubSequence()
        {
            if (!CheckAuthority(AuthorityDefinition.EditSequence))
            {
                return;
            }

            string seqName;

            #region SequenceName

            do
            {
                _currentSequenceId++;
                seqName = "Sequence" + _currentSequenceId.ToString();
            } while (_userSequences.Contains(seqName));

            #endregion

            if (!SequenceTable.Name.Equals("UserSequenceList"))
            {
                throw new Exception("This should not happen, DGV must be UserSequenceList");
            }

            #region Testflow: AddSequence

            TestflowDesigntimeSession.AddSequence(seqName, "", SequenceGroup.Sequences.Count);

            #endregion

            #region _userSequences: 添加

            _userSequences.Add(seqName);

            #endregion

            #region 添加行

            SequenceTable.Rows.Insert(SequenceTable.RowCount, seqName);

            #endregion

            #region 选中行

            SequenceTable.CurrentCell = SequenceTable.Rows[SequenceTable.RowCount - 1].Cells[0];

            #endregion
        }

        private void DeleteSubSequence()
        {
            #region Testflow: RemoveSequence

            TestflowDesigntimeSession.RemoveSequence(CurrentSeq);

            #endregion

            #region _userSequences： 删除

            _userSequences.RemoveAt(SequenceTable.CurrentRow.Index);

            #endregion

            #region 删除行

            SequenceTable.Rows.RemoveAt(SequenceTable.CurrentRow.Index);

            #endregion
        }


        #endregion

        #region 序列运行

        private void RunSequence()
        {
            _operationPanel?.Close();
            _operationPanel = null;
            // 序列未保存则弹出保存界面，如果保存失败或取消则返回不执行
            if (string.IsNullOrWhiteSpace(SequenceGroup.Info.SequenceGroupFile))
            {
                ShowMessage("Please save sequence data to file before running.", "Run Sequence", MessageBoxIcon.Warning);
                bool saveSuccess = SaveAsSequence();
                if (!saveSuccess)
                {
                    viewController_Main.State = RunState.EditIdle.ToString();
                    return;
                }
            }

            #region 参数检查

            // 参数检查提醒后如果用户取消则停止执行
            if (!CheckParameter())
            {
                if (_globalInfo.Session.HasAuthority(AuthorityDefinition.EditSequence))
                {
                    viewController_Main.State = RunState.EditIdle.ToString();
                }
                return;
            }

            #endregion

            _start = true;

            try
            {
                // 配置引擎以调试方式运行
                _globalInfo.TestflowEntity.EngineController.ConfigData.SetProperty("RuntimeType", RuntimeType.Debug);
                // Runtime SequenceGroup
                this._sequenceMaintainer = new SequenceMaintainer(SequenceGroup, _globalInfo);
                ISequenceGroup runtimeSequenceGroup = _sequenceMaintainer.GetRuntimeSequence();

                // Testflow: RuntimeService.Load
                _testflowRuntimeService.Initialize();
                _testflowRuntimeService.Load(runtimeSequenceGroup);
                _globalInfo.TestflowEntity.EngineController.ConfigData.SetProperty("TestName", SequenceGroup.Name);

                // 添加事件
                _eventController = new EventController(_globalInfo, SequenceGroup, this, _sequenceMaintainer);
                _eventController.RegisterEvents();
                
                tabCon_Seq.SelectedIndex = 0;
                DataGridView seqTable = ((DataGridView)tabCon_Seq.TabPages[0].Controls[0]);
                seqTable.CurrentCell = seqTable.Rows[0].Cells[0];
                ResetRuntimeStatus();
                tabControl_settings.SelectedTab = tabPage_runtimeInfo;
                _watchVariables.Clear();
                ShowOperationPanel(runtimeSequenceGroup);
                _testflowRuntimeService.Run();
                viewController_Main.State = RunState.Running.ToString();
            }
            catch (ApplicationException ex)
            {
                Log.Print(LogLevel.ERROR, ex.Message);
                ShowMessage(ex.Message, "Error", MessageBoxIcon.Error);
                viewController_Main.State = RunState.RunIdle.ToString();
            }
        }

        // 参数检查，如果用户选择继续返回true
        private bool CheckParameter()
        {
            IList<string> checkResult = SequenceGroupChecker.Check(SequenceGroup);
            if (null == checkResult || checkResult.Count == 0)
            {
                return true;
            }
            ErrorInfoForm errorInfoForm = new ErrorInfoForm(checkResult);
            errorInfoForm.ShowDialog(this);
            return errorInfoForm.Continue;
        }

        private void ShowOperationPanel(ISequenceGroup runtimeSequence)
        {
            RuntimeDataCache dataCache = new RuntimeDataCache(_globalInfo.TestflowEntity, _globalInfo.Session,
                _globalInfo.Equipment)
            {
                EnableStartTiming = _sequenceMaintainer.UserStartTiming,
                EnableEndTiming = _sequenceMaintainer.UserEndTiming,
                MainStartStack = _sequenceMaintainer.MainStartStack,
                MainOverStack = _sequenceMaintainer.MainOverStack,
                PostStartStack = _sequenceMaintainer.PostStartStack,
                SequenceData = runtimeSequence,
                RunType = RunType.Slave,
                SequenceName = SequenceGroup.Name,
                Target = _globalInfo.ConfigManager.GetConfig<long>("Target")
            };
            _eventController.DataCache = dataCache;
            dataCache.InitModelInfo(_globalInfo.Equipment);
            ThreadPool.QueueUserWorkItem(state =>
            {
                _operationPanel = new OperationPanelForm(dataCache);
                _operationPanel.Initialize();
                Application.Run(_operationPanel);
            });
            // 等待
            while (null == _operationPanel)
            {
                Thread.Yield();
            }
            _operationPanel.Start();
        }

        private void SuspendSequence()
        {
            IDebuggerHandle debuggerHandle =
                _globalInfo.TestflowEntity.EngineController.GetRuntimeInfo<IDebuggerHandle>("DebugHandle");
            debuggerHandle.Pause(0);
            viewController_Main.State = RunState.RunProcessing.ToString();
        }

        private void StopRunning()
        {
            _stopSign = true;
            _start = false;
            _globalInfo.TestflowEntity.EngineController.AbortRuntime(0);

            tabControl_settings.Visible = true;
            // tabControl_setting.SelectedTab = tabControl_setting.TabPages[1]; ;
            _stepTable.BackgroundColor = Color.White;

            toolStripButton_New.Enabled = true;
            toolStripButton_Open.Enabled = true;
            toolStripButton_Save.Enabled = true;
            toolStripButton_Run.Enabled = true;
            toolStripButton_Suspend.Enabled = false;
            toolStripButton_Stop.Enabled = false;

            Menu_AddStep.Enabled = true;
            Menu_DeleteStep.Enabled = true;
            Menu_DeleteSubSeq.Enabled = true;
            Menu_AddSubSeq.Enabled = true;
        }

        #endregion

        #region 状态更新

        private readonly Dictionary<string, string> _watchVariables = new Dictionary<string, string>(10);
        private void button_addWatch_Click(object sender, EventArgs e)
        {
            int sequenceIndex;
            string runtimeVariableName;
            string variableShowName;
            GetSelectedVariable(out sequenceIndex, out runtimeVariableName, out variableShowName);
            if (null == runtimeVariableName)
            {
                return;
            }
            if (!_watchVariables.ContainsKey(variableShowName))
            {
                try
                {
                    _globalInfo.TestflowEntity.EngineController.AddRuntimeObject("WatchData", 0, sequenceIndex,
                        runtimeVariableName);
                    _watchVariables.Add(variableShowName, runtimeVariableName);
                    dataGridView_variableValues.Rows.Add(variableShowName, "");
                }
                catch (TestflowException ex)
                {
                    Log.Print(LogLevel.ERROR, ex.Message);
                    ShowMessage(ex.Message, "Runtime", MessageBoxIcon.Error);
                }
            }
        }

        private void GetSelectedVariable(out int sequenceIndex, out string runtimeVariableName, out string variableShowName)
        {
            runtimeVariableName = null;
            variableShowName = null;
            sequenceIndex = -3;
            ISequence currentSequence = _eventController.CurrentSequence;
            VariableForm variableForm = new VariableForm(SequenceGroup.Variables, currentSequence.Variables,
                string.Empty);
            variableForm.ShowDialog(this);
            string variableName = variableForm.Value;
            if (variableForm.IsCancelled || string.IsNullOrWhiteSpace(variableName))
            {
                return;
            }
            IVariable variable = null;
            sequenceIndex = variableForm.SequenceIndex;
            if (variableForm.IsGlobalVariable)
            {
                variable = SequenceGroup.Variables.FirstOrDefault(item => item.Name.Equals(variableName));
                sequenceIndex = Constants.SequenceGroupIndex;
            }
            else
            {
                variable = currentSequence.Variables.FirstOrDefault(item => item.Name.Equals(variableName));
            }
            variableShowName = variable?.Name ?? null;
            runtimeVariableName = _sequenceMaintainer.GetRuntimeVariable(sequenceIndex, variableShowName);
        }

        private void button_deleteWatch_Click(object sender, EventArgs e)
        {
            if (null == dataGridView_variableValues.CurrentCell)
            {
                return;
            }
            int rowIndex = dataGridView_variableValues.CurrentCell.RowIndex;
            string variableName = dataGridView_variableValues.Rows[rowIndex].Cells[0].Value.ToString();
            dataGridView_variableValues.Rows.RemoveAt(rowIndex);
            // TODO 后台不删除，只是前台不显示
//            if (_watchVariables.ContainsKey(variableName))
//            {
//                
//            }
        }

        public void ResetRuntimeStatus()
        {
            textBoxReport.Text = string.Empty;
            dataGridView_variableValues.Rows.Clear();
            AddVariableToTable(SequenceGroup.Variables);
            AddVariableToTable(SequenceGroup.SetUp.Variables);
            AddVariableToTable(SequenceGroup.TearDown.Variables);
            foreach (ISequence sequence in SequenceGroup.Sequences)
            {
                AddVariableToTable(sequence.Variables);
            }
        }
        
        private void AddVariableToTable(IVariableCollection variables)
        {
            foreach (IVariable variable in variables.Where(variable => variable.ReportRecordLevel == RecordLevel.Trace ||
                                                                                variable.ReportRecordLevel == RecordLevel.FinalResult))
            {
                dataGridView_variableValues.Rows.Add(variable.Name, "");
            }
        }

        public void AppendOutput(string message)
        {
            int currentLength = textBox_output.TextLength;
            if (message.Length + currentLength > textBox_output.MaxLength)
            {
                int startIndex = currentLength/2;
                textBox_output.Text = textBox_output.Text.Substring(startIndex, currentLength - startIndex);
            }
            textBox_output.AppendText(message);
            textBox_output.AppendText(Environment.NewLine);
            textBox_output.Focus();//获取焦点
            textBox_output.Select(this.textBox_output.TextLength, 0);//光标定位到文本最后
            textBox_output.ScrollToCaret();//滚动到光标处
        }

        public void PrintInfo(string message)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() =>
                {
                    AppendOutput(message);
                }));
            }
            else
            {
                AppendOutput(message);
            }
        }

        public void RefreshVariableValues(IDictionary<string, string> values)
        {
            if (null == values)
            {
                return;
            }
            foreach (DataGridViewRow row in dataGridView_variableValues.Rows)
            {
                string varName = row.Cells[0].Value.ToString();
                if (values.ContainsKey(varName))
                {
                    row.Cells[1].Value = values[varName];
                }
            }
        }

        public void RefreshStepResult(ISequenceStep step, IList<ResultState> stepResult, IList<string> variables)
        {
            UpdateCurrentSequence(step);
            ShowStepResults(step, stepResult, variables);
        }

        private void UpdateCurrentSequence(ISequenceStep step)
        {
            ISequence sequence = GetSequence(step);
            int tabIndex = sequence.Index <= 2 ? 0 : 1;
            if (tabCon_Seq.SelectedIndex != tabIndex)
            {
                tabCon_Seq.SelectedIndex = tabIndex;
            }
            DataGridView seqTable = (DataGridView) tabCon_Seq.TabPages[tabIndex].Controls[0];
            int currentRowIndex = seqTable.CurrentRow.Index;
            ISequence currentSeq = GetSequence(tabIndex, currentRowIndex);
            if (!sequence.Equals(currentSeq))
            {
                int rowIndex = GetSeqRowIndex(sequence.Index);
                seqTable.CurrentCell = seqTable.Rows[rowIndex].Cells[0];
            }
        }

        private void ShowStepResults(ISequenceStep step, IList<ResultState> stepResults, IList<string> variables)
        {
            DataGridView stepTable = _stepTable;
            int stepIndex = step.Index;
            for (int i = 0; i < stepTable.RowCount; i++)
            {
                string variableValue = variables[i];
                DataGridViewCell dataCell = stepTable.Rows[i].Cells[StepTableDataCol];
                if (!variableValue.Equals(dataCell.Value.ToString()))
                {
                    dataCell.Value = variableValue;
                }
                ResultState stepResult = stepResults[i];
                DataGridViewCell resultCell = stepTable.Rows[i].Cells[StepTableResultCol];
                string state = resultCell.Value.ToString();
                string newState = stepResult != ResultState.NA ? stepResult.ToString() : string.Empty;
                if (state.Equals(newState))
                {
                    continue;
                }
                resultCell.Value = newState;
                
                switch (stepResult)
                {
                    case ResultState.NA:
                        resultCell.Style.BackColor = NAColor;
                        break;
                    case ResultState.Running:
                        resultCell.Style.BackColor = RunningColor;
                        break;
                    case ResultState.Skip:
                        resultCell.Style.BackColor = SkipColor;
                        break;
                    case ResultState.Pass:
                        resultCell.Style.BackColor = PassColor;
                        break;
                    case ResultState.Fail:
                        resultCell.Style.BackColor = FailedColor;
                        break;
                    case ResultState.Error:
                        resultCell.Style.BackColor = FailedColor;
                        break;
                    case ResultState.Done:
                        resultCell.Style.BackColor = PassColor;
                        break;
                    default:
                        break;
                }
            }
            stepTable.CurrentCell = stepTable.Rows[stepIndex].Cells[StepTableNameCol];
        }

        private ISequence GetSequence(ISequenceStep step)
        {
            ISequenceStep current = step;
            while (current.Parent is ISequenceStep)
            {
                current = (ISequenceStep) current.Parent;
            }
            return (ISequence) current.Parent;
        }

        private int GetSeqRowIndex(int sequenceIndex)
        {
            if (sequenceIndex <= 2)
            {
                // 运行时状态只显示主序列的数据
                if (viewController_Main.StateValue > (int) RunState.EditProcess)
                {
                    return 0;
                }
                switch (sequenceIndex)
                {
                    case -2:
                        return 4;
                        break;
                    case -1:
                        return 0;
                        break;
                    default:
                        return sequenceIndex + 1;
                        break;
                }
            }
            else
            {
                return sequenceIndex - 3;
            }
        }

        private ISequence GetSequence(int tabIndex, int rowIndex)
        {
            if (tabIndex == 0)
            {
                if (rowIndex == 0)
                {
                    return SequenceGroup.SetUp;
                }
                else if (rowIndex == 4)
                {
                    return SequenceGroup.TearDown;
                }
                else
                {
                    return SequenceGroup.Sequences[rowIndex - 1];
                }
            }
            else
            {
                return SequenceGroup.Sequences[rowIndex + 3];
            }
        }

        public void RunningOver()
        {
            viewController_Main.State = RunState.RunOver.ToString();
            if (_globalInfo.Session.HasAuthority(AuthorityDefinition.EditSequence))
            {
                viewController_Main.State = RunState.EditIdle.ToString();
            }
            if (!_operationPanel.FormUnavaiable)
            {
                _operationPanel.BeginInvoke(new Action(() =>
                {
                    Thread.Sleep(200);
                    _operationPanel.Close();
                }));
            }
        }

        internal void PrintUutResults(IList<ProductTestResult> results)
        {
//            int passedCount = results.Count(item => item.Result == ResultState.Pass);
//            StringBuilder reportValue = new StringBuilder(textBoxReport.Text.Length + 200);
//
//            reportValue.Append($"Total UUT counts:    {results.Count}");
//            reportValue.Append(Environment.NewLine);
//            reportValue.Append($"Passed UUT counts:   {passedCount}");
//            reportValue.Append(Environment.NewLine);
//            reportValue.Append($"Failed UUT counts:   {results.Count - passedCount}");
//            reportValue.Append(Environment.NewLine);
//            reportValue.Append(Environment.NewLine);
//            reportValue.Append(textBoxReport.Text);
//            textBoxReport.Text = reportValue.ToString();
            tabCon_Step.SelectedTab = ReportTab;
        }

        internal void PrintUutResult(string uutResult)
        {
            textBoxReport.Text = uutResult;
//            textBoxReport.AppendText(uutResult);
//            textBoxReport.AppendText(Environment.NewLine);
//            // 滚动到最下面
//            textBoxReport.Select(this.textBox_output.TextLength - 1, 0);//光标定位到文本最后
//            textBoxReport.ScrollToCaret();//滚动到光标处
        }

        public void BreakPointHittedAction(ISequenceStep step, IList<ResultState> stepResult, IList<string> variables,
            IDictionary<string, string> watchData)
        {
            viewController_Main.State = "RunBlock";
            RefreshStepResult(step, stepResult, variables);
            RefreshVariableValues(watchData);
        }

        #endregion

        // 检查权限，如果不通过则弹出错误窗口并返回false
        private bool CheckAuthority(string authority)
        {
            try
            {
                _globalInfo.Session.CheckAuthority(authority);
                return true;
            }
            catch (AuthenticationException ex)
            {
                Log.Print(LogLevel.ERROR, ex.Message);
                ShowMessage(ex.Message, "Authority Check", MessageBoxIcon.Error);
                return false;
            }
        }

        public void ShowMessage(string message, string caption, MessageBoxIcon icon = MessageBoxIcon.Warning)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => { MessageBox.Show(message, caption, MessageBoxButtons.OK, icon); }));
            }
            else
            {
                MessageBox.Show(message, caption, MessageBoxButtons.OK, icon);
            }
        }

        private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AboutForm aboutForm = new AboutForm();
            aboutForm.ShowDialog(this);
        }

        private void addSequenceToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ISequenceGroup sequenceGroup = SequenceGroup;
            ISequence sequence = TestflowDesigntimeSession.AddSequence("", "", sequenceGroup.Sequences.Count);
            TreeNode parentNode = FindTreeNode(sequenceGroup);
            TreeNode newSeqNode = new TreeNode(sequence.Name);
            parentNode.Nodes.Add(newSeqNode);
            treeView_sequenceTree.SelectedNode = newSeqNode;
        }

        private void insertSequenceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode selectedNode = treeView_sequenceTree.SelectedNode;
            ISequence selectedSequence = FindSelectedSequence(selectedNode);
            if (selectedSequence.Index == CommonConst.SetupIndex || selectedSequence.Index == CommonConst.TeardownIndex)
            {
                return;
            }
            SequenceGroup.Sequences.Remove(selectedSequence);
            selectedNode.Parent.Nodes.Remove(selectedNode);
        }

        private void deleteSequenceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode selectedNode = treeView_sequenceTree.SelectedNode;
            ISequence selectedSequence = FindSelectedSequence(selectedNode);
            if (selectedSequence.Index == CommonConst.SetupIndex || selectedSequence.Index == CommonConst.TeardownIndex)
            {
                return;
            }
            SequenceGroup.Sequences.Remove(selectedSequence);
            selectedNode.Parent.Nodes.Remove(selectedNode);
        }

        private TreeNode FindTreeNode(ISequenceGroup sequenceGroup)
        {
            return treeView_sequenceTree.Nodes[0].Nodes[0];
        }

        private TreeNode FindTreeNode(ISequence sequence)
        {
            TreeNode parentNode = treeView_sequenceTree.Nodes[0].Nodes[0];
            TreeNode seqNode;
            if (sequence.Index == CommonConst.SetupIndex)
            {
                seqNode = parentNode.Nodes[0];
            }
            else if (sequence.Index == CommonConst.TeardownIndex)
            {
                seqNode = parentNode.Nodes[1];
            }
            else
            {
                seqNode = parentNode.Nodes[sequence.Index + 2];
            }
            return seqNode;
        }

        private ISequence FindSelectedSequence(TreeNode node)
        {
            if (null == node)
            {
                return null;
            }
            int nodeIndex = node.Index;
            if (node.Level == 1)
            {
                if (nodeIndex == 0)
                {
                    // TestProject SetUp
                    return null;
                }
                else if (nodeIndex == 1)
                {
                    // TestProject TearDown
                    return null;
                }
            }
            else if (node.Level != 2)
            {
                return null;
            }
            ISequenceGroup sequenceGroup = SequenceGroup;
            if (nodeIndex == 0)
            {
                return sequenceGroup.SetUp;
            }
            if (nodeIndex == 1)
            {
                return sequenceGroup.TearDown;
            }
            return sequenceGroup.Sequences[nodeIndex - 2];
        }

        private ISequenceStep FindSelectedStep(TreeNode node)
        {
            if (null == node)
            {
                return null;
            }
            Stack<TreeNode> nodeStack = new Stack<TreeNode>(5);
            do
            {
                nodeStack.Push(node);
                node = node.Parent;
            } while (null != node);
            ISequence currentSeq = CurrentSeq;
            ISequenceStep step = currentSeq.Steps[nodeStack.Pop().Index];
            for (int i = nodeStack.Count - 1; i >= 0; i--)
            {
                step = step.SubSteps[nodeStack.Pop().Index];
            }
            return step;
        }

        private void treeView_sequenceTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            ISequence selectedSequence = FindSelectedSequence(e.Node);
            if (selectedSequence != CurrentSeq)
            {
                CurrentSeq = selectedSequence;
                treeView_stepView.SelectedNode = null;
            }
            CurrentSeq = selectedSequence;
            if (null != CurrentSeq)
            {
                ShowSteps(CurrentSeq);
            }
            CreateDGVVariable(1);
        }

        private void ShowSequences(ISequenceGroup sequenceGroup)
        {
            treeView_sequenceTree.Nodes.Clear();
            if (null == sequenceGroup)
            {
                return;
            }
            TreeNode projectNode = treeView_sequenceTree.Nodes.Add("TestProject");
            TreeNode seqGroupNode = projectNode.Nodes.Add(sequenceGroup.Name);
            seqGroupNode.Nodes.Add(sequenceGroup.SetUp.Name);
            seqGroupNode.Nodes.Add(sequenceGroup.TearDown.Name);
            foreach (ISequence sequence in sequenceGroup.Sequences)
            {
                seqGroupNode.Nodes.Add(sequence.Name);
            }
            treeView_sequenceTree.ExpandAll();
        }

        private void ShowSteps(ISequence sequence)
        {
            treeView_stepView.Nodes.Clear();
            if (null == sequence)
            {
                return;
            }
            TreeNode treeNode = treeView_stepView.Nodes.Add(sequence.Name);
            AddStepToParent(sequence.Steps, treeNode);
            treeView_stepView.ExpandAll();
        }

        private void AddStepToParent(ISequenceStepCollection steps, TreeNode parentNode)
        {
            foreach (ISequenceStep sequenceStep in steps)
            {
                TreeNode stepNode = parentNode.Nodes.Add(sequenceStep.Name);
                if (sequenceStep.HasSubSteps)
                {
                    AddStepToParent(sequenceStep.SubSteps, stepNode);
                }
            }
        }

        private void ShowStepInfo(ISequenceStep step)
        {
            comboBox_asserFailedAction.Text = step.AssertFailedAction.ToString();
            comboBox_invokeFailedAction.Text = step.InvokeErrorAction.ToString();
            comboBox_runType.Text = step.Behavior.ToString();
            if (null != step.LoopCounter)
            {
                LoopTypecomboBox.SelectedIndex = 1;
                LoopTimesnumericUpDown.Value = step.LoopCounter.MaxValue;
                LoopTimesnumericUpDown.Enabled = true;
                numericUpDown_retryTime.Enabled = false;
                numericUpDown_retryTime.Enabled = false;
            }
            else if (null != step.RetryCounter)
            {
                LoopTypecomboBox.SelectedIndex = 2;
                numericUpDown_retryTime.Value = step.RetryCounter.MaxRetryTimes;
                numericUpDown_passTimes.Value = step.RetryCounter.PassTimes;
                LoopTimesnumericUpDown.Enabled = false;
                numericUpDown_retryTime.Enabled = true;
                numericUpDown_retryTime.Enabled = true;
            }
        }

        private void treeView_stepView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            ISequenceStep selectedStep = FindSelectedStep(e.Node);
            CurrentStep = selectedStep;
            if (null != selectedStep)
            {
                ShowStepInfo(selectedStep);
                tabControl_settings.Enabled = true;
            }
            else
            {
                tabControl_settings.Enabled = false;
                tabControl_settings.SelectedIndex = 0;
            }
            UpdateSettings();
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            ISequenceStep selectedStep = FindSelectedStep(treeView_stepView.SelectedNode);
            if (null == selectedStep)
            {
                return;
            }
            RenameForm renameForm = new RenameForm(selectedStep);
            renameForm.ShowDialog(this);
            if (renameForm.Name.Equals(selectedStep.Name))
            {
                return;
            }
            int index = 1;
            ISequenceStepCollection sameLevelSteps = selectedStep.Parent is ISequence
                ? ((ISequence) selectedStep.Parent).Steps
                : ((ISequenceStep) selectedStep.Parent).SubSteps;
            string newName = renameForm.Name;
            while(sameLevelSteps.Any(item => item.Name.Equals(newName)))
            {
                newName = $"{renameForm.Name}{index++}";
            }
            selectedStep.Name = newName;
            treeView_stepView.SelectedNode.Text = newName;
        }

        private void renameSequenceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ISequence selectedSeq = FindSelectedSequence(treeView_sequenceTree.SelectedNode);
            if (null == selectedSeq)
            {
                return;
            }
            RenameForm renameForm = new RenameForm(selectedSeq);
            renameForm.ShowDialog(this);
            if (renameForm.Name.Equals(selectedSeq.Name))
            {
                return;
            }
            int index = 1;
            string newName = renameForm.Name;
            while (SequenceGroup.Sequences.Any(item => item.Name.Equals(newName)))
            {
                newName = $"{renameForm.Name}{index++}";
            }
            selectedSeq.Name = newName;
            treeView_sequenceTree.SelectedNode.Text = newName;
        }
    }
}