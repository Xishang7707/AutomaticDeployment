using Model.In;
using Model.In.FlowProject;
using Model.Out;
using System;
using System.Collections.Generic;
using System.Text;
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
    }
}
