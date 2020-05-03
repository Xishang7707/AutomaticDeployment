using Model;
using Model.Out;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Common
{
    /// <summary>
    /// SFTP
    /// </summary>
    public class SftpHelper
    {
        private string host;
        private int port;
        private string user;
        private string pwd;

        /// <summary>
        /// SSH连接
        /// </summary>
        private SftpClient sftpClient;

        public SftpHelper(string host, int port, string user, string pwd)
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
                if (sftpClient == null || !sftpClient.IsConnected)
                {
                    sftpClient = new SftpClient(host, port, user, pwd);
                    sftpClient.Connect();
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
        /// 上传文件
        /// </summary>
        /// <param name="filePath">文件+路径</param>
        /// <param name="targetPath">目标路径</param>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        public Result Upload(string filePath, string targetPath, string fileName)
        {
            Result res = Connect();
            if (!res.result)
            {
                return res;
            }
            res.result = false;
            if (!File.Exists(filePath))
            {
                res.msg = Tip.TIP_15;
                return res;
            }
            if (!sftpClient.Exists(targetPath))
            {
                CreateDirectory(targetPath);
            }
            using FileStream fs = File.Open(filePath, FileMode.Open);
            sftpClient.UploadFile(fs, GetCommon.GetFileSplicing(targetPath, fileName));
            res.result = true;
            return res;
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        public void Close()
        {
            if (sftpClient != null && sftpClient.IsConnected)
            {
                sftpClient.Disconnect();
            }
        }

        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="path"></param>
        public Result CreateDirectory(string path)
        {
            Result res = Connect();
            if (!res.result)
            {
                return res;
            }
            string[] directoryArr = GetCommon.GetDirectoryNames(path);
            for (int i = 1; i <= directoryArr.Length; i++)
            {
                string stepPath = string.Join('/', directoryArr, 0, i);
                if (!sftpClient.Exists(stepPath))
                {
                    sftpClient.CreateDirectory(stepPath);
                }
            }
            return res;
        }
    }
}
