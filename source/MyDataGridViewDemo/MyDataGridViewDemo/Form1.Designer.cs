namespace MyDataGridViewDemo
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.button1 = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.wordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.excelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.记事本ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.访问博客ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.myDataGridView1 = new Oyi319.WinFroms.Controls.MyDataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.myDataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Location = new System.Drawing.Point(568, 12);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(265, 552);
            this.propertyGrid1.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 507);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 29);
            this.button1.TabIndex = 2;
            this.button1.Text = "刷新";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(113, 514);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(54, 16);
            this.checkBox1.TabIndex = 3;
            this.checkBox1.Text = "行号:";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(173, 513);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(120, 21);
            this.numericUpDown1.TabIndex = 4;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(379, 507);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 29);
            this.button2.TabIndex = 5;
            this.button2.Text = "复制";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(460, 507);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 29);
            this.button3.TabIndex = 6;
            this.button3.Text = "启动";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.wordToolStripMenuItem,
            this.excelToolStripMenuItem,
            this.记事本ToolStripMenuItem,
            this.toolStripMenuItem1,
            this.访问博客ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(134, 98);
            // 
            // wordToolStripMenuItem
            // 
            this.wordToolStripMenuItem.Name = "wordToolStripMenuItem";
            this.wordToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.wordToolStripMenuItem.Text = "Word";
            this.wordToolStripMenuItem.Click += new System.EventHandler(this.wordToolStripMenuItem_Click);
            // 
            // excelToolStripMenuItem
            // 
            this.excelToolStripMenuItem.Name = "excelToolStripMenuItem";
            this.excelToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.excelToolStripMenuItem.Text = "Excel";
            this.excelToolStripMenuItem.Click += new System.EventHandler(this.excelToolStripMenuItem_Click);
            // 
            // 记事本ToolStripMenuItem
            // 
            this.记事本ToolStripMenuItem.Name = "记事本ToolStripMenuItem";
            this.记事本ToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.记事本ToolStripMenuItem.Text = "记事本";
            this.记事本ToolStripMenuItem.Click += new System.EventHandler(this.记事本ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(130, 6);
            // 
            // 访问博客ToolStripMenuItem
            // 
            this.访问博客ToolStripMenuItem.Name = "访问博客ToolStripMenuItem";
            this.访问博客ToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.访问博客ToolStripMenuItem.Text = "访问博客...";
            this.访问博客ToolStripMenuItem.Click += new System.EventHandler(this.访问博客ToolStripMenuItem_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 552);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(479, 12);
            this.label1.TabIndex = 7;
            this.label1.Text = "说明：1) 设定行号后，执行刷新命令；2) 复制选定内容，启动到Excel或者Word，粘贴。";
            // 
            // myDataGridView1
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.myDataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.myDataGridView1.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            this.myDataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.myDataGridView1.Location = new System.Drawing.Point(12, 12);
            this.myDataGridView1.Name = "myDataGridView1";
            this.myDataGridView1.RowNumberEnabled = true;
            this.myDataGridView1.RowTemplate.Height = 23;
            this.myDataGridView1.Size = new System.Drawing.Size(550, 489);
            this.myDataGridView1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(845, 576);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.propertyGrid1);
            this.Controls.Add(this.myDataGridView1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "MyDataGridView 演示";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.myDataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Oyi319.WinFroms.Controls.MyDataGridView myDataGridView1;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem wordToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem excelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 记事本ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 访问博客ToolStripMenuItem;
        private System.Windows.Forms.Label label1;
    }
}

