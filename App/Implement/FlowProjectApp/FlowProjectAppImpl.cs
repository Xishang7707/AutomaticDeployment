using App.Interface;
using Model.In;
using Model.In.FlowProject;
using Model.Out;
using Server;
using Server.Interface;
using System.Threading.Tasks;

namespace App.Implement.FlowProjectApp
{
    class FlowProjectAppImpl : IFlowProjectApp
    {
        IFlowProjectServer server = ServerFactory.Get<IFlowProjectServer>();
        public async Task<Result> AddProject(In<AddProjectIn> inData)
        {
            return await server.AddProject(inData);
        }

        public async Task<Result> GetProjectList()
        {
            return await server.GetProjectList();
        }
    }
}
