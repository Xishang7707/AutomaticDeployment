using Model.In;
using Model.Out;

namespace App.Interface
{
    public interface IDemoApp : IApp
    {
        /// <summary>
        /// 发布
        /// </summary>
        /// <param name="appData"></param>
        /// <returns></returns>
        public Result Publish(In appData);
    }
}
