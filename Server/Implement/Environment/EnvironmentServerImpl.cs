using Model.Extend;
using Model.In.Environment;
using Model.Out;
using Server.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.Implement.Environment
{
    abstract class EnvironmentServerImpl
    {
        //命令
        protected static List<BashItem> bashItems = new List<BashItem>
        {
            new BashItem
            {
                title="GIT",
                bash="git",
                install="yum install -y git",
                ParseVersion = i =>
                {
                    IOSManageServer osManageServer = i as IOSManageServer;
                    Result<List<string>> result = osManageServer.Exec("git --version").Cast<Result<List<string>>>();
                    if(!result.result)
                    {
                        return result;
                    }
                    List<string> version = result.msg.Split(' ').ToList();
                    result.data = new List<string>
                    {
                        version[2]
                    };
                    return result;
                }
            },
            new BashItem
            {
                title="压缩ZIP",
                bash="zip",
                install="yum install -y zip",
            },
            new BashItem
            {
                title="解压ZIP",
                bash="unzip",
                install="yum install -y unzip",
            },
            new BashItem
            {
                title="Net Core",
                bash="dotnet",
                install=@"sudo rpm -Uvh https://packages.microsoft.com/config/centos/7/packages-microsoft-prod.rpm && sudo yum install dotnet-sdk-3.1",
                ParseVersion = i =>
                {
                    IOSManageServer osManageServer = i as IOSManageServer;
                    Result<List<string>> result = osManageServer.Exec("dotnet --list-sdks").Cast<Result<List<string>>>();
                    if(!result.result)
                    {
                        return result;
                    }
                    result.data = result.msg.Split('\n').Where(w=>!string.IsNullOrWhiteSpace(w)).Select(s=>s.Split(' ')[0]).ToList();
                    return result;
                }
            }
        };
    }
}
