using Microsoft.AspNetCore.Http;
using Model.Out;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomaticDeployment.Extend
{
    public static class ResponseEx
    {
        /// <summary>
        /// @xis 向客户端写数据 2020-2-21 12:43:19
        /// </summary>
        /// <param name="res"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Task WriteBodyAsync(this HttpResponse res, Result obj, int code)
        {
            res.HttpContext.Response.StatusCode = code;
            string str = JsonConvert.SerializeObject(obj);
            var bt = Encoding.Default.GetBytes(str);
            res.ContentType = "application/json";
            return res.Body.WriteAsync(bt, 0, bt.Length);
        }
    }
}
