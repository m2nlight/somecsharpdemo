using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// 提供删除文件或文件夹的功能。
/// 注意：删除文件线程运行期间，创建已经请求删除的文件同名称，有可能会被删除。
/// </summary>
public class TaskDeleteFiles
{
	/// <summary>
	/// 每个任务的生存周期
	/// </summary>
	public TimeSpan TaskLifeTimeSpan {get; set;}
	
	/// <summary>
	/// 每个文件或文件夹的删除失败后增加的延期
	/// </summary>
	public TimeSpan ExpiredTimeSpan {get; set;}

    //构造
	public TaskDeleteFiles(TimeSpan taskLifeTimeSpan, TimeSpan expiredTimeSpan)
	{
		this.TaskLifeTimeSpan=taskLifeTimeSpan;
		this.ExpiredTimeSpan=expiredTimeSpan;
	}

	/// <summary>
	/// 启动一个删除文件任务
	/// </summary>
	/// <param name="files">文件名清单</param>
	public void DeleteFiles(IEnumerable<string> files)
	{
		if (files == null || files.FirstOrDefault() == default(string)) return;
		Task.Factory.StartNew(DoDeleteFiles, files);
	}

	private void DoDeleteFiles(object obj)
	{
		var files = ((IEnumerable<string>)obj).Distinct(StringComparer.OrdinalIgnoreCase); //去除重复的文件名
		var currentTime = DateTime.Now;
		var expires = files.ToDictionary(file => file, file => currentTime); //超时文件清单
		if (expires.Count == 0) return;

		var threadLife = DateTime.Now + this.TaskLifeTimeSpan;

		if (DateTime.Now >= threadLife)
			return;

		while (expires.Count > 0)
		{
			while (true)
			{
				var expiredFile = expires.FirstOrDefault(n => DateTime.Now >= n.Value);
				if (expiredFile.Equals(default(KeyValuePair<string, DateTime>)))
				{
					Thread.Sleep(1000); //等待下一秒到来
					if (DateTime.Now < threadLife)
					{
						return;
					}
					
					break;
				}

				var path = expiredFile.Key;
				if (File.Exists(path))
				{
					//是文件
					try
					{
						File.Delete(path);
					}
					catch
					{
						expires[path] = expiredFile.Value + this.ExpiredTimeSpan;
						continue; //跳过移除，进入下一个
					}
				}
				else if (Directory.Exists(path))
				{
					//是目录
					try
					{
						Directory.Delete(path, true);
					}
					catch
					{
						expires[path] = expiredFile.Value + this.ExpiredTimeSpan;
						continue; //跳过移除，进入下一个
					}
				}

				//文件已经删除，或者是无效的文件或目录名，从清单中移除
				expires.Remove(path);
				break;
			}
		}
	}
}


//~ public class Demo
//~ {
	//~ public static void Main()
	//~ {
		//~ //创建一个TaskDeleteFiles实例，指定删除任务生存周期为1小时，每个文件的失败延期为2分钟
		//~ var taskDeleteFiles=new TaskDeleteFiles(TimeSpan.FromHours(1), TimeSpan.FromMinutes(2));
		//~ //创建一个欲删除文件清单（可以有重复，可以包含不存在的文件或文件夹）
		//~ var list=new List<string>();
		//~ list.Add(@"C:\test\a.txt"); //文件
		//~ list.Add(@"C:\test\b.txt");
		//~ list.Add(@"C:\test\c.txt");
		//~ list.Add(@"C:\test\c.txt"); //重复
		//~ list.Add(@"C:\test\folder1"); //文件夹
		//~ list.Add(@"C:\test\folder2");
		//~ //执行删除
		//~ taskDeleteFiles.DeleteFiles(list);
		
		//~ //等待用户按ENTER结束主进程
		//~ Console.WriteLine("Executing delete task, press <ENTER> to EXIT...");
		//~ Console.ReadLine();
	//~ }
//~ }