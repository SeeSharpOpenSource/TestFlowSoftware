namespace TestFlowExecutor
{
    partial class MainForm
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
            this.label_sequenceFile = new System.Windows.Forms.Label();
            this.button_assemblyFileSelect = new System.Windows.Forms.Button();
            this.textBox_assemblyPath = new System.Windows.Forms.TextBox();
            this.button_run = new System.Windows.Forms.Button();
            this.openFileDialog_panelSelector = new System.Windows.Forms.OpenFileDialog();
            this.textBox_output = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label_sequenceFile
            // 
            this.label_sequenceFile.AutoSize = true;
            this.label_sequenceFile.Location = new System.Drawing.Point(13, 27);
            this.label_sequenceFile.Name = "label_sequenceFile";
            this.label_sequenceFile.Size = new System.Drawing.Size(83, 12);
            this.label_sequenceFile.TabIndex = 0;
            this.label_sequenceFile.Text = "SequenceFile:";
            // 
            // button_assemblyFileSelect
            // 
            this.button_assemblyFileSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_assemblyFileSelect.Location = new System.Drawing.Point(524, 23);
            this.button_assemblyFileSelect.Name = "button_assemblyFileSelect";
            this.button_assemblyFileSelect.Size = new System.Drawing.Size(35, 23);
            this.button_assemblyFileSelect.TabIndex = 5;
            this.button_assemblyFileSelect.Text = "...";
            this.button_assemblyFileSelect.UseVisualStyleBackColor = true;
            this.button_assemblyFileSelect.Click += new System.EventHandler(this.button_assemblyFileSelect_Click);
            // 
            // textBox_assemblyPath
            // 
            this.textBox_assemblyPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_assemblyPath.Location = new System.Drawing.Point(99, 24);
            this.textBox_assemblyPath.Name = "textBox_assemblyPath";
            this.textBox_assemblyPath.ReadOnly = true;
            this.textBox_assemblyPath.Size = new System.Drawing.Size(419, 21);
            this.textBox_assemblyPath.TabIndex = 4;
            // 
            // button_run
            // 
            this.button_run.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_run.Location = new System.Drawing.Point(238, 202);
            this.button_run.Name = "button_run";
            this.button_run.Size = new System.Drawing.Size(75, 23);
            this.button_run.TabIndex = 6;
            this.button_run.Text = "Run";
            this.button_run.UseVisualStyleBackColor = true;
            this.button_run.Click += new System.EventHandler(this.button_run_Click);
            // 
            // openFileDialog_panelSelector
            // 
            this.openFileDialog_panelSelector.Filter = "sequence files|*.tfseq";
            // 
            // textBox_output
            // 
            this.textBox_output.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_output.Location = new System.Drawing.Point(15, 59);
            this.textBox_output.Multiline = true;
            this.textBox_output.Name = "textBox_output";
            this.textBox_output.ReadOnly = true;
            this.textBox_output.Size = new System.Drawing.Size(543, 137);
            this.textBox_output.TabIndex = 7;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(570, 235);
            this.Controls.Add(this.textBox_output);
            this.Controls.Add(this.button_run);
            this.Controls.Add(this.button_assemblyFileSelect);
            this.Controls.Add(this.textBox_assemblyPath);
            this.Controls.Add(this.label_sequenceFile);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TestFlow Executor";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_sequenceFile;
        private System.Windows.Forms.Button button_assemblyFileSelect;
        private System.Windows.Forms.TextBox textBox_assemblyPath;
        private System.Windows.Forms.Button button_run;
        private System.Windows.Forms.OpenFileDialog openFileDialog_panelSelector;
        private System.Windows.Forms.TextBox textBox_output;
    }
}

