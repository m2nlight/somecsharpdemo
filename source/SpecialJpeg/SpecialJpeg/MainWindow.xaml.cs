//此例子解决WPF无法载入的JPEG文件时，是如何利用GDI+的方法转换为WPF的ImageSource的
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DragDropEffects = System.Windows.DragDropEffects;
using DragEventArgs = System.Windows.DragEventArgs;
using Image = System.Windows.Controls.Image;

namespace SpecialJpeg
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //image1.MouseEnter += (sender, e) => { this.LayoutRoot.Background = Brushes.Blue; };
            //image1.MouseLeave += (sender, e) => { this.LayoutRoot.Background = Brushes.Beige; };
        }

        private void Image1DragEnter(object sender, DragEventArgs e)
        {
            e.Effects = e.Data.GetFormats().Contains("FileDrop") ? DragDropEffects.Copy : DragDropEffects.None;
        }

        private void Image1Drop(object sender, DragEventArgs e)
        {
            DropFile(image1, e);
            LoadBackground(image1);

        }

        private void Image2Drop(object sender, DragEventArgs e)
        {
            DropFileSpecialJpeg(pictureBox1, e);
            LoadBackground(pictureBox1);
        }

        private void LoadBackground(System.Windows.Forms.PictureBox pictureBox)
        {
            //方法一：通过GDI+保存为文件，再由WPF打开的办法
            if (pictureBox.Image != null)
            {
                var image = pictureBox.Image;
                var tempName = Path.GetTempFileName();
                image.Save(tempName);
                this.LayoutRoot.Background =
                    new ImageBrush(
                        BitmapFrame.Create(new Uri(tempName),
                                           BitmapCreateOptions.PreservePixelFormat,
                                           BitmapCacheOption.OnLoad))
                        {
                            Opacity = 0.4,
                            Stretch = Stretch.Uniform
                        };
                File.Delete(tempName);
            }
            else
            {
                this.LayoutRoot.Background = null;
            }
        }

        private void LoadBackground(Image image)
        {
            this.LayoutRoot.Background = image.Source != null
                                             ? new ImageBrush(image.Source) { Opacity = 0.4, Stretch = Stretch.Uniform }
                                             : null;
        }

        private void DropFile(Image image, DragEventArgs e)
        {
            if (e.Data.GetFormats().Contains("FileDrop"))
            {
                var pathes = (string[])e.Data.GetData("FileDrop");
                var path = pathes.FirstOrDefault(File.Exists);
                if (path != null)
                {
                    try
                    {
                        var bitmap = BitmapFrame.Create(new Uri(path));
                        bitmap.Freeze();
                        image.Source = bitmap;
                        textBox1.Visibility = Visibility.Hidden;
                    }
                    catch (Exception ex)
                    {
                        textBox1.Text = "L:" + ex.Message;
                        textBox1.Visibility = Visibility.Visible;

                        var imageSource = GetImageSourceByGdi(path);
                        image.Source = imageSource;
                    }
                }
            }
        }

        public BitmapFrame GetImageSourceByGdi(string path)
        {
            //方法二：通过GDI+保存为内存流，再由WPF打开的办法
            try
            {
                using (var bitmap = new System.Drawing.Bitmap(path))
                using (var ms = new MemoryStream())
                {
                    bitmap.Save(ms, bitmap.RawFormat);
                    ms.Seek(0, SeekOrigin.Begin);
                    var bitmapFrame = BitmapFrame.Create(ms, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
                    bitmapFrame.Freeze();
                    return bitmapFrame;
                }
            }
            catch
            {
                return null;
            }
        }

        private void DropFileSpecialJpeg(System.Windows.Forms.PictureBox pictureBox, DragEventArgs e)
        {
            if (e.Data.GetFormats().Contains("FileDrop"))
            {
                var pathes = (string[])e.Data.GetData("FileDrop");
                var path = pathes.FirstOrDefault(File.Exists);
                if (path != null)
                {
                    try
                    {
                        var bitmap = new System.Drawing.Bitmap(path);
                        if (pictureBox.Image != null)
                        {
                            pictureBox.Image.Dispose();
                        }
                        pictureBox.Image = bitmap;
                        textBox1.Visibility = Visibility.Hidden;
                    }
                    catch (Exception ex)
                    {
                        textBox1.Text = "R:" + ex.Message;
                        textBox1.Visibility = Visibility.Visible;
                    }
                }
            }

        }

    }
}
