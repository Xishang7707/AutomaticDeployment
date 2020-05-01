using System;
using System.Collections.Generic;
using System.Text;

namespace Model.T
{
    public class TServer
    {
        public int id { get; set; }

        /// <summary>
        /// 服务器ip
        /// </summary>
        public string ip { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        public int port { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string account { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string password { get; set; }
    }
}
