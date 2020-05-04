using System;
using System.Collections.Generic;
using System.Text;

namespace Model.In.OSManage
{
    /// <summary>
    /// SSH key连接
    /// </summary>
    public class SShConnectIn : ConnectIn
    {
        /// <summary>
        /// 秘钥
        /// </summary>
        public string secret { get; set; }
    }
}
