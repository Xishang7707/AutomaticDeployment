using Common;
using Common.Extend;
using DAO;
using Model;
using Model.Db;
using Model.Db.Enum;
using Model.Extend;
using Model.In;
using Model.In.Service;
using Model.Out;
using Model.Out.Service;
using Model.Variable;
using Server.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Implement.Service
{
    /// <summary>
    /// 服务器
    /// </summary>
    class ServiceServerImpl : IServiceServer
    {
        private IPageNoticeServer noticeServer = ServerFactory.Get<IPageNoticeServer>();

        /// <summary>
        /// 验证添加数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private Result VerifyAddService(AddServiceIn data)
        {
            Result result = new Result();
            if (data == null)
            {
                result.msg = Tip.TIP_1;
                return result;
            }
            //验证服务器信息
            if (!string.IsNullOrWhiteSpace(data.server_name))
            {
                data.server_name = data.server_name.Trim();
                if (!VerifyCommon.ServiceNameLength(data.server_name))
                {
                    result.msg = Tip.TIP_36;
                    return result;
                }
            }
            if (!VerifyCommon.OSPlatform(data.server_platform))
            {
                result.msg = Tip.TIP_22;
                return result;
            }
            if (string.IsNullOrWhiteSpace(data.server_ip))
            {
                result.msg = Tip.TIP_5;
                return result;
            }
            if (!VerifyCommon.IP(data.server_ip))
            {
                result.msg = Tip.TIP_6;
                return result;
            }
            if (string.IsNullOrWhiteSpace(data.server_port))
            {
                result.msg = Tip.TIP_7;
                return result;
            }
            if (!VerifyCommon.Port(data.server_port))
            {
                result.msg = Tip.TIP_8;
                return result;
            }
            if (string.IsNullOrWhiteSpace(data.server_account))
            {
                result.msg = Tip.TIP_9;
                return result;
            }
            if (!VerifyCommon.ServiceAccountLength(data.server_account))
            {
                result.msg = Tip.TIP_17;
                return result;
            }
            if (string.IsNullOrWhiteSpace(data.server_password))
            {
                result.msg = Tip.TIP_10;
                return result;
            }
            if (!VerifyCommon.ServicePasswordLength(data.server_password))
            {
                result.msg = Tip.TIP_18;
                return result;
            }
            if (!string.IsNullOrWhiteSpace(data.server_space))
            {
                data.server_space = data.server_space.Trim();
                if (!VerifyCommon.ServiceWorkspaceLength(data.server_space))
                {
                    result.msg = Tip.TIP_39;
                    return result;
                }
            }
            result.result = true;
            return result;
        }

        public async Task<Result> AddService(In<AddServiceIn> inData)
        {
            Result result = VerifyAddService(inData.data);
            if (!result.result)
            {
                return result;
            }

            SQLiteHelper db = new SQLiteHelper();
            try
            {
                int id = await DAO.Service.ServiceDao.InsertAsync(db, inData.data);
                if (id <= 0)
                {
                    db.Close();
                    result.msg = Tip.TIP_38;
                    return result;
                }
                db.Close();

                //通知页面
                noticeServer.Add(DefPagePid.Service);
                result.result = true;
                result.msg = Tip.TIP_37;
            }
            catch (Exception e)
            {
                db.Close();
                result.msg = Tip.TIP_38;
            }
            return result;
        }

        public async Task<Result> GetDropService()
        {
            SQLiteHelper db = new SQLiteHelper();
            List<t_service> serviceList = await DAO.Service.ServiceDao.GetKvAll(db);
            Result<List<IntValue>> result = new Result<List<IntValue>> { result = true, data = new List<IntValue>() };
            foreach (var item in serviceList)
            {
                var service = new IntValue
                {
                    value = item.id
                };
                if (string.IsNullOrWhiteSpace(item.name))
                {
                    service.name = $"{item.conn_ip}";
                }
                else
                {
                    service.name = $"{item.name}({item.conn_ip})";
                }
                result.data.Add(service);
            }

            return result;
        }

        public async Task<Result> GetServiceList()
        {
            SQLiteHelper db = new SQLiteHelper();
            List<t_service> serviceList = await DAO.Service.ServiceDao.GetListAll(db);
            Result<List<ServiceItemResult>> result = new Result<List<ServiceItemResult>> { result = true, data = new List<ServiceItemResult>() };
            foreach (var item in serviceList)
            {
                var service = new ServiceItemResult
                {
                    server_id = item.id,
                    server_name = item.name,
                    server_account = item.conn_user,
                    server_ip = item.conn_ip,
                    workspace = item.work_spacepath.Ns(),
                    server_port = item.conn_port,
                    server_platform = ((EOSPlatform)item.platform_type).GetDesc(),
                    act_delete = !await DAO.Service.ServiceDao.UsedService(db, item.id)
                };
                if (string.IsNullOrWhiteSpace(item.name))
                {
                    service.server_name = $"{item.conn_ip}:{item.conn_port}";
                }
                else
                {
                    service.server_name = $"{item.name}";
                }
                result.data.Add(service);
            }

            return result;
        }

        /// <summary>
        /// 验证删除数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private Result VerifyDeleteService(DeleteServiceIn data)
        {
            Result result = new Result();
            if (data == null)
            {
                result.msg = Tip.TIP_1;
                return result;
            }
            if (string.IsNullOrWhiteSpace(data.service_id) || !int.TryParse(data.service_id, out int service_id) || service_id <= 0)
            {
                result.msg = Tip.TIP_41;
                return result;
            }

            result.result = true;
            return result;
        }

        public async Task<Result> DeleteService(In<DeleteServiceIn> inData)
        {
            Result result = VerifyDeleteService(inData.data);
            if (!result.result)
            {
                return result;
            }
            result.result = false;
            int id = int.Parse(inData.data.service_id);
            SQLiteHelper db = new SQLiteHelper();
            bool is_used = await DAO.Service.ServiceDao.UsedService(db, id);
            if (is_used)
            {
                db.Close();
                result.msg = Tip.TIP_48;
                return result;
            }

            bool delete_flag = await DAO.Service.ServiceDao.Delete(db, id);
            db.Close();
            if (!delete_flag)
            {
                result.msg = Tip.TIP_49;
                return result;
            }

            //通知页面
            noticeServer.Delete(DefPagePid.Service, id.ToString());

            result.result = true;
            result.msg = Tip.TIP_50;
            return result;
        }

        /// <summary>
        /// 验证获取数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private Result VerifyGetService(string data)
        {
            Result result = new Result();
            if (data == null)
            {
                result.msg = Tip.TIP_1;
                return result;
            }
            if (string.IsNullOrWhiteSpace(data) || !int.TryParse(data, out int service_id) || service_id <= 0)
            {
                result.msg = Tip.TIP_41;
                return result;
            }

            result.result = true;
            return result;
        }

        public async Task<Result> GetService(In<string> inData)
        {
            Result<ServiceInfoResult> result = VerifyGetService(inData.data).Cast<Result<ServiceInfoResult>>();
            if (!result.result)
            {
                return result;
            }

            SQLiteHelper db = new SQLiteHelper();
            t_service server_model = await DAO.Service.ServiceDao.GetService(db, int.Parse(inData.data));
            db.Close();
            if (server_model == null)
            {
                result.msg = Tip.TIP_41;
                return result;
            }

            result.data = new ServiceInfoResult
            {
                server_account = server_model.conn_user.Ns(),
                server_id = server_model.id,
                server_ip = server_model.conn_ip.Ns(),
                server_name = server_model.name.Ns(),
                server_password = string.IsNullOrWhiteSpace(server_model.conn_password) ? "" : GetCommon.GetHidePassword(),
                server_platform = server_model.platform_type,
                server_port = server_model.conn_port,
                workspace = server_model.work_spacepath.Ns()
            };

            return result;
        }

        /// <summary>
        /// 验证修改数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private Result VerifyEditService(EditServiceIn data)
        {
            Result result = new Result();
            if (data == null)
            {
                result.msg = Tip.TIP_1;
                return result;
            }
            //验证服务器信息
            if (string.IsNullOrWhiteSpace(data.server_id) || !int.TryParse(data.server_id, out int service_id) || service_id <= 0)
            {
                result.msg = Tip.TIP_41;
                return result;
            }
            if (!string.IsNullOrWhiteSpace(data.server_name))
            {
                data.server_name = data.server_name.Trim();
                if (!VerifyCommon.ServiceNameLength(data.server_name))
                {
                    result.msg = Tip.TIP_36;
                    return result;
                }
            }
            if (!VerifyCommon.OSPlatform(data.server_platform))
            {
                result.msg = Tip.TIP_22;
                return result;
            }
            if (string.IsNullOrWhiteSpace(data.server_ip))
            {
                result.msg = Tip.TIP_5;
                return result;
            }
            if (!VerifyCommon.IP(data.server_ip))
            {
                result.msg = Tip.TIP_6;
                return result;
            }
            if (string.IsNullOrWhiteSpace(data.server_port))
            {
                result.msg = Tip.TIP_7;
                return result;
            }
            if (!VerifyCommon.Port(data.server_port))
            {
                result.msg = Tip.TIP_8;
                return result;
            }
            if (string.IsNullOrWhiteSpace(data.server_account))
            {
                result.msg = Tip.TIP_9;
                return result;
            }
            if (!VerifyCommon.ServiceAccountLength(data.server_account))
            {
                result.msg = Tip.TIP_17;
                return result;
            }
            if (string.IsNullOrWhiteSpace(data.server_password))
            {
                result.msg = Tip.TIP_10;
                return result;
            }
            if (!VerifyCommon.ServicePasswordLength(data.server_password))
            {
                result.msg = Tip.TIP_18;
                return result;
            }
            if (!string.IsNullOrWhiteSpace(data.server_space))
            {
                data.server_space = data.server_space.Trim();
                if (!VerifyCommon.ServiceWorkspaceLength(data.server_space))
                {
                    result.msg = Tip.TIP_39;
                    return result;
                }
            }
            result.result = true;
            return result;
        }

        public async Task<Result> EditService(In<EditServiceIn> inData)
        {
            Result result = VerifyEditService(inData.data);
            if (!result.result)
            {
                return result;
            }

            SQLiteHelper db = new SQLiteHelper();
            result.result = await DAO.Service.ServiceDao.Update(db, inData.data);
            db.Close();
            if (!result.result)
            {
                result.msg = Tip.TIP_35;
                return result;
            }

            //通知页面
            noticeServer.Update(DefPagePid.Service);

            result.msg = Tip.TIP_34;
            return result;
        }
    }
}
