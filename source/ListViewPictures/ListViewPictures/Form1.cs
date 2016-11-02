using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Runtime.Remoting.Messaging;
using System.Windows.Forms;

namespace ListViewPictures
{
    public partial class Form1 : Form
    {
        private void UpdateListView(Dictionary<string, Image> imageDic)
        {
            ListView lv = doubleBufferListView1;
            lv.Cursor = Cursors.WaitCursor;
            lv.BeginUpdate();
            lv.Items.Clear();
            lv.Groups.Clear();
            imageList1.Images.Clear();
            int imageListPointer = 0; //图像位置
            foreach (KeyValuePair<string, Image> pair in imageDic)
            {
                //向ImageList添加图像
                imageList1.Images.Add(pair.Value);
                pair.Value.Dispose();

                //向列表视添加项
                ListViewItem item = new ListViewItem(
                    Path.GetFileName(pair.Key),
                    imageListPointer++,
                    new ListViewGroup("图像列表")) { Tag = pair.Key };
                lv.Items.Add(item);
            }
            lv.EndUpdate();
            lv.Cursor = Cursors.Default;
        }

        //获得目录中图像
        private void GetImageFiles(string path)
        {
            AsyncCallback callBack = AsyncInvokeCallback;
            AsyncInvoke getImages = AsyncRefresh.GetImages;
            ProgressForm f = new ProgressForm();
            IAsyncResult asyncResult = getImages.BeginInvoke(path, f, callBack, f);
            f.ShowDialog();
            UpdateListView(asyn_dic);
        }

        //试着从文件载入图像
        private static bool TryLoadImageFromFile(string filename, out Image image)
        {
            bool r = false;
            image = null;
            try
            {
                Image img = Image.FromFile(filename);
                image = CreateThumbnail(
                            new Bitmap(img),
                            256,
                            256,
                            Brushes.Transparent);
                img.Dispose();
                r = true;
            }
            catch (OutOfMemoryException)
            {
                //图像格式不支持
            }
            catch (FileNotFoundException)
            {
                //文件不存在
            }
            catch (ArgumentException)
            {
                //参数 filename 有误
            }
            return r;
        }

        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="originalBmp">原始图像</param>
        /// <param name="desiredWidth">目标宽</param>
        /// <param name="desiredHeight">目标高</param>
        /// <param name="backgroundColor">空白的填充色</param>
        /// <returns></returns>
        public static Bitmap CreateThumbnail(Bitmap originalBmp, int desiredWidth, int desiredHeight, Brush backgroundColor)
        {
            //if (originalBmp.Width <= desiredWidth && originalBmp.Height <= desiredHeight)
            if (originalBmp.Width == desiredWidth && originalBmp.Height == desiredHeight)
            {
                return originalBmp;
            }

            int newWidth, newHeight;
            if (originalBmp.Width > desiredWidth || originalBmp.Height > desiredHeight)
            {
                // scale down the smaller dimension
                if (desiredWidth * originalBmp.Height < desiredHeight * originalBmp.Width)
                {
                    newWidth = desiredWidth;
                    newHeight = (int)Math.Round((decimal)originalBmp.Height * desiredWidth / originalBmp.Width);
                }
                else
                {
                    newHeight = desiredHeight;
                    newWidth = (int)Math.Round((decimal)originalBmp.Width * desiredHeight / originalBmp.Height);
                }
            }
            else
            {
                newWidth = originalBmp.Width;
                newHeight = originalBmp.Height;
            }
            
            int newX = (int)Math.Round((desiredWidth - newWidth) / 2.0);
            int newY = (int)Math.Round((desiredHeight - newHeight) / 2.0);

            Bitmap bmpOut = new Bitmap(desiredWidth, desiredHeight);

            using (Graphics graphics = Graphics.FromImage(bmpOut))
            {
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.FillRectangle(backgroundColor, 0, 0, desiredWidth, desiredHeight);
                //graphics.FillRectangle(Brushes.White, 0, 0, newWidth, newHeight);
                graphics.DrawImage(originalBmp, newX, newY, newWidth, newHeight);
            }

            return bmpOut;
        }

        #region 异步刷新

        static object syncLock = new object();
        Dictionary<string, Image> asyn_dic = null;

        private void AsyncInvokeCallback(IAsyncResult iresult)
        {
            AsyncResult ar = (AsyncResult)iresult;
            AsyncInvoke getImages = (AsyncInvoke)ar.AsyncDelegate;
            Form f = (Form)ar.AsyncState;
            asyn_dic = getImages.EndInvoke(ar); //保存结果
            BeginInvoke(new MethodInvoker(delegate
            {
                if (f != null && !f.IsDisposed) f.Dispose();
            }));
        }

        //用于异步刷新
        private delegate Dictionary<string, Image> AsyncInvoke(string path, ProgressForm f);
        public class AsyncRefresh
        {
            public static Dictionary<string, Image> GetImages(string path, ProgressForm f)
            {
                Dictionary<string, Image> dic = new Dictionary<string, Image>();
                DirectoryInfo di = null;
                try
                {
                    di = new DirectoryInfo(path);
                    FileInfo[] fiArray = di.GetFiles();
                    for (int i = 0; i < fiArray.Length; i++)
                    {
                        Image image;
                        if (TryLoadImageFromFile(fiArray[i].FullName, out image))
                        {
                            //加入图像
                            string key = fiArray[i].FullName;
                            dic[key] = image;
                        }
                        f.BeginInvoke(new MethodInvoker(delegate
                        {
                            f.Pass(i, fiArray.Length);
                        }));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "异常");
                }
                return dic;
            }
        }
        #endregion

        #region 无兴趣的

        public Form1()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            this.Text += " - 请在列表视中点击鼠标右键";
            this.BeginInvoke(new MethodInvoker(delegate
            {
                CommandBrowseFolder();
            }));
            base.OnLoad(e);
        }

        //protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        //{
        //    if (keyData.Equals(keyData & (Keys.Control | Keys.O)))
        //        CommandBrowseFolder();
        //    else if (keyData == Keys.F2)
        //        CommandExplorer();
        //    else if (keyData == Keys.F1)
        //        CommandNavigate();
        //    return base.ProcessCmdKey(ref msg, keyData);
        //}

        private void 选择文件夹FToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CommandBrowseFolder();
        }

        private void 打开所在文件夹ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CommandExplorer();
        }

        private void 转到博客ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CommandNavigate();
        }

        private void CommandNavigate()
        {
            System.Diagnostics.Process.Start("http://blog.csdn.net/oyi319/archive/2010/03/17/5390581.aspx");
        }

        private void CommandBrowseFolder()
        {
            DialogResult dr = folderBrowserDialog1.ShowDialog();
            if (DialogResult.OK != dr)
                return; //return
            CommandLoadImages(folderBrowserDialog1.SelectedPath);
        }

        private void CommandLoadImages(string path)
        {
            GetImageFiles(path);
        }

        private void CommandExplorer()
        {
            if (doubleBufferListView1.SelectedItems.Count > 0)
                System.Diagnostics.Process.Start(
                    Environment.GetEnvironmentVariable("SYSTEMROOT") + "\\Explorer.exe",
                    "/select," + doubleBufferListView1.SelectedItems[0].Tag.ToString());
        }

        #endregion
    }
}
