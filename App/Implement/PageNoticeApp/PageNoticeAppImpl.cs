using App.Interface;
using Microsoft.AspNetCore.SignalR;
using Server;
using Server.Implement.PageNotice;
using Server.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Implement.PageNoticeApp
{
    class PageNoticeAppImpl : IPageNoticeApp
    {
        private IPageNoticeServer server = ServerFactory.Get<IPageNoticeServer>();
        public PageNoticeAppImpl(IHubContext<PageNoticeHub> hubContext)
        {
            ServerFactory.Get<IPageNoticeServer>(hubContext);
        }
        public void Update(string pid, string data = null)
        {
            server.Update(pid, data);
        }
    }
}
