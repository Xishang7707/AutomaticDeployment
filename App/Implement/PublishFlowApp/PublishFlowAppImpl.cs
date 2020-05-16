using App.Interface;
using Model.In;
using Model.In.PublishFlow;
using Model.Out;
using Server;
using Server.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App.Implement.PublishFlowApp
{
    internal class PublishFlowAppImpl : IPublishFlowApp
    {
        public async Task<Result> PublishAsync(In<PublishQuickProject> inData)
        {
            IPublishFlowServer server = ServerFactory.Get<IPublishFlowServer>();
            return await server.PublishAsync(inData);
        }
    }
}
