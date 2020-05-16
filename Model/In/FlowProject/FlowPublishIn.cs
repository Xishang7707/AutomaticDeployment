using System;
using System.Collections.Generic;
using System.Text;

namespace Model.In.FlowProject
{
    /// <summary>
    /// 流水项目发布信息
    /// </summary>
    public class FlowPublishIn
    {
        /// <summary>
        /// 构建前命令
        /// </summary>
        public string build_before_command { get; set; }

        /// <summary>
        /// 构建命令
        /// </summary>
        public string build_command { get; set; }

        /// <summary>
        /// 构建后命令
        /// </summary>
        public string build_after_command { get; set; }

        /// <summary>
        /// 编译后需发布的文件路径
        /// </summary>
        public string publish_file_path { get; set; }

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
