using Common.Extend;
using DAO;
using DAO.PublishLog;
using Microsoft.AspNetCore.SignalR;
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
            await PublishLogDao.InsertAsync(Instance.dbHelper, new Model.Db.t_publish_log
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
            await Instance.hubContext.Clients.Client(info.proj_guid).SendAsync("log", info);
        }
    }
}
