namespace TestFlow.Software.WinformCommonOi.ValueInputOi
{
    partial class ValueInputOiForm
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
            this.tableLayoutPanel_configItems = new System.Windows.Forms.TableLayoutPanel();
            this.button_start = new System.Windows.Forms.Button();
            this.label_sequenceName = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // tableLayoutPanel_configItems
            // 
            this.tableLayoutPanel_configItems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel_configItems.ColumnCount = 2;
            this.tableLayoutPanel_configItems.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel_configItems.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel_configItems.Location = new System.Drawing.Point(12, 107);
            this.tableLayoutPanel_configItems.Name = "tableLayoutPanel_configItems";
            this.tableLayoutPanel_configItems.RowCount = 1;
            this.tableLayoutPanel_configItems.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel_configItems.Size = new System.Drawing.Size(700, 50);
            this.tableLayoutPanel_configItems.TabIndex = 0;
            // 
            // button_start
            // 
            this.button_start.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_start.Font = new System.Drawing.Font("SimHei", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_start.Location = new System.Drawing.Point(501, 170);
            this.button_start.Name = "button_start";
            this.button_start.Size = new System.Drawing.Size(113, 47);
            this.button_start.TabIndex = 2;
            this.button_start.Text = "Start";
            this.button_start.UseVisualStyleBackColor = true;
            this.button_start.Click += new System.EventHandler(this.button_start_Click);
            // 
            // label_sequenceName
            // 
            this.label_sequenceName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label_sequenceName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label_sequenceName.Font = new System.Drawing.Font("SimHei", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_sequenceName.Location = new System.Drawing.Point(12, 21);
            this.label_sequenceName.Name = "label_sequenceName";
            this.label_sequenceName.Size = new System.Drawing.Size(697, 58);
            this.label_sequenceName.TabIndex = 3;
            this.label_sequenceName.Text = "sequenceName";
            this.label_sequenceName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ValueInputOiForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(721, 229);
            this.Controls.Add(this.label_sequenceName);
            this.Controls.Add(this.button_start);
            this.Controls.Add(this.tableLayoutPanel_configItems);
            this.Name = "ValueInputOiForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ValueInputOiForm";
            this.Load += new System.EventHandler(this.ValueInputOiForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel_configItems;
        private System.Windows.Forms.Button button_start;
        private System.Windows.Forms.Label label_sequenceName;
    }
}