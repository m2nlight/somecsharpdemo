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
using System.ComponentModel;
using System.Windows.Controls.Primitives;

namespace WpfWebBrowser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            foreach (var item in this.BrowserHost.Items)
            {
                var tabItem = item as TabItem;
                if (tabItem != null)
                {
                    var helperInstance = HelperRegistery.GetHelperInstance(tabItem);
                    if (helperInstance != null)
                    {
                        helperInstance.NewWindow -= WebBrowserOnNewWindow;
                        helperInstance.Disconnect();
                    }
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var uriWindow = new UriWindow();
            if ((bool)uriWindow.ShowDialog())
            {
                CreateNewTab(uriWindow.Uri);
            }
        }

        private void CreateNewTab(Uri uri)
        {
            var webBrowser = new WebBrowser { Source = uri };
            webBrowser.Navigated += new NavigatedEventHandler(WebBrowserOnNavigated);

            var tabItem = new TabItem { Content = webBrowser };

            var webBrowserHelper = new WebBrowserHelper(webBrowser);
            HelperRegistery.SetHelperInstance(tabItem, webBrowserHelper);
            webBrowserHelper.NewWindow += WebBrowserOnNewWindow;

            this.BrowserHost.Items.Add(tabItem);
            tabItem.IsSelected = true;
        }

        private void WebBrowserOnNavigated(object sender, NavigationEventArgs e)
        {
            dynamic browser = sender;
            browser.Parent.Header = browser.Document.IHTMLDocument2_title;
        }

        private void WebBrowserOnNewWindow(object sender, CancelEventArgs e)
        {
            dynamic browser = sender;
            dynamic activeElement = browser.Document.activeElement;
            var link = activeElement.ToString();
            // 这儿是在新窗口中打开，如果要在内部打开，改变当前browser的Source就行了
            CreateNewTab(new Uri(link));
            e.Cancel = true;
        }
    }
}
