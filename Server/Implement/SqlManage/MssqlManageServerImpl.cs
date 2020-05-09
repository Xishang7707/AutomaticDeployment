using Common;
using Model;
using Model.In.SqlManage;
using Model.Out;
using Server.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Server.Implement.SqlManage
{
    /// <summary>
    /// mssql
    /// </summary>
    internal class MssqlManageServerImpl : ISqlManageServer
    {
        /// <summary>
        /// 连接信息
        /// </summary>
        private MssqlDbHelper dbHelper { get; set; }

        public void Close()
        {
            dbHelper.Close();
        }

        public Result Connect(string host, int port, string user, string password)
        {
            dbHelper = new MssqlDbHelper(host, port, user, password);
            return dbHelper.Connect();
        }

        public Result Connect(UserConnectIn conn)
        {
            dbHelper = new MssqlDbHelper(conn.host, conn.port, conn.user, conn.password);
            return dbHelper.Connect();
        }

        public async Task<T> ExecAsync<T>(string sql)
        {
            return await dbHelper.ExecScalarAsync<T>(sql);
        }

        public async Task<Result> ExecFileAsync(string filePath)
        {
            Result<string> result = new Result<string>();
            if (!File.Exists(filePath))
            {
                result.msg = Tip.TIP_33;
                return result;
            }
            try
            {
                using FileStream fs = new FileStream(filePath, FileMode.Open);
                using StreamReader reader = new StreamReader(fs);
                string sql = await reader.ReadToEndAsync();
                result.msg = await dbHelper.ExecScalarAsync<string>(sql);
                //go 一行执行
                //while (!reader.EndOfStream)
                //{
                //    string lineBuf = "";
                //    while (!reader.EndOfStream)
                //    {
                //        string str = reader.ReadLine();
                //        if (str?.ToLower() == "go")
                //        {
                //            break;
                //        }
                //        lineBuf += str;
                //        result.msg += await dbHelper.ExecScalarAsync<string>(lineBuf);
                //    }
                //}
                result.result = true;
                return result;
            }
            catch (Exception e)
            {
                result.msg = e.Message;
                return result;
            }
        }
    }
}
