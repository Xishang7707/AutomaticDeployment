using Model.Out;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Server.Interface
{
    /// <summary>
    /// 环境
    /// </summary>
    public interface IEnvironmentServer : IServer
    {
        /// <summary>
        /// 检查
        /// </summary>
        /// <returns></returns>
        Task<Result> CheckLocalBashes();
    }
}
