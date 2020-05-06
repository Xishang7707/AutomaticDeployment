using Microsoft.AspNetCore.SignalR;
using Model.Db.Enum;
using Model.In.PublishLog;
using Server.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Implement.PublishLog
{
    /// <summary>
    /// 发布记录Hub
    /// </summary>
    public class PublishLogHub : Hub
    {
        private static Dictionary<string, List<PublishLogHub>> clients = new Dictionary<string, List<PublishLogHub>>();

        public PublishLogHub() { }

        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public void Publish(string proj_guid)
        {
            this.Groups.AddToGroupAsync(this.Context.ConnectionId, GetPublishGroup(proj_guid));
        }

        public string GetPublishGroup(string proj_guid)
        {
            return $"publish-{proj_guid}";
        }
    }
}
