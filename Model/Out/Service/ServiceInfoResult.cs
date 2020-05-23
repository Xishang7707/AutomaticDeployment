using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Out.Service
{
    /// <summary>
    /// 服务器信息
    /// </summary>
    public class ServiceInfoResult
    {
        /// <summary>
        /// 服务器id
        /// </summary>
        public int server_id { get; set; }

        /// <summary>
        /// 服务器ip
        /// </summary>
        public string server_ip { get; set; }

        /// <summary>
        /// 服务器端口
        /// </summary>
        public int server_port { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string server_account { get; set; }

        /// <summary>
        /// 服务器密码
        /// </summary>
        public string server_password { get; set; }

        /// <summary>
        /// 服务器名称
        /// </summary>
        public string server_name { get; set; }

        /// <summary>
        /// 平台
        /// </summary>
        public int server_platform { get; set; }

        /// <summary>
        /// 工作目录
        /// </summary>
        public string workspace { get; set; }
    }
}
