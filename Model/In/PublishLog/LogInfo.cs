using System;
using System.Collections.Generic;
using System.Text;

namespace Model.In.PublishLog
{
    /// <summary>
    /// 发布信息
    /// </summary>
    public class LogInfo
    {
        /// <summary>
        /// 项目guid
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
    }
}
