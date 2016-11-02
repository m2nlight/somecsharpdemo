using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ListViewPictures
{
    public partial class ProgressForm : Form
    {
        public ProgressForm()
        {
            InitializeComponent();
        }

        internal void Pass(int i, int p)
        {
            label1.Text = string.Format("已经读取文件 {0}/{1} ...", i, p);
            progressBar1.Maximum = p;
            progressBar1.Value = i;
        }
    }
}
