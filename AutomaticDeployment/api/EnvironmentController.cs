using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App;
using App.Interface;
using Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutomaticDeployment.api
{
    /// <summary>
    /// 环境
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class EnvironmentController : BaseController
    {
        private IEnvironmentApp localApp = AppFactory.Get<IEnvironmentApp>(GetCommon.GetCurrentOS());

        /// <summary>
        /// 检查本地命令
        /// </summary>
        /// <returns></returns>
        [HttpGet("checklocalbashes")]
        public async Task<IActionResult> CheckLocalBashes()
        {
            return await localApp.CheckLocalBashes();
        }
    }
}
