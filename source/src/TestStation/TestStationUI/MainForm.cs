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
using TestStation.Runtime;
using TestStationLimit;
using LogLevel = TestStation.Common.LogLevel;

namespace TestStation
{
    public partial class MainForm : Form
    {
        private const string ExistingObjName = "<Existing Object>";
        private const string ExistingObjParent = "Existing Object";

        #region Id
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
        // 表达式匹配模式，第1组为数组的源数据，第二组为数组的变量名称
        private readonly Regex _expRegex;

        #endregion

        #region 私有字段-被选中序列相关的标识

        private string _oldVariableType;
        private string _oldVariableValue;
        private IComInterfaceDescription _currentComDescription;
        private IClassInterfaceDescription _currentClassDescription;

        #endregion

        #region 私有字段-序列控件相关

        // 当前的变量列表
        private DataGridView VaraibleTable => (DataGridView) (tabCon_Variable.SelectedTab.Controls[0]);
        // 参数列表
        private TreeDataGridView _paramTable;

        #endregion

        #region 界面初始化
        
        public MainForm(string sequencePath)
        {
            #region Testflow：模块与服务初始化
            _globalInfo = GlobalInfo.GetInstance();
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
//            tabControl_settings.TabPages.Remove(tabpage_parameters);
            RuntimeStatusForm runtimeStatusForm = new RuntimeStatusForm(this);
            RuntimeStatusTab.Controls.Clear();
            RuntimeStatusTab.Controls.Add(runtimeStatusForm);
            runtimeStatusForm.Dock = DockStyle.Fill;
            runtimeStatusForm.Show();
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
            if (null == CurrentSeq && tabNumber != 0 && tabCon_Variable.TabPages.Count >= 2)
            {
                tabCon_Variable.TabPages[1].Controls.Clear();
                return;
            }
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

            if (tabNumber == 0)
            {
                dgv_variable.Name = "GlobalVariableList";
                globalVariableTab.Controls.Clear();
                globalVariableTab.Controls.Add(dgv_variable);
            }
            else if (null != CurrentSeq) //tabNumber == 1
            {
                if (tabCon_Variable.TabPages.Count == 1)
                {
                    tabCon_Variable.TabPages.Add(CurrentSeq.Name, "Var:" + CurrentSeq.Name);
                }
                else
                {
                    tabCon_Variable.TabPages[1].Name = CurrentSeq.Name;
                    tabCon_Variable.TabPages[1].Text = "Var:" + CurrentSeq.Name;
                }

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
//            _paramTable.CellValueChanged += TdgvParamCellValueChanged;
//            _paramTable.CellContentClick += TdgvParamCellContentClick;
            _paramTable.CellBeginEdit += TdgvParamCellEnterEdit;
        }

        #region 窗体事件

        private void Form1_Load(object sender, EventArgs e)
        {
            viewController_Main.State = RunState.EditIdle.ToString();
            EditModeAction();
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            UpdateToolStripButtonsState();
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
//                _globalInfo.Session.CheckAuthority(AuthorityDefinition.EditSequence);
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
        }


        private void endTimingToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void waitToolStripMenuItem_Click(object sender, EventArgs e)
        {
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
        }

        private void reloginToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        #endregion

        #endregion

        #region 运行时信息窗口事件

        private void button_copyOutput_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBox_output.Text))
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

        #region 步骤列表右键菜单事件

        private void AddAction_Click(object sender, EventArgs e)
        {
        }

        private void AddSequenceCall_Click(object sender, EventArgs e)
        {
        }

        private void stringValueTestToolStripMenuItem1_Click(object sender, EventArgs e)
        {
        }

        private void numericLimitTestToolStripMenuItem1_Click(object sender, EventArgs e)
        {
        }

        private void booleanTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void ToolStripMenuItem_commonTest_Click(object sender, EventArgs e)
        {
        }

        private void booleanTestToolStripMenuItem1_Click(object sender, EventArgs e)
        {
        }

        private void numericLimitTestToolStripMenuItem2_Click(object sender, EventArgs e)
        {
        }

        private void stringValueTestToolStripMenuItem2_Click(object sender, EventArgs e)
        {
        }

        private void actionToolStripMenuItem3_Click(object sender, EventArgs e)
        {
        }

        private void sequenceCallToolStripMenuItem2_Click(object sender, EventArgs e)
        {
        }


        private void startTimingToolStripMenuItem2_Click(object sender, EventArgs e)
        {
        }


        private void endTimingToolStripMenuItem1_Click(object sender, EventArgs e)
        {
        }


        private void waitToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
        }

        #endregion

        #region 变量窗体右键菜单事件


        private void cMS_DgvVariable_Opening(object sender, CancelEventArgs e)
        {
            if (_internalOperation) return;
            _internalOperation = true;

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

            _internalOperation = false;
        }

        private void AddVariable_Click(object sender, EventArgs e)
        {
            if (null == TestflowDesigntimeSession || _internalOperation)
            {
                return;
            }
            _internalOperation = true;
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
            _internalOperation = false;
        }

        private void DeleteVariable_Click(object sender, EventArgs e)
        {
            if (_internalOperation) return;
            _internalOperation = true;
            string varName = VaraibleTable.CurrentRow.Cells["VariableName"].Value.ToString();
            VaraibleTable.Rows.Remove(VaraibleTable.CurrentRow);
            TestflowDesigntimeSession.RemoveVariable(VaraibleTable.Name.Equals("GlobalVariableList") ?
                                                               (ISequenceFlowContainer)SequenceGroup : CurrentSeq,
                                                      varName);
            _internalOperation = false;
        }

        #endregion

        #endregion

        private void buttonOpenReport_Click(object sender, EventArgs e)
        {
//            if (_eventController?.CurrentReport == null || !File.Exists(_eventController.CurrentReport))
//            {
//                return;
//            }
//            try
//            {
//                Process.Start("notepad.exe", _eventController.CurrentReport);
//            }
//            catch (Exception ex)
//            {
//                Log.Print(LogLevel.ERROR, ex.Message);
//            }
        }

        private void button_openReportDir_Click(object sender, EventArgs e)
        {
//            if (_eventController?.ReportDir == null || !Directory.Exists(_eventController.ReportDir))
//            {
//                return;
//            }
//            try
//            {
//                Process.Start(_eventController.ReportDir);
//            }
//            catch (Exception ex)
//            {
//                Log.Print(LogLevel.ERROR, ex.Message);
//            }
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
        }

        #region Sequence表格事件

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
        #endregion

        #region Step详情-Properties页面控件事件

        private void tabControl_settings_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_internalOperation) return;
            _internalOperation = true;
            UpdateSettings();
            _internalOperation = false;
        }

        private void StepTypecomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_internalOperation) { return; }
            _internalOperation = true;
            SetStepProperties();
            _internalOperation = false;
        }

        private void checkBox_RecordResult_CheckedChanged(object sender, EventArgs e)
        {
            if (null == CurrentStep || _internalOperation)
            {
                return;
            }
            _internalOperation = true;
            SetStepProperties();
            _internalOperation = false;
        }
        
        private void LoopTypecomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_internalOperation) return;
            _internalOperation = true;
            ISequenceStep currentStep = CurrentStep;
            if (null == currentStep)
            {
                return;
            }
            SetStepProperties();
            _internalOperation = false;
        }

        private void LoopTimesnumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (_internalOperation)
            {
                return;
            }
            _internalOperation = true;
            SetStepProperties();
            _internalOperation = false;
        }
        
        private void numericUpDown_retryTime_ValueChanged(object sender, EventArgs e)
        {
            if (_internalOperation)
            {
                return;
            }
            _internalOperation = true;
            CurrentStep.RetryCounter.MaxRetryTimes = Convert.ToInt32(numericUpDown_retryTime.Value);
            _internalOperation = false;
        }

        private void numericUpDown_passTimes_ValueChanged(object sender, EventArgs e)
        {
            if (_internalOperation)
            {
                return;
            }
            _internalOperation = true;
            CurrentStep.RetryCounter.PassTimes = Convert.ToInt32(numericUpDown_passTimes.Value);
            _internalOperation = false;
        }

        private void comboBox_conditionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_internalOperation)
            {
                return;
            }
            _internalOperation = true;
            SetStepProperties();
            _internalOperation = false;
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

            if (openSign || _internalOperation)
            {
                return;
            }
            _internalOperation = true;
            // 清空Parameters, comboBox_RootClass, comboBox_Method, _currentClassDescription, _currentFuncDescription
            _paramTable.Clear();
            comboBox_RootClass.DataSource = null;
            comboBox_Method.DataSource = null;
            comboBox_Constructor.DataSource = null;
            _currentClassDescription = null;
            

            // 判断空选项
            if (comboBox_assembly.Text.Equals(""))
            {
                _internalOperation = false;
                _currentComDescription = null;
                return;
            }

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
            _internalOperation = false;
        }

        //类选择改变：展示方法名
        private void comboBox_RootClass_Validated(object sender, EventArgs e)
        {
            if (_internalOperation || stepSelection)
            {
                return;
            }
            _internalOperation = true;
            // 清空Parameters、Method、Constructor
            comboBox_Method.DataSource = null;
            comboBox_Constructor.DataSource = null;
            //_currentFuncDescription = null;
            //_currentConstructorDescription = null;

            _paramTable.Clear();

            // 判断空选项
            if (comboBox_RootClass.SelectedValue == null || comboBox_RootClass.SelectedValue.Equals(""))
            {
                _currentClassDescription = null;
                _internalOperation = false;
                return;
            }

            // Testflow:获得类description
            //获取当前选择的类的description：comboBox_RootClass.SelectedIndex 对应 Classes的Index
            _currentClassDescription = _currentComDescription.Classes[comboBox_RootClass.SelectedIndex];

            // 展示方法名
            ShowConstructorsAndMethods();

            _internalOperation = false;
        }

        private void comboBox_Constructor_Validated(object sender, EventArgs e)
        {
            if (_internalOperation || string.IsNullOrWhiteSpace(comboBox_Constructor.Text) ||
                !comboBox_Constructor.Enabled)
            {
                return;
            }
            _internalOperation = true;
            // 清空Parameter
            _paramTable.Clear();

            InitializeConstructorStep();

            // 添加Parameter
            UpdateTDGVParameter();
            _internalOperation = false;
        }

        private void InitializeConstructorStep()
        {
            // 判断空选项 , Existing Object
            ISequenceStep constructorStep;
            ISequenceStep functionStep;
            GetConstructAndFuncStep(CurrentStep, out constructorStep, out functionStep);
            if (string.IsNullOrWhiteSpace(comboBox_Constructor.Text) || comboBox_Constructor.Text.Equals(ExistingObjName))
            {
                //_currentConstructorDescription = null;
                if (constructorStep != null)
                {
                    TestflowDesigntimeSession.RemoveSequenceStep(CurrentStep, constructorStep);
                }
                return;
            }
            // Testflow: 获得构造函数description
            IFuncInterfaceDescription constructorDescription =
                _currentClassDescription.Functions.FirstOrDefault(
                    item => GetConstructorSignature(item).Equals(comboBox_Constructor.Text));

            // 创建构造函数SubStep
            if (constructorStep == null)
            {
                constructorStep = TestflowDesigntimeSession.AddSequenceStep(CurrentStep, $"Constructor", "Constructor", 1);
            }

            // 判断选择的构造函数有没有变
            if (constructorStep.Function == null ||
                !IsFunctionCreatedFromDescription(constructorStep.Function, constructorDescription))
            {
                // 创建functionData， 并改变_currentFunctionStep的functionData
                IFunctionData constructorData =
                    _globalInfo.TestflowEntity.SequenceManager.CreateFunctionData(constructorDescription);
                constructorStep.Function = constructorData;

                // 删除functionStep里的instance
                if (functionStep?.Function != null)
                {
                    TestflowDesigntimeSession.SetInstance("", functionStep);
                }
            }
        }

        private void comboBox_Method_Validated(object sender, EventArgs e)
        {
            if (_internalOperation)
            {
                return;
            }
            _internalOperation = true;
            // 清空Parameter
            _paramTable.Clear();

            // 根据用户选择的方法，初始化FunctionStep
            InitializeFunctionStep();
            // 如果是实例方法且未配置Constructor，则修改Constructor为ExistingObject
            ISequenceStep functionStep;
            ISequenceStep constructorStep;
            GetConstructAndFuncStep(CurrentStep, out constructorStep, out functionStep);
            if (null != CurrentStep && null != functionStep && IsInstanceFunction(functionStep.Function) &&
                string.IsNullOrWhiteSpace(comboBox_Constructor.Text))
            {
                comboBox_Constructor.Text = ExistingObjName;
            }
            // 添加Parameter
            UpdateTDGVParameter();
            ShowStepInfo(CurrentStep);
            _internalOperation = false;
        }

        private void InitializeFunctionStep()
        {
            ISequenceStep currentStep = CurrentStep;
            if (null == currentStep)
            {
                return;
            }
            ISequenceStep functionStep;
            ISequenceStep constructorStep;
            GetConstructAndFuncStep(CurrentStep, out constructorStep, out functionStep);
            // Static Class, remove Constructor
            if (_currentClassDescription.IsStatic && constructorStep != null)
            {
                TestflowDesigntimeSession.RemoveSequenceStep(currentStep, constructorStep);
            }

            // 判断空选项
            if (string.IsNullOrWhiteSpace(comboBox_Method.Text))
            {
                if (functionStep != null)
                {
                    TestflowDesigntimeSession.RemoveSequenceStep(currentStep, functionStep);
                }
                return;
            }
            // 如果FunctionStep不存在则创建
            if (functionStep == null)
            {
                //添加在constructorStep后面
                functionStep = TestflowDesigntimeSession.AddSequenceStep(currentStep, Constants.MethodStepName,
                    "Method", (constructorStep == null) ? 1 : 2);
            }

            IFuncInterfaceDescription funcDescription =
                    _currentClassDescription.Functions.FirstOrDefault(
                        item => GetMethodSignature(item).Equals(comboBox_Method.Text));
            //_currentFunctionStep没有方法 或 新的funcDescription
            if (functionStep.Function == null ||
                !IsFunctionCreatedFromDescription(functionStep.Function, funcDescription))
            {
                //创建functionData， 并改变_currentFunctionStep的functionData
                IFunctionData functionData = _globalInfo.TestflowEntity.SequenceManager.CreateFunctionData(funcDescription);
                functionStep.Function = functionData;
            }
            //Instance是constructorStep的instance
            if (!string.IsNullOrWhiteSpace(constructorStep?.Function.Instance))
            {
                functionStep.Function.Instance = constructorStep.Function.Instance;
            }
        }

        private bool IsInstanceFunction(IFunctionData function)
        {
            return null != function && function.Type != FunctionType.StaticFieldSetter &&
                   function.Type != FunctionType.StaticFunction && function.Type != FunctionType.StaticPropertySetter;
        }

        private void TdgvParamCellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // ColumnHeaders不作为,标题行不作为
            if (e.RowIndex < 0 || _paramTable.IsParent(e.RowIndex) || _internalOperation)
            {
                return;
            }
            _internalOperation = true;

            try
            {
                int columnIndex = e.ColumnIndex;
                if (_paramTable.Columns[columnIndex].Name.Equals("Parameterfx"))
                {
                    SetParamValueFromFx(e);
                }
                else if (_paramTable.Columns[columnIndex].Name.Equals("ParameterCheck"))
                {
                    CheckParamValue(e);
                }
            }
            catch (ApplicationException ex)
            {
                ShowMessage(ex.Message, "Set Parameter", MessageBoxIcon.Error);
            }
            _internalOperation = false;
        }

        private void SetParamValueFromFx(DataGridViewCellEventArgs e)
        {
            string value = _paramTable.Rows[e.RowIndex].Cells["ParameterValue"].Value?.ToString();
            VariableForm variableForm = new VariableForm(SequenceGroup.Variables, CurrentSeq.Variables, _globalInfo, value,
                true);
            variableForm.ShowDialog(this);
            if (!variableForm.IsCancelled)
            {
                SetParamValue(true, e.RowIndex, ParamTableValueCol);
                _paramTable.Rows[e.RowIndex].Cells["ParameterValue"].Value = variableForm.ParamValue;
            }
            variableForm.Dispose();
        }

        private void CheckParamValue(DataGridViewCellEventArgs e)
        {
            string name = _paramTable.Rows[e.RowIndex].Cells["ParameterName"].Value.ToString();
            string value = _paramTable.Rows[e.RowIndex].Cells["ParameterValue"].Value?.ToString();
            string group = _paramTable.FindNodeGroup(e.RowIndex);
            IWarningInfo warningInfo = null;
            ISequenceFlowContainer[] arr = new ISequenceFlowContainer[] {SequenceGroup, CurrentSeq};

            #region Constructor

            ISequenceStep functionStep;
            ISequenceStep constructorStep;
            GetConstructAndFuncStep(CurrentStep, out constructorStep, out functionStep);
            if (@group.Equals("Constructor"))
            {
                #region 检查 Existing Object

                if (name.Equals(ExistingObjParent))
                {
                    if (functionStep != null)
                    {
                        warningInfo = _globalInfo.TestflowEntity.ParameterChecker.CheckInstance(functionStep, arr, false);
                    }
                    else
                    {
                        if (
                            (warningInfo =
                                _globalInfo.TestflowEntity.ParameterChecker.CheckPropertyType(CurrentSeq, value,
                                    _currentClassDescription.ClassType, false)).WarnCode == 1025)
                        {
                            warningInfo = _globalInfo.TestflowEntity.ParameterChecker.CheckPropertyType(SequenceGroup, value,
                                _currentClassDescription.ClassType, false);
                        }
                    }
                }
                    #endregion

                    #region 检查构造函数

                else
                {
                    #region 空值

                    //todo 如果值为空表示，实例不为变量，在运行开始前再创造变量
                    if (string.IsNullOrEmpty(value) || !name.Equals("Return Value"))
                    {
                    }
                        #endregion

                    else
                    {
                        warningInfo = _globalInfo.TestflowEntity.ParameterChecker.CheckInstance(constructorStep, arr, false);
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
                    warningInfo = _globalInfo.TestflowEntity.ParameterChecker.CheckReturn(functionStep, arr, false);
                }
                    #endregion

                    #region 检查参数

                else
                {
                    warningInfo = _globalInfo.TestflowEntity.ParameterChecker.CheckParameterData(functionStep.Function,
                        (functionStep.Function.ReturnType == null)
                            ? _paramTable.IndexInGroup(e.RowIndex)
                            : _paramTable.IndexInGroup(e.RowIndex - 1),
                        arr, false);
                }

                #endregion
            }

            #endregion

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
            ISequenceStep functionStep;
            ISequenceStep constructorStep;
            GetConstructAndFuncStep(CurrentStep, out constructorStep, out functionStep);
            if (@group.Equals(Constants.MethodStepName) && functionStep?.Function != null)
            {
                function = functionStep.Function;
            }
            else if (constructorStep?.Function != null)
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
                SetParamValue(false, e.RowIndex, e.ColumnIndex);
            }
            catch (ApplicationException ex)
            {
                ShowMessage(ex.Message, "Set Parameter", MessageBoxIcon.Error);
                
                // 将默认值配置为空，连带更新值的变更
                _paramTable.Rows[e.RowIndex].Cells["ParameterValue"].Value = string.Empty;
            }
            _internalOperation = false;
        }

        private void SetParamValue(bool setByFx, int rowIndex, int columnIndex)
        {
            //注：只有value的值会改变
            string paramName = _paramTable.Rows[rowIndex].Cells["ParameterName"].Value?.ToString() ?? string.Empty;
            DataGridViewCell currentCell = _paramTable.Rows[rowIndex].Cells["ParameterValue"];
            string value = currentCell.Value?.ToString() ?? string.Empty;
            // 如果是在编辑第三列的数据，并且值等于local.或者global.，则弹出变量选择列表
            if (columnIndex == 4 &&
                (value.Equals(Constants.GlobalVarPrefix + Constants.VaraibleDelim,
                    StringComparison.CurrentCultureIgnoreCase) ||
                 value.Equals(Constants.LocalVarPrefix + Constants.VaraibleDelim, StringComparison.CurrentCultureIgnoreCase)))
            {
                Rectangle rectangle = _paramTable.GetCellDisplayRectangle(columnIndex, rowIndex, true);
                ShowVariableList(value, rowIndex, rectangle);
                return;
            }

            
            ISequenceStep functionStep;
            ISequenceStep constructorStep;
            GetConstructAndFuncStep(CurrentStep, out constructorStep, out functionStep);
            ISequenceStep currentStep = CurrentStep;
            // 修改构造方法的参数
            if (_paramTable.FindNodeGroup(rowIndex).Equals("Constructor"))
            {
                // 实例为已存在的变量
                if (paramName.Equals(ExistingObjParent))
                {
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        TestflowDesigntimeSession.SetInstance(string.Empty, functionStep);
                        return;
                    }
                    IVariable variable = GetAvailableVariable(currentStep, value, functionStep.Function.ClassType);
                    TestflowDesigntimeSession.SetInstance(value, functionStep);
                    SetVariableType(functionStep.Function.ClassType, variable);
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
                        TestflowDesigntimeSession.SetInstance(value, constructorStep);
                        SetVariableType(constructorStep.Function.ClassType, variable);
                        if (null != functionStep)
                        {
                            TestflowDesigntimeSession.SetInstance(value, functionStep);
                        }
                        currentCell.Value = (null != variable) ? Utility.GetShowVariableName(variable) : string.Empty;
                    }
                    else
                    {
                        SetStepParamValue(constructorStep, paramName, value, currentCell, setByFx);
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
                    SetStepParamValue(functionStep, paramName, value, currentCell, setByFx);
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

        private void SetStepParamValue(ISequenceStep step, string paramName, string value, DataGridViewCell currentCell, bool setByFx)
        {
            IArgument argument = step.Function.ParameterType.FirstOrDefault(item => item.Name.Equals(paramName));
            if (null == argument)
            {
                return;
            }
            // 如果确认为变量，则直接写入参数
            if (setByFx)
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


        #region UI上展示数据 Show/Update

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
                if (null == variable.Type)
                {
                    rowIndex = dgv.Rows.Add(variable.Name, "Object", "");
                }
                else
                {
                    switch (variable.Type.Name)
                    {
                        case "String":
                            rowIndex = dgv.Rows.Add(variable.Name, "String", variable.Value);
                            break;
                        case "Boolean":
                            rowIndex = dgv.Rows.Add(variable.Name, "Boolean", variable.Value);
                            break;
                        case "Double":
                        case "Single":
                        case "Int32":
                        case "UInt32":
                        case "Int16":
                        case "UInt16":
                        case "Byte":
                        case "Char":
                            rowIndex = dgv.Rows.Add(variable.Name, "Numeric", variable.Value);
                            break;
                        //Object
                        default:
                            rowIndex = dgv.Rows.Add(variable.Name, "Object", variable.Type.Name);
                            //                            dgv.Rows[rowIndex].Cells["VariableType"].ReadOnly = true;
                            break;
                    }
                }
                dgv.Rows[rowIndex].Tag = variable.Name;
            }
        }

        private void UpdateSingleVariable(IVariable variable, bool isGlobal)
        {
            DataGridView varTable = isGlobal
                ? (DataGridView) tabCon_Variable.TabPages[0].Controls[0]
                : (DataGridView) tabCon_Variable.TabPages[1].Controls[0];
            DataGridViewRow editRow = null;
            foreach (DataGridViewRow row in varTable.Rows)
            {
                if (variable.Name.Equals(row.Cells[0].Value?.ToString()))
                {
                    editRow = row;
                }
            }
            if (null == editRow)
            {
                return;
            }
            string type;
            string value;
            if (null == variable.Type)
            {
                type = "Object";
                value = "";
            }
            else
            {
                
                switch (variable.Type.Name)
                {
                    case "String":
                        type= "String";
                        value = variable.Value ?? string.Empty;
                        break;
                    case "Boolean":
                        type = "Boolean";
                        value = variable.Value ?? true.ToString();
                        break;
                    case "Double":
                    case "Single":
                    case "Int32":
                    case "UInt32":
                    case "Int16":
                    case "UInt16":
                    case "Byte":
                    case "Char":
                        type = "Numeric";
                        value = variable.Value ?? "0";
                        break;

                    //Object
                    default:
                        type = "Object";
                        value = variable.Type.Name;
                        break;
                }
            }
            editRow.Cells[1].Value = type;
            editRow.Cells[2].Value = value;
        }

        private void UpdateModule()
        {
            ISequenceStep functionStep;
            ISequenceStep constructorStep;
            GetConstructAndFuncStep(CurrentStep, out constructorStep, out functionStep);
            if (functionStep == null && constructorStep == null)
            {
                return;
            }
            IFunctionData function = (functionStep == null)
                ? (constructorStep.Function)
                : (functionStep.Function);
            if (null == function || null == function.ClassType || string.IsNullOrWhiteSpace(function.MethodName))
            {
                ClearSettings();
                return;
            }
            _currentComDescription = _testflowDesigntimeService.Components[function.ClassType.AssemblyName];
            if (!comboBox_assembly.Items.Contains(_currentComDescription.Assembly.Path))
            {
                comboBox_assembly.Items.Add(_currentComDescription.Assembly.Path);
            }
            comboBox_assembly.Text = _currentComDescription.Assembly.Path;

            ShowClasses(function.ClassType);

            ShowConstructorsAndMethods(functionStep?.Function, constructorStep?.Function);

            UpdateTDGVParameter();
        }

        private void UpdateSettings()
        {
            ISequenceStep step = FindSelectedStep(treeView_stepView.SelectedNode);
            if (null == step)
            {
//                tabControl_settings.Enabled = false;
//                tabControl_settings.SelectedIndex = 0;
                ClearSettings();
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
            // 清空Properties
            //注：StepTypeComboBox与LoopTypeComboBox没有空值，也就是不用把SelectedIndex变成-1.
            LoopTimesnumericUpDown.Value = 2;
            ClearModules();
        }

        private void ClearModules()
        {
            // 清空module
            comboBox_assembly.Text = "";
            comboBox_RootClass.DataSource = null;
            comboBox_RootClass.Text = "";
            comboBox_Method.DataSource = null;
            comboBox_Method.Text = "";
            comboBox_Constructor.DataSource = null;
            comboBox_Method.Text = "";
            _paramTable.Clear();
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

            ISequenceStep constructorStep;
            ISequenceStep functionStep;
            GetConstructAndFuncStep(CurrentStep, out constructorStep, out functionStep);
            if (functionStep?.Function != null && functionStep.Function.Instance != "")
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

        private void GetConstructAndFuncStep(ISequenceStep currentStep, out ISequenceStep constructorStep, out ISequenceStep functionStep)
        {
            constructorStep = null != currentStep.Function && currentStep.Function.Type == FunctionType.Constructor
                ? currentStep
                : null;
            functionStep = null == constructorStep ? currentStep : null;
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
            ISequenceStep constructorStep;
            ISequenceStep functionStep;
            GetConstructAndFuncStep(CurrentStep, out constructorStep, out functionStep);
            #region 构造函数
            if (!string.IsNullOrWhiteSpace(comboBox_Constructor.Text))
            {
                // 包含ConstructorStep
                if (!comboBox_Constructor.Text.Equals(ExistingObjName) && null != constructorStep?.Function)
                {
                    _paramTable.AddParent($"{ _currentClassDescription.ClassType.Namespace}.{ _currentClassDescription.ClassType.Name}", "Constructor");
                    ShowReturnAndParameters(constructorStep, constructorStep.Function);
                }
                else if (comboBox_Constructor.SelectedValue.Equals(ExistingObjName) && null != functionStep?.Function)
                {
                    _paramTable.AddParent(ExistingObjParent, "Constructor");
                    string instanceValue = Utility.GetShowVariableName(functionStep.Function.Instance,
                        functionStep);
                    _paramTable.Rows.Add(new object[] { null, ExistingObjParent,
                    $"Object({_currentClassDescription.ClassType.Namespace}.{_currentClassDescription.ClassType.Name})", "In",
                                                        instanceValue});
                }
            }
            #endregion

            // 方法显示
            if (!string.IsNullOrWhiteSpace(comboBox_Method.Text) && null != functionStep)
            {
                _paramTable.AddParent($"{comboBox_Method.SelectedValue}", "Method");
                ShowReturnAndParameters(functionStep, functionStep.Function);
            }
        }

        #endregion

        #region 状态更新事件及界面联动

        private void viewController_Main_PreListeners(int oldState, int newState)
        {
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
            treeView_sequenceTree.ContextMenuStrip = contextMenuStrip_sequence;
            treeView_stepView.ContextMenuStrip = cMS_DgvStep;
            // 隐藏运行时变量值窗体
            splitContainer_runtime.Panel1Collapsed = true;
            // step表格只读，且不响应值变更事件
            treeView_stepView.AfterSelect += treeView_stepView_AfterSelect;

            // 隐藏返回编辑状态的菜单
            editSequenceToolStripMenuItem.Visible = false;
            // 显示编辑序列的菜单项
            addSequenceToolStripMenuItem.Visible = true;
            addStepToolStripMenuItem.Visible = true;

            if (null != SequenceGroup)
            {
                ShowSequences(SequenceGroup);
            }
        }

        private void RunModeAction()
        {
            _paramTable.CellValueChanged -= TdgvParamCellValueChanged;
            _paramTable.CellContentClick -= TdgvParamCellContentClick;
            treeView_sequenceTree.ContextMenuStrip = null;
            treeView_stepView.ContextMenuStrip = null;
            // 显示运行时变量值窗体
            splitContainer_runtime.Panel1Collapsed = false;
            // step表格只读，且不响应值变更事件
            treeView_stepView.AfterSelect -= treeView_stepView_AfterSelect;

            // 显示运行时信息列
            // 显示返回编辑状态的菜单
            editSequenceToolStripMenuItem.Visible = true;
            // 隐藏编辑序列的菜单项
            addSequenceToolStripMenuItem.Visible = false;
            addStepToolStripMenuItem.Visible = false;
            // 删除step表格的右键菜单
            if (null != SequenceGroup)
            {
                ShowSequences(SequenceGroup);
            }
        }

        private void ClearAll()
        {
            // Variable
            if (tabCon_Variable.TabPages.Count > 1)
            {
                tabCon_Variable.TabPages.RemoveAt(1);
            }
            ((DataGridView)globalVariableTab.Controls[0]).Rows.Clear();
            // Module
//            tabControl_settings.Visible = false;
            comboBox_assembly.Text = "";
            comboBox_RootClass.DataSource = null;
            comboBox_RootClass.Text = "";
            comboBox_Method.DataSource = null;
            comboBox_Method.Text = "";
            _paramTable?.Clear();
            // Limit
            treeView_stepView.Nodes.Clear();
            treeView_sequenceTree.Nodes.Clear();
        }

        #endregion

        #region 序列组管理

        // 创建新的序列
        private void CreateNewSequence()
        {
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

            _currentSequenceId = 0;
            _currentVariableId = 0;

            #endregion

            string projectName = "SequenceGroup";
            labelProject.Text = projectName + Constants.ProjectNamePostfix;

            // Testflow: 创建新的TestProject => SequenceGroup => Setup/Cleanup, MainSequence
            _testflowDesigntimeService.Unload();
            _testflowDesigntimeService.Load("Test Project", "");
            _testflowDesigntimeService.AddComponent(_interfaceManger.GetComponentDescriptions()[0]); //加载System.mscorlib
            _testflowDesigntimeService.AddSequenceGroup(projectName, "");

            // Sequence UI
            tabCon_Seq.SelectedIndex = 0;
            ShowSequences(SequenceGroup);
            EnableAllEditControl();
            treeView_sequenceTree.SelectedNode = treeView_sequenceTree.Nodes[0].Nodes[0];
            treeView_stepView.SelectedNode = null;
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

            #region 清空UI

            ClearAll();

            #endregion

            #region Variables UI

            ShowVariables(0);

            #endregion

            ResetPathComboBox();
            // Sequence UId
            ShowSequences(loadedSequenceGroup);
            tabCon_Seq.SelectedIndex = 0;
            UpdateSettings();
            EnableAllEditControl();
            tabCon_Step.SelectedTab = tabPage_stepData;
            UpdateToolStripButtonsState();
            treeView_sequenceTree.SelectedNode = treeView_sequenceTree.Nodes[0].Nodes[0];
            treeView_stepView.SelectedNode = null;
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

        private void AddSequenceStep(object sender, EventArgs args)
        {
            if (_internalOperation || null == CurrentSeq)
            {
                return;
            }
            ToolStripMenuItem control = sender as ToolStripMenuItem;
            string tag = control?.Tag?.ToString();
            SequenceStepType type;
            if (string.IsNullOrWhiteSpace(tag) || !Enum.TryParse<SequenceStepType>(tag, out type))
            {
                return;
            }
            _internalOperation = true;
            AddStep(type);
            _internalOperation = false;
        }

        private void InsertSequenceStep(object sender, EventArgs args)
        {
            if (_internalOperation || null == CurrentSeq)
            {
                return;
            }
            ToolStripMenuItem control = sender as ToolStripMenuItem;
            string tag = control?.Tag?.ToString();
            SequenceStepType type;
            if (string.IsNullOrWhiteSpace(tag) || !Enum.TryParse<SequenceStepType>(tag, out type))
            {
                return;
            }
            _internalOperation = true;
            InsertStep(type);
            _internalOperation = false;
        }

        private void DeleteSequenceStep(object sender, EventArgs args)
        {
            if (_internalOperation || null == CurrentSeq || null == CurrentStep)
            {
                return;
            }
            _internalOperation = true;
            DeleteStep();
            _internalOperation = false;
        }

        private void AddStep(SequenceStepType type)
        {
            TreeNode selectedNode = treeView_stepView.SelectedNode;
            ISequenceStep selectedStep = FindSelectedStep(selectedNode);
            if (null == selectedNode || (null == selectedStep && selectedNode.Level != 0))
            {
                ClearSettings();
                return;
            }
            TreeNode parentNode = selectedNode;
            ISequenceFlowContainer parent = selectedStep;
            int index = selectedStep?.SubSteps?.Count ?? 0;
            if (null == selectedStep)
            {
                parent = CurrentSeq;
                index = CurrentSeq.Steps.Count;
            }
            ISequenceStep newStep = AddStepToParent(parent, index, type);
            ShowAddStep(newStep, parentNode);
            CurrentStep = newStep;
            UpdateSettings();
        }

        private void InsertStep(SequenceStepType type)
        {
            TreeNode selectedNode = treeView_stepView.SelectedNode;
            ISequenceStep selectedStep = FindSelectedStep(selectedNode);
            if (null == selectedStep)
            {
                ClearSettings();
                return;
            }
            TreeNode parentNode = selectedNode.Parent;
            ISequenceFlowContainer parent = selectedStep.Parent;
            int index = selectedStep.Index;
            ISequenceStep newStep = AddStepToParent(parent, index, type);
            ShowAddStep(newStep, parentNode);
            CurrentStep = newStep;
            UpdateSettings();
        }

        private ISequenceStep AddStepToParent(ISequenceFlowContainer parent, int index, SequenceStepType type)
        {
            ISequenceStep newStep = type == SequenceStepType.Execution
                ? _globalInfo.TestflowEntity.SequenceManager.CreateSequenceStep(false)
                : _globalInfo.TestflowEntity.SequenceManager.CreateNonExecutionStep(type);
            TestflowDesigntimeSession.AddSequenceStep(parent, newStep, index);
            return newStep;
        }

        private void ShowAddStep(ISequenceStep step, TreeNode parent)
        {
            TreeNode newNode;
            if (step.Index >= parent.Nodes.Count)
            {
                newNode = parent.Nodes.Add(step.Name);
            }
            else
            {
                newNode = parent.Nodes.Insert(step.Index, step.Name);
            }
            if (step.HasSubSteps)
            {
                foreach (ISequenceStep subStep in step.SubSteps)
                {
                    ShowAddStep(subStep, newNode);
                }
            }
            treeView_stepView.SelectedNode = newNode;
        }

        private void DeleteStep()
        {
            TreeNode selectedNode = treeView_stepView.SelectedNode;
            ISequenceStep selectedStep = FindSelectedStep(selectedNode);
            if (null == selectedNode)
            {
                ClearSettings();
                return;
            }
            TreeNode parentNode = selectedNode.Parent;
            int index = selectedStep.Index;
            DeleteStepFromParent(selectedStep.Parent, index);
            CurrentStep = ShowDeleteStep(selectedStep, parentNode);
            UpdateSettings();
        }

        private void DeleteStepFromParent(ISequenceFlowContainer parent, int index)
        {
            ISequenceStep parentStep = parent as ISequenceStep;
            ISequence parentSeq = parent as ISequence;

            if (parentStep?.SubSteps != null && parentStep.SubSteps.Count > index)
            {
                parentStep.SubSteps.RemoveAt(index);
            }
            else if (null != parentSeq && parentSeq.Steps.Count > index)
            {
                parentSeq.Steps.RemoveAt(index);
            }
        }

        private ISequenceStep ShowDeleteStep(ISequenceStep step, TreeNode parent)
        {
            if (parent.Nodes.Count <= step.Index)
            {
                return null;
            }
            parent.Nodes.RemoveAt(step.Index);
            TreeNode selectNode = null;
            ISequenceStep currentStep;
            ISequenceStepCollection stepCollection = step.Parent is ISequenceStep
                ? ((ISequenceStep) step.Parent).SubSteps
                : ((ISequence) step.Parent).Steps;

            if (parent.Nodes.Count == 0)
            {
                selectNode = parent;
                currentStep = null;
            }
            else if (step.Index > 0)
            {
                selectNode = treeView_stepView.Nodes[step.Index - 1];
                currentStep = stepCollection[step.Index - 1];
            }
            else
            {
                selectNode = treeView_stepView.Nodes[step.Index];
                currentStep = stepCollection[step.Index];
            }
            treeView_stepView.SelectedNode = selectNode;
            return currentStep;
        }

        #endregion

        #region 序列运行

        private void RunSequence()
        {
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

            // 参数检查

            _start = true;

            try
            {
                // 配置引擎以调试方式运行
                _globalInfo.TestflowEntity.EngineController.ConfigData.SetProperty("RuntimeType", RuntimeType.Debug);
                // Runtime SequenceGroup
                ISequenceGroup runtimeSequenceGroup = SequenceGroup;
                // Testflow: RuntimeService.Load
                _testflowRuntimeService.Initialize();
                _testflowRuntimeService.Load(runtimeSequenceGroup);
                _globalInfo.TestflowEntity.EngineController.ConfigData.SetProperty("TestName", SequenceGroup.Name);

                // 添加事件
                tabCon_Seq.SelectedIndex = 0;
                ResetRuntimeStatus();
                tabControl_settings.SelectedTab = tabPage_runtimeInfo;
                _watchVariables.Clear();
                ShowOperationPanel(runtimeSequenceGroup);
                RuntimeStatusForm runtimeStatusForm = (RuntimeStatusForm)RuntimeStatusTab.Controls[0];
                runtimeStatusForm.LoadSequence(runtimeSequenceGroup);
                runtimeStatusForm.RegisterEvent();
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
//            RuntimeDataCache dataCache = new RuntimeDataCache(_globalInfo.TestflowEntity,
//                _globalInfo.Equipment)
//            {
//                EnableStartTiming = _sequenceMaintainer.UserStartTiming,
//                EnableEndTiming = _sequenceMaintainer.UserEndTiming,
//                MainStartStack = _sequenceMaintainer.MainStartStack,
//                MainOverStack = _sequenceMaintainer.MainOverStack,
//                PostStartStack = _sequenceMaintainer.PostStartStack,
//                SequenceData = runtimeSequence,
//                RunType = RunType.Slave,
//                SequenceName = SequenceGroup.Name,
//                Target = _globalInfo.ConfigManager.GetConfig<long>("Target")
//            };
//            _eventController.DataCache = dataCache;
//            dataCache.InitModelInfo(_globalInfo.Equipment);
//            ThreadPool.QueueUserWorkItem(state =>
//            {
//                _operationPanel = new OperationPanelForm(dataCache);
//                _operationPanel.Initialize();
//                Application.Run(_operationPanel);
//            });
//            // 等待
//            while (null == _operationPanel)
//            {
//                Thread.Yield();
//            }
//            _operationPanel.Start();
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

            toolStripButton_New.Enabled = true;
            toolStripButton_Open.Enabled = true;
            toolStripButton_Save.Enabled = true;
            toolStripButton_Run.Enabled = true;
            toolStripButton_Suspend.Enabled = false;
            toolStripButton_Stop.Enabled = false;

            Menu_AddStep.Enabled = true;
            Menu_DeleteStep.Enabled = true;
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
//            ISequence currentSequence = _eventController.CurrentSequence;
//            VariableForm variableForm = new VariableForm(SequenceGroup.Variables, currentSequence.Variables,
//                string.Empty);
//            variableForm.ShowDialog(this);
//            string variableName = variableForm.Value;
//            if (variableForm.IsCancelled || string.IsNullOrWhiteSpace(variableName))
//            {
//                return;
//            }
//            IVariable variable = null;
//            sequenceIndex = variableForm.SequenceIndex;
//            if (variableForm.IsGlobalVariable)
//            {
//                variable = SequenceGroup.Variables.FirstOrDefault(item => item.Name.Equals(variableName));
//                sequenceIndex = Constants.SequenceGroupIndex;
//            }
//            else
//            {
//                variable = currentSequence.Variables.FirstOrDefault(item => item.Name.Equals(variableName));
//            }
//            variableShowName = variable?.Name ?? null;
//            runtimeVariableName = variableShowName;
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
//            DataGridView stepTable = _stepTable;
//            int stepIndex = step.Index;
//            for (int i = 0; i < stepTable.RowCount; i++)
//            {
//                string variableValue = variables[i];
//                DataGridViewCell dataCell = stepTable.Rows[i].Cells[StepTableDataCol];
//                if (!variableValue.Equals(dataCell.Value.ToString()))
//                {
//                    dataCell.Value = variableValue;
//                }
//                ResultState stepResult = stepResults[i];
//                DataGridViewCell resultCell = stepTable.Rows[i].Cells[StepTableResultCol];
//                string state = resultCell.Value.ToString();
//                string newState = stepResult != ResultState.NA ? stepResult.ToString() : string.Empty;
//                if (state.Equals(newState))
//                {
//                    continue;
//                }
//                resultCell.Value = newState;
//                
//                switch (stepResult)
//                {
//                    case ResultState.NA:
//                        resultCell.Style.BackColor = NAColor;
//                        break;
//                    case ResultState.Running:
//                        resultCell.Style.BackColor = RunningColor;
//                        break;
//                    case ResultState.Skip:
//                        resultCell.Style.BackColor = SkipColor;
//                        break;
//                    case ResultState.Pass:
//                        resultCell.Style.BackColor = PassColor;
//                        break;
//                    case ResultState.Fail:
//                        resultCell.Style.BackColor = FailedColor;
//                        break;
//                    case ResultState.Error:
//                        resultCell.Style.BackColor = FailedColor;
//                        break;
//                    case ResultState.Done:
//                        resultCell.Style.BackColor = PassColor;
//                        break;
//                    default:
//                        break;
//                }
//            }
//            stepTable.CurrentCell = stepTable.Rows[stepIndex].Cells[StepTableNameCol];
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
            viewController_Main.State = RunState.EditIdle.ToString();
            // TODO 释放OI面板
//            if (!_operationPanel.FormUnavaiable)
//            {
//                _operationPanel.BeginInvoke(new Action(() =>
//                {
//                    Thread.Sleep(200);
//                    _operationPanel.Close();
//                }));
//            }
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
            tabCon_Step.SelectedTab = RuntimeStatusTab;
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

        private void ShowMessage(string message, string caption, MessageBoxIcon icon = MessageBoxIcon.Warning)
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
            ISequenceGroup sequenceGroup = SequenceGroup;
            ISequence selectedSequence = FindSelectedSequence(treeView_sequenceTree.SelectedNode);
            if (null == selectedSequence || selectedSequence.Index == CommonConst.SetupIndex ||
                selectedSequence.Index == CommonConst.TeardownIndex)
            {
                return;
            }
            ISequence sequence = TestflowDesigntimeSession.AddSequence("", "", selectedSequence.Index);
            TreeNode parentNode = FindTreeNode(sequenceGroup);
            TreeNode newSeqNode = new TreeNode(sequence.Name);
            parentNode.Nodes.Insert(sequence.Index + 2, newSeqNode);
            treeView_sequenceTree.SelectedNode = newSeqNode;
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
            if (null == node || node.Level == 0)
            {
                return null;
            }
            Stack<TreeNode> nodeStack = new Stack<TreeNode>(5);
            do
            {
                nodeStack.Push(node);
                node = node.Parent;
            } while (null != node && node.Level > 0);
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
            if (_internalOperation || _lastSelectSeqNode == e.Node) return;
            _internalOperation = true;

            if (null != _lastSelectSeqNode)
            {
                _lastSelectSeqNode.ForeColor = _nonSelectedColor;
                _lastSelectSeqNode = null;
            }
            ISequence selectedSequence = FindSelectedSequence(e.Node);
            if (selectedSequence != CurrentSeq)
            {
                CurrentSeq = selectedSequence;
                treeView_stepView.SelectedNode = null;
                _lastSelectSeqNode = e.Node;
                _lastSelectSeqNode.ForeColor = _selectedColor;
            }
            CurrentSeq = selectedSequence;
            if (null != CurrentSeq)
            {
                ShowSteps(CurrentSeq);
            }
            CreateDGVVariable(1);
            UpdateSettings();
            _internalOperation = false;
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
            checkBox_RecordStatus.Checked = step.RecordStatus;
            if (null != step.LoopCounter)
            {
                LoopTypecomboBox.SelectedIndex = 1;
                LoopTimesnumericUpDown.Value = step.LoopCounter.MaxValue;
                numericUpDown_retryTime.Value = 0;
                numericUpDown_passTimes.Value = 1;
                LoopTimesnumericUpDown.Enabled = true;
                numericUpDown_passTimes.Enabled = false;
                numericUpDown_retryTime.Enabled = false;
            }
            else if (null != step.RetryCounter)
            {
                LoopTypecomboBox.SelectedIndex = 2;
                LoopTimesnumericUpDown.Value = 1;
                numericUpDown_retryTime.Value = step.RetryCounter.MaxRetryTimes;
                numericUpDown_passTimes.Value = step.RetryCounter.PassTimes;
                LoopTimesnumericUpDown.Enabled = false;
                numericUpDown_passTimes.Enabled = true;
                numericUpDown_retryTime.Enabled = true;
            }
            else
            {
                LoopTypecomboBox.SelectedIndex = 0;
                LoopTimesnumericUpDown.Value = 1;
                numericUpDown_retryTime.Value = 0;
                numericUpDown_passTimes.Value = 1;
                LoopTimesnumericUpDown.Enabled = false;
                numericUpDown_passTimes.Enabled = true;
                numericUpDown_retryTime.Enabled = true;
            }
        }

        private TreeNode _lastSelectSeqNode;
        private TreeNode _lastSelectStepNode;
        private Color _selectedColor = Color.DeepSkyBlue;
        private Color _nonSelectedColor = Color.FromArgb(0, 0, 0, 0);

        private void treeView_stepView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (_internalOperation || _lastSelectStepNode == e.Node) return;
            _internalOperation = true;

            if (null != _lastSelectStepNode)
            {
                _lastSelectStepNode.ForeColor = _nonSelectedColor;
                _lastSelectStepNode = null;
            }
            ISequenceStep selectedStep = FindSelectedStep(e.Node);
            CurrentStep = selectedStep;
            if (null != selectedStep)
            {
                ShowStepInfo(selectedStep);
                tabControl_settings.Visible = true;
                _lastSelectStepNode = e.Node;
                _lastSelectStepNode.ForeColor = _selectedColor;
            }
            else
            {
                tabControl_settings.Visible = false;
                tabControl_settings.SelectedIndex = 0;
            }
            UpdateSettings();
            _internalOperation = false;
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
            if (null == selectedSeq || selectedSeq.Index == CommonConst.SetupIndex || selectedSeq.Index == CommonConst.TeardownIndex)
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

        private void SetStepProperties()
        {
            ISequenceStep selectedStep = FindSelectedStep(treeView_stepView.SelectedNode);
            if (null == selectedStep)
            {
                return;
            }
            selectedStep.Behavior = (RunBehavior) Enum.Parse(typeof (RunBehavior), comboBox_runType.Text);
            selectedStep.AssertFailedAction =
                (FailedAction) Enum.Parse(typeof (FailedAction), comboBox_asserFailedAction.Text);
            selectedStep.InvokeErrorAction =
                (FailedAction) Enum.Parse(typeof (FailedAction), comboBox_invokeFailedAction.Text);
            selectedStep.RecordStatus = checkBox_RecordStatus.Checked;
            ISequenceManager sequenceManager = _globalInfo.TestflowEntity.SequenceManager;
            switch (LoopTypecomboBox.Text)
            {
                case "None":
                    selectedStep.LoopCounter = null;
                    LoopTimesnumericUpDown.Enabled = false;
                    numericUpDown_passTimes.Enabled = false;
                    numericUpDown_retryTime.Enabled = false;
                    selectedStep.RetryCounter = null;
                    selectedStep.LoopCounter = null;
                    break;
                case "FixedTimes":
                    LoopTimesnumericUpDown.Enabled = true;
                    numericUpDown_passTimes.Enabled = false;
                    numericUpDown_retryTime.Enabled = false;
                    selectedStep.RetryCounter = null;
                    if (selectedStep.LoopCounter == null)
                    {
                        selectedStep.LoopCounter = sequenceManager.CreateLoopCounter();
                    }
                    selectedStep.LoopCounter.CounterEnabled = true;
                    selectedStep.LoopCounter.CounterEnabled = true;
                    selectedStep.LoopCounter.MaxValue = (int)LoopTimesnumericUpDown.Value;
                    break;
                case "PassTimes":
                    LoopTimesnumericUpDown.Enabled = false;
                    numericUpDown_passTimes.Enabled = true;
                    numericUpDown_retryTime.Enabled = true;
                    selectedStep.LoopCounter = null;
                    if (selectedStep.RetryCounter == null)
                    {
                        selectedStep.RetryCounter = sequenceManager.CreateRetryCounter();
                    }
                    selectedStep.RetryCounter.RetryEnabled = true;
                    selectedStep.RetryCounter.MaxRetryTimes = (int)numericUpDown_retryTime.Value;
                    selectedStep.RetryCounter.PassTimes = (int)numericUpDown_passTimes.Value;
                    break;
            }
        }
    }
}