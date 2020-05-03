using Model.Db.Enum;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Common
{
    /// <summary>
    /// 获取
    /// </summary>
    public static class GetCommon
    {
        /// <summary>
        /// 获取文件后缀名（包含.）
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        public static string GetFileSuffix(string fileName)
        {
            int index = fileName.LastIndexOf('.');
            return index == -1 || index + 1 > fileName.Length ? "" : fileName.Substring(index);
        }

        /// <summary>
        /// 获取文件名
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        public static string GetFileName(string fileName)
        {
            int index = fileName.LastIndexOf('.');
            return index == -1 || index == 0 ? "" : fileName.Substring(0, index);
        }

        /// <summary>
        /// 拼接路径和文件名
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        public static string GetFileSplicing(string path, string fileName)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return fileName;
            }

            if (path == "/" || path == "\\")
            {
                return path + fileName;
            }

            char lastChar = path.Last();
            if (lastChar == '/' || lastChar == '\\')
            {
                return path + fileName;
            }

            return path + '/' + fileName;
        }

        /// <summary>
        /// 切割路径的目录
        /// 清除空白目录名
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns></returns>
        public static string[] GetDirectoryNames(string path)
        {
            List<string> list = path.Split('/', '\\').Where(w => !string.IsNullOrWhiteSpace(w)).ToList();
            if (path[0] == '/' || path[0] == '\\')
            {
                list.Insert(0, "/");
            }
            return list.ToArray();
        }

        /// <summary>
        /// 获取命令/分行
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string[] GetCommands(string str)
        {
            return str.Split('\n').Where(w => !string.IsNullOrWhiteSpace(w)).ToArray();
        }

        /// <summary>
        /// 时间转换
        /// </summary>
        /// <param name="t"></param>
        /// <param name="s"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public static bool GetCastTime(string s, out DateTime v)
        {
            v = new DateTime();
            if (string.IsNullOrWhiteSpace(s))
            {
                return false;
            }
            DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();
            dtFormat.FullDateTimePattern = "yyyy-MM-dd HH:mm:ss";
            try
            {
                v = Convert.ToDateTime(s, dtFormat);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
    }
}
