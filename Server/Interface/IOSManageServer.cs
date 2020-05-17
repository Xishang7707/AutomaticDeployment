using Model.In.OSManage;
using Model.Out;
using Model.Out.OSManage;
using System.Text;

namespace Server.Interface
{
    /// <summary>
    /// 操作系统管理
    /// </summary>
    public interface IOSManageServer : IServer
    {
        /// <summary>
        /// 用户名密码连接
        /// </summary>
        /// <param name="conn">连接信息</param>
        /// <returns></returns>
        Result Connect(UserConnectIn conn);

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="shell">命令</param>
        /// <returns></returns>
        ExecResult Exec(string shell);

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="originPath">源路路径</param>
        /// <param name="targetPath">目标路径</param>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        Model.Out.OSManage.UploadResult Upload(string originPath, string targetPath, string fileName);

        /// <summary>
        /// 切换工作命令
        /// --之后一切操作将都在工作目录下执行/除非使用绝对路径
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns></returns>
        Result ChangeWorkspace(string path);

        /// <summary>
        /// 解压
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        Result UnZip(string fileName);

        /// <summary>
        /// 压缩
        /// </summary>
        /// <param name="name">打包文件名</param>
        /// <param name="path">路径</param>
        /// <returns></returns>
        Result Zip(string name, string path);

        /// <summary>
        /// 关闭
        /// </summary>
        void Close();
    }
}
