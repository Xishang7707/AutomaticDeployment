using Model.Out;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App.Interface
{
    /// <summary>
    /// 环境
    /// </summary>
    public interface IEnvironmentApp : IApp
    {
        /// <summary>
        /// 检查
        /// </summary>
        /// <returns></returns>
        Task<Result> CheckLocalBashes();
    }
}
