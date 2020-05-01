using Model.In;
using Model.Out;
using Server.Interface;

namespace Server.Implement
{
    internal class DemoServerImpl : IDemoServer
    {
        public Result Publish(In serverData)
        {
            return new Result
            {
            };
        }
    }
}
