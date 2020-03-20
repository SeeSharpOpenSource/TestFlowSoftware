namespace TestStation.Controls
{
    partial class VariableForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.treeView_variables = new System.Windows.Forms.TreeView();
            this.button_confirm = new System.Windows.Forms.Button();
            this.button_cancel = new System.Windows.Forms.Button();
            this.splitContainer_varControls = new System.Windows.Forms.SplitContainer();
            this.textBox_expression = new System.Windows.Forms.TextBox();
            this.label_expression = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_varControls)).BeginInit();
            this.splitContainer_varControls.Panel1.SuspendLayout();
            this.splitContainer_varControls.Panel2.SuspendLayout();
            this.splitContainer_varControls.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView_variables
            // 
            this.treeView_variables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView_variables.Location = new System.Drawing.Point(0, 0);
            this.treeView_variables.Name = "treeView_variables";
            this.treeView_variables.Size = new System.Drawing.Size(260, 281);
            this.treeView_variables.TabIndex = 0;
            this.treeView_variables.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseDoubleClick);
            // 
            // button_confirm
            // 
            this.button_confirm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_confirm.Location = new System.Drawing.Point(43, 332);
            this.button_confirm.Name = "button_confirm";
            this.button_confirm.Size = new System.Drawing.Size(75, 23);
            this.button_confirm.TabIndex = 1;
            this.button_confirm.Text = "Confirm";
            this.button_confirm.UseVisualStyleBackColor = true;
            this.button_confirm.Click += new System.EventHandler(this.button_confirm_Click);
            // 
            // button_cancel
            // 
            this.button_cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_cancel.Location = new System.Drawing.Point(161, 332);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(75, 23);
            this.button_cancel.TabIndex = 2;
            this.button_cancel.Text = "Cancel";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // splitContainer_varControls
            // 
            this.splitContainer_varControls.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer_varControls.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer_varControls.Location = new System.Drawing.Point(12, 8);
            this.splitContainer_varControls.Name = "splitContainer_varControls";
            this.splitContainer_varControls.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer_varControls.Panel1
            // 
            this.splitContainer_varControls.Panel1.Controls.Add(this.treeView_variables);
            // 
            // splitContainer_varControls.Panel2
            // 
            this.splitContainer_varControls.Panel2.Controls.Add(this.label_expression);
            this.splitContainer_varControls.Panel2.Controls.Add(this.textBox_expression);
            this.splitContainer_varControls.Size = new System.Drawing.Size(260, 318);
            this.splitContainer_varControls.SplitterDistance = 281;
            this.splitContainer_varControls.TabIndex = 3;
            // 
            // textBox_expression
            // 
            this.textBox_expression.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_expression.Location = new System.Drawing.Point(81, 6);
            this.textBox_expression.Name = "textBox_expression";
            this.textBox_expression.Size = new System.Drawing.Size(176, 21);
            this.textBox_expression.TabIndex = 0;
            // 
            // label_expression
            // 
            this.label_expression.AutoSize = true;
            this.label_expression.Location = new System.Drawing.Point(4, 10);
            this.label_expression.Name = "label_expression";
            this.label_expression.Size = new System.Drawing.Size(71, 12);
            this.label_expression.TabIndex = 1;
            this.label_expression.Text = "Expression:";
            // 
            // VariableForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 363);
            this.Controls.Add(this.splitContainer_varControls);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_confirm);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "VariableForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Variables";
            this.Load += new System.EventHandler(this.VariableForm_Load);
            this.splitContainer_varControls.Panel1.ResumeLayout(false);
            this.splitContainer_varControls.Panel2.ResumeLayout(false);
            this.splitContainer_varControls.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_varControls)).EndInit();
            this.splitContainer_varControls.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeView_variables;
        private System.Windows.Forms.Button button_confirm;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.SplitContainer splitContainer_varControls;
        private System.Windows.Forms.Label label_expression;
        private System.Windows.Forms.TextBox textBox_expression;
    }
}