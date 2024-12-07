using Microsoft.AspNetCore.Mvc;

namespace ComponentUILibrary.ViewComponents
{
    public class TableTreeComponent : ViewComponent
    {
        public IViewComponentResult Invoke(Models.TableTreeModel model)
        {
            return View(model);
        }

    }

}
