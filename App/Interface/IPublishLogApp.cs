using Model.In.PublishLog;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Interface
{
    /// <summary>
    /// 发布记录
    /// </summary>
    public interface IPublishLogApp : IApp
    {
        /// <summary>
        /// 记录
        /// </summary>
        /// <param name="info">信息</param>
        void LogAsync(LogInfo info);
    }
}
