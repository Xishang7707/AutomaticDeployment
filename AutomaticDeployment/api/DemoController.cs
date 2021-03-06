﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App;
using App.Interface;
using Microsoft.AspNetCore.Mvc;
using Model.In;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AutomaticDeployment.api
{
    [Route("api/[controller]")]
    public class DemoController : Controller
    {
        /// <summary>
        /// 模拟发布
        /// </summary>
        /// <param name="demoPublishIn"></param>
        /// <returns></returns>
        [HttpPost("publishdemo")]
        public IActionResult PublishDemo([FromBody]DemoPublishIn demoPublishIn)
        {
            IPublishApp publishApp = AppFactory.Get<IPublishApp>();
            return publishApp.PublishDemo(demoPublishIn);
        }
    }
}
