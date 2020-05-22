using System;
using System.Collections.Generic;
using System.Text;

namespace App.Interface
{
    /// <summary>
    /// 页面通知
    /// </summary>
    public interface IPageNoticeApp : IApp
    {
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="pid">页面id</param>
        /// <param name="data">数据</param>
        void Update(string pid, string data = null);
    }
}
