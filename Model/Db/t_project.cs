using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Db
{
    /// <summary>
    /// 项目【总表】
    /// </summary>
    public class t_project
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 标识
        /// </summary>
        public string proj_guid { get; set; }

        /// <summary>
        /// 项目类型
        /// </summary>
        public int proj_type { get; set; }

        /// <summary>
        /// 最后一次发布时间
        /// </summary>
        public string last_publish_time { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public string add_time { get; set; }

        /// <summary>
        /// 代码来源
        /// </summary>
        public string code_source { get; set; }

        /// <summary>
        /// 代码工具
        /// </summary>
        public int code_mode { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string remark { get; set; }
    }
}
