using Server.Implement;
using Server.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    public static class ServerFactory
    {
        public static T Get<T>() where T : class, IServer
        {
            if (typeof(T) == typeof(IDemoServer))
                return new DemoServerImpl() as T;

            return null;
        }
    }
}
