using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    /// <summary>
    /// Server专用公共类
    /// </summary>
    internal static class ServerCommon
    {
        /// <summary>
        /// 获取上传路径
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        public static string GetUploadPath(string fileName)
        {
            return $"{ServerConfig.UploadDirectory}/{fileName}";
        }
    }
}
