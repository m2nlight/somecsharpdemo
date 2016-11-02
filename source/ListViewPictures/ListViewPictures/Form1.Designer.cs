namespace ListViewPictures
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
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.选择文件夹FToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.转到博客ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.doubleBufferListView1 = new ListViewPictures.DoubleBufferListView();
            this.打开所在文件夹ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(256, 256);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.选择文件夹FToolStripMenuItem,
            this.打开所在文件夹ToolStripMenuItem,
            this.toolStripMenuItem1,
            this.转到博客ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(207, 98);
            // 
            // 选择文件夹FToolStripMenuItem
            // 
            this.选择文件夹FToolStripMenuItem.Name = "选择文件夹FToolStripMenuItem";
            this.选择文件夹FToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.选择文件夹FToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            this.选择文件夹FToolStripMenuItem.Text = "选择文件夹(&F)...";
            this.选择文件夹FToolStripMenuItem.Click += new System.EventHandler(this.选择文件夹FToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(203, 6);
            // 
            // 转到博客ToolStripMenuItem
            // 
            this.转到博客ToolStripMenuItem.Name = "转到博客ToolStripMenuItem";
            this.转到博客ToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.转到博客ToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            this.转到博客ToolStripMenuItem.Text = "转到博客";
            this.转到博客ToolStripMenuItem.Click += new System.EventHandler(this.转到博客ToolStripMenuItem_Click);
            // 
            // folderBrowserDialog1
            // 
            this.folderBrowserDialog1.Description = "请选择一个相册文件夹";
            this.folderBrowserDialog1.ShowNewFolderButton = false;
            // 
            // doubleBufferListView1
            // 
            this.doubleBufferListView1.BackColor = System.Drawing.Color.White;
            this.doubleBufferListView1.ContextMenuStrip = this.contextMenuStrip1;
            this.doubleBufferListView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.doubleBufferListView1.LargeImageList = this.imageList1;
            this.doubleBufferListView1.Location = new System.Drawing.Point(0, 0);
            this.doubleBufferListView1.Name = "doubleBufferListView1";
            this.doubleBufferListView1.Size = new System.Drawing.Size(624, 442);
            this.doubleBufferListView1.TabIndex = 0;
            this.doubleBufferListView1.UseCompatibleStateImageBehavior = false;
            // 
            // 打开所在文件夹ToolStripMenuItem
            // 
            this.打开所在文件夹ToolStripMenuItem.Name = "打开所在文件夹ToolStripMenuItem";
            this.打开所在文件夹ToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F2;
            this.打开所在文件夹ToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            this.打开所在文件夹ToolStripMenuItem.Text = "打开所在文件夹";
            this.打开所在文件夹ToolStripMenuItem.Click += new System.EventHandler(this.打开所在文件夹ToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 442);
            this.Controls.Add(this.doubleBufferListView1);
            this.Name = "Form1";
            this.Text = "ListViewPictures";
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DoubleBufferListView doubleBufferListView1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 选择文件夹FToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 转到博客ToolStripMenuItem;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.ToolStripMenuItem 打开所在文件夹ToolStripMenuItem;
    }
}

