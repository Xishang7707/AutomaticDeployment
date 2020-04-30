using System;
using System.Collections.Generic;
using System.Text;

namespace App.Model.Result
{
    public class AppResult
    {
    }

    public class AppResult<T>:AppResult
    {
        public T Data { get; set; }
    }
}
