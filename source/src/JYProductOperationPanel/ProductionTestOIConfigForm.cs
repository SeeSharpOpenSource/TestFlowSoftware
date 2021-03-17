using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Testflow.Data.Sequence;
using Testflow.Runtime.OperationPanel;

namespace JYProductOperationPanel
{
    public partial class ProductionTestOIConfigForm : Form
    {
        private string _assemblyPath;
        private string _panelClassName;
        public bool IsConfirmed { get; private set; }

        public ProductionTestOIConfigForm(string assemblyPath, string panelClassName, string messageVariableName, string[] variableNames)
        {
            this.IsConfirmed = false;
            InitializeComponent();
            this._assemblyPath = assemblyPath;
            this._panelClassName = panelClassName;

            this.comboBox_variables.Items.Clear();
            this.comboBox_variables.Items.Add(string.Empty);
            this.comboBox_variables.Items.AddRange(variableNames);
            if (variableNames.Any(item => item.Equals(messageVariableName)))
            {
                this.comboBox_variables.Text = messageVariableName;
            }
            else
            {
                this.comboBox_variables.Text = string.Empty;
            }
            this.textBox_assemblyPath.Text = assemblyPath;
            this.openFileDialog_panelSelector.FileName = assemblyPath;
        }

        private void ShowCurrentParam()
        {
            this.comboBox_classes.Items.Clear();
            this.comboBox_classes.Items.Add(string.Empty);
            this.comboBox_classes.SelectedIndex = 0;
            try
            {
                _assemblyPath = this.textBox_assemblyPath.Text;
                Assembly assembly = Assembly.LoadFrom(this._assemblyPath);
                IEnumerable<Type> exportedTypes = assembly.ExportedTypes;
                List<string> panelClasses = new List<string>(10);
                foreach (Type exportedType in exportedTypes)
                {
                    if (IsMethodExist(exportedType, "ShowMessage", typeof(string)) &&
                        IsMethodExist(exportedType, "ShowError", typeof(bool), typeof(string)) &&
                        IsMethodExist(exportedType, "SetStatus", typeof(bool), typeof(bool), typeof(bool)) &&
                        IsEventExist(exportedType, "StartSequence"))
                    {
                        panelClasses.Add($"{exportedType.Namespace}.{exportedType.Name}");
                    }
                }
                this.comboBox_classes.Items.AddRange(panelClasses.ToArray());
                if (panelClasses.Contains(this._panelClassName))
                {
                    this.comboBox_classes.Text = this._panelClassName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "OI Configuration", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool IsMethodExist(Type type, string methodName, params Type[] paramTypes)
        {
            MethodInfo methodInfo = type.GetMethod(methodName,
                BindingFlags.Instance | BindingFlags.Public, null, paramTypes, null);
            return methodInfo != null;
        }

        private bool IsEventExist(Type type, string eventName)
        {
            EventInfo eventInfo = type.GetEvent(eventName,
                BindingFlags.Public | BindingFlags.Instance);
            return eventInfo != null;
        }

        private void button_confirm_Click(object sender, EventArgs e)
        {
            this.IsConfirmed = true;
            this.Close();
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.IsConfirmed = false;
            this.Close();
        }

        public void GetParameter(out string assemblyPath, out string panelClassName, out string messageVariableName)
        {
            if (!string.IsNullOrWhiteSpace(this.textBox_assemblyPath.Text) && !string.IsNullOrWhiteSpace(this.comboBox_classes.Text))
            {
                assemblyPath = this.textBox_assemblyPath.Text;
                panelClassName = this.comboBox_classes.Text;
            }
            else
            {
                assemblyPath = string.Empty;
                panelClassName = string.Empty;
            }
            messageVariableName = this.comboBox_variables.Text;
        }

        private void button_assemblyFileSelect_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = this.openFileDialog_panelSelector.ShowDialog(this);
            if (dialogResult == DialogResult.OK)
            {
                this.textBox_assemblyPath.Text = this.openFileDialog_panelSelector.FileName;
            }
        }

        private void textBox_assemblyPath_TextChanged(object sender, EventArgs e)
        {
            ShowCurrentParam();
        }
    }
}
