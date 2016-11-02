using System;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SymmetricAlgorithm_Demo
{
    class Program
    {
        [STAThread]
        private static void Main()
        {
            string plainText = "这是一个测试文本..";
            //TEST1
            {
                Console.WriteLine("------ 自定义扩展的加密解密[字符串] ------");
                string key = "oyi319";
                string result1 = plainText.Encrypt(key); //用oyi319作为Key加密数据
                var result2 = result1.Decrypt(key); //用oyi319作为Key解密数据
                Console.WriteLine("加密数据：" + result1); //注意输出编辑进行了Base64转换，并非原始字节数据
                Console.WriteLine("解密数据：" + result2);
                Console.WriteLine("用户Key：" + key + "（此Key与对称算法中的Key可能不是一个，详细见扩展代码）");
            }
            //TEST2
            {
                Console.WriteLine("\n------ SymmetricAlgorithm的加密解密[字符串] ------");
                var des = new DESCryptoServiceProvider(); //des的键和向量会随机产生一组
                var result1 = MySymmetricAlgorithm.Encrypt(plainText, des).BytesToString(); //将加密结果的字节转换为字符输出
                var result2 = MySymmetricAlgorithm.Decrypt(result1.HexStringToBytes(), des); //用同样的密钥进行解密
                Console.WriteLine("加密数据：" + result1);
                Console.WriteLine("解密数据：" + result2);
                Console.WriteLine("Key:{0}   IV:{1}", des.Key.BytesToString(), des.IV.BytesToString());
                des.Clear(); //清除安全数据且释放资源
            }
            //TEST3
            {
                Console.WriteLine("\n------ SymmetricAlgorithm的加密解密[文件] ------");
                var algName = "DES";
                var key = MyExtendMethod.GetKey();
                var iv = MyExtendMethod.GetIv();
                Console.WriteLine("算法: {0}   Key: {1}   IV: {2}", algName, key.BytesToString(),
                                  iv.BytesToString());
                var openFileDialog = new OpenFileDialog {Title = "请选择一个数据文件", FileName = ""};
                openFileDialog.ShowDialog();
                var fileName = openFileDialog.FileName;
                if(File.Exists(fileName))
                {
                    Console.WriteLine("[+] 输入文件：" + fileName);
                    var encryptFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                                                   Path.GetFileName(fileName) + ".encrypt");
                    var symmetric = new MySymmetricAlgorithm(); //声明MySymmetricAlgorithm对象
                    var rijndaelManaged = new RijndaelManaged();
                    Console.WriteLine("[+] 输出加密文件：" + encryptFile);
                    var t =
                        new Thread(
                            () =>
                            symmetric.EncryptData(fileName, encryptFile, algName, key, iv, ProgressHandle, EndHandle,
                                                  symmetric))
                            {
                                Name = "EncryptData",
                                IsBackground = true
                            };
                    t.Start();
                    t.Join();

                    Console.Write("按任意键开始对此文件进行解密测试...");
                    Console.ReadKey(true);
                    var decryptFile = Path.ChangeExtension(encryptFile, ".decrypt");
                    Console.WriteLine("\r[+] 输出解密文件：" + decryptFile);
                    var t1 =
                        new Thread(
                            () =>
                            symmetric.DecryptData(encryptFile, decryptFile, algName, key, iv, ProgressHandle, EndHandle,
                                                  symmetric))
                        {
                            Name = "DecryptData",
                            IsBackground = true
                        };
                    t1.Start();
                    t1.Join();

                }
                else
                {
                    Console.WriteLine("*ERR* 没有选择数据文件，不进行文件加密解密测试。");
                }
            }

            Console.Write("\n__全部测试结束__按任意键退出...");
            Console.ReadKey(true);
        }

        //处理文件加密解密的结束回调
        private static void EndHandle(MySymmetricAlgorithm.EndState endstate)
        {
            var msg = endstate.IsCancel ? "\n[-] 中途取消" : "\n[+] 计算完成";
            Console.WriteLine(msg);
        }

        //处理文件加密解密的进度回调
        private static void ProgressHandle(MySymmetricAlgorithm.ProgressState progressstate)
        {
            var percent = (int) ((float) progressstate.BytesRead/progressstate.TotalBytesLength*100);
            Console.Write("\r[+] 正在计算 ...{0}%    ", percent);
        }
    }

    public static class MyExtendMethod
    {
        /// <summary>
        /// 16进制格式字符串转换为字节数组
        /// </summary>
        public static byte[] HexStringToBytes(this string hexString)
        {
            string s = hexString.Replace(" ", "").Replace("-", "");
            int length = s.Length;
            if (length < 2 || length % 2 != 0)
            {
                return new byte[0];
            }
            var buffer = new byte[length / 2];
            for (int i = 0, j = 0; i < length; i += 2, j++)
            {
                buffer[j] = Byte.Parse(s.Substring(i, 2), NumberStyles.HexNumber);
            }
            return buffer;
        }

        /// <summary>
        /// 字节数组转换为显示字符串
        /// </summary>
        public static string BytesToString(this byte[] buffer)
        {
            return BitConverter.ToString(buffer).Replace("-", "").ToLower();
        }
        
        
        
        //------ 简单字符串加密解密的扩展 ------

        //指定对称算法的向量
        private static readonly byte[] Iv = new byte[] { 0xe4, 0x00, 0x18, 0x4e, 0xf5, 0x4d, 0x24, 0xed };

        //指定对称算法的默认键
        private static readonly byte[] Key = new byte[] { 0x2f, 0xd6, 0x84, 0xe2, 0x37, 0x95, 0xcf, 0x26 };

        public static byte[] GetKey()
        {
            return Key;
        }

        public static byte[] GetIv()
        {
            return Iv;
        }

        //指定对称算法（可以为 Rijndael、DES、RC2和TripleDES）
        private const string AlgName = "DES";

        /// <summary>
        /// 加密数据 - 按AlgName常量的算法加密数据，返回加密数据的Base64字符串。
        /// </summary>
        /// <param name="plainText">明文</param>
        /// <param name="key">有8位字符的键</param>
        /// <returns>加密后的Base64字符串</returns>
        public static string Encrypt(this string plainText, string key)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(key); //用ASCII编辑获取字节
            var buffer = (byte[])Key.Clone(); //得到默认键
            Buffer.BlockCopy(bytes, 0, buffer, 0, bytes.Length < 8 ? bytes.Length : 8); //对输入键处理，保证键大小是8位的
            byte[] cypher = MySymmetricAlgorithm.Encrypt(plainText, AlgName, buffer, Iv); //进行加密
            return Convert.ToBase64String(cypher); //转换为Base64编码
        }

        /// <summary>
        /// 解密数据 - 按AlgName常量的算法加密数据，输入Encrypt方法得到的Base64编码，返回明文。
        /// 解密失败将导致异常。
        /// </summary>
        /// <param name="cypherText">加密后的Base64编码形式字符串</param>
        /// <param name="key">有8位字符的键</param>
        /// <returns>明文</returns>
        public static string Decrypt(this string cypherText, string key)
        {
            byte[] cypher = Convert.FromBase64String(cypherText); //将Base64解码为字节数组
            byte[] bytes = Encoding.ASCII.GetBytes(key);
            var buffer = (byte[])Key.Clone();
            Buffer.BlockCopy(bytes, 0, buffer, 0, bytes.Length < 8 ? bytes.Length : 8);
            return MySymmetricAlgorithm.Decrypt(cypher, AlgName, buffer, Iv);
        }
    }
}