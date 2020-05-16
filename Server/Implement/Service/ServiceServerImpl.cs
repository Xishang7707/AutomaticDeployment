using Common;
using DAO;
using DAO.Service;
using Model;
using Model.Db;
using Model.In;
using Model.In.Service;
using Model.Out;
using Model.Out.Service;
using Server.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Server.Implement.Service
{
    /// <summary>
    /// 服务器
    /// </summary>
    class ServiceServerImpl : IServiceServer
    {
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
            if (!string.IsNullOrWhiteSpace(data.workspace))
            {
                data.workspace = data.workspace.Trim();
                if (!VerifyCommon.ServiceWorkspaceLength(data.workspace))
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
            Result<List<ServiceKvResult>> result = new Result<List<ServiceKvResult>> { result = true, data = new List<ServiceKvResult>() };
            foreach (var item in serviceList)
            {
                var service = new ServiceKvResult
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
    }
}
