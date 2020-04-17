using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TestFlow.DevSoftware
{
    public partial class NumericEditor : ValueEditor
    {
        public NumericEditor(string value) : base(value)
        {
            InitializeComponent();

            #region Get Type and Digits
            string[] arr = value.Split('.');
            if(arr.Length == 1)
            {
                TypecomboBox.Text = "Integer";
                ValuetextBox.Text = value;
                FormattedNumbertextBox.Text = value;
            }
            else
            {
                TypecomboBox.Text = "Real";
                DigitNumber.Value = Convert.ToDecimal(arr[1].Length);
                ValuetextBox.Text = Double.Parse(value).ToString();
                FormattedNumbertextBox.Text = value;
            }
            #endregion
        }

        #region Display Number
        private string DisplayInteger(int value)
        {
            return value.ToString();
        }

        private string DisplayReal(double value, int digits)
        {
            return value.ToString($"f{digits}");
        }
        #endregion

        #region 事件
        private void ValuetextBox_TextChanged(object sender, EventArgs e)
        {
            #region 空值
            if (ValuetextBox.Text == "")
            {
                FormattedNumbertextBox.Text = "";
                return;
            }
            #endregion

            switch (TypecomboBox.SelectedItem.ToString())
            {
                case "Real":
                    double valueDouble;
                    if (!Double.TryParse(ValuetextBox.Text, out valueDouble))
                    {
                        MessageBox.Show("Not a real number.");
                        valueDouble = 0;
                        ValuetextBox.Text = base._value;
                    }
                    FormattedNumbertextBox.Text = DisplayReal(valueDouble, Convert.ToInt32(DigitNumber.Value));
                    break;

                case "Integer":
                    int valueInt;
                    if (!Int32.TryParse(ValuetextBox.Text, out valueInt))
                    {
                        MessageBox.Show("Not an Integer.");
                        valueInt = 0;
                        ValuetextBox.Text = (0).ToString();
                    }
                    FormattedNumbertextBox.Text = DisplayInteger(valueInt);
                    break;
            }
        }

        private void DigitNumber_ValueChanged(object sender, EventArgs e)
        {
            #region 空值
            if (ValuetextBox.Text == "")
            {
                FormattedNumbertextBox.Text = "";
                return;
            }
            #endregion

            FormattedNumbertextBox.Text = DisplayReal(Double.Parse(ValuetextBox.Text), Convert.ToInt32(DigitNumber.Value));
        }

        private void TypecomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (TypecomboBox.SelectedItem.ToString())
            {
                case "Real":
                    ValuetextBox.Text = (0).ToString();
                    DigitNumber.Value = 4;
                    DigitNumber.ReadOnly = false;
                    DigitNumber.Enabled = true;
                    FormattedNumbertextBox.Text = DisplayReal(0, 4);
                    break;
                case "Integer":
                    ValuetextBox.Text = (0).ToString();
                    DigitNumber.Value = 0;
                    DigitNumber.ReadOnly = true;
                    DigitNumber.Enabled = false;
                    FormattedNumbertextBox.Text = DisplayInteger(0);
                    break;
            }
        }
        #endregion

        protected override void OkButton_Click(object sender, EventArgs e)
        {
            string newValue = FormattedNumbertextBox.Text;
            if (!newValue.Equals(base._value))
            {
                base._value = FormattedNumbertextBox.Text;
                base._valueChanged = true;
            }

            base.OkButton_Click(sender, e);
        }
    }
}
