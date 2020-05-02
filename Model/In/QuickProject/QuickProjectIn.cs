using System;
using System.Collections.Generic;
using System.Text;

namespace Model.In.QuickProject
{
    /// <summary>
    /// 快速项目 -项目信息
    /// </summary>
    public class QuickProjectIn : ProjectIn
    {
        /// <summary>
        /// 项目名称
        /// </summary>
        public string project_name { get; set; }

        /// <summary>
        /// 项目描述
        /// </summary>
        public string project_remark { get; set; }
    }
}
