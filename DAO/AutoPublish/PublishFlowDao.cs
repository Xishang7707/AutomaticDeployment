using Common.Extend;
using Model.Db;
using Model.Db.Enum;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAO.AutoPublish
{
    public class PublishFlowDao
    {
        /// <summary>
        /// 获取第一条项目信息
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <returns></returns>
        public static async Task<t_publish_flow> GetProject(SQLiteHelper dbHelper)
        {
            string sql = @"SELECT * FROM t_publish_flow WHERE status=@status ORDER BY id asc limit 1;";
            return await dbHelper.QueryAsync<t_publish_flow>(sql, new { status = (int)EStatus.Enable });
        }

        /// <summary>
        /// 发布中
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <param name="id">发布id</param>
        /// <returns></returns>
        public static async Task<bool> SetPublishProgress(SQLiteHelper dbHelper, int id)
        {
            string sql = @"UPDATE t_publish_flow SET publish_time=@publish_time,publish_status=@publish_status WHERE id=@id AND status=@status";
            return (await dbHelper.ExecAsync(sql, new { publish_time = DateTime.Now.GetSQLTime(), publish_status = (int)EPublishStatus.Progress, status = (int)EStatus.Enable, id = id })) > 0;
        }

        /// <summary>
        /// 更新发布状态
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <param name="id">发布id</param>
        /// <param name="status">发布状态</param>
        /// <returns></returns>
        public static async Task<bool> SetPublishStatus(SQLiteHelper dbHelper, int id, EPublishStatus status)
        {
            string sql = @"UPDATE t_publish_flow SET publish_status=@publish_status WHERE id=@id";
            return (await dbHelper.ExecAsync(sql, new { publish_status = (int)status, id = id })) > 0;
        }

        /// <summary>
        /// 发布成功状态更新
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <param name="id">发布id</param>
        /// <returns></returns>
        public static async Task<bool> SetPublishSuccess(SQLiteHelper dbHelper, int id)
        {
            string sql = @"UPDATE t_publish_flow SET publish_status=@publish_status,status=@status WHERE id=@id";
            return (await dbHelper.ExecAsync(sql, new { publish_status = (int)EPublishStatus.Success, status = (int)EStatus.Disabled, id = id })) > 0;
        }

        /// <summary>
        /// 发布失败状态更新
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <param name="id">发布id</param>
        /// <returns></returns>
        public static async Task<bool> SetPublishFailed(SQLiteHelper dbHelper, int id)
        {
            string sql = @"UPDATE t_publish_flow SET publish_status=@publish_status,status=@status WHERE id=@id";
            return (await dbHelper.ExecAsync(sql, new { publish_status = (int)EPublishStatus.Failed, status = (int)EStatus.Disabled, id = id })) > 0;
        }
    }
}
