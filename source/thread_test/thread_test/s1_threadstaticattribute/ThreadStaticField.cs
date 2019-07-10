using System;
using System.Threading;

namespace thread_test.s1_threadstaticattribute
{
    public class ThreadStaticField
    {
        //ThreadStaticAttributeʹ��̬�ֶ���ÿ���߳�����Ψһ��ֵ
        [ThreadStatic] public static string Bar;

        /// <summary>
        /// ��ʾ��̬�ֶ�ֵ
        /// </summary>
        /// <param name="initStaticFields">��ʾǰ�Ƿ���ֶν��г�ʼ��</param>
        public static void DisplayStaticFieldValue(object initStaticFields)
        {
            //���߳��Դ˷�����������ThreadStaticAttribute���εľ�̬�ֶ��ڴ˽��г�ʼ��
            if ((bool) initStaticFields) InitStaticFields();

            string msg = string.Format("{0,4}{1,4}{2,4}\t{3}", Thread.CurrentThread.ManagedThreadId,
                                       Thread.CurrentThread.IsThreadPoolThread ? "�̳߳�" : "��ͨ", Thread.CurrentThread.Name,
                                       Bar);
            Console.WriteLine(msg);

            //Ϊ�˲���ThreadStatic�ֶ����̳߳صĹ��������
            //���߳������ӳ���ʹ�̳߳�����������ͬһ����ָ�й��߳�Id����ͬ�ģ��̡߳�
            Thread.Sleep(100);
        }

        /// <summary>
        /// ��ʼ���߳̾�̬�ֶ�
        /// </summary>
        private static void InitStaticFields()
        {
            Bar = "�ѳ�ʼ"; //��ʼ��Bar��̬�ֶ�
        }

        /// <summary>
        /// ���Դ���
        /// </summary>
        public static void TestStaticField()
        {
            DisplayStaticFieldValue(true); //�ڵ�ǰ�̣߳���һ�Σ���ʼ��Bar��̬�ֶΣ���ʾ��ֵ

            var t1 = new Thread(DisplayStaticFieldValue) {Name = "t1"};
            t1.Start(true); //��t1�߳��ϳ�ʼ��Bar����ʾ��ֵ

            var t2 = new Thread(DisplayStaticFieldValue) {Name = "t2"};
            t2.Start(false); //��t2�߳���δ��ʼ��Bar����ʾ��ֵ

            //ʹ��QueueUserWorkItem���������̳߳ض��У���ô���̳߳��߳��ϣ�
            //����̵߳�Id����ͬ����ʹ��ͬһ��ThreadStatic�ֶ�
            ThreadPool.QueueUserWorkItem(DisplayStaticFieldValue, true); //��ʼ��
            ThreadPool.QueueUserWorkItem(DisplayStaticFieldValue, false); //������δ��ʼ��
            ThreadPool.QueueUserWorkItem(DisplayStaticFieldValue, false); //������δ��ʼ��
            ThreadPool.QueueUserWorkItem(DisplayStaticFieldValue, false); //������δ��ʼ��
            ThreadPool.QueueUserWorkItem(DisplayStaticFieldValue, false); //������δ��ʼ��
            ThreadPool.QueueUserWorkItem(DisplayStaticFieldValue, false); //������δ��ʼ��
            //�鿴�����֪���е��̳߳��߳̾�̬�ֶβ��������ǵ�����ֵ����ʵֵ�ƺ����߳�Id���й�ϵ
            //����ThreadStatic���Եľ�̬�ֶ���ʹ���̳߳��߳�ʱ��Ҫע����������ķ���

            DisplayStaticFieldValue(false); //�ڵ�ǰ�߳�����ʾBar��̬�ֶε�ֵ��ǰ���Ѿ���ʼ������
        }
    }
}