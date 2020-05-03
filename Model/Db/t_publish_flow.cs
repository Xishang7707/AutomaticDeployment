using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Db
{
    /// <summary>
    /// 发布流
    /// </summary>
    public class t_publish_flow
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
        /// 项目类型
        /// </summary>
        public int proj_type { get; set; }

        /// <summary>
        /// 发布状态
        /// </summary>
        public int publish_status { get; set; }

        /// <summary>
        /// 发布时间
        /// </summary>
        public string publish_time { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public string add_time { get; set; }

        /// <summary>
        /// 服务器信息
        /// </summary>
        public string server_info { get; set; }

        /// <summary>
        /// 项目信息
        /// </summary>
        public string proj_info { get; set; }

        /// <summary>
        /// 发布信息
        /// </summary>
        public string publish_info { get; set; }

        /// <summary>
        /// 状态 可进行发布状态
        /// </summary>
        public int status { get; set; }

        /// <summary>
        /// 扩展信息
        /// </summary>
        public string extern_info { get; set; }
    }
}
