using App.Implement;
using App.Implement.QuickProjectApp;
using App.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace App
{
    public static class AppFactory
    {
        public static T Get<T>() where T : class, IApp
        {
            if (typeof(T) == typeof(IDemoApp))
                return new DemoAppImpl() as T;

            if (typeof(T) == typeof(IUploadApp))
                return new UploadAppImpl() as T;

            if (typeof(T) == typeof(IPublishApp))
                return new PublishAppImpl() as T;

            if (typeof(T) == typeof(IQuickProjectApp))
                return new QuickProjectAppImpl() as T;

            return null;
        }
    }
}
