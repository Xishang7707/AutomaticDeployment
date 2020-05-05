using App.Interface;
using Microsoft.AspNetCore.SignalR;
using Model.In.PublishLog;
using Server;
using Server.Implement.PublishLog;
using Server.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Implement.PublishLogApp
{
    internal class PublishLogAppImpl : IPublishLogApp
    {
        public PublishLogAppImpl(IHubContext<PublishLogHub> hubContext)
        {
            ServerFactory.Get<IPublishLogServer>(hubContext);
        }

        public void LogAsync(LogInfo info)
        {
            ServerFactory.Get<IPublishLogServer>().LogAsync(info);
        }
    }
}
