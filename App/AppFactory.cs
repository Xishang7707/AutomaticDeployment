using App.Implement;
using App.Implement.AutoPublishApp;
using App.Implement.Environment;
using App.Implement.FlowProjectApp;
using App.Implement.PageNoticeApp;
using App.Implement.PublishLogApp;
using App.Implement.QuickProjectApp;
using App.Implement.ServiceApp;
using App.Interface;
using Microsoft.AspNetCore.SignalR;
using Model.Db.Enum;
using Server.Implement.PageNotice;
using Server.Implement.PublishLog;
using Server.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace App
{
    public static class AppFactory
    {
        public static T Get<T>(params object[] o) where T : class, IApp
        {
            if (typeof(T) == typeof(IDemoApp))
                return new DemoAppImpl() as T;

            if (typeof(T) == typeof(IUploadApp))
                return new UploadAppImpl() as T;

            if (typeof(T) == typeof(IPublishApp))
                return new PublishAppImpl() as T;

            if (typeof(T) == typeof(IQuickProjectApp))
                return new QuickProjectAppImpl() as T;

            if (typeof(T) == typeof(IAutoPublishApp))
                return new AutoPublishAppImpl() as T;

            if (typeof(T) == typeof(IServiceApp))
                return new ServiceAppImpl() as T;

            if (typeof(T) == typeof(IFlowProjectApp))
                return new FlowProjectAppImpl() as T;

            if (typeof(T) == typeof(IPublishLogApp))
                return new PublishLogAppImpl(o[0] as IHubContext<PublishLogHub>) as T;

            if (typeof(T) == typeof(IPageNoticeApp))
                return new PageNoticeAppImpl(o[0] as IHubContext<PageNoticeHub>) as T;

            if (typeof(T) == typeof(IEnvironmentApp))
                return new EnvironmentAppImpl((EOSPlatform)o[0]) as T;

            return null;
        }
    }
}
