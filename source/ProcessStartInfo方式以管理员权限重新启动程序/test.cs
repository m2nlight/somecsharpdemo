using System;
using System.Text;

class Program
{
	static void Main(params string[] args)
	{
		var windowsIdentity=System.Security.Principal.WindowsIdentity.GetCurrent(); //获得当前Windows用户标识
		var windowsPrincipal=new System.Security.Principal.WindowsPrincipal(windowsIdentity); //检查 windows 用户的 windows 组成员身份。
		
		if(windowsPrincipal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator))
		{
			Output("拥有管理员权限。");
			if(args.Length>0)
			{
				Output("参数："+string.Join(", ",args));
			}
		}
		else
		{
			Output("正在以管理员权限启动一个新的实例...");
			var commandLineArgs=Environment.GetCommandLineArgs();
			var startInfo=new System.Diagnostics.ProcessStartInfo();
			startInfo.UseShellExecute=true;
			startInfo.WorkingDirectory=Environment.CurrentDirectory;
			startInfo.FileName=commandLineArgs[0];
			if(commandLineArgs.Length>1)
			{
				startInfo.Arguments=string.Join(" ",commandLineArgs,1,commandLineArgs.Length-1);
			}
			startInfo.Verb="runas";
			try
			{
				System.Diagnostics.Process.Start(startInfo);
			}
			catch(System.ComponentModel.Win32Exception ex)
			{
				Output("Win32Exception: "+ex.Message);
			}
			catch(Exception ex)
			{
				Output("Exception: "+ex);
			}
		}
		
		Output("按任意键结束...");
		Console.ReadKey(true);
	}
	
	static void Output(string text)
	{
		Console.WriteLine("[{0:HH:mm:ss.fff}] {1}",DateTime.Now,text);
	}
}