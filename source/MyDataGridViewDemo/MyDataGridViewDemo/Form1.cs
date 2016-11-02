using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace MyDataGridViewDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitUi();
        }

        protected override void OnLoad(EventArgs e)
        {
            Bind();
            myDataGridView1.AutoResizeColumns();            
            base.OnLoad(e);
        }

        private void InitUi()
        {
            propertyGrid1.SelectedObject = myDataGridView1;
            numericUpDown1.DataBindings.Add(new Binding("Value", myDataGridView1, "RowNumberStart"));
            numericUpDown1.DataBindings.Add(new Binding("Enabled", checkBox1, "Checked"));
            checkBox1.DataBindings.Add(new Binding("Checked", myDataGridView1, "RowNumberEnabled"));
        }

        private void Bind()
        {
            var dataset = new DataSet();
            dataset.ReadXml("XMLFile1.xml");
            myDataGridView1.DataSource = dataset.Tables[0];
        }

        private void button1_Click(object sender, EventArgs e)
        {
            myDataGridView1.Refresh();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            myDataGridView1.CmdCopy();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            contextMenuStrip1.Show(button3, new Point(0, button3.Height));
        }

        private void wordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Start("winword");
        }

        private void excelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Start("excel");
        }

        private void 记事本ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Start("notepad");
        }

        private void 访问博客ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Start("http://blog.csdn.net/oyi319/archive/2010/05/26/5625074.aspx");
        }

        private static void Start(string fileName)
        {
            try
            {
                Process.Start(fileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, @"启动失败", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }
    }
}
