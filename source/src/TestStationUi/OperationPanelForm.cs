using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using TestStation.Authentication;
using TestStation.Common;

namespace TestStation.OperationPanel
{
    public partial class OperationPanelForm : Form
    {
        private readonly RuntimeDataCache _dataCache;
        private EventController _eventController;
        private Color[] _uutStatusColor = {Color.White, Color.Yellow, Color.FromArgb(102, 255, 51),
            Color.Red, Color.Red, Color.Red};

        private readonly SizeAdapter _sizeAdapter;
        public OperationPanelForm(RuntimeDataCache dataCache)
        {
            InitializeComponent();
            this.BackColor = Color.FromArgb(171, 228, 255);
            this._dataCache = dataCache;
            // 更新当前时间
            timer_currentTime_Tick(null, null);
            OiReady = false;
//            // 清楚第二个Status面板的数据，后续再添加
//            Control statusControl = tableLayoutPanel_statusValue.GetControlFromPosition(1, 0);
//            statusControl?.Controls.Clear();
//            statusControl = tableLayoutPanel_statusValue.GetControlFromPosition(1, 1);
//            statusControl?.Controls.Clear();
//            this.tableLayoutPanel_statusValue.ColumnCount = 1;
            _formDisposed = 0;
            _sizeAdapter = new SizeAdapter();
        }

        public bool OiReady { get; private set; }

        public bool FormUnavaiable => _formDisposed == 1;

        private void OperationPanelForm_Load(object sender, EventArgs e)
        {
            RefreshStaticInformation();
            // 如果以Slave方式运行，则菜单和部分按钮不可用
            if (_dataCache.RunType == RunType.Slave)
            {
                menuStrip_main.Enabled = false;
                button_userSwitch.Enabled = false;
                button_stopDevice.Enabled = false;
                button_resetInfo.Enabled = false;
            }
            RegisterResizeControls();
        }

        private void RegisterResizeControls()
        {
            _sizeAdapter.RegisterControl(label_status1);
            _sizeAdapter.RegisterControl(label_statusLabel);
            _sizeAdapter.RegisterControl(label_serialNoLabel);
            _sizeAdapter.RegisterControl(label_status1Label);
            _sizeAdapter.RegisterControl(label_sn1Value);
            _sizeAdapter.RegisterControl(label_targetLabel);
            _sizeAdapter.RegisterControl(label_qualifiedLabel);
            _sizeAdapter.RegisterControl(label_unqualifiedLabel);
            _sizeAdapter.RegisterControl(label_passRatioLabel);
            _sizeAdapter.RegisterControl(label_target);
            _sizeAdapter.RegisterControl(label_qualified);
            _sizeAdapter.RegisterControl(label_unqualified);
            _sizeAdapter.RegisterControl(label_qualifiedRatio);
            _sizeAdapter.RegisterControl(label_customerLabel);
            _sizeAdapter.RegisterControl(label_modelLabel);
            _sizeAdapter.RegisterControl(label_serialNumberLabel);
            _sizeAdapter.RegisterControl(label_testVersionLabel);
            _sizeAdapter.RegisterControl(label_deviceLabel);
            _sizeAdapter.RegisterControl(label_userLabel);
            _sizeAdapter.RegisterControl(label_testTimeLabel);
            _sizeAdapter.RegisterControl(label_customer);
            _sizeAdapter.RegisterControl(label_model);
            _sizeAdapter.RegisterControl(label_productNumber);
            _sizeAdapter.RegisterControl(label_testversion);
            _sizeAdapter.RegisterControl(label_testDevice);
            _sizeAdapter.RegisterControl(label_user);
            _sizeAdapter.RegisterControl(label_testtime);
            _sizeAdapter.RegisterControl(button_setTarget);
            _sizeAdapter.RegisterControl(button_userSwitch);
            _sizeAdapter.RegisterControl(button_resetInfo);
            _sizeAdapter.RegisterControl(button_stopDevice);
        }

        private void LoadConfigurationFile()
        {
            string fileName = openFileDialog_configFile.FileName;
            try
            {
                _dataCache.InitModelInfo(_dataCache.Equipment);
                OiReady = true;
            }
            catch (ApplicationException ex)
            {
                MessageBox.Show(ex.Message, "Configure", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void button_setTarget_Click(object sender, EventArgs e)
        {
            TargetSetForm targetSetForm = new TargetSetForm(this, _dataCache.Target);
            targetSetForm.Show(this);
        }

        private void button_userSwitch_Click(object sender, EventArgs e)
        {
            _dataCache.Session = AuthenticationManage.GetLoginSession(_dataCache.Session);
        }

        private void button_resetInfo_Click(object sender, EventArgs e)
        {
            // TODO 功能只在OI独立运行时使能，待完成
            RefreshStaticInformation();
        }

        private void button_stopDevice_Click(object sender, EventArgs e)
        {
            _dataCache.TestflowEntity.EngineController.AbortRuntime(0);
        }

        private void RefreshStaticInformation()
        {
            this.label_target.Text = _dataCache.Target.ToString();
            label_customer.Text = _dataCache.Customer;
            label_model.Text = _dataCache.Model;
            label_productNumber.Text = _dataCache.ProductNumber;
            label_testversion.Text = _dataCache.TestVersion;
            label_testDevice.Text = _dataCache.Device;
            label_user.Text = _dataCache.Session.UserName;
        }

        public void RefreshProductTestInformation(UutStatus uutStatus)
        {
            if (!Constants.NASerialNo.Equals(_dataCache.SerialNumber))
            {
                label_sn1Value.Text = _dataCache.SerialNumber;
            }
            label_testtime.Text = _dataCache.ElapsedSeconds.ToString("F3");
            label_qualified.Text = _dataCache.QualifiedCount.ToString();
            label_unqualified.Text = _dataCache.UnqualifiedCount.ToString();
            label_status1.Text = uutStatus.ToString();
            label_status1.BackColor = _uutStatusColor[(int) uutStatus];
            int totalCount = (_dataCache.UnqualifiedCount + _dataCache.QualifiedCount);
            if (totalCount > 0)
            {
                label_qualifiedRatio.Text = (_dataCache.QualifiedCount*100 / totalCount).ToString("F1");
            }
            progressBar_progress.Value = (int) (progressBar_progress.Maximum*_dataCache.UutProgress);
        }

        public void SetTarget(long target)
        {
            if (target == _dataCache.Target)
            {
                return;
            }
            _dataCache.Target = target;
            label_target.Text = target.ToString();
            try
            {
                ConfigManager configManager = new ConfigManager();
                configManager.ApplyConfig("Target", target.ToString());
                configManager.WriteConfigData();
            }
            catch (ApplicationException ex)
            {
                Log.Print(LogLevel.WARN, $"Write target failed: {ex.Message}");
            }
        }

        public void Initialize()
        {
            _eventController = new EventController(this, _dataCache);
            _eventController.Initialize();
        }

        public void Start()
        {
            SetViewControllerState(RunState.Running);
            ResetOperationPanel();
            // 如果是Slave方式运行则返回
            if (_dataCache.RunType == RunType.Slave)
            {
                return;
            }
        }

        private void ResetOperationPanel()
        {
            label_status1.Text = UutStatus.Waitting.ToString();
            label_status1.BackColor = _uutStatusColor[(int) UutStatus.Waitting];
        }

        public void Abort()
        {
            // ignore
        }

        public void RefreshCommonRunningInformation(UutStatus uutState)
        {
            label_testtime.Text = _dataCache.ElapsedSeconds.ToString("F3");
            progressBar_progress.Value = (int)(progressBar_progress.Maximum * _dataCache.UutProgress);
            if (!label_status1.Text.Equals(uutState.ToString()))
            {
                label_status1.Text = uutState.ToString();
                label_status1.BackColor = _uutStatusColor[(int)uutState];
            }
        }

        public void ShowUutStatus(UutStatus status)
        {
            // ignore nothing to do
            label_status1.Text = status.ToString();
            label_status1.BackColor = _uutStatusColor[(int)status];
        }

        #region 事件更新

        public void SetViewControllerState(RunState state)
        {
            this.viewController_main.State = state.ToString();
            if (state == RunState.RunOver)
            {
                string currentStatus = label_status1.Text;
                if (currentStatus.Equals(UutStatus.Running.ToString()) ||
                    currentStatus.Equals(UutStatus.Waitting.ToString()))
                {
                    label_status1.Text = UutStatus.Pass.ToString();
                    label_status1.BackColor = _uutStatusColor[(int)UutStatus.Pass];
                }
            }
        }

        private void timer_currentTime_Tick(object sender, EventArgs e)
        {
            this.label_time.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        #endregion

        private void viewController_main_PostListeners(int arg1, int arg2)
        {
            // 如果以Slave方式运行，则菜单和部分按钮不可用
            if (_dataCache.RunType == RunType.Slave)
            {
                menuStrip_main.Enabled = false;
                button_userSwitch.Enabled = false;
                button_stopDevice.Enabled = false;
            }
            toolStripStatusLabel_state.Text = viewController_main.State;
        }

        private int _formDisposed;
        private void OperationPanelForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Thread.VolatileWrite(ref _formDisposed, 1);
            if (null == _eventController)
            {
                return;
            }
            _eventController.Dispose();
            Thread.MemoryBarrier();
            _eventController = null;
            Application.DoEvents();
            this.Dispose();
        }

        private void OperationPanelForm_Resize(object sender, EventArgs e)
        {
            _sizeAdapter.Resize();
        }
    }
}
