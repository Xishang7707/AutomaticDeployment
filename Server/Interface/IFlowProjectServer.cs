﻿using Model.In;
using Model.In.FlowProject;
using Model.In.PublishFlow;
using Model.Out;
using System.Threading.Tasks;

namespace Server.Interface
{
    /// <summary>
    /// 流水
    /// </summary>
    public interface IFlowProjectServer : IServer
    {
        /// <summary>
        /// 添加项目
        /// </summary>
        /// <param name="inData"></param>
        /// <returns></returns>
        Task<Result> AddProject(In<AddProjectIn> inData);

        /// <summary>
        /// 获取项目列表
        /// </summary>
        /// <returns></returns>
        Task<Result> GetProjectList();

        /// <summary>
        /// 发布
        /// </summary>
        /// <param name="inData"></param>
        /// <returns></returns>
        Task<Result> Publish(In<PublishFlowProject> inData);

        /// <summary>
        /// 获取项目信息
        /// </summary>
        /// <param name="inData"></param>
        /// <returns></returns>
        Task<Result> GetProject(In<string> inData);
    }
}
