using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Dao.QuickProject
{
    /// <summary>
    /// 更新快速项目的服务器密码
    /// </summary>
    public class UpdateServerPasswordIn
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string password { get; set; }

        /// <summary>
        /// 混合
        /// </summary>
        public string salt { get; set; }
    }
}
