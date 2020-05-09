using Dapper;
using Model.Out;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// MSSQL 工具类
    /// </summary>
    public class MssqlDbHelper
    {
        private IDbConnection conn;
        private IDbTransaction tran;
        /// <summary>
        /// MSSQL
        /// </summary>
        /// <param name="host">主机</param>
        /// <param name="port">端口</param>
        /// <param name="user">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="database">数据库</param>
        public MssqlDbHelper(string host, int port, string user, string password, string database = null)
        {
            string str = $"Data Source={host}:{port};{(!string.IsNullOrWhiteSpace(database) ? $"Initial Catalog={database};" : null)}Persist Security Info=True;User ID={user};Password={password};";
            conn = new SqlConnection(str);
        }

        /// <summary>
        /// 连接
        /// </summary>
        /// <returns></returns>
        public Result Connect()
        {
            Result result = new Result();
            try
            {
                ConnectionState[] conStateArr = new ConnectionState[] { ConnectionState.Open, ConnectionState.Executing, ConnectionState.Fetching };
                if (!conStateArr.Any(a => a == conn.State))
                {
                    conn.Open();
                }
                result.result = true;
            }
            catch (Exception e)
            {
                result.msg = e.Message;
                return result;
            }
            return result;
        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        /// <returns></returns>
        public Result Close()
        {
            Result result = new Result();
            try
            {
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }
                result.result = true;
            }
            catch (Exception e)
            {
                result.msg = e.Message;
                return result;
            }
            return result;
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<T> ExecScalarAsync<T>(string sql, object param = null)
        {
            return await conn.ExecuteScalarAsync<T>(sql, param, tran);
        }
    }
}
