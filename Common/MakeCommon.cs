using System;
using System.Collections.Generic;
using System.Linq;
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
        /// <param name="mode">模式</param>
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
        /// salt
        /// </summary>
        /// <param name="obj">混合内容</param>
        /// <returns></returns>
        public static string MakeSalt(params string[] obj)
        {
            string[] v = new string[] { };
            foreach (var item in obj)
            {
                v.Append(MakeMD5(item, MakeGUID("N")));
            }

            return MakeMD5(MakeGUID(), string.Join("-", v));
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
