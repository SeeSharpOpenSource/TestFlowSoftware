using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestFlow.DevSoftware
{
    public partial class ValueEditor : Form
    {
        public string _value;
        public bool _valueChanged;

        public ValueEditor(string value)
        {
            InitializeComponent();
            this._value = value;
            this._valueChanged = false;
        }

        #region 无视，继承需要
        private ValueEditor()
        {
            InitializeComponent();
        }
        #endregion

        protected virtual void OkButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
