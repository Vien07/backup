using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Mvc;

namespace ComponentUILibrary.ViewComponents
{
    public class ModalInfoImageComponent : ViewComponent
    {
        public IViewComponentResult Invoke(EditorModel model)
        {
            return View(model);
        }
    }


}
