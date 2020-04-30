using App.Implement;
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
            return null;
        }
    }
}
