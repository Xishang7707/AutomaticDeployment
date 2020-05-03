using Model.In;
using Model.In.Upload;
using Model.Out;
using System.Threading.Tasks;

namespace App.Interface
{
    /// <summary>
    /// 文件上传
    /// </summary>
    public interface IUploadApp : IApp
    {
        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="uploadAppData"></param>
        /// <returns></returns>
        Task<UploadResult> UploadDemo(UploadIn uploadAppData);

        /// <summary>
        /// 发布文件上传
        /// </summary>
        /// <param name="uploadServerData"></param>
        /// <returns></returns>
        Task<UploadResult> PublishUpload(PublishUploadIn uploadServerData);
    }
}
