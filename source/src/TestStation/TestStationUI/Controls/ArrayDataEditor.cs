using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Testflow.Data;
using Testflow.Data.Description;
using Testflow.Modules;
using TestFlow.DevSoftware.Common;
using TestFlow.SoftDevCommon;

namespace TestFlow.DevSoftware.Controls
{
    public partial class ArrayDataEditor : Form
    {
        public ArrayDataEditor(IClassInterfaceDescription classInterface, IComInterfaceManager internfaceManager, string valueStr)
        {
            if (string.IsNullOrWhiteSpace(valueStr))
            {
                _elements = new List<string>(10);
            }
            else
            {
                _elements = JsonConvert.DeserializeObject<List<string>>(valueStr);
            }
            InitializeComponent();
            this._classInterface = classInterface;
            this._interfaceManager = internfaceManager;
            _comInterface = internfaceManager.GetComInterfaceByName(classInterface.ClassType.AssemblyName);
            _internalOperation = false;

            string fullName = $"{_classInterface.ClassType.Namespace}.{_classInterface.ClassType.Name}";
            if (_comInterface.Enumerations.ContainsKey(fullName))
            {
                DataGridViewComboBoxCell comboBoxCell = new DataGridViewComboBoxCell();
                Column_value.CellTemplate = comboBoxCell;
                comboBoxCell.Items.AddRange(_comInterface.Enumerations[fullName]);
            }
        }

        private IComInterfaceManager _interfaceManager;
        private IClassInterfaceDescription _classInterface;
        private IComInterfaceDescription _comInterface;
        private List<string> _elements;
        private bool _internalOperation;

        public bool IsCancelled { get; private set; }

        private void ArrayDataEditor_Load(object sender, EventArgs e)
        {
            label_elementType.Text = $"{_classInterface.ClassType.Namespace}.{_classInterface.ClassType.Name}";
            UpdateUIData();
        }

        private void UpdateUIData()
        {
            _internalOperation = true;
            dataGridView_elements.Rows.Clear();
            int index = 1;
            foreach (string element in _elements)
            {
                dataGridView_elements.Rows.Add(index.ToString(), element);
                index++;
            }
            _internalOperation = false;
        }

        public string GetValue()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                DateParseHandling = DateParseHandling.None
            };
            return JsonConvert.SerializeObject(_elements, settings);
        }

        private void dataGridView_elements_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (_internalOperation || e.RowIndex >= _elements.Count || e.RowIndex < 0)
            {
                return;
            }
            _elements[e.RowIndex] = (string) dataGridView_elements.Rows[e.RowIndex].Cells[1].Value;
        }

        private void button_confirm_Click(object sender, EventArgs e)
        {
            this.IsCancelled = false;
            this.Close();
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.IsCancelled = true;
            this.Close();
        }

        private void button_add_Click(object sender, EventArgs e)
        {
            _elements.Add(string.Empty);
            UpdateUIData();
        }

        private void button_insert_Click(object sender, EventArgs e)
        {
            int index;
            if (null != dataGridView_elements.CurrentRow && (index = dataGridView_elements.CurrentRow.Index) >= 0)
            {
                _elements.Insert(index, string.Empty);
                UpdateUIData();
            }
        }

        private void button_remove_Click(object sender, EventArgs e)
        {
            DataGridViewRow currentRow = dataGridView_elements.CurrentRow;
            if (null == currentRow || currentRow.Index < 0)
            {
                return;
            }
            else
            {
                _elements.RemoveAt(currentRow.Index);
            }
            UpdateUIData();
        }

        private void dataGridView_elements_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (_internalOperation || e.RowIndex >= _elements.Count)
            {
                return;
            }
            DataGridViewCell valueCell = dataGridView_elements.Rows[e.RowIndex].Cells[1];
            string changedValue = ShowComplexDataEditor((string) valueCell.Value);
            if (!string.IsNullOrWhiteSpace(changedValue))
            {
                valueCell.Value = changedValue;
            }
            else
            {
                dataGridView_elements.EndEdit();
            }
        }

        private string ShowComplexDataEditor(string valueStr)
        {
            if (_classInterface.Kind != VariableType.Struct && _classInterface.Kind != VariableType.Class)
            {
                return null;
            }
            ITypeData typeData = _classInterface.ClassType;
            bool isArrayType = Utility.IsArrayType(typeData);
            IClassInterfaceDescription classInterface = null;
            if (isArrayType)
            {
                classInterface = Utility.GetElementInterface(_interfaceManager, typeData);
            }
            else
            {
                IComInterfaceDescription comInterface = _interfaceManager.GetComInterfaceByName(typeData.AssemblyName);
                classInterface = comInterface?.Classes.FirstOrDefault(item => item.Name.Equals(typeData.Name));
            }
            if (classInterface == null)
            {
                return null;
            }
            if (isArrayType)
            {
                IClassInterfaceDescription elementInterface = Utility.GetElementInterface(_interfaceManager, typeData);
                if (null == elementInterface)
                {
                    return null;
                }
                ArrayDataEditor arrayDataEditor = new ArrayDataEditor(elementInterface, _interfaceManager, valueStr);
                arrayDataEditor.ShowDialog(this);
                return arrayDataEditor.IsCancelled ? null : arrayDataEditor.GetValue();

            }
            else
            {
                SimpleClassEditor classEditor = new SimpleClassEditor(classInterface, _interfaceManager, valueStr);
                classEditor.ShowDialog(this);
                return classEditor.IsCancelled ? null : classEditor.GetValue();
            }
        }

    }
}
