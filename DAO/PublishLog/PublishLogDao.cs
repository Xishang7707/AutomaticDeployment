using Model.Db;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAO.PublishLog
{
    /// <summary>
    /// 发布记录DAO
    /// </summary>
    public static class PublishLogDao
    {
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static async Task<bool> InsertAsync(SQLiteHelper dbHelper, t_publish_log model)
        {
            string sql = $@"INSERT INTO t_publish_log (
                              proj_guid,
                              publish_id,
                              publish_info,
                              add_time
                          )
                          VALUES (
                              @proj_guid,
                              @publish_id,
                              @publish_info,
                              @add_time
                          );";
            return await dbHelper.ExecAsync(sql, model) > 0;
        }
    }
}
