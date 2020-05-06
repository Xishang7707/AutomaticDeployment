using Model.Db.Enum;
using Model.In.PublishLog;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Interface
{
    /// <summary>
    /// 发布记录服务
    /// </summary>
    public interface IPublishLogServer : IServer
    {
        /// <summary>
        /// 记录
        /// </summary>
        /// <param name="info">信息</param>
        void LogAsync(LogInfo info);

        /// <summary>
        /// 记录
        /// </summary>
        /// <param name="proj_guid">项目guid</param>
        /// <param name="publish_id">发布id</param>
        /// <param name="info">信息</param>
        void LogAsync(string proj_guid, int publish_id, string info);

        /// <summary>
        /// 发布结果
        /// </summary>
        /// <param name="proj_guid">项目id</param>
        /// <param name="flow_id">发布id</param>
        /// <param name="status">结果</param>
        void SendToPublishResultAsync(string proj_guid, int flow_id, EPublishStatus status);
    }
}
