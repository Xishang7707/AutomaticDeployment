using System;
using System.Collections.Generic;
using System.Text;

namespace Model.In.Service
{
    /// <summary>
    /// 服务器信息
    /// </summary>
    public class AddServiceIn : ServerIn
    {
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
