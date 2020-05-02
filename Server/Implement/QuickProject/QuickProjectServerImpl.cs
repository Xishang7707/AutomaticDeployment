using Common;
using DAO;
using DAO.QuickProject;
using Model;
using Model.Dao.QuickProject;
using Model.Db;
using Model.Db.Enum;
using Model.In;
using Model.In.QuickProject;
using Model.Out;
using Server.Interface;
using System;
using System.Collections.Generic;
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
            result.result = true;
            return result;
        }

        public async Task<Result> AddQuickProjectAsync(In<AddQuickProjectIn> inData)
        {
            Result result = VerifyAddQuickProject(inData.Data);
            if (!result.result)
            {
                return result;
            }
            result.result = false;

            SQLiteHelper dbHelper = new SQLiteHelper();
            try
            {
                await dbHelper.BeginTransactionAsync();

                AddProjectDaoResult addProjectDaoResult = await ProjectDao.AddProjectModelAsync(dbHelper, inData.Data.project);
                if (!addProjectDaoResult.result)
                {
                    await dbHelper.RollbackAsync();
                    result.msg = Tip.TIP_20;
                    return result;
                }
                result.result = await QuickProjectDao.AddQuickProjectModelAsync(dbHelper, inData.Data, addProjectDaoResult.proj_guid);
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
    }
}
