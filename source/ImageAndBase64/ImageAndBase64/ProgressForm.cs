using System.Windows.Forms;

namespace ImageAndBase64
{
    public partial class ProgressForm : Form
    {
        public ProgressForm()
        {
            InitializeComponent();

            progressBar1.MarqueeAnimationSpeed = 50;
            progressBar1.Style = ProgressBarStyle.Marquee;
            label1.Text = "正在转换...";
        }

        //internal void Pass(int i, int p)
        //{
        //    label1.Text = string.Format("已经读取文件 {0}/{1} ...", i, p);
        //    progressBar1.Maximum = p;
        //    progressBar1.Value = i;
        //}
    }
}
