using System;
using System.Windows;

namespace DriveDetectorDemo
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Console.Title = "Drive Detector";
            Console.WriteLine("WPF窗口出现后，插入或者弹出USB设备，WPF窗口会显示监听信息。");
            var win = new DebugWindow("Drive Detector (WPF)");
            win.Show();
            var dd = new DriveDetector();
            dd.DeviceArrived += (sender, e) => win.Write("Drive arrived {0}.", e.Drive);
            dd.DeviceRemoved += (sender, e) => win.Write("Drive removed {0}.", e.Drive);
            WpfInteropHelper.AddHook(win, dd.WndProc);
            var app = new Application();
            app.Run();
        }
    }
}
