using App.Interface;
using Model.Db.Enum;
using Model.Out;
using Server;
using Server.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App.Implement.Environment
{
    /// <summary>
    /// 环境
    /// </summary>
    class EnvironmentAppImpl : IEnvironmentApp
    {
        private IEnvironmentServer server;

        public EnvironmentAppImpl(EOSPlatform os)
        {
            server = ServerFactory.Get<IEnvironmentServer>(os);
        }

        public async Task<Result> CheckLocalBashes()
        {
            return await server.CheckLocalBashes();
        }
    }
}
