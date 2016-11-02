using System;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            var theProcess = GetRunningProcess();   //得到已经运行的进程
            if (theProcess != null)
            {
                //应该程序已经运行，将其主窗口置前
                NativeMethods.ShowWindowAsync(theProcess.MainWindowHandle, 1);
                NativeMethods.SetForegroundWindow(theProcess.MainWindowHandle);
            }
            else
            {
                //正常运行程序
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            }
        }

        /// <summary>
        /// 得到已经运行的进程
        /// </summary>
        /// <returns></returns>
        static System.Diagnostics.Process GetRunningProcess()
        {
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

            return theProcess;
        }

        #region PInvoke 代码
        [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public struct HWND__
        {

            /// int
            public int unused;
        }

        public partial class NativeMethods
        {

            /// Return Type: BOOL->int
            ///hWnd: HWND->HWND__*
            ///nCmdShow: int
            [System.Runtime.InteropServices.DllImportAttribute("user32.dll", EntryPoint = "ShowWindowAsync")]
            [return: System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)]
            public static extern bool ShowWindowAsync([System.Runtime.InteropServices.InAttribute()] System.IntPtr hWnd, int nCmdShow);


            /// Return Type: BOOL->int
            ///hWnd: HWND->HWND__*
            [System.Runtime.InteropServices.DllImportAttribute("user32.dll", EntryPoint = "SetForegroundWindow")]
            [return: System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)]
            public static extern bool SetForegroundWindow([System.Runtime.InteropServices.InAttribute()] System.IntPtr hWnd);

        }
        #endregion
    }
}
