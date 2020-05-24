using App.Interface;
using Model.In;
using Model.In.PublishFlow;
using Model.In.QuickProject;
using Model.Out;
using Server;
using Server.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App.Implement.QuickProjectApp
{
    /// <summary>
    /// 快速发布
    /// </summary>
    internal class QuickProjectAppImpl : IQuickProjectApp
    {
        private IQuickProjectServer server = ServerFactory.Get<IQuickProjectServer>();
        public async Task<Result> AddProjectAsync(In<AddQuickProjectIn> inData)
        {
            return await server.AddQuickProjectAsync(inData);
        }

        public async Task<Result> DeleteProject(In<DeleteProjectIn> inData)
        {
            return await server.DeleteProject(inData);
        }

        public async Task<Result> EditProject(In<EditQuickProjectIn> inData)
        {
            return await server.EditProject(inData);
        }

        public async Task<Result> GetProjectAsync(In<string> inData)
        {
            return await server.GetProjectAsync(inData);
        }

        public async Task<Result> GetProjectClassify(In inData)
        {
            return await server.GetProjectClassify(inData);
        }

        public async Task<Result> GetProjectListAsync(In<SearchProjectIn> data)
        {
            return await server.QuickProjectListAsync(data);
        }

        public async Task<Result> Publish(In<PublishQuickProject> inData)
        {
            return await server.Publish(inData);
        }

    }
}
