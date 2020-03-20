using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Testflow.Data.Sequence;

namespace TestStation.Controls
{
    public partial class VariableForm : Form
    {
        public string Value { get; private set; }
        public bool IsGlobalVariable { get; private set; }
        public bool IsCancelled { get; private set; }
        public int SequenceIndex { get; private set; }
        public bool IsExpression { get; private set; }
        private readonly Regex _expRegex;

        private readonly TreeNode _globalNode;
        private readonly bool _expressionEnabled;
        private readonly List<TreeNode> _localNodes = new List<TreeNode>(10);

        private void CreateNode(TreeNode parent, IVariableCollection variables, string group)
        {
            string variableName = Value;
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

        public VariableForm(IVariableCollection globalVariables, IVariableCollection localVariables, string group,
            string value = "", bool expressionEnabled = false)
        {
            this._expRegex = new Regex("^(([^\\.]+)(?:\\.[^\\.]+)*)\\[\\d+\\]$");
            InitializeComponent();
            this.Value = value;
            this.IsGlobalVariable = localVariables.Any(item => item.Name.Equals(value));

            // Global Variables
            _globalNode = treeView_variables.Nodes.Add("Global Variables");
            CreateNode(_globalNode, globalVariables, group);
            _globalNode.Tag = -3;

            // Local Variables
            _localNodes.Add(treeView_variables.Nodes.Add("Local Variables"));
            CreateNode(_localNodes[0], localVariables, group);
            _localNodes[0].Tag = -1;
            this.IsCancelled = true;
            this.IsExpression = false;

            textBox_expression.Text = Value;
            _expressionEnabled = expressionEnabled;
        }

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (treeView_variables.SelectedNode != null && treeView_variables.SelectedNode != _globalNode && !_localNodes.Contains(treeView_variables.SelectedNode) )
            {
                this.Value = (string) treeView_variables.SelectedNode.Tag;
                this.IsGlobalVariable = treeView_variables.SelectedNode.Parent == _globalNode;
            }
            else
            {
                Value = string.Empty;
            }
            if (e.Node == _globalNode || _localNodes.Contains(e.Node))
            {
                return;
            }
            SequenceIndex = (int) e.Node.Parent.Tag;
            if (_expressionEnabled)
            {
                textBox_expression.Text = Value;
            }
            else
            {
                this.IsCancelled = false;
                this.Close();
            }
        }

        private void button_confirm_Click(object sender, System.EventArgs e)
        {
            if (treeView_variables.SelectedNode == null || treeView_variables.SelectedNode == _globalNode || _localNodes.Contains(treeView_variables.SelectedNode) )
            {
                MessageBox.Show("Please select a variable.", "Variable", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            SequenceIndex = (int)treeView_variables.SelectedNode.Parent.Tag;
            this.IsGlobalVariable = treeView_variables.SelectedNode.Parent == _globalNode;
            if (_expressionEnabled)
            {
                if (textBox_expression.Text.Equals(this.Value))
                {
                    this.IsCancelled = false;
                }
                else if (_expRegex.IsMatch(textBox_expression.Text))
                {
                    this.Value = textBox_expression.Text;
                    this.IsExpression = true;
                    this.IsCancelled = false;
                }
                else
                {
                    MessageBox.Show("Invalid expression.", "Expression", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                this.Value = (string)treeView_variables.SelectedNode.Tag;
                this.IsCancelled = false;
            }
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
    }
}
