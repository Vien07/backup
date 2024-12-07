using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;

namespace ComponentUILibrary.ViewComponents
{
    public class FromDateToDateComponent : ViewComponent
    { 
        //note: dùng isValid_@Model.Id để kiểm tra nếu cần valiate
      //note: dùng isValidMess_@Model.Id để lấy thông báo valid
        public IViewComponentResult Invoke(FromDateToDateModel model)
        {
            return View(model);
        }
        public static async Task<IHtmlContent> Render(ViewContext viewContext, FromDateToDateModel model)
        {
            var viewComponentHelper = (IViewComponentHelper)viewContext.HttpContext.RequestServices.GetRequiredService(typeof(IViewComponentHelper));
            (viewComponentHelper as IViewContextAware)?.Contextualize(viewContext);
            return await viewComponentHelper.InvokeAsync(nameof(FromDateToDateComponent), new { model = model });
        }
    }
}
