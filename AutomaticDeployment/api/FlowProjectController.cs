using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App;
using App.Interface;
using Microsoft.AspNetCore.Mvc;
using Model.In.FlowProject;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AutomaticDeployment.api
{
    [Route("api/[controller]")]
    public class FlowProjectController : BaseController
    {
        private IFlowProjectApp app = AppFactory.Get<IFlowProjectApp>();

        /// <summary>
        /// 添加项目
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("addproject")]
        public async Task<IActionResult> AddProject([FromBody]AddProjectIn data)
        {
            return await app.AddProject(PackRequest(data));
        }

        /// <summary>
        /// 获取项目列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("getprojectlist")]
        public async Task<IActionResult> GetProjectList()
        {
            return await app.GetProjectList();
        }
    }
}
