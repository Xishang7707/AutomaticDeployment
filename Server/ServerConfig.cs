using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Server
{
    /// <summary>
    /// 配置
    /// </summary>
    internal class ServerConfig
    {
        public static string ProgramPath { get; } = Directory.GetCurrentDirectory();

        /// <summary>
        /// 工作目录
        /// </summary>
        public static string WorkspaceName { get; } = "workspace";

        /// <summary>
        /// 上传文件路径
        /// </summary>
        public static string UploadDirectory { get; } = $"{WorkspaceName}/upload";
    }
}
