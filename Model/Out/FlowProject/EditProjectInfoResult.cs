using Model.In.FlowProject;
using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Out.FlowProject
{
    /// <summary>
    /// 修改信息
    /// </summary>
    public class EditProjectInfoResult
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
