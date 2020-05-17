using Model.In;
using Model.In.PublishFlow;
using Model.Out;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Server.Interface
{
    /// <summary>
    /// 发布流
    /// </summary>
    public interface IPublishFlowServer : IServer
    {
        /// <summary>
        /// 发布快速项目
        /// </summary>
        /// <param name="inData"></param>
        /// <returns></returns>
        Task<Result> PublishAsync(In<PublishQuickProject> inData);

        /// <summary>
        /// 发布流式项目
        /// </summary>
        /// <param name="inData"></param>
        /// <returns></returns>
        Task<Result> PublishAsync(In<PublishFlowProject> inData);
    }
}
