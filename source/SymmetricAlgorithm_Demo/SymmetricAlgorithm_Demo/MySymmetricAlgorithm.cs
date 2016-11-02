using System.IO;
using System.Security.Cryptography;

namespace SymmetricAlgorithm_Demo
{
    public class MySymmetricAlgorithm
    {
        /// <summary>
        /// 按指定对称密钥加密字符串
        /// </summary>
        public static byte[] Encrypt(string plainText, SymmetricAlgorithm alg)
        {
            var ms = new MemoryStream();
            var encStream = new CryptoStream(ms, alg.CreateEncryptor(), CryptoStreamMode.Write);
            var sw = new StreamWriter(encStream);
            sw.WriteLine(plainText);
            sw.Close();
            encStream.Close();
            var buffer = ms.ToArray();
            ms.Close();
            return buffer;
        }

        /// <summary>
        /// 按指定对称密钥解密数据
        /// </summary>
        public static string Decrypt(byte[] cypherText, SymmetricAlgorithm alg)
        {
            var ms = new MemoryStream(cypherText);
            var encStream = new CryptoStream(ms, alg.CreateDecryptor(), CryptoStreamMode.Read);
            var sr = new StreamReader(encStream);
            var val = sr.ReadLine();
            sr.Close();
            encStream.Close();
            ms.Close();
            return val;
        }

        /// <summary>
        /// 按指定对称算法、键和向量加密字符串
        /// </summary>
        public static byte[] Encrypt(string plainText, string algName, byte[] rgbKey, byte[] rgbIv)
        {
            var alg = SymmetricAlgorithm.Create(algName);
            var transform = alg.CreateEncryptor(rgbKey, rgbIv);
            var ms = new MemoryStream();
            var encStream = new CryptoStream(ms, transform, CryptoStreamMode.Write);
            var sw = new StreamWriter(encStream);
            sw.WriteLine(plainText);
            sw.Close();
            encStream.Close();
            var buffer = ms.ToArray();
            ms.Close();
            return buffer;
        }

        /// <summary>
        /// 按指定对称算法、键和向量解密数据
        /// </summary>
        public static string Decrypt(byte[] cypherText, string algName, byte[] rgbKey, byte[] rgbIv)
        {
            var alg = SymmetricAlgorithm.Create(algName);
            var transform = alg.CreateDecryptor(rgbKey, rgbIv);
            var ms = new MemoryStream(cypherText);
            var encStream = new CryptoStream(ms, transform, CryptoStreamMode.Read);
            var sr = new StreamReader(encStream);
            var val = sr.ReadLine();
            sr.Close();
            encStream.Close();
            ms.Close();
            return val;
        }

        
        //------下面为对文件/流加密解密的需要实例化的部分------

        /// <summary>
        /// 完成计算的回调
        /// </summary>
        public delegate void EndCallback(EndState endState);

        /// <summary>
        /// 进行计算时的回调
        /// </summary>
        public delegate void ProgressCallback(ProgressState progressState);

        private bool _cancel; //取消计算

        /// <summary>
        /// 停止计算
        /// </summary>
        public void Stop()
        {
            _cancel = true;
        }

        /// <summary>
        /// 按指定对称算法、键和向量将输入文件加密结果保存为输出文件
        /// </summary>
        /// <param name="inFileName">输入文件</param>
        /// <param name="outFileName">输出文件</param>
        /// <param name="algName">对称算法名称</param>
        /// <param name="rgbKey">键</param>
        /// <param name="rgbIv">向量</param>
        /// <param name="progressCallback">进度回调函数</param>
        /// <param name="endCallback">结束计算的回调函数</param>
        /// <param name="state">传递给回调的参数</param>
        public void EncryptData(string inFileName, string outFileName, string algName, byte[] rgbKey, byte[] rgbIv, ProgressCallback progressCallback, EndCallback endCallback, object state)
        {
            var alg = SymmetricAlgorithm.Create(algName);
            alg.Key = rgbKey;
            alg.IV = rgbIv;
            EncryptData(inFileName, outFileName, alg, progressCallback, endCallback, state);
        }

        /// <summary>
        /// 按指定对称密钥将输入文件加密结果保存为输出文件
        /// </summary>
        /// <param name="inFileName">输入文件</param>
        /// <param name="outFileName">输出文件</param>
        /// <param name="alg">对称密钥</param>
        /// <param name="progressCallback">进度回调函数</param>
        /// <param name="endCallback">结束计算的回调函数</param>
        /// <param name="state">传递给回调的参数</param>
        public void EncryptData(string inFileName, string outFileName, SymmetricAlgorithm alg, ProgressCallback progressCallback, EndCallback endCallback, object state)
        {
            using (var infs = new FileStream(inFileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (var outfs = new FileStream(outFileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read))
                {
                    EncryptData(infs, outfs, alg, progressCallback, endCallback, state);
                }
            }
        }

        /// <summary>
        /// 按指定对称密钥将输入流加密结果保存为输出流
        /// </summary>
        /// <param name="inStream">输入流（可读）</param>
        /// <param name="outStream">输出流（可写）</param>
        /// <param name="alg">对称密钥</param>
        /// <param name="progressCallback">进度回调函数</param>
        /// <param name="endCallback">结束计算的回调函数</param>
        /// <param name="state">传递给回调的参数</param>
        public void EncryptData(Stream inStream, Stream outStream, SymmetricAlgorithm alg, ProgressCallback progressCallback, EndCallback endCallback, object state)
        {
            const int bufferSize = 100;
            _cancel = false;

            long bytesRead = 0L;
            long totalBytesLength = inStream.Length;
            var buffer = new byte[bufferSize];

            using (var encStream = new CryptoStream(outStream, alg.CreateEncryptor(), CryptoStreamMode.Write))
            {
                int num;
                do
                {
                    num = inStream.Read(buffer, 0, bufferSize);
                    encStream.Write(buffer, 0, num);
                    if (progressCallback == null) continue;
                    bytesRead += num;
                    progressCallback(new ProgressState(bytesRead, totalBytesLength, state)); //进度回调
                } while (num > 0 && !_cancel);
                if (endCallback != null) endCallback(new EndState(_cancel, state)); //计算结束回调
            }
        }

        /// <summary>
        /// 按指定对称算法、键和向量将输入文件解密结果保存为输出文件
        /// </summary>
        /// <param name="inFileName">输入文件</param>
        /// <param name="outFileName">输出文件</param>
        /// <param name="algName">对称算法名称</param>
        /// <param name="rgbKey">键</param>
        /// <param name="rgbIv">向量</param>
        /// <param name="progressCallback">进度回调函数</param>
        /// <param name="endCallback">结束计算的回调函数</param>
        /// <param name="state">传递给回调的参数</param>
        public void DecryptData(string inFileName, string outFileName, string algName, byte[] rgbKey, byte[] rgbIv, ProgressCallback progressCallback, EndCallback endCallback, object state)
        {
            var alg = SymmetricAlgorithm.Create(algName);
            alg.Key = rgbKey;
            alg.IV = rgbIv;
            DecryptData(inFileName, outFileName, alg, progressCallback, endCallback, state);
        }

        /// <summary>
        /// 按指定对称密钥将输入文件解密结果保存为输出文件
        /// </summary>
        /// <param name="inFileName">输入文件</param>
        /// <param name="outFileName">输出文件</param>
        /// <param name="alg">对称密钥</param>
        /// <param name="progressCallback">进度回调函数</param>
        /// <param name="endCallback">结束计算的回调函数</param>
        /// <param name="state">传递给回调的参数</param>
        public void DecryptData(string inFileName, string outFileName, SymmetricAlgorithm alg, ProgressCallback progressCallback, EndCallback endCallback, object state)
        {
            using (var infs = new FileStream(inFileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (var outfs = new FileStream(outFileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read))
                {
                    DecryptData(infs, outfs, alg, progressCallback, endCallback, state);
                }
            }
        }

        /// <summary>
        /// 按指定对称密钥将输入流解密结果保存为输出流
        /// </summary>
        /// <param name="inStream">输入流（可读）</param>
        /// <param name="outStream">输出流（可写）</param>
        /// <param name="alg">对称密钥</param>
        /// <param name="progressCallback">进度回调函数</param>
        /// <param name="endCallback">结束计算的回调函数</param>
        /// <param name="state">传递给回调的参数</param>
        public void DecryptData(Stream inStream, Stream outStream, SymmetricAlgorithm alg, ProgressCallback progressCallback, EndCallback endCallback, object state)
        {
            const int bufferSize = 100;
            _cancel = false;

            long bytesRead = 0L;
            long totalBytesLength = inStream.Length;
            var buffer = new byte[bufferSize];

            using (var encStream = new CryptoStream(inStream, alg.CreateDecryptor(), CryptoStreamMode.Read))
            {
                int num;
                do
                {
                    num = encStream.Read(buffer, 0, bufferSize);
                    outStream.Write(buffer, 0, num);
                    if (progressCallback == null) continue;
                    bytesRead += num;
                    progressCallback(new ProgressState(bytesRead, totalBytesLength, state)); //进度回调
                } while (num > 0 && !_cancel);
                if (endCallback != null) endCallback(new EndState(_cancel, state)); //计算结束回调
            }
        }


        /// <summary>
        /// 为EndCallback提供数据
        /// </summary>
        public class EndState
        {
            internal EndState(bool isCancel, object state)
            {
                IsCancel = isCancel;
                State = state;
            }

            /// <summary>
            /// 是否取消退出的
            /// </summary>
            public bool IsCancel { get; private set; }

            /// <summary>
            /// 获得传递的参数
            /// </summary>
            public object State { get; private set; }
        }


        /// <summary>
        /// 为ProgressCallback提供数据
        /// </summary>
        public class ProgressState
        {
            internal ProgressState(long byteRead, long totalBytesLength, object state)
            {
                BytesRead = byteRead;
                TotalBytesLength = totalBytesLength;
                State = state;
            }

            /// <summary>
            /// 获得已经计算完成的字节数
            /// </summary>
            public long BytesRead { get; private set; }

            /// <summary>
            /// 获得总字节数
            /// </summary>
            public long TotalBytesLength { get; private set; }

            /// <summary>
            /// 获得传递的参数
            /// </summary>
            public object State { get; private set; }
        }
    }
}

/*
 * algName:
 * Rijndael     - RijndaelManaged
 * DES          - DESCryptoServiceProvider
 * RC2          - RC2CryptoServiceProvider
 * TripleDES    - TripleDESCryptoServiceProvider
 */