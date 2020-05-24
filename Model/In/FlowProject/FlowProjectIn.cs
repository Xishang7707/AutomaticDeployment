using System;
using System.Collections.Generic;
using System.Text;

namespace Model.In.FlowProject
{
    /// <summary>
    /// 流水项目信息
    /// </summary>
    public class FlowProjectIn : ProjectIn
    {
        /// <summary>
        /// 项目名称
        /// </summary>
        public string project_name { get; set; }

        /// <summary>
        /// 归类
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
        /// 代码获取命令
        /// </summary>
        public string code_get_cmd { get; set; }

        /// <summary>
        /// 项目发布路径
        /// </summary>
        public string project_path { get; set; }

        /// <summary>
        /// 服务器id
        /// </summary>
        public string service_id { get; set; }
    }
}
