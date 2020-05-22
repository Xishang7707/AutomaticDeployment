﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Out.OSManage
{
    /// <summary>
    /// 执行结果
    /// </summary>
    public class ExecResult : Result
    {
        /// <summary>
        /// 返回码
        /// </summary>
        public int return_code { get; set; }
    }
}
