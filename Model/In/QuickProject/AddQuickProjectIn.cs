using System;
using System.Collections.Generic;
using System.Text;

namespace Model.In.QuickProject
{
    /// <summary>
    /// 添加快速项目
    /// </summary>
    public class AddQuickProjectIn
    {
        /// <summary>
        /// 服务器信息
        /// </summary>
        public QuickServerIn server { get; set; }

        /// <summary>
        /// 项目信息
        /// </summary>
        public QuickProjectIn project { get; set; }

        /// <summary>
        /// 发布信息
        /// </summary>
        public QuickPublishIn publish { get; set; }
    }
}
