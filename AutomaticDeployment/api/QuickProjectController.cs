using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App;
using App.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.In.PublishFlow;
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

        /// <summary>
        /// 发布
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost("publish")]
        public async Task<IActionResult> Publish([FromBody]PublishQuickProject form)
        {
            IQuickProjectApp app = AppFactory.Get<IQuickProjectApp>();
            return await app.Publish(PackRequest(form));
        }

        /// <summary>
        /// 获取项目信息
        /// </summary>
        /// <param name="project_uid">项目guid</param>
        /// <returns></returns>
        [HttpGet("getproject")]
        public async Task<IActionResult> GetProject([FromQuery]string project_uid)
        {
            IQuickProjectApp app = AppFactory.Get<IQuickProjectApp>();
            return await app.GetProjectAsync(PackRequest(project_uid));
        }

        /// <summary>
        /// 编辑项目
        /// </summary>
        /// <param name="model">项目信息</param>
        /// <returns></returns>
        [HttpPost("editproject")]
        public async Task<IActionResult> EditProject([FromBody]EditQuickProjectIn model)
        {
            IQuickProjectApp app = AppFactory.Get<IQuickProjectApp>();
            return await app.EditProject(PackRequest(model));
        }
    }
}
