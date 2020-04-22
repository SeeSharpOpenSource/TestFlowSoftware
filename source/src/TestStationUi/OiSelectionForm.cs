﻿using System;
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
using TestFlow.SoftDevCommon;
using TestFlow.Software.WinformCommonOi;

namespace TestFlow.Software.OperationPanel
{
    public partial class OiSelectionForm : Form
    {
        private IOperationPanelInfo _oiInfo;
        private GlobalInfo _globalInfo;
        private List<IClassInterfaceDescription> _filteredClasses;
        private Assembly _oiAssembly;
        private IComInterfaceDescription _comDescription;

        private IAssemblyInfo _originalAssemblyInfo;
        private ITypeData _originalTypeData;
        private string _originalParamValue;

        public OiSelectionForm(IOperationPanelInfo oiInfo, GlobalInfo globalInfo)
        {
            InitializeComponent();
            _oiInfo = oiInfo;
            _originalAssemblyInfo = _oiInfo.Assembly;
            _originalTypeData = _oiInfo.OperationPanelClass;
            _originalParamValue = _oiInfo.Parameters;
            _globalInfo = globalInfo;
            _filteredClasses = new List<IClassInterfaceDescription>(20);
            if (null != oiInfo.OperationPanelClass)
            {
                label_assemblyPath.Text = oiInfo.Assembly.Path;
                try
                {
                    InitClasses();
                    if (_filteredClasses.Any(item => item.ClassType.Equals(oiInfo.OperationPanelClass)))
                    {
                        comboBox_classes.Text = oiInfo.OperationPanelClass.Name;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

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
            _comDescription = null;
            comboBox_classes.Text = string.Empty;
            comboBox_classes.Items.Clear();
            _oiAssembly = null;
            IComInterfaceManager interfaceManager = _globalInfo.TestflowEntity.ComInterfaceManager;
            string commonOiPath = _globalInfo.TestflowHome + "WinformCommonOi.dll";
            IComInterfaceDescription oiBaseInterfaceDescription = interfaceManager.GetComponentInterface(commonOiPath);
            ITypeData oiBaseType = interfaceManager.GetTypeByName("WinformOperationPanelBase", "TestFlow.Software.WinformCommonOi");

            _comDescription = interfaceManager.GetComponentInterface(label_assemblyPath.Text);
            _filteredClasses.Clear();
            foreach (IClassInterfaceDescription classDescription in _comDescription.Classes)
            {
                if (interfaceManager.IsDerivedFrom(classDescription.ClassType, oiBaseType))
                {
                    _filteredClasses.Add(classDescription);
                }
            }
            if (_filteredClasses.Count <= 0)
            {
                return;
            }
            _oiAssembly = Assembly.LoadFrom(label_assemblyPath.Text);
            IEnumerable<string> oiClassNames = from item in _filteredClasses select item.ClassType.Name;
            foreach (string className in oiClassNames)
            {
                comboBox_classes.Items.Add(className);
            }
        }

        private void comboBox_classes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(comboBox_classes.Text))
            {
                button_configOi.Enabled = false;
                return;
            }
            try
            {
                IClassInterfaceDescription selectedClass = _filteredClasses[comboBox_classes.SelectedIndex];
                Type oiType = _oiAssembly.GetType($"{selectedClass.ClassType.Namespace}.{selectedClass.ClassType.Name}");
                ConstructorInfo constructor = oiType.GetConstructor(new Type[0]);
                object oiNstanceObj = Activator.CreateInstance(oiType);
                WinformOperationPanelBase oiInstance = oiNstanceObj as WinformOperationPanelBase;
                button_configOi.Enabled = true;
                _oiInfo.Assembly = _comDescription.Assembly;
                _oiInfo.OperationPanelClass = selectedClass.ClassType;
                Type configFormType = oiInstance?.ConfigFormType;
                if (null == configFormType)
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
                IClassInterfaceDescription selectedClass = _filteredClasses[comboBox_classes.SelectedIndex];
                Type oiType = _oiAssembly.GetType($"{selectedClass.ClassType.Namespace}.{selectedClass.ClassType.Name}");
                WinformOperationPanelBase oiInstance = (WinformOperationPanelBase)Activator.CreateInstance(oiType);
                Type configFormType = oiInstance?.ConfigFormType;
                IOiConfigForm configForm = (IOiConfigForm)Activator.CreateInstance(configFormType);
                if (null == configForm)
                {
                    return;
                }
                configForm.ShowDialog();
                string configData = configForm.GetOiConfigData();
                if (!string.IsNullOrWhiteSpace(configData))
                {
                    _oiInfo.Parameters = configData;
                }
            }
            catch (ApplicationException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
