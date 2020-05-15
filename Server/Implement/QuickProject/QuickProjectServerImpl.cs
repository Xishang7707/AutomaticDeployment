using Common;
using Common.Extend;
using DAO;
using DAO.QuickProject;
using Model;
using Model.Dao.QuickProject;
using Model.Db;
using Model.Db.Enum;
using Model.In;
using Model.In.PublishFlow;
using Model.In.QuickProject;
using Model.Out;
using Model.Out.QuickProject;
using Server.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Implement.QuickProject
{
    /// <summary>
    /// 快速发布
    /// </summary>
    internal class QuickProjectServerImpl : IQuickProjectServer
    {
        /// <summary>
        /// 验证添加数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private Result VerifyAddQuickProject(AddQuickProjectIn data)
        {
            Result result = new Result();
            if (data == null)
            {
                result.msg = Tip.TIP_1;
                return result;
            }
            //验证服务器信息
            if (data.server == null)
            {
                result.msg = Tip.TIP_4;
                return result;
            }
            if (!VerifyCommon.OSPlatform(data.server.server_platform))
            {
                result.msg = Tip.TIP_22;
                return result;
            }
            if (string.IsNullOrWhiteSpace(data.server.server_ip))
            {
                result.msg = Tip.TIP_5;
                return result;
            }
            if (!VerifyCommon.IP(data.server.server_ip))
            {
                result.msg = Tip.TIP_6;
                return result;
            }
            if (string.IsNullOrWhiteSpace(data.server.server_port))
            {
                result.msg = Tip.TIP_7;
                return result;
            }
            if (!VerifyCommon.Port(data.server.server_port))
            {
                result.msg = Tip.TIP_8;
                return result;
            }
            if (string.IsNullOrWhiteSpace(data.server.server_account))
            {
                result.msg = Tip.TIP_9;
                return result;
            }
            if (!VerifyCommon.ServiceAccountLength(data.server.server_account))
            {
                result.msg = Tip.TIP_17;
                return result;
            }
            if (string.IsNullOrWhiteSpace(data.server.server_password))
            {
                result.msg = Tip.TIP_10;
                return result;
            }
            if (!VerifyCommon.ServicePasswordLength(data.server.server_password))
            {
                result.msg = Tip.TIP_18;
                return result;
            }
            //验证项目信息
            if (data.project == null)
            {
                result.msg = Tip.TIP_11;
                return result;
            }
            if (string.IsNullOrWhiteSpace(data.project.project_name))
            {
                result.msg = Tip.TIP_12;
                return result;
            }
            if (!VerifyCommon.ProjectNameLength(data.project.project_name))
            {
                result.msg = Tip.TIP_19;
                return result;
            }
            //发布信息验证
            if (data.publish == null)
            {
                result.msg = Tip.TIP_13;
                return result;
            }
            if (string.IsNullOrWhiteSpace(data.publish.publish_path))
            {
                result.msg = Tip.TIP_14;
                return result;
            }
            if (!VerifyCommon.PublishProjectPathLength(data.publish.publish_path))
            {
                result.msg = Tip.TIP_13;
                return result;
            }
            result.result = true;
            return result;
        }

        public async Task<Result> AddQuickProjectAsync(In<AddQuickProjectIn> inData)
        {
            Result result = VerifyAddQuickProject(inData.data);
            if (!result.result)
            {
                return result;
            }
            result.result = false;

            SQLiteHelper dbHelper = new SQLiteHelper();
            try
            {
                await dbHelper.BeginTransactionAsync();

                AddProjectDaoResult addProjectDaoResult = await ProjectDao.AddProjectModelAsync(dbHelper, inData.data.project);
                if (!addProjectDaoResult.result)
                {
                    await dbHelper.RollbackAsync();
                    result.msg = Tip.TIP_20;
                    return result;
                }
                result.result = await QuickProjectDao.AddQuickProjectModelAsync(dbHelper, inData.data, addProjectDaoResult.proj_guid);
                if (!result.result)
                {
                    await dbHelper.RollbackAsync();
                    result.msg = Tip.TIP_20;
                    return result;
                }

                await dbHelper.CommitAsync();
                dbHelper.Close();
                result.msg = Tip.TIP_21;
                return result;
            }
            catch (Exception e)
            {
                await dbHelper.RollbackAsync();
                dbHelper.Close();
                result.msg = Tip.TIP_20;
                return result;
            }
        }

        public async Task<Result> QuickProjectListAsync(In data)
        {
            List<ProjectInfoResult> resultList = new List<ProjectInfoResult>();
            Result<List<ProjectInfoResult>> result = new Result<List<ProjectInfoResult>> { result = true, data = resultList };

            SQLiteHelper dbHelper = new SQLiteHelper();

            List<t_project> projList = await ProjectDao.GetProjectList(dbHelper);
            List<t_quick_project> quickList = new List<t_quick_project>();
            if (projList.Count > 0)
            {
                quickList = await QuickProjectDao.GetProjectList(dbHelper, projList.Select(s => s.proj_guid).ToArray());
            }

            foreach (var item in projList)
            {
                t_quick_project quickItem = quickList.First(f => f.proj_guid == item.proj_guid);

                resultList.Add(new ProjectInfoResult
                {
                    project = new ProjectResult
                    {
                        project_uid = item.proj_guid,
                        project_name = item.name,
                        project_remark = item.remark,

                    },
                    server = new ServerResult
                    {
                        server_account = quickItem.conn_user,
                        server_ip = quickItem.conn_ip,
                        server_connect_mode = ((EOSConnectMode)quickItem.conn_mode).GetDesc()
                    },
                    publish = new PublishResult
                    {
                        publish_path = quickItem.publish_path,
                        publish_before_command = quickItem.publish_before_cmd,
                        publish_after_command = quickItem.publish_after_cmd,
                        publish_time = !GetCommon.GetCastTime(item.last_publish_time, out DateTime publishVal) ? "暂未发布" : publishVal.ToString("yyyy-MM-dd HH:mm:dd"),
                        publish_status = !GetCommon.GetCastTime(item.last_publish_time, out DateTime publishVal2) ? "暂未发布" : ((EPublishStatus)item.last_publish_status).GetDesc()
                    }
                });
            }

            return result;
        }

        /// <summary>
        /// 验证发布数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private async Task<Result> VerifyPublish(PublishQuickProject data)
        {
            Result result = new Result();
            if (data == null)
            {
                result.msg = Tip.TIP_1;
                return result;
            }
            SQLiteHelper dbHelper = new SQLiteHelper();
            if (string.IsNullOrWhiteSpace(data.project_uid) || !await ProjectDao.IsExist(dbHelper, data.project_uid))
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

        public async Task<Result> Publish(In<PublishQuickProject> inData)
        {
            Result result = await VerifyPublish(inData.data);
            IPublishFlowServer publishFlowServer = ServerFactory.Get<IPublishFlowServer>();
            result = await publishFlowServer.PublishAsync(inData);
            if (!result.result)
            {
                return result;
            }

            //发布数据添加完成 通知服务发布
            IAutoPublishServer autoPublishServer = ServerFactory.Get<IAutoPublishServer>();
            autoPublishServer.Notice();
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

        public async Task<Result> GetProjectAsync(In<string> inData)
        {
            Result result = VerifyGetProject(inData.data);
            if (!result.result)
            {
                return result;
            }

            result.result = false;

            SQLiteHelper dbHelper = new SQLiteHelper();

            t_project proj = await ProjectDao.GetProject(dbHelper, inData.data);
            if (proj == null)
            {
                result.msg = Tip.TIP_24;
                return result;
            }

            t_quick_project quickProj = await QuickProjectDao.GetProject(dbHelper, inData.data);
            ProjectInfoResult info = new ProjectInfoResult
            {
                project = new ProjectResult
                {
                    project_uid = proj.proj_guid,
                    project_name = proj.name,
                    project_remark = proj.remark
                },
                server = new ServerResult
                {
                    server_account = quickProj.conn_user,
                    server_ip = quickProj.conn_ip,
                    server_connect_mode = ((EOSConnectMode)quickProj.conn_mode).GetDesc(),
                    server_password = string.IsNullOrWhiteSpace(quickProj.conn_password) ? "" : GetCommon.GetHidePassword(),
                    server_port = quickProj.conn_port.ToString()
                },
                publish = new PublishResult
                {
                    publish_path = quickProj.publish_path,
                    publish_before_command = quickProj.publish_before_cmd,
                    publish_after_command = quickProj.publish_after_cmd,
                    publish_time = !GetCommon.GetCastTime(proj.last_publish_time, out DateTime publishVal) ? "暂未发布" : publishVal.ToString("yyyy-MM-dd HH:mm:dd"),
                    publish_status = !GetCommon.GetCastTime(proj.last_publish_time, out _) ? "暂未发布" : ((EPublishStatus)proj.last_publish_status).GetDesc()
                }
            };

            return new Result<ProjectInfoResult>
            {
                data = info,
                result = true
            };
        }

        /// <summary>
        /// 验证编辑数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private async Task<Result> VerifyEditQuickProject(EditQuickProjectIn data)
        {
            Result result = new Result();
            if (data == null)
            {
                result.msg = Tip.TIP_1;
                return result;
            }
            //验证服务器信息
            if (data.server == null)
            {
                result.msg = Tip.TIP_4;
                return result;
            }
            if (!VerifyCommon.OSPlatform(data.server.server_platform))
            {
                result.msg = Tip.TIP_22;
                return result;
            }
            if (string.IsNullOrWhiteSpace(data.server.server_ip))
            {
                result.msg = Tip.TIP_5;
                return result;
            }
            if (!VerifyCommon.IP(data.server.server_ip))
            {
                result.msg = Tip.TIP_6;
                return result;
            }
            if (string.IsNullOrWhiteSpace(data.server.server_port))
            {
                result.msg = Tip.TIP_7;
                return result;
            }
            if (!VerifyCommon.Port(data.server.server_port))
            {
                result.msg = Tip.TIP_8;
                return result;
            }
            if (string.IsNullOrWhiteSpace(data.server.server_account))
            {
                result.msg = Tip.TIP_9;
                return result;
            }
            if (!VerifyCommon.ServiceAccountLength(data.server.server_account))
            {
                result.msg = Tip.TIP_17;
                return result;
            }
            if (string.IsNullOrWhiteSpace(data.server.server_password))
            {
                result.msg = Tip.TIP_10;
                return result;
            }
            if (!VerifyCommon.ServiceAccountLength(data.server.server_password))
            {
                result.msg = Tip.TIP_18;
                return result;
            }
            //验证项目信息
            if (data.project == null)
            {
                result.msg = Tip.TIP_11;
                return result;
            }
            if (string.IsNullOrWhiteSpace(data.project.project_name))
            {
                result.msg = Tip.TIP_12;
                return result;
            }
            if (!VerifyCommon.ProjectNameLength(data.project.project_name))
            {
                result.msg = Tip.TIP_19;
                return result;
            }
            //发布信息验证
            if (data.publish == null)
            {
                result.msg = Tip.TIP_13;
                return result;
            }
            if (string.IsNullOrWhiteSpace(data.publish.publish_path))
            {
                result.msg = Tip.TIP_14;
                return result;
            }
            if (!VerifyCommon.PublishProjectPathLength(data.publish.publish_path))
            {
                result.msg = Tip.TIP_13;
                return result;
            }
            //验证项目是否存在
            if (string.IsNullOrWhiteSpace(data.project?.project_guid?.Trim()))
            {
                result.msg = Tip.TIP_24;
                return result;
            }
            data.project.project_guid = data.project.project_guid.Trim();
            SQLiteHelper db = new SQLiteHelper();
            bool project_exist_flag = await ProjectDao.IsExist(db, data.project.project_guid);
            db.Close();
            if (!project_exist_flag)
            {
                result.msg = Tip.TIP_24;
                return result;
            }
            result.result = true;
            return result;
        }

        public async Task<Result> EditProject(In<EditQuickProjectIn> inData)
        {
            Result result = await VerifyEditQuickProject(inData.data);
            if (!result.result)
            {
                return result;
            }

            SQLiteHelper dbHelper = new SQLiteHelper();
            try
            {
                await dbHelper.BeginTransactionAsync();
                result.result = await ProjectDao.EditProject(dbHelper, inData.data.project);
                if (!result.result)
                {
                    await dbHelper.RollbackAsync();
                    result.msg = Tip.TIP_35;
                    return result;
                }

                result.result = await QuickProjectDao.EditQuickProjectModelAsync(dbHelper, inData.data);
                if (!result.result)
                {
                    await dbHelper.RollbackAsync();
                    result.msg = Tip.TIP_35;
                    return result;
                }

                await dbHelper.CommitAsync();
                dbHelper.Close();
                result.msg = Tip.TIP_34;
                return result;
            }
            catch (Exception e)
            {
                await dbHelper.RollbackAsync();
                dbHelper.Close();
                result.msg = Tip.TIP_35;
                return result;
            }
        }
    }
}
