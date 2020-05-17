using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Out.FlowProject
{
    /// <summary>
    /// 项目信息
    /// </summary>
    public class ProjectInfoResult
    {
        /// <summary>
        /// 项目
        /// </summary>
        public ProjectResult project { get; set; }

        /// <summary>
        /// 服务器
        /// </summary>
        public ServiceResult server { get; set; }

        /// <summary>
        /// 发布信息
        /// </summary>
        public PublishResult publish { get; set; }
    }
}
