using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Testflow.Data;
using Testflow.Data.Sequence;
using Testflow.Modules;
using Testflow.Runtime;
using Testflow.Runtime.Data;
using Testflow.Usr;
using TestFlow.DevSoftware.Common;
using TestFlow.SoftDevCommon;

namespace TestFlow.DevSoftware.Runtime
{
    public partial class RuntimeStatusForm : Form
    {
        private GlobalInfo _globalInfo;

        private DateTime[] _startTimes;
        private MainForm _mainForm;

        public RuntimeStatusForm(MainForm parentForm)
        {
            InitializeComponent();
            this._mainForm = parentForm;
            this.TopLevel = false;
            _globalInfo = GlobalInfo.GetInstance();
        }

        const int ColumnCount = 8;
        const int SessionCol = 0;
        const int SequenceCol = 1;
        const int SequenceName = 2;
        const int StatusCol = 3;
        const int StartTimeCol = 4;
        const int EndTimeCol = 5;
        const int ElapsedTimeCol = 6;
        const int ResultCol = 7;

        const int SetUpRow = 0;
        const int SequenceRowOffset = 1;

        public void RegisterEvent()
        {
            IEngineController engineController = _globalInfo.TestflowEntity.EngineController;
            engineController.RegisterRuntimeEvent(new RuntimeDelegate.TestGenerationAction(TestGenStart), "TestGenerationStart",
                0);
            engineController.RegisterRuntimeEvent(new RuntimeDelegate.TestGenerationAction(TestGenOver), "TestGenerationEnd", 0);


            engineController.RegisterRuntimeEvent(new RuntimeDelegate.SequenceStatusAction(SequenceStarted), "SequenceStarted",
                0);
            engineController.RegisterRuntimeEvent(new RuntimeDelegate.SessionStatusAction(SessionOver), "SessionOver", 0);
            engineController.RegisterRuntimeEvent(new RuntimeDelegate.StatusReceivedAction(StatusReceived), "StatusReceived", 0);
            engineController.RegisterRuntimeEvent(new RuntimeDelegate.SequenceStatusAction(SequenceOver), "SequenceOver", 0);
            engineController.RegisterRuntimeEvent(new RuntimeDelegate.TestInstanceStatusAction(TestInstanceOver),
                "TestInstanceOver", 0);
        }


        public void TestGenStart(ITestGenerationInfo generationInfo)
        {
            Invoke(new Action(() =>
            {
                for (int i = 0; i < dataGridView_status.RowCount; i++)
                {
                    dataGridView_status.Rows[i].Cells[StatusCol].Value = RuntimeState.TestGen.ToString();
                }
            }));
        }

        public void TestGenOver(ITestGenerationInfo generationInfo)
        {
            Invoke(new Action(() =>
            {
                string status = RuntimeState.StartIdle.ToString();
                if (generationInfo.GenerationInfos[0].Status != GenerationStatus.Success)
                {
                    status = generationInfo.GenerationInfos[0].Status.ToString();
                    _mainForm.PrintInfo("Test Generation Failed.");
                    _mainForm.RunningOver();
                }
                for (int i = 0; i < dataGridView_status.RowCount; i++)
                {
                    dataGridView_status.Rows[i].Cells[StatusCol].Value = status;
                }
            }));
        }

        private void SessionOver(ITestResultCollection statistics)
        {
            PrintWatchData(statistics.WatchData);
            string runtimeHash = _globalInfo.TestflowEntity.EngineController.GetRuntimeInfo<string>("RuntimeHash");
            string reportPath = GetReportPath();
            _globalInfo.TestflowEntity.ResultManager.PrintReport(reportPath, runtimeHash, ReportType.txt);
            Invoke(new Action(() =>
            {
                ReportForm reportForm = new ReportForm(reportPath);
                reportForm.ShowDialog(this);
            }));
        }

        private void PrintWatchData(IDictionary<IVariable, string> rawWatchDatta)
        {
//            if (null != rawWatchDatta && rawWatchDatta.Count > 0)
//            {
//                Dictionary<string, string> watchData = new Dictionary<string, string>(rawWatchDatta.Count);
//                foreach (KeyValuePair<IVariable, string> keyValuePair in rawWatchDatta)
//                {
//                    watchData.Add(keyValuePair.Key.Value, keyValuePair.Value);
//                }
//                Invoke(new Action(() => { _mainForm.UpdateVariableValues(watchData); }));
//            }
        }

        private string GetReportPath()
        {
            string workspaceDir = _globalInfo.TestflowEntity.ConfigurationManager.ConfigData.GetProperty<string[]>("WorkspaceDir")[0];
            string testName = _globalInfo.TestflowEntity.EngineController.GetRuntimeInfo<string>("TestName");
            string filePath = $"{workspaceDir}{testName}.txt";
            int index = 1;
            while (File.Exists(filePath))
            {
                filePath = $"{workspaceDir}{testName}({index++}).txt";
            }
            return filePath;
        }

        private void SequenceStarted(ISequenceTestResult statistics)
        {
            Invoke(new Action(() =>
            {
                int rowIndex = GetRowIndex(statistics.SequenceIndex);
                dataGridView_status.Rows[rowIndex].Cells[StatusCol].Value = statistics.ResultState.ToString();
                dataGridView_status.Rows[rowIndex].Cells[StartTimeCol].Value =
                    statistics.StartTime.ToString("HH:mm:ss.fff");
                _startTimes[rowIndex] = statistics.StartTime;
            }));
        }

        private void StatusReceived(IRuntimeStatusInfo statusinfo)
        {
            Invoke(new Action(() =>
            {
                for (int i = 0; i < dataGridView_status.RowCount; i++)
                {
                    if (DateTime.MaxValue == _startTimes[i])
                    {
                        continue;
                    }
                    dataGridView_status.Rows[i].Cells[ElapsedTimeCol].Value =
                        ((statusinfo.CurrentTime - _startTimes[i]).TotalMilliseconds / 1000).ToString("F3");
                }

                PrintWatchData(statusinfo.WatchDatas);
                if (null != statusinfo.FailedInfos)
                {
                    foreach (KeyValuePair<int, IFailedInfo> keyValuePair in statusinfo.FailedInfos)
                    {
                        _mainForm.PrintInfo($"Sequence {keyValuePair.Key} failed: {keyValuePair.Value.Message}");
                    }
                }
            }));
        }

        private void SequenceOver(ISequenceTestResult statistics)
        {
            PrintWatchData(statistics.VariableValues);
            Invoke(new Action(() =>
            {
                int rowIndex = GetRowIndex(statistics.SequenceIndex);
                dataGridView_status.Rows[rowIndex].Cells[StatusCol].Value = statistics.ResultState;
                string result = statistics.ResultState <= RuntimeState.Success ? "Success" : "Failed";
                dataGridView_status.Rows[rowIndex].Cells[ResultCol].Value = result;
                dataGridView_status.Rows[rowIndex].Cells[EndTimeCol].Value = statistics.EndTime.ToString("HH:mm:ss.fff");
                if (statistics.ResultState > RuntimeState.Success && null != statistics.FailedInfo)
                {
                    _mainForm.PrintInfo($"Sequence <{statistics.SequenceIndex}> failed: {statistics.FailedInfo.Message}");
                }
                else
                {
                    _mainForm.PrintInfo($"Sequence <{statistics.SequenceIndex}> over.");
                }
                _startTimes[rowIndex] = DateTime.MaxValue;
            }));
        }

        private void TestInstanceOver(IList<ITestResultCollection> statistics)
        {
            Invoke(new Action(() =>
            {
                _globalInfo.RunState = RunState.RunIdle;
                _mainForm.RunningOver();
            }));
        }

        public void LoadSequence(ISequenceGroup sequenceData)
        {
            dataGridView_status.Rows.Clear();
            ISequenceGroup sequenceGroup = sequenceData;

            label_nameValue.Text = sequenceGroup.Name;
            textBox_testInstanceName.Text = label_nameValue.Text;

            string[] columnData = new string[ColumnCount];
            LoadSequenceData(sequenceGroup.SetUp, columnData);
            foreach (ISequence sequence in sequenceGroup.Sequences)
            {
                LoadSequenceData(sequence, columnData);
            }
            LoadSequenceData(sequenceGroup.TearDown, columnData);
            _startTimes = new DateTime[dataGridView_status.RowCount];
            for (int i = 0; i < _startTimes.Length; i++)
            {
                _startTimes[i] = DateTime.MaxValue;
            }
        }

        private void LoadSequenceData(ISequence sequence, string[] values)
        {
            values[SessionCol] = "0";
            values[SequenceCol] = sequence.Index.ToString();
            values[SequenceName] = sequence.Name;
            values[StatusCol] = RuntimeState.Idle.ToString();
            values[StartTimeCol] = "";
            values[EndTimeCol] = "";
            values[ElapsedTimeCol] = "0";
            values[ResultCol] = StepResult.NotAvailable.ToString();
            dataGridView_status.Rows.Add(values);
        }

        private int GetRowIndex(int sequenceIndex)
        {
            int rowIndex;
            switch (sequenceIndex)
            {
                case CommonConst.SetupIndex:
                    rowIndex = SetUpRow;
                    break;
                case CommonConst.TeardownIndex:
                    rowIndex = dataGridView_status.RowCount - 1;
                    break;
                default:
                    rowIndex = sequenceIndex + SequenceRowOffset;
                    break;
            }
            return rowIndex;
        }

    }
}
