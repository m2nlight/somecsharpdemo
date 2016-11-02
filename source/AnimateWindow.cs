using System;
using System.Windows.Forms;

static class MyExtendMethod
{
	/// <summary>
	/// 窗口的淡入效果
	/// </summary>
	/// <param name="form"></param>
	/// <returns></returns>
	public static bool FadeIn(this System.Windows.Forms.Form form)
	{
		return GuiUtility.AnimateWindow(
			form.Handle,
			500,
			GuiUtility.AnimateWindowStyle.FadeIn);
	}

	/// <summary>
	/// 窗口的淡出效果
	/// </summary>
	/// <param name="form"></param>
	/// <returns></returns>
	public static bool FadeOut(this System.Windows.Forms.Form form)
	{
		return GuiUtility.AnimateWindow(
			form.Handle,
			500,
			GuiUtility.AnimateWindowStyle.FadeOut);
	}
}

static class GuiUtility
{
	public static bool AnimateWindow(IntPtr handle, uint time, AnimateWindowStyle style)
	{
		uint flags = 0;
		switch (style)
		{
			case AnimateWindowStyle.None:
				return false;
			case AnimateWindowStyle.SlideInUpToDown:
				flags = NativeConstants.AW_SLIDE + NativeConstants.AW_VER_POSITIVE + NativeConstants.AW_ACTIVATE;
				break;
			case AnimateWindowStyle.SlideInDownToUp:
				flags = NativeConstants.AW_SLIDE + NativeConstants.AW_VER_NEGATIVE + NativeConstants.AW_ACTIVATE;
				break;
			case AnimateWindowStyle.SlideInLeftToRight:
				flags = NativeConstants.AW_SLIDE + NativeConstants.AW_HOR_POSITIVE + NativeConstants.AW_ACTIVATE;
				break;
			case AnimateWindowStyle.SlideInRightToLeft:
				flags = NativeConstants.AW_SLIDE + NativeConstants.AW_HOR_NEGATIVE + NativeConstants.AW_ACTIVATE;
				break;
			case AnimateWindowStyle.SlideOutUpToDown:
				flags = NativeConstants.AW_SLIDE + NativeConstants.AW_VER_POSITIVE + NativeConstants.AW_HIDE;
				break;
			case AnimateWindowStyle.SlideOutDownToUp:
				flags = NativeConstants.AW_SLIDE + NativeConstants.AW_VER_NEGATIVE + NativeConstants.AW_HIDE;
				break;
			case AnimateWindowStyle.SlideOutLeftToRight:
				flags = NativeConstants.AW_SLIDE + NativeConstants.AW_HOR_POSITIVE + NativeConstants.AW_HIDE;
				break;
			case AnimateWindowStyle.SlideOutRightToLeft:
				flags = NativeConstants.AW_SLIDE + NativeConstants.AW_HOR_NEGATIVE + NativeConstants.AW_HIDE;
				break;
			case AnimateWindowStyle.FadeIn:
				flags = NativeConstants.AW_BLEND + NativeConstants.AW_ACTIVATE;
				break;
			case AnimateWindowStyle.FadeOut:
				flags = NativeConstants.AW_BLEND + NativeConstants.AW_HIDE;
				break;
			case AnimateWindowStyle.CenterIn:
				flags = NativeConstants.AW_CENTER + NativeConstants.AW_ACTIVATE;
				break;
			case AnimateWindowStyle.CenterOut:
				flags = NativeConstants.AW_CENTER + NativeConstants.AW_HIDE;
				break;
		}
		return NativeMethods.AnimateWindow(handle, time, flags);
	}

	/// <summary>
	/// 动画窗口风格
	/// </summary>
	public enum AnimateWindowStyle
	{
		None = 0,               //无
		SlideInUpToDown,        //由上向下滑入
		SlideInDownToUp,        //由下向上滑入
		SlideInLeftToRight,     //由左向右滑入
		SlideInRightToLeft,     //由右向左滑入
		SlideOutUpToDown,       //由上向下滑出
		SlideOutDownToUp,       //由下向上滑出
		SlideOutLeftToRight,    //由左向右滑出
		SlideOutRightToLeft,    //由右向左滑出
		FadeIn,                 //淡入
		FadeOut,                //淡出
		CenterIn,               //从中心扩展
		CenterOut               //向中心收缩
	}
}

public partial class NativeMethods
{
	/// Return Type: BOOL->int
	///hWnd: HWND->HWND__*
	///dwTime: DWORD->unsigned int
	///dwFlags: DWORD->unsigned int
	[System.Runtime.InteropServices.DllImportAttribute("user32.dll", EntryPoint = "AnimateWindow")]
	[return: System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)]
	public static extern bool AnimateWindow([System.Runtime.InteropServices.InAttribute()] System.IntPtr hWnd, uint dwTime, uint dwFlags);
}
	
public partial class NativeConstants
{
	/// AW_VER_POSITIVE -> 0x00000004
	public const int AW_VER_POSITIVE = 4;

	/// AW_VER_NEGATIVE -> 0x00000008
	public const int AW_VER_NEGATIVE = 8;

	/// AW_HOR_POSITIVE -> 0x00000001
	public const int AW_HOR_POSITIVE = 1;

	/// AW_HOR_NEGATIVE -> 0x00000002
	public const int AW_HOR_NEGATIVE = 2;

	/// AW_ACTIVATE -> 0x00020000
	public const int AW_ACTIVATE = 131072;

	/// AW_CENTER -> 0x00000010
	public const int AW_CENTER = 16;

	/// AW_SLIDE -> 0x00040000
	public const int AW_SLIDE = 262144;

	/// AW_BLEND -> 0x00080000
	public const int AW_BLEND = 524288;

	/// AW_HIDE -> 0x00010000
	public const int AW_HIDE = 65536;
}

class Program
{
	[STAThread]
    static void Main(string[] args)
    {
		Application.EnableVisualStyles();
		Application.SetCompatibleTextRenderingDefault(false);
		Application.Run(new DemoForm(){Text="主窗口    F1-Blog"});
    }
}

class DemoForm:Form
{
	Button button;
	
	public DemoForm()
	{
		this.SuspendLayout();
		button=new Button()
		{
			Text="单击打开新窗口",
			Location=new System.Drawing.Point(20,20),
			Parent=this
		};
		button.Click+=delegate
		{
			new DemoForm().Show();
		};
		Text="动画窗口演示    F1-Blog";
		FormBorderStyle=FormBorderStyle.FixedToolWindow;
		this.ResumeLayout(false);
	}
	
	protected override void OnLoad(EventArgs e)
	{
		base.OnLoad(e);
		this.FadeIn();
	}
	
	protected override void OnFormClosing(FormClosingEventArgs e)
	{
		base.OnFormClosing(e);
		this.FadeOut();
	}
	
	protected override bool ProcessCmdKey(ref Message msg,Keys keyData)   
	{   
       if(keyData==Keys.F1)System.Diagnostics.Process.Start("http://hi.baidu.com/wingingbob/blog/item/d591073d65829fe53d6d97e9.html");   
       return base.ProcessCmdKey(ref msg,keyData);   
    }   
}