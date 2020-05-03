using Model.Db;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAO.PublishFlow
{
    /// <summary>
    /// 快速项目
    /// </summary>
    public static class QuickProjectDao
    {
        /// <summary>
        /// 获取项目信息
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <param name="proj_guid">项目标识</param>
        /// <returns></returns>
        public static async Task<t_quick_project> GetProject(SQLiteHelper dbHelper, string proj_guid)
        {
            string sql = @"SELECT * FROM t_quick_project WHERE proj_guid=@proj_guid";
            return await dbHelper.QueryAsync<t_quick_project>(sql, new { proj_guid = proj_guid });
        }
    }
}
