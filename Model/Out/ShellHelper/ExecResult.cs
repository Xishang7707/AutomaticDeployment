using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Out.ShellHelper
{
    public class ExecResult : Result
    {
        /// <summary>
        /// 返回码
        /// </summary>
        public int return_code { get; set; }
    }
}
