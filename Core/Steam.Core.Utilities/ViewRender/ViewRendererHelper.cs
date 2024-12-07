using Microsoft.AspNetCore.Http;


using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc.Abstractions;

namespace Steam.Core.Utilities.STeamHelper
{
    public class ViewRendererHelper : IViewRendererHelper
    {
        private IRazorViewEngine _viewEngine;
        private ITempDataProvider _tempDataProvider;
        private IServiceProvider _serviceProvider;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewEngine"></param>
        /// <param name="tempDataProvider"></param>
        /// <param name="serviceProvider"></param>
        public ViewRendererHelper(
            IRazorViewEngine viewEngine,
            ITempDataProvider tempDataProvider,
            IServiceProvider serviceProvider)
        {
            _viewEngine = viewEngine;
            _tempDataProvider = tempDataProvider;
            _serviceProvider = serviceProvider;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="name"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public string RenderPartialViewToString<TModel>(string Path, string name, TModel model)
        {
    
            var actionContext = GetActionContext();
            //var viewEngineResulta = _viewEngine.GetView("~/Pages/CMS/_Partial/", "_PartialModal.cshtml", false);
            var viewEngineResult = _viewEngine.GetView(Path, name, false);
            // var viewEngineResult = _viewEngine.GetView("/Views/Home/_Partial/", name, false);
            if (!viewEngineResult.Success)
            {
                throw new InvalidOperationException(string.Format("Couldn't find view '{0}'", name));
            }
            var view = viewEngineResult.View;
            using (var output = new StringWriter())
            {
                var viewContext = new ViewContext(
                    actionContext,
                    view,
                    new ViewDataDictionary<TModel>(
                        metadataProvider: new EmptyModelMetadataProvider(),
                        modelState: new ModelStateDictionary())
                    {
                        Model = model
                    },
                    new TempDataDictionary(
                        actionContext.HttpContext,
                        _tempDataProvider),
                    output,
                    new HtmlHelperOptions());
                view.RenderAsync(viewContext).GetAwaiter().GetResult();
                return output.ToString();
            }
        }
        public string RenderPartialViewToString<TModel>(string Path,TModel model)
        {
            var arrPath = Path.Split("/");
            string name = arrPath[arrPath.Length - 1];
            var actionContext = GetActionContext();
            //var viewEngineResulta = _viewEngine.GetView("~/Pages/CMS/_Partial/", "_PartialModal.cshtml", false);
            var viewEngineResult = _viewEngine.GetView(Path, name, false);
            // var viewEngineResult = _viewEngine.GetView("/Views/Home/_Partial/", name, false);
            if (!viewEngineResult.Success)
            {
                throw new InvalidOperationException(string.Format("Couldn't find view '{0}'", name));
            }
            var view = viewEngineResult.View;
            using (var output = new StringWriter())
            {
                var viewContext = new ViewContext(
                    actionContext,
                    view,
                    new ViewDataDictionary<TModel>(
                        metadataProvider: new EmptyModelMetadataProvider(),
                        modelState: new ModelStateDictionary())
                    {
                        Model = model
                    },
                    new TempDataDictionary(
                        actionContext.HttpContext,
                        _tempDataProvider),
                    output,
                    new HtmlHelperOptions());
                view.RenderAsync(viewContext).GetAwaiter().GetResult();
                return output.ToString();
            }
        }

        private ActionContext GetActionContext()
        {
            var httpContext = new DefaultHttpContext
            {
                RequestServices = _serviceProvider
            };
            return new ActionContext(httpContext, new RouteData(), new ActionDescriptor());
        }
    }
}
