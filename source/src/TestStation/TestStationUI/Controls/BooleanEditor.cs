using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TestStation
{
    public partial class BooleanEditor : TestStation.ValueEditor
    {
        public BooleanEditor(string value) : base(value)
        {
            InitializeComponent();

            ValuecomboBox.Text = value;
        }

        protected override void OkButton_Click(object sender, EventArgs e)
        {
            string newValue = ValuecomboBox.Text;
            if (!newValue.Equals(base._value))
            {
                base._value = ValuecomboBox.Text;
                base._valueChanged = true;
            }

            base.OkButton_Click(sender, e);
        }
    }
}
