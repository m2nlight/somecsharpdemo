using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace thread_test.s2_synchronized
{
    public class SaferMemberAccess
    {
        /// <summary>
        /// lock(˽�ж���) ��ʽ���϶� _numeric �������̰߳�ȫ��
        /// lock�ؼ��ֲ�Ҫ�ڹ������ͻ��߳������֮���ʵ����ʹ�ã����׵���������
        /// </summary>
        public static class Method1Lock
        {
            private static object syncObj = new object(); //��һ��˽�ж�����Ϊͬ��������
            private static int _numeric = 1; //��Ҫ��ȫ���ʵĶ���

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
        /// ��MethodImpl(MethodImplOptions.Synchronized)���Ա�����̬�����Ĵ���
        /// </summary>
        public static class Method2MethodImpl
        {
            private static int _numeric = 1; //��Ҫ��ȫ���ʵĶ���

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
        /// Monitor.TryEnter����֧�ֳ�ʱֵ�趨����δ��ʱ�ڼ�õ�������true�����򷵻�false��
        /// TryEnter�е�����������ָ����ʱ�������ذ汾���Ǹ��汾�����������̣��������ؽ����
        /// �����ʾ�࣬������ͬ������SyncRoot�����ⲿ����������
        /// ͨ���������Խ���ڶ���߳�ͬʱ����ModifyNumeric����ReadNumericʱ��
        /// ���ܶ�ȡ�������߳��޸ĵ�ֵ�����⡣�ο����ô������ʾ���롣
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
        /// lock�����Ĳ��Դ���
        /// </summary>
        private static void TestLock(object state)
        {
            Method1Lock.IncrementNumeric(); //��ʾ��1�Ͷ�ֵ����
            int numeric = Method1Lock.ReadNumeric(); //�ڶ��̵߳���ʱ���˴�������ֵ�ܿ����������̲߳�����Ľ����Ϊ�˽��������⣬�������TestMonitor��������
            Console.Write("{0,4}", numeric); //��ʾ���
        }

        /// <summary>
        /// Monitor.TryEnter�����Ĳ��Դ���
        /// </summary>
        private static void TestMonitor(object state)
        {
            var newValue = (int)state; //��ʾ�޸ĺͶ�ֵ����
            var num = 0;
            if (Monitor.TryEnter(Method3Monitor.SyncRoot, 250)) //ͨ��Test2Monitor������ͬ������SyncRoot��ʹ��дֵͬ��
            {
                Method3Monitor.ModifyNumeric(newValue);
                num = Method3Monitor.ReadNumeric();
                Monitor.Exit(Method3Monitor.SyncRoot);
            }
            Console.Write("{0,4}", num); //��ʾ��ȷ�Ľ��
        }

        #region ����Ҫ
        /// <summary>
        /// ���Դ���
        /// </summary>
        public static void TestSyncLock()
        {
            int workerThreads; //�̳߳ؿ����ڸ����̵߳���Ŀ
            int completionPortThreads; //�̳߳ؿ������첽I/O�̵߳���Ŀ
            ThreadPool.GetAvailableThreads(out workerThreads, out completionPortThreads);
            int count = Math.Min(50, workerThreads);
            //����1
            int initialValue = 20;
            int expectValue = initialValue + count;
            Method1Lock.ModifyNumeric(initialValue);
            Console.WriteLine("����1: lock(syncObj)");
            Console.WriteLine("�̳߳ز��Զ�����: {0}, ��ֵ: {1}, ����ֵ{1}����{0}�εļ�1����, �������: {2}", count, initialValue, expectValue);
            Console.Write("���:\t");
            for (int i = 0; i < count; i++)
            {
                ThreadPool.QueueUserWorkItem(TestLock);
            }
            Thread.Sleep(250);
            Console.WriteLine("\n���ս��: " + Method1Lock.ReadNumeric());
            
            //����2
            Console.WriteLine("����2: [MethodImpl(MethodImplOptions.Synchronized)]  ���ԣ���ϸ�����룩");
            
            //����3
            var maxValue = 1000;
            Console.WriteLine("����3: Monitor.TryEnter(syncObj)");
            Console.WriteLine("�̳߳ز��Զ�����: {0}, �޸�_numeric�ֶ�{0}��, �޸�ֵΪ{1}�ڵķǸ����������", count, maxValue);
            Console.Write("���:\t");
            var rand = new Random();
            for (int i = 0; i < count; i++)
            {
                ThreadPool.QueueUserWorkItem(TestMonitor, rand.Next(maxValue)); //���������ȡ����ֵ������ȡ����ֵ(����������ֵΪ0)
            }
            Thread.Sleep(250);
            Console.WriteLine("\n���ս��: " + Method3Monitor.ReadNumeric());
        }
        #endregion
    }
}