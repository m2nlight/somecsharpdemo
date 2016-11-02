using System;
using System.IO;
using System.Windows.Forms;

namespace JsAndCsharp
{
    [System.Runtime.InteropServices.ComVisibleAttribute(true)]
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Text = @"JS与C#互操作 - WebBrowser";

            FileInfo file = new FileInfo("index.htm");
            webBrowser1.Url = new Uri(file.FullName);
            webBrowser1.ObjectForScripting = this;  //为Js提供访问的C#类，这个类要求ComVisibleAttribute(true)
        }

        private void button1_Click(object sender, EventArgs e)
        {
            object[] objects=new object[1];
            objects[0] = "C#访问JavaScript脚本";
            webBrowser1.Document.InvokeScript("messageBox", objects); //调用Js的messageBox方法
        }

        //Js将会调用此方法
        public void MyMessageBox(string message)
        {
            MessageBox.Show(message);
        }
    }
}
