using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;

namespace ComponentUILibrary.ViewComponents
{
    public class ConfigModalComponent : ViewComponent
    {
        public IViewComponentResult Invoke(ConfigModalModel model)
        {
            return View(model);
        }

        public static async Task<IHtmlContent> Render(ViewContext viewContext, ConfigModalModel selectModel)
        {
            var viewComponentHelper = (IViewComponentHelper)viewContext.HttpContext.RequestServices.GetRequiredService(typeof(IViewComponentHelper));
            (viewComponentHelper as IViewContextAware)?.Contextualize(viewContext);
            return await viewComponentHelper.InvokeAsync(nameof(ConfigModalComponent), new { model = selectModel });
        }
    }

    public class ConfigModalModel : MasterModel
    {
        public string Id { get; set; }
        public string Class { get; set; }
        public string Name { get; set; }
        public string ID { get; set; }
        public string BodyUrl { get; set; }
        public List<ConfigModel> Data { get; set; }
        public void GetConfigModel(Dictionary<string, string> models)
        {

            List<ConfigModel> configModels = new List<ConfigModel>();

            foreach (var item in models)
            {
                var configModel = new ConfigModel();
                var key = item.Key;
                var value = item.Value.ToString();

                if (key != null)
                {
                    configModel.Key = key;
                }
                if (value != null)
                {
                    configModel.Value = item.Value.ToString();
                }

                configModels.Add(configModel);
            }
            this.Data = configModels;
        }
        void GetConfigModel_old<T>(List<T> models, string keyPropertyName, string valuePropertyName)
        {

            List<ConfigModel> configModels = new List<ConfigModel>();

            foreach (var item in models)
            {
                var configModel = new ConfigModel();

                // Use reflection to set Key and Value properties dynamically
                var keyProperty = item.GetType().GetProperty(keyPropertyName);
                if (keyProperty != null)
                {
                    configModel.Key = keyProperty.GetValue(item)?.ToString(); // Convert to string if necessary
                }

                var valueProperty = item.GetType().GetProperty(valuePropertyName);
                if (valueProperty != null)
                {
                    configModel.Value = valueProperty.GetValue(item)?.ToString(); // Convert to string if necessary
                }

                configModels.Add(configModel);
            }
            this.Data= configModels;
        }
    }
    public class ConfigModel
    {
        public string Key { get; set; }
        public string Value { get; set; }

    }

}
