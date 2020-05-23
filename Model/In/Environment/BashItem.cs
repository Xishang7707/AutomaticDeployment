using Model.Out;
using System;
using System.Collections.Generic;
using System.Text;

namespace Model.In.Environment
{
    /// <summary>
    /// 命令配置
    /// </summary>
    public class BashItem
    {
        /// <summary>
        /// 命令
        /// </summary>
        public string bash { get; set; }

        /// <summary>
        /// 安装脚本
        /// </summary>
        public string install { get; set; }

        /// <summary>
        /// 解析版本
        /// </summary>
        public Func<object, Result<List<string>>> ParseVersion;

        /// <summary>
        /// 标题
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }
    }
}
