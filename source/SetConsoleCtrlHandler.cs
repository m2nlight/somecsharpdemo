using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;

class Program
{
	//存储命令行参数
	public static Hashtable CommandLineArgs = new Hashtable();
	
	static void Main(string[] args)
	{
		ParseCommandLineArgs(args); //解释命令行参数
		
		//声明委托方法，如果在运行时产生异常，可以在后面告诉GC保留它(用GC.KeepAlive方法)，防止被回收掉。
		PHANDLER_ROUTINE handlerRoutine = HandlerRoutine; 
		//注册handlerRoutine为监听回调函数。这里没有检查返回值是否成功，假设它是成功的。
        NativeMethods.SetConsoleCtrlHandler(handlerRoutine, true);
		
		//显示些数据
		Console.WriteLine("SetConsoleCtrlHandler Demo by Oyi319@csdn");
		Console.WriteLine(new string('-',10));
		if (Information.ShowTime) Console.WriteLine(DateTime.Now);
		if (Information.UserName.Length>0) Console.WriteLine("Hi, "+Information.UserName);
		Console.WriteLine("输入'exit'正常退出，或者测试<Ctrl+C>，<Ctrl+Break>，或者直接点击关闭按钮。。。");
		for(;;)
		{
			var input = Console.ReadLine();
			if(input!=null && input.Equals("exit", StringComparison.OrdinalIgnoreCase))
			{
				CloseConsole("正常退出",0); //退出控制台
			}
		}
		
		//GC.KeepAlive(handlerRoutine); //使GC不要回收委托函数
	}
	
	private static int HandlerRoutine(uint ctrltype)
	{
		string outputText = "";
		switch (ctrltype)
		{
			case NativeConstants.CTRL_C_EVENT:
		        outputText = "按下了 Ctrl+C";
		        break;
		    case NativeConstants.CTRL_BREAK_EVENT:
				outputText = "按下了 Ctrl+Break";
		        break;
		    case NativeConstants.CTRL_CLOSE_EVENT:
				outputText = "收到关闭窗口的消息(按下了窗口关闭按钮 或者 被任务管理器结束任务)";
		        break;
			case NativeConstants.CTRL_LOGOFF_EVENT:
				outputText = "注销系统";
		        break;
			case NativeConstants.CTRL_SHUTDOWN_EVENT:
				outputText = "关机";
		        break;
		}
		var r = (int)ctrltype; //作为程序的返回值
		CloseConsole(outputText, r); //调用退出方法
		return 1; //返回1 (TRUE)。 另说明一下：由于CloseConsole方法使用调用退出函数，此句代码不会被执行。
	}
	
	/// <summary>
    /// 退出控制台
    /// </summary>
	/// <param name="outputText">输出字符串</param>
	/// <param name="r">退出代码</param>
	static void CloseConsole(string outputText, int r)
	{
		Console.WriteLine("程序结束："+outputText);
		Console.WriteLine("退出代码："+r);
		Thread.Sleep(3000); //等待3秒钟，让用户观察到退出信息。
		Environment.Exit(r); //退出代码，通常用0表示程序的正常退出，大于0表示非正常退出的代号。
	}
	
	static void ParseCommandLineArgs(string[] args)
	{
		//读取命令行参数
		if(args.Length != 0)
		{
			//命令行参数格式：/argname:argvalue
			string pattern = @"(?<argname>/\w+)(:(?<argvalue>\w+))?";
			foreach(var arg in args)
			{
				var match = Regex.Match(arg, pattern);
				if (!match.Success)
					throw new ArgumentException(
                        "The command line arguments are improperly formed. Use /argname:argvalue.");
				
				CommandLineArgs[match.Groups["argname"].Value] = match.Groups["argvalue"].Value;
			}
		}
		
		if (CommandLineArgs.ContainsKey("/name"))
		{
			Information.UserName = CommandLineArgs["/name"].ToString();
		}
		
		if (CommandLineArgs.ContainsKey("/showtime"))
		{
			Information.ShowTime = true;
		}
	}
	
	
	
	#region SetConsoleCtrlHandler

	/// Return Type: BOOL->int
	///CtrlType: DWORD->unsigned int
	public delegate int PHANDLER_ROUTINE(uint CtrlType);
	//~ 说明：PHANDLER_ROUTINE被声明为委托，它是SetConsoleCtrlHandler的回调函数指针，
	//~ 当控制台退出事件发生时，我们利用此委托执行回调函数。
	//~ 参数CtrlType由系统传入，通过它能判断出控制台退出事件的原因，它会是下面NativeConstants类
	//~ 中CTRL_开头的常量的值，意义见后面的解释。
	//~ 返回值为BOOL类型，这里声明为int，我们可以返回0或者1。用0表示false，用1表示true。
	//~ 当我们返回false时，如果我们用SetConsoleCtrlHandler注册了多个监听，将会继续完成其他的回调函数；
	//~ 而我们返回true时，假设我们用SetConsoleCtrlHandler注册了多个监听，其他的回调函数将不再执行，
	//~ 起一个拦截的作用。
	//~ 更多资料请参考：http://msdn.microsoft.com/en-us/library/ms683242(VS.85).aspx
	
	public class NativeMethods
	{
		/// Return Type: BOOL->int
		///HandlerRoutine: PHANDLER_ROUTINE
		///Add: BOOL->int
		[DllImport("kernel32.dll", EntryPoint = "SetConsoleCtrlHandler")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool SetConsoleCtrlHandler(PHANDLER_ROUTINE HandlerRoutine,
														[MarshalAs(UnmanagedType.Bool)] bool Add);
		
		//~ 说明：SetConsoleCtrlHandler API函数，当控制台程序并非从主函数正常退出的情况进行监视回调。
	    //~ 被监视的操作包括：用户按Ctrl+C、按Ctrl+Break、收到关闭控制台窗口消息（点击控制台窗口的关闭按钮
		//~ 或者在任务管理器里“结束任务”）、注销Windows系统、或者关机。
		//~ 它的第一个参数HandlerRoutine为回调函数，就是处理此类操作时我们让程序执行的代码；
		//~ 第二个参数设定为true时，注册HandlerRoutine回调函数，而为false时，则取消HandlerRoutine回调函数，
		//~ 因此，我们可以注册一系列处理方法，类似事件的注册和移除。
		//~ 执行SetConsoleCtrlHandler函数会立即返回结果，当返回0时，表示函数执行成功，返回非0值，表示失败，
		//~ 使用GetLastError API函数得到失败消息。
		//~ 更多材料参考：http://msdn.microsoft.com/en-us/library/ms686016(VS.85).aspx
	}

	public class NativeConstants
	{
		/// CTRL_SHUTDOWN_EVENT -> 6
		public const int CTRL_SHUTDOWN_EVENT = 6; //关机

		/// CTRL_LOGOFF_EVENT -> 5
		public const int CTRL_LOGOFF_EVENT = 5; //注销

		/// CTRL_CLOSE_EVENT -> 2
		public const int CTRL_CLOSE_EVENT = 2; //收到关闭消息

		/// CTRL_BREAK_EVENT -> 1
		public const int CTRL_BREAK_EVENT = 1; //Ctrl+Break 中断

		/// CTRL_C_EVENT -> 0
		public const int CTRL_C_EVENT = 0; //Ctrl+C 中断
	}
	
	#endregion
}

//保存信息的静态类，这是为了演示主程序命令行参数而定义的
public static class Information
{
	public static string UserName = "";
	public static bool ShowTime;
}
