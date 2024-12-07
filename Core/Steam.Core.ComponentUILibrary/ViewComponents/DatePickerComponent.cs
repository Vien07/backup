using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;

namespace ComponentUILibrary.ViewComponents
{
    public class DatePickerComponent : ViewComponent
    {
        public IViewComponentResult Invoke(DatePickerModel model)
        {
            return View(model);
        }
        public static async Task<IHtmlContent> Render(ViewContext viewContext, DatePickerModel selectModel)
        {
            var viewComponentHelper = (IViewComponentHelper)viewContext.HttpContext.RequestServices.GetRequiredService(typeof(IViewComponentHelper));
            (viewComponentHelper as IViewContextAware)?.Contextualize(viewContext);
            return await viewComponentHelper.InvokeAsync(nameof(DatePickerComponent), new { model = selectModel });
        }
    }
}
