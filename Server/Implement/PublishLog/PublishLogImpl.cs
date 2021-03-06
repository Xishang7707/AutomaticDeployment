﻿using Common.Extend;
using DAO;
using DAO.PublishLog;
using Microsoft.AspNetCore.SignalR;
using Model.Db.Enum;
using Model.In.PublishLog;
using Newtonsoft.Json;
using Server.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Server.Implement.PublishLog
{
    /// <summary>
    /// 发布记录
    /// </summary>
    internal class PublishLogImpl : IPublishLogServer
    {
        /// <summary>
        /// 唯一实例
        /// </summary>
        private static PublishLogImpl Instance { get; set; }

        /// <summary>
        /// 数据库操作
        /// </summary>
        private SQLiteHelper dbHelper;

        /// <summary>
        /// SignalR操作
        /// </summary>
        private IHubContext<PublishLogHub> hubContext;

        public PublishLogImpl()
        {
            if (Instance != null)
            {
                return;
            }
            Instance = this;
            dbHelper = new SQLiteHelper();
        }

        public PublishLogImpl(IHubContext<PublishLogHub> hubContext)
        {
            if (Instance != null)
            {
                return;
            }
            Instance = this;
            dbHelper = new SQLiteHelper();
            this.hubContext = hubContext;
        }

        public async void LogAsync(LogInfo info)
        {
            bool flag = await PublishLogDao.InsertAsync(Instance.dbHelper, new Model.Db.t_publish_log
            {
                proj_guid = info.proj_guid,
                publish_id = info.publish_id,
                add_time = DateTime.Now.GetTime(),
                publish_info = info.publish_info
            });
            SendInfoAsync(info);
        }

        /// <summary>
        /// 向客户端发送记录
        /// </summary>
        /// <param name="info"></param>
        private async void SendInfoAsync(LogInfo info)
        {
            await Instance.hubContext.Clients.Group(GetPublishGroup(info.proj_guid)).SendAsync("log", info);
        }

        public void LogAsync(string proj_guid, int publish_id, string info)
        {
            LogAsync(new LogInfo { proj_guid = proj_guid, publish_id = publish_id, publish_info = info });
        }

        public async void SendToPublishResultAsync(string proj_guid, int flow_id, EPublishStatus status)
        {
            await Instance.hubContext.Clients.Group(GetPublishGroup(proj_guid)).SendAsync("result", new LogInfo { proj_guid = proj_guid, publish_id = flow_id, publish_info = status.GetDesc() });
        }

        public string GetPublishGroup(string proj_guid)
        {
            return $"publish-{proj_guid}";
        }
    }
}
