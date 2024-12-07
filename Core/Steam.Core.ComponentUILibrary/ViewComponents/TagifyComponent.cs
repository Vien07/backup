using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Mvc;

namespace ComponentUILibrary.ViewComponents
{
    public class TagifyComponent : ViewComponent
    {
        public IViewComponentResult Invoke(TagifyModel model)
        {
            return View(model);
        }
    }


}
