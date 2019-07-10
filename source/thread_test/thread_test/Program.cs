/* 多线程代码演示 - Oyi319整理
 * 代码是《C# 3.0 Cookbook》的演化版本
 */
using System;
using System.Diagnostics;
using System.Threading;
using thread_test.s1_threadstaticattribute;
using thread_test.s2_synchronized;
using thread_test.s3_asyncCallback;
using thread_test.s4_unhandledException;

namespace thread_test
{
    public class Program
    {
        private static void Test()
        {
            //普通的静态字段在整个应用程序域中是线程间共享的。
            //而使用ThreadStaticAttribute属性修饰静态字段，可以使它不在线程间共享。
            //然而这样做会引发一个小问题：
            //静态字段的初始化在一个应用程序域中只会执行一次，即使是使用ThreadStatic属性修饰的静态字段，
            //由于这种静态字段的值在每个线程是唯一的，所以导致ThreadStatic静态字段只能在其中一个线程初始化，
            //而在其他线程中的值没有被初始，它们的值则会是字段类型的缺省值（0、false或null）。
            //为了解决这个问题，只有在线程的启动方法里进行对ThreadStatic静态字段的初始化。
            //注意：ThreadStatic字段在线程池中应用会产生些问题，详细见测试代码的注释。
            //指南：阅读测试代码和注释，观察测试结果。
            WriteTitleLine("测试1：线程中的静态字段 ThreadStaticAttribute");
            ThreadStaticField.TestStaticField();
            Thread.Sleep(500);

            //当多个线程对同一资源（如文件句柄、网络连接和内存）进行共享访问时，应该提供线程的安全访问。
            //使用独占锁技术，可以防止资源被多个线程不安全的读写。
            //1.使用lock关键字的独占锁
            //2.使用MethodImpl(MethodImplOption.Synchronized)属性修饰的静态方法
            //3.使用Monitor.TryEnter方法的独占锁
            //指南：阅读三种方式的类声明代码和注释
            WriteTitleLine("测试2：多线程的线程安全访问（独占锁） synchronized");
            SaferMemberAccess.TestSyncLock();
            Thread.Sleep(100);

            //多线程同步面临的另一类问题是使用异步委托的情况。
            //使用委托对象的BeginInvoke方法来异步地启动一个委托，
            //若需要，我们可以利用一个回调函数，在委托完成后调用。
            //显然，这种委托回调的方式要比轮询IsCompleted或者依赖于WaitOne方法阻塞一段时间线程好用得多。
            //指南：阅读演示代码和注释，观察测试结果。
            WriteTitleLine("测试3：异步委托完成后的回调 AsyncCallback");
            TestAsyncInvoke.TestAsyncDelegate();
            Thread.Sleep(100);

            //如果某个线程上发生未处理异常，将会导致这个线程因发生异常而悄悄终止。
            //应该在传递给线程的启动方法里，使用try-catch块或者try-catch-finally块进行异常处理，
            //可以监听当前应用程序域的（非UI线程上的）未处理异常，监听AppDomain.CurrentDomain.UnhandledException事件
            //特别地，WinForm的UI线程未处理异常，监听System.Windows.Forms.Application.ThreadException事件
            //WPF的UI线程未处理异常，监听System.Windows.Application.DispatcherUnhandledException事件
            //指南：阅读演示代码和注释
            WriteTitleLine("测试4：捕获辅助线程上的未处理异常 unhandledException");
            CatchThreadException.TestThreadException();
            Thread.Sleep(500);

        }
        
        #region 不重要

        private static void Main()
        {
            Console.Title="多线程代码演示，按F1键访问博客，按ESC键退出...";

            new Thread(Test).Start(); //执行测试

            ConsoleKeyInfo keyInfo;
            do
            {
                keyInfo = Console.ReadKey(true);
                if (keyInfo.Key == ConsoleKey.F1) new Thread(NavigateBlog).Start();
            } while (keyInfo.Key != ConsoleKey.Escape);
            Environment.Exit(0); //不等待其他线程结束，直接退出，返回0
        }

        private static readonly ConsoleColor ConsoleForeColor = Console.ForegroundColor;
        private static void WriteTitleLine(string title)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(title);
            Console.ForegroundColor = ConsoleForeColor;
        }

        private static void NavigateBlog()
        {
            Process.Start("http://blog.csdn.net/oyi319/archive/2010/10/28/5972655.aspx");
        }
        
        #endregion
    }
}