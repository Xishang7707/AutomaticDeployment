using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App;
using App.Interface;
using Microsoft.AspNetCore.Mvc;
using Model.In.QuickProject;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AutomaticDeployment.api
{
    [Route("api/[controller]")]
    public class QuickProjectController : BaseController
    {
        /// <summary>
        /// 添加项目
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("addproject")]
        public async Task<IActionResult> AddProject([FromBody]AddQuickProjectIn model)
        {
            IQuickProjectApp app = AppFactory.Get<IQuickProjectApp>();
            return await app.AddProjectAsync(PackRequest(model));
        }

        /// <summary>
        /// 获取项目列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("getprojectlist")]
        public async Task<IActionResult> GetProjectList()
        {
            IQuickProjectApp app = AppFactory.Get<IQuickProjectApp>();
            return await app.GetProjectListAsync(PackRequest());
        }
    }
}
