using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;

namespace ListViewFileIcon
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            //启用排序功能
            _listViewSorter = new ListViewColumnSorter();
            SwitchListViewSort(true);
            this.listView1.ColumnClick += new ColumnClickEventHandler(ListViewHelper.ListView_ColumnClick);
            ListViewHelper.ListView_ColumnClick(this.listView1, new ColumnClickEventArgs(0));   //默认按第一列排序
        }

        //获取信息按钮
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string[] exts = textBox1.Text.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                GetIcons(exts);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }
        }

        /// <summary>
        /// 通过扩展名得到图标和描述
        /// </summary>
        /// <param name="ext">扩展名(如“.txt”)</param>
        /// <param name="largeIcon">得到大图标</param>
        /// <param name="smallIcon">得到小图标</param>
        /// <param name="description">得到类型描述或者空字符串</param>
        /// <param name="mime">MIME信息</param>
        void GetExtsIconAndDescription(string ext, out Icon largeIcon, out Icon smallIcon, out string description, out string mime)
        {
            GetDefaultIcon(out largeIcon, out smallIcon);   //得到缺省图标
            description = "";                               //缺省类型描述
            mime = "";                                      //缺省MIME信息
            RegistryKey extsubkey = Registry.ClassesRoot.OpenSubKey(ext);   //从注册表中读取扩展名相应的子键
            if (extsubkey == null) return;

            var extdefaultvalue = extsubkey.GetValue(null) as string;     //取出扩展名对应的文件类型名称
            if (extdefaultvalue == null) return;

            mime = extsubkey.GetValue("Content Type") as string ?? string.Empty;//得到MIME信息

            if (extdefaultvalue.Equals("exefile", StringComparison.OrdinalIgnoreCase))  //扩展名类型是可执行文件
            {
                RegistryKey exefilesubkey = Registry.ClassesRoot.OpenSubKey(extdefaultvalue);  //从注册表中读取文件类型名称的相应子键
                if (exefilesubkey != null)
                {
                    string exefiledescription = exefilesubkey.GetValue(null) as string;   //得到exefile描述字符串
                    if (exefiledescription != null) description = exefiledescription;
                }
                System.IntPtr exefilePhiconLarge = new IntPtr();
                System.IntPtr exefilePhiconSmall = new IntPtr();
                NativeMethods.ExtractIconExW(Path.Combine(Environment.SystemDirectory, "shell32.dll"), 2, ref exefilePhiconLarge, ref exefilePhiconSmall, 1);
                if (exefilePhiconLarge.ToInt32() > 0) largeIcon = Icon.FromHandle(exefilePhiconLarge);
                if (exefilePhiconSmall.ToInt32() > 0) smallIcon = Icon.FromHandle(exefilePhiconSmall);
                return;
            }

            RegistryKey typesubkey = Registry.ClassesRoot.OpenSubKey(extdefaultvalue);  //从注册表中读取文件类型名称的相应子键
            if (typesubkey == null) return;

            string typedescription = typesubkey.GetValue(null) as string;   //得到类型描述字符串
            if (typedescription != null) description = typedescription;

            RegistryKey defaulticonsubkey = typesubkey.OpenSubKey("DefaultIcon");  //取默认图标子键
            if (defaulticonsubkey == null) return;

            //得到图标来源字符串
            string defaulticon = defaulticonsubkey.GetValue(null) as string; //取出默认图标来源字符串
            if (defaulticon == null) return;
            string[] iconstringArray = defaulticon.Split(',');
            int nIconIndex = 0; //声明并初始化图标索引
            if (iconstringArray.Length > 1)
                if (!int.TryParse(iconstringArray[1], out nIconIndex))
                    nIconIndex = 0;     //int.TryParse转换失败，返回0

            //得到图标
            System.IntPtr phiconLarge = new IntPtr();
            System.IntPtr phiconSmall = new IntPtr();
            NativeMethods.ExtractIconExW(iconstringArray[0].Trim('"'), nIconIndex, ref phiconLarge, ref phiconSmall, 1);
            if (phiconLarge.ToInt32() > 0) largeIcon = Icon.FromHandle(phiconLarge);
            if (phiconSmall.ToInt32() > 0) smallIcon = Icon.FromHandle(phiconSmall);
        }

        /// <summary>
        /// 获取缺省图标
        /// </summary>
        /// <param name="largeIcon"></param>
        /// <param name="smallIcon"></param>
        private static void GetDefaultIcon(out Icon largeIcon, out Icon smallIcon)
        {
            largeIcon = smallIcon = null;
            System.IntPtr phiconLarge = new IntPtr();
            System.IntPtr phiconSmall = new IntPtr();
            NativeMethods.ExtractIconExW(Path.Combine(Environment.SystemDirectory, "shell32.dll"), 0, ref phiconLarge, ref phiconSmall, 1);
            if (phiconLarge.ToInt32() > 0) largeIcon = Icon.FromHandle(phiconLarge);
            if (phiconSmall.ToInt32() > 0) smallIcon = Icon.FromHandle(phiconSmall);
        }

        void GetIcons(string[] exts)
        {
            //listView1.BeginUpdate();
            listView1.Items.Clear();
            foreach (string ext in exts)
            {
                AddLvItem(ext);
            }
            //listView1.EndUpdate();
        }

        private void listView1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right) contextMenuStrip1.Show(listView1, e.Location);
        }

        private void 大图标ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView1.View = View.LargeIcon;
        }

        private void 小图标ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView1.View = View.SmallIcon;
        }

        private void 列表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView1.View = View.List;
        }

        private void 详细信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView1.View = View.Details;
        }

        private void 平铺ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView1.View = View.Tile;
        }

        private void 访问网站ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NavigateBlog();
        }

        //快捷键支持
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData.Equals(Keys.Control | Keys.A))
            {
                AllSelected();
            }
            else if (keyData == Keys.Delete)
            {
                Remove();
            }
            else if (keyData == Keys.F2)
            {
                Rename();
            }
            else if (keyData == Keys.F1)
            {
                NavigateBlog();
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void NavigateBlog()
        {
            System.Diagnostics.Process.Start("http://blog.csdn.net/oyi319/archive/2010/03/22/5404722.aspx");
        }

        //全选
        private void AllSelected()
        {
            listView1.BeginUpdate();
            foreach (ListViewItem item in listView1.Items)
            {
                item.Selected = true;
            }
            listView1.EndUpdate();
        }

        //开关排序
        System.Collections.IComparer _listViewSorter;
        private void SwitchListViewSort(bool isSorted)
        {
            if (isSorted)
            {
                listView1.ListViewItemSorter = _listViewSorter;
            }
            else
            {
                _listViewSorter = listView1.ListViewItemSorter;   //临时关闭排序功能
                listView1.ListViewItemSorter = null;
            }
        }

        //添加条目
        private void AddLvItem(string ext)
        {
            SwitchListViewSort(false);
            string suffix = ext.Trim();
            if (suffix.Length == 0) return; //跳过空字符串
            string prefix = suffix[0] != '.' ? "." : string.Empty;
            string fixExt = prefix + suffix;
            Icon largeIcon, smallIcon;
            string description;
            string mime;
            GetExtsIconAndDescription(fixExt, out largeIcon, out smallIcon, out description,out mime);
            if (smallIcon != null) imageList1.Images.Add(fixExt, smallIcon);
            if (largeIcon != null) imageList2.Images.Add(fixExt, largeIcon);
            ListViewItem lvItem = listView1.Items.Add(fixExt, fixExt, fixExt);
            //if (string.IsNullOrEmpty(description)) lvItem.SubItems.Add("<未知类型>");
            //else lvItem.SubItems.Add(description);
            lvItem.SubItems.Add(description);
            lvItem.SubItems.Add(mime);
            SwitchListViewSort(true);
        }

        //删除条目
        private void RemoveLvItem(string key)
        {
            SwitchListViewSort(false);
            listView1.Items.RemoveByKey(key);
            imageList1.Images.RemoveByKey(key);
            imageList2.Images.RemoveByKey(key);
            SwitchListViewSort(true);
        }

        /// <summary>
        /// 删除
        /// </summary>
        private void Remove()
        {
            List<string> exts = GetSelectedItems();
            if (exts.Count == 0)
            {
                MessageBox.Show("请选择需要删除的条目。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }

            string text, caption;
            if (exts.Count == 1)
            {
                text = string.Format("确实要删除「{0}」吗？", Path.GetFileName(exts[0]));
                caption = "确认条目删除";
            }
            else
            {
                text = string.Format("确实要删除 {0} 个条目吗？", exts.Count);
                caption = "确认删除多个条目";
            }

            if (DialogResult.OK != MessageBox.Show(text, caption, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation)) return;

            foreach (var ext in exts)
            {
                string key = ext;
                RemoveLvItem(key);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private List<string> GetSelectedItems()
        {
            List<string> r = new List<string>();
            foreach (ListViewItem item in listView1.SelectedItems)
            {
                r.Add(item.Text);
            }
            return r;
        }

        /// <summary>
        /// 重命名
        /// </summary>
        private void Rename()
        {
            if (listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("请选择一个需要重命名的条目。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }
            ListViewItem item = listView1.SelectedItems[0];
            item.BeginEdit();
        }

        string _orgExt; //用于重命名

        private void listView1_BeforeLabelEdit(object sender, LabelEditEventArgs e)
        {
            _orgExt = ((ListView)sender).Items[e.Item].Text;
        }

        private void listView1_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            var newName = e.Label;
            if (string.IsNullOrEmpty(e.Label))
            {
                e.CancelEdit = true;
                return;
            }
            if (!newName.StartsWith(".")) //扩展名以.开头
            {
                newName = "." + newName;
            }

            if (((ListView)sender).Items.Find(newName, false).Length > 0)
            {
                e.CancelEdit = true;
                MessageBox.Show("已经存在此名称，请重新命名。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }

            SwitchListViewSort(false);
            //更新Tag信息、图标、说明信息和MIME信息
            ListViewItem item = ((ListView)sender).Items[e.Item];
            item.Name = newName; //新key
            Icon largeicon, smallicon;
            string desc;
            string mime;
            GetExtsIconAndDescription(Path.GetExtension(newName), out largeicon, out smallicon, out desc,out mime);
            imageList1.Images.Add(newName, smallicon);
            imageList2.Images.Add(newName, largeicon);
            item.ImageKey = newName;
            item.SubItems[1].Text = desc;
            item.SubItems[2].Text = mime;
            imageList1.Images.RemoveByKey(_orgExt);
            imageList2.Images.RemoveByKey(_orgExt);
            e.CancelEdit = true;
            item.Text = newName;
            SwitchListViewSort(true);
            item.EnsureVisible(); //使其可见
        }

        //重命名
        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Rename();
        }

        //删除
        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            Remove();
        }


    }


    //----以下是Pinvoke生成代码----

    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public struct HICON__
    {

        /// int
        public int unused;
    }

    public partial class NativeMethods
    {

        /// Return Type: UINT->unsigned int
        ///lpszFile: LPCWSTR->WCHAR*
        ///nIconIndex: int
        ///phiconLarge: HICON*
        ///phiconSmall: HICON*
        ///nIcons: UINT->unsigned int
        [System.Runtime.InteropServices.DllImportAttribute("shell32.dll", EntryPoint = "ExtractIconExW", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern uint ExtractIconExW([System.Runtime.InteropServices.InAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPWStr)] string lpszFile, int nIconIndex, ref System.IntPtr phiconLarge, ref System.IntPtr phiconSmall, uint nIcons);

    }

}
