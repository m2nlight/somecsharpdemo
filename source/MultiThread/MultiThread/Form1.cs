using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Oyi319.Demo.DotNet.MultiThread
{
    public partial class Form1 : Form
    {
        private const int PrimesFrom = 1;
        private const int PrimesTo = 10000;

        private readonly List<int> _results = new List<int>();
        private readonly Stopwatch _stopwatch = new Stopwatch();
        private readonly ReaderWriterLockSlim _readerWriterLockSlim = new ReaderWriterLockSlim();

        public Form1()
        {
            InitializeComponent();

            toolTip1.SetToolTip(button1, "methon 1: BackgroundWorker");
            toolTip1.SetToolTip(button2, "methon 1: BackgroundWorker");
            toolTip1.SetToolTip(button3, "method 2: Delegate");
            toolTip1.SetToolTip(button4, "method 3: ThreadPool");
            toolTip1.SetToolTip(button5, "method 4: Thread");
            toolTip1.SetToolTip(button6, "method 5: Task");
            toolTip1.SetToolTip(button7, "method 5: Task");
            toolTip1.SetToolTip(button8, "method 5: Parallel Task");
            toolTip1.SetToolTip(button9, "method 5: Parallel Task");
            toolTip1.SetToolTip(button10, "Result to sort.");
            toolTip1.SetToolTip(listBox1, "Result");

            this.Text = string.Format("{0}-{1} Primes - MultiThread Demo - F1 to BLOG", PrimesFrom, PrimesTo);
            this.KeyPreview = true;
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            if (e.KeyCode.Equals(Keys.F1))
            {
                Process.Start("http://blog.csdn.net/oyi319");
            }
        }

        /// <summary>
        /// UI updated
        /// </summary>
        /// <param name="isCalculating">true for starting, false for completed</param>
        void DisplayCalculate(bool isCalculating)
        {
            if (isCalculating)
            {
                button1.Enabled = false;
                button3.Enabled = false;
                button4.Enabled = false;
                button5.Enabled = false;
                button6.Enabled = false;
                button8.Enabled = false;
                progressBar1.Value = 0;
                listBox1.Items.Clear();
                this.Cursor = Cursors.AppStarting;
                _count = 0;
                _stopwatch.Restart();
            }
            else
            {
                _stopwatch.Stop();
                listBox1.Items.Clear();
                foreach (var result in _results)
                {
                    listBox1.Items.Add(result.ToString());
                }

                this.Text = string.Format("ListItems count: {0}, Elapaed: {1}", listBox1.Items.Count, _stopwatch.Elapsed);

                button1.Enabled = true;
                button2.Enabled = false;
                button3.Enabled = true;
                button4.Enabled = true;
                button5.Enabled = true;
                button6.Enabled = true;
                button7.Enabled = false;
                button8.Enabled = true;
                button9.Enabled = false;
                this.Cursor = Cursors.Default;
            }
        }

        //------ BackgroundWorker ------

        private void button1_Click(object sender, EventArgs e)
        {
            DisplayCalculate(true);
            button2.Enabled = true;
            this.Text = @"Calculating in BackgroundWorker method...";
            backgroundWorker1.RunWorkerAsync(new Parameters(PrimesFrom, PrimesTo));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            backgroundWorker1.CancelAsync();
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            DisplayCalculate(false);
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            var parameters = (Parameters)e.Argument;
            _results.Clear();
            for (int count = parameters.Min; count <= parameters.Max; count += 2)
            {
                //Debug.WriteLine(count);
                bool isPrime = true;
                for (int x = 1; x <= count / 2; x++)
                {
                    for (int y = 1; y <= x; y++)
                    {
                        if (x * y == count)
                        {
                            isPrime = false;
                            break;
                        }
                    }
                    if (!isPrime)
                    {
                        break;
                    }
                }
                if (isPrime)
                {
                    _results.Add(count);
                }

                var p = (int)((count - parameters.Min) / (double)(parameters.Max - parameters.Min) * 100);
                backgroundWorker1.ReportProgress(p);

                if (backgroundWorker1.CancellationPending)
                {
                    return;
                }
            }

            backgroundWorker1.ReportProgress(100);
        }

        //------ Delegate ------

        class Parameters
        {
            public int Min { get; private set; }
            public int Max { get; private set; }

            public Parameters(int min, int max)
            {
                this.Min = min;
                this.Max = max;
            }
        }

        private void TaskCompleted()
        {
            progressBar1.Value = 100;
            DisplayCalculate(false);
        }

        private static int _count;
        private void DisplayProgress(Parameters parameters, int step)
        {
            Interlocked.Increment(ref _count);
            var p = (int)(_count / (double)(parameters.Max - parameters.Min) * 100 * step);
            progressBar1.Value = p;
        }

        void FindPrimesViaDelegate(Parameters parameters)
        {
            var progress = new Action<Parameters, int>(DisplayProgress);

            _results.Clear();
            for (int count = parameters.Min; count <= parameters.Max; count += 2)
            {
                //Debug.WriteLine(count);
                var isPrime = true;
                for (int x = 1; x <= count / 2; x++)
                {
                    for (int y = 1; y <= x; y++)
                    {
                        if (x * y == count)
                        {
                            isPrime = false;
                            break;
                        }
                    }

                    if (!isPrime)
                    {
                        break;
                    }
                }

                if (isPrime)
                {
                    _results.Add(count);
                }

                if (this.InvokeRequired)
                {
                    this.BeginInvoke(progress, parameters, 2);
                }
            }

            var update = new Action(TaskCompleted);
            if (this.InvokeRequired)
            {
                this.BeginInvoke(update);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var mr = MessageBox.Show(@"This demo will not be cancelled.
Continue?"
                                     , @"NOTE", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (mr != DialogResult.Yes)
                return;

            DisplayCalculate(true);
            this.Text = @"Calculating in Delegate method...";
            var worker = new Action<Parameters>(FindPrimesViaDelegate);
            worker.BeginInvoke(new Parameters(PrimesFrom, PrimesTo), TaskComplete, null);
        }

        private void TaskComplete(IAsyncResult ar)
        {
            var update = new Action<bool>(DisplayCalculate);
            this.BeginInvoke(update, false);
        }

        //------ ThreadPool ------

        void FindPrimes(object state)
        {
            var parameters = (Parameters)state;
            var progress = new Action<Parameters, int>(DisplayProgress);

            _results.Clear();
            for (int count = parameters.Min; count <= parameters.Max; count += 2)
            {
                //Debug.WriteLine(count);
                var isPrime = true;
                for (int x = 1; x <= count / 2; x++)
                {
                    for (int y = 1; y <= x; y++)
                    {
                        if (x * y == count)
                        {
                            isPrime = false;
                            break;
                        }
                    }
                    if (!isPrime)
                    {
                        break;
                    }
                }
                if (isPrime)
                {
                    _results.Add(count);
                }
                if (this.InvokeRequired)
                {
                    this.BeginInvoke(progress, parameters, 2);
                }
            }

            var update = new Action(TaskCompleted);
            if (this.InvokeRequired)
            {
                this.BeginInvoke(update);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var mr = MessageBox.Show(@"This demo will not be cancelled.
Continue?"
                                     , @"NOTE", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (mr != DialogResult.Yes)
                return;

            DisplayCalculate(true);
            this.Text = @"Calculating in ThreadPool method...";
            ThreadPool.QueueUserWorkItem(FindPrimes, new Parameters(PrimesFrom, PrimesTo));
        }

        //------ Thread ------

        private void button5_Click(object sender, EventArgs e)
        {
            var mr = MessageBox.Show(@"This demo will not be cancelled.
Continue?"
                                     , @"NOTE", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (mr != DialogResult.Yes)
                return;

            DisplayCalculate(true);
            this.Text = @"Calculating in Thread method...";
            var t = new Thread(FindPrimes)
                        {
                            Name = "FindPrimes",
                            IsBackground = true
                        };
            t.Start(new Parameters(PrimesFrom, PrimesTo));
        }

        //------ Task ------

        private CancellationTokenSource _tokenSource;

        private void button6_Click(object sender, EventArgs e)
        {
            DisplayCalculate(true);
            button7.Enabled = true;
            this.Text = @"Calculating in Task method...";
            _tokenSource = new CancellationTokenSource();
            Task.Factory.StartNew(FindPrimesInTask, new Parameters(PrimesFrom, PrimesTo), _tokenSource.Token);
        }

        private void FindPrimesInTask(object obj)
        {
            var parameters = (Parameters)obj;
            var progress = new Action<Parameters, int>(DisplayProgress);

            _results.Clear();
            for (int count = parameters.Min; count <= parameters.Max; count += 2)
            {
                //Debug.WriteLine(count);
                var isPrime = true;
                for (int x = 1; x <= count / 2; x++)
                {
                    for (int y = 1; y <= x; y++)
                    {
                        if (x * y == count)
                        {
                            isPrime = false;
                            break;
                        }
                    }
                    if (!isPrime)
                    {
                        break;
                    }
                }
                if (isPrime)
                {
                    _results.Add(count);
                }

                if (this.InvokeRequired)
                {
                    this.BeginInvoke(progress, parameters, 2);
                }

                if (_tokenSource.IsCancellationRequested)
                {
                    break;
                }
            }

            if (_tokenSource.IsCancellationRequested)
            {
                var update = new Action<bool>(DisplayCalculate);
                if (this.InvokeRequired)
                {
                    this.BeginInvoke(update, false);
                }
            }
            else
            {
                var completed = new Action(TaskCompleted);
                if (this.InvokeRequired)
                {
                    this.BeginInvoke(completed);
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            button7.Enabled = false;
            _tokenSource.Cancel();
        }

        //------ Parallel Task ------

        private void button8_Click(object sender, EventArgs e)
        {
            DisplayCalculate(true);
            button9.Enabled = true;
            this.Text = @"Calculating in Parallel Task method...";
            _tokenSource = new CancellationTokenSource();
            Task.Factory.StartNew(FindPrimesInParallel, new Parameters(PrimesFrom, PrimesTo), _tokenSource.Token);
        }

        private void FindPrimesInParallel(object obj)
        {
            var parameters = (Parameters)obj;
            var progress = new Action<Parameters, int>(DisplayProgress);

            _results.Clear();
            Parallel.For(parameters.Min, parameters.Max,
                         (count, loop) =>
                         {
                             if (count % 2 == 0)
                             {
                                 return;
                             }
                             //Debug.WriteLine(count);
                             var isPrime = true;
                             for (int x = 1; x <= count / 2; x++)
                             {
                                 for (int y = 1; y <= x; y++)
                                 {
                                     if (x * y == count)
                                     {
                                         isPrime = false;
                                         break;
                                     }
                                 }
                                 if (!isPrime)
                                 {
                                     break;
                                 }
                             }
                             if (isPrime)
                             {
                                 _readerWriterLockSlim.EnterWriteLock();
                                 try
                                 {
                                     _results.Add(count);
                                 }
                                 finally
                                 {
                                     _readerWriterLockSlim.ExitWriteLock();
                                 }
                             }

                             if (this.InvokeRequired)
                             {
                                 this.BeginInvoke(progress, parameters, 2);
                             }

                             if (_tokenSource.IsCancellationRequested)
                             {
                                 loop.Stop();
                             }
                         });

            if (_tokenSource.IsCancellationRequested)
            {
                var update = new Action<bool>(DisplayCalculate);
                this.BeginInvoke(update, false);
            }
            else
            {
                var completed = new Action(TaskCompleted);
                this.BeginInvoke(completed);
            }

        }

        private void button10_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            _readerWriterLockSlim.EnterReadLock();
            try
            {
                foreach (var item in _results.OrderBy(n => n).Select(n => n.ToString()))
                {
                    listBox1.Items.Add(item);
                }
            }
            finally
            {
                _readerWriterLockSlim.ExitReadLock();
            }
        }
    }
}
