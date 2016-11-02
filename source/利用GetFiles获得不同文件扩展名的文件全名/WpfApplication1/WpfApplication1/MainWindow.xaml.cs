using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Net.Cache;
using Utility;

namespace WpfApplication1
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_DragEnter(object sender, DragEventArgs e)
        {
            e.Effects = e.Data.GetFormats().Contains("FileDrop") ? DragDropEffects.Copy : DragDropEffects.None;
        }

        private void Window_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetFormats().Contains("FileDrop"))
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();

                _count = 0;
                this.wrapPanel1.Children.Clear();
                var pathes = (string[])e.Data.GetData("FileDrop");
                foreach (var path in pathes)
                {
                    if (File.Exists(path))
                    {
                        LoadImageFromFile(path);
                    }
                    else if (Directory.Exists(path))
                    {
                        LoadImagesFromDirectory(path);
                    }
                }

                this.Title = string.Format("共 {0:#,##0} 张图像", _count);
            }
        }


        private void LoadImagesFromDirectory(string dir)
        {
            try
            {
                var files = GetFiles(dir, _exts, SearchOption.AllDirectories);

                foreach (var file in files)
                {
                    LoadImageFromFile(file);
                }
            }
            catch(Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("LoadImagesFromDirectory ERROR:"+ex.Message);
            }
        }

        string[] _exts = new[] { ".jpg", ".jpeg", ".bmp", ".png" };
        int _count = 0;
        private void LoadImageFromFile(string file)
        {
            var ext = System.IO.Path.GetExtension(file);
            if (_exts.Contains(ext, StringComparer.OrdinalIgnoreCase))
            {
                BitmapImage bitmapImage = null;
                try
                {
                    bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.DecodePixelWidth = 200;
                    bitmapImage.UriSource = new Uri(file);
                    bitmapImage.EndInit();
                }
                catch
                {
                    return;
                }

                this.wrapPanel1.Children.Add(new Image()
                {
                    Source = bitmapImage,
                    Margin = new Thickness(10),
                    MaxHeight = 200,
                    MaxWidth = 200,
                    Stretch = Stretch.Uniform
                });

                this.Title = string.Format("已加载 {0:#,##0} 张图像...", ++_count);
            }
        }


        private static IEnumerable<string> GetFiles(string sourceFolder, string[] exts, System.IO.SearchOption searchOption)
        {
            return System.IO.Directory.GetFiles(sourceFolder, "*.*", searchOption)
                    .Where(s => exts.Contains(System.IO.Path.GetExtension(s), StringComparer.OrdinalIgnoreCase));
        }

    }
}
