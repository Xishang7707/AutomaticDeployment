using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Db
{
    /// <summary>
    /// 服务器
    /// </summary>
    public class t_service
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 服务器名称
        /// </summary>
        public string name { get; set; }

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
        /// 工作目录
        /// </summary>
        public int work_spacepath { get; set; }

        /// <summary>
        /// 服务器系统类型
        /// </summary>
        public int platform_type { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public string add_time { get; set; }
    }
}
