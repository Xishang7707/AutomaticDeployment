using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server.Interface
{
    /// <summary>
    /// 发布服务 自动流
    /// </summary>
    public interface IAutoPublishServer : IServer
    {
        /// <summary>
        /// 通知有新任务发布
        /// </summary>
        void Notice();

        /// <summary>
        /// 启动
        /// </summary>
        Task Start();
    }
}
