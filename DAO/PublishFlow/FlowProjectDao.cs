using Model.Db;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAO.PublishFlow
{
    public static class FlowProjectDao
    {
        /// <summary>
        /// 获取项目信息
        /// </summary>
        /// <param name="db"></param>
        /// <param name="proj_guid"></param>
        /// <returns></returns>
        public static async Task<t_flow_project> GetProject(SQLiteHelper db, string proj_guid)
        {
            string sql = @"SELECT * FROM t_flow_project WHERE proj_guid=@proj_guid";
            return await db.QueryAsync<t_flow_project>(sql, new { proj_guid = proj_guid });
        }
    }
}
