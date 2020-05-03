using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Model.In.Upload
{
    /// <summary>
    /// 项目发布文件上传
    /// </summary>
    public class PublishUploadIn
    {
        /// <summary>
        /// 项目guid
        /// </summary>
        public string project_uid { get; set; }

        /// <summary>
        /// 项目文件
        /// </summary>
        public IFormFileCollection files { get; set; }
    }
}
