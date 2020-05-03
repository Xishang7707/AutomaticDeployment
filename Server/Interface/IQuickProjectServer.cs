using Model.In;
using Model.In.PublishFlow;
using Model.In.QuickProject;
using Model.Out;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Server.Interface
{
    /// <summary>
    /// 快速项目
    /// </summary>
    public interface IQuickProjectServer : IServer
    {
        /// <summary>
        /// 添加快速项目
        /// </summary>
        /// <param name="inData"></param>
        /// <returns></returns>
        Task<Result> AddQuickProjectAsync(In<AddQuickProjectIn> inData);

        /// <summary>
        /// 项目列表
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Task<Result> QuickProjectListAsync(In data);

        /// <summary>
        /// 发布
        /// </summary>
        /// <param name="inData"></param>
        /// <returns></returns>
        Task<Result> Publish(In<PublishQuickProject> inData);
    }
}
