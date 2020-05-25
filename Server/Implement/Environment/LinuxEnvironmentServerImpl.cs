using Model.Db.Enum;
using Model.In.Environment;
using Model.Out;
using Model.Out.Environment;
using Server.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Implement.Environment
{
    /// <summary>
    /// Linux环境
    /// </summary>
    class LinuxEnvironmentServerImpl : EnvironmentServerImpl, IEnvironmentServer
    {
        public async Task<Result> CheckLocalBashes()
        {
            IOSManageServer osServer = ServerFactory.Get<IOSManageServer>(EOSPlatform.Linux);
            Result res = osServer.Connect(new Model.In.OSManage.UserConnectIn { host = "127.0.0.1", password = ServerConfig.OSPassword, port = ServerConfig.OSPort, user = ServerConfig.OSUser });
            if (!res.result)
            {
                return res;
            }

            Result<List<CheckItemResult>> result = new Result<List<CheckItemResult>> { result = true, data = new List<CheckItemResult>() };
            foreach (var item in bashItems)
            {
                CheckItemResult it = new CheckItemResult
                {
                    bash = item.bash,
                    pass = osServer.Exec($"rpm -qa | grep {item.bash}").result,
                    title = item.title,
                    version = ""
                };
                if (item.ParseVersion != null)
                {
                    Result<List<string>> it_res = item.ParseVersion(osServer);
                    if (it_res.result)
                    {
                        it.version = string.Join(" ", it_res.data.Select(s => $"@{s}"));
                    }
                }
                result.data.Add(it);
            }
            osServer.Close();
            return result;
        }
    }
}
