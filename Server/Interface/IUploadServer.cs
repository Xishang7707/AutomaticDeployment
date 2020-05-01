using Model.In;
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
    }
}
