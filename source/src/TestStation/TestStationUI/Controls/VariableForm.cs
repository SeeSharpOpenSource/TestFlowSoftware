using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Testflow.Data.Sequence;

namespace TestStation.Controls
{
    public partial class VariableForm : Form
    {
        public string VariableName { get; private set; }
        public bool IsGlobalVariable { get; private set; }
        public bool IsCancelled { get; private set; }
        public int SequenceIndex { get; private set; }

        private readonly TreeNode _globalNode;
        private readonly List<TreeNode> _localNodes = new List<TreeNode>(10);

        private void CreateNode(TreeNode parent, IVariableCollection variables, string group)
        {
            foreach (IVariable variable in variables)
            {
                TreeNode variableNode = parent.Nodes.Add(GetVariableShowText(variable));
                variableNode.Tag = variable.Name;
                if (VariableName != null && VariableName.Equals(variable.Name))
                {
                    treeView_variables.SelectedNode = variableNode;
                }
            }
        }

        public VariableForm(IVariableCollection globalVariables, IVariableCollection localVariables, string group, string name = "")
        {
            InitializeComponent();
            this.VariableName = name;
            this.IsGlobalVariable = localVariables.Any(item => item.Name.Equals(name));

            // Global Variables
            _globalNode = treeView_variables.Nodes.Add("Global Variables");
            CreateNode(_globalNode, globalVariables, group);
            _globalNode.Tag = -3;

            // Local Variables
            _localNodes.Add(treeView_variables.Nodes.Add("Local Variables"));
            CreateNode(_localNodes[0], localVariables, group);
            _localNodes[0].Tag = -1;
            this.IsCancelled = true;
        }

        public VariableForm(ISequenceGroup sequenceGroup, string group)
        {
            InitializeComponent();
            this.IsGlobalVariable = false;

            // Global Variables
            _globalNode = treeView_variables.Nodes.Add("Global Variables");
            _globalNode.Tag = -3;
            CreateNode(_globalNode, sequenceGroup.Variables, group);

            _localNodes.Add(treeView_variables.Nodes.Add($"{sequenceGroup.SetUp.Name} Variables"));
            CreateNode(_localNodes[0], sequenceGroup.SetUp.Variables, group);
            _localNodes[0].Tag = -1;

            _localNodes.Add(treeView_variables.Nodes.Add($"{sequenceGroup.TearDown.Name} Variables"));
            CreateNode(_localNodes[1], sequenceGroup.TearDown.Variables, group);
            _localNodes[1].Tag = -2;

            for (int i = 0; i < sequenceGroup.Sequences.Count; i++)
            {
                // Local Variables
                TreeNode treeNode = treeView_variables.Nodes.Add($"{sequenceGroup.Sequences[i].Name} Variables");
                treeNode.Tag = sequenceGroup.Sequences[i].Index;
                _localNodes.Add(treeNode);
                CreateNode(treeNode, sequenceGroup.Sequences[i].Variables, group);
            }
            foreach (TreeNode localNode in _localNodes)
            {
                if (0 == localNode.Nodes.Count)
                {
                    treeView_variables.Nodes.Remove(localNode);
                }
            }
            this.IsCancelled = true;
        }

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (treeView_variables.SelectedNode != null && treeView_variables.SelectedNode != _globalNode && !_localNodes.Contains(treeView_variables.SelectedNode) )
            {
                this.VariableName = (string) treeView_variables.SelectedNode.Tag;
                this.IsGlobalVariable = treeView_variables.SelectedNode.Parent == _globalNode;
            }
            else
            {
                VariableName = string.Empty;
            }
            if (e.Node == _globalNode || _localNodes.Contains(e.Node))
            {
                return;
            }
            SequenceIndex = (int) e.Node.Parent.Tag;
            this.IsCancelled = false;
            this.Close();
        }

        private void button_confirm_Click(object sender, System.EventArgs e)
        {
            if (treeView_variables.SelectedNode == null || treeView_variables.SelectedNode == _globalNode || _localNodes.Contains(treeView_variables.SelectedNode) )
            {
                MessageBox.Show("Please select a variable.", "Variable", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            SequenceIndex = (int)treeView_variables.SelectedNode.Parent.Tag;
            this.VariableName = (string) treeView_variables.SelectedNode.Tag;
            this.IsGlobalVariable = treeView_variables.SelectedNode.Parent == _globalNode;
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
        }
    }
}
