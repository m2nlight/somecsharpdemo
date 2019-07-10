namespace HtmlReportDemo
{
    partial class HtmlReportForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HtmlReportForm));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsSaveToFile = new System.Windows.Forms.ToolStripButton();
            this.tsPrintPreview = new System.Windows.Forms.ToolStripButton();
            this.tsPrint = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsZoomout = new System.Windows.Forms.ToolStripButton();
            this.tsZoomIn = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsClose = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tssStatusText = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.mnuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.保存到文件SToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiPrintSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiPrint = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiPrintPreview = new System.Windows.Forms.ToolStripMenuItem();
            this.tsSeprartor1 = new System.Windows.Forms.ToolStripSeparator();
            this.关闭XToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.toolStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsSaveToFile,
            this.tsPrintPreview,
            this.tsPrint,
            this.toolStripSeparator2,
            this.tsZoomout,
            this.tsZoomIn,
            this.toolStripSeparator1,
            this.tsClose});
            this.toolStrip1.Location = new System.Drawing.Point(0, 25);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(784, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsSaveToFile
            // 
            this.tsSaveToFile.Image = ((System.Drawing.Image)(resources.GetObject("tsSaveToFile.Image")));
            this.tsSaveToFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsSaveToFile.Name = "tsSaveToFile";
            this.tsSaveToFile.Size = new System.Drawing.Size(88, 22);
            this.tsSaveToFile.Text = "保存到文件";
            this.tsSaveToFile.Click += new System.EventHandler(this.TsSaveToFileClick);
            // 
            // tsPrintPreview
            // 
            this.tsPrintPreview.Image = ((System.Drawing.Image)(resources.GetObject("tsPrintPreview.Image")));
            this.tsPrintPreview.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsPrintPreview.Name = "tsPrintPreview";
            this.tsPrintPreview.Size = new System.Drawing.Size(76, 22);
            this.tsPrintPreview.Text = "打印预览";
            this.tsPrintPreview.Click += new System.EventHandler(this.TsPrintPreviewClick);
            // 
            // tsPrint
            // 
            this.tsPrint.Image = ((System.Drawing.Image)(resources.GetObject("tsPrint.Image")));
            this.tsPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsPrint.Name = "tsPrint";
            this.tsPrint.Size = new System.Drawing.Size(52, 22);
            this.tsPrint.Text = "打印";
            this.tsPrint.Click += new System.EventHandler(this.TsPrintClick);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // tsZoomout
            // 
            this.tsZoomout.Image = ((System.Drawing.Image)(resources.GetObject("tsZoomout.Image")));
            this.tsZoomout.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsZoomout.Name = "tsZoomout";
            this.tsZoomout.Size = new System.Drawing.Size(76, 22);
            this.tsZoomout.Text = "缩小文字";
            this.tsZoomout.Click += new System.EventHandler(this.TsZoomoutClick);
            // 
            // tsZoomIn
            // 
            this.tsZoomIn.Image = ((System.Drawing.Image)(resources.GetObject("tsZoomIn.Image")));
            this.tsZoomIn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsZoomIn.Name = "tsZoomIn";
            this.tsZoomIn.Size = new System.Drawing.Size(76, 22);
            this.tsZoomIn.Text = "放大文字";
            this.tsZoomIn.Click += new System.EventHandler(this.TsZoomInClick);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tsClose
            // 
            this.tsClose.Image = ((System.Drawing.Image)(resources.GetObject("tsClose.Image")));
            this.tsClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsClose.Name = "tsClose";
            this.tsClose.Size = new System.Drawing.Size(52, 22);
            this.tsClose.Text = "关闭";
            this.tsClose.Click += new System.EventHandler(this.TsCloseClick);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tssStatusText});
            this.statusStrip1.Location = new System.Drawing.Point(0, 540);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.ManagerRenderMode;
            this.statusStrip1.Size = new System.Drawing.Size(784, 22);
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tssStatusText
            // 
            this.tssStatusText.Name = "tssStatusText";
            this.tssStatusText.Size = new System.Drawing.Size(44, 17);
            this.tssStatusText.Text = "完成。";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFile});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(784, 25);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // mnuFile
            // 
            this.mnuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.保存到文件SToolStripMenuItem,
            this.toolStripMenuItem1,
            this.tsmiPrintSettings,
            this.tsmiPrint,
            this.tsmiPrintPreview,
            this.tsSeprartor1,
            this.关闭XToolStripMenuItem});
            this.mnuFile.Name = "mnuFile";
            this.mnuFile.Size = new System.Drawing.Size(58, 21);
            this.mnuFile.Text = "文件(&F)";
            // 
            // 保存到文件SToolStripMenuItem
            // 
            this.保存到文件SToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("保存到文件SToolStripMenuItem.Image")));
            this.保存到文件SToolStripMenuItem.Name = "保存到文件SToolStripMenuItem";
            this.保存到文件SToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.保存到文件SToolStripMenuItem.Text = "另存为(&A)...";
            this.保存到文件SToolStripMenuItem.Click += new System.EventHandler(this.保存到文件sToolStripMenuItemClick);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(149, 6);
            // 
            // tsmiPrintSettings
            // 
            this.tsmiPrintSettings.Image = ((System.Drawing.Image)(resources.GetObject("tsmiPrintSettings.Image")));
            this.tsmiPrintSettings.Name = "tsmiPrintSettings";
            this.tsmiPrintSettings.Size = new System.Drawing.Size(152, 22);
            this.tsmiPrintSettings.Text = "页面设置(&U)...";
            this.tsmiPrintSettings.Click += new System.EventHandler(this.页面设置ToolStripMenuItem_Click);
            // 
            // tsmiPrint
            // 
            this.tsmiPrint.Image = ((System.Drawing.Image)(resources.GetObject("tsmiPrint.Image")));
            this.tsmiPrint.Name = "tsmiPrint";
            this.tsmiPrint.Size = new System.Drawing.Size(152, 22);
            this.tsmiPrint.Text = "打印(&P)...";
            this.tsmiPrint.Click += new System.EventHandler(this.打印pToolStripMenuItemClick);
            // 
            // tsmiPrintPreview
            // 
            this.tsmiPrintPreview.Image = ((System.Drawing.Image)(resources.GetObject("tsmiPrintPreview.Image")));
            this.tsmiPrintPreview.Name = "tsmiPrintPreview";
            this.tsmiPrintPreview.Size = new System.Drawing.Size(152, 22);
            this.tsmiPrintPreview.Text = "打印预览(&V)...";
            this.tsmiPrintPreview.Click += new System.EventHandler(this.打印预览vToolStripMenuItemClick);
            // 
            // tsSeprartor1
            // 
            this.tsSeprartor1.Name = "tsSeprartor1";
            this.tsSeprartor1.Size = new System.Drawing.Size(149, 6);
            // 
            // 关闭XToolStripMenuItem
            // 
            this.关闭XToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("关闭XToolStripMenuItem.Image")));
            this.关闭XToolStripMenuItem.Name = "关闭XToolStripMenuItem";
            this.关闭XToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.关闭XToolStripMenuItem.Text = "关闭(&C)";
            this.关闭XToolStripMenuItem.Click += new System.EventHandler(this.关闭xToolStripMenuItemClick);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.webBrowser1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 50);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(784, 490);
            this.panel1.TabIndex = 3;
            // 
            // webBrowser1
            // 
            this.webBrowser1.AllowWebBrowserDrop = false;
            this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser1.IsWebBrowserContextMenuEnabled = false;
            this.webBrowser1.Location = new System.Drawing.Point(0, 0);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(782, 488);
            this.webBrowser1.TabIndex = 0;
            // 
            // HtmlReportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(400, 300);
            this.Name = "HtmlReportForm";
            this.Text = "查看报告";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsSaveToFile;
        private System.Windows.Forms.ToolStripButton tsPrintPreview;
        private System.Windows.Forms.ToolStripButton tsPrint;
        private System.Windows.Forms.ToolStripButton tsClose;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mnuFile;
        private System.Windows.Forms.ToolStripMenuItem 保存到文件SToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem tsmiPrintPreview;
        private System.Windows.Forms.ToolStripMenuItem tsmiPrint;
        private System.Windows.Forms.ToolStripSeparator tsSeprartor1;
        private System.Windows.Forms.ToolStripMenuItem 关闭XToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsmiPrintSettings;
        private System.Windows.Forms.ToolStripStatusLabel tssStatusText;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton tsZoomIn;
        private System.Windows.Forms.ToolStripButton tsZoomout;
    }
}