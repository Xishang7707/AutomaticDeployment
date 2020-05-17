using Model.Db;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAO.PublishFlow
{
    public static class PublishDao
    {
        /// <summary>
        /// 获取发布信息
        /// </summary>
        /// <param name="db"></param>
        /// <param name="proj_guid"></param>
        /// <returns></returns>
        public static async Task<t_publish> GetPublish(SQLiteHelper db, string proj_guid)
        {
            string sql = @"SELECT * FROM t_publish WHERE proj_guid=@proj_guid";
            return await db.QueryAsync<t_publish>(sql, new { proj_guid = proj_guid });
        }
    }
}
