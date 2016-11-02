using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Windows.Forms;

namespace ImageAndBase64
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Text += "          F1 - Blog";
            pictureBox1.BackgroundImageLayout = ImageLayout.Tile;
            pictureBox1.BackgroundImage = GraphicsUtility.DrawChessBoard(Brushes.Gray, Brushes.White, 10, 2);
        }

        private void DrawUi(string base64String, Image image)
        {
            richTextBox1.Text = base64String; //采用RichTextBox而不是TextBox，在执行此语句时RichTextBox更高效
            pictureBox1.Image = image;
            lblBase64Info.Text = string.Format("character count: {0:#,##0}", base64String.Length);
            lblImageInfo.Text = string.Format("{0} x {1} {2}", image.Width, image.Height, image.PixelFormat);
            CleanMemory(500); //500毫秒进行GC回收时间
        }

        private void btnImageOpen_Click(object sender, EventArgs e)
        {
            bool retry = false; //判定是否重新选择文件
            openFileDialog1.Title = "请选择一张图像";
            openFileDialog1.Filter = "*.jpg;*.bmp;*.png;*.gif|*.jpg;*.bmp;*.png;*.gif|所有文件|*.*";
            openFileDialog1.FileName = "";
            openFileDialog1.Multiselect = false;
        Label_Dialog:
            DialogResult dr = openFileDialog1.ShowDialog();
            if (DialogResult.Cancel == dr) return; //return            
            string fileName = openFileDialog1.FileName;
            try
            {
                Image image = Image.FromFile(fileName);
                ProgressForm f = new ProgressForm();
                string base64String = AsyncImageToBase64(image, ImageFormat.Jpeg); //以Jpeg格式转换图像
                if (base64String.Length == 0)
                {
                    if (DialogResult.Yes ==
                            MessageBox.Show("转换失败。是否选择另一个图像文件？",
                            this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Error))
                    {
                        retry = true;
                    }
                }
                else
                {
                    DrawUi(base64String, new Bitmap(image));
                    image.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: " + ex.Message);
            }
            if (retry)
            {
                retry = false;
                goto Label_Dialog;
            }
        }

        private void btnImageSave_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null)
            {
                MessageBox.Show("请先打开一幅图像。", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return; //return
            }
            saveFileDialog1.Title = "图像另存为";
            saveFileDialog1.Filter = "*.jpg|*.jpg|*.bmp|*.bmp|*.png|*.png|*.gif|*.gif";
            saveFileDialog1.FileName = "";
            DialogResult dr = saveFileDialog1.ShowDialog();
            if (DialogResult.Cancel == dr) return; //return
            try
            {
                //按指定格式保存图像
                ImageFormat imageFormat = ImageFormat.Jpeg;
                switch (Path.GetExtension(saveFileDialog1.FileName).ToLower())
                {
                    case ".bmp":
                        imageFormat = ImageFormat.Bmp;
                        break;
                    case ".png":
                        imageFormat = ImageFormat.Png;
                        break;
                    case ".gif":
                        imageFormat = ImageFormat.Gif;
                        break;
                    case ".jpg":
                    default:
                        break;
                }
                Bitmap bitmap = new Bitmap(pictureBox1.Image);
                bitmap.Save(saveFileDialog1.FileName, imageFormat);
                bitmap.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: " + ex.Message);
            }
        }

        private void btnBase64Open_Click(object sender, EventArgs e)
        {
            bool retry = false; //判定是否重新选择文件
            openFileDialog1.Title = "请选择一个内容为Base64字符串的文本文件";
            openFileDialog1.Filter = "*.txt|*.txt|所有文件|*.*";
            openFileDialog1.FileName = string.Empty;
            openFileDialog1.Multiselect = false;
        Label_Dialog:
            DialogResult dr = openFileDialog1.ShowDialog();
            if (DialogResult.Cancel == dr) return; //return
            try
            {
                string fileName = openFileDialog1.FileName;
                FileInfo fi = new FileInfo(fileName);
                ConfigManager cm = new ConfigManager();
                string maxSizeCfg = cm.ReadConfig("ReadTextFileMaxSize"); //从配置文件中读取最大文件大小限制配置信息
                long maxSize;
                if (!long.TryParse(maxSizeCfg, out maxSize)) maxSize = -1;
                if (fi.Length > maxSize && maxSize >= 0)
                {
                    MessageBox.Show(
                        string.Format("文件大小 {0:#,##0.00} KB 超过最大限制 {1:#,##0.00} KB。请重新选择。", fi.Length / 1024.0, maxSize / 1024.0),
                        this.Text,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    retry = true; //需要重新显示打开对话框
                }
                else
                {
                    string base64String = File.ReadAllText(fi.FullName, Encoding.UTF8);
                    Image image = AsyncBase64ToImage(base64String); //Base64转换成图像
                    if (image == null)
                    {
                        if (DialogResult.Yes ==
                            MessageBox.Show("转换失败。是否选择另一个文本文件？",
                            this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Error))
                        {
                            retry = true;
                        }
                    }
                    else
                    {
                        DrawUi(base64String, new Bitmap(image));
                        image.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: " + ex.Message);
            }
            if (retry)
            {
                retry = false;
                goto Label_Dialog;
            }
        }

        private void btnBase64Save_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Title = "将Base64编码另存为";
            saveFileDialog1.Filter = "*.txt|*.txt";
            saveFileDialog1.FileName = "";
            DialogResult dr = saveFileDialog1.ShowDialog();
            if (DialogResult.Cancel == dr) return; //return
            try
            {
                File.WriteAllText(saveFileDialog1.FileName, richTextBox1.Text, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: " + ex.Message);
            }
        }

        private void btnCleanMemory_Click(object sender, EventArgs e)
        {
            CleanMemory(System.Threading.Timeout.Infinite); //相当于传递参数为-1
        }

        private static void CleanMemory(int millisecondsTimeout)
        {
            GC.Collect();
            GC.Collect();
            GC.WaitForFullGCComplete(millisecondsTimeout);
        }


        #region 异步请求

        //static object syncLock = new object();
        static string syncBase64String = "";
        static Image syncImage = null;

        private Image AsyncBase64ToImage(string base64String)
        {
            AsyncCallback callBack = AsyncInvokeBase64ToImageCallback;
            AsyncInvokeBase64ToImage base64ToImage = AsyncInvoker.Base64ToImage;
            ProgressForm f = new ProgressForm();
            IAsyncResult asyncResult = base64ToImage.BeginInvoke(base64String, callBack, f);
            f.ShowDialog();
            return syncImage;
        }

        private string AsyncImageToBase64(Image image, ImageFormat format)
        {
            AsyncCallback callBack = AsyncInvokeImageToBase64Callback;
            AsyncInvokeImageToBase64 imageToBase64 = AsyncInvoker.ImageToBase64;
            ProgressForm f = new ProgressForm();
            IAsyncResult asyncResult = imageToBase64.BeginInvoke(image, format, callBack, f);
            f.ShowDialog();
            return syncBase64String;
        }

        private void AsyncInvokeImageToBase64Callback(IAsyncResult iresult)
        {
            AsyncResult ar = (AsyncResult)iresult;
            AsyncInvokeImageToBase64 imageToBase64 = (AsyncInvokeImageToBase64)ar.AsyncDelegate;
            Form f = (Form)ar.AsyncState;
            syncBase64String = imageToBase64.EndInvoke(ar); //保存结果
            BeginInvoke(new MethodInvoker(delegate
            {
                if (f != null && !f.IsDisposed) f.Dispose();
            }));
        }

        private void AsyncInvokeBase64ToImageCallback(IAsyncResult iresult)
        {
            AsyncResult ar = (AsyncResult)iresult;
            AsyncInvokeBase64ToImage base64ToImage = (AsyncInvokeBase64ToImage)ar.AsyncDelegate;
            Form f = (Form)ar.AsyncState;
            syncImage = base64ToImage.EndInvoke(ar); //保存结果
            BeginInvoke(new MethodInvoker(delegate
            {
                if (f != null && !f.IsDisposed) f.Dispose();
            }));
        }

        //用于异步刷新
        private delegate string AsyncInvokeImageToBase64(Image image, ImageFormat format);
        private delegate Image AsyncInvokeBase64ToImage(string base64String);

        public class AsyncInvoker
        {
            public static string ImageToBase64(Image image, ImageFormat format)
            {
                return GraphicsUtility.ImageToBase64(image, format);
            }

            public static Image Base64ToImage(string base64String)
            {
                return GraphicsUtility.Base64ToImage(base64String);
            }
        }
        #endregion

        #region 无兴趣的
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F1)
                System.Diagnostics.Process.Start("http://blog.csdn.net/oyi319/archive/2010/03/30/5433656.aspx");
            return base.ProcessCmdKey(ref msg, keyData);
        }
        #endregion
    }
}
