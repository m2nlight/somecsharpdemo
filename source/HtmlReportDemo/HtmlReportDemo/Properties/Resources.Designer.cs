﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:2.0.50727.4927
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace HtmlReportDemo.Properties {
    using System;
    
    
    /// <summary>
    ///   强类型资源类，用于查找本地化字符串等。
    /// </summary>
    // 此类是由 StronglyTypedResourceBuilder
    // 类通过类似于 ResGen 或 Visual Studio 的工具自动生成的。
    // 若要添加或移除成员，请编辑 .ResX 文件，然后重新运行 ResGen
    // (以 /str 作为命令选项)，或重新生成 VS 项目。
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   返回此类使用的缓存 ResourceManager 实例。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("HtmlReportDemo.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   为使用此强类型资源类的所有资源查找
        ///   重写当前线程的 CurrentUICulture 属性。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   查找类似 
        ////*
        ///body{font-size: 14px;line-height:1.5em;}
        ///table{
        ///  	width:90%;
        ///	margin-top: 10px;
        ///	margin-right: auto;
        ///	margin-bottom: 0px;
        ///	margin-left: auto;
        ///}
        ///body{background-color:#D5DFE9;}
        ///table{border-spacing:1px; border:1px solid #A2C0DA;}
        ///td, th{padding:2px 5px;border-collapse:collapse;text-align:left; font-weight:normal;}
        ///thead tr th{background:#B0D1FC;border:1px solid white;font-weight:bold;}
        ///thead tr th.line1{background:#D3E5FD;}
        ///thead tr th.line4{background:#C6C6C6;}
        ///tbody tr td{height:auto;b [字符串的其余部分被截断]&quot;; 的本地化字符串。
        /// </summary>
        internal static string report_css {
            get {
                return ResourceManager.GetString("report_css", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 &lt;!DOCTYPE html PUBLIC &quot;-//W3C//DTD XHTML 1.0 Transitional//EN&quot; &quot;http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd&quot;&gt;
        ///&lt;html xmlns=&quot;http://www.w3.org/1999/xhtml&quot;&gt;
        ///&lt;head&gt;
        ///&lt;meta http-equiv=&quot;Content-Type&quot; content=&quot;text/html; charset=utf-8&quot; /&gt;
        ///&lt;title&gt;{0}&lt;/title&gt;
        ///{1}
        ///&lt;/head&gt;
        ///&lt;body&gt;
        ///{2}
        ///&lt;/body&gt;
        ///&lt;/html&gt;
        /// 的本地化字符串。
        /// </summary>
        internal static string report_html {
            get {
                return ResourceManager.GetString("report_html", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 
        ///function zoomIn(){
        ///    var zoom = document.body.style.zoom ? parseFloat(document.body.style.zoom) : 1;
        ///    if (zoom &lt; 4.0) zoom = zoom + 0.1;
        ///    document.body.style.zoom = zoom;
        ///}
        ///function zoomOut(){
        ///    var zoom = document.body.style.zoom ? parseFloat(document.body.style.zoom) : 1;
        ///    if (zoom &gt; 0.2) zoom = zoom - 0.1;
        ///    document.body.style.zoom = zoom;
        ///}
        /// 的本地化字符串。
        /// </summary>
        internal static string report_js {
            get {
                return ResourceManager.GetString("report_js", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Create directory {0} is fault. 的本地化字符串。
        /// </summary>
        internal static string WebGenerater_LoadTemplates_Create_Directory__0__is_fault_ {
            get {
                return ResourceManager.GetString("WebGenerater_LoadTemplates_Create_Directory__0__is_fault_", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Load css file {0} is fault. 的本地化字符串。
        /// </summary>
        internal static string WebGenerater_LoadTemplates_Load_css_file__0__is_fault_ {
            get {
                return ResourceManager.GetString("WebGenerater_LoadTemplates_Load_css_file__0__is_fault_", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Load html file {0} is fault. 的本地化字符串。
        /// </summary>
        internal static string WebGenerater_LoadTemplates_Load_html_file__0__is_fault_ {
            get {
                return ResourceManager.GetString("WebGenerater_LoadTemplates_Load_html_file__0__is_fault_", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Load js file {0} is fault. 的本地化字符串。
        /// </summary>
        internal static string WebGenerater_LoadTemplates_Load_js_file__0__is_fault_ {
            get {
                return ResourceManager.GetString("WebGenerater_LoadTemplates_Load_js_file__0__is_fault_", resourceCulture);
            }
        }
    }
}