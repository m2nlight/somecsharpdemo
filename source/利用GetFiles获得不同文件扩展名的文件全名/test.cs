using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

static class Program
{
	private static void Main()
	{
		try
		{
			var stopwatch=new System.Diagnostics.Stopwatch();
			//~ method1
			var filters="*.jpg|*.gif|*.png|*.bmp|*.jpe|*.jpeg|*.wmf|*.emf|*.xbm|*.ico|*.eps|*.tif|*.tiff|*.g01|*.g02|*.g03|*.g04|*.g05|*.g06|*.g07|*.g08";
			Console.WriteLine("The method1 is start...");
			stopwatch.Start();
			var files=GetFiles(@"D:\Bob",filters,SearchOption.AllDirectories).ToArray();
			stopwatch.Stop();
			Console.WriteLine("Elapsed: {0:#,##0}ms，count: {1:#,##0} files。",stopwatch.ElapsedMilliseconds,files.Count());
			Console.WriteLine();
			//~ method2
			var exts=new []{".jpg", ".gif", ".png", ".bmp", ".jpe", ".jpeg", ".wmf", ".emf", ".xbm", ".ico", ".eps", ".tif", ".tiff", ".g01", ".g02", ".g03", ".g04", ".g05", ".g06", ".g07", ".g08"};		
			Console.WriteLine("The method2 is start...");
			stopwatch.Restart();
			var files1=GetFiles(@"D:\Bob",exts,SearchOption.AllDirectories).ToArray();
			stopwatch.Stop();
			Console.WriteLine("Elapsed: {0:#,##0}ms，count: {1:#,##0} files。",stopwatch.ElapsedMilliseconds,files.Count());
		}
		catch(Exception ex)
		{
			Console.WriteLine(ex);
		}
	}
	
	//~ method1
	private static IEnumerable<string> GetFiles(string sourceFolder, string filters, System.IO.SearchOption searchOption)
	{
		return filters.Split('|').SelectMany(filter => System.IO.Directory.GetFiles(sourceFolder, filter, searchOption));
	}
	
	//~ method2 is faster.
	private static IEnumerable<string> GetFiles(string sourceFolder, string[] exts, System.IO.SearchOption searchOption)
	{
		return System.IO.Directory.GetFiles(sourceFolder, "*.*", searchOption)
				.Where(s=>exts.Contains(System.IO.Path.GetExtension(s), StringComparer.OrdinalIgnoreCase));
	}
}

/*
>test
The method1 is start...
Elapsed: 18,592ms，count: 9,823 files。

The method2 is start...
Elapsed: 1,249ms，count: 9,823 files。
>Exit code: 0    Time: 20.032
*/
