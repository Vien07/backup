using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using Org.BouncyCastle.Utilities;
using System.Drawing.Printing;
using X.PagedList;

namespace ComponentUILibrary.ViewComponents
{
    public class TableComponent : ViewComponent
    {
        public IViewComponentResult Invoke(Models.TableData table)
        {
            return View(table);
        }
        public static async Task<IHtmlContent> Render(ViewContext viewContext, Models.TableData model)
        {
            var viewComponentHelper = (IViewComponentHelper)viewContext.HttpContext.RequestServices.GetRequiredService(typeof(IViewComponentHelper));
            (viewComponentHelper as IViewContextAware)?.Contextualize(viewContext);
            return await viewComponentHelper.InvokeAsync(nameof(TableComponent), new { table = model });
        }
        public class TableData<TModel>
        {
            public int STT { get; set; }
            public IPagedList<TModel> Items { get; set; }
            private void GetSTT()
            {
                try
                {
                    this.STT = Items.PageNumber * Items.PageSize - Items.PageSize;

                }
                catch (Exception)
                {

                    this.STT = 0;
                }
            }
        }
    }



}
