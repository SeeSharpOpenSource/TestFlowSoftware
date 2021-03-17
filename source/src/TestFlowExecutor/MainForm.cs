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
                this._testflowRunner.DesigntimeInitialize();
                this._testflowRunner.RuntimeInitialize();
                _testflowRunner.EngineController.ExceptionRaised += EngineControllerOnExceptionRaised;
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
                _testflowRunner.RuntimeService.Initialize();
                ISequenceGroup sequenceGroup = _testflowRunner.SequenceManager.LoadSequenceGroup(SerializationTarget.File, sequenceFile);
                _testflowRunner.RuntimeService.Load(sequenceGroup);
                ThreadPool.QueueUserWorkItem((state) =>
                {
                    _testflowRunner.EngineController.Start();
                });
            }
            catch (Exception ex)
            {
                AppendErrorInfo(ex);
            }
        }

        private void EngineControllerOnExceptionRaised(Exception ex)
        {
            Invoke(new Action(() => { AppendErrorInfo(ex); }));
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
