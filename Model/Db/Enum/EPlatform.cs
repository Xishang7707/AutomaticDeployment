using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Model.Db.Enum
{
    /// <summary>
    /// 平台
    /// </summary>
    public enum EOSPlatform
    {
        /// <summary>
        /// Linux
        /// </summary>
        [Description("Linux")]
        Linux = 0,

        /// <summary>
        /// Windows
        /// </summary>
        [Description("Windows")]
        Windows = 1,
    }
}
