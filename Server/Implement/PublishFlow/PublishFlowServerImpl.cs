using DAO;
using DAO.PublishFlow;
using Model;
using Model.Db;
using Model.Db.Enum;
using Model.In;
using Model.In.PublishFlow;
using Model.Out;
using Server.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Server.Implement.PublishFlow
{
    public class PublishFlowServerImpl : IPublishFlowServer
    {
        /// <summary>
        /// 验证快速发布数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private async Task<Result> VerifyQuickPublish(PublishQuickProject data)
        {
            Result result = new Result();
            if (data == null)
            {
                result.msg = Tip.TIP_1;
                return result;
            }
            if (string.IsNullOrWhiteSpace(data.project_uid))
            {
                result.msg = Tip.TIP_24;
                return result;
            }
            SQLiteHelper dbHelper = new SQLiteHelper();
            t_project projModel = await ProjectDao.GetProjectTypeAsync(dbHelper, data.project_uid);
            if (projModel == null || ((EProjectType)projModel.proj_type) != EProjectType.Quick)
            {
                result.msg = Tip.TIP_24;
                return result;
            }
            dbHelper.Close();
            if (data.files == null || data.files.Count == 0)
            {
                result.msg = Tip.TIP_25;
                return result;
            }
            result.result = true;
            return result;
        }

        public async Task<Result> PublishAsync(In<PublishQuickProject> inData)
        {
            Result result = await VerifyQuickPublish(inData.data);
            SQLiteHelper dbHelper = new SQLiteHelper();
            t_quick_project quickProject = await QuickProjectDao.GetProject(dbHelper, inData.data.project_uid);
            result.result = await PublishFlowDao.Insert(dbHelper, quickProject, inData.data.files);
            if (!result.result)
            {
                result.msg = Tip.TIP_26;
                return result;
            }
            result.result = true;
            result.msg = Tip.TIP_27;
            return result;
        }
    }
}
