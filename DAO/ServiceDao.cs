using Model.Db;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAO
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
        /// <param name="model"></param>
        /// <returns></returns>
        public static async Task<int> InsertAsync(SQLiteHelper sqlHelper, t_service model)
        {
            string sql = @"INSERT INTO t_service(name,conn_ip,conn_port,conn_mode,conn_user,conn_password,ssh_key,secret_key,work_spacepath,platform_type,add_time)
                                        VALUES(@name,@conn_ip,@conn_port,@conn_mode,@conn_user,@conn_password,@ssh_key,@secret_key,@work_spacepath,@platform_type,@add_time);select last_insert_rowid();";

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
    }
}
