using AutomaticDeployment.Extend;
using Microsoft.AspNetCore.Http;
using Model;
using Model.Out;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutomaticDeployment.Middleware
{
    /// <summary>
    /// 授权+异常
    /// </summary>
    public class AuthMiddleware
    {
        class ExceptionResult : Result
        {
            public string ex { get; set; }
        }

        private RequestDelegate next;
        public AuthMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (Exception ex)
            {
                ExceptionResult res = new ExceptionResult
                {
                    result = false,
                    msg = Tip.TIP_23,
                    ex = JsonConvert.SerializeObject(ex)
                };
                await context.Response.WriteBodyAsync(res, 500);
            }
        }
    }
}
