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
    }
}
