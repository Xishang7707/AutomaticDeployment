using Model.In;
using Model.In.PublishFlow;
using Model.Out;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App.Interface
{
    /// <summary>
    /// 发布流
    /// </summary>
    public interface IPublishFlowApp : IApp
    {
        /// <summary>
        /// 发布快速项目
        /// </summary>
        /// <param name="inData"></param>
        /// <returns></returns>
        Task<Result> PublishAsync(In<PublishQuickProject> inData);
    }
}
