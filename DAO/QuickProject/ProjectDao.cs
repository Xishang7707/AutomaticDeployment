using Common;
using Common.Extend;
using Model.Dao.QuickProject;
using Model.Db;
using Model.Db.Enum;
using Model.In.QuickProject;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAO.QuickProject
{
    /// <summary>
    /// 项目
    /// </summary>
    public static class ProjectDao
    {
        #region SQL
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        private static async Task<int> InsertAsync(SQLiteHelper dbHelper, t_project model)
        {
            string sql = $@"INSERT INTO t_project (
                          name,
                          proj_guid,
                          proj_type,
                          add_time,
                          remark
                      )
                      VALUES (
                          @name,
                          @proj_guid,
                          @proj_type,
                          @add_time,
                          @remark
                      );select last_insert_rowid();";
            return await dbHelper.ExecAsync<int>(sql, model);
        }

        #endregion

        /// <summary>
        /// 添加项目数据
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <param name="data"></param>
        public static async Task<AddProjectDaoResult> AddProjectModelAsync(SQLiteHelper dbHelper, QuickProjectIn data)
        {
            t_project model = new t_project
            {
                add_time = DateTime.Now.GetSQLTime(),
                name = data.project_name,
                remark = data.project_remark,
                proj_guid = MakeCommon.MakeGUID("N"),
                proj_type = (int)EProjectType.Quick
            };

            AddProjectDaoResult result = new AddProjectDaoResult();
            result.id = await InsertAsync(dbHelper, model);
            if (result.id <= 0)
                return result;

            result.proj_guid = model.proj_guid;
            result.result = true;
            return result;
        }

        /// <summary>
        /// 获取项目列表
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <returns></returns>
        public static async Task<List<t_project>> GetProjectList(SQLiteHelper dbHelper)
        {
            string sql = @"SELECT name,proj_guid,last_publish_time,last_publish_status,add_time,remark FROM t_project WHERE proj_type=@proj_type;";
            return await dbHelper.QueryListAsync<t_project>(sql, new { proj_type = (int)EProjectType.Quick });
        }

        /// <summary>
        /// 项目是否存在
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <param name="proj_guid">项目guid</param>
        /// <returns></returns>
        public static async Task<bool> IsExist(SQLiteHelper dbHelper, string proj_guid)
        {
            string sql = @"SELECT id FROM t_project WHERE proj_guid=@proj_guid AND proj_type=@proj_type";
            return (await dbHelper.QueryAsync<int>(sql, new { proj_guid = proj_guid, proj_type = (int)EProjectType.Quick })) > 0;
        }

        /// <summary>
        /// 获取项目
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <param name="proj_guid">项目guid</param>
        /// <returns></returns>
        public static async Task<t_project> GetProject(SQLiteHelper dbHelper, string proj_guid)
        {
            string sql = @"SELECT name,proj_guid,last_publish_time,last_publish_status,add_time,remark FROM t_project WHERE proj_guid=@proj_guid AND proj_type=@proj_type;";
            return await dbHelper.QueryAsync<t_project>(sql, new { proj_type = (int)EProjectType.Quick, proj_guid = proj_guid });
        }

        /// <summary>
        /// 更新项目数据
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <param name="proj_guid">项目guid</param>
        /// <returns></returns>
        public static async Task<bool> EditProject(SQLiteHelper dbHelper, QuickProjectIn data)
        {
            string sql = @"UPDATE t_project set name=@name,remark=@remark WHERE proj_guid=@proj_guid AND proj_type=@proj_type;";
            return await dbHelper.ExecAsync(sql, new { proj_type = (int)EProjectType.Quick, proj_guid = data.project_guid, name = data.project_name, remark = data.project_remark }) > 0;
        }

        /// <summary>
        /// 删除项目
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <param name="proj_guid">项目guid</param>
        /// <returns></returns>
        public static async Task<bool> DeleteProject(SQLiteHelper dbHelper, string proj_guid)
        {
            string sql = @"DELETE FROM t_project WHERE proj_guid=@proj_guid AND proj_type=@proj_type";
            return (await dbHelper.ExecAsync(sql, new { proj_guid = proj_guid, proj_type = (int)EProjectType.Quick })) > 0;
        }
    }
}
