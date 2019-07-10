using System;
using System.Runtime.Remoting.Messaging;
using System.Threading;

namespace thread_test.s3_asyncCallback
{
    //声明一个与异步调用方法参数和返回值签名相同的委托，它支持out和ref参数
    public delegate double AsyncInvoke(double a, double b, out double c);

    public class TestAsyncInvoke
    {
        /// <summary>
        /// 已经直角三角形的两直角边a和b的长度，求夹角ac的孤度值和斜边c的长度
        /// </summary>
        /// <param name="a">对边长度</param>
        /// <param name="b">临边长度</param>
        /// <param name="c">斜边长度</param>
        /// <returns>夹角ac的孤度值</returns>
        public static double ComputeRtAngle(double a, double b, out double c)
        {
            Console.WriteLine("{0} 计算线程：TestAsyncInvoke.ComputeRtAngle 计算直角三角形", Thread.CurrentThread.ManagedThreadId);
            c = Math.Sqrt(a * a + b * b); //得到斜边长度
            var angle = Math.Atan(a / b); //得到a,c边夹角的弧度值
            return angle;
        }

        /// <summary>
        /// 测试代码
        /// </summary>
        public static void TestAsyncDelegate()
        {
            var aa = new AsyncAction();
            aa.CallbackAsyncDelegate(); //进行演示
        }
    }


    public class AsyncAction
    {
        public void CallbackAsyncDelegate()
        {
            AsyncCallback callback = DelegateCallback; //回调函数的委托
            AsyncInvoke computeRtAngle = TestAsyncInvoke.ComputeRtAngle; //异步调用的委托
            Console.WriteLine("{0} 调用线程：执行BeginInvoke前的代码", Thread.CurrentThread.ManagedThreadId);
            double a = 5;
            double b = 9;
            double c; //注意参数c使用out传递，同样支持ref参数传递
            IAsyncResult asyncResult = computeRtAngle.BeginInvoke(a, b, out c, callback, computeRtAngle); //异步启动委托
            Console.WriteLine("{0} 调用线程：执行BeginInvoke后的代码, a={1}, b={2}, c={3} ——参数c使用out传递",
                              Thread.CurrentThread.ManagedThreadId, a, b, c);
            //异步调用，用EndInvoke才能得到斜边c及最终的结果，此示例的结果是在回调函数里获得的
        }

        /// <summary>
        /// 异步委托完成的回调函数
        /// </summary>
        /// <param name="ar">异步结果</param>
        public static void DelegateCallback(IAsyncResult ar)
        {
            var asyncResult = (AsyncResult)ar; //将结果转换为AsyncResult对象
            var asyncInvoke = (AsyncInvoke)asyncResult.AsyncDelegate; //得到异步调用的委托
            double c; //计算的out参数返回值（斜边长度）
            double angle = asyncInvoke.EndInvoke(out c, asyncResult); //得到异步调用方法的最终结果
            Console.WriteLine("{0} 回调函数，结果：angle={1}={2}°,c={3}", Thread.CurrentThread.ManagedThreadId,
                              angle, angle * (180 / Math.PI), c); //注意：回调函数工作在异步委托线程里。
        }
    }
}