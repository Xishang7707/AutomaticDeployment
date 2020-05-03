using System;
using System.Collections.Generic;
using System.Text;

namespace App.Interface
{
    /// <summary>
    /// 自动发布
    /// </summary>
    public interface IAutoPublishApp : IApp
    {
        /// <summary>
        /// 通知有新任务发布
        /// </summary>
        void Notice();

        /// <summary>
        /// 启动
        /// </summary>
        void Start();
    }
}
