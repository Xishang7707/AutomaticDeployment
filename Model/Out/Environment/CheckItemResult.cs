using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Out.Environment
{
    /// <summary>
    /// 检查结果
    /// </summary>
    public class CheckItemResult
    {
        /// <summary>
        /// 命令
        /// </summary>
        public string bash { get; set; }

        /// <summary>
        /// 检查通过
        /// </summary>
        public bool pass { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string version { get; set; }
    }
}
