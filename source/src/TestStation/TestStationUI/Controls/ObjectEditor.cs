using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using Newtonsoft.Json;
using Testflow;
using Testflow.Data;
using Testflow.Data.Description;
using Testflow.Data.Sequence;
using Testflow.Modules;

namespace TestFlow.DevSoftware.Controls
{
    public partial class ObjectEditor : Form
    {
        private IComInterfaceManager _interfaceManager;
        private const string DoubleFormat = "0.#####";
        private readonly IVariable _variable;
        private bool _isCancelled;

        private string _originalValue;
        private ITypeData _originalType;
        private HashSet<string> _supportedArray;

        public ObjectEditor(IVariable variable)
        {
            _supportedArray = new HashSet<string>()
            {
                "String[]",
                "Boolean[]",
                "Double[]",
                "Single[]",
                "Int32[]",
                "UInt32[]",
                "Int16[]",
                "UInt16[]",
                "Byte[]"
            };
            InitializeComponent();
            this._variable = variable;
            this._originalType = variable.Type;
            this._originalValue = _variable.Value;

            if (null != variable.Type)
            {
                comboBox_type.Items.Insert(0, variable.Type.Name);
                comboBox_type.Text = variable.Type.Name;
            }
            else
            {
                comboBox_type.SelectedIndex = 0;
            }
            TestflowRunner testflowRunner = TestflowRunner.GetInstance();
            _interfaceManager = testflowRunner.ComInterfaceManager;
            _isCancelled = true;
        }

        private void ValuecomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string variableType = GetVariableType();
            bool isSupportedArray = _supportedArray.Contains(variableType);
            dataGridView_element.ReadOnly = !isSupportedArray;
            dataGridView_element.AllowUserToAddRows = isSupportedArray;
            if (variableType.Equals("Object"))
            {
                _variable.Type = null;
                _variable.Value = string.Empty;
            }
            else if (null != _originalType && variableType.Equals(_originalType.Name))
            {
                _variable.Type = _originalType;
                _variable.Value = _originalValue;
            }
            else if (null != _variable.Type && _variable.Type.Name.Equals(variableType))
            {
                return;
            }
            else
            {
                try
                {
                    IClassInterfaceDescription description = _interfaceManager.GetClassDescriptionByType("mscorlib",
                        "System", variableType);
                    _variable.Type = description.ClassType;
                    _variable.Value = string.Empty;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Object Editor", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    comboBox_type.Text = "Object";
                    _variable.Value = string.Empty;
                }
            }
            dataGridView_element.Rows.Clear();
            UpdateDataTableColumn(variableType);
            ShowElements();
        }

        private void UpdateDataTableColumn(string variableType)
        {
            if (variableType.Equals("Boolean[]") && dataGridView_element.Columns[0] is DataGridViewTextBoxColumn)
            {
                dataGridView_element.AllowUserToAddRows = false;
                dataGridView_element.Columns.Clear();
                DataGridViewComboBoxColumn column = new DataGridViewComboBoxColumn();
                column.DataSource = new string[] {"True", "False"};
                column.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
                column.HeaderText = "Value";
                column.Name = "Column_value";
                dataGridView_element.Columns.Add(column);
                dataGridView_element.AllowUserToAddRows = true;
            }
            else if (!variableType.Equals("Boolean[]") && !(dataGridView_element.Columns[0] is DataGridViewTextBoxColumn))
            {
                dataGridView_element.Columns.Clear();
                DataGridViewComboBoxColumn column = new DataGridViewComboBoxColumn();
                column.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
                column.HeaderText = "Value";
                column.Name = "Column_value";
                dataGridView_element.Columns.Add(column);
            }
        }

        private string GetVariableType()
        {
            string varType;
            switch (comboBox_type.Text)
            {
                case "Object":
                    varType = "Object";
                    break;
                case "Array of String":
                    varType = "String[]";
                    break;
                case "Array of Boolean":
                    varType = "Boolean[]";
                    break;
                case "Array of Decimal(Double)":
                    varType = "Double[]";
                    break;
                case "Array of Decimal(Float)":
                    varType = "Single[]";
                    break;
                case "Array of Decimal(Int)":
                    varType = "Int32[]";
                    break;
                case "Array of Decimal(UInt)":
                    varType = "UInt32[]";
                    break;
                case "Array of Decimal(Short)":
                    varType = "Int16[]";
                    break;
                case "Array of Decimal(UShort)":
                    varType = "UInt16[]";
                    break;
                case "Array of Decimal(Byte)":
                    varType = "Byte[]";
                    break;
                default:
                    varType = comboBox_type.Text;
                    break;
            }
            return varType;
        }

        private void ShowElements()
        {
            dataGridView_element.Rows.Clear();
            if (string.IsNullOrWhiteSpace(_variable.Value) || null == _variable.Type)
            {
                return;
            }
            string variableType = GetVariableType();
            IEnumerable arrayValue = null;
            try
            {
                switch (variableType)
                {
                    case "String[]":
                        arrayValue = JsonConvert.DeserializeObject<string[]>(_variable.Value);
                        break;
                    case "Boolean[]":
                        arrayValue = JsonConvert.DeserializeObject<bool[]>(_variable.Value);
                        break;
                    case "Double[]":
                        arrayValue = JsonConvert.DeserializeObject<double[]>(_variable.Value);
                        break;
                    case "Single[]":
                        arrayValue = JsonConvert.DeserializeObject<float[]>(_variable.Value);
                        break;
                    case "Int32[]":
                        arrayValue = JsonConvert.DeserializeObject<int[]>(_variable.Value);
                        break;
                    case "UInt32[]":
                        arrayValue = JsonConvert.DeserializeObject<uint[]>(_variable.Value);
                        break;
                    case "Int16[]":
                        arrayValue = JsonConvert.DeserializeObject<short[]>(_variable.Value);
                        break;
                    case "UInt16[]":
                        arrayValue = JsonConvert.DeserializeObject<ushort[]>(_variable.Value);
                        break;
                    case "Byte[]":
                        arrayValue = JsonConvert.DeserializeObject<byte[]>(_variable.Value);
                        break;
                }
                if (null == arrayValue)
                {
                    return;
                }
                foreach (object element in arrayValue)
                {
                    dataGridView_element.Rows.Add(element.ToString());
                }
            }
            catch (JsonException ex)
            {
                MessageBox.Show(ex.Message, "Object Editor", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _variable.Value = string.Empty;
            }
        }


        private string GetVariableValue()
        {
            string variableType = GetVariableType();
            object serializeObject = null;
            switch (variableType)
            {
                case "String[]":
                    serializeObject = GetConvertedValue(typeof(string), (source) => source.ToString());
                    break;
                case "Boolean[]":
                    serializeObject = GetConvertedValue(typeof(bool), (source) => bool.Parse(source));
                    break;
                case "Double[]":
                    serializeObject = GetConvertedValue(typeof(double), (source) => double.Parse(source));
                    break;
                case "Single[]":
                    serializeObject = GetConvertedValue(typeof(float), (source) => float.Parse(source));
                    break;
                case "Int32[]":
                    serializeObject = GetConvertedValue(typeof(int), (source) => int.Parse(source));
                    break;
                case "UInt32[]":
                    serializeObject = GetConvertedValue(typeof(uint), (source) => uint.Parse(source));
                    break;
                case "Int16[]":
                    serializeObject = GetConvertedValue(typeof(short), (source) => short.Parse(source));
                    break;
                case "UInt16[]":
                    serializeObject = GetConvertedValue(typeof(ushort), (source) => ushort.Parse(source));
                    break;
                case "Byte[]":
                    serializeObject = GetConvertedValue(typeof(byte), (source) => byte.Parse(source));
                    break;
            }
            return null != serializeObject ? JsonConvert.SerializeObject(serializeObject) : string.Empty;
        }

        private Array GetConvertedValue(Type type, Func<string, object> convertFunc)
        {
            int rowCount = dataGridView_element.RowCount;
            // 用户最后一行不填写值可能为null
            if (rowCount > 0 && null == dataGridView_element.Rows[rowCount - 1].Cells[0].Value)
            {
                rowCount -= 1;
            }
            Array array = Array.CreateInstance(type, rowCount);
            int index = 0;
            for (int i = 0; i < rowCount; i++)
            {
                object element = dataGridView_element.Rows[i].Cells[0].Value;
                if (null == element)
                {
                    throw new FormatException("The element of array should not be null.");
                }
                object value = convertFunc.Invoke(element.ToString());
                array.SetValue(value, index++);
            }
            return array;
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            this._variable.Type = _originalType;
            this._variable.Value = _originalValue;
            this.Close();
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            try
            {
                string variableValue = GetVariableValue();
                _variable.Value = variableValue;
                _isCancelled = false;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Object Editor", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ObjectEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_isCancelled)
            {
                this._variable.Type = _originalType;
                this._variable.Value = _originalValue;
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridViewRow currentRow = dataGridView_element.CurrentRow;
            if (null == currentRow || currentRow.Index == dataGridView_element.RowCount - 1)
            {
                return;
            }
            dataGridView_element.Rows.RemoveAt(currentRow.Index);
        }
    }
}
