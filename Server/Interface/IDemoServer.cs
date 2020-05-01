using Model.In;
using Model.Out;

namespace Server.Interface
{
    public interface IDemoServer : IServer
    {
        /// <summary>
        /// 发布
        /// </summary>
        /// <param name="serverData"></param>
        /// <returns></returns>
        Result Publish(In serverData);
    }
}
