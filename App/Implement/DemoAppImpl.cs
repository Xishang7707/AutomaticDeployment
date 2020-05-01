using App.Interface;
using Model.In;
using Model.Out;
using Server;
using Server.Interface;

namespace App.Implement
{
    /// <summary>
    /// 模拟
    /// </summary>
    internal class DemoAppImpl : IDemoApp
    {
        public Result Publish(In appData)
        {
            IDemoServer server = ServerFactory.Get<IDemoServer>();
            return server.Publish(appData);
        }
    }
}
