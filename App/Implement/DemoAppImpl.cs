using App.Interface;
using App.Model.Data;
using App.Model.Result;
using Server;
using Server.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Implement
{
    /// <summary>
    /// 模拟
    /// </summary>
    internal class DemoAppImpl : IDemoApp
    {
        public AppResult<string> Publish(AppData appData)
        {
            IDemoServer server = ServerFactory.Get<IDemoServer>();
            var result = server.Publish(null);
            return new AppResult<string>
            {
                Data = result.Data
            };
        }
    }
}
