using Model.Db.Enum;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Common
{
    /// <summary>
    /// 验证
    /// </summary>
    public static class VerifyCommon
    {
        /// <summary>
        /// 验证ip
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IP(string ip)
        {
            string parttem = @"^((?:(?:25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d)))\.){3}(?:25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d))))$";
            return Regex.IsMatch(ip, parttem);
        }

        /// <summary>
        /// 验证端口
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public static bool Port(string port) => !(!int.TryParse(port, out int verifyServerPort) || verifyServerPort <= 0 || verifyServerPort > 65535);

        /// <summary>
        /// 验证服务器账号长度
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public static bool ServiceAccountLength(string account) => account.Length < 20;

        /// <summary>
        /// 验证服务器密码长度
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool ServicePasswordLength(string password) => password.Length < 20;

        /// <summary>
        /// 验证项目名称长度
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool ProjectNameLength(string name) => name.Length < 20;

        /// <summary>
        /// 验证发布路径长度
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool PublishProjectPathLength(string path) => path.Length < 1024;

        /// <summary>
        /// 验证平台类型
        /// </summary>
        /// <param name="plat"></param>
        /// <returns></returns>
        public static bool OSPlatform(string plat) => !string.IsNullOrWhiteSpace(plat) && int.TryParse(plat, out int tmpVal) && Enum.IsDefined(typeof(EOSPlatform), tmpVal);

        /// <summary>
        /// 验证类型类型
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public static bool FileType(string fileName, EFileType type)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return false;
            }
            int index = fileName.LastIndexOf('.');
            if (index == -1 || index + 1 == fileName.Length)
            {
                return false;
            }

            string ex = fileName.Substring(index);
            return type switch
            {
                EFileType.ZIP => ex.ToLower() == ".zip",
                EFileType.RAR => ex.ToLower() == ".rar",
                EFileType._7Z => ex.ToLower() == ".7z",
                EFileType.SQL => ex.ToLower() == ".sql",
                _ => false,
            };
        }
    }
}
