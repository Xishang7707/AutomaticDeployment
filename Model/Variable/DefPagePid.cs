using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Variable
{
    /// <summary>
    /// 页面pid
    /// </summary>
    public static class DefPagePid
    {
        /// <summary>
        /// 自动发布项目
        /// </summary>
        public const string FlowProject = "flowproject";

        /// <summary>
        /// 添加项目
        /// </summary>
        public const string FlowProject_Add = "addflowproject";

        /// <summary>
        /// 编辑项目
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public static string FlowProject_Edit(string uid) => "editflowproject#" + uid;
        
        /// <summary>
        /// 发布项目
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public static string FlowProject_Publish(string uid) => "flowpublish#" + uid;

        /// <summary>
        /// 快速发布项目
        /// </summary>
        public const string QuickProject = "quickproject";

        /// <summary>
        /// 服务器
        /// </summary>
        public const string Service = "service";
    }
}
