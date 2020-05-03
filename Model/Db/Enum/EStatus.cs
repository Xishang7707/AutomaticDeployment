using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Model.Db.Enum
{
    /// <summary>
    /// 状态
    /// </summary>
    public enum EStatus
    {
        /// <summary>
        /// 停用
        /// </summary>
        [Description("停用")]
        Disabled = 0,

        /// <summary>
        /// 启用
        /// </summary>
        [Description("正常")]
        Enable = 1
    }
}
