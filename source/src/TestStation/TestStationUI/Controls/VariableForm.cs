using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Testflow.Data;
using Testflow.Data.Description;
using Testflow.Data.Sequence;
using Testflow.Modules;
using Testflow.Usr;
using TestFlow.DevSoftware.Common;
using TestFlow.SoftDevCommon;

namespace TestFlow.DevSoftware.Controls
{
    public partial class VariableForm : Form
    {
        public string ParamValue { get; private set; }
        public bool IsCancelled { get; private set; }
        public bool IsExpression { get; private set; }
        private readonly Regex _expRegex;
        private readonly GlobalInfo _globalInfo;

        private readonly TreeNode _globalNode;
        private readonly bool _expressionEnabled;
        private readonly List<TreeNode> _localNodes = new List<TreeNode>(10);
        private IVariableCollection _globalVariables;
        private IVariableCollection _localVariables;
        private readonly Regex _propertyItemRegex;

        public VariableForm(IVariableCollection globalVariables, IVariableCollection localVariableses, GlobalInfo globalInfo, string paramValue = "", bool expressionEnabled = false)
        {
            this._expRegex = new Regex("^(([^\\.]+)(?:\\.[^\\.]+)*)\\[\\d+\\]$");
            _propertyItemRegex = new Regex("^(.+)\\(.+\\)$");
            InitializeComponent();
            this.ParamValue = paramValue;
            _globalVariables = globalVariables;
            _localVariables = localVariableses;

            // Global Variables
            _globalNode = treeView_variables.Nodes.Add("Global Variables");
            CreateNode(_globalNode, _globalVariables);
            _globalNode.Tag = -3;

            // Local Variables
            _localNodes.Add(treeView_variables.Nodes.Add("Local Variables"));
            CreateNode(_localNodes[0], _localVariables);
            _localNodes[0].Tag = -1;
            this.IsCancelled = true;
            this.IsExpression = false;
            this._globalInfo = globalInfo;
            _expressionEnabled = expressionEnabled;

            if (!string.IsNullOrWhiteSpace(paramValue))
            {
                textBox_expression.Text = paramValue;
            }
        }

        private void CreateNode(TreeNode parent, IVariableCollection variables)
        {
            string variableName = ParamValue;
            Match match = _expRegex.Match(variableName);
            if (match.Success)
            {
                variableName = match.Groups[2].Value;
            }
            foreach (IVariable variable in variables)
            {
                TreeNode variableNode = parent.Nodes.Add(GetVariableShowText(variable));
                variableNode.Tag = variable.Name;
                if (variableName != null && variableName.Equals(variable.Name))
                {
                    treeView_variables.SelectedNode = variableNode;
                }
            }
        }

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (treeView_variables.SelectedNode != null && treeView_variables.SelectedNode != _globalNode && !_localNodes.Contains(treeView_variables.SelectedNode))
            {
                this.ParamValue = (string)treeView_variables.SelectedNode.Tag;
            }
            else
            {
                ParamValue = string.Empty;
            }
            if (e.Node == _globalNode || _localNodes.Contains(e.Node))
            {
                return;
            }
            textBox_expression.Text = ParamValue;
            if (!_expressionEnabled)
            {
                this.IsCancelled = false;
                this.Close();
            }
        }

        private void button_confirm_Click(object sender, System.EventArgs e)
        {
            string showValue = textBox_expression.Text;
            if (string.IsNullOrWhiteSpace(showValue))
            {
                MessageBox.Show("Please select a variable.", "Variable", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            bool isVariable;
            this.ParamValue = Utility.GetParamValue(showValue, out isVariable);
            Match matchData = _expRegex.Match(this.ParamValue);
            string variableName = _expressionEnabled && matchData.Success
                ? matchData.Groups[2].Value
                : Utility.GetVariableName(ParamValue);
            bool isLocalVariable = _localVariables.Any(item => item.Name.Equals(variableName));
            if (!isLocalVariable && !_globalVariables.Any(item => item.Name.Equals(variableName)))
            {
                MessageBox.Show($"Variable <{variableName}> does not exist.", "Variable", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
            this.IsExpression = matchData.Success;
            this.IsCancelled = false;
            this.Close();
        }

        private void button_cancel_Click(object sender, System.EventArgs e)
        {
            this.IsCancelled = true;
            this.Close();
        }

        private string GetVariableShowText(IVariable variable)
        {
            return null != variable.Type ? $"{variable.Name}({variable.Type.Name})" : variable.Name;
        }

        private void VariableForm_Load(object sender, System.EventArgs e)
        {
            treeView_variables.ExpandAll();
            splitContainer_varControls.Panel2Collapsed = !_expressionEnabled;
        }

        private void textBox_expression_TextChanged(object sender, System.EventArgs e)
        {
            if (!_expressionEnabled)
            {
                return;
            }
            listBox_properties.Items.Clear();
            string showValue = textBox_expression.Text;
            if (string.IsNullOrWhiteSpace(showValue) || _expRegex.IsMatch(showValue))
            {
                return;
            }
            bool isVariable;
            string paramValue = Utility.GetParamValue(showValue, out isVariable);
            IVariable variable = GetVariable(paramValue);
            if (variable?.Type == null)
            {
                return;
            }
            IComInterfaceManager interfaceManager = _globalInfo.TestflowEntity.ComInterfaceManager;
            try
            {
                ITypeData propertyType = variable.Type;
                if (!paramValue.Equals(variable.Name))
                {
                    string propertyStr = paramValue.Substring(variable.Name.Length + 1,
                    paramValue.Length - variable.Name.Length - 1);
                    propertyType = interfaceManager.GetPropertyType(variable.Type, propertyStr);
                }
                IAssemblyInfo assemblyInfo;
                IClassInterfaceDescription classDescription = interfaceManager.GetClassDescriptionByType(propertyType, out assemblyInfo);
                if (null == classDescription ||
                    (classDescription.Kind != VariableType.Class && classDescription.Kind != VariableType.Struct))
                {
                    return;
                }
                Dictionary<string, string> properties = interfaceManager.GetTypeProperties(propertyType);
                const string itemFormat = "{0}({1})";
                if (null != properties)
                {
                    foreach (KeyValuePair<string, string> keyValuePair in properties)
                    {
                        int typeNameStartIndex = keyValuePair.Value.LastIndexOf('.') + 1;
                        string itemType = keyValuePair.Value.Substring(typeNameStartIndex, keyValuePair.Value.Length - typeNameStartIndex);
                        listBox_properties.Items.Add(string.Format(itemFormat, keyValuePair.Key, itemType));
                    }
                }
                Dictionary<string, string> fields = interfaceManager.GetTypeFields(propertyType);
                if (null != fields)
                {
                    foreach (KeyValuePair<string, string> keyValuePair in fields)
                    {
                        int typeNameStartIndex = keyValuePair.Value.LastIndexOf('.') + 1;
                        string itemType = keyValuePair.Value.Substring(typeNameStartIndex, keyValuePair.Value.Length - typeNameStartIndex);
                        listBox_properties.Items.Add(string.Format(itemFormat, keyValuePair.Key, itemType));
                    }
                }
            }
            catch (TestflowException exception)
            {
                MessageBox.Show(exception.Message, "Variable", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private IVariable GetVariable(string paramValue)
        {
            string variableName = Utility.GetVariableName(paramValue);
            IVariable variable = _localVariables.FirstOrDefault(item => item.Name.Equals(variableName));
            if (null != variable)
            {
                return variable;
            }
            return _globalVariables.FirstOrDefault(item => item.Name.Equals(variableName));
        }

        private void listBox_properties_DoubleClick(object sender, System.EventArgs e)
        {
            if (listBox_properties.SelectedIndex < 0)
            {
                return;
            }
            Match matchData = _propertyItemRegex.Match(listBox_properties.Items[listBox_properties.SelectedIndex].ToString());
            if (!matchData.Success)
            {
                return;
            }
            string selectProperty = matchData.Groups[1].Value;
            textBox_expression.AppendText($".{selectProperty}");
        }
    }
}
