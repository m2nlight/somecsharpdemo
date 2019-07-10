/* ���̴߳�����ʾ - Oyi319����
 * �����ǡ�C# 3.0 Cookbook�����ݻ��汾
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
            //��ͨ�ľ�̬�ֶ�������Ӧ�ó����������̼߳乲��ġ�
            //��ʹ��ThreadStaticAttribute�������ξ�̬�ֶΣ�����ʹ�������̼߳乲��
            //Ȼ��������������һ��С���⣺
            //��̬�ֶεĳ�ʼ����һ��Ӧ�ó�������ֻ��ִ��һ�Σ���ʹ��ʹ��ThreadStatic�������εľ�̬�ֶΣ�
            //�������־�̬�ֶε�ֵ��ÿ���߳���Ψһ�ģ����Ե���ThreadStatic��̬�ֶ�ֻ��������һ���̳߳�ʼ����
            //���������߳��е�ֵû�б���ʼ�����ǵ�ֵ������ֶ����͵�ȱʡֵ��0��false��null����
            //Ϊ�˽��������⣬ֻ�����̵߳�������������ж�ThreadStatic��̬�ֶεĳ�ʼ����
            //ע�⣺ThreadStatic�ֶ����̳߳���Ӧ�û����Щ���⣬��ϸ�����Դ����ע�͡�
            //ָ�ϣ��Ķ����Դ����ע�ͣ��۲���Խ����
            WriteTitleLine("����1���߳��еľ�̬�ֶ� ThreadStaticAttribute");
            ThreadStaticField.TestStaticField();
            Thread.Sleep(500);

            //������̶߳�ͬһ��Դ�����ļ�������������Ӻ��ڴ棩���й������ʱ��Ӧ���ṩ�̵߳İ�ȫ���ʡ�
            //ʹ�ö�ռ�����������Է�ֹ��Դ������̲߳���ȫ�Ķ�д��
            //1.ʹ��lock�ؼ��ֵĶ�ռ��
            //2.ʹ��MethodImpl(MethodImplOption.Synchronized)�������εľ�̬����
            //3.ʹ��Monitor.TryEnter�����Ķ�ռ��
            //ָ�ϣ��Ķ����ַ�ʽ�������������ע��
            WriteTitleLine("����2�����̵߳��̰߳�ȫ���ʣ���ռ���� synchronized");
            SaferMemberAccess.TestSyncLock();
            Thread.Sleep(100);

            //���߳�ͬ�����ٵ���һ��������ʹ���첽ί�е������
            //ʹ��ί�ж����BeginInvoke�������첽������һ��ί�У�
            //����Ҫ�����ǿ�������һ���ص���������ί����ɺ���á�
            //��Ȼ������ί�лص��ķ�ʽҪ����ѯIsCompleted����������WaitOne��������һ��ʱ���̺߳��õöࡣ
            //ָ�ϣ��Ķ���ʾ�����ע�ͣ��۲���Խ����
            WriteTitleLine("����3���첽ί����ɺ�Ļص� AsyncCallback");
            TestAsyncInvoke.TestAsyncDelegate();
            Thread.Sleep(100);

            //���ĳ���߳��Ϸ���δ�����쳣�����ᵼ������߳������쳣��������ֹ��
            //Ӧ���ڴ��ݸ��̵߳����������ʹ��try-catch�����try-catch-finally������쳣����
            //���Լ�����ǰӦ�ó�����ģ���UI�߳��ϵģ�δ�����쳣������AppDomain.CurrentDomain.UnhandledException�¼�
            //�ر�أ�WinForm��UI�߳�δ�����쳣������System.Windows.Forms.Application.ThreadException�¼�
            //WPF��UI�߳�δ�����쳣������System.Windows.Application.DispatcherUnhandledException�¼�
            //ָ�ϣ��Ķ���ʾ�����ע��
            WriteTitleLine("����4���������߳��ϵ�δ�����쳣 unhandledException");
            CatchThreadException.TestThreadException();
            Thread.Sleep(500);

        }
        
        #region ����Ҫ

        private static void Main()
        {
            Console.Title="���̴߳�����ʾ����F1�����ʲ��ͣ���ESC���˳�...";

            new Thread(Test).Start(); //ִ�в���

            ConsoleKeyInfo keyInfo;
            do
            {
                keyInfo = Console.ReadKey(true);
                if (keyInfo.Key == ConsoleKey.F1) new Thread(NavigateBlog).Start();
            } while (keyInfo.Key != ConsoleKey.Escape);
            Environment.Exit(0); //���ȴ������߳̽�����ֱ���˳�������0
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