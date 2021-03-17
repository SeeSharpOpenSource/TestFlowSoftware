using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Testflow;
using Testflow.Data.Sequence;
using Testflow.External.RunnerInvoker;
using Testflow.Runtime;
using Testflow.Usr;

namespace TestFlowExecutor
{
    public partial class MainForm : Form
    {
        private TestflowRunner _testflowRunner;

        public MainForm()
        {
            InitializeComponent();
            try
            {
                TestflowRunnerOptions runnerOptions = new TestflowRunnerOptions();
                this._testflowRunner = TestFlowRunnerInvoker.CreateInstance(runnerOptions);
                this._testflowRunner.Initialize();
                this._testflowRunner.RuntimeInitialize();
                _testflowRunner.EngineController.ExceptionRaised += EngineControllerOnExceptionRaised;
                _testflowRunner.DesigntimeInitialize();
            }
            catch (Exception ex)
            {
                AppendErrorInfo(ex);
            }
        }

        private void button_run_Click(object sender, EventArgs e)
        {
            string sequenceFile = this.textBox_assemblyPath.Text;
            if (!File.Exists(sequenceFile))
            {
                MessageBox.Show("Sequence file not found.", "Run Sequence", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int dirLength = sequenceFile.LastIndexOf(Path.DirectorySeparatorChar) + 1;
            string directory = sequenceFile.Substring(0, dirLength);
            
            try
            {
                ISequenceGroup sequenceGroup = _testflowRunner.SequenceManager.LoadSequenceGroup(SerializationTarget.File, sequenceFile);
                _testflowRunner.ComInterfaceManager.GetComponentInterfaces(sequenceGroup.Assemblies);
                _testflowRunner.RuntimeInitialize();
                _testflowRunner.RuntimeService.Initialize();
                _testflowRunner.RuntimeService.Load(sequenceGroup);
                this._testflowRunner.EngineController.RegisterRuntimeEvent(
                    new RuntimeDelegate.SessionStatusAction(FrmMain_SessionStart), "SessionStart", 0);
                this._testflowRunner.EngineController.RegisterRuntimeEvent(
                    new RuntimeDelegate.SessionStatusAction(FrmMain_SessionOver), "SessionOver", 0);
                                ThreadPool.QueueUserWorkItem((state) =>
                {
                    _testflowRunner.EngineController.Start();
                });
                this.button_run.Enabled = false;
            }
            catch (Exception ex)
            {
                AppendErrorInfo(ex);
            }
        }

        private void EngineControllerOnExceptionRaised(Exception ex)
        {
            Invoke(new Action(() => {
                AppendErrorInfo(ex);
                this.button_run.Enabled = true;
            }));
        }

        private void FrmMain_SessionStart(Testflow.Runtime.Data.ITestResultCollection statistics)
        {
            Invoke(new Action(() => {
                string showInfo = "Sequence Start...";
                this.textBox_output.AppendText(showInfo);
                this.textBox_output.AppendText(Environment.NewLine);
            }));
        }

        private void FrmMain_SessionOver(Testflow.Runtime.Data.ITestResultCollection statistics)
        {
            Invoke(new Action(() => {
                string showInfo = "Sequence over.";
                if (statistics.Any(item => item.Value.ResultState != RuntimeState.Success))
                {
                    showInfo = "Sequence Failed.";
                }
                this.textBox_output.AppendText(showInfo);
                this.textBox_output.AppendText(Environment.NewLine);
                this.button_run.Enabled = true;
            }));
        }

        private void AppendErrorInfo(Exception ex)
        {
            this.textBox_output.AppendText(ex.Message);
            this.textBox_output.AppendText(Environment.NewLine);
            this.textBox_output.Focus(); //获取焦点
            this.textBox_output.Select(this.textBox_output.TextLength, 0); //光标定位到文本最后
            this.textBox_output.ScrollToCaret(); //滚动到光标处
        }

        private void button_assemblyFileSelect_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = this.openFileDialog_panelSelector.ShowDialog(this);
            if (dialogResult == DialogResult.OK)
            {
                this.textBox_assemblyPath.Text = this.openFileDialog_panelSelector.FileName;
            }
        }
    }
}
