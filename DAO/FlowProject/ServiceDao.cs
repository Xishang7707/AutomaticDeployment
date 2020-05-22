using Model.Db;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAO.FlowProject
{
    /// <summary>
    /// 服务器
    /// </summary>
    public static class ServiceDao
    {
        /// <summary>
        /// 获取服务器信息
        /// </summary>
        /// <param name="db"></param>
        /// <param name="serv_arr"></param>
        /// <returns></returns>
        public static async Task<List<t_service>> GetServiceList(SQLiteHelper db, int[] serv_arr)
        {
            string sql = @"SELECT * FROM t_service WHERE id in @serv_arr";

            return await db.QueryListAsync<t_service>(sql, new { serv_arr = serv_arr });
        }

        /// <summary>
        /// 获取服务器信息
        /// </summary>
        /// <param name="db"></param>
        /// <param name="serv_id"></param>
        /// <returns></returns>
        public static async Task<t_service> GetService(SQLiteHelper db, int serv_id)
        {
            string sql = @"SELECT * FROM t_service WHERE id = @serv_id";

            return await db.QueryAsync<t_service>(sql, new { serv_id = serv_id });
        }
    }
}
