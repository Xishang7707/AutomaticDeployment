using System.Collections.Generic;
using System.Data.SQLite;
using System.Threading.Tasks;
using System.Data.Common;
using Dapper;
using System.Linq;

namespace DAO
{
    public class SQLiteHelper
    {
        private const string conn_str = @"data source=asset/autodev.db;version=3;";
        private SQLiteConnection conn;
        private DbTransaction tran;
        public SQLiteHelper()
        {
        }

        /// <summary>
        /// 连接
        /// </summary>
        /// <returns></returns>
        public async Task ConnectAsync()
        {
            if (conn == null || conn.State != System.Data.ConnectionState.Open && conn.State != System.Data.ConnectionState.Executing && conn.State != System.Data.ConnectionState.Fetching)
            {
                conn = new SQLiteConnection(conn_str);
                await conn.OpenAsync();
            }
        }

        /// <summary>
        /// 断开
        /// </summary>
        /// <returns></returns>
        public void Close()
        {
            if (conn != null && conn.State != System.Data.ConnectionState.Closed)
            {
                conn.CloseAsync();
            }
        }

        /// <summary>
        /// 开启事务
        /// </summary>
        /// <returns></returns>
        public async Task BeginTransactionAsync()
        {
            await ConnectAsync();
            tran = await conn.BeginTransactionAsync();
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        /// <returns></returns>
        public async Task CommitAsync()
        {
            await tran.CommitAsync();
            tran = null;
        }

        /// <summary>
        /// 回滚
        /// </summary>
        /// <returns></returns>
        public async Task RollbackAsync()
        {
            if (tran != null)
                await tran.RollbackAsync();
            tran = null;
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="sql">SQL</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public async Task<List<T>> QueryListAsync<T>(string sql, object param = null)
        {
            return (await conn.QueryAsync<T>(sql, param, tran)).ToList();
        }

        /// <summary>
        /// 查询单条
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">SQL</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public async Task<T> QueryAsync<T>(string sql, object param = null)
        {
            return await conn.QueryFirstOrDefaultAsync<T>(sql, param, tran);
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<T> ExecAsync<T>(string sql, object param)
        {
            return await conn.ExecuteScalarAsync<T>(sql, param, tran);
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<int> ExecAsync(string sql, object param)
        {
            return await conn.ExecuteAsync(sql, param, tran);
        }
    }
}
