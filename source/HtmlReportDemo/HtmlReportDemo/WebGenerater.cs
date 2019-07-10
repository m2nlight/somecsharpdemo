using System;
using System.IO;
using HtmlReportDemo.Properties;

namespace HtmlReportDemo
{
    public static class WebGenerater
    {
        private const string WebFileDir = "web";
        private const string WebFileHtml = "report.html";
        private const string WebFileCss = "report.css";
        private const string WebFileJs = "report.js";

        private static string _html;
        private static string _css;
        private static string _js;

        public static string Generate()
        {
            string html = _html, css = _css, js = _js;
            if (string.IsNullOrEmpty(_html))
            {
                LoadTemplates(out html, out css, out js);
                _html = html;
                _css = css;
                _js = js;
            }

            var title = "Demo";
            var body = @"<a href=""http://blog.csdn.com/oyi319"" title=""·ÃÎÊ²©¿Í"">BLOG!</a>";

            css = string.Format("<style>{0}</style>", css);
            js = string.Format("<script>{0}</script>", js);
            return string.Format(html, title, css + js, body);
        }

        private static void LoadTemplates(out string html, out string css, out string js)
        {
            html = "";
            css = "";
            js = "";

            var path = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, WebFileDir);
            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch
                {
                    Console.WriteLine(Resources.WebGenerater_LoadTemplates_Create_Directory__0__is_fault_, path);
                    return;
                }
            }

            var htmlfile = Path.Combine(path, WebFileHtml);
            var cssfile = Path.Combine(path, WebFileCss);
            var jsfile = Path.Combine(path, WebFileJs);

            if (File.Exists(htmlfile))
            {
                try
                {
                    css = File.ReadAllText(htmlfile);
                }
                catch
                {
                    Console.WriteLine(Resources.WebGenerater_LoadTemplates_Load_html_file__0__is_fault_,htmlfile);
                }
            }

            if (File.Exists(cssfile))
            {
                try
                {
                    css = File.ReadAllText(cssfile);
                }
                catch
                {
                    Console.WriteLine(Resources.WebGenerater_LoadTemplates_Load_css_file__0__is_fault_, cssfile);
                }
            }

            if (File.Exists(jsfile))
            {
                try
                {
                    css = File.ReadAllText(jsfile);
                }
                catch
                {
                    Console.WriteLine(Resources.WebGenerater_LoadTemplates_Load_js_file__0__is_fault_, jsfile);
                }
            }

            if (string.IsNullOrEmpty(html)) html = Resources.report_html;
            if (string.IsNullOrEmpty(css)) css = Resources.report_css;
            if (string.IsNullOrEmpty(js)) js = Resources.report_js;
        }
    }
}