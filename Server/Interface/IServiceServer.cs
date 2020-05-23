using Model.In;
using Model.In.Service;
using Model.Out;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Server.Interface
{
    /// <summary>
    /// 服务器
    /// </summary>
    public interface IServiceServer : IServer
    {
        /// <summary>
        /// 添加服务器
        /// </summary>
        /// <param name="inData"></param>
        /// <returns></returns>
        Task<Result> AddService(In<AddServiceIn> inData);

        /// <summary>
        /// 获取服务器列表
        /// </summary>
        /// <returns></returns>
        Task<Result> GetDropService();

        /// <summary>
        /// 获取服务器
        /// </summary>
        /// <returns></returns>
        Task<Result> GetServiceList();

        /// <summary>
        /// 删除服务器
        /// </summary>
        /// <param name="inData"></param>
        /// <returns></returns>
        Task<Result> DeleteService(In<DeleteServiceIn> inData);

        /// <summary>
        /// 获取服务器
        /// </summary>
        /// <param name="inData"></param>
        /// <returns></returns>
        Task<Result> GetService(In<string> inData);

        /// <summary>
        /// 修改服务器
        /// </summary>
        /// <param name="inData"></param>
        /// <returns></returns>
        Task<Result> EditService(In<EditServiceIn> inData);
    }
}
