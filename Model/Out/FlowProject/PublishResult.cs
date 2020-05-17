using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Out.FlowProject
{
    /// <summary>
    /// 发布信息
    /// </summary>
    public class PublishResult
    {
        /// <summary>
        /// 发布前命令
        /// </summary>
        public string publish_before_command { get; set; }

        /// <summary>
        /// 发布后命令
        /// </summary>
        public string publish_after_command { get; set; }

        /// <summary>
        /// 最新发布时间
        /// </summary>
        public string publish_time { get; set; }

        /// <summary>
        /// 最新发布状态
        /// </summary>
        public string publish_status { get; set; }
    }
}
