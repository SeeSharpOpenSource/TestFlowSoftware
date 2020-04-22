using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Testflow.Data.Sequence;
using Testflow.Runtime;
using Testflow.Usr;

namespace TestFlow.Software.WinformCommonOi.ValueInputOi
{
    public partial class RuntimeStatusForm : Form
    {
        private ISequenceGroup _sequenceGroup;
        public RuntimeStatusForm(ISequenceGroup sequenceData)
        {
            InitializeComponent();
            this._sequenceGroup = sequenceData;
        }

        const int StarTimeColIndex = 1;
        const int EndTimeColIndex = 2;
        const int StatusColIndex = 3;

        public void PrintInformation(string information)
        {
            this.Invoke(new Action(() =>
            {
                textBox_status.AppendText(information);
                textBox_status.AppendText(Environment.NewLine);
            }));
        }

        public void ShowStatus(RuntimeState state)
        {
            this.Invoke(new Action(() =>
            {
                label_statusValue.Text = state.ToString();
                if (state <= RuntimeState.TestGen)
                {
                    led_result.Value = false;
                }
                else if (state <= RuntimeState.AbortRequested)
                {
                    led_result.BlinkOn = true;
                }
                else if (state >= RuntimeState.Success)
                {
                    led_result.BlinkOn = false;
                    led_result.Value = true;
                    if (state != RuntimeState.Success && state != RuntimeState.Over)
                    {
                        led_result.OnColor = Color.Red;
                    }
                }
            }));
        }

        public void ShowSequenceState(int sequenceIndex, RuntimeState state)
        {
            int rowIndex = GetSequenceRowIndex(sequenceIndex);
            this.Invoke(new Action(() => dataGridView_sequenceView.Rows[rowIndex].Cells[StatusColIndex].Value = state));
            ;
        }

        private int GetSequenceRowIndex(int sequenceIndex)
        {
            int rowIndex;
            if (sequenceIndex == CommonConst.SetupIndex && sequenceIndex != CommonConst.TeardownIndex)
            {
                rowIndex = sequenceIndex + 1;
            }
            else
            {
                rowIndex = sequenceIndex == CommonConst.SetupIndex ? 0 : dataGridView_sequenceView.RowCount - 1;
            }
            return rowIndex;
        }

        public void ShowStartTime(int sequenceIndex, string value)
        {
            int rowIndex = GetSequenceRowIndex(sequenceIndex);
            this.Invoke(new Action(() => dataGridView_sequenceView.Rows[rowIndex].Cells[StarTimeColIndex].Value = value));
        }

        public void ShowEndTime(int sequenceIndex, string value)
        {
            int rowIndex = GetSequenceRowIndex(sequenceIndex);
            this.Invoke(new Action(() => dataGridView_sequenceView.Rows[rowIndex].Cells[EndTimeColIndex].Value = value));
        }

        public void ShowAllSequenceState(RuntimeState state)
        {
            Invoke(new Action(() =>
            {
                foreach (DataGridViewRow row in dataGridView_sequenceView.Rows)
                {
                    row.Cells[StatusColIndex].Value = state;
                }
            }));
        }

        private void RuntimeStatusForm_Load(object sender, EventArgs e)
        {
            label_sequenceName.Text = _sequenceGroup.Name;
            DataGridViewRowCollection rowCollection = dataGridView_sequenceView.Rows;
            rowCollection.Add(_sequenceGroup.SetUp.Name, string.Empty, string.Empty, RuntimeState.Idle);
            foreach (ISequence sequence in _sequenceGroup.Sequences)
            {
                rowCollection.Add(sequence.Name, string.Empty, string.Empty, RuntimeState.Idle);
            }
            rowCollection.Add(_sequenceGroup.TearDown.Name, string.Empty, string.Empty, RuntimeState.Idle);
        }
    }
}
