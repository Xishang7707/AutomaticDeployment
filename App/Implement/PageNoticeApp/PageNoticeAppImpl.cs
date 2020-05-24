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
        private IPageNoticeServer server;
        public PageNoticeAppImpl(IHubContext<PageNoticeHub> hubContext)
        {
            server = ServerFactory.Get<IPageNoticeServer>(hubContext);
        }

        public void Add(string pid, string data = null)
        {
            server.Add(pid, data);
        }

        public void Delete(string pid, string data = null)
        {
            server.Delete(pid, data);
        }

        public void Update(string pid, string data = null)
        {
            server.Update(pid, data);
        }
    }
}
