using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;

namespace ComponentUILibrary.ViewComponents
{
    public class MoveableModalComponent : ViewComponent
    {
        public IViewComponentResult Invoke(MoveableModalModel model)
        {
            return View(model);
        }
        public static async Task<IHtmlContent> Render(ViewContext viewContext, MoveableModalModel model)
        {
            var viewComponentHelper = (IViewComponentHelper)viewContext.HttpContext.RequestServices.GetRequiredService(typeof(IViewComponentHelper));
            (viewComponentHelper as IViewContextAware)?.Contextualize(viewContext);
            return await viewComponentHelper.InvokeAsync(nameof(MoveableModalComponent), model);
        }
    }
    public class MoveableModalModel
    {
        public string Id { get; set; }
        public string Class { get; set; }
        public string Content { get; set; }
        public string Cols { get; set; }
        public string Rows { get; set; }
        public int Height { get; set; } = 400;
        public bool FirstLoadLib { get; set; } = false;

    }


}
