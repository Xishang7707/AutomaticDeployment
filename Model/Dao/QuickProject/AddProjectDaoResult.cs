using Model.Dao;
using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Dao.QuickProject
{
    /// <summary>
    /// 添加项目
    /// </summary>
    public class AddProjectDaoResult : DaoResult
    {
        /// <summary>
        /// 项目id
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 项目guid
        /// </summary>
        public string proj_guid { get; set; }
    }
}
