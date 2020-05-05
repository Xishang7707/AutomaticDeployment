using Microsoft.AspNetCore.SignalR;
using Server.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Implement.PublishLog
{
    /// <summary>
    /// 发布记录Hub
    /// </summary>
    public class PublishLogHub : Hub
    {
        public PublishLogHub() { }
    }
}
