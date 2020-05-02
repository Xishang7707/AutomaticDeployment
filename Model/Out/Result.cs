using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Model.Out
{
    public class Result : IActionResult
    {
        /// <summary>
        /// 执行结果
        /// </summary>
        public bool result { get; set; }

        /// <summary>
        /// 信息
        /// </summary>
        public string msg { get; set; }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            var resp = context.HttpContext.Response;
            resp.StatusCode = result ? 200 : 403;
            resp.ContentType = "application/json";
            await resp.WriteAsync(JsonConvert.SerializeObject(this));
        }
    }

    public class Result<T> : Result
    {
        public T data { get; set; }
    }
}
