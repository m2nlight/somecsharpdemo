using System.Threading;

#region HashAlgorithm 逻辑
using System;
using System.IO;
using HashAlg = System.Security.Cryptography.HashAlgorithm;

public class HashAlgorithm
{
	#region Delegates

	/// <summary>
	/// 完成Hash计算的回调
	/// </summary>
	public delegate void EndCallback(EndState endState);

	/// <summary>
	/// 进行Hash计算时的回调
	/// </summary>
	public delegate void HashingCallback(HashingState hashingState);

	#endregion

	private bool _cancel; //取消计算

	/// <summary>
	/// 获得Hash结果
	/// </summary>
	public byte[] Hash { get; private set; }

	/// <summary>
	/// 输出Hash结果的字符串版本
	/// </summary>
	/// <returns></returns>
	public override string ToString()
	{
		return Hash != null ? BitConverter.ToString(Hash).Replace("-", "").ToLower() : "";
	}

	/// <summary>
	/// 停止计算
	/// </summary>
	public void Stop()
	{
		_cancel = true;
	}

	/// <summary>
	/// 计算文件的Hash值
	/// </summary>
	/// <param name="fileName">文件名</param>
	/// <param name="hashName">Hash名</param>
	/// <param name="hashingCallback">进度回调函数</param>
	/// <param name="endCallback">结束计算的回调函数</param>
	/// <param name="state">传递给回调的参数</param>
	public void ComputeHash(string fileName, string hashName, HashingCallback hashingCallback,
							EndCallback endCallback, object state)
	{
		using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
		{
			ComputeHash(fs, hashName, hashingCallback, endCallback, state);
		}
	}

	/// <summary>
	/// 计算流的Hash值
	/// </summary>
	/// <param name="stream">流</param>
	/// <param name="hashName">Hash名</param>
	/// <param name="hashingCallback">进度回调函数</param>
	/// <param name="endCallback">结束计算的回调函数</param>
	/// <param name="state">传递给回调的参数</param>
	public void ComputeHash(Stream stream, string hashName, HashingCallback hashingCallback, EndCallback endCallback,
							object state)
	{
		const int bufferSize = 4096; //0x1000
		Hash = null;
		_cancel = false;

		bool nullHashingCallback = hashingCallback == null; //未指定计算时的回调
		long bytesRead = 0L; //已经完成的数据
		long totalBytesLength = stream.Length;

		var hash = HashAlg.Create(hashName);
		int num;
		var buffer = new byte[bufferSize];
		do
		{
			num = stream.Read(buffer, 0, bufferSize);
			if (num == bufferSize)
			{
				hash.TransformBlock(buffer, 0, num, buffer, 0);
			}
			else if (totalBytesLength == stream.Position)
			{
				hash.TransformFinalBlock(buffer, 0, num);
			}
			else if (num > 0)
			{
				Console.WriteLine("-------HASH-------HERE===");
				var buffer2 = new byte[num];
				Buffer.BlockCopy(buffer, 0, buffer2, 0, num);
				hash.TransformBlock(buffer2, 0, num, buffer2, 0);
			}

			if (num <= 0 || nullHashingCallback) continue;
			bytesRead += num;
			hashingCallback(new HashingState(bytesRead, totalBytesLength, state)); //执行计算时回调
		} while (num > 0 && !_cancel);

		Hash = _cancel ? new byte[0] : hash.Hash; //获得计算结果
		if (endCallback != null) endCallback(new EndState(_cancel, state)); //执行结束回调
	}

	#region Nested type: EndState

	/// <summary>
	/// 为EndCallback提供数据
	/// </summary>
	public class EndState
	{
		internal EndState(bool isCancel, object state)
		{
			IsCancel = isCancel;
			State = state;
		}

		/// <summary>
		/// 是否取消退出的
		/// </summary>
		public bool IsCancel { get; private set; }

		/// <summary>
		/// 获得传递的参数
		/// </summary>
		public object State { get; private set; }
	}

	#endregion

	#region Nested type: HashingState

	/// <summary>
	/// 为HashingCallback提供数据
	/// </summary>
	public class HashingState
	{
		internal HashingState(long byteRead, long totalBytesLength, object state)
		{
			BytesRead = byteRead;
			TotalBytesLength = totalBytesLength;
			State = state;
		}

		/// <summary>
		/// 获得已经计算完成的字节数
		/// </summary>
		public long BytesRead { get; private set; }

		/// <summary>
		/// 获得总字节数
		/// </summary>
		public long TotalBytesLength { get; private set; }

		/// <summary>
		/// 获得传递的参数
		/// </summary>
		public object State { get; private set; }
	}

	#endregion
}
#endregion




public class Program
{
	static void Main()
	{
		Console.Write("File: ");
		var f=Console.ReadLine();
		f=f.Trim('\"');
		if(File.Exists(f))
		{
			var hasher=new HashAlgorithm();
			var t =  
				new Thread(() =>  
						   hasher.ComputeHash(  
							   f, "SHA1",  
							   hashingState => UpdateProgress(hashingState.BytesRead, hashingState.TotalBytesLength),  
							   endState => Finish(hasher, endState.IsCancel), null))  
					{  
						Name = "ComputeHash",  
						IsBackground = true  
					};  
			Console.WriteLine("Press <ENTER> to EXIT...");
			Console.WriteLine();
			Console.Write("Computing SHA1...");
			t.Start();  
			Console.ReadLine();
		}
		else
		{
			Console.WriteLine("ERROR fileName");
		}
	}
	
	static void UpdateProgress(long bytesRead, long total)
	{
		Console.Write("\rComputing SHA1...{0:0.00%}               ",bytesRead/(double)total);
	}
	
	static void Finish(HashAlgorithm hasher, bool isCancel)
	{
		Console.WriteLine();
		Console.WriteLine("IsCancel: "+isCancel);
		Console.WriteLine("SHA1: "+ hasher.ToString());
		Environment.Exit(0);
	}
}