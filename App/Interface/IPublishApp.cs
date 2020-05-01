using Model.In;
using Model.Out;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Interface
{
    /// <summary>
    /// 发布
    /// </summary>
    public interface IPublishApp : IApp
    {
        /// <summary>
        /// 演示发布
        /// </summary>
        /// <param name="demoPublishIn"></param>
        /// <returns></returns>
        Result PublishDemo(DemoPublishIn demoPublishIn);
    }
}
