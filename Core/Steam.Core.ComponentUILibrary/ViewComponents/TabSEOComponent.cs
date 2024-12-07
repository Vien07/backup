using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Mvc;

namespace ComponentUILibrary.ViewComponents
{
    public class TabSEOComponent : ViewComponent
    {
        public IViewComponentResult Invoke(TabSEOCModel model)
        {
            return View(model);
        }

    }

}
