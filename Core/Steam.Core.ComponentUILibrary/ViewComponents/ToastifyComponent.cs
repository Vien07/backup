using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Mvc;

namespace ComponentUILibrary.ViewComponents
{
    public class ToastifyComponent : ViewComponent
    {
        public IViewComponentResult Invoke(EditorModel model)
        {
            return View(model);
        }
    }


}
