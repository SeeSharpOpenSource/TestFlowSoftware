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
using Testflow.Data.Sequence;
using Testflow.Modules;
using TestFlow.DevSoftware.Common;
using TestFlow.SoftDevCommon;

namespace TestFlow.DevSoftware.Controls
{
    public partial class SimpleClassEditor : Form
    {
        public SimpleClassEditor(IClassInterfaceDescription classInterface, IComInterfaceManager internfaceManager, string valueStr)
        {
            InitializeComponent();
            _internalOperation = false;
            _propertySetter = classInterface.Functions.FirstOrDefault(item => item.FuncType == FunctionType.InstancePropertySetter);
            _fieldSetter =
                classInterface.Functions.FirstOrDefault(item => item.FuncType == FunctionType.InstanceFieldSetter);
            if (string.IsNullOrWhiteSpace(valueStr))
            {
                _propertyToValue = new Dictionary<string, string>(20);
            }
            else
            {
                _propertyToValue = JsonConvert.DeserializeObject<Dictionary<string, string>>(valueStr);
            }
            _internfaceManager = internfaceManager;
            IsCancelled = true;
        }

        private readonly IComInterfaceManager _internfaceManager;

        private bool _internalOperation;
        private IFuncInterfaceDescription _fieldSetter;
        private IFuncInterfaceDescription _propertySetter;
        private Dictionary<string, string> _propertyToValue;

        public bool IsCancelled { get; private set; }

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

        private void SimpleClassEditor_Load(object sender, EventArgs e)
        {
            _internalOperation = true;
            if (null != _propertySetter)
            {
                AddTableDatas(_propertySetter.Arguments, dataGridView_properties);
            }
            if (null != _fieldSetter)
            {
                AddTableDatas(_fieldSetter.Arguments, dataGridView_fields);
            }
            if (null == _propertySetter && null != _fieldSetter)
            {
                splitContainer_main.Panel1Collapsed = true;
            }
            else if (null == _fieldSetter)
            {
                splitContainer_main.Panel2Collapsed = true;
            }
            _internalOperation = false;
        }

        private void AddTableDatas(IList<IArgumentDescription> arguments, DataGridView table)
        {
            string[] rowValue = new string[3];
            foreach (IArgumentDescription argument in arguments)
            {
                rowValue[0] = argument.Name;
                rowValue[1] = argument.Type.Name;
                rowValue[2] = string.Empty;
                if (_propertyToValue.ContainsKey(argument.Name))
                {
                    rowValue[2] = _propertyToValue[argument.Name];
                }
                int rowIndex = table.Rows.Add(rowValue);
                string[] enumerations;
                if (argument.ArgumentType == VariableType.Enumeration &&
                    null != (enumerations = GetEnumerations(argument.Type)))
                {
                    DataGridViewComboBoxCell comboBoxCell = new DataGridViewComboBoxCell();
                    table.Rows[rowIndex].Cells[2] = comboBoxCell;
                    comboBoxCell.Items.AddRange(enumerations);
                    if (_propertyToValue.ContainsKey(argument.Name))
                    {
                        comboBoxCell.Value = _propertyToValue[argument.Name];
                    }
                }
            }
        }

        private void dataGridView_properties_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (_internalOperation || e.RowIndex < 0)
            {
                return;
            }
            string paramName = (string) dataGridView_properties.Rows[e.RowIndex].Cells[0].Value;
            string paramValue = (string) dataGridView_properties.Rows[e.RowIndex].Cells[2].Value;
            if (string.IsNullOrWhiteSpace(paramValue))
            {
                if (_propertyToValue.ContainsKey(paramName))
                {
                    _propertyToValue.Remove(paramName);
                }
                return;
            }
            if (_propertyToValue.ContainsKey(paramName))
            {
                _propertyToValue[paramName] = paramValue;
            }
            else
            {
                _propertyToValue.Add(paramName, paramValue);
            }
        }

        private void dataGridView_fields_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (_internalOperation || e.RowIndex < 0)
            {
                return;
            }
            string paramName = (string)dataGridView_fields.Rows[e.RowIndex].Cells[0].Value;
            string paramValue = (string)dataGridView_fields.Rows[e.RowIndex].Cells[2].Value;
            if (string.IsNullOrWhiteSpace(paramValue))
            {
                if (_propertyToValue.ContainsKey(paramName))
                {
                    _propertyToValue.Remove(paramName);
                }
                return;
            }
            if (_propertyToValue.ContainsKey(paramName))
            {
                _propertyToValue[paramName] = paramValue;
            }
            else
            {
                _propertyToValue.Add(paramName, paramValue);
            }
        }

        private string[] GetEnumerations(ITypeData type)
        {
            IComInterfaceDescription comInterface = _internfaceManager.GetComInterfaceByName(type.AssemblyName);
            if (null == comInterface)
            {
                return null;
            }
            string typeFullName = $"{type.Namespace}.{type.Name}";
            if (null != comInterface.Enumerations && comInterface.Enumerations.ContainsKey(typeFullName))
            {
                return comInterface.Enumerations[typeFullName];
            }
            return null;
        }

        public string GetValue()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                DateParseHandling = DateParseHandling.None
            };
            return JsonConvert.SerializeObject(_propertyToValue, settings);
        }

        private void dataGridView_properties_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            string paramName = (string)dataGridView_properties.Rows[e.RowIndex].Cells[0].Value;
            string paramValue = (string)dataGridView_properties.Rows[e.RowIndex].Cells[2].Value;
            IArgumentDescription argument = _propertySetter.Arguments.FirstOrDefault(item => item.Name.Equals(paramName));
            if (argument.ArgumentType == VariableType.Class || argument.ArgumentType == VariableType.Struct)
            {
                string changedValue = ShowComplexDataEditor(argument, paramValue);
                if (!string.IsNullOrWhiteSpace(changedValue))
                {
                    dataGridView_properties.Rows[e.RowIndex].Cells[2].Value = changedValue;
                }
                else
                {
                    dataGridView_properties.EndEdit();
                }
            }
        }

        private void dataGridView_fields_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            string paramName = (string)dataGridView_fields.Rows[e.RowIndex].Cells[0].Value;
            string paramValue = (string)dataGridView_fields.Rows[e.RowIndex].Cells[2].Value;
            IArgumentDescription argument = _fieldSetter.Arguments.FirstOrDefault(item => item.Name.Equals(paramName));
            if (argument.ArgumentType == VariableType.Class || argument.ArgumentType == VariableType.Struct)
            {
                string changedValue = ShowComplexDataEditor(argument, paramValue);
                if (!string.IsNullOrWhiteSpace(changedValue))
                {
                    dataGridView_fields.Rows[e.RowIndex].Cells[2].Value = changedValue;
                }
                else
                {
                    dataGridView_fields.EndEdit();
                }
            }
        }

        private string ShowComplexDataEditor(IArgumentDescription argument, string valueStr)
        {
            if (argument.ArgumentType != VariableType.Struct && argument.ArgumentType != VariableType.Class)
            {
                return null;
            }
            ITypeData typeData = argument.Type;
            bool isArrayType = Utility.IsArrayType(typeData);
            IClassInterfaceDescription classInterface = null;
            if (isArrayType)
            {
                classInterface = Utility.GetElementInterface(_internfaceManager, typeData);
            }
            else
            {
                IComInterfaceDescription comInterface = _internfaceManager.GetComInterfaceByName(typeData.AssemblyName);
                classInterface =
                    comInterface?.Classes.FirstOrDefault(item => item.Name.Equals(typeData.Name));
            }
            if (classInterface == null)
            {
                return null;
            }
            
            if (isArrayType)
            {
                IClassInterfaceDescription elementInterface = Utility.GetElementInterface(_internfaceManager, typeData);
                if (null == elementInterface)
                {
                    return null;
                }
                ArrayDataEditor arrayDataEditor = new ArrayDataEditor(elementInterface, _internfaceManager, valueStr);
                arrayDataEditor.ShowDialog(this);
                return arrayDataEditor.IsCancelled ? null : arrayDataEditor.GetValue();

            }
            else
            {
                SimpleClassEditor classEditor = new SimpleClassEditor(classInterface, _internfaceManager, valueStr);
                classEditor.ShowDialog(this);
                return classEditor.IsCancelled ? null : classEditor.GetValue();
            }
        }
    }
}
