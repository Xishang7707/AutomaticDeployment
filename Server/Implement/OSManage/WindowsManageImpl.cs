using Model.In.OSManage;
using Model.Out;
using Model.Out.OSManage;
using Server.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Implement.OSManage
{
    /// <summary>
    /// Windows管理
    /// </summary>
    public class WindowsManageImpl : IOSManageServer
    {
        public Result ChangeWorkspace(string path)
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public Result Connect(UserConnectIn conn)
        {
            throw new NotImplementedException();
        }

        public ExecResult Exec(string shell)
        {
            throw new NotImplementedException();
        }

        public Result UnZip(string fileName)
        {
            throw new NotImplementedException();
        }

        public Model.Out.OSManage.UploadResult Upload(string originPath, string targetPath, string fileName)
        {
            throw new NotImplementedException();
        }
    }
}
