using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public class DoubleBufferRichTextBox : RichTextBox
    {
        public DoubleBufferRichTextBox()
        {
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            UpdateStyles();
        }
    }
}
