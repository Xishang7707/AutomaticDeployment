using Common;
using Common.Extend;
using Model.Db;
using Model.Db.Enum;
using Model.In.FlowProject;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAO.FlowProject
{
    /// <summary>
    /// 项目Dao
    /// </summary>
    public static class ProjectDao
    {
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <param name="data"></param>
        /// <param name="guid">guid</param>
        /// <returns></returns>
        public static async Task<int> InsertAsync(SQLiteHelper dbHelper, AddProjectIn data, string guid)
        {
            t_project model = new t_project
            {
                add_time = DateTime.Now.GetSQLTime(),
                name = data.project.project_name,
                proj_type = (int)EProjectType.Flow,
                proj_guid = guid,
                remark = data.project.project_remark?.Trim()
            };
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

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static async Task<bool> UpdateAsync(SQLiteHelper dbHelper, EditProjectIn data)
        {
            t_project model = new t_project
            {
                name = data.project.project_name,
                proj_guid = data.project_uid,
                remark = data.project.project_remark?.Trim()
            };
            string sql = $@"UPDATE t_project SET
                          name=@name,
                          remark=@remark
                          WHERE proj_guid=@proj_guid";

            return await dbHelper.ExecAsync(sql, model) > 0;
        }

        /// <summary>
        /// 获取项目列表
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <returns></returns>
        public static async Task<List<t_project>> GetProjectList(SQLiteHelper dbHelper)
        {
            string sql = @"SELECT name,proj_guid,last_publish_time,last_publish_status,add_time,remark FROM t_project WHERE proj_type=@proj_type;";
            return await dbHelper.QueryListAsync<t_project>(sql, new { proj_type = (int)EProjectType.Flow });
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
            return (await dbHelper.QueryAsync<int>(sql, new { proj_guid = proj_guid, proj_type = (int)EProjectType.Flow })) > 0;
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
            return await dbHelper.QueryAsync<t_project>(sql, new { proj_type = (int)EProjectType.Flow, proj_guid = proj_guid });
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
            return (await dbHelper.ExecAsync(sql, new { proj_guid = proj_guid, proj_type = (int)EProjectType.Flow })) > 0;
        }

    }
}
