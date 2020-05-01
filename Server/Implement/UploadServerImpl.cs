using Common;
using Microsoft.AspNetCore.Http;
using Model;
using Model.In;
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

            string path = Path.GetFullPath(ServerCommon.GetUploadPath(fileName + fileSuffix));
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
    }
}
