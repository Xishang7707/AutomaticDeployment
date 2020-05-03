using Common;
using Microsoft.AspNetCore.Http;
using Model;
using Model.Extend;
using Model.In;
using Model.In.Upload;
using Model.Out;
using Server.Interface;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Server.Implement
{
    internal class UploadServerImpl : IUploadServer
    {
        /// <summary>
        /// 文件上传验证
        /// </summary>
        /// <param name="uploadIn"></param>
        /// <returns></returns>
        private Result VerifyUpload(UploadIn uploadIn)
        {
            Result result = new Result();
            if (uploadIn == null)
            {
                result.msg = Tip.TIP_1;
                return result;
            }

            if (uploadIn.Files?.Count == 0)
            {
                result.msg = Tip.TIP_2;
                return result;
            }

            result.result = true;
            return result;
        }

        public async Task<UploadResult> Upload(UploadIn uploadIn)
        {
            var verifyResult = VerifyUpload(uploadIn);
            if (!verifyResult.result)
            {
                return verifyResult as UploadResult;
            }
            IFormFile file = uploadIn.Files[0];
            string fileSuffix = GetCommon.GetFileSuffix(file.FileName);
            string fileName = MakeCommon.MakeFileName();

            string path = Path.GetFullPath(ServerCommon.GetPublishUploadPath(fileName + fileSuffix));
            if (!Directory.Exists(ServerConfig.UploadDirectory))
            {
                Directory.CreateDirectory(ServerConfig.UploadDirectory);
            }
            using (FileStream fs = new FileStream(path, FileMode.CreateNew))
            {
                await file.CopyToAsync(fs);
            }

            return new UploadResult
            {
                id = fileName + fileSuffix,
                msg = Tip.TIP_3,
                result = true
            };
        }

        /// <summary>
        /// 发布文件上传验证
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private Result VerifyPublishUpload(PublishUploadIn data)
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

            if (data.files == null || data.files.Count == 0)
            {
                result.msg = Tip.TIP_2;
                return result;
            }

            result.result = true;
            return result;
        }

        public async Task<UploadResult> PublishUpload(PublishUploadIn uploadServerData)
        {
            UploadResult result = VerifyPublishUpload(uploadServerData)?.Cast<UploadResult>();
            if (!result.result)
            {
                return result;
            }
            IFormFile file = uploadServerData.files[0];
            string path = Path.GetFullPath(ServerCommon.GetPublishUploadPath(uploadServerData.project_uid));
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filepath = ServerCommon.GetPublishUploadPath(uploadServerData.project_uid, file.FileName);
            using FileStream fs = new FileStream(filepath, FileMode.Create);
            await file.CopyToAsync(fs);
            result.id = file.FileName;
            return result;
        }
    }
}
