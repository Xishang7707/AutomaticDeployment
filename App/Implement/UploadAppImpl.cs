using App.Interface;
using Model.In;
using Model.Out;
using Server;
using Server.Interface;
using System.Threading.Tasks;

namespace App.Implement
{
    /// <summary>
    /// 文件上传
    /// </summary>
    internal class UploadAppImpl : IUploadApp
    {
        private IUploadServer uploadServer = ServerFactory.Get<IUploadServer>();

        public async Task<UploadResult> UploadDemo(UploadIn uploadAppData)
        {
            return await uploadServer.Upload(uploadAppData);
        }
    }
}
