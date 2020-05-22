using System;
using System.Collections.Generic;
using System.Text;

namespace Model.In.PageNotice
{
    /// <summary>
    /// 页面通知数据
    /// </summary>
    public class PageNoticeIn
    {
        /// <summary>
        /// 操作
        /// </summary>
        public string act { get; set; }

        /// <summary>
        /// 页面id
        /// </summary>
        public string pid { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public string data { get; set; }
    }
}
