using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Model.Db.Enum
{
    /// <summary>
    /// 项目类型
    /// </summary>
    public enum EProjectType
    {
        /// <summary>
        /// 快速项目
        /// </summary>
        [Description("快速项目")]
        Quick = 0,

        /// <summary>
        /// 流水项目
        /// </summary>
        [Description("流水项目")]
        Flow = 1
    }
}
