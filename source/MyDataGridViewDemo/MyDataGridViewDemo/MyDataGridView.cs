using System.ComponentModel;
using System.Drawing;
using System.Security.Permissions;
using System.Windows.Forms;

namespace Oyi319.WinFroms.Controls
{
    public class MyDataGridView : DataGridView
    {
        private int _rowNumberStart = 1;

        [DefaultValue(1), Description("显示起始行号")]
        public virtual int RowNumberStart
        {
            get { return _rowNumberStart; }
            set { _rowNumberStart = value; }
        }

        [DefaultValue(false), Description("是否显示行号")]
        public virtual bool RowNumberEnabled { get; set; }

        protected override void OnRowPostPaint(DataGridViewRowPostPaintEventArgs e)
        {
            if (RowNumberEnabled)
            {
                var rectangle = new Rectangle(e.RowBounds.Location.X,
                                              e.RowBounds.Location.Y,
                                              RowHeadersWidth - 4,
                                              e.RowBounds.Height);

                TextRenderer.DrawText(e.Graphics, (e.RowIndex + RowNumberStart).ToString(),
                                      RowHeadersDefaultCellStyle.Font,
                                      rectangle,
                                      RowHeadersDefaultCellStyle.ForeColor,
                                      TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
            }

            base.OnRowPostPaint(e);
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        protected override bool ProcessDataGridViewKey(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Insert:
                case Keys.C:
                    return ProcessInsertKey(e.KeyData);
                default:
                    break;
            }
            return base.ProcessDataGridViewKey(e);
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        private new bool ProcessInsertKey(Keys keyData)
        {
            if ((((keyData & (Keys.Alt | Keys.Control | Keys.Shift)) == Keys.Control) || (((keyData & (Keys.Alt | Keys.Control | Keys.Shift)) == (Keys.Control | Keys.Shift)) && ((keyData & Keys.KeyCode) == Keys.C))) && (ClipboardCopyMode != DataGridViewClipboardCopyMode.Disable))
            {
                var clipboardContent = GetClipboardContent();
                if (clipboardContent != null)
                {
                    CmdCopy();
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 复制选中内容
        /// </summary>
        public virtual void CmdCopy()
        {
            var clipboardContent = GetClipboardContent();
            if (clipboardContent != null)
            {
                Clipboard.SetText(clipboardContent.GetData(DataFormats.UnicodeText).ToString());
            }
        }

    }
}