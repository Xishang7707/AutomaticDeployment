using System;
using System.Collections.Generic;
using System.Text;

namespace Model.T
{
    /// <summary>
    /// 发布
    /// </summary>
    public class TPublish
    {
        public int id { get; set; }

        /// <summary>
        /// 发布路径
        /// </summary>
        public string path { get; set; }

        /// <summary>
        /// 发布前执行命令
        /// </summary>
        public string before_command { get; set; }

        /// <summary>
        /// 发布后执行命令
        /// </summary>
        public string after_command { get; set; }
    }
}
