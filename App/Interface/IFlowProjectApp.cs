using Model.In;
using Model.In.FlowProject;
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
    }
}
