using Microsoft.AspNetCore.SignalR;
using Model.In.PageNotice;
using Server.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Implement.PageNotice
{
    class PageNoticeServerImpl : IPageNoticeServer
    {
        /// <summary>
        /// 唯一实例
        /// </summary>
        private static PageNoticeServerImpl Instance { get; set; }

        /// <summary>
        /// SignalR操作
        /// </summary>
        private IHubContext<PageNoticeHub> hubContext;
        public PageNoticeServerImpl()
        {
            if (Instance != null)
            {
                return;
            }
            Instance = this;
        }

        public PageNoticeServerImpl(IHubContext<PageNoticeHub> hubContext)
        {
            if (Instance != null)
            {
                return;
            }
            Instance = this;
            this.hubContext = hubContext;
        }

        public void Update(string pid, string data = null)
        {
            Instance.hubContext.Clients.All.SendAsync("update", new PageNoticeIn { act = "update", pid = pid, data = data });
        }

        public void Delete(string pid, string data = null)
        {
            Instance.hubContext.Clients.All.SendAsync("delete", new PageNoticeIn { act = "delete", pid = pid, data = data });
        }

        public void Add(string pid, string data = null)
        {
            Instance.hubContext.Clients.All.SendAsync("add", new PageNoticeIn { act = "add", pid = pid, data = data });
        }
    }
}
