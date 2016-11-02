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

namespace PaintPath
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Window1 : Window
    {
        Path _Path;
        Random rand;
        public Window1()
        {
            InitializeComponent();
            rand = new Random();

        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            lineArea.Children.Clear();
            _Path = null;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            DrawASegment();
        }

        private void DrawASegment()
        {
            if (_Path == null)
            {
                _Path = new Path()
                {
                    Stroke = Brushes.DeepSkyBlue,
                    StrokeThickness = 2,
                    StrokeDashArray = new DoubleCollection(new double[] { 1, 1, 1 })
                };
            }
            var path = _Path;

            if (path.Data == null) path.Data = new PathGeometry();
            var pathGeometry = path.Data as PathGeometry;

            var figures = pathGeometry.Figures;
            PathFigure figure = null;
            if (figures.Count == 0)
            {
                figure = new PathFigure();
                figure.StartPoint = new Point(0, 0);
                figures.Add(figure);
            }
            else
            {
                figure = figures.Last();
            }

            var segments = figure.Segments;
            LineSegment segment = null;
            Point lastLocation = new Point(0, 0);
            if (segments.Count > 0)
            {
                segment = segments.Last() as LineSegment;
            }
            lastLocation = segment != null ? segment.Point : lastLocation;
            var ri = rand.Next(10);
            var newLocation = new Point(lastLocation.X + 20, ri <= 7 ? lastLocation.Y + 30 : lastLocation.Y - 30);

            LineSegment newsegment = new LineSegment(newLocation, true);
            segments.Add(newsegment);

            if (!lineArea.Children.Contains(path))
            {
                lineArea.Children.Add(path);
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            RemoveLastSegment();
        }

        private void RemoveLastSegment()
        {
            if (_Path != null)
            {
                var path = _Path;
                if (path.Data != null)
                {
                    var pathGeometry = path.Data as PathGeometry;
                    if (pathGeometry.Figures.Count > 0)
                    {
                        var figure = pathGeometry.Figures.Last();
                        if (figure.Segments.Count > 0)
                        {
                            var segment = figure.Segments.Last();
                            figure.Segments.Remove(segment);
                        }
                    }
                }
            }
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 10; i++)
            {
                DrawASegment();
            }
        }

        private void button5_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 10; i++)
            {
                RemoveLastSegment();
            }
        }
    }
}


//<Path Stroke="Black" StrokeThickness="1">
//  <Path.Data>
//    <PathGeometry>
//      <PathFigure StartPoint="10,50">
//        <LineSegment Point="200,70" />
//      </PathFigure>
//    </PathGeometry>
//  </Path.Data>
//</Path>