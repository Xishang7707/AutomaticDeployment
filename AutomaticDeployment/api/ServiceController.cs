using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App;
using App.Interface;
using Microsoft.AspNetCore.Mvc;
using Model.In.Service;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AutomaticDeployment.api
{
    /// <summary>
    /// 服务器
    /// </summary>
    [Route("api/[controller]")]
    public class ServiceController : BaseController
    {
        private IServiceApp serviceApp = AppFactory.Get<IServiceApp>();

        /// <summary>
        /// 添加服务器
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("addservice")]
        public async Task<IActionResult> AddService([FromBody] AddServiceIn model)
        {
            return await serviceApp.AddService(PackRequest(model));
        }

        /// <summary>
        /// 获取服务器列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("getdropservice")]
        public async Task<IActionResult> GetDropService()
        {
            return await serviceApp.GetDropService();
        }

        /// <summary>
        /// 获取服务器列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("getservicelist")]
        public async Task<IActionResult> GetServiceList()
        {
            return await serviceApp.GetServiceList();
        }

        /// <summary>
        /// 删除服务器
        /// </summary>
        /// <returns></returns>
        [HttpPost("deleteservice")]
        public async Task<IActionResult> DeleteService([FromBody] DeleteServiceIn model)
        {
            return await serviceApp.DeleteService(PackRequest(model));
        }

        /// <summary>
        /// 获取服务器
        /// </summary>
        /// <returns></returns>
        [HttpGet("getservice")]
        public async Task<IActionResult> GetService([FromQuery] string id)
        {
            return await serviceApp.GetService(PackRequest(id));
        }

        /// <summary>
        /// 修改服务器
        /// </summary>
        /// <returns></returns>
        [HttpPost("editservice")]
        public async Task<IActionResult> EditService([FromBody] EditServiceIn model)
        {
            return await serviceApp.EditService(PackRequest(model));
        }
    }
}
