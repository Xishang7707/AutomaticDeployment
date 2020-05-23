﻿using Model.In;
using Model.In.FlowProject;
using Model.In.PublishFlow;
using Model.Out;
using System.Threading.Tasks;

namespace App.Interface
{
    /// <summary>
    /// 流水项目
    /// </summary>
    public interface IFlowProjectApp : IApp
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

        /// <summary>
        /// 获取项目信息
        /// </summary>
        /// <param name="inData"></param>
        /// <returns></returns>
        Task<Result> GetProjectInfo(In<string> inData);

        /// <summary>
        /// 修改项目信息
        /// </summary>
        /// <param name="inData"></param>
        /// <returns></returns>
        Task<Result> EditProject(In<EditProjectIn> inData);

        /// <summary>
        /// 删除项目
        /// </summary>
        /// <param name="inData"></param>
        /// <returns></returns>
        Task<Result> DeleteProject(In<DeleteProjectIn> inData);
    }
}
