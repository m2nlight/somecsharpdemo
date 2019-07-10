using System;
using System.Threading;

namespace thread_test.s4_unhandledException
{
    public class CatchThreadException
    {
        /// <summary>
        /// 测试代码
        /// </summary>
        public static void TestThreadException()
        {
            Console.WriteLine("{0} 调用者线程 托管线程ID: {0}", Thread.CurrentThread.ManagedThreadId);

            //监听当前应用程序域的未处理异常（通常写在程序入口函数Main开始处）
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainUnhandledException;

            new Thread(Worker.DoWork).Start();  //在新线程启动Worker.DoWork
            
            //new Thread(Worker.DoWorkNoCatch).Start();
            Thread.Sleep(100);
            AppDomain.CurrentDomain.UnhandledException -= CurrentDomainUnhandledException; //取消事件监听
        }

        //未处理异常
        static void CurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                var ex = e.ExceptionObject as Exception;
                if (ex != null) 
                    Console.WriteLine("{0} 未处理异常：{1}", Thread.CurrentThread.ManagedThreadId, ex.Message);
            }
            catch(Exception ex)
            {
                Console.WriteLine("在未处理异常的捕获代码中发生了异常：" + ex.Message);
            }
            finally
            {
                //其他的异常记录和清理代码
            }
        }
    }


    public class Worker
    {
        //正确的做法
        public static void DoWork()
        {
            try
            {
                int z = 9999999;
                checked { z *= 999999999; } //使之引发溢出异常
                Console.WriteLine(z);
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0} Worker线程 捕获到异常：{1}", Thread.CurrentThread.ManagedThreadId, ex.Message);
            }
        }

        //错误的做法
        public static void DoWorkNoCatch()
        {
            int z = 9999999;
            checked { z *= 999999999; } //使之引发溢出异常
            Console.WriteLine(z);
        }
    }
}