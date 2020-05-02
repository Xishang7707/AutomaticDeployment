using Model.In;
using Model.In.QuickProject;
using Model.Out;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Server.Interface
{
    /// <summary>
    /// 快速项目
    /// </summary>
    public interface IQuickProjectServer : IServer
    {
        /// <summary>
        /// 添加快速项目
        /// </summary>
        /// <param name="inData"></param>
        /// <returns></returns>
        Task<Result> AddQuickProjectAsync(In<AddQuickProjectIn> inData);
    }
}
