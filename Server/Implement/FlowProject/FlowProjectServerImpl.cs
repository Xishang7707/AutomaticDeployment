using Common;
using Common.Extend;
using DAO;
using DAO.FlowProject;
using Model;
using Model.Db;
using Model.Db.Enum;
using Model.In;
using Model.In.FlowProject;
using Model.In.PublishFlow;
using Model.Out;
using Model.Out.FlowProject;
using Server.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity.Hierarchy;
using System.Linq;
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

        public async Task<Result> GetProjectList()
        {
            List<ProjectInfoResult> resultList = new List<ProjectInfoResult>();
            Result<List<ProjectInfoResult>> result = new Result<List<ProjectInfoResult>> { result = true, data = resultList };

            SQLiteHelper dbHelper = new SQLiteHelper();

            List<t_project> projList = await ProjectDao.GetProjectList(dbHelper);
            List<t_flow_project> flowList = new List<t_flow_project>();
            List<t_publish> publishList = new List<t_publish>();
            List<t_service> serviceList = new List<t_service>();
            if (projList.Count > 0)
            {
                flowList = await FlowProjectDao.GetProjectList(dbHelper, projList.Select(s => s.proj_guid).ToArray());
                publishList = await PublishDao.GetList(dbHelper, projList.Select(s => s.proj_guid).ToArray());
                serviceList = await DAO.FlowProject.ServiceDao.GetServiceList(dbHelper, flowList.Select(s => s.serv_id).ToArray());
            }

            foreach (var item in projList)
            {
                t_flow_project flowItem = flowList.First(f => f.proj_guid == item.proj_guid);
                t_publish publishItem = publishList.First(f => f.proj_guid == item.proj_guid);
                t_service serviceItem = serviceList.First(f => f.id == flowItem.serv_id);

                resultList.Add(new ProjectInfoResult
                {
                    project = new ProjectResult
                    {
                        project_uid = item.proj_guid,
                        project_name = item.name,
                        project_remark = item.remark,
                        code_souce_tool = ((ECodeTools)flowItem.code_source).GetDesc(),
                        project_path = flowItem.proj_path
                    },
                    server = new ServiceResult
                    {
                        name = (string.IsNullOrWhiteSpace(serviceItem.name) ? serviceItem.conn_ip : $"{serviceItem.name}({serviceItem.conn_ip})")
                    },
                    publish = new PublishResult
                    {
                        publish_before_command = publishItem.publish_before_cmd,
                        publish_after_command = publishItem.publish_after_cmd,
                        publish_time = !GetCommon.GetCastTime(item.last_publish_time, out DateTime publishVal) ? "暂未发布" : publishVal.ToString("yyyy-MM-dd HH:mm:dd"),
                        publish_status = !GetCommon.GetCastTime(item.last_publish_time, out DateTime publishVal2) ? "暂未发布" : ((EPublishStatus)item.last_publish_status).GetDesc()
                    }
                });
            }

            return result;
        }

        private async Task<Result> VerifyPublish(PublishFlowProject data)
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
            bool proj_exist_flag = await ProjectDao.IsExist(db, data.project_uid.Trim());
            db.Close();
            if (!proj_exist_flag)
            {
                result.msg = Tip.TIP_24;
                return result;
            }
            result.result = true;
            return result;
        }

        public async Task<Result> Publish(In<PublishFlowProject> inData)
        {
            Result result = await VerifyPublish(inData.data);
            if (!result.result)
            {
                return result;
            }

            result = await ServerFactory.Get<IPublishFlowServer>().PublishAsync(inData);
            if (!result.result)
            {
                return result;
            }

            //发布数据添加完成 通知服务发布
            ServerFactory.Get<IAutoPublishServer>().Notice();
            result.msg = Tip.TIP_27;
            return result;
        }

        /// <summary>
        /// 获取项目验证
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private Result VerifyGetProject(string data)
        {
            Result result = new Result();
            if (string.IsNullOrWhiteSpace(data))
            {
                result.msg = Tip.TIP_24;
                return result;
            }
            result.result = true;
            return result;
        }

        public async Task<Result> GetProject(In<string> inData)
        {
            Result result = VerifyGetProject(inData.data);
            if (!result.result)
            {
                return result;
            }
            result.result = false;

            SQLiteHelper db = new SQLiteHelper();
            t_project proj_model = await ProjectDao.GetProject(db, inData.data);
            if (proj_model == null)
            {
                result.msg = Tip.TIP_24;
                return result;
            }
            t_flow_project flow_model = await FlowProjectDao.GetProject(db, proj_model.proj_guid);
            t_publish publish_model = await PublishDao.GetPublish(db, proj_model.proj_guid);
            t_service service_model = await DAO.FlowProject.ServiceDao.GetService(db, flow_model.serv_id);
            ProjectInfoResult info = new ProjectInfoResult
            {
                project = new ProjectResult
                {
                    project_uid = proj_model.proj_guid,
                    project_name = proj_model.name,
                    project_remark = proj_model.remark,
                    code_souce_tool = ((ECodeTools)flow_model.code_source).GetDesc(),
                    project_path = flow_model.proj_path
                },
                server = new ServiceResult
                {
                    name = (string.IsNullOrWhiteSpace(service_model.name) ? service_model.conn_ip : $"{service_model.name}({service_model.conn_ip})")
                },
                publish = new PublishResult
                {
                    publish_before_command = publish_model.publish_before_cmd,
                    publish_after_command = publish_model.publish_after_cmd,
                    publish_time = !GetCommon.GetCastTime(proj_model.last_publish_time, out DateTime publishVal) ? "暂未发布" : publishVal.ToString("yyyy-MM-dd HH:mm:dd"),
                    publish_status = !GetCommon.GetCastTime(proj_model.last_publish_time, out DateTime publishVal2) ? "暂未发布" : ((EPublishStatus)proj_model.last_publish_status).GetDesc()
                }
            };

            Result<ProjectInfoResult> res = new Result<ProjectInfoResult> { data = info, result = true };
            return res;
        }

        /// <summary>
        /// 获取项目验证
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private Result VerifyGetProjectInfo(string data)
        {
            Result result = new Result();
            if (string.IsNullOrWhiteSpace(data))
            {
                result.msg = Tip.TIP_24;
                return result;
            }
            result.result = true;
            return result;
        }

        public async Task<Result> GetProjectInfo(In<string> inData)
        {
            Result result = VerifyGetProjectInfo(inData.data);
            if (!result.result)
            {
                return result;
            }
            result.result = false;

            SQLiteHelper db = new SQLiteHelper();
            t_project proj_model = await ProjectDao.GetProject(db, inData.data);
            if (proj_model == null)
            {
                result.msg = Tip.TIP_24;
                return result;
            }
            t_flow_project flow_model = await FlowProjectDao.GetProject(db, proj_model.proj_guid);
            t_publish publish_model = await PublishDao.GetPublish(db, proj_model.proj_guid);

            EditProjectInfoResult info = new EditProjectInfoResult
            {
                project = new FlowProjectIn
                {
                    code_get_cmd = flow_model.code_cmd,
                    code_souce_tool = flow_model.code_source.ToString(),
                    project_guid = flow_model.proj_guid,
                    project_name = proj_model.name,
                    project_path = flow_model.proj_path,
                    project_remark = proj_model.remark,
                    service_id = flow_model.serv_id.ToString(),
                },
                publish = new FlowPublishIn
                {
                    build_after_command = publish_model.build_after_cmd,
                    publish_after_command = publish_model.publish_after_cmd,
                    build_before_command = publish_model.build_before_cmd,
                    build_command = publish_model.build_cmd,
                    publish_before_command = publish_model.publish_before_cmd,
                    publish_file_path = publish_model.publish_file_path
                }
            };

            Result<EditProjectInfoResult> res = new Result<EditProjectInfoResult> { data = info, result = true };
            return res;
        }

        /// <summary>
        /// 验证修改项目数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private async Task<Result> VerifyEditProject(EditProjectIn data)
        {
            Result result = new Result();
            if (data == null)
            {
                result.msg = Tip.TIP_1;
                return result;
            }
            result.result = false;
            //验证项目信息
            if (data.project == null)
            {
                result.msg = Tip.TIP_11;
                return result;
            }
            if (string.IsNullOrWhiteSpace(data.project_uid))
            {
                result.msg = Tip.TIP_24;
                return result;
            }
            data.project_uid = data.project_uid.Trim();
            SQLiteHelper db = new SQLiteHelper();
            bool proj_exist_flag = await ProjectDao.IsExist(db, data.project_uid);
            if (!proj_exist_flag)
            {
                db.Close();
                result.msg = Tip.TIP_24;
                return result;
            }
            if (string.IsNullOrWhiteSpace(data.project.service_id) || !int.TryParse(data.project.service_id, out int service_id))
            {
                db.Close();
                result.msg = Tip.TIP_40;
                return result;
            }
            data.project.service_id = data.project.service_id.Trim();

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
        public async Task<Result> EditProject(In<EditProjectIn> inData)
        {
            Result result = await VerifyEditProject(inData.data);
            if (!result.result)
            {
                return result;
            }
            result.result = false;
            SQLiteHelper db = new SQLiteHelper();
            try
            {
                await db.BeginTransactionAsync();
                bool update_proj_flag = await ProjectDao.UpdateAsync(db, inData.data);
                if (!update_proj_flag)
                {
                    await db.RollbackAsync();
                    result.msg = Tip.TIP_35;
                    return result;
                }

                bool update_flow_flag = await FlowProjectDao.Update(db, inData.data);
                if (!update_flow_flag)
                {
                    await db.RollbackAsync();
                    result.msg = Tip.TIP_35;
                    return result;
                }

                bool update_publish_flag = await PublishDao.Update(db, inData.data);
                if (!update_publish_flag)
                {
                    await db.RollbackAsync();
                    result.msg = Tip.TIP_35;
                    return result;
                }
                await db.CommitAsync();
                result.msg = Tip.TIP_34;
                result.result = true;
            }
            catch (Exception e)
            {
                await db.RollbackAsync();
                result.msg = Tip.TIP_35;
            }
            return result;
        }
    }
}
