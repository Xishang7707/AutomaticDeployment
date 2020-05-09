using System;
using System.Collections.Generic;
using System.Text;

namespace Model.In.SqlManage
{
    /// <summary>
    /// 连接信息
    /// </summary>
    public abstract class ConnectIn
    {
        /// <summary>
        /// 主机
        /// </summary>
        public string host { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        public int port { get; set; }
    }
}
