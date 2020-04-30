using System;
using System.Collections.Generic;
using System.Text;

namespace App.Model.Data
{
    public class AppData
    {
    }

    public class AppData<T> : AppData
    {
        public T Data { get; set; }
    }
}
