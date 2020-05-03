using System;
using System.Collections.Generic;
using System.Text;

namespace Model.In
{
    /// <summary>
    /// 参数数据
    /// </summary>
    public class In
    {

    }

    /// <summary>
    /// 参数数据
    /// </summary>
    public class In<T>
    {
        /// <summary>
        /// 数据
        /// </summary>
        public T data { get; set; }
    }
}
