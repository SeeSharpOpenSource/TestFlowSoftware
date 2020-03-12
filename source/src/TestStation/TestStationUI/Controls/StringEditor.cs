using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TestStation
{
    public partial class StringEditor : TestStation.ValueEditor
    {

        public StringEditor(string value) : base(value)
        {
            InitializeComponent();

            ValuetextBox.Text = value;
        }

        protected override void OkButton_Click(object sender, EventArgs e)
        {
            string newValue = ValuetextBox.Text;
            if (!newValue.Equals(base._value))
            {
                base._value = ValuetextBox.Text;
                base._valueChanged = true;
            }

            base.OkButton_Click(sender, e);
        }
    }
}
 