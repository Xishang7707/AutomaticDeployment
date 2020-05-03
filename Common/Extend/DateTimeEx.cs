using System;

namespace Common.Extend
{
    /// <summary>
    /// 时间扩展
    /// </summary>
    public static class DateTimeEx
    {
        /// <summary>
        /// 获取存数据库时间 yyyy-MM-dd HH:mm:ss:ms
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string GetSQLTime(this DateTime t)
        {
            return t.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 获取展示时间 yyyy-MM-dd HH:mm:ss
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string GetTime(this DateTime t)
        {
            return t.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}
