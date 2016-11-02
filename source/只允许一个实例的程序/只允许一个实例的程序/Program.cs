/* 
 * 标题：只允许运行一个实例的程序（互斥程序）示例
 * 原文：http://hi.baidu.com/wingingbob/blog/item/943f5f2396637846925807cf.html
 */

#define METHOD1   //方法一
//#define METHOD2   //方法二
//#define METHOD3   //方法三

using System;
using System.Text;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("===============");
            Console.WriteLine("互斥程序示例");
            Console.WriteLine("===============");


#if METHOD1     //方法一：同步基元
            bool runone;
            System.Threading.Mutex run = new System.Threading.Mutex(true, "只允许一个实例的程序_Bob", out runone);
            if (runone)
            {
                run.ReleaseMutex();
                MyMain();           //正常运行
            }
            else
            {
                PrintWarnning();    //输出警告
            }


#elif METHOD2   //方法二：判断进程

            System.Diagnostics.Process theProcess = null;
            System.Diagnostics.Process current = System.Diagnostics.Process.GetCurrentProcess();    //当前进程
            System.Diagnostics.Process[] processes = System.Diagnostics.Process.GetProcesses();     //所有进程
            var myFilename = System.Reflection.Assembly.GetExecutingAssembly().Location.Replace("/", @"\"); //得到当前进程名
            foreach (var process in processes)
            {
                //查找与当前进程相同名称的进程
                if (process.Id != current.Id && process.ProcessName == current.ProcessName)
                {
                    //确认相同名称进程的程序运行位置是否一样
                    try
                    {
                        if (myFilename == process.MainModule.FileName)
                        {
                            theProcess = process;    //找到了此程序的另一个进程
                        }
                    }
                    catch { }
                }
            }

            if (theProcess == null)
            {
                MyMain();           //正常运行
            }
            else
            {
                PrintWarnning();    //输出警告
            }

#elif METHOD3   //方法三：全局原子法
            if (NativeMethods.GlobalFindAtomW("只允许一个实例的程序_Bob") == 0) //没找到原子"只允许一个实例的程序"
            {
                var Atom = NativeMethods.GlobalAddAtomW("只允许一个实例的程序_Bob");  //添加原子"只允许一个实例的程序"
                MyMain();           //正常运行
                NativeMethods.GlobalDeleteAtom(Atom);   //一定记得程序退出时删除你的全局原子，否则只有在关机时它才会被清除。
            }
            else
            {
                //NativeMethods.GlobalDeleteAtom(NativeMethods.GlobalFindAtomW("只允许一个实例的程序_Bob"));
                PrintWarnning();    //输出警告
            }

#endif
        }

        /// <summary>
        /// 输出警告
        /// </summary>
        private static void PrintWarnning()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("警告：已经运行了一个实例了。");
            Console.ReadLine();
        }

        /// <summary>
        /// 正常运行
        /// </summary>
        private static void MyMain()
        {
            Console.WriteLine("正常运行...");
            Console.ReadLine();
        }



#if METHOD3     //方法三：全局原子法
        #region 查找原子

        public partial class NativeMethods
        {

            /// Return Type: ATOM->WORD->unsigned short
            ///lpString: LPCWSTR->WCHAR*
            [System.Runtime.InteropServices.DllImportAttribute("kernel32.dll", EntryPoint = "GlobalFindAtomW")]
            public static extern ushort GlobalFindAtomW([System.Runtime.InteropServices.InAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPWStr)] string lpString);


            /// Return Type: ATOM->WORD->unsigned short
            ///lpString: LPCSTR->CHAR*
            [System.Runtime.InteropServices.DllImportAttribute("kernel32.dll", EntryPoint = "GlobalFindAtomA")]
            public static extern ushort GlobalFindAtomA([System.Runtime.InteropServices.InAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string lpString);

        }
        #endregion

        #region 添加原子

        public partial class NativeMethods
        {

            /// Return Type: ATOM->WORD->unsigned short
            ///lpString: LPCWSTR->WCHAR*
            [System.Runtime.InteropServices.DllImportAttribute("kernel32.dll", EntryPoint = "GlobalAddAtomW")]
            public static extern ushort GlobalAddAtomW([System.Runtime.InteropServices.InAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPWStr)] string lpString);


            /// Return Type: ATOM->WORD->unsigned short
            ///lpString: LPCSTR->CHAR*
            [System.Runtime.InteropServices.DllImportAttribute("kernel32.dll", EntryPoint = "GlobalAddAtomA")]
            public static extern ushort GlobalAddAtomA([System.Runtime.InteropServices.InAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string lpString);

        }
        #endregion

        #region 删除原子
        public partial class NativeMethods
        {

            /// Return Type: ATOM->WORD->unsigned short
            ///nAtom: ATOM->WORD->unsigned short
            [System.Runtime.InteropServices.DllImportAttribute("kernel32.dll", EntryPoint = "GlobalDeleteAtom")]
            public static extern ushort GlobalDeleteAtom(ushort nAtom);

        }
        #endregion

#endif

    }
}