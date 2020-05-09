using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Model.Db.Enum
{
    /// <summary>
    /// 数据库类型
    /// </summary>
    public enum EDatabaseType
    {
        /// <summary>
        /// mssql
        /// </summary>
        [Description("MSSQL")]
        MSSQL = 1,

        /// <summary>
        /// mysql
        /// </summary>
        [Description("MYSQL")]
        MYSQL = 2
    }
}
