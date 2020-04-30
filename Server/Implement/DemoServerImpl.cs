using Server.Interface;
using Server.Model.Data;
using Server.Model.Result;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Implement
{
    internal class DemoServerImpl : IDemoServer
    {
        public ServerResult<string> Publish(ServerData serverData)
        {
            return new ServerResult<string>
            {
                Data = "发布"
            };
        }
    }
}
