using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace WpfPlayGif
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            grid.Children.Add(
                new OldGifImage(new Uri("pack://application:,,/Resources/alarm.gif")));

            grid1.Children.Add(
                new GifImage(new Uri("pack://application:,,/Resources/alarm.gif")));

        }

        private int _current;
        private readonly Uri[] _gifImageUri = {
                                         new Uri("pack://application:,,/Resources/alarm.gif"),
                                         new Uri("pack://application:,,/Resources/animated.gif")
                                     };

        private void GifImage_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _current = (_current + 1) % _gifImageUri.Length;
            var uri = _gifImageUri[_current];
            LoadGifFile(uri);
        }

        private OpenFileDialog _openFileDialog;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (_openFileDialog == null)
            {
                _openFileDialog = new OpenFileDialog { Filter = "*.gif|*.gif", InitialDirectory = AppDomain.CurrentDomain.BaseDirectory };
            }

            if (_openFileDialog.ShowDialog() == true)
            {
                var uri = new Uri(_openFileDialog.FileName);
                LoadGifFile(uri);
            }
        }

        private void LoadGifFile(Uri uri)
        {
            gifImage.GifImageUri = uri;

            wrapPanel.Children.Clear();
            var d = new GifBitmapDecoder(uri, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
            var i = 0;
            foreach (var bitmapFrame in d.Frames)
            {
                bitmapFrame.Freeze();
                var image = new Image { Source = bitmapFrame, Height = 100, Width = 100 };
                var textblock = new TextBlock { Text = (i++).ToString(CultureInfo.InvariantCulture) };
                var stackPanel = new StackPanel { Orientation = Orientation.Vertical };
                stackPanel.Children.Add(image);
                stackPanel.Children.Add(textblock);
                wrapPanel.Children.Add(stackPanel);
            }
        }
    }
}
