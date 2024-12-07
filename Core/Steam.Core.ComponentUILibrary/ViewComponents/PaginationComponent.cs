using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
namespace ComponentUILibrary.ViewComponents
{
    public class PaginationComponent : ViewComponent
    {
        public IViewComponentResult Invoke(PaginationModel model)
        {
            return View(model);
        }
        public static async Task<IHtmlContent> Render(ViewContext viewContext, PaginationModel model)
        {
            var viewComponentHelper = (IViewComponentHelper)viewContext.HttpContext.RequestServices.GetRequiredService(typeof(IViewComponentHelper));
            (viewComponentHelper as IViewContextAware)?.Contextualize(viewContext);
            return await viewComponentHelper.InvokeAsync(nameof(PaginationComponent), model);
        }
    }
    public static class PaginationComponentInfo
    {
        public static string Path = "~/Views/Shared/Components/PaginationComponent/";
        public static string Name = "Default.cshtml";
    }

}
