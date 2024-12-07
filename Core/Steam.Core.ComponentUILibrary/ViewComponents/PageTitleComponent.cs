using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Mvc;

namespace ComponentUILibrary.ViewComponents
{
    public class PageTitleComponent : ViewComponent
    {
        public IViewComponentResult Invoke(PageTitleModel model)
        {
            return View(model);
        }
    }
}
