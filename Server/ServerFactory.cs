using Server.Implement;
using Server.Implement.AutoPublish;
using Server.Implement.PublishFlow;
using Server.Implement.QuickProject;
using Server.Interface;

namespace Server
{
    public static class ServerFactory
    {
        public static T Get<T>() where T : class, IServer
        {
            if (typeof(T) == typeof(IDemoServer))
                return new DemoServerImpl() as T;

            if (typeof(T) == typeof(IUploadServer))
                return new UploadServerImpl() as T;

            if (typeof(T) == typeof(IPublishServer))
                return new PublishServerImpl() as T;

            if (typeof(T) == typeof(IQuickProjectServer))
                return new QuickProjectServerImpl() as T;

            if (typeof(T) == typeof(IPublishFlowServer))
                return new PublishFlowServerImpl() as T;

            if (typeof(T) == typeof(IAutoPublishServer))
                return new AutoPublishServerImpl() as T;

            return null;
        }
    }
}
