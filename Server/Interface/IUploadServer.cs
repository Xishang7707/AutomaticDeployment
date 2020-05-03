using Model.In;
using Model.In.Upload;
using Model.Out;
using System.Threading.Tasks;

namespace Server.Interface
{
    /// <summary>
    /// 文件上传
    /// </summary>
    public interface IUploadServer : IServer
    {
        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="uploadServerData"></param>
        /// <returns></returns>
        Task<UploadResult> Upload(UploadIn uploadServerData);

        /// <summary>
        /// 发布文件上传
        /// </summary>
        /// <param name="uploadServerData"></param>
        /// <returns></returns>
        Task<UploadResult> PublishUpload(PublishUploadIn uploadServerData);
    }
}
