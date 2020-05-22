using Model.Extend;
using Model.Out;
using Model.Ssh;
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

        public SshHelper() { }

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
        public SshResult Connect()
        {
            SshResult result = new SshResult();
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
        /// 连接
        /// </summary>
        /// <param name="host">主机</param>
        /// <param name="port">端口</param>
        /// <param name="user">用户名</param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        public SshResult Connect(string host, int port, string user, string pwd)
        {
            SshResult result = new SshResult();
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
        public ExecResult Exec(string shell)
        {
            ExecResult res = Connect().Cast<ExecResult>();
            if (!res.result)
            {
                return res;
            }
            res.result = false;

            using (var cmd = sshClient.CreateCommand(shell, Encoding.UTF8))
            {
                res.msg = cmd.Execute();
                res.return_code = cmd.ExitStatus;
                if (cmd.ExitStatus != 0)
                {
                    res.msg = cmd.Error;
                    return res;
                }
                else if (string.IsNullOrWhiteSpace(res.msg))
                {
                    res.msg = cmd.Error;
                }
            }
            res.result = true;
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
