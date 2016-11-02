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
using System.Windows.Shapes;

namespace WpfWebBrowser
{
    /// <summary>
    /// Interaction logic for UriWindow.xaml
    /// </summary>
    public partial class UriWindow : Window
    {
        #region Uri

        /// <summary>
        /// Uri Dependency Property
        /// </summary>
        public static readonly DependencyProperty UriProperty =
            DependencyProperty.Register("Uri", typeof(Uri), typeof(UriWindow),
                new FrameworkPropertyMetadata(null));

        /// <summary>
        /// Gets or sets the Uri property.  This dependency property 
        /// indicates ....
        /// </summary>
        public Uri Uri
        {
            get { return (Uri)GetValue(UriProperty); }
            set { SetValue(UriProperty, value); }
        }

        #endregion        

        public UriWindow()
        {
            InitializeComponent();
        }

        private void btn_Ok_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
