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
        public async Task<Result> AddProjectAsync(In<AddQuickProjectIn> inData)
        {
            IQuickProjectServer server = ServerFactory.Get<IQuickProjectServer>();
            return await server.AddQuickProjectAsync(inData);
        }

        public async Task<Result> GetProjectListAsync(In data)
        {
            IQuickProjectServer server = ServerFactory.Get<IQuickProjectServer>();
            return await server.QuickProjectListAsync(data);
        }

        public async Task<Result> Publish(In<PublishQuickProject> inData)
        {
            IQuickProjectServer server = ServerFactory.Get<IQuickProjectServer>();
            return await server.Publish(inData);
        }
    }
}
