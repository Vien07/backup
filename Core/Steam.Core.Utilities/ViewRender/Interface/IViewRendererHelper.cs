using System.Collections.Generic;
using System.Threading.Tasks;

namespace Steam.Core.Utilities.STeamHelper
{
    public interface IViewRendererHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="name"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        string RenderPartialViewToString<TModel>(string Path, string name, TModel model);
        string RenderPartialViewToString<TModel>(string Path, TModel model);


    }
}