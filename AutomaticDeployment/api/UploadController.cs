using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App;
using App.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.In;
using Model.In.Upload;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AutomaticDeployment.api
{
    [Route("api/[controller]")]
    public class UploadController : BaseController
    {
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="uploadIn"></param>
        /// <returns></returns>
        [HttpPost("uploaddemo")]
        public async Task<IActionResult> UploadDemo([FromForm]IFormCollection form)
        {
            IUploadApp uploadApp = AppFactory.Get<IUploadApp>();
            return await uploadApp.UploadDemo(new UploadIn { Files = form.Files });
        }

        /// <summary>
        /// 发布文件上传
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm]IFormCollection info)
        {
            IUploadApp uploadApp = AppFactory.Get<IUploadApp>();
            return await uploadApp.PublishUpload(new PublishUploadIn { files = info.Files, project_uid = info["project_uid"] });
        }
    }
}
