using System;
using System.Threading;

namespace thread_test.s4_unhandledException
{
    public class CatchThreadException
    {
        /// <summary>
        /// ���Դ���
        /// </summary>
        public static void TestThreadException()
        {
            Console.WriteLine("{0} �������߳� �й��߳�ID: {0}", Thread.CurrentThread.ManagedThreadId);

            //������ǰӦ�ó������δ�����쳣��ͨ��д�ڳ�����ں���Main��ʼ����
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainUnhandledException;

            new Thread(Worker.DoWork).Start();  //�����߳�����Worker.DoWork
            
            //new Thread(Worker.DoWorkNoCatch).Start();
            Thread.Sleep(100);
            AppDomain.CurrentDomain.UnhandledException -= CurrentDomainUnhandledException; //ȡ���¼�����
        }

        //δ�����쳣
        static void CurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                var ex = e.ExceptionObject as Exception;
                if (ex != null) 
                    Console.WriteLine("{0} δ�����쳣��{1}", Thread.CurrentThread.ManagedThreadId, ex.Message);
            }
            catch(Exception ex)
            {
                Console.WriteLine("��δ�����쳣�Ĳ�������з������쳣��" + ex.Message);
            }
            finally
            {
                //�������쳣��¼���������
            }
        }
    }


    public class Worker
    {
        //��ȷ������
        public static void DoWork()
        {
            try
            {
                int z = 9999999;
                checked { z *= 999999999; } //ʹ֮��������쳣
                Console.WriteLine(z);
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0} Worker�߳� �����쳣��{1}", Thread.CurrentThread.ManagedThreadId, ex.Message);
            }
        }

        //���������
        public static void DoWorkNoCatch()
        {
            int z = 9999999;
            checked { z *= 999999999; } //ʹ֮��������쳣
            Console.WriteLine(z);
        }
    }
}