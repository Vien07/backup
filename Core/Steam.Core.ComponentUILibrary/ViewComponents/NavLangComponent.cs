using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ComponentUILibrary.ViewComponents
{
    public class NavLangComponent : ViewComponent
    {
        public List<MultilangModel> _multiLang;
        public NavLangComponent(IOptions<List<MultilangModel>> multiLang)
        {
            _multiLang = multiLang.Value;
        }
        public IViewComponentResult Invoke(NavLangModel model)
        {
            if(model == null)
            {
                model = new NavLangModel();
            }    
            if(model.Active == String.Empty)
            {
                if(_multiLang != null && _multiLang.Any())
                {
                    model.Active = _multiLang.Where(p => p.isDefault == true).FirstOrDefault().Lang;
                    model.DefaultLang = model.Active;
                }
            }    
            model.ListLangs = _multiLang.OrderBy(p=>p.Order).ToList();
            return View(model);
        }

    }


}
