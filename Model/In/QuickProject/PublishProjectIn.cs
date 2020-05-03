using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Model.In.QuickProject
{
    /// <summary>
    /// 发布项目信息
    /// </summary>
    public class PublishProjectIn
    {
        /// <summary>
        /// 项目标识
        /// </summary>
        public string project_uid { get; set; }

        /// <summary>
        /// 文件
        /// </summary>
        public IFormFileCollection files { get; set; }
    }
}
