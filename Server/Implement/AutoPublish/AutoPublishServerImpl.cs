using Common;
using DAO;
using DAO.AutoPublish;
using Model;
using Model.Db;
using Model.Db.Enum;
using Model.Extend;
using Model.In.PublishFlow;
using Model.Out;
using Model.Ssh;
using Newtonsoft.Json;
using Server.Interface;
using System;
using System.Collections.Generic;
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
            TimeSpan taskTimeOut = TimeSpan.FromMinutes(5);//5分钟后取消
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
                            //开始发布
                            bool startFlag = await PublishStart(publishFlow.proj_guid, publishFlow.id);
                            if (!startFlag)
                            {
                                timer?.Dispose();
                                return;
                            }

                            //进行中
                            Result result = DoWorkFile(publishFlow);

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
                        catch (Exception)
                        {

                        }
                    }, cancelToken.Token);
                }
            });
        }

        /// <summary>
        /// 工作 --文件发布
        /// </summary>
        /// <param name="model"></param>
        private Result DoWorkFile(t_publish_flow model)
        {
            WorkInfo<List<FileModePublish>> info = new WorkInfo<List<FileModePublish>>(model);
            Result result = ConnectService(info);
            publishLogServer.LogAsync(new Model.In.PublishLog.LogInfo { proj_guid = info.proj_info.proj_guid, publish_id = info.flow_id, publish_info = result.msg });
            if (!result.result)
            {
                return result;
            }
            //发布前命令
            result = ExecBeforeCommand(info);
            publishLogServer.LogAsync(new Model.In.PublishLog.LogInfo { proj_guid = info.proj_info.proj_guid, publish_id = info.flow_id, publish_info = result.msg });
            if (!result.result)
            {
                return result;
            }
            //上传文件
            result = PublishToService(info);
            publishLogServer.LogAsync(new Model.In.PublishLog.LogInfo { proj_guid = info.proj_info.proj_guid, publish_id = info.flow_id, publish_info = result.msg });
            if (!result.result)
            {
                return result;
            }
            //解压
            result = ExecUnZip(info);
            publishLogServer.LogAsync(new Model.In.PublishLog.LogInfo { proj_guid = info.proj_info.proj_guid, publish_id = info.flow_id, publish_info = result.msg });
            if (!result.result)
            {
                return result;
            }
            //发布后命令
            result = ExecAfterCommand(info);
            publishLogServer.LogAsync(new Model.In.PublishLog.LogInfo { proj_guid = info.proj_info.proj_guid, publish_id = info.flow_id, publish_info = result.msg });
            if (!result.result)
            {
                return result;
            }
            info.osManagerServer.Close();
            publishLogServer.LogAsync(new Model.In.PublishLog.LogInfo { proj_guid = info.proj_info.proj_guid, publish_id = info.flow_id, publish_info = result.msg });
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
        class WorkInfo
        {
            /// <summary>
            /// 项目信息
            /// </summary>
            public t_project proj_info { get; set; }

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

            public WorkInfo() { }
            public WorkInfo(t_publish_flow model)
            {
                proj_info = JsonConvert.DeserializeObject<t_project>(model.proj_info);
                service_info = JsonConvert.DeserializeObject<t_service>(model.server_info);
                publish_info = JsonConvert.DeserializeObject<t_publish>(model.publish_info);
            }
        }

        /// <summary>
        /// 工作任务信息
        /// </summary>
        class WorkInfo<T> : WorkInfo where T : class, new()
        {
            /// <summary>
            /// 扩展信息
            /// </summary>
            public T extend_info { get; set; }

            public WorkInfo() { }
            public WorkInfo(t_publish_flow model) : base(model) => extend_info = (string.IsNullOrWhiteSpace(model.extern_info) ? null : JsonConvert.DeserializeObject<T>(model.extern_info));
        }

        /// <summary>
        /// 发布第一步 连接服务器
        /// </summary>
        /// <param name="service">服务器信息</param>
        /// <returns></returns>
        private Result ConnectService(WorkInfo<List<FileModePublish>> info)
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
        /// 发布前命令 --连接
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private Result ExecBeforeCommand(WorkInfo<List<FileModePublish>> info)
        {
            Result result = new Result();
            if (string.IsNullOrWhiteSpace(info.publish_info.publish_before_cmd))
            {
                result.result = true;
                return result;
            }

            ExecResult execResult = info.osManagerServer.Exec(info.publish_info.publish_before_cmd).Cast<ExecResult>();
            if (!execResult.result)
            {
                result.msg = execResult.msg;
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
        private Result PublishToService(WorkInfo<List<FileModePublish>> info)
        {
            Result result = new Result();

            if (info.extend_info == null || info.extend_info.Count == 0)
            {
                result.msg = Tip.TIP_12;
                return result;
            }
            foreach (var item in info.extend_info)
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
        /// 发布后命令 --连接
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private Result ExecAfterCommand(WorkInfo<List<FileModePublish>> info)
        {
            Result result = new Result();
            if (string.IsNullOrWhiteSpace(info.publish_info.publish_after_cmd))
            {
                result.result = true;
                return result;
            }

            ExecResult execResult = info.osManagerServer.Exec(info.publish_info.publish_after_cmd).Cast<ExecResult>();
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
        private Result ExecUnZip(WorkInfo<List<FileModePublish>> info)
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

        public async Task Start()
        {
            await Instance.StartWorkAsync();
        }
    }
}
