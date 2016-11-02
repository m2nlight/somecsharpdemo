using System;
using System.Drawing;
using System.Windows.Forms;
using EeekSoft.Text;
using System.Text;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            #region immaturity
            cbWhole.Enabled = false;
            #endregion
        }

        protected override void OnLoad(EventArgs e)
        {
            richTextBox1.Rtf = WindowsFormsApplication1.Properties.Resources.rtf;
            this.WindowState = FormWindowState.Maximized;
            txtKeyword.Text = "text,keyword";
            txtKeyword.Focus();
            base.OnLoad(e);
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == openFileDialog1.ShowDialog())
            {
                try
                {
                    richTextBox1.LoadFile(openFileDialog1.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == saveFileDialog1.ShowDialog())
            {
                try
                {
                    richTextBox1.SaveFile(saveFileDialog1.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void CheckBoxControl_CheckedChanged(object sender, EventArgs e)
        {
            DoInquiry();
        }

        private void txtKeyword_TextChanged(object sender, EventArgs e)
        {
            DoInquiry();
        }

        private void DoInquiry()
        {
            txtResult.Text = "";
            int count = 0;
            string[] keywords = GetKeywords();

            if (keywords.Length == 0)
            {
                richTextBox1.SelectAll();
                richTextBox1.SelectionBackColor = richTextBox1.BackColor;
                richTextBox1.SelectionLength = 0;
            }
            else
            {
                StringSearch ss = new StringSearch(keywords, !cbCase.Checked);
                StringSearchResult[] ssr = ss.FindAll(richTextBox1.Text);
                richTextBox1.SelectAll();
                richTextBox1.SelectionBackColor = richTextBox1.BackColor;
                StringBuilder sb = new StringBuilder();
                foreach (StringSearchResult sr in ssr)
                {
                    sb.AppendFormat("{{{0}:{1}}} ", sr.Index, sr.Keyword);
                    richTextBox1.Select(sr.Index, sr.Keyword.Length);
                    richTextBox1.SelectionBackColor = Color.Yellow;
                }
                txtResult.Text = sb.ToString();
                count = ssr.Length;
            }
            txtStatus.Text = string.Format("共找到 {0} 个匹配的文本", count);
        }

        private string[] GetKeywords()
        {
            string[] keywords = new string[0];
            if (txtKeyword.Text.Length > 0)
            {
                if (cbComma.Checked)
                {
                    keywords = txtKeyword.Text.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                }
                else
                {
                    keywords = new string[] { txtKeyword.Text };
                }
            }
            return keywords;
        }

    }
}
