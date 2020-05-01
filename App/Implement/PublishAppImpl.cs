using App.Interface;
using Model.In;
using Model.Out;
using Server;
using Server.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Implement
{
    internal class PublishAppImpl : IPublishApp
    {
        public Result PublishDemo(DemoPublishIn demoPublishIn)
        {
            IPublishServer publishServer = ServerFactory.Get<IPublishServer>();
            return publishServer.PublishDemo(demoPublishIn);
        }
    }
}
