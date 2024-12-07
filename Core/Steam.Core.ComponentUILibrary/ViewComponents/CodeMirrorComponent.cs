using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Mvc;

namespace ComponentUILibrary.ViewComponents
{
    public class CodeMirrorComponent : ViewComponent
    {
        public IViewComponentResult Invoke(CodeMirrorModel model)
        {
            return View(model);
        }
    }


}
