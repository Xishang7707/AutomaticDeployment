using Common;
using Model.Extend;
using Model.In.OSManage;
using Model.Out;
using Model.Out.OSManage;
using Server.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Server.Implement.OSManage
{
    /// <summary>
    /// Linux管理
    /// </summary>
    public class LinuxManageImpl : IOSManageServer
    {
        /// <summary>
        /// 数据信息
        /// </summary>
        class OSInfo
        {
            /// <summary>
            /// 工作目录
            /// </summary>
            public string workspace { get; set; }
        }

        /// <summary>
        /// 数据信息
        /// </summary>
        private OSInfo osInfo { get; set; }

        /// <summary>
        /// SSH管理工具
        /// </summary>
        private SshHelper sshHelper { get; set; }

        /// <summary>
        /// SFTP管理工具
        /// </summary>
        private SftpHelper sftpHelper { get; set; }

        public LinuxManageImpl()
        {
            sshHelper = new SshHelper();
            sftpHelper = new SftpHelper();
            osInfo = new OSInfo();
        }

        public Result Connect(UserConnectIn conn)
        {
            //连接ssh
            Result result = sshHelper.Connect(conn.host, conn.port, conn.user, conn.password);
            if (!result.result)
            {
                return result;
            }

            //连接sftp
            result = sftpHelper.Connect(conn.host, conn.port, conn.user, conn.password);
            return result;
        }

        public ExecResult Exec(string shell)
        {
            return sshHelper.Exec(string.Join("&&", GetCommon.GetCommands(shell))).Cast<ExecResult>();
        }

        public Model.Out.OSManage.UploadResult Upload(string originPath, string targetPath, string fileName)
        {
            return sftpHelper.Upload(originPath, targetPath, fileName).Cast<Model.Out.OSManage.UploadResult>();
        }

        public Result ChangeWorkspace(string path)
        {
            ExecResult result;
            if (!sftpHelper.IsExistDirectory(path))
            {
                result = sftpHelper.CreateDirectory(path).Cast<ExecResult>();
                if (!result.result)
                {
                    return result;
                }
            }

            result = sshHelper.Exec(Command_cd(path)).Cast<ExecResult>();
            if (!result.result)
            {
                return result;
            }
            osInfo.workspace = path;
            return result;
        }

        #region 命令

        /// <summary>
        /// cd
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        string Command_cd(string path)
        {
            return $"cd {path}";
        }

        /// <summary>
        /// unzip
        /// </summary>
        /// <param name="path">文件名</param>
        /// <returns></returns>
        string Command_unzip(string path)
        {
            string cd = string.IsNullOrWhiteSpace(osInfo.workspace) ? "" : Command_cd(osInfo.workspace) + "&&";
            return $"{cd}unzip -O GBK {path}";
        }

        #endregion

        public Result UnZip(string fileName)
        {
            return sshHelper.Exec(Command_unzip(fileName));
        }

        public void Close()
        {
            sshHelper.Close();
            sftpHelper.Close();
        }
    }
}
