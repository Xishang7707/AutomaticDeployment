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

        /// <summary>
        /// 项目是否存在
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <param name="proj_guid">项目guid</param>
        /// <param name="proj_type">项目类型</param>
        /// <returns></returns>
        public static async Task<bool> IsExist(SQLiteHelper dbHelper, string proj_guid, EProjectType proj_type)
        {
            string sql = @"SELECT id FROM t_project WHERE proj_guid=@proj_guid AND proj_type=@proj_type";
            return (await dbHelper.QueryAsync<int>(sql, new { proj_guid = proj_guid, proj_type = proj_type })) > 0;
        }
    }
}
