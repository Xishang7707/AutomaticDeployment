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

        /// <summary>
        /// 获取发布上传路径
        /// </summary>
        /// <param name="proj_guid">项目guid</param>
        /// <returns></returns>
        public static string GetPublishUploadPath(string proj_guid)
        {
            return $"{ServerConfig.UploadDirectory}/{proj_guid}";
        }

        /// <summary>
        /// 获取发布上传路径
        /// </summary>
        /// <param name="proj_guid">项目guid</param>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        public static string GetPublishUploadPath(string proj_guid, string fileName)
        {
            return $"{ServerConfig.UploadDirectory}/{proj_guid}/{fileName}";
        }
    }
}
