using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Testflow.Data;
using Testflow.Data.Sequence;
using Testflow.Modules;
using TestFlow.DevSoftware.Controls;
using TestFlow.SoftDevCommon;

namespace TestFlow.DevSoftware
{
    public partial class NumericEditor : ValueEditor
    {
        public NumericEditor(IVariable variable) : base(variable)
        {
            InitializeComponent();
            ValuetextBox.Text = string.IsNullOrWhiteSpace(Variable.Value) ? "0" : Variable.Value;
        }

        private void ValuetextBox_TextChanged(object sender, EventArgs e)
        {
        }

        protected override void OkButton_Click(object sender, EventArgs e)
        {
            try
            {
                CheckValue(ValuetextBox.Text, TypecomboBox.SelectedIndex);
                ITypeData typeData = GetTypeData(TypecomboBox.SelectedIndex);
                Variable.Type = typeData;
                Variable.Value = ValuetextBox.Text;
                Variable.ReportRecordLevel = (RecordLevel) Enum.Parse(typeof (RecordLevel), comboBox_recordLevel.Text);
            }
            catch (ApplicationException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            base.OkButton_Click(sender, e);
        }

        private void CheckValue(string text, int selectedIndex)
        {
            bool isLegalValue = false;
            switch (selectedIndex)
            {
                case 0:
                    double doubleValue;
                    isLegalValue = double.TryParse(text, out doubleValue);
                    break;
                case 1:
                    float floatValue;
                    isLegalValue = float.TryParse(text, out floatValue);
                    break;
                case 2:
                    int intValue;
                    isLegalValue = int.TryParse(text, out intValue);
                    break;
                case 3:
                    uint uintValue;
                    isLegalValue = uint.TryParse(text, out uintValue);
                    break;
                case 4:
                    short shortValue;
                    isLegalValue = short.TryParse(text, out shortValue);
                    break;
                case 5:
                    ushort ushortValue;
                    isLegalValue = ushort.TryParse(text, out ushortValue);
                    break;
                case 6:
                    byte byteValue;
                    isLegalValue = byte.TryParse(text, out byteValue);
                    break;
                case 7:
                    char charValue;
                    isLegalValue = char.TryParse(text, out charValue);
                    break;
            }
            if (!isLegalValue)
            {
                throw new ApplicationException("Invalid variable value.");
            }
        }

        private void NumericEditor_Load(object sender, EventArgs e)
        {
            comboBox_recordLevel.Items.AddRange(Enum.GetNames(typeof(RecordLevel)));
            comboBox_recordLevel.Text = Variable.ReportRecordLevel.ToString();
            int selectedIndex = 0;
            switch (Variable.Type.Name)
            {
                case "Double":
                    selectedIndex = 0;
                    break;
                case "Single":
                    selectedIndex = 1;
                    break;
                case "Int32":
                    selectedIndex = 2;
                    break;
                case "UInt32":
                    selectedIndex = 3;
                    break;
                case "Int16":
                    selectedIndex = 4;
                    break;
                case "UInt16":
                    selectedIndex = 5;
                    break;
                case "Byte":
                    selectedIndex = 6;
                    break;
                case "Char":
                    selectedIndex = 7;
                    break;
            }
            TypecomboBox.SelectedIndex = selectedIndex;
        }

        private ITypeData GetTypeData(int index)
        {
            GlobalInfo globalInfo = GlobalInfo.GetInstance();
            IComInterfaceManager interfaceManager = globalInfo.TestflowEntity.ComInterfaceManager;

            string typeName = GetTypeName(index);
            return interfaceManager.GetTypeByName(typeName, "System");
        }

        private string GetTypeName(int selectedIndex)
        {
            string typeName = string.Empty;
            switch (selectedIndex)
            {
                case 0:
                    typeName = "Double";
                    break;
                case 1:
                    typeName = "Single";
                    break;
                case 2:
                    typeName = "Int32";
                    break;
                case 3:
                    typeName = "UInt32";
                    break;
                case 4:
                    typeName = "Int16";
                    break;
                case 5:
                    typeName = "UInt16";
                    break;
                case 6:
                    typeName = "Byte";
                    break;
                case 7:
                    typeName = "Char";
                    break;
            }
            return typeName;
        }
    }
}
