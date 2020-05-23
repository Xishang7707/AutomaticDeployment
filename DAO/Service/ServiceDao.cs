using Common;
using Common.Extend;
using Model.Db;
using Model.Db.Enum;
using Model.In.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAO.Service
{
    /// <summary>
    /// 服务器
    /// </summary>
    public static class ServiceDao
    {
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="sqlHelper"></param>
        /// <param name="addData"></param>
        /// <returns></returns>
        public static async Task<int> InsertAsync(SQLiteHelper sqlHelper, AddServiceIn addData)
        {
            t_service model = new t_service
            {
                conn_ip = addData.server_ip.Trim(),
                conn_mode = (int)EOSConnectMode.UserAndPassword,
                conn_password = ConcealCommon.EncryptDES(addData.server_password),
                conn_port = int.Parse(addData.server_port),
                conn_user = addData.server_account,
                name = addData.server_name,
                platform_type = int.Parse(addData.server_platform),
                work_spacepath = addData.server_space,
                add_time = DateTime.Now.GetSQLTime()
            };

            string sql = @"INSERT INTO t_service(name,conn_ip,conn_port,conn_mode,conn_user,conn_password,ssh_key,secret_salt,work_spacepath,platform_type,add_time)
                                        VALUES(@name,@conn_ip,@conn_port,@conn_mode,@conn_user,@conn_password,@ssh_key,@secret_salt,@work_spacepath,@platform_type,@add_time);select last_insert_rowid();";

            return await sqlHelper.ExecAsync(sql, model);
        }

        /// <summary>
        /// 获取所有服务器
        /// </summary>
        /// <param name="sqlHelper"></param>
        /// <returns></returns>
        public static async Task<List<t_service>> GetListAll(SQLiteHelper sqlHelper)
        {
            string sql = @"SELECT * FROM t_service";
            return await sqlHelper.QueryListAsync<t_service>(sql);
        }

        /// <summary>
        /// 获取所有服务器 下拉
        /// </summary>
        /// <param name="sqlHelper"></param>
        /// <returns></returns>
        public static async Task<List<t_service>> GetKvAll(SQLiteHelper sqlHelper)
        {
            string sql = @"SELECT id,name,conn_ip,conn_user FROM t_service";
            return await sqlHelper.QueryListAsync<t_service>(sql);
        }

        /// <summary>
        /// 服务器是否存在
        /// </summary>
        /// <param name="sqlHelper"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static async Task<bool> IsExist(SQLiteHelper sqlHelper, int id)
        {
            string sql = @"SELECT id FROM t_service WHERE id=@id";
            return await sqlHelper.QueryAsync<int>(sql, new { id = id }) > 0;
        }

        /// <summary>
        /// 检查服务器是否被使用
        /// </summary>
        /// <param name="db"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static async Task<bool> UsedService(SQLiteHelper db, int id)
        {
            string flow_sql = @"SELECT COUNT(1) FROM t_flow_project WHERE serv_id=@serv_id";

            return await db.QueryAsync<int>(flow_sql, new { serv_id = id }) > 0;
        }

        /// <summary>
        /// 删除服务器
        /// </summary>
        /// <param name="db"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static async Task<bool> Delete(SQLiteHelper db, int id)
        {
            string sql = @"DELETE FROM t_service WHERE id=@id";
            return await db.ExecAsync(sql, new { id = id }) > 0;
        }

        /// <summary>
        /// 获取服务器
        /// </summary>
        /// <param name="db"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static async Task<t_service> GetService(SQLiteHelper db, int id)
        {
            string sql = @"SELECT * FROM t_service WHERE id=@id";
            return await db.QueryAsync<t_service>(sql, new { id = id });
        }

        /// <summary>
        /// 更新服务器
        /// </summary>
        /// <param name="db"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static async Task<bool> Update(SQLiteHelper db, EditServiceIn data)
        {
            t_service model = new t_service
            {
                id = int.Parse(data.server_id),
                conn_ip = data.server_ip,
                conn_password = ConcealCommon.EncryptDES(data.server_password),
                conn_port = int.Parse(data.server_port),
                conn_user = data.server_account,
                name = data.server_name,
                platform_type = int.Parse(data.server_platform),
                work_spacepath = data.server_space
            };

            string sql = $@"UPDATE t_service
   SET 
       name = @name,
       conn_ip = @conn_ip,
       conn_port = @conn_port,
       conn_user = @conn_user,
       {(data.server_password == GetCommon.GetHidePassword() ? "" : "conn_password = @conn_password,")}
       ssh_key = @ssh_key,
       secret_salt = @secret_salt,
       work_spacepath = @work_spacepath,
       platform_type = @platform_type
 WHERE id = @id";
            return await db.ExecAsync(sql, model) > 0;
        }
    }
}
