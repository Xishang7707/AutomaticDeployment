using System;
using System.Collections.Generic;
using System.Text;

namespace Model.In
{
    /// <summary>
    /// 发布
    /// </summary>
    public class PublishIn
    {
        /// <summary>
        /// 发布路径
        /// </summary>
        public string publish_path { get; set; }

        /// <summary>
        /// 发布前命令
        /// </summary>
        public string publish_before_command { get; set; }

        /// <summary>
        /// 发布后命令
        /// </summary>
        public string publish_after_command { get; set; }
    }
}
