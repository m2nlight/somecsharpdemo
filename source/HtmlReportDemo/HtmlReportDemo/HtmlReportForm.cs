using System;
using System.Windows.Forms;

namespace HtmlReportDemo
{
    public partial class HtmlReportForm : Form
    {
        private bool _supportPrintFunction = true;

        /// <summary>
        /// 加载Uri，否则加载DocumentText
        /// </summary>
        public bool IsUriOrDocumentText { get; set; }
        /// <summary>
        /// 是否开启打印功能
        /// </summary>
        public bool SupportPrintFunction
        {
            get
            {
                return _supportPrintFunction;
            }
            set
            {
                tsPrint.Visible = value;
                tsPrintPreview.Visible = value;
                tsSeprartor1.Visible = value;
                tsmiPrint.Visible = value;
                tsmiPrintPreview.Visible = value;
                tsmiPrintSettings.Visible = value;
                _supportPrintFunction = value;
            }
        }

        public Uri Uri { get; set; }
        public string DocumentText { get; set; }

        #region ctors
        public HtmlReportForm(Uri uri)
            : this(uri, null, true)
        {
        }

        public HtmlReportForm(string documentText)
            : this(null, documentText, false)
        {
        }

        public HtmlReportForm(Uri uri, string documentText, bool isUriOrDocumentText)
            : this()
        {
            Uri = uri;
            DocumentText = documentText;
            IsUriOrDocumentText = isUriOrDocumentText;
        }

        public HtmlReportForm(Uri uri, string documentText, bool isUriOrDocumentText, bool supportPrintFunction)
            : this(uri, documentText, isUriOrDocumentText)
        {
            SupportPrintFunction = supportPrintFunction;
        }

        private HtmlReportForm()
        {
            InitializeComponent();
            webBrowser1.StatusTextChanged += WebBrowser1StatusTextChanged;
        }
        #endregion

        #region UI
        protected override void OnLoad(EventArgs e)
        {
            LoadUriOrDocumentText();
            base.OnLoad(e);
        }

        void WebBrowser1StatusTextChanged(object sender, EventArgs e)
        {
            string statusText = ((WebBrowser)sender).StatusText;
            tssStatusText.Text = string.IsNullOrEmpty(statusText) ? "完成" : statusText;
        }

        private void 保存到文件sToolStripMenuItemClick(object sender, EventArgs e)
        {
            CmdSaveToFile();
        }

        private void 打印预览vToolStripMenuItemClick(object sender, EventArgs e)
        {
            CmdPrintPreview();
        }

        private void 打印pToolStripMenuItemClick(object sender, EventArgs e)
        {
            CmdPrint(true);
        }

        private void 页面设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CmdPageSetup();
        }

        private void 关闭xToolStripMenuItemClick(object sender, EventArgs e)
        {
            CmdClose();
        }

        private void TsSaveToFileClick(object sender, EventArgs e)
        {
            CmdSaveToFile();
        }

        private void TsPrintPreviewClick(object sender, EventArgs e)
        {
            CmdPrintPreview();
        }

        private void TsPrintClick(object sender, EventArgs e)
        {
            CmdPrint(false);
        }

        private void TsCloseClick(object sender, EventArgs e)
        {
            CmdClose();
        }
        #endregion

        #region Commands
        private void CmdClose()
        {
            Close();
        }

        private void CmdSaveToFile()
        {
            webBrowser1.ShowSaveAsDialog();
        }

        private void CmdPrintPreview()
        {
            webBrowser1.ShowPrintPreviewDialog();
        }

        private void CmdPrint(bool showPrintDialog)
        {
            if (showPrintDialog)
            {
                webBrowser1.ShowPrintDialog();
                return;
            }
            webBrowser1.Print();
        }

        private void CmdPageSetup()
        {
            webBrowser1.ShowPageSetupDialog();
        }
        #endregion

        /// <summary>
        /// 根据LoadUriOrDocumentText选择加载Uri或者HTML文档
        /// </summary>
        private void LoadUriOrDocumentText()
        {
            if (IsUriOrDocumentText)
            {
                webBrowser1.Navigate(Uri);
            }
            else
            {
                webBrowser1.DocumentText = DocumentText;
            }
        }

        private void TsZoomInClick(object sender, EventArgs e)
        {
            if (webBrowser1.Document != null) webBrowser1.Document.InvokeScript("zoomIn");
        }

        private void TsZoomoutClick(object sender, EventArgs e)
        {
            if (webBrowser1.Document != null) webBrowser1.Document.InvokeScript("zoomOut");
        }
    }
}
