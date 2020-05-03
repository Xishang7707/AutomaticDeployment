using System;
using System.Collections.Generic;
using System.Text;

namespace Model.In.PublishFlow
{
    /// <summary>
    /// 发布快速项目
    /// </summary>
    public class PublishQuickProject
    {
        /// <summary>
        /// 项目标识
        /// </summary>
        public string project_uid { get; set; }

        /// <summary>
        /// 文件
        /// </summary>
        public List<FileModePublish> files { get; set; }
    }
}
