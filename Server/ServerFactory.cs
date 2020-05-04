using Model.Db.Enum;
using Server.Implement;
using Server.Implement.AutoPublish;
using Server.Implement.OSManage;
using Server.Implement.PublishFlow;
using Server.Implement.QuickProject;
using Server.Interface;

namespace Server
{
    public static class ServerFactory
    {
        public static T Get<T>(params object[] v) where T : class, IServer
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

            if (typeof(T) == typeof(IOSManageServer))
            {
                if (v.Length == 0 || !(v[0] is EOSPlatform))
                {
                    return null;
                }
                return GetOSPlatform((EOSPlatform)v[0]) as T;
            }

            return null;
        }

        /// <summary>
        /// 获取IOSMamanerServer的实现
        /// </summary>
        /// <param name="os">系统类型</param>
        /// <returns></returns>
        public static IOSManageServer GetOSPlatform(EOSPlatform os)
        {
            switch (os)
            {
                case EOSPlatform.Linux:
                    return new LinuxManageImpl();
                case EOSPlatform.Windows:
                    return new WindowsManageImpl();
                default:
                    return null;
            }
        }
    }
}
