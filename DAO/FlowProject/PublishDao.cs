using Model.Db;
using Model.In.FlowProject;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAO.FlowProject
{
    /// <summary>
    /// 发布Dao
    /// </summary>
    public static class PublishDao
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
            t_publish model = new t_publish
            {
                proj_guid = guid,
                build_after_cmd = data.publish.build_after_command,
                build_before_cmd = data.publish.build_before_command,
                publish_after_cmd = data.publish.publish_after_command,
                build_cmd = data.publish.build_command,
                publish_before_cmd = data.publish.publish_before_command,
                publish_file_path = data.publish.publish_file_path
            };
            string sql = @"INSERT INTO t_publish (
                          proj_guid,
                          build_cmd,
                          build_before_cmd,
                          build_after_cmd,
                          publish_before_cmd,
                          publish_after_cmd,
                          publish_file_path
                      )
                      VALUES (
                          @proj_guid,
                          @build_cmd,
                          @build_before_cmd,
                          @build_after_cmd,
                          @publish_before_cmd,
                          @publish_after_cmd,
                          @publish_file_path
                      );";
            return await db.ExecAsync(sql, model) > 0;
        }
    }
}
