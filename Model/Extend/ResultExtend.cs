using Model.Out;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Extend
{
    public static class ResultExtend
    {
        /// <summary>
        /// 转换
        /// </summary>
        /// <typeparam name="T">结果类型</typeparam>
        /// <param name="res"></param>
        /// <returns></returns>
        public static T Cast<T>(this Result res) where T : Result, new()
        {
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(res));
        }
    }
}
