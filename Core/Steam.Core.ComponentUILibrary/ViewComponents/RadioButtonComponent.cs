using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Mvc;

namespace ComponentUILibrary.ViewComponents
{
    public class RadioButtonComponent : ViewComponent
    {
        public IViewComponentResult Invoke(RadioButtonModel model)
        {
            return View(model);
        }
    }
}
