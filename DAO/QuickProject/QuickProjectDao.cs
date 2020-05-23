using Common;
using Model.Dao.QuickProject;
using Model.Db;
using Model.Db.Enum;
using Model.In.QuickProject;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAO.QuickProject
{
    /// <summary>
    /// 快速项目DAO
    /// </summary>
    public static class QuickProjectDao
    {
        #region SQL
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        private static async Task<int> InsertAsync(SQLiteHelper dbHelper, t_quick_project model)
        {
            string sql = @"INSERT INTO t_quick_project (
                                proj_guid,
                                conn_ip,
                                conn_port,
                                conn_mode,
                                conn_user,
                                conn_password,
                                publish_path,
                                platform_type,
                                publish_before_cmd,
                                publish_after_cmd
                            )
                            VALUES (
                                @proj_guid,
                                @conn_ip,
                                @conn_port,
                                @conn_mode,
                                @conn_user,
                                @conn_password,
                                @publish_path,
                                @platform_type,
                                @publish_before_cmd,
                                @publish_after_cmd
                            );select last_insert_rowid();";
            return await dbHelper.ExecAsync<int>(sql, model);
        }

        /// <summary>
        /// 更新快速项目的服务器密码
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <param name="update"></param>
        /// <returns></returns>
        private static async Task<bool> UpdataPassword(SQLiteHelper dbHelper, UpdateServerPasswordIn update)
        {
            string sql = $@"UPDATE t_quick_project
                            SET
                                conn_password = @password
                            WHERE id = @id;";
            return await dbHelper.ExecAsync(sql, update) > 0;
        }
        #endregion

        /// <summary>
        /// 添加快速项目数据
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <param name="data"></param>
        public static async Task<bool> AddQuickProjectModelAsync(SQLiteHelper dbHelper, AddQuickProjectIn data, string proj_guid)
        {
            t_quick_project model = new t_quick_project
            {
                conn_ip = data.server.server_ip,
                conn_mode = (int)EOSConnectMode.UserAndPassword,
                conn_password = data.server.server_password,
                conn_port = int.Parse(data.server.server_port),
                conn_user = data.server.server_account,
                platform_type = int.Parse(data.server.server_platform),
                proj_guid = proj_guid,
                publish_path = data.publish.publish_path,
                publish_before_cmd = data.publish.publish_before_command,
                publish_after_cmd = data.publish.publish_after_command
            };
            int id = await InsertAsync(dbHelper, model);
            if (id <= 0)
            {
                return false;
            }
            string encPassword = ConcealCommon.EncryptDES(data.server.server_password);
            return await UpdataPassword(dbHelper, new UpdateServerPasswordIn
            {
                id = id,
                password = encPassword
            });
        }

        /// <summary>
        /// 更新项目数据
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <param name="data"></param>
        public static async Task<bool> EditQuickProjectModelAsync(SQLiteHelper dbHelper, EditQuickProjectIn data)
        {
            t_quick_project model = new t_quick_project
            {
                conn_ip = data.server.server_ip,
                conn_mode = (int)EOSConnectMode.UserAndPassword,
                conn_password = data.server.server_password == GetCommon.GetHidePassword() ? "" : ConcealCommon.EncryptDES(data.server.server_password),
                conn_port = int.Parse(data.server.server_port),
                conn_user = data.server.server_account,
                platform_type = int.Parse(data.server.server_platform),
                proj_guid = data.project.project_guid,
                publish_path = data.publish.publish_path,
                publish_before_cmd = data.publish.publish_before_command,
                publish_after_cmd = data.publish.publish_after_command
            };

            string sql = $@"UPDATE t_quick_project SET
                                conn_ip=@conn_ip,
                                conn_port=@conn_port,
                                conn_mode=@conn_mode,
                                conn_user=@conn_user,
                                {(data.server.server_password == GetCommon.GetHidePassword() ? "" : "conn_password = @conn_password,")}
                                publish_path = @publish_path,
                                platform_type = @platform_type,
                                publish_before_cmd = @publish_before_cmd,
                                publish_after_cmd = @publish_after_cmd
                                WHERE proj_guid = @proj_guid; ";

            return await dbHelper.ExecAsync(sql, model) > 0;
        }

        /// <summary>
        /// 批量获取项目配置信息
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <param name="proj_guids"></param>
        /// <returns></returns>
        public static async Task<List<t_quick_project>> GetProjectList(SQLiteHelper dbHelper, string[] proj_guids)
        {
            string sql = @"SELECT proj_guid,conn_ip,conn_user,conn_mode,publish_path,platform_type,publish_before_cmd,publish_after_cmd FROM t_quick_project WHERE proj_guid in @proj_guids;";
            return await dbHelper.QueryListAsync<t_quick_project>(sql, new { proj_guids = proj_guids });
        }

        /// <summary>
        /// 获取项目配置信息
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <param name="proj_guid">项目guid</param>
        /// <returns></returns>
        public static async Task<t_quick_project> GetProject(SQLiteHelper dbHelper, string proj_guid)
        {
            string sql = @"SELECT proj_guid,conn_ip,conn_port,conn_user,conn_password,conn_mode,publish_path,platform_type,publish_before_cmd,publish_after_cmd FROM t_quick_project WHERE proj_guid=@proj_guid;";
            return await dbHelper.QueryAsync<t_quick_project>(sql, new { proj_guid = proj_guid });
        }

        /// <summary>
        /// 删除项目
        /// </summary>
        /// <param name="db"></param>
        /// <param name="proj_guid"></param>
        /// <returns></returns>
        public static async Task<bool> DeleteProject(SQLiteHelper db, string proj_guid)
        {
            string sql = @"DELETE FROM t_quick_project WHERE proj_guid=@proj_guid";

            return await db.ExecAsync(sql, new { proj_guid = proj_guid }) > 0;
        }
    }
}
