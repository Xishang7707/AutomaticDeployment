using App.Interface;
using Server;
using Server.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Implement.AutoPublishApp
{
    internal class AutoPublishAppImpl : IAutoPublishApp
    {
        public void Notice()
        {
            IAutoPublishServer server = ServerFactory.Get<IAutoPublishServer>();
            server.Notice();
        }

        public void Start()
        {
            IAutoPublishServer server = ServerFactory.Get<IAutoPublishServer>();
            server.Start();
        }
    }
}
