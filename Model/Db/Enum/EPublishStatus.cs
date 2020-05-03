using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Model.Db.Enum
{
    /// <summary>
    /// 发布状态
    /// </summary>
    public enum EPublishStatus
    {
        /// <summary>
        /// 等待发布
        /// </summary>
        [Description("等待发布")]
        Waitting = 0,

        /// <summary>
        /// 进行中
        /// </summary>
        [Description("进行中")]
        Progress = 1,

        /// <summary>
        /// 发布成功
        /// </summary>
        [Description("发布成功")]
        Success = 2,

        /// <summary>
        /// 发布失败
        /// </summary>
        [Description("发布失败")]
        Failed = 3,
    }
}
