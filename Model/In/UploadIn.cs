using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Model.In
{
    /// <summary>
    /// 文件上传
    /// </summary>
    public class UploadIn
    {
        /// <summary>
        /// 上传的文件
        /// </summary>
        public IFormFileCollection Files { get; set; }
    }
}
