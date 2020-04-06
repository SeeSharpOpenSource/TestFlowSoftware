using System;
using System.IO;
using System.Windows.Forms;

namespace TestStation.Runtime
{
    public partial class ReportForm : Form
    {
        private string _reportPath;

        public ReportForm(string reportPath)
        {
            InitializeComponent();
            _reportPath = reportPath;
        }

        private void button_openReportDIr_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("Explorer.exe", "/select," + _reportPath);
        }

        private void ReportForm_Load(object sender, EventArgs e)
        {
            label_reportPath.Text = _reportPath;
            StreamReader reader = null;
            try
            {
                reader = new StreamReader(_reportPath);
                string lineData;
                while (null != (lineData = reader.ReadLine()))
                {
                    textBox_reportValue.AppendText(lineData);
                    textBox_reportValue.AppendText(Environment.NewLine);
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.Message, "读取报表失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                reader?.Dispose();
            }
        }
    }
}
