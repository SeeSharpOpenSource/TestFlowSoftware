using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JY9702ProductTest
{
    public partial class JY9702ProductTestPanel : Form
    {
        public JY9702ProductTestPanel()
        {
            InitializeComponent();
        }

        public event Action<bool, Dictionary<string, object>> StartSequence;

        public void ShowMessage(string message)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    this.textBox1.AppendText(message);
                    this.textBox1.AppendText(Environment.NewLine);
                }));
            }
            else
            {
                this.textBox1.AppendText(message);
                this.textBox1.AppendText(Environment.NewLine);
            }
        }

        public void ShowError(bool canStillRun, string message)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    MessageBox.Show(message, "Production Test", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }));
            }
            else
            {
                MessageBox.Show(message, "Production Test", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void SetStatus(bool isRunning, bool isError, bool isFailed)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    this.led1.BlinkOn = isRunning;
                    if (isRunning)
                    {
                        this.label1.Text = "Running";
                        this.label1.BackColor = Color.Orange;
                    }
                    else if (isError)
                    {
                        this.label1.Text = "Error";
                        this.label1.BackColor = Color.Red;
                    }
                    else if (isFailed)
                    {
                        this.label1.Text = "Failed";
                        this.label1.BackColor = Color.Red;
                    }
                    else
                    {
                        this.label1.Text = "Passed";
                        this.label1.BackColor = Color.Green;
                    }
                }));
            }
            else
            {
                this.led1.BlinkOn = isRunning;
                if (isError)
                {
                    this.label1.Text = "Error";
                    this.label1.BackColor = Color.Red;
                }
                else if (isFailed)
                {
                    this.label1.Text = "Failed";
                    this.label1.BackColor = Color.Red;
                }
                else
                {
                    this.label1.Text = "Passed";
                    this.label1.BackColor = Color.Green;
                }
            }
        }

        private void button_start_Click(object sender, EventArgs e)
        {
            Dictionary<string, object> data = new Dictionary<string, object>(10);
            data.Add("SlotNumber", this.comboBox1.SelectedIndex);
            StartSequence?.Invoke(true, data);
        }

        private void button_stop_Click(object sender, EventArgs e)
        {

        }

        private void JY9702ProductTestPanel_FormClosing(object sender, FormClosingEventArgs e)
        {
            StartSequence?.Invoke(false, null);
        }
    }
}
