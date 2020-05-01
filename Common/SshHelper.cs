using Model.Out;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    /// <summary>
    /// SSH
    /// </summary>
    public class SshHelper
    {
        private string host;
        private int port;
        private string user;
        private string pwd;

        /// <summary>
        /// SSH连接
        /// </summary>
        private SshClient sshClient;

        public SshHelper(string host, int port, string user, string pwd)
        {
            this.host = host;
            this.port = port;
            this.user = user;
            this.pwd = pwd;
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
                if (sshClient == null || !sshClient.IsConnected)
                {
                    sshClient = new SshClient(host, port, user, pwd);
                    sshClient.Connect();
                }
                result.result = true;
            }
            catch (Exception e)
            {
                result.msg = e.Message;
            }
            return result;
        }

        /// <summary>
        /// 执行脚本
        /// </summary>
        /// <param name="shell"></param>
        /// <returns></returns>
        public Result Exec(string shell)
        {
            Result res = Connect();
            if (!res.result)
            {
                return res;
            }

            using var cmd = sshClient.CreateCommand(shell);
            res.result = true;
            res.msg = cmd.Execute();
            return res;
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        public void Close()
        {
            if (sshClient != null && sshClient.IsConnected)
            {
                sshClient.Disconnect();
            }
        }
    }
}
