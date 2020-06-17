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
using Testflow.Data;
using Testflow.Data.Description;
using Testflow.Modules;
using Testflow.Usr;

namespace TestFlow.DevSoftware
{
    public partial class AssemblyConfigForm : Form
    {
        private IAssemblyInfoCollection _assemblyInfos;
        private IComInterfaceManager _interfaceManager;
        private List<AssemblyInformation> _informations;

        public bool IsCancelled { get; private set; }

        public AssemblyConfigForm(IAssemblyInfoCollection assemblyInfos, IComInterfaceManager interfaceManager)
        {
            InitializeComponent();
            this._assemblyInfos = assemblyInfos;
            this._interfaceManager = interfaceManager;
            IsCancelled = true;
        }

        private void AssemblyConfigForm_Load(object sender, EventArgs e)
        {
            _informations = new List<AssemblyInformation>(_assemblyInfos.Count);
            dataGridView_assemblies.Rows.Clear();
            foreach (IAssemblyInfo assemblyInfo in _assemblyInfos)
            {
                string assemblyPath = assemblyInfo.Path ?? string.Empty;
                AssemblyInformation assemblyInformation = new AssemblyInformation()
                {
                    Available = false,
                    AssemblyName = assemblyInfo.AssemblyName,
                    UsedVersion = assemblyInfo.Version,
                    DllVersion = string.Empty,
                    AssemblyFullPath = assemblyPath,
                };
                _informations.Add(assemblyInformation);
                UpdateAssemblyInfo(assemblyInformation, assemblyPath, assemblyInfo);
            }
            foreach (AssemblyInformation assemblyInformation in _informations)
            {
                dataGridView_assemblies.Rows.Add(assemblyInformation.Available, assemblyInformation.AssemblyName,
                    assemblyInformation.UsedVersion, assemblyInformation.DllVersion,
                    assemblyInformation.AssemblyFullPath, "...");
            }
        }

        private void UpdateAssemblyInfo(AssemblyInformation assemblyInformation, string assemblyPath, IAssemblyInfo assemblyInfo)
        {
            assemblyInformation.Available = false;
            if (!string.IsNullOrWhiteSpace(assemblyPath) && File.Exists(assemblyPath))
            {
                try
                {
                    IComInterfaceDescription interfaceDescription =
                        _interfaceManager.GetComponentInterface(assemblyPath);
                    assemblyInformation.DllVersion = interfaceDescription.Assembly.Version;
                    assemblyInformation.AssemblyFullPath = assemblyPath;
                    assemblyInformation.Available = IsValidVersion(assemblyInfo.Version, assemblyInformation.DllVersion);
                }
                catch (TestflowException ex)
                {
                    // ignore
                }
            }
        }

        private bool IsValidVersion(string usedVersion, string dllVersion)
        {
            if (string.IsNullOrWhiteSpace(usedVersion) || string.IsNullOrWhiteSpace(dllVersion))
            {
                return false;
            }
            string[] usedVersionElem = usedVersion.Split('.');
            string[] dllVersionElem = dllVersion.Split('.');
            if (usedVersionElem.Length != 4 || dllVersionElem.Length != 4)
            {
                return false;
            }
            for (int i = 0; i < 4; i++)
            {
                int dllVerItemValue;
                int usedVerItemValue;
                if (!int.TryParse(usedVersionElem[i], out usedVerItemValue) ||
                    !int.TryParse(dllVersionElem[i], out dllVerItemValue) || usedVerItemValue > dllVerItemValue)
                {
                    return false;
                }
                else if (usedVerItemValue < dllVerItemValue)
                {
                    return true;
                }
            }
            return true;
        }

        private void dataGridView_assemblies_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != Column_selectPath.Index || e.RowIndex < 0)
            {
                return;
            }
            AssemblyInformation assemblyInformation = _informations[e.RowIndex];
            openFileDialog_assembly.FileName = assemblyInformation.AssemblyFullPath;
            DialogResult dialogResult = openFileDialog_assembly.ShowDialog(this);
            if (dialogResult == DialogResult.OK)
            {
                UpdateAssemblyInfo(assemblyInformation, openFileDialog_assembly.FileName, _assemblyInfos[e.RowIndex]);
                dataGridView_assemblies.Rows[e.RowIndex].Cells[Column_assemblyPath.Index].Value =
                    assemblyInformation.AssemblyFullPath;
                dataGridView_assemblies.Rows[e.RowIndex].Cells[Column_currentVersion.Index].Value =
                    assemblyInformation.DllVersion;
                dataGridView_assemblies.Rows[e.RowIndex].Cells[Column_available.Index].Value =
                    assemblyInformation.Available;
            }
        }

        private void button_confirm_Click(object sender, EventArgs e)
        {
            if (_informations.Any(item => !item.Available))
            {
                MessageBox.Show("Unavailable assembly exist in current configuration.", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
            IsCancelled = false;
            for (int i = 0; i < _assemblyInfos.Count; i++)
            {
                _assemblyInfos[i].Version = _informations[i].UsedVersion;
                _assemblyInfos[i].Path = _informations[i].AssemblyFullPath;
            }
            this.Close();
        }

        private void dataGridView_assemblies_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != Column_specifiedVersion.Index || e.RowIndex < 0)
            {
                return;
            }
            _informations[e.RowIndex].UsedVersion =
                dataGridView_assemblies.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString() ?? string.Empty;
            _informations[e.RowIndex].Available = IsValidVersion(_informations[e.RowIndex].UsedVersion,
                _informations[e.RowIndex].DllVersion);
            dataGridView_assemblies.Rows[e.RowIndex].Cells[Column_available.Index].Value =
                _informations[e.RowIndex].Available;
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.IsCancelled = true;
            this.Close();
        }
    }

    class AssemblyInformation
    {
        public bool Available { get; set; }
        public string AssemblyName { get; set; }
        public string UsedVersion { get; set; }
        public string DllVersion { get; set; }
        public string AssemblyFullPath { get; set; }
    }
}
