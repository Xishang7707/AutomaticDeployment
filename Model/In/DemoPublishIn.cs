using Model.In.Demo;
using System;
using System.Collections.Generic;
using System.Text;

namespace Model.In
{
    /// <summary>
    /// 演示发布
    /// </summary>
    public class DemoPublishIn
    {
        /// <summary>
        /// 服务器
        /// </summary>
        public DemoServerIn server { get; set; }

        /// <summary>
        /// 项目
        /// </summary>
        public DemoProjectIn project { get; set; }

        /// <summary>
        /// 发布
        /// </summary>
        public PublishIn publish { get; set; }
    }
}
