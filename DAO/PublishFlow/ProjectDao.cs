using Model.Db;
using Model.Db.Enum;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAO.PublishFlow
{
    /// <summary>
    /// 项目
    /// </summary>
    public static class ProjectDao
    {
        /// <summary>
        /// 获取项目类型
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <param name="proj_guid">项目标识</param>
        /// <returns></returns>
        public static async Task<t_project> GetProjectTypeAsync(SQLiteHelper dbHelper, string proj_guid)
        {
            string sql = @"SELECT proj_type FROM t_project WHERE proj_guid=@proj_guid";
            return (await dbHelper.QueryAsync<t_project>(sql, new { proj_guid = proj_guid }));
        }
    }
}
