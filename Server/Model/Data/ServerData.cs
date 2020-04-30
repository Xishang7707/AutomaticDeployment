using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Model.Data
{
    public class ServerData
    {
    }

    public class ServerData<T> : ServerData
    {
        public T Data { get; set; }
    }
}
