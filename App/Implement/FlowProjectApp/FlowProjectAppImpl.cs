using App.Interface;
using Model.In;
using Model.In.FlowProject;
using Model.In.PublishFlow;
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

        public async Task<Result> DeleteProject(In<DeleteProjectIn> inData)
        {
            return await server.DeleteProject(inData);
        }

        public async Task<Result> EditProject(In<EditProjectIn> inData)
        {
            return await server.EditProject(inData);
        }

        public async Task<Result> GetProject(In<string> inData)
        {
            return await server.GetProject(inData);
        }

        public async Task<Result> GetProjectInfo(In<string> inData)
        {
            return await server.GetProjectInfo(inData);
        }

        public async Task<Result> GetProjectList()
        {
            return await server.GetProjectList();
        }

        public async Task<Result> Publish(In<PublishFlowProject> inData)
        {
            return await server.Publish(inData);
        }
    }
}
