using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Common
{
    /// <summary>
    /// 生成
    /// </summary>
    public static class MakeCommon
    {
        /// <summary>
        /// 生成MD5
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="make">混合</param>
        /// <returns></returns>
        public static string MakeMD5(string str, string make = "")
        {
            byte[] data = Encoding.Default.GetBytes(make + str + make);
            byte[] result = MD5.Create("SHA256").ComputeHash(data);
            return Encoding.Default.GetString(result);
        }

        /// <summary>
        /// 生成GUID
        /// </summary>
        /// <param name="fmt">格式</param>
        /// <returns></returns>
        public static string MakeGUID(string fmt = "N")
        {
            return Guid.NewGuid().ToString(fmt);
        }

        /// <summary>
        /// 生成文件名
        /// </summary>
        /// <param name="make">混合</param>
        /// <returns></returns>
        public static string MakeFileName(string make = "")
        {
            string guid = MakeGUID("D");
            return guid;
        }
    }
}
