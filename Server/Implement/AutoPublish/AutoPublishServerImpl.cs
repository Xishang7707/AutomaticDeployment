using Common;
using DAO;
using DAO.AutoPublish;
using Model;
using Model.Db;
using Model.Db.Enum;
using Model.Extend;
using Model.In.PublishFlow;
using Model.In.SqlManage;
using Model.Out;
using Model.Ssh;
using Newtonsoft.Json;
using Server.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server.Implement.AutoPublish
{
    internal class AutoPublishServerImpl : IAutoPublishServer
    {
        /// <summary>
        /// 启动标识
        /// </summary>
        private bool startFlag = false;

        /// <summary>
        /// 数据库连接工具
        /// </summary>
        private SQLiteHelper dbHelper { get; }

        /// <summary>
        /// 只有一个执行实例
        /// </summary>
        private static AutoPublishServerImpl Instance { get; set; }

        /// <summary>
        /// 线程阻塞
        /// </summary>
        private ManualResetEvent manualResetEvent { get; }

        /// <summary>
        /// 日志服务
        /// </summary>
        private IPublishLogServer publishLogServer { get; }

        /// <summary>
        /// 本地操作
        /// </summary>
        private IOSManageServer osLocal;

        public AutoPublishServerImpl()
        {
            if (Instance != null)
            {
                return;
            }
            Instance = this;
            dbHelper = new SQLiteHelper();
            manualResetEvent = new ManualResetEvent(true);
            publishLogServer = ServerFactory.GetPublisLog();
            osLocal = ServerFactory.GetOSPlatform(ServerConfig.OSPlatform);
            osLocal.Connect(new Model.In.OSManage.UserConnectIn
            {
                host = "127.0.0.1",
                password = ServerConfig.OSPassword,
                user = ServerConfig.OSUser,
                port = ServerConfig.OSPort
            });
        }

        public void Notice()
        {
            lock (Instance.manualResetEvent)
            {
                Instance.manualResetEvent.Set();
            }
        }

        /// <summary>
        /// 暂停
        /// </summary>
        private void Pause()
        {
            lock (Instance.manualResetEvent)
            {
                Instance.manualResetEvent.Reset();
            }
        }

        /// <summary>
        /// 开启工作
        /// </summary>
        private async Task StartWorkAsync()
        {
            if (Instance.startFlag)
                return;
            await StartWorkInternal();
            Instance.startFlag = true;
        }

        /// <summary>
        /// 执行任务
        /// </summary>
        /// <returns></returns>
        private async Task StartWorkInternal()
        {
            TimeSpan taskTimeOut = TimeSpan.FromMinutes(15);//15分钟后取消
            await Task.Run(async () =>
            {
                while (true)
                {
                    manualResetEvent.WaitOne();

                    t_publish_flow publishFlow = await PublishFlowDao.GetProject(dbHelper);
                    if (publishFlow == null)
                    {
                        Pause();
                        continue;
                    }
                    CancellationTokenSource cancelToken = new CancellationTokenSource(taskTimeOut);//任务超时取消
                    Timer timer = TaskTimeoutCancel(taskTimeOut, async () => await WorkTimerOutAct(publishFlow.proj_guid, publishFlow.id));//任务超时
                    await Task.Run(async () =>
                    {
                        try
                        {
                            //进行中
                            Result result = new Result();

                            switch ((EProjectType)publishFlow.proj_type)
                            {
                                case EProjectType.Quick:
                                    result = await DoWorkFile(publishFlow);
                                    break;
                                case EProjectType.Flow:
                                    result = await DoWorkBuild(publishFlow);
                                    break;
                                default:
                                    break;
                            }

                            //发布结果
                            if (result.result)
                            {
                                //发布成功
                                await PublishSuccess(publishFlow.proj_guid, publishFlow.id);
                            }
                            else
                            {
                                //发布失败
                                await PublishFailed(publishFlow.proj_guid, publishFlow.id);
                            }

                            //注销超时定时器
                            timer?.Dispose();
                        }
                        catch (Exception ex)
                        {
                            timer?.Dispose();
                            publishLogServer.LogAsync(publishFlow.proj_guid, publishFlow.id, ex.Message);
                            //发布失败
                            await PublishFailed(publishFlow.proj_guid, publishFlow.id);
                        }
                    }, cancelToken.Token);
                }
            });
        }

        /// <summary>
        /// 工作 --文件发布
        /// </summary>
        /// <param name="model"></param>
        private async Task<Result> DoWorkFile(t_publish_flow model)
        {
            WorkInfo<t_project, List<FileModePublish>> info = new WorkInfo<t_project, List<FileModePublish>>(model);
            Result result = await DoWorkBeforeExec(info);
            if (!result.result)
            {
                return result;
            }
            result = DoWorkFileFlow(info, ConnectService, ExecBeforeCommand, PublishToService, ExecUnZip, ConnectSQL, ExecSql, ExecAfterCommand, DoWorkAfterExec);

            return result;
        }

        /// <summary>
        /// 工作流 -回调列表形式进行
        /// </summary>
        /// <param name="info">发布信息</param>
        /// <param name="flows">步骤流</param>
        /// <returns></returns>
        private Result DoWorkFileFlow(WorkInfo<t_project, List<FileModePublish>> info, params Func<WorkInfo<t_project, List<FileModePublish>>, Result>[] flows)
        {
            Result result = new Result { result = true, msg = Tip.TIP_16 };
            foreach (var item in flows)
            {
                result = item(info);
                publishLogServer.LogAsync(info.proj_info.proj_guid, info.flow_id, result.msg);
                if (!result.result)
                {
                    return result;
                }
            }
            return result;
        }

        /// <summary>
        /// 发布前
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private async Task<Result> DoWorkBeforeExec(WorkInfo<t_project, List<FileModePublish>> info)
        {
            Result result = new Result
            {
                result = await PublishStart(info.proj_info.proj_guid, info.flow_id)
            };
            if (!result.result)
            {
                result.msg = Tip.TIP_26;
            }
            publishLogServer.SendToPublishResultAsync(info.proj_info.proj_guid, info.flow_id, EPublishStatus.Progress);
            return result;
        }

        /// <summary>
        /// 发布前
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private async Task<Result> DoWorkBeforeExec(WorkInfo<t_flow_project> info)
        {
            Result result = new Result
            {
                result = await PublishStart(info.proj_info.proj_guid, info.flow_id)
            };
            if (!result.result)
            {
                result.msg = Tip.TIP_26;
            }
            publishLogServer.SendToPublishResultAsync(info.proj_info.proj_guid, info.flow_id, EPublishStatus.Progress);
            return result;
        }

        /// <summary>
        /// 发布后
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private Result DoWorkAfterExec(WorkInfo<t_project, List<FileModePublish>> info)
        {
            info.osManagerServer.Close();
            info.sqlManageServer?.Close();
            Result result = new Result
            {
                result = true,
                msg = Tip.TIP_16
            };
            return result;
        }

        /// <summary>
        /// 发布后
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private Result DoWorkAfterExec(WorkInfo<t_flow_project> info)
        {
            info.osManagerServer.Close();
            info.sqlManageServer?.Close();
            Result result = new Result
            {
                result = true,
                msg = Tip.TIP_16
            };
            return result;
        }

        /// <summary>
        /// 任务超时取消
        /// </summary>
        /// <param name="span">超时时间</param>
        /// <param name="act">取消执行回调</param>
        /// <returns></returns>
        private Timer TaskTimeoutCancel(TimeSpan span, Action act = null)
        {
            Timer timer = new Timer(o =>
            {
                act?.Invoke();
            }, null, (int)span.TotalMilliseconds, 0);
            return timer;
        }

        /// <summary>
        /// 超时任务执行
        /// </summary>
        /// <param name="proj_guid"></param>
        /// <param name="flow_id"></param>
        /// <returns></returns>
        private async Task WorkTimerOutAct(string proj_guid, int flow_id)
        {
            await PublishFailed(proj_guid, flow_id);
        }

        /// <summary>
        /// 发布失败
        /// </summary>
        /// <param name="proj_guid">项目guid</param>
        /// <param name="flow_id">发布id</param>
        /// <returns></returns>
        private async Task PublishFailed(string proj_guid, int flow_id)
        {
            SQLiteHelper dbHelper = new SQLiteHelper();
            try
            {
                await dbHelper.BeginTransactionAsync();
                bool flag = await ProjectDao.SetPublishStatus(dbHelper, proj_guid, EPublishStatus.Failed);
                if (!flag)
                {
                    await dbHelper.RollbackAsync();
                    return;
                }
                flag = await PublishFlowDao.SetPublishFailed(dbHelper, flow_id);
                if (!flag)
                {
                    await dbHelper.RollbackAsync();
                    return;
                }
                await dbHelper.CommitAsync();
            }
            catch (Exception e)
            {
                await dbHelper.RollbackAsync();
                dbHelper.Close();
            }
            dbHelper.Close();
            publishLogServer.SendToPublishResultAsync(proj_guid, flow_id, EPublishStatus.Failed);
        }

        /// <summary>
        /// 发布成功
        /// </summary>
        /// <param name="proj_guid">项目guid</param>
        /// <param name="flow_id">发布id</param>
        /// <returns></returns>
        private async Task PublishSuccess(string proj_guid, int flow_id)
        {
            SQLiteHelper dbHelper = new SQLiteHelper();
            try
            {
                await dbHelper.BeginTransactionAsync();
                bool flag = await ProjectDao.SetPublishStatus(dbHelper, proj_guid, EPublishStatus.Success);
                if (!flag)
                {
                    await dbHelper.RollbackAsync();
                    return;
                }
                flag = await PublishFlowDao.SetPublishSuccess(dbHelper, flow_id);
                if (!flag)
                {
                    await dbHelper.RollbackAsync();
                    return;
                }
                await dbHelper.CommitAsync();
            }
            catch (Exception e)
            {
                await dbHelper.RollbackAsync();
                dbHelper.Close();
            }
            dbHelper.Close();
            publishLogServer.SendToPublishResultAsync(proj_guid, flow_id, EPublishStatus.Success);
        }

        /// <summary>
        /// 发布开始
        /// </summary>
        /// <param name="proj_guid">项目guid</param>
        /// <param name="flow_id">发布id</param>
        /// <returns></returns>
        private async Task<bool> PublishStart(string proj_guid, int flow_id)
        {
            publishLogServer.LogAsync(new Model.In.PublishLog.LogInfo { proj_guid = proj_guid, publish_id = flow_id, publish_info = "开始发布" });
            SQLiteHelper dbHelper = new SQLiteHelper();
            try
            {
                await dbHelper.BeginTransactionAsync();
                bool flag = await ProjectDao.SetPublishProgress(dbHelper, proj_guid);
                if (!flag)
                {
                    await dbHelper.RollbackAsync();
                    return flag;
                }
                flag = await PublishFlowDao.SetPublishProgress(dbHelper, flow_id);
                if (!flag)
                {
                    await dbHelper.RollbackAsync();
                    return flag;
                }
                await dbHelper.CommitAsync();
            }
            catch (Exception e)
            {
                await dbHelper.RollbackAsync();
                dbHelper.Close();
                return false;
            }
            dbHelper.Close();
            return true;
        }

        /// <summary>
        /// 工作任务信息
        /// </summary>
        class WorkInfo<T_Proj> where T_Proj : class, new()
        {
            /// <summary>
            /// 项目信息
            /// </summary>
            public T_Proj proj_info { get; set; }

            /// <summary>
            /// 服务器信息
            /// </summary>
            public t_service service_info { get; set; }

            /// <summary>
            /// 发布信息
            /// </summary>
            public t_publish publish_info { get; set; }

            /// <summary>
            /// 发布流标识
            /// </summary>
            public int flow_id { get; set; }

            /// <summary>
            /// 操作系统服务
            /// </summary>
            public IOSManageServer osManagerServer { get; set; }

            /// <summary>
            /// 数据库服务
            /// </summary>
            public ISqlManageServer sqlManageServer { get; set; }

            public WorkInfo() { }
            public WorkInfo(t_publish_flow model)
            {
                flow_id = model.id;
                proj_info = JsonConvert.DeserializeObject<T_Proj>(model.proj_info);
                service_info = JsonConvert.DeserializeObject<t_service>(model.server_info);
                publish_info = JsonConvert.DeserializeObject<t_publish>(model.publish_info);
            }
        }

        /// <summary>
        /// 工作任务信息
        /// </summary>
        class WorkInfo<T_Proj, T_Exten> : WorkInfo<T_Proj>
            where T_Proj : class, new()
            where T_Exten : class, new()
        {
            /// <summary>
            /// 扩展信息
            /// </summary>
            public T_Exten extend_info { get; set; }

            public WorkInfo() { }
            public WorkInfo(t_publish_flow model) : base(model) => extend_info = (string.IsNullOrWhiteSpace(model.extern_info) ? null : JsonConvert.DeserializeObject<T_Exten>(model.extern_info));
        }

        /// <summary>
        /// 发布第一步 连接服务器
        /// </summary>
        /// <param name="service">服务器信息</param>
        /// <returns></returns>
        private Result ConnectService(WorkInfo<t_project, List<FileModePublish>> info)
        {
            string decPassword = ConcealCommon.DecryptDES(info.service_info.conn_password);
            info.osManagerServer = ServerFactory.GetOSPlatform((EOSPlatform)info.service_info.platform_type);
            Result result = info.osManagerServer.Connect(new Model.In.OSManage.UserConnectIn
            {
                host = info.service_info.conn_ip,
                port = info.service_info.conn_port,
                user = info.service_info.conn_user,
                password = decPassword
            });
            if (!result.result)
            {
                return result;
            }

            //切换到工作目录
            if (!string.IsNullOrWhiteSpace(info.service_info.work_spacepath))
            {
                result = info.osManagerServer.ChangeWorkspace(info.service_info.work_spacepath);
                if (!result.result)
                {
                    return result;
                }
            }

            return result;
        }

        /// <summary>
        /// 发布第一步 连接服务器
        /// </summary>
        /// <param name="service">服务器信息</param>
        /// <returns></returns>
        private Result ConnectService(WorkInfo<t_flow_project> info)
        {
            publishLogServer.LogAsync(info.proj_info.proj_guid, info.flow_id, $"连接服务器");
            string decPassword = ConcealCommon.DecryptDES(info.service_info.conn_password);
            info.osManagerServer = ServerFactory.GetOSPlatform((EOSPlatform)info.service_info.platform_type);
            Result result = info.osManagerServer.Connect(new Model.In.OSManage.UserConnectIn
            {
                host = info.service_info.conn_ip,
                port = info.service_info.conn_port,
                user = info.service_info.conn_user,
                password = decPassword
            });
            if (!result.result)
            {
                publishLogServer.LogAsync(info.proj_info.proj_guid, info.flow_id, $"连接服务器失败");
                return result;
            }

            //切换到工作目录
            if (!string.IsNullOrWhiteSpace(info.service_info.work_spacepath))
            {
                result = info.osManagerServer.ChangeWorkspace(info.service_info.work_spacepath);
                if (!result.result)
                {
                    return result;
                }
            }

            return result;
        }

        /// <summary>
        /// 发布前命令 --连接
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private Result ExecBeforeCommand(WorkInfo<t_project, List<FileModePublish>> info)
        {
            Result result = new Result();
            if (string.IsNullOrWhiteSpace(info.publish_info.publish_before_cmd))
            {
                result.result = true;
                return result;
            }

            ExecResult execResult = info.osManagerServer.Exec(info.publish_info.publish_before_cmd).Cast<ExecResult>();
            result.msg = execResult.msg;
            if (!execResult.result)
            {
                return result;
            }
            result.result = true;
            return result;
        }

        /// <summary>
        /// 发布前命令 --连接
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private Result ExecBeforeCommand(WorkInfo<t_flow_project> info)
        {
            Result result = new Result();
            if (string.IsNullOrWhiteSpace(info.publish_info.publish_before_cmd))
            {
                result.result = true;
                return result;
            }
            publishLogServer.LogAsync(info.proj_info.proj_guid, info.flow_id, $"发布前命令执行：{info.publish_info.publish_before_cmd}");
            ExecResult execResult = info.osManagerServer.Exec(info.publish_info.publish_before_cmd).Cast<ExecResult>();
            result.msg = execResult.msg;
            if (!execResult.result)
            {
                return result;
            }
            result.result = true;
            return result;
        }

        /// <summary>
        /// 发布文件到服务器
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private Result PublishToService(WorkInfo<t_project, List<FileModePublish>> info)
        {
            Result result = new Result();

            if (info.extend_info == null)
            {
                result.msg = Tip.TIP_12;
                return result;
            }
            List<FileModePublish> list = info.extend_info.Where(w => VerifyCommon.FileType(w.file_id, EFileType.ZIP)).ToList();
            if (list.Count == 0)
            {
                result.msg = Tip.TIP_12;
                return result;
            }
            foreach (var item in list)
            {
                string filePath = ServerCommon.GetPublishUploadPath(info.proj_info.proj_guid, item.file_id);

                result = info.osManagerServer.Upload(filePath, info.service_info.work_spacepath, item.file_id);
                if (!result.result)
                {
                    return result;
                }
            }

            result.result = true;
            return result;
        }

        /// <summary>
        /// 发布文件到服务器
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private Result PublishToService(WorkInfo<t_flow_project> info)
        {
            publishLogServer.LogAsync(info.proj_info.proj_guid, info.flow_id, $"正在上传文件");
            string path = Path.GetFullPath(ServerCommon.GetBuildPath(info.proj_info.proj_guid));
            Result result = info.osManagerServer.Upload($"{path}/{info.proj_info.proj_guid}.zip", $"{info.service_info.work_spacepath}/{info.proj_info.proj_path}", $"{info.proj_info.proj_guid}.zip");
            if (!result.result)
            {
                publishLogServer.LogAsync(info.proj_info.proj_guid, info.flow_id, $"上传文件失败：{info.service_info.work_spacepath}/{info.proj_info.proj_path}");
                return result;
            }
            result.result = true;
            return result;
        }

        /// <summary>
        /// 发布后命令 --连接
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private Result ExecAfterCommand(WorkInfo<t_project, List<FileModePublish>> info)
        {
            Result result = new Result();
            if (string.IsNullOrWhiteSpace(info.publish_info.publish_after_cmd))
            {
                result.result = true;
                return result;
            }

            ExecResult execResult = info.osManagerServer.Exec(info.publish_info.publish_after_cmd).Cast<ExecResult>();
            result.msg = execResult.msg;
            if (!execResult.result)
            {
                result.msg = execResult.msg;
                return result;
            }

            result.result = true;
            return result;
        }

        /// <summary>
        /// 发布后命令 --连接
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private Result ExecAfterCommand(WorkInfo<t_flow_project> info)
        {
            Result result = new Result();
            if (string.IsNullOrWhiteSpace(info.publish_info.publish_after_cmd))
            {
                result.result = true;
                return result;
            }
            publishLogServer.LogAsync(info.proj_info.proj_guid, info.flow_id, $"发布后命令执行：{info.publish_info.publish_after_cmd}");
            ExecResult execResult = info.osManagerServer.Exec(info.publish_info.publish_after_cmd).Cast<ExecResult>();
            result.msg = execResult.msg;
            if (!execResult.result)
            {
                result.msg = execResult.msg;
                return result;
            }

            result.result = true;
            return result;
        }

        /// <summary>
        /// 解压缩文件
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private Result ExecUnZip(WorkInfo<t_project, List<FileModePublish>> info)
        {
            Result result = new Result();
            foreach (var item in info.extend_info)
            {
                result = info.osManagerServer.UnZip(item.file_id);
                if (!result.result)
                {
                    return result;
                }
            }
            result.result = true;
            return result;
        }

        /// <summary>
        /// 解压缩文件
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private Result ExecUnZip(WorkInfo<t_flow_project> info)
        {
            publishLogServer.LogAsync(info.proj_info.proj_guid, info.flow_id, "正在解压文件");
            Result result = info.osManagerServer.UnZip($"{info.proj_info.proj_path}/{info.proj_info.proj_guid}.zip -d {info.proj_info.proj_path}");
            if (!result.result)
            {
                publishLogServer.LogAsync(info.proj_info.proj_guid, info.flow_id, "解压文件失败");
                return result;
            }
            result.result = true;
            return result;
        }

        /// <summary>
        /// 连接到数据库
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private Result ConnectSQL(WorkInfo<t_project, List<FileModePublish>> info)
        {
            Result result = new Result();
            if (info.extend_info == null || info.extend_info.Where(w => VerifyCommon.FileType(w.file_id, EFileType.SQL)).Count() == 0)
            {
                result.msg = Tip.TIP_31;
                result.result = true;
                return result;
            }
            info.sqlManageServer = ServerFactory.Get<ISqlManageServer>(EDatabaseType.MSSQL);
            return info.sqlManageServer.Connect(new UserConnectIn());
        }

        /// <summary>
        /// 执行数据库文件
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private Result ExecSql(WorkInfo<t_project, List<FileModePublish>> info)
        {
            Result result = new Result();
            List<FileModePublish> fileList = info.extend_info.Where(w => VerifyCommon.FileType(w.file_id, EFileType.SQL)).ToList();
            if (fileList.Count() == 0)
            {
                result.msg = Tip.TIP_32;
                result.result = true;
                return result;
            }

            //执行脚本
            foreach (var item in fileList)
            {
                Result res = info.sqlManageServer.ExecFileAsync(ServerCommon.GetPublishUploadPath(info.proj_info.proj_guid, item.file_id)).Result;
                result.msg += res;
                if (!res.result)
                {
                    return result;
                }
            }
            result.result = true;
            return result;
        }

        public async Task Start()
        {
            await Instance.StartWorkAsync();
        }

        /// <summary>
        /// 工作 --构建
        /// </summary>
        /// <param name="model"></param>
        private async Task<Result> DoWorkBuild(t_publish_flow model)
        {
            WorkInfo<t_flow_project> info = new WorkInfo<t_flow_project>(model);
            Result result = await DoWorkBeforeExec(info);
            if (!result.result)
            {
                return result;
            }
            //获取代码、连接服务器、发布前命令、构建前命令、构建、构建后命令、打包、上传、解压、发布后命令、关闭连接
            result = DoWorkBuildFlow(info, ConnectService, GetCode, ExecBeforeCommand, ExecBuild, PackageBuildFile, PublishToService, ExecUnZip, ExecAfterCommand, DoWorkAfterExec);

            return result;
        }

        /// <summary>
        /// 工作流 -回调列表形式进行
        /// </summary>
        /// <param name="info">发布信息</param>
        /// <param name="flows">步骤流</param>
        /// <returns></returns>
        private Result DoWorkBuildFlow(WorkInfo<t_flow_project> info, params Func<WorkInfo<t_flow_project>, Result>[] flows)
        {
            Result result = new Result { result = true, msg = Tip.TIP_16 };
            foreach (var item in flows)
            {
                result = item(info);
                publishLogServer.LogAsync(info.proj_info.proj_guid, info.flow_id, result.msg);
                if (!result.result)
                {
                    return result;
                }
            }
            return result;
        }

        /// <summary>
        /// 获取源代码
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private Result GetCode(WorkInfo<t_flow_project> info)
        {
            publishLogServer.LogAsync(info.proj_info.proj_guid, info.flow_id, "正在获取代码");
            string path = Path.GetFullPath(ServerCommon.GetBuildPath(info.proj_info.proj_guid));
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            Result result = osLocal.ChangeWorkspace(path);
            if (!result.result)
            {
                return result;
            }
            //先拉取代码
            result = osLocal.Exec($"cd {path}&&git pull {info.proj_info.code_cmd}");
            if (!result.result)
            {
                switch (result.Cast<ExecResult>().return_code)
                {
                    case 128://仓库不存在
                        if (Directory.Exists(path))
                        {
                            result = osLocal.Exec($"rm -rf {path}");//清理项目目录
                            if (!result.result)
                            {
                                publishLogServer.LogAsync(info.proj_info.proj_guid, info.flow_id, "源代码获取失败[1]");
                                return result;
                            }
                            Directory.CreateDirectory(path);
                        }
                        publishLogServer.LogAsync(info.proj_info.proj_guid, info.flow_id, "创建代码仓库");
                        result = osLocal.Exec($"cd {path}&&git init");//创建仓库
                        if (!result.result)
                        {
                            publishLogServer.LogAsync(info.proj_info.proj_guid, info.flow_id, "源代码获取失败[2]");
                            return result;
                        }
                        publishLogServer.LogAsync(info.proj_info.proj_guid, info.flow_id, "拉取代码");
                        result = osLocal.Exec($"cd {path}&&git pull {info.proj_info.code_cmd}");
                        if (!result.result)
                        {
                            publishLogServer.LogAsync(info.proj_info.proj_guid, info.flow_id, "源代码获取失败[3]");
                            return result;
                        }
                        break;
                    default:
                        break;
                }
            }
            return result;
        }

        /// <summary>
        /// 构建
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private Result ExecBuild(WorkInfo<t_flow_project> info)
        {
            publishLogServer.LogAsync(info.proj_info.proj_guid, info.flow_id, "正在构建项目");
            string path = Path.GetFullPath(ServerCommon.GetBuildPath(info.proj_info.proj_guid));
            Result result = osLocal.Exec($"cd {path}&&{info.publish_info.build_cmd}");
            if (!result.result)
            {
                publishLogServer.LogAsync(info.proj_info.proj_guid, info.flow_id, $"项目构建失败");
            }
            return result;
        }

        /// <summary>
        /// 文件打包
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private Result PackageBuildFile(WorkInfo<t_flow_project> info)
        {
            publishLogServer.LogAsync(info.proj_info.proj_guid, info.flow_id, $"正在打包文件");
            string path = Path.GetFullPath(ServerCommon.GetBuildPath(info.proj_info.proj_guid));
            Result result = osLocal.ChangeWorkspace($"{path}/{info.publish_info.publish_file_path}");
            if (!result.result)
            {
                return result;
            }
            result = osLocal.Zip($"{path}/{info.proj_info.proj_guid}", $"*");
            if (!result.result)
            {
                publishLogServer.LogAsync(info.proj_info.proj_guid, info.flow_id, $"打包文件失败");
                return result;
            }
            osLocal.ChangeWorkspace(path);
            return result;
        }
    }
}
