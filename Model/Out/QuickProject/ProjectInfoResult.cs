using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Out.QuickProject
{
    /// <summary>
    /// 项目列表
    /// </summary>
    public class ProjectInfoResult
    {
        /// <summary>
        /// 项目信息
        /// </summary>
        public ProjectResult project { get; set; }

        /// <summary>
        /// 服务器信息
        /// </summary>
        public ServerResult server { get; set; }

        /// <summary>
        /// 发布信息
        /// </summary>
        public PublishResult publish { get; set; }
    }
}
