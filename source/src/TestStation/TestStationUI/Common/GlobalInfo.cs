using System;
using System.Threading;
using Testflow;
using Testflow.Loader;
using TestStation.Authentication;

namespace TestStation.Common
{
    public class GlobalInfo
    {
        private static GlobalInfo _inst;
        private static object _instLock = new object();

        public static GlobalInfo GetInstance()
        {
            if (null != _inst)
            {
                return _inst;
            }
            Thread.MemoryBarrier();
            lock (_instLock)
            {
                if (null != _inst)
                {
                    return _inst;
                }
                _inst = new GlobalInfo();
                return _inst;
            }
        }

        public EquipmentData Equipment { get; set; }

        public event Action<RunState> StateChanged;

        public AuthenticationSession Session { get; set; }

        public TestflowRunner TestflowEntity { get; }

        public bool BreakIfFailed { get; set; }

        public string BarCodeVarName => "barCode";

        public ConfigManager ConfigManager { get; }

        private int _runState;
        public RunState RunState
        {
            get { return (RunState)_runState; }
            set
            {
                if ((int)value == _runState)
                {
                    return;
                }
                Thread.VolatileWrite(ref _runState, (int)value);
                OnStateChanged();
            }
        }

        private GlobalInfo()
        {
            TestflowRunnerOptions options = new TestflowRunnerOptions();
            TestflowRunner runner = TestflowActivator.CreateRunner(options);
            TestflowEntity = runner;
            TestflowEntity.Initialize();
            TestflowEntity.DesigntimeInitialize();
            RunState = RunState.NotAvailable;
            this.ConfigManager = new ConfigManager();
            this.ConfigManager.LoadConfigData();
            this.BreakIfFailed = true;
        }
        

        private void OnStateChanged()
        {
            CheckSession();
            StateChanged?.Invoke(RunState);
        }

        private void CheckSession()
        {
            RunState runState = RunState;
            if (null == Session || runState == RunState.NotAvailable)
            {
                return;
            }
            switch (runState)
            {
                case RunState.NotAvailable:
                    break;
                case RunState.EditIdle:
                    break;
                case RunState.EditProcess:
                    Session.CheckAuthority(AuthorityDefinition.EditSequence);
                    break;
                case RunState.RunIdle:
                case RunState.RunProcessing:
                case RunState.Running:
                case RunState.RunOver:
                    Session.CheckAuthority(AuthorityDefinition.RunSequence);
                    break;
                case RunState.RunBlock:
                    Session.CheckAuthority(AuthorityDefinition.DebugSequence);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}