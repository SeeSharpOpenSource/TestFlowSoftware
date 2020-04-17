using System;
using System.Windows.Forms;
using Testflow.Data;
using Testflow.Data.Sequence;

namespace TestFlow.DevSoftware.Controls
{
    public partial class ValueEditor : Form
    {
        public string _value;
        public bool _valueChanged;

        protected IVariable Variable;

        private void ValueEditor_Load(object sender, EventArgs e)
        {
            
        }

        public ValueEditor()
        {
            InitializeComponent();
        }

        #region 无视，继承需要
        protected ValueEditor(IVariable variable)
        {
            Variable = variable;
            InitializeComponent();
            this._value = variable.Value ?? string.Empty;
            this._valueChanged = false;
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
