using Common;
using DAO;
using DAO.FlowProject;
using Model;
using Model.Db.Enum;
using Model.In;
using Model.In.FlowProject;
using Model.Out;
using Server.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity.Hierarchy;
using System.Text;
using System.Threading.Tasks;

namespace Server.Implement.FlowProject
{
    class FlowProjectServerImpl : IFlowProjectServer
    {
        /// <summary>
        /// 验证添加项目数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private async Task<Result> VerifyAddProject(AddProjectIn data)
        {
            Result result = new Result();
            if (data == null)
            {
                result.msg = Tip.TIP_1;
                return result;
            }

            //验证项目信息
            if (data.project == null)
            {
                result.msg = Tip.TIP_11;
                return result;
            }
            if (string.IsNullOrWhiteSpace(data.project.service_id) || !int.TryParse(data.project.service_id, out int service_id))
            {
                result.msg = Tip.TIP_40;
                return result;
            }
            data.project.service_id = data.project.service_id.Trim();
            SQLiteHelper db = new SQLiteHelper();
            bool service_exist_flag = await DAO.Service.ServiceDao.IsExist(db, service_id);
            db.Close();
            if (!service_exist_flag)
            {
                result.msg = Tip.TIP_41;
                return result;
            }
            if (string.IsNullOrWhiteSpace(data.project.project_name))
            {
                result.msg = Tip.TIP_12;
                return result;
            }
            data.project.project_name = data.project.project_name.Trim();
            if (!VerifyCommon.ProjectNameLength(data.project.project_name))
            {
                result.msg = Tip.TIP_19;
                return result;
            }
            if (string.IsNullOrWhiteSpace(data.project.code_souce_tool) || !int.TryParse(data.project.code_souce_tool, out int code_mode))
            {
                result.msg = Tip.TIP_42;
                return result;
            }
            if (code_mode != (int)ECodeMode.GIT)
            {
                result.msg = Tip.TIP_43;
                return result;
            }
            data.project.code_souce_tool = data.project.code_souce_tool.Trim();
            if (string.IsNullOrWhiteSpace(data.project.code_get_cmd))
            {
                result.msg = Tip.TIP_44;
                return result;
            }
            data.project.code_get_cmd = data.project.code_get_cmd.Trim();
            if (!VerifyCommon.CodeGetCommandLength(data.project.code_get_cmd))
            {
                result.msg = Tip.TIP_45;
                return result;
            }
            if (!string.IsNullOrWhiteSpace(data.project.project_path))
            {
                data.project.project_path = data.project.project_path.Trim();
                if (!VerifyCommon.PublishProjectPathLength(data.project.project_path))
                {
                    result.msg = Tip.TIP_46;
                    return result;
                }
            }
            //发布信息验证
            if (data.publish == null)
            {
                result.msg = Tip.TIP_13;
                return result;
            }
            if (!string.IsNullOrWhiteSpace(data.publish.build_before_command))
            {
                data.publish.build_before_command = data.publish.build_before_command.Trim();
            }
            if (!string.IsNullOrWhiteSpace(data.publish.build_after_command))
            {
                data.publish.build_after_command = data.publish.build_after_command.Trim();
            }
            if (string.IsNullOrWhiteSpace(data.publish.build_command))
            {
                result.msg = Tip.TIP_47;
                return result;
            }
            data.publish.build_command = data.publish.build_command.Trim();
            if (!string.IsNullOrWhiteSpace(data.publish.publish_before_command))
            {
                data.publish.publish_before_command = data.publish.publish_before_command.Trim();
            }
            if (!string.IsNullOrWhiteSpace(data.publish.publish_after_command))
            {
                data.publish.publish_after_command = data.publish.publish_after_command.Trim();
            }
            result.result = true;
            return result;
        }
        public async Task<Result> AddProject(In<AddProjectIn> inData)
        {
            Result result = await VerifyAddProject(inData.data);
            if (!result.result)
            {
                return result;
            }
            result.result = false;
            SQLiteHelper db = new SQLiteHelper();
            try
            {
                await db.BeginTransactionAsync();
                string guid = MakeCommon.MakeGUID("N");
                int id = await ProjectDao.InsertAsync(db, inData.data, guid);
                if (id <= 0)
                {
                    await db.RollbackAsync();
                    result.msg = Tip.TIP_20;
                    return result;
                }

                bool add_flow_flag = await FlowProjectDao.Insert(db, inData.data, guid);
                if (!add_flow_flag)
                {
                    await db.RollbackAsync();
                    result.msg = Tip.TIP_20;
                    return result;
                }

                bool add_publish_flag = await PublishDao.Insert(db, inData.data, guid);
                if (!add_publish_flag)
                {
                    await db.RollbackAsync();
                    result.msg = Tip.TIP_20;
                    return result;
                }
                await db.CommitAsync();
                result.msg = Tip.TIP_21;
                result.result = true;
            }
            catch (Exception e)
            {
                await db.RollbackAsync();
                result.msg = Tip.TIP_20;
            }
            return result;
        }
    }
}
