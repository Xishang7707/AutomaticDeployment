using System;
using System.Collections.Generic;
using System.Text;

namespace Model.In
{
    /// <summary>
    /// 服务器
    /// </summary>
    public class ServerIn
    {
        /// <summary>
        /// 服务器ip
        /// </summary>
        public string server_ip { get; set; }

        /// <summary>
        /// 服务器端口
        /// </summary>
        public string server_port { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string server_account { get; set; }

        /// <summary>
        /// 服务器密码
        /// </summary>
        public string server_password { get; set; }
    }
}
