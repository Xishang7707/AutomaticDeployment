using Common.Extend;
using Model.Db.Enum;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAO.AutoPublish
{
    /// <summary>
    /// 
    /// </summary>
    public static class ProjectDao
    {
        /// <summary>
        /// 发布中
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <param name="proj_guid">项目guid</param>
        /// <returns></returns>
        public static async Task<bool> SetPublishProgress(SQLiteHelper dbHelper, string proj_guid)
        {
            string sql = @"UPDATE t_project SET last_publish_time=@last_publish_time,last_publish_status=@last_publish_status WHERE proj_guid=@proj_guid";
            return (await dbHelper.ExecAsync(sql, new { last_publish_time = DateTime.Now.GetSQLTime(), last_publish_status = (int)EPublishStatus.Progress, proj_guid = proj_guid })) > 0;
        }

        /// <summary>
        /// 更新发布状态
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <param name="proj_guid">项目guid</param>
        /// <param name="status">发布状态</param>
        /// <returns></returns>
        public static async Task<bool> SetPublishStatus(SQLiteHelper dbHelper, string proj_guid, EPublishStatus status)
        {
            string sql = @"UPDATE t_project SET last_publish_status=@last_publish_status WHERE proj_guid=@proj_guid";
            return (await dbHelper.ExecAsync(sql, new { last_publish_status = (int)status, proj_guid = proj_guid })) > 0;
        }
    }
}
