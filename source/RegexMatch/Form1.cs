using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace RegexMatch
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void textBoxControl_TextChanged(object sender, EventArgs e)
        {
            string[] values = null;
            try
            {
                values = RegexUtility.RegexSearch(textBox1.Text, textBox2.Text).ToArray();
                if (values.Length > 0)
                {
                    if (textBox3.ForeColor != Color.Green) textBox3.ForeColor = Color.Green;
                    textBox3.Text = "成功匹配 " + values.Length + " 项";
                }
                else
                {
                    if (textBox3.ForeColor != Color.Red) textBox3.ForeColor = Color.Red;
                    textBox3.Text = "失败";
                }
            }
            catch (Exception ex)
            {
                if (textBox3.ForeColor != Color.Purple) textBox3.ForeColor = Color.Purple;
                textBox3.Text = "错误：" + ex.Message;
            }
            if (values != null)
            {
                listBox1.BeginUpdate();
                listBox1.Items.Clear();
                listBox1.Items.AddRange(values);
                listBox1.EndUpdate();
            }
        }
    }
}
