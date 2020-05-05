using Model.In.PublishLog;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Interface
{
    /// <summary>
    /// 发布记录服务
    /// </summary>
    public interface IPublishLogServer : IServer
    {
        /// <summary>
        /// 记录
        /// </summary>
        /// <param name="info">信息</param>
        void LogAsync(LogInfo info);
    }
}
