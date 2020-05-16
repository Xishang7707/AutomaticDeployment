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
                work_spacepath = addData.workspace,
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
            string sql = @"SELECT name,conn_ip,conn_port,conn_mode,conn_user,conn_password,ssh_key,secret_key,work_spacepath,platform_type,add_time FROM t_service";
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
    }
}
