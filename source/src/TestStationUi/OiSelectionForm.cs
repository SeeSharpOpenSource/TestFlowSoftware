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
using Testflow.Data;
using Testflow.Data.Description;
using Testflow.Data.Sequence;
using Testflow.Modules;
using Testflow.Runtime.OperationPanel;
using TestFlow.SoftDevCommon;
using TestFlow.Software.WinformCommonOi;
using TestFlow.Software.WinformCommonOi.ValueInputOi;

namespace TestFlow.Software.OperationPanel
{
    public partial class OiSelectionForm : Form
    {
        private IOperationPanelInfo _oiInfo;
        private ISequenceGroup _sequenceData;
        private GlobalInfo _globalInfo;
        private List<Type> _filteredClasses;
        private Assembly _oiAssembly;
        private IComInterfaceDescription _comDescription;

        private IAssemblyInfo _originalAssemblyInfo;
        private ITypeData _originalTypeData;
        private string _originalParamValue;
        private Type _configFormType;

        public OiSelectionForm(ISequenceGroup sequenceGroup, GlobalInfo globalInfo)
        {
            InitializeComponent();
            _sequenceData = sequenceGroup;
            _oiInfo = sequenceGroup.Info.OperationPanelInfo;
            _originalAssemblyInfo = _oiInfo.Assembly;
            _originalTypeData = _oiInfo.OperationPanelClass;
            _originalParamValue = _oiInfo.Parameters;
            _globalInfo = globalInfo;
            _filteredClasses = new List<Type>(20);
            string assemblyPath = this._oiInfo.Assembly?.Path;
            if (null == assemblyPath)
            {
                assemblyPath = GetDefaultOiAssembly();
                label_assemblyPath.Text = assemblyPath;
            }
            else
            {
                label_assemblyPath.Text = assemblyPath;
                label_currentOiClass.Text = _oiInfo.OperationPanelClass?.Name ?? string.Empty;
            }
            _comDescription = this._globalInfo.TestflowEntity.ComInterfaceManager.GetComponentInterface(assemblyPath);
            ShowCurrentAssemblyAndClassInfo();
        }

        private void button_confirm_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            _oiInfo.Assembly = _originalAssemblyInfo;
            _oiInfo.OperationPanelClass = _originalTypeData;
            _oiInfo.Parameters = _originalParamValue;
            this.Close();
        }

        private void button_selectAssembly_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = openFileDialog_assembly.ShowDialog(this);
            if (dialogResult != DialogResult.OK)
            {
                return;
            }
            label_assemblyPath.Text = openFileDialog_assembly.FileName;
            ShowCurrentAssemblyAndClassInfo();
        }

        private void ShowCurrentAssemblyAndClassInfo()
        {
            try
            {
                InitClasses();
                if (comboBox_classes.Items.Count > 0 && string.IsNullOrWhiteSpace(comboBox_classes.Text))
                {
                    comboBox_classes.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitClasses()
        {
            comboBox_classes.Text = string.Empty;
            comboBox_classes.Items.Clear();
            _oiAssembly = null;
            string oiPath = this.label_assemblyPath.Text;
            _filteredClasses.Clear();
            _oiAssembly = Assembly.LoadFrom(oiPath);
            foreach (Type exportedType in _oiAssembly.ExportedTypes)
            {
                if (typeof(IOperationPanel).IsAssignableFrom(exportedType))
                {
                    this._filteredClasses.Add(exportedType);
                }
            }
            if (_filteredClasses.Count <= 0)
            {
                return;
            }
            _oiAssembly = Assembly.LoadFrom(label_assemblyPath.Text);
            IEnumerable<string> oiClassNames = from item in _filteredClasses select item.Name;
            _comDescription = this._globalInfo.TestflowEntity.ComInterfaceManager.GetComponentInterface(oiPath);
            foreach (string className in oiClassNames)
            {
                comboBox_classes.Items.Add(className);
            }
        }

        private static string GetDefaultOiAssembly()
        {
            string workSpaceDir = Environment.GetEnvironmentVariable("TESTFLOW_WORKSPACE").Split(';')[0];
            string defaultOiPath = workSpaceDir + "JYProductOperationPanel.dll";
            return defaultOiPath;
        }

        private void comboBox_classes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(comboBox_classes.Text))
            {
                button_configOi.Enabled = false;
                return;
            }
            _configFormType = null;
            try
            {
                Type selectedClass = _filteredClasses[comboBox_classes.SelectedIndex];
                Type oiType = _oiAssembly.GetType($"{selectedClass.Namespace}.{selectedClass.Name}");
                PropertyInfo configPanelInfo =
                    oiType.GetProperty("ConfigPanel", BindingFlags.Public | BindingFlags.Static);
                
                button_configOi.Enabled = true;
                _oiInfo.Assembly = _comDescription.Assembly;
                _oiInfo.OperationPanelClass = this._comDescription.Classes
                    .First(item => item.Name.Equals(selectedClass.Name)).ClassType;
                this._configFormType = configPanelInfo?.GetValue(null) as Type;
                if (null == this._configFormType)
                {
                    button_configOi.Enabled = false;
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button_configOi_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(comboBox_classes.Text))
                {
                    button_configOi.Enabled = false;
                    return;
                }
                Type configFormType = _configFormType;
                IOIConfigPanel configForm = (IOIConfigPanel)Activator.CreateInstance(configFormType);
                if (null == configForm)
                {
                    return;
                }

                configForm.ShowOiConfigPanel(this._sequenceData, GetShowParameter());
                bool isConfirmed = false;
                string configData = configForm.GetParameter(out isConfirmed);
                if (!isConfirmed)
                {
                    return;
                }
                _oiInfo.Parameters = configData ?? string.Empty;
            }
            catch (ApplicationException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private object[] GetShowParameter()
        {
            List<object> oiExtraParams = new List<object>(10);
            // TestFlow平台运行时关联路径
            List<string> platformDirs = new List<string>(10);
            IConfigurationManager configManager = this._globalInfo.TestflowEntity.ConfigurationManager;
            platformDirs.Add(Utility.GetParentDir(this._sequenceData.Info.SequenceGroupFile));
            platformDirs.AddRange(configManager.ConfigData.GetProperty<string[]>("WorkspaceDir"));
            platformDirs.Add(configManager.ConfigData.GetProperty<string>("PlatformLibDir"));
            platformDirs.Add(configManager.ConfigData.GetProperty<string>("TestflowHome"));
            oiExtraParams.Add(platformDirs.ToArray());

            return oiExtraParams.ToArray();
        }

        private void button_removeOi_Click(object sender, EventArgs e)
        {
            this._oiInfo.Parameters = string.Empty;
            this._oiInfo.Assembly = null;
            this._oiInfo.OperationPanelClass = null;
            this.label_currentoiAssembly.Text = string.Empty;
            this.label_currentOiClass.Text = string.Empty;
        }
    }
}
