//~ CaptureDesktop代码摘自：http://social.msdn.microsoft.com/forums/en-US/wpf/thread/7a28f9e6-73fc-4157-bc40-1366267155c3
using System;

public class Program
{
	[STAThread]
	static void Main()
	{
		Console.WriteLine("桌面截图演示");
		Console.WriteLine("2秒后抓屏...");
		System.Threading.Thread.Sleep(2000);
		var image=CaptureDesktop();
		var filename=string.Format("capture_{0}.png",DateTime.Now.Ticks);
		var output=System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),filename);
		try
		{
			image.Save(output, System.Drawing.Imaging.ImageFormat.Png);
			Console.WriteLine("输出文件夹：桌面");
			Console.WriteLine("输出文件："+filename);
		}
		catch
		{
			Console.WriteLine("截图失败！");
		}
		image.Dispose(); //销毁位图
	}
	
	public static System.Drawing.Bitmap CaptureDesktop()
	{
		System.Drawing.Rectangle screenRect = System.Windows.Forms.SystemInformation.VirtualScreen;
		int screenWidth=screenRect.Width;
		int screenHeight=screenRect.Height;
		//~ WPF中通过SystemParameters得到屏幕宽高
		//~ var screenWidth = (int)System.Windows.SystemParameters.VirtualScreenWidth;
		//~ var screenHeight = (int)System.Windows.SystemParameters.VirtualScreenHeight;
		var desktopImage = new System.Drawing.Bitmap(screenWidth, screenHeight); //Bitmap默认采用PixelFormat.Format32bppArgb格式
		System.Drawing.Graphics deskGraphics;
		using(deskGraphics=System.Drawing.Graphics.FromImage(desktopImage))
		{
			IntPtr desktopHdc=deskGraphics.GetHdc();
			IntPtr hWnd=NativeMethods.GetDesktopWindow();
			IntPtr wndDc=NativeMethods.GetDC(hWnd);
			NativeMethods.BitBlt(desktopHdc, 0, 0, screenWidth, screenHeight, wndDc, 0, 0, NativeConstants.SRCCOPY | NativeConstants.CAPTUREBLT);
			//释放资源
			NativeMethods.ReleaseDC(hWnd, wndDc);
			deskGraphics.ReleaseHdc();
			desktopHdc = IntPtr.Zero;
			hWnd = IntPtr.Zero;
		}
		return desktopImage;
	}

}


[System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
public struct HWND__ {
    
    /// int
    public int unused;
}

[System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
public struct HDC__ {
    
    /// int
    public int unused;
}

public partial class NativeMethods {
    
    /// Return Type: hWnd->HWND__*
    [System.Runtime.InteropServices.DllImportAttribute("user32.dll", EntryPoint="GetDesktopWindow")]
    public static extern  System.IntPtr GetDesktopWindow() ;
	
	/// Return Type: HDC->HDC__*
    ///hWnd: hWnd->HWND__*
    [System.Runtime.InteropServices.DllImportAttribute("user32.dll", EntryPoint="GetDC")]
    public static extern  System.IntPtr GetDC([System.Runtime.InteropServices.InAttribute()] System.IntPtr hWnd) ;
	
	/// Return Type: BOOL->int
    ///hdc: HDC->HDC__*
    ///x: int
    ///y: int
    ///cx: int
    ///cy: int
    ///hdcSrc: HDC->HDC__*
    ///x1: int
    ///y1: int
    ///rop: DWORD->unsigned int
    [System.Runtime.InteropServices.DllImportAttribute("gdi32.dll", EntryPoint="BitBlt")]
    [return: System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)]
    public static extern  bool BitBlt([System.Runtime.InteropServices.InAttribute()] System.IntPtr hdc, int x, int y, int cx, int cy, [System.Runtime.InteropServices.InAttribute()] System.IntPtr hdcSrc, int x1, int y1, uint rop) ;
	
	/// Return Type: int
    ///hWnd: hWnd->HWND__*
    ///hDC: HDC->HDC__*
    [System.Runtime.InteropServices.DllImportAttribute("user32.dll", EntryPoint="ReleaseDC")]
    public static extern  int ReleaseDC([System.Runtime.InteropServices.InAttribute()] System.IntPtr hWnd, [System.Runtime.InteropServices.InAttribute()] System.IntPtr hDC) ;
}


public partial class NativeConstants 
{    
	//常量值参考WinGDI.h 或者 http://pinvoke.net/default.aspx/gdi32/BitBlt.html
	public const int SRCCOPY = 0x00CC0020;  //source
	public const int CAPTUREBLT = 0x40000000;  //Include layered windows  为了支持透明窗口
}
