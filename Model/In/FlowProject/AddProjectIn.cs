using System;
using System.Collections.Generic;
using System.Text;

namespace Model.In.FlowProject
{
    /// <summary>
    /// 添加项目
    /// </summary>
    public class AddProjectIn
    {
        /// <summary>
        /// 项目信息
        /// </summary>
        public FlowProjectIn project { get; set; }

        /// <summary>
        /// 发布信息
        /// </summary>
        public FlowPublishIn publish { get; set; }
    }
}
