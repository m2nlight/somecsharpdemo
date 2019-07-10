using System;
using System.Runtime.Remoting.Messaging;
using System.Threading;

namespace thread_test.s3_asyncCallback
{
    //����һ�����첽���÷��������ͷ���ֵǩ����ͬ��ί�У���֧��out��ref����
    public delegate double AsyncInvoke(double a, double b, out double c);

    public class TestAsyncInvoke
    {
        /// <summary>
        /// �Ѿ�ֱ�������ε���ֱ�Ǳ�a��b�ĳ��ȣ���н�ac�Ĺ¶�ֵ��б��c�ĳ���
        /// </summary>
        /// <param name="a">�Ա߳���</param>
        /// <param name="b">�ٱ߳���</param>
        /// <param name="c">б�߳���</param>
        /// <returns>�н�ac�Ĺ¶�ֵ</returns>
        public static double ComputeRtAngle(double a, double b, out double c)
        {
            Console.WriteLine("{0} �����̣߳�TestAsyncInvoke.ComputeRtAngle ����ֱ��������", Thread.CurrentThread.ManagedThreadId);
            c = Math.Sqrt(a * a + b * b); //�õ�б�߳���
            var angle = Math.Atan(a / b); //�õ�a,c�߼нǵĻ���ֵ
            return angle;
        }

        /// <summary>
        /// ���Դ���
        /// </summary>
        public static void TestAsyncDelegate()
        {
            var aa = new AsyncAction();
            aa.CallbackAsyncDelegate(); //������ʾ
        }
    }


    public class AsyncAction
    {
        public void CallbackAsyncDelegate()
        {
            AsyncCallback callback = DelegateCallback; //�ص�������ί��
            AsyncInvoke computeRtAngle = TestAsyncInvoke.ComputeRtAngle; //�첽���õ�ί��
            Console.WriteLine("{0} �����̣߳�ִ��BeginInvokeǰ�Ĵ���", Thread.CurrentThread.ManagedThreadId);
            double a = 5;
            double b = 9;
            double c; //ע�����cʹ��out���ݣ�ͬ��֧��ref��������
            IAsyncResult asyncResult = computeRtAngle.BeginInvoke(a, b, out c, callback, computeRtAngle); //�첽����ί��
            Console.WriteLine("{0} �����̣߳�ִ��BeginInvoke��Ĵ���, a={1}, b={2}, c={3} ��������cʹ��out����",
                              Thread.CurrentThread.ManagedThreadId, a, b, c);
            //�첽���ã���EndInvoke���ܵõ�б��c�����յĽ������ʾ���Ľ�����ڻص��������õ�
        }

        /// <summary>
        /// �첽ί����ɵĻص�����
        /// </summary>
        /// <param name="ar">�첽���</param>
        public static void DelegateCallback(IAsyncResult ar)
        {
            var asyncResult = (AsyncResult)ar; //�����ת��ΪAsyncResult����
            var asyncInvoke = (AsyncInvoke)asyncResult.AsyncDelegate; //�õ��첽���õ�ί��
            double c; //�����out��������ֵ��б�߳��ȣ�
            double angle = asyncInvoke.EndInvoke(out c, asyncResult); //�õ��첽���÷��������ս��
            Console.WriteLine("{0} �ص������������angle={1}={2}��,c={3}", Thread.CurrentThread.ManagedThreadId,
                              angle, angle * (180 / Math.PI), c); //ע�⣺�ص������������첽ί���߳��
        }
    }
}