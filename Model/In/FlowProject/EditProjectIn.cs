using System;
using System.Collections.Generic;
using System.Text;

namespace Model.In.FlowProject
{
    /// <summary>
    /// 修改项目信息
    /// </summary>
    public class EditProjectIn
    {
        /// <summary>
        /// 项目guid
        /// </summary>
        public string project_uid { get; set; }
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
