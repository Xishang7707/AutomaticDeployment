using Model.Db.Enum;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Server
{
    /// <summary>
    /// 配置
    /// </summary>
    internal static class ServerConfig
    {
        static ServerConfig()
        {
            if (RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Linux))
            {
                OSPlatform = EOSPlatform.Linux;
            }
            if (RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows))
            {
                OSPlatform = EOSPlatform.Windows;
            }
            OSPlatform = EOSPlatform.Linux;
            using var sr = new StreamReader("appsettings.json", Encoding.UTF8);
            string str = sr.ReadToEnd();
            JObject json = JObject.Parse(str);

            OSUser = json["os_user"].ToString();
            OSPassword = json["os_pwd"].ToString();
            OSPort = int.Parse(json["os_port"].ToString());
        }

        public static string ProgramPath { get; } = Directory.GetCurrentDirectory();

        /// <summary>
        /// 工作目录
        /// </summary>
        public static string WorkspaceName { get; } = "workspace";

        /// <summary>
        /// 上传文件路径
        /// </summary>
        public static string UploadDirectory { get; } = $"{WorkspaceName}/upload";

        /// <summary>
        /// 本机用户名
        /// </summary>
        public static string OSUser { get; set; }

        /// <summary>
        /// 本机密码
        /// </summary>
        public static string OSPassword { get; set; }

        /// <summary>
        /// 本机连接端口
        /// </summary>
        public static int OSPort { get; set; }

        /// <summary>
        /// 运行平台
        /// </summary>
        public static EOSPlatform OSPlatform { get; set; }
    }
}
