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
    internal class PublishFlowServerImpl : IPublishFlowServer
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
            bool proj_exist_flag = await ProjectDao.IsExist(dbHelper, data.project_uid, EProjectType.Quick);
            dbHelper.Close();
            if (!proj_exist_flag)
            {
                result.msg = Tip.TIP_24;
                return result;
            }
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
            if (!result.result)
            {
                return result;
            }
            SQLiteHelper dbHelper = new SQLiteHelper();
            t_quick_project quickProject = await QuickProjectDao.GetProject(dbHelper, inData.data.project_uid);
            result.result = await PublishFlowDao.Insert(dbHelper, quickProject, inData.data.files);
            if (!result.result)
            {
                result.msg = Tip.TIP_26;
                return result;
            }

            result.msg = Tip.TIP_27;
            return result;
        }

        /// <summary>
        /// 验证流水项目
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private async Task<Result> VerifyFlowPublish(PublishFlowProject data)
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
            SQLiteHelper db = new SQLiteHelper();

            bool proj_exist_flag = await ProjectDao.IsExist(db, data.project_uid.Trim(), EProjectType.Flow);
            db.Close();
            if (!proj_exist_flag)
            {
                result.msg = Tip.TIP_24;
                return result;
            }
            result.result = true;
            return result;
        }

        public async Task<Result> PublishAsync(In<PublishFlowProject> inData)
        {
            Result result = await VerifyFlowPublish(inData.data);
            if (!result.result)
            {
                return result;
            }
            SQLiteHelper db = new SQLiteHelper();
            t_flow_project proj_model = await FlowProjectDao.GetProject(db, inData.data.project_uid);
            t_service service_model = await DAO.PublishFlow.ServiceDao.GetService(db, proj_model.serv_id);
            t_publish publish_modle = await PublishDao.GetPublish(db, proj_model.proj_guid);

            result.result = await PublishFlowDao.Insert(db, proj_model, service_model, publish_modle);
            if (!result.result)
            {
                result.msg = Tip.TIP_26;
                return result;
            }

            result.msg = Tip.TIP_27;
            return result;
        }
    }
}
