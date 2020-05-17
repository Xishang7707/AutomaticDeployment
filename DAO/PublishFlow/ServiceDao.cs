using Model.Db;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAO.PublishFlow
{
    public static class ServiceDao
    {
        /// <summary>
        /// 获取服务器信息
        /// </summary>
        /// <param name="db"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static async Task<t_service> GetService(SQLiteHelper db, int id)
        {
            string sql = @"SELECT * FROM t_service WHERE id=@id";
            return await db.QueryAsync<t_service>(sql, new { id = id });
        }
    }
}
