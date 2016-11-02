using System;
using System.Windows;
using System.Windows.Interop;

namespace DriveDetectorDemo
{
    /// <summary>
    /// 支持Windows消息
    /// </summary>
    public static class WpfInteropHelper
    {
        /// <summary>
        /// 获得某窗口句柄
        /// </summary>
        /// <param name="window"></param>
        /// <returns></returns>
        public static IntPtr GetHwnd(Window window)
        {
            var interopHelper = new WindowInteropHelper(window);
            return interopHelper.Handle;
        }

        /// <summary>
        /// 在所有者窗口显示对话框
        /// </summary>
        /// <param name="dialog"></param>
        /// <param name="owner"></param>
        /// <returns></returns>
        public static bool? ShowDialog(Window dialog, Window owner)
        {
            var ownerHwnd = GetHwnd(owner);
            return ShowDialog(dialog, ownerHwnd);
        }

        /// <summary>
        /// 在所有者窗口显示对话框
        /// </summary>
        /// <param name="dialog"></param>
        /// <param name="owner"></param>
        /// <returns></returns>
        public static bool? ShowDialog(Window dialog, IntPtr owner)
        {
            var interopHelper = new WindowInteropHelper(dialog);
            interopHelper.Owner = owner;
            return dialog.ShowDialog();
        }

        /// <summary>
        /// 增加消息处理
        /// </summary>
        /// <param name="window">窗口</param>
        /// <param name="hook">HwndSourceHook委托</param>
        public static void AddHook(Window window, HwndSourceHook hook)
        {
            var hwnd = GetHwnd(window);
            var hwndSource = HwndSource.FromHwnd(hwnd);
            if (hwndSource == null) throw new ArgumentException("window");
            hwndSource.AddHook(hook);
        }
    }
}
