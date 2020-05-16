using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Db
{
    /// <summary>
    /// 流水项目
    /// </summary>
    public class t_flow_project
    {
        public int id { get; set; }

        /// <summary>
        /// 项目guid
        /// </summary>
        public string proj_guid { get; set; }

        /// <summary>
        /// 源码获取命令
        /// </summary>
        public string code_cmd { get; set; }

        /// <summary>
        /// 代码来源
        /// </summary>
        public int code_source { get; set; }

        /// <summary>
        /// 项目目录
        /// </summary>
        public string proj_path { get; set; }

        /// <summary>
        /// 服务器id
        /// </summary>
        public int serv_id { get; set; }
    }
}
