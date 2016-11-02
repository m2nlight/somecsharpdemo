using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace Utility
{
    public static class ExtandMethod
    {
        #region �ַ������ֽ�����ת��

        /// <summary>
        /// 16���Ƹ�ʽ�ַ���ת��Ϊ�ֽ�����
        /// </summary>
        /// <returns>�����ֽ�����</returns>
        /// <exception cref="ArgumentNullException">�������޷�ת��</exception>
        /// <exception cref="ArgumentException">�޷�ת��</exception>
        /// <remarks>
        /// 16���Ƹ�ʽ��Ҫ��ÿ���ֽ���2λʮ����������ʾ������2λ�벹0����ת�����̻�ȥ���ո�͡�-�����ţ�
        /// �����ִ�Сд�� 
        /// ���漸������ͬ�ģ�
        /// 89a701fe
        /// 89 a7 01 fe
        /// 89-a7-01-fe
        /// </remarks>
        public static byte[] ConvertToByteArray(this string hexString)
        {
            if (hexString == null) throw new ArgumentNullException("hexString");

            var s = hexString.Replace(" ", "").Replace("-", "");
            var length = s.Length;
            if (length < 2 || length % 2 != 0)
            {
                //return new byte[0];
                throw new ArgumentException("hexString");
            }
            var buffer = new byte[length / 2];
            for (int i = 0, j = 0; i < length; i += 2, j++)
            {
                if (!Byte.TryParse(s.Substring(i, 2), NumberStyles.HexNumber, null, out buffer[j]))
                {
                    throw new ArgumentException("hexString");
                }
            }
            return buffer;
        }

        /// <summary>
        /// �ֽ�����ת��Ϊ��ʾ�ַ���
        /// </summary>
        /// <exception cref="ArgumentNullException">�������޷�ת��</exception>
        /// <remarks>
        /// ��ʾ������������ߡ�-����16��������ʽ
        /// </remarks>
        private static string ConvertToString(this IEnumerable<byte> buffer)
        {
            if (buffer == null) throw new ArgumentNullException("buffer");
            return BitConverter.ToString(buffer.ToArray()).Replace("-", "");
        }

        /// <summary>
        /// �ֽ�����ת��Ϊ��ʾ�ַ���
        /// </summary>
        /// <exception cref="ArgumentNullException">�������޷�ת��</exception>
        /// <remarks>
        /// ��ʾ������������ߡ�-����16������Сд��ʽ���磺
        /// 89a701fe
        /// </remarks>
        public static string ConvertToLowerString(this IEnumerable<byte> buffer)
        {
            return buffer.ConvertToString().ToLower();
        }

        /// <summary>
        /// �ֽ�����ת��Ϊ��ʾ�ַ���
        /// </summary>
        /// <exception cref="ArgumentNullException">�������޷�ת��</exception>
        /// <remarks>
        /// ��ʾ������������ߡ�-����16��������д��ʽ���磺
        /// 89A701FE
        /// </remarks>
        public static string ConvertToUpperString(this IEnumerable<byte> buffer)
        {
            return buffer.ConvertToString().ToUpper();
        }


        #endregion

        #region �ַ����ϲ��ָ�������

        /// <summary>
        /// ���˵�ĳЩ�ַ�
        /// </summary>
        /// <param name="string"></param>
        /// <param name="characters"></param>
        /// <returns></returns>
        public static string FilterIt(this string @string, char[] characters)
        {
            if (characters == null)
            {
                throw new ArgumentNullException("characters");
            }
            if (string.IsNullOrEmpty(@string))
            {
                return "";
            }

            var chars = new char[@string.Length];
            int i = 0;
            foreach (char @ch in @string)
            {
                if (!characters.Contains(ch))
                {
                    chars[i++] = ch;
                }
            }
            return new String(chars).Substring(0, i);
        }

        /// <summary>
        /// �ָ��ַ���(ָ��һ���ָ�����)
        /// </summary>
        /// <param name="text"></param>
        /// <param name="sperator"></param>
        /// <returns></returns>
        public static string[] SplitIt(this String text, char sperator)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            return text.SplitIt(new[] { sperator });
        }

        /// <summary>
        /// �ָ��ַ���
        /// </summary>
        /// <param name="text"></param>
        /// <param name="sperators"></param>
        /// <returns></returns>
        public static string[] SplitIt(this String text, params char[] sperators)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            string[] splited = text.Split(sperators, StringSplitOptions.RemoveEmptyEntries);
            IEnumerable<string> q = from ss in splited
                                    let s = ss.Trim()
                                    where s.Length > 0
                                    select s;
            return q.ToArray();
        }

        /// <summary>
        /// �ϲ�Ϊ�ַ���
        /// </summary>
        /// <param name="stringArray"></param>
        /// <param name="sperator"></param>
        /// <returns></returns>
        public static string MergeIt(this IEnumerable<string> stringArray, char sperator)
        {
            return MergeIt(stringArray, sperator.ToString());
        }

        /// <summary>
        /// �ϲ�Ϊ�ַ���
        /// </summary>
        /// <param name="stringArray"></param>
        /// <param name="sperator"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns></returns>
        public static string MergeIt(this IEnumerable<string> stringArray, string sperator)
        {
            if (null == stringArray) throw new ArgumentNullException("stringArray");

            if (stringArray.Count() == 0)
            {
                return "";
            }
            var sb = new StringBuilder();
            foreach (var str in stringArray)
            {
                sb.AppendFormat("{0}{1}", sperator, str);
            }
            return sb.Remove(0, sperator.Length).ToString();
        }

        /// <summary>
        /// ���Ϊ�����ַ���
        /// </summary>
        /// <param name="stringArray"></param>
        /// <returns></returns>
        public static string ToMulitLineString(this IEnumerable<String> stringArray)
        {
            if (null == stringArray) throw new ArgumentNullException("stringArray");

            if (stringArray.Count() == 0)
            {
                return "";
            }
            var sb = new StringBuilder();
            foreach (string t in stringArray)
            {
                sb.AppendLine(t);
            }
            return sb.ToString();
        }

        #endregion

        #region ����ʱ�������ж�

        /// <summary>
        /// �ж�����DateTime�����Ƿ���ȣ��жϵ���
        /// </summary>
        /// <param name="dt1"></param>
        /// <param name="dt2"></param>
        /// <returns></returns>
        public static bool EqualsBySecond(this DateTime dt1, DateTime dt2)
        {
            return
                dt1.Second == dt2.Second &&
                dt1.Minute == dt2.Minute &&
                dt1.Hour == dt2.Hour &&
                dt1.Day == dt2.Day &&
                dt1.Month == dt2.Month &&
                dt1.Year == dt2.Year;
        }

        #endregion

        #region ����Ƚ�

        /// <summary>
        /// �Ƚ����������Ƿ����
        /// </summary>
        /// <param name="array1">Դ����</param>
        /// <param name="array2">�Ƚ�����</param>
        /// <returns></returns>
        public static bool IsEquals(this Array array1, Array array2)
        {
            //�Ƚ������Ƿ�һ��
            if (!ReferenceEquals(array1.GetType(), array2.GetType()))
            {
                return false;
            }

            //�Ƚϳ����Ƿ�һ��
            if (array1.GetLength(0) != array2.GetLength(0))
            {
                return false;
            }

            //�Ƚϳ�Ա�Ƿ��Ӧ���
            for (int i = 0; i < array1.GetLength(0); i++)
            {
                var v1 = (ValueType)array1.GetValue(i);
                var v2 = (ValueType)array2.GetValue(i);

                if (!v1.Equals(v2))
                {
                    return false;
                }
            }
            return true;
        }

        #endregion

        #region �Ƚ�IP��ַ

        /// <summary>
        /// �Ƚ�IP��ַ
        /// </summary>
        /// <param name="thisIp"></param>
        /// <param name="otherIp"></param>
        /// <returns></returns>
        public static bool IsEquals(this IPAddress thisIp, string otherIp)
        {
            IPAddress address;
            if (IPAddress.TryParse(otherIp, out address))
            {
                return thisIp.IsEquals(address);
            }
            return false;
        }

        /// <summary>
        /// �Ƚ�IP��ַ
        /// </summary>
        /// <param name="thisIp"></param>
        /// <param name="otherIp"></param>
        /// <returns></returns>
        public static bool IsEquals(this IPAddress thisIp, IPAddress otherIp)
        {
            return thisIp.Equals(otherIp);
        }

        #endregion
    }
}
