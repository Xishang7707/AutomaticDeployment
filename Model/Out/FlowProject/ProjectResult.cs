using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Out.FlowProject
{
    /// <summary>
    /// 项目
    /// </summary>
    public class ProjectResult
    {
        /// <summary>
        /// 项目guid
        /// </summary>
        public string project_uid { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string project_name { get; set; }
        
        /// <summary>
        /// 项目归类
        /// </summary>
        public string project_classify { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string project_remark { get; set; }

        /// <summary>
        /// 代码来源工具
        /// </summary>
        public string code_souce_tool { get; set; }

        /// <summary>
        /// 项目发布路径
        /// </summary>
        public string project_path { get; set; }
    }
}
