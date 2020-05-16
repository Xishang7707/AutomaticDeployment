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
        public  static async Task<int> InsertAsync(SQLiteHelper dbHelper, AddProjectIn data, string guid)
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
    }
}
