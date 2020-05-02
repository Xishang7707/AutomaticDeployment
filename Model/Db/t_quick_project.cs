using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Db
{
    /// <summary>
    /// 快速发布项目
    /// </summary>
    public class t_quick_project
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
        /// 连接ip
        /// </summary>
        public string conn_ip { get; set; }

        /// <summary>
        /// 连接端口
        /// </summary>
        public int conn_port { get; set; }

        /// <summary>
        /// 连接模式
        /// </summary>
        public int conn_mode { get; set; }

        /// <summary>
        /// 连接用户名
        /// </summary>
        public string conn_user { get; set; }

        /// <summary>
        /// 连接密码
        /// </summary>
        public string conn_password { get; set; }

        /// <summary>
        /// 公钥连接
        /// </summary>
        public string ssh_key { get; set; }

        public string secret_salt { get; set; }

        /// <summary>
        /// 发布目录
        /// </summary>
        public int publish_path { get; set; }

        /// <summary>
        /// 服务器系统类型
        /// </summary>
        public int platform_type { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public string update_time { get; set; }
    }
}
