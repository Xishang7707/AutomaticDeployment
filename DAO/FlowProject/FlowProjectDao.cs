using Model.Db;
using Model.In.FlowProject;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAO.FlowProject
{
    /// <summary>
    /// 流水项目
    /// </summary>
    public static class FlowProjectDao
    {
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="db"></param>
        /// <param name="data"></param>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static async Task<bool> Insert(SQLiteHelper db, AddProjectIn data, string guid)
        {
            t_flow_project model = new t_flow_project
            {
                proj_guid = guid,
                code_cmd = data.project.code_get_cmd,
                code_source = int.Parse(data.project.code_souce_tool),
                proj_path = data.project.project_path,
                serv_id = int.Parse(data.project.service_id)
            };

            string sql = @"INSERT INTO t_flow_project (
                               proj_guid,
                               code_cmd,
                               code_source,
                               proj_path,
                               serv_id
                           )
                           VALUES (
                               @proj_guid,
                               @code_cmd,
                               @code_source,
                               @proj_path,
                               @serv_id
                           );";

            return await db.ExecAsync(sql, model) > 0;
        }
    }
}
