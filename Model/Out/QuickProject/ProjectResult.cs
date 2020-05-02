using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Out.QuickProject
{
    /// <summary>
    /// 项目信息
    /// </summary>
    public class ProjectResult
    {
        /// <summary>
        /// 项目标识
        /// </summary>
        public string project_uid { get; set; }

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
