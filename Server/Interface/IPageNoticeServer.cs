﻿using Model.In.PageNotice;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations.Model;
using System.Text;

namespace Server.Interface
{
    /// <summary>
    /// 页面通知
    /// </summary>
    public interface IPageNoticeServer : IServer
    {
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="pid">页面id</param>
        /// <param name="data">数据</param>
        void Update(string pid, string data = null);
    }
}
