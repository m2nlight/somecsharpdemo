using System;
using System.IO;
using System.Reflection;
using System.Xml;

namespace ImageAndBase64
{
    /// <summary>
    /// 管理配置文件
    /// </summary>
    public class ConfigManager
    {
        string _configFileName;

        /// <summary>
        /// 配置文件名
        /// </summary>
        public string FileName { get { return _configFileName; } }

        /// <summary>
        /// 管理应用程序配置文件的类
        /// </summary>
        public ConfigManager()
        {
            _configFileName = Assembly.GetExecutingAssembly().Location + ".config";
        }

        /// <summary>
        /// 管理配置文件的类
        /// </summary>
        /// <param name="configfilename">配置文件名</param>
        public ConfigManager(string fileName)
        {
            try
            {
                FileInfo fi = new FileInfo(fileName);
                if (fi.Exists) _configFileName = fileName;
                else throw new ArgumentException("fileName");
            }
            catch
            {
                GC.SuppressFinalize(this);
                throw;
            }
        }

        /// <summary>
        /// 更新配置
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <returns>是否成功</returns>
        public bool UpdateConfig(string key, string value)
        {
            bool r = false;
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(_configFileName);
                XmlNode node = doc.SelectSingleNode(string.Format(@"//add[@key='{0}']", key));
                if (node != null)
                {
                    XmlElement ele = (XmlElement)node;
                    ele.SetAttribute("value", value);
                    doc.Save(_configFileName);
                    r = true;
                }
            }
            catch
            {
                throw;
            }
            return r;
        }

        /// <summary>
        /// 读取配置
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>值</returns>
        public string ReadConfig(string key)
        {
            string r = "";
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(_configFileName);
                XmlNode node = doc.SelectSingleNode(string.Format(@"//add[@key='{0}']", key));
                if (node != null)
                {
                    XmlElement ele = (XmlElement)node;
                    r = ele.GetAttribute("value");
                }
            }
            catch
            {
                throw;
            }
            return r;
        }
    }
}
