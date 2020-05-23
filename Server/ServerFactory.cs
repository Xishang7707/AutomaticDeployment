using Microsoft.AspNetCore.SignalR;
using Model.Db.Enum;
using Server.Implement;
using Server.Implement.AutoPublish;
using Server.Implement.FlowProject;
using Server.Implement.OSManage;
using Server.Implement.PageNotice;
using Server.Implement.PublishFlow;
using Server.Implement.PublishLog;
using Server.Implement.QuickProject;
using Server.Implement.Service;
using Server.Implement.SqlManage;
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

            if (typeof(T) == typeof(IServiceServer))
                return new ServiceServerImpl() as T;

            if (typeof(T) == typeof(IFlowProjectServer))
                return new FlowProjectServerImpl() as T;

            if (typeof(T) == typeof(IOSManageServer))
            {
                if (v.Length == 0 || !(v[0] is EOSPlatform))
                {
                    return null;
                }
                return GetOSPlatform((EOSPlatform)v[0]) as T;
            }

            if (typeof(T) == typeof(IPublishLogServer))
            {
                if (v.Length != 0 && !(v[0] is IHubContext<PublishLogHub>))
                {
                    return null;
                }
                return (v.Length > 0 ? GetPublisLog((IHubContext<PublishLogHub>)v[0]) : GetPublisLog()) as T;
            }

            if (typeof(T) == typeof(IPageNoticeServer))
            {
                if (v.Length != 0 && !(v[0] is IHubContext<PageNoticeHub>))
                {
                    return null;
                }
                return (v.Length > 0 ? GetPageNotice((IHubContext<PageNoticeHub>)v[0]) : GetPageNotice()) as T;
            }

            if (typeof(T) == typeof(ISqlManageServer))
            {
                if (v.Length == 0 || !(v[0] is EDatabaseType))
                {
                    return null;
                }
                return GetSqlManage((EDatabaseType)v[0]) as T;
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
            return os switch
            {
                EOSPlatform.Linux => new LinuxManageImpl(),
                EOSPlatform.Windows => new WindowsManageImpl(),
                _ => null,
            };
        }

        public static IPublishLogServer GetPublisLog(IHubContext<PublishLogHub> hubContext = null)
        {
            if (hubContext == null)
            {
                return new PublishLogImpl();
            }
            return new PublishLogImpl(hubContext);
        }

        public static IPageNoticeServer GetPageNotice(IHubContext<PageNoticeHub> hubContext = null)
        {
            if (hubContext == null)
            {
                return new PageNoticeServerImpl();
            }
            return new PageNoticeServerImpl(hubContext);
        }

        public static ISqlManageServer GetSqlManage(EDatabaseType type)
        {
            return type switch
            {
                EDatabaseType.MSSQL => new MssqlManageServerImpl(),
                EDatabaseType.MYSQL => null,
                _ => null
            };
        }
    }
}
