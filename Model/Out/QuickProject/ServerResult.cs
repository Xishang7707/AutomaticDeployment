using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Out.QuickProject
{
    /// <summary>
    /// 服务器信息
    /// </summary>
    public class ServerResult
    {
        /// <summary>
        /// 服务器ip
        /// </summary>
        public string server_ip { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string server_account { get; set; }

        /// <summary>
        /// 服务器连接模式
        /// </summary>
        public string server_connect_mode { get; set; }
    }
}
