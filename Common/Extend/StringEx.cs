using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Extend
{
    public static class StringEx
    {
        /// <summary>
        /// 设置字符串为空的值
        /// </summary>
        /// <param name="s"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public static string Ns(this string s, string v = "")
        {
            return string.IsNullOrWhiteSpace(s) ? v : s;
        }
    }
}
