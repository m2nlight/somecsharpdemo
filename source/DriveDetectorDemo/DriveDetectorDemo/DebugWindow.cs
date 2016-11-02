using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace DriveDetectorDemo
{
    public class DebugWindow : Window
    {
        private static readonly FontFamily MyFontFamily = new FontFamily("Lucida Console");
        private DateTime _last;
        private readonly StackPanel _stackOutput;
        private static readonly Brush BackgroundBrush = Brushes.White;
        private static readonly Brush AlternateBackgroundBrush = Brushes.AliceBlue;
        private Brush _lastBrush = BackgroundBrush;
        private const string TimeFormat = "yyyy-MM-dd H:mm:ss.fffffffzz";

        public DebugWindow(string title)
        {
            //Title = "##  Debug Panel  ##";
            Title = title;
            Topmost = true;
            Width = 600;
            Height = 300;
            ResizeMode = ResizeMode.CanResize;
            WindowStartupLocation = WindowStartupLocation.Manual;
            Left = SystemParameters.WorkArea.Right - SystemParameters.ResizeFrameVerticalBorderWidth * 2 - Width;
            Top = SystemParameters.WorkArea.Bottom - SystemParameters.ResizeFrameHorizontalBorderHeight * 2 - Height;

            FontFamily = MyFontFamily;

            var grid = new Grid();
            Content = grid;

            var rowdef = new RowDefinition { Height = GridLength.Auto };
            grid.RowDefinitions.Add(rowdef);

            rowdef = new RowDefinition { Height = new GridLength(100, GridUnitType.Star) };
            grid.RowDefinitions.Add(rowdef);

            var textHeadings = new TextBlock
            {
                FontFamily = MyFontFamily,
                Inlines =
                                           {
                                               new Underline(
                                                   new Run(string.Format("{0} at {1}",title,DateTime.Now)))
                                           }
            };
            grid.Children.Add(textHeadings);
            Grid.SetRow(textHeadings, 0);

            var scroll = new ScrollViewer();
            grid.Children.Add(scroll);
            Grid.SetRow(scroll, 1);

            _stackOutput = new StackPanel();
            scroll.Content = _stackOutput;
        }

        //        private string TypeWithoutNamespace(object obj)
        //        {
        //            string[] astr = obj.GetType().ToString().Split('.');
        //            return astr[astr.Length - 1];
        //        }

        public void Write(string format, params object[] objects)
        {
            Write(string.Format(format, objects));
        }

        public void Write(string text)
        {
            DateTime now = DateTime.Now;
            if (now - _last > TimeSpan.FromMilliseconds(100))
                _stackOutput.Children.Add(new TextBlock(new Run(now.ToString(TimeFormat)))
                {
                    Background = Brushes.CadetBlue,
                    Foreground = Brushes.White,
                    FontFamily = FontFamily
                });
            _last = now;

            var textBlock = new TextBlock
            {
                FontFamily = FontFamily,
                Text = text,
                Background =
                    _last == now && _lastBrush == BackgroundBrush
                        ? _lastBrush = AlternateBackgroundBrush
                        : _lastBrush = BackgroundBrush,
            };
            _stackOutput.Children.Add(textBlock);
            ((ScrollViewer)_stackOutput.Parent).ScrollToBottom();
        }

        /*protected override void OnClosed(EventArgs e)
        {
            if (MessageBox.Show("是否退出调试？", Title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
            base.OnClosed(e);
        }*/
    }
}
