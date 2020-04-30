using Server.Model.Data;
using Server.Model.Result;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Interface
{
    public interface IDemoServer : IServer
    {
        /// <summary>
        /// 发布
        /// </summary>
        /// <param name="serverData"></param>
        /// <returns></returns>
        ServerResult<string> Publish(ServerData serverData);
    }
}
