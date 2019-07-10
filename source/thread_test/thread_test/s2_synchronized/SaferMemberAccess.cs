using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace thread_test.s2_synchronized
{
    public class SaferMemberAccess
    {
        /// <summary>
        /// lock(私有对象) 方式保障对 _numeric 操作的线程安全。
        /// lock关键字不要在公共类型或者程序控制之外的实例上使用，容易导致死锁。
        /// </summary>
        public static class Method1Lock
        {
            private static object syncObj = new object(); //用一个私有对象作为同步锁对象
            private static int _numeric = 1; //需要安全访问的对象

            public static void IncrementNumeric()
            {
                lock (syncObj)
                {
                    ++_numeric;
                }
            }

            public static void ModifyNumeric(int newValue)
            {
                lock (syncObj)
                {
                    _numeric = newValue;
                }
            }

            public static int ReadNumeric()
            {
                lock (syncObj)
                {
                    return _numeric;
                }
            }
        }


        /// <summary>
        /// 用MethodImpl(MethodImplOptions.Synchronized)属性保护静态方法的代码
        /// </summary>
        public static class Method2MethodImpl
        {
            private static int _numeric = 1; //需要安全访问的对象

            [MethodImpl(MethodImplOptions.Synchronized)]
            public static void IncrementNumeric()
            {
                ++_numeric;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            public static void ModifyNumeric(int newValue)
            {
                _numeric = newValue;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            public static int ReadNumeric()
            {
                return _numeric;
            }
        }


        /// <summary>
        /// Monitor.TryEnter方法支持超时值设定，在未超时期间得到锁返回true，否则返回false。
        /// TryEnter有单个参数（不指定超时）的重载版本，那个版本不会阻塞进程，立即返回结果。
        /// 这个演示类，公开了同步对象SyncRoot，供外部代码锁定，
        /// 通过它，可以解决在多个线程同时调用ModifyNumeric接着ReadNumeric时，
        /// 可能读取到其他线程修改的值的问题。参看调用此类的演示代码。
        /// </summary>
        public class Method3Monitor
        {
            private static object syncObj = new object();
            private static int _numeric = 1;

            public static object SyncRoot { get { return syncObj; } }

            public static void IncrementNumeric()
            {
                if (Monitor.TryEnter(syncObj, 250))
                {
                    try
                    {
                        ++_numeric;
                    }
                    finally
                    {
                        Monitor.Exit(syncObj);
                    }
                }
            }

            public static void ModifyNumeric(int newValue)
            {
                if (Monitor.TryEnter(syncObj, 250))
                {
                    try
                    {
                        _numeric = newValue;
                    }
                    finally
                    {
                        Monitor.Exit(syncObj);
                    }
                }
            }

            public static int ReadNumeric()
            {
                if (Monitor.TryEnter(syncObj))
                {
                    try
                    {
                        return _numeric;
                    }
                    finally
                    {
                        Monitor.Exit(syncObj);
                    }
                }
                return -1;
            }
        }


        /// <summary>
        /// lock方法的测试代码
        /// </summary>
        private static void TestLock(object state)
        {
            Method1Lock.IncrementNumeric(); //演示加1和读值操作
            int numeric = Method1Lock.ReadNumeric(); //在多线程调用时，此处读到的值很可能是其他线程操作后的结果，为了解决这个问题，见下面的TestMonitor方法代码
            Console.Write("{0,4}", numeric); //显示结果
        }

        /// <summary>
        /// Monitor.TryEnter方法的测试代码
        /// </summary>
        private static void TestMonitor(object state)
        {
            var newValue = (int)state; //演示修改和读值操作
            var num = 0;
            if (Monitor.TryEnter(Method3Monitor.SyncRoot, 250)) //通过Test2Monitor公开的同步对象SyncRoot，使读写值同步
            {
                Method3Monitor.ModifyNumeric(newValue);
                num = Method3Monitor.ReadNumeric();
                Monitor.Exit(Method3Monitor.SyncRoot);
            }
            Console.Write("{0,4}", num); //显示正确的结果
        }

        #region 不重要
        /// <summary>
        /// 测试代码
        /// </summary>
        public static void TestSyncLock()
        {
            int workerThreads; //线程池可用于辅助线程的数目
            int completionPortThreads; //线程池可用于异步I/O线程的数目
            ThreadPool.GetAvailableThreads(out workerThreads, out completionPortThreads);
            int count = Math.Min(50, workerThreads);
            //方法1
            int initialValue = 20;
            int expectValue = initialValue + count;
            Method1Lock.ModifyNumeric(initialValue);
            Console.WriteLine("方法1: lock(syncObj)");
            Console.WriteLine("线程池测试队列数: {0}, 初值: {1}, 将初值{1}进行{0}次的加1操作, 期望结果: {2}", count, initialValue, expectValue);
            Console.Write("结果:\t");
            for (int i = 0; i < count; i++)
            {
                ThreadPool.QueueUserWorkItem(TestLock);
            }
            Thread.Sleep(250);
            Console.WriteLine("\n最终结果: " + Method1Lock.ReadNumeric());
            
            //方法2
            Console.WriteLine("方法2: [MethodImpl(MethodImplOptions.Synchronized)]  （略，详细见代码）");
            
            //方法3
            var maxValue = 1000;
            Console.WriteLine("方法3: Monitor.TryEnter(syncObj)");
            Console.WriteLine("线程池测试队列数: {0}, 修改_numeric字段{0}次, 修改值为{1}内的非负的随机整数", count, maxValue);
            Console.Write("结果:\t");
            var rand = new Random();
            for (int i = 0; i < count; i++)
            {
                ThreadPool.QueueUserWorkItem(TestMonitor, rand.Next(maxValue)); //随机数不会取上限值，可以取下限值(此重载下限值为0)
            }
            Thread.Sleep(250);
            Console.WriteLine("\n最终结果: " + Method3Monitor.ReadNumeric());
        }
        #endregion
    }
}