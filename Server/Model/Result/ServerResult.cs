using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Model.Result
{
    public class ServerResult
    {

    }

    public class ServerResult<T> : ServerResult
    {
        public T Data { get; set; }
    }
}
