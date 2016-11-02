using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace RegexMatch
{
    /// <summary>
    /// 正则运算
    /// </summary>
    public static class RegexUtility
    {
        /// <summary>
        /// 判断一个字符串是否匹配某正则表达式
        /// </summary>
        /// <param name="input">输入一个字符串</param>
        /// <param name="pattern">正则表达式</param>
        /// <returns>是否匹配</returns>
        public static bool RegexMatch(string input, string pattern)
        {
            Regex r = new Regex(pattern);
            Match m = r.Match(input);
            return m.Success;
        }

        /// <summary>
        /// 从一个字符串内得到匹配正则表达式的结果集合
        /// </summary>
        /// <param name="input"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static IList<String> RegexSearch(string input, string pattern)
        {
            IList<string> results = new List<string>();
            Regex r = new Regex(pattern);
            MatchCollection mc = r.Matches(input);
            foreach (Match m in mc)         
                results.Add(string.Format("[{0}:{1}] {2}", m.Index, m.Length, m.Value));
            return results;
        }
    }
}
