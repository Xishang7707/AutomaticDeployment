using App.Model.Data;
using App.Model.Result;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Interface
{
    public interface IDemoApp : IApp
    {
        /// <summary>
        /// 发布
        /// </summary>
        /// <param name="appData"></param>
        /// <returns></returns>
        public AppResult<string> Publish(AppData appData);
    }
}
