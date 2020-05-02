using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Model.Db.Enum
{
    /// <summary>
    /// 代码来源
    /// </summary>
    public enum ECodeMode
    {
        /// <summary>
        /// 文件
        /// </summary>
        [Description("文件")]
        File = 0,

        /// <summary>
        /// git地址
        /// </summary>
        [Description("GIT")]
        GIT = 1,
    }
}
