using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestFlow.DevSoftware.Common;
using TestFlow.SoftDevCommon;

namespace TestFlow.DevSoftware
{
    public partial class ConfigureForm : Form
    {
        private GlobalInfo _globalInfo;
        private string[] _nameElement;
        const string DefaultPath = "<Sequence File Directory>";

        public ConfigureForm(GlobalInfo globalInfo)
        {
            InitializeComponent();
            _globalInfo = globalInfo;
        }

        private void ConfigureForm_Load(object sender, EventArgs e)
        {
            _nameElement = new string[]
            {
                "SequenceName",
                "BaseName",
                "SerialNumber",
                "DUTIndex",
                "Date",
                "Time",
                "Postfix",
                "UUTResult"
            };
            listBox_nameElement.Items.AddRange(_nameElement);

            ConfigManager configManager = _globalInfo.ConfigManager;
            this.comboBox_actionBreakIfFailed.Text = configManager.GetConfig<bool>("ActionBreakIfFailed").ToString();
            this.comboBox_actionRecordResult.Text = configManager.GetConfig<bool>("ActionRecordStatus").ToString();

            this.comboBox_testBreakIfFailed.Text = configManager.GetConfig<bool>("TestBreakIfFailed").ToString();
            this.comboBox_testRecordResult.Text = configManager.GetConfig<bool>("TestRecordStatus").ToString();

            this.comboBox_seqCallBreakIfFailed.Text = configManager.GetConfig<bool>("SeqCallBreakIfFailed").ToString();
            this.comboBox_seqCallRecordResult.Text = configManager.GetConfig<bool>("SeqCallRecordStatus").ToString();

            this.textBox_reportNameFormat.Text = configManager.GetConfig<string>("ReportNameFormat");
            this.textBox_baseName.Text = configManager.GetConfig<string>("BaseName");

            string reportPath = configManager.GetConfig<string>("ReportPath");
            bool useDefaultPath = configManager.GetConfig<bool>("UseDefaultReportPath");
            comboBox_reportPath.Items.Add(DefaultPath);
            if (!string.IsNullOrWhiteSpace(reportPath) && Directory.Exists(reportPath))
            {
                comboBox_reportPath.Items.Add(reportPath);
            }
            comboBox_reportPath.Text = useDefaultPath ? DefaultPath : reportPath;
        }

        private void button_apply_Click(object sender, EventArgs e)
        {
            ConfigManager configManager = _globalInfo.ConfigManager;
            configManager.ApplyConfig("ActionBreakIfFailed", comboBox_actionBreakIfFailed.Text);
            configManager.ApplyConfig("ActionRecordStatus", comboBox_actionRecordResult.Text);
            configManager.ApplyConfig("TestBreakIfFailed", comboBox_testBreakIfFailed.Text);
            configManager.ApplyConfig("TestRecordStatus", comboBox_testRecordResult.Text);
            configManager.ApplyConfig("SeqCallBreakIfFailed", comboBox_seqCallBreakIfFailed.Text);
            configManager.ApplyConfig("SeqCallRecordStatus", comboBox_seqCallRecordResult.Text);
            configManager.ApplyConfig("ReportNameFormat", textBox_reportNameFormat.Text);
            configManager.ApplyConfig("ReportNameFormat", textBox_reportNameFormat.Text);

            string reportPath = comboBox_reportPath.Text;
            if (string.IsNullOrWhiteSpace(reportPath) || reportPath.Equals(DefaultPath) || !Directory.Exists(reportPath))
            {
                configManager.ApplyConfig("ReportPath", string.Empty);
                configManager.ApplyConfig("UseDefaultReportPath", true.ToString());
            }
            else
            {
                configManager.ApplyConfig("ReportPath", reportPath);
                configManager.ApplyConfig("UseDefaultReportPath", false.ToString());
            }

            configManager.ApplyConfig("BaseName", textBox_baseName.Text);
            try
            {
                configManager.WriteConfigData();
                this.Close();
            }
            catch (ApplicationException ex)
            {
                MessageBox.Show(ex.Message, "Configure", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button_selectReportPath_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = this.folderBrowserDialog_reportPath.ShowDialog(this);
            if (dialogResult != DialogResult.OK)
            {
                return;
            }
            comboBox_reportPath.Text = this.folderBrowserDialog_reportPath.SelectedPath;
        }

        private void textBox_reportNameFormat_TextChanged(object sender, EventArgs e)
        {
            listBox_nameElement.Invalidate();
            textbox_reportNamePreview.Text = GetPreviewData();
        }

        const string ElemFormat = "{{{0}}}";
        const char DefaultDelim = '_';
        const string ExtensionDelim = ".";
        private void button_addNameElem_Click(object sender, EventArgs e)
        {
            if (listBox_nameElement.SelectedIndex < 0)
            {
                return;
            }
            string selectedItem = _nameElement[listBox_nameElement.SelectedIndex];
            string currentFormat = textBox_reportNameFormat.Text;
            currentFormat = AddElement(currentFormat, selectedItem);
            textBox_reportNameFormat.Text = currentFormat;
        }


        private void listBox_nameElement_DoubleClick(object sender, EventArgs e)
        {
            button_addNameElem_Click(null, null);
        }


        private void button_removeNameElem_Click(object sender, EventArgs e)
        {
            if (listBox_nameElement.SelectedIndex < 0)
            {
                return;
            }
            string selectedItem = _nameElement[listBox_nameElement.SelectedIndex];
            string currentFormat = textBox_reportNameFormat.Text;
            textBox_reportNameFormat.Text = RemoveElement(currentFormat, selectedItem);
        }

        private string AddElement(string currentFormat, string element)
        {
            string elementName = string.Format(ElemFormat, element);
            if (currentFormat.Contains(elementName))
            {
                return currentFormat;
            }
            int nameIndex = 0;
            while (nameIndex < _nameElement.Length && !_nameElement[nameIndex].Equals(element))
            {
                nameIndex++;
            };
            if (nameIndex >= _nameElement.Length)
            {
                return currentFormat;
            }
            int insertIndex = -1;
            for (int i = 0; i < nameIndex; i++)
            {
                string nameFormat = string.Format(ElemFormat, _nameElement[i]);
                if (currentFormat.Contains(nameFormat))
                {
                    insertIndex = currentFormat.IndexOf(nameFormat) + nameFormat.Length;
                }
            }
            
            if (insertIndex <= 0)
            {
                if (currentFormat.Length <= 0 || currentFormat[0].Equals(ExtensionDelim[0]))
                {
                    currentFormat = elementName + currentFormat;
                }
                else
                {
                    currentFormat = elementName + DefaultDelim + currentFormat;
                }
            }
            else if (insertIndex >= currentFormat.Length)
            {
                if (currentFormat.Length <= 0)
                {
                    currentFormat = elementName;
                }
                else
                {
                    currentFormat = currentFormat + DefaultDelim + elementName;
                }
            }
            else
            {
                currentFormat = currentFormat.Insert(insertIndex, DefaultDelim + elementName);
            }
            return currentFormat;
        }

        private string RemoveElement(string currentFormat, string element)
        {
            element = string.Format(ElemFormat, element);
            if (!currentFormat.Contains(element))
            {
                return currentFormat;
            }
            int index = currentFormat.IndexOf(element, StringComparison.Ordinal);
            int removeLength = element.Length;
            if (index > 0 && currentFormat[index - 1] == DefaultDelim)
            {
                removeLength++;
                index--;
            }
            else if (index + removeLength + 1 < currentFormat.Length && currentFormat[index + removeLength] == DefaultDelim)
            {
                removeLength++;
            }
            currentFormat = currentFormat.Substring(0, index) +
                   currentFormat.Substring(index + removeLength, currentFormat.Length - (index + removeLength));
            // 替换
            while (currentFormat.Contains("__"))
            {
                currentFormat = currentFormat.Replace("__", "_");
            }
            return currentFormat;
        }

        private string GetPreviewData()
        {
            string format = textBox_reportNameFormat.Text;
            StringBuilder previewData = new StringBuilder(format);
            previewData.Replace("{SequenceName}", "BM3035Test");
            previewData.Replace("{BaseName}", textBox_baseName.Text);
            previewData.Replace("{SerialNumber}", "[EGB46234]");
            previewData.Replace("{DUTIndex}", "[1]");
            previewData.Replace("{Date}", "20191215");
            previewData.Replace("{Time}", "142547");
            previewData.Replace("{Postfix}", "00001");
            previewData.Replace("{UUTResult}", "Pass");
            return previewData.ToString();
        }

        private void listBox_nameElement_DrawItem(object sender, DrawItemEventArgs e)
        {
            int index = e.Index;
            if (index < 0 || index > listBox_nameElement.Items.Count) return;
            e.DrawBackground();
            string reportNameFormat = textBox_reportNameFormat.Text;
            Brush mybsh = Brushes.Black;
            string formatName = string.Format(ElemFormat, _nameElement[index]);
            if (reportNameFormat.Contains(formatName))
            {
                mybsh = Brushes.DimGray;
            }
            // 焦点框
            e.DrawFocusRectangle();
            //文本 
            e.Graphics.DrawString(listBox_nameElement.Items[index].ToString(), e.Font, mybsh, e.Bounds, StringFormat.GenericDefault);
        }
    }
}