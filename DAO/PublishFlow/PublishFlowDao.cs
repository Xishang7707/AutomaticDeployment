using Common.Extend;
using Model.Db;
using Model.Db.Enum;
using Model.In.PublishFlow;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAO.PublishFlow
{
    /// <summary>
    /// 发布流
    /// </summary>
    public static class PublishFlowDao
    {
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <param name="proj_info">项目信息</param>
        /// <param name="files">文件</param>
        /// <returns></returns>
        public static async Task<bool> Insert(SQLiteHelper dbHelper, t_quick_project proj_info, List<FileModePublish> files)
        {
            t_publish_flow model = new t_publish_flow
            {
                add_time = DateTime.Now.GetSQLTime(),
                extern_info = JsonConvert.SerializeObject(files),
                proj_guid = proj_info.proj_guid,
                proj_info = JsonConvert.SerializeObject(new t_project
                {
                    proj_guid = proj_info.proj_guid,
                }),
                publish_info = JsonConvert.SerializeObject(new t_publish
                {
                    proj_guid = proj_info.proj_guid,
                    publish_before_cmd = proj_info.publish_before_cmd,
                    publish_after_cmd = proj_info.publish_after_cmd,
                }),
                server_info = JsonConvert.SerializeObject(new t_service
                {
                    conn_ip = proj_info.conn_ip,
                    conn_mode = proj_info.conn_mode,
                    platform_type = proj_info.platform_type,
                    conn_password = proj_info.conn_password,
                    conn_port = proj_info.conn_port,
                    conn_user = proj_info.conn_user,
                    secret_salt = proj_info.secret_salt,
                    ssh_key = proj_info.ssh_key,
                    work_spacepath = proj_info.publish_path
                }),
                proj_type = (int)EProjectType.Quick,
                status = (int)EStatus.Enable,
                publish_status = (int)EPublishStatus.Waitting
            };

            string sql = @"INSERT INTO t_publish_flow (
                               proj_guid,
                               proj_type,
                               publish_status,
                               add_time,
                               server_info,
                               publish_info,
                               proj_info,
                               status,
                               extern_info
                           )
                           VALUES (
                               @proj_guid,
                               @proj_type,
                               @publish_status,
                               @add_time,
                               @server_info,
                               @publish_info,
                               @proj_info,
                               @status,
                               @extern_info
                           );";

            return (await dbHelper.ExecAsync(sql, model)) > 0;
        }
    }
}
