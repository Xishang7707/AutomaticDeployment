using System;
using System.Collections.Generic;
using System.Text;

namespace Model.In.SqlManage
{
    /// <summary>
    /// 用户名密码登录
    /// </summary>
    public class UserConnectIn : ConnectIn
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string user { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string password { get; set; }
    }
}
