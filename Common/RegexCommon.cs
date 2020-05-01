using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Common
{
    /// <summary>
    /// 正则
    /// </summary>
    public static class RegexCommon
    {
        /// <summary>
        /// 验证ip
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IP(string str)
        {
            string parttem = @"^((?:(?:25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d)))\.){3}(?:25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d))))$";
            return Regex.IsMatch(str, parttem);
        }
    }
}
