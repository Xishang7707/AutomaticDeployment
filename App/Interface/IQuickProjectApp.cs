using Model.In;
using Model.In.QuickProject;
using Model.Out;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App.Interface
{
    /// <summary>
    /// 快速发布
    /// </summary>
    public interface IQuickProjectApp : IApp
    {
        /// <summary>
        /// 添加快速发布项目
        /// </summary>
        /// <param name="inData"></param>
        /// <returns></returns>
        Task<Result> AddProjectAsync(In<AddQuickProjectIn> inData);

        /// <summary>
        /// 获取项目列表
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Task<Result> GetProjectListAsync(In data);
    }
}
