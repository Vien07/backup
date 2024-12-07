using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Mvc;

namespace ComponentUILibrary.ViewComponents
{
    public class KeyValueComponent : ViewComponent
    {
        public IViewComponentResult Invoke(KeyValueModel model)
        {
            return View(model);
        }
    }
}
