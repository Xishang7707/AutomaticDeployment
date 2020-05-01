using Common;
using Model;
using Model.In;
using Model.Out;
using Model.Ssh;
using Server.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Server.Implement
{
    /// <summary>
    /// 发布
    /// </summary>
    internal class PublishServerImpl : IPublishServer
    {
        /// <summary>
        /// 验证数据
        /// </summary>
        /// <param name="demoPublishIn"></param>
        /// <returns></returns>
        private Result VerifyDemoPublish(DemoPublishIn demoPublishIn)
        {
            Result result = new Result();
            if (demoPublishIn == null)
            {
                result.msg = Tip.TIP_1;
                return result;
            }

            if (demoPublishIn.server == null)
            {
                result.msg = Tip.TIP_4;
                return result;
            }
            if (string.IsNullOrWhiteSpace(demoPublishIn.server.server_ip))
            {
                result.msg = Tip.TIP_5;
                return result;
            }
            if (!RegexCommon.IP(demoPublishIn.server.server_ip))
            {
                result.msg = Tip.TIP_6;
                return result;
            }
            if (string.IsNullOrWhiteSpace(demoPublishIn.server.server_port))
            {
                result.msg = Tip.TIP_7;
                return result;
            }
            if (!int.TryParse(demoPublishIn.server.server_port, out int verifyServerPort) || verifyServerPort <= 0 || verifyServerPort > 65535)
            {
                result.msg = Tip.TIP_8;
                return result;
            }
            if (string.IsNullOrWhiteSpace(demoPublishIn.server.server_account))
            {
                result.msg = Tip.TIP_9;
                return result;
            }
            if (string.IsNullOrWhiteSpace(demoPublishIn.server.server_password))
            {
                result.msg = Tip.TIP_10;
                return result;
            }
            if (demoPublishIn.project == null)
            {
                result.msg = Tip.TIP_11;
                return result;
            }
            if (string.IsNullOrWhiteSpace(demoPublishIn.project.file_id))
            {
                result.msg = Tip.TIP_12;
                return result;
            }
            if (demoPublishIn.publish == null)
            {
                result.msg = Tip.TIP_13;
                return result;
            }
            if (string.IsNullOrWhiteSpace(demoPublishIn.publish.publish_path))
            {
                result.msg = Tip.TIP_14;
                return result;
            }
            result.result = true;
            return result;
        }

        public Result PublishDemo(DemoPublishIn demoPublishIn)
        {

            Result verifyResult = VerifyDemoPublish(demoPublishIn);
            if (!verifyResult.result)
            {
                return verifyResult;
            }

            PublishInfo info = ConnectService(demoPublishIn);
            if (!info.result)
            {
                return info;
            }
            //发布前命令
            info = ExecBeforeCommand(info);
            if (!info.result)
            {
                return info;
            }
            //上传文件
            info = PublishToService(info);
            if (!info.result)
            {
                return info;
            }
            //发布后命令
            info = ExecAfterCommand(info);
            if (!info.result)
            {
                return info;
            }

            return new Result
            {
                msg = Tip.TIP_16,
                result = info.result
            };
        }

        /// <summary>
        /// 发布数据对象
        /// </summary>
        class PublishInfo : Result
        {
            /// <summary>
            /// ssh连接
            /// </summary>
            public SshHelper sshHelper { get; set; }

            /// <summary>
            /// sftp连接
            /// </summary>
            public SftpHelper sftpHelper { get; set; }

            /// <summary>
            /// 发布信息
            /// </summary>
            public DemoPublishIn PublishData { get; set; }
        }

        /// <summary>
        /// 发布第一步 连接服务器
        /// </summary>
        /// <param name="demoPublishIn"></param>
        /// <returns></returns>
        private PublishInfo ConnectService(DemoPublishIn demoPublishIn)
        {
            PublishInfo info = new PublishInfo();
            info.PublishData = demoPublishIn;
            if (!string.IsNullOrWhiteSpace(demoPublishIn.publish.publish_before_command) ||
                !string.IsNullOrWhiteSpace(demoPublishIn.publish.publish_after_command))
            {
                //连接ssh
                info.sshHelper = new SshHelper(demoPublishIn.server.server_ip, int.Parse(demoPublishIn.server.server_port), demoPublishIn.server.server_account, demoPublishIn.server.server_password);
                Result sshResult = info.sshHelper.Connect();
                if (!sshResult.result)
                {
                    info.msg = sshResult.msg;
                    return info;
                }
            }

            //连接sftp
            info.sftpHelper = new SftpHelper(demoPublishIn.server.server_ip, int.Parse(demoPublishIn.server.server_port), demoPublishIn.server.server_account, demoPublishIn.server.server_password);
            Result sftpResult = info.sftpHelper.Connect();
            if (!sftpResult.result)
            {
                info.msg = sftpResult.msg;
                return info;
            }

            info.result = true;
            return info;
        }

        /// <summary>
        /// 发布前命令 --连接
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private PublishInfo ExecBeforeCommand(PublishInfo info)
        {
            if (string.IsNullOrWhiteSpace(info.PublishData.publish.publish_before_command))
            {
                info.result = true;
                return info;
            }
            info.result = false;
            if (info.sshHelper == null)
            {
                return info;
            }

            ExecResult execResult = info.sshHelper.Exec(string.Join("&&", GetCommon.GetCommands(info.PublishData.publish.publish_before_command)));
            if (!execResult.result)
            {
                info.msg = execResult.msg;
                return info;
            }

            info.result = true;
            return info;
        }

        /// <summary>
        /// 发布文件到服务器
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private PublishInfo PublishToService(PublishInfo info)
        {
            info.result = false;
            if (info.sftpHelper == null)
            {
                return info;
            }

            string filePath = ServerCommon.GetUploadPath(info.PublishData.project.file_id);

            Result uploadResult = info.sftpHelper.Upload(filePath, info.PublishData.publish.publish_path, info.PublishData.project.file_id);
            info.sftpHelper.Close();
            if (!uploadResult.result)
            {
                info.msg = uploadResult.msg;
                return info;
            }

            info.result = true;
            return info;
        }

        /// <summary>
        /// 发布后命令 --连接
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private PublishInfo ExecAfterCommand(PublishInfo info)
        {
            if (string.IsNullOrWhiteSpace(info.PublishData.publish.publish_after_command))
            {
                info.result = true;
                return info;
            }
            info.result = false;
            if (info.sshHelper == null)
            {
                return info;
            }

            ExecResult execResult = info.sshHelper.Exec(string.Join("&&", GetCommon.GetCommands(info.PublishData.publish.publish_after_command)));
            if (!execResult.result)
            {
                info.msg = execResult.msg;
                return info;
            }
            info.result = true;
            return info;
        }
    }
}
