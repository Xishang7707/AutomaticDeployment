using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Model.Db.Enum
{
    /// <summary>
    /// 连接模式
    /// </summary>
    public enum EOSConnectMode
    {
        /// <summary>
        /// 用户名和密码
        /// </summary>
        [Description("用户名和密码")]
        UserAndPassword = 0,

        /// <summary>
        /// SSH
        /// </summary>
        [Description("SSH")]
        SSH = 1
    }
}
