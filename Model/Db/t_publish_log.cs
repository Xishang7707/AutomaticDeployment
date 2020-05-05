using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Db
{
    /// <summary>
    /// 发布记录
    /// </summary>
    public class t_publish_log
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 项目标识
        /// </summary>
        public string proj_guid { get; set; }

        /// <summary>
        /// 发布id
        /// </summary>
        public int publish_id { get; set; }

        /// <summary>
        /// 发布信息
        /// </summary>
        public string publish_info { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public string add_time { get; set; }
    }
}
