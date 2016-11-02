namespace WindowsFormsApplication1
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.txtKeyword = new System.Windows.Forms.TextBox();
            this.richTextBox1 = new DoubleBufferRichTextBox();
            this.txtStatus = new System.Windows.Forms.TextBox();
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.cbComma = new System.Windows.Forms.CheckBox();
            this.cbWhole = new System.Windows.Forms.CheckBox();
            this.cbCase = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // txtKeyword
            // 
            this.txtKeyword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtKeyword.Location = new System.Drawing.Point(228, 197);
            this.txtKeyword.Name = "txtKeyword";
            this.txtKeyword.Size = new System.Drawing.Size(138, 21);
            this.txtKeyword.TabIndex = 3;
            this.txtKeyword.TextChanged += new System.EventHandler(this.txtKeyword_TextChanged);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox1.Location = new System.Drawing.Point(12, 12);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(576, 153);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // txtStatus
            // 
            this.txtStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtStatus.Location = new System.Drawing.Point(12, 242);
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.Size = new System.Drawing.Size(576, 21);
            this.txtStatus.TabIndex = 7;
            // 
            // btnLoad
            // 
            this.btnLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnLoad.Location = new System.Drawing.Point(13, 172);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(75, 23);
            this.btnLoad.TabIndex = 1;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.Location = new System.Drawing.Point(13, 195);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "*.rtf|*.rtf|all|*.*";
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "*.rtf|*.rtf|all|*.*";
            // 
            // txtResult
            // 
            this.txtResult.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtResult.Location = new System.Drawing.Point(12, 269);
            this.txtResult.Name = "txtResult";
            this.txtResult.Size = new System.Drawing.Size(576, 21);
            this.txtResult.TabIndex = 8;
            // 
            // cbComma
            // 
            this.cbComma.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cbComma.AutoSize = true;
            this.cbComma.Checked = true;
            this.cbComma.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbComma.Location = new System.Drawing.Point(447, 171);
            this.cbComma.Name = "cbComma";
            this.cbComma.Size = new System.Drawing.Size(132, 16);
            this.cbComma.TabIndex = 4;
            this.cbComma.Text = "英文逗号作为分隔符";
            this.cbComma.UseVisualStyleBackColor = true;
            this.cbComma.CheckedChanged += new System.EventHandler(this.CheckBoxControl_CheckedChanged);
            // 
            // cbWhole
            // 
            this.cbWhole.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cbWhole.AutoSize = true;
            this.cbWhole.Location = new System.Drawing.Point(447, 215);
            this.cbWhole.Name = "cbWhole";
            this.cbWhole.Size = new System.Drawing.Size(72, 16);
            this.cbWhole.TabIndex = 6;
            this.cbWhole.Text = "全字匹配";
            this.cbWhole.UseVisualStyleBackColor = true;
            this.cbWhole.CheckedChanged += new System.EventHandler(this.CheckBoxControl_CheckedChanged);
            // 
            // cbCase
            // 
            this.cbCase.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cbCase.AutoSize = true;
            this.cbCase.Checked = true;
            this.cbCase.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbCase.Location = new System.Drawing.Point(447, 193);
            this.cbCase.Name = "cbCase";
            this.cbCase.Size = new System.Drawing.Size(84, 16);
            this.cbCase.TabIndex = 5;
            this.cbCase.Text = "区分大小写";
            this.cbCase.UseVisualStyleBackColor = true;
            this.cbCase.CheckedChanged += new System.EventHandler(this.CheckBoxControl_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 298);
            this.Controls.Add(this.cbCase);
            this.Controls.Add(this.cbWhole);
            this.Controls.Add(this.cbComma);
            this.Controls.Add(this.txtResult);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.txtStatus);
            this.Controls.Add(this.txtKeyword);
            this.MinimumSize = new System.Drawing.Size(616, 336);
            this.Name = "Form1";
            this.Text = "StringSearch";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtKeyword;
        private DoubleBufferRichTextBox richTextBox1;
        private System.Windows.Forms.TextBox txtStatus;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.TextBox txtResult;
        private System.Windows.Forms.CheckBox cbComma;
        private System.Windows.Forms.CheckBox cbWhole;
        private System.Windows.Forms.CheckBox cbCase;
    }
}

