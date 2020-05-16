using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Db
{
    /// <summary>
    /// 发布
    /// </summary>
    public class t_publish
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 关联项目guid
        /// </summary>
        public string proj_guid { get; set; }

        /// <summary>
        /// 构建命令
        /// </summary>
        public string build_cmd { get; set; }

        /// <summary>
        /// 构建前命令
        /// </summary>
        public string build_before_cmd { get; set; }

        /// <summary>
        /// 构建后命令
        /// </summary>
        public string build_after_cmd { get; set; }

        /// <summary>
        /// 发布前命令
        /// </summary>
        public string publish_before_cmd { get; set; }

        /// <summary>
        /// 发布后命令
        /// </summary>
        public string publish_after_cmd { get; set; }

    }
}
