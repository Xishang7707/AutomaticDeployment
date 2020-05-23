using System;
using System.Collections.Generic;
using System.Text;

namespace Model.In.Service
{
    /// <summary>
    /// 修改服务器信息
    /// </summary>
    public class EditServiceIn
    {
        /// <summary>
        /// 服务器id
        /// </summary>
        public string server_id { get; set; }

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

        /// <summary>
        /// 服务器名称
        /// </summary>
        public string server_name { get; set; }

        /// <summary>
        /// 平台
        /// </summary>
        public string server_platform { get; set; }

        /// <summary>
        /// 工作目录
        /// </summary>
        public string server_space { get; set; }
    }
}
