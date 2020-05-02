using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Model.In;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AutomaticDeployment.api
{
    public class BaseController : Controller
    {
        /// <summary>
        /// 包装请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public In<T> PackRequest<T>(T model)
        {
            return new In<T> { Data = model };
        }
    }
}
