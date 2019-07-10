using System;
using System.Threading;

namespace thread_test.s1_threadstaticattribute
{
    public class ThreadStaticField
    {
        //ThreadStaticAttribute使静态字段在每个线程中有唯一的值
        [ThreadStatic] public static string Bar;

        /// <summary>
        /// 显示静态字段值
        /// </summary>
        /// <param name="initStaticFields">显示前是否对字段进行初始化</param>
        public static void DisplayStaticFieldValue(object initStaticFields)
        {
            //多线程以此方法启动，对ThreadStaticAttribute修饰的静态字段在此进行初始化
            if ((bool) initStaticFields) InitStaticFields();

            string msg = string.Format("{0,4}{1,4}{2,4}\t{3}", Thread.CurrentThread.ManagedThreadId,
                                       Thread.CurrentThread.IsThreadPoolThread ? "线程池" : "普通", Thread.CurrentThread.Name,
                                       Bar);
            Console.WriteLine(msg);

            //为了测试ThreadStatic字段在线程池的工作情况。
            //把线程寿命延长，使线程池来不及重用同一个（指托管线程Id号相同的）线程。
            Thread.Sleep(100);
        }

        /// <summary>
        /// 初始化线程静态字段
        /// </summary>
        private static void InitStaticFields()
        {
            Bar = "已初始"; //初始化Bar静态字段
        }

        /// <summary>
        /// 测试代码
        /// </summary>
        public static void TestStaticField()
        {
            DisplayStaticFieldValue(true); //在当前线程（第一次）初始化Bar静态字段，显示其值

            var t1 = new Thread(DisplayStaticFieldValue) {Name = "t1"};
            t1.Start(true); //在t1线程上初始化Bar，显示其值

            var t2 = new Thread(DisplayStaticFieldValue) {Name = "t2"};
            t2.Start(false); //在t2线程上未初始化Bar，显示其值

            //使用QueueUserWorkItem方法加入线程池队列，那么在线程池线程上，
            //如果线程的Id号相同，会使用同一个ThreadStatic字段
            ThreadPool.QueueUserWorkItem(DisplayStaticFieldValue, true); //初始化
            ThreadPool.QueueUserWorkItem(DisplayStaticFieldValue, false); //期望是未初始化
            ThreadPool.QueueUserWorkItem(DisplayStaticFieldValue, false); //期望是未初始化
            ThreadPool.QueueUserWorkItem(DisplayStaticFieldValue, false); //期望是未初始化
            ThreadPool.QueueUserWorkItem(DisplayStaticFieldValue, false); //期望是未初始化
            ThreadPool.QueueUserWorkItem(DisplayStaticFieldValue, false); //期望是未初始化
            //查看结果可知：有的线程池线程静态字段并不是我们的期望值，真实值似乎与线程Id号有关系
            //利用ThreadStatic属性的静态字段在使用线程池线程时需要注意这种问题的发生

            DisplayStaticFieldValue(false); //在当前线程上显示Bar静态字段的值（前面已经初始化过）
        }
    }
}