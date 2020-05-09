using Model.In.SqlManage;
using Model.Out;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Server.Interface
{
    /// <summary>
    /// 数据库服务
    /// </summary>
    public interface ISqlManageServer : IServer
    {
        /// <summary>
        /// 数据库连接
        /// </summary>
        /// <param name="host">主机</param>
        /// <param name="port">端口</param>
        /// <param name="user">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        Result Connect(string host, int port, string user, string password);

        /// <summary>
        /// 数据库连接 用户名密码登录
        /// </summary>
        /// <param name="conn">登录信息</param>
        /// <returns></returns>
        Result Connect(UserConnectIn conn);

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="sql">SQL</param>
        /// <returns></returns>
        Task<T> ExecAsync<T>(string sql);

        /// <summary>
        /// 执行文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        Task<Result> ExecFileAsync(string filePath);

        /// <summary>
        /// 关闭连接
        /// </summary>
        void Close();
    }
}
