using Model.Out;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Ssh
{
    /// <summary>
    /// 命令执行结果
    /// </summary>
    public class ExecResult : SshResult
    {
        /// <summary>
        /// 返回码
        /// </summary>
        public int return_code { get; set; }
    }
}
