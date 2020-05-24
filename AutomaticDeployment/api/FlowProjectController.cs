using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App;
using App.Interface;
using Microsoft.AspNetCore.Mvc;
using Model.In.FlowProject;
using Model.In.PublishFlow;

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
        public async Task<IActionResult> AddProject([FromBody] AddProjectIn data)
        {
            return await app.AddProject(PackRequest(data));
        }

        /// <summary>
        /// 获取项目列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("getprojectlist")]
        public async Task<IActionResult> GetProjectList([FromQuery] SearchProjectIn model)
        {
            return await app.GetProjectList(PackRequest(model));
        }

        /// <summary>
        /// 发布
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("publish")]
        public async Task<IActionResult> Publish([FromBody] PublishFlowProject data)
        {
            return await app.Publish(PackRequest(data));
        }

        /// <summary>
        /// 获取项目信息
        /// </summary>
        /// <param name="project_uid"></param>
        /// <returns></returns>
        [HttpGet("getproject")]
        public async Task<IActionResult> GetProject([FromQuery] string project_uid)
        {
            return await app.GetProject(PackRequest(project_uid));
        }

        /// <summary>
        /// 获取项目信息
        /// </summary>
        /// <param name="project_uid"></param>
        /// <returns></returns>
        [HttpGet("getprojectinfo")]
        public async Task<IActionResult> GetProjectInfo([FromQuery] string project_uid)
        {
            return await app.GetProjectInfo(PackRequest(project_uid));
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("editproject")]
        public async Task<IActionResult> EditProject([FromBody] EditProjectIn data)
        {
            return await app.EditProject(PackRequest(data));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("deleteproject")]
        public async Task<IActionResult> DeleteProject([FromBody] DeleteProjectIn data)
        {
            return await app.DeleteProject(PackRequest(data));
        }

        /// <summary>
        /// 获取项目归类
        /// </summary>
        /// <returns></returns>
        [HttpGet("getclassify")]
        public async Task<IActionResult> GetClassify()
        {
            return await app.GetProjectClassify(PackRequest());
        }
    }
}
