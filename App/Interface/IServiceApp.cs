using Model.In;
using Model.In.Service;
using Model.Out;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App.Interface
{
    /// <summary>
    /// 服务器
    /// </summary>
    public interface IServiceApp : IApp
    {
        /// <summary>
        /// 添加服务器
        /// </summary>
        /// <param name="inData"></param>
        /// <returns></returns>
        Task<Result> AddService(In<AddServiceIn> inData);
    }
}
