using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Steam.Core.ComponentUILibrary.Constant;
using Steam.Core.Utilities.STeamHelper;
using Steam.Core.Utilities.SteamModels;
using Steam.Core.Base.Constant;
using static ComponentUILibrary.Models.FileUploadControlModel;
using Steam.Core.Common.SteamString;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

using Microsoft.Extensions.DependencyInjection;
namespace ComponentUILibrary.ViewComponents
{
    public class FileUploadFilePondComponent : ViewComponent
    {

        public IViewComponentResult Invoke(FileUploadControlModel model)
        {
            return View(model);
        }
        public static async Task<IHtmlContent> Render(ViewContext viewContext, FileUploadControlModel model)
        {
            var viewComponentHelper = (IViewComponentHelper)viewContext.HttpContext.RequestServices.GetRequiredService(typeof(IViewComponentHelper));
            (viewComponentHelper as IViewContextAware)?.Contextualize(viewContext);
            return await viewComponentHelper.InvokeAsync(nameof(FileUploadFilePondComponent), new { model = model });
        }
        public SaveImageModel SaveImage(SaveImageModel saveImageModel)
        {
            FileHelper _fileHelper = new FileHelper();

            SaveImageModel rs = new SaveImageModel();
            try
            {
               
                if (saveImageModel.Filestatus == "remove" && saveImageModel.File == null)
                {
                   
                     rs.ImageName="";
                     rs.FilePath="";
                    return rs;
                }
                if (saveImageModel.Filestatus == "server" && saveImageModel.File == null)
                {

                    var arrFile = saveImageModel.FilePath.Split('/');
                    rs.ImageName = arrFile[arrFile.Length - 1];
                    rs.FilePath = SystemInfo.PathFileManager + "/" + saveImageModel.FilePath.Replace(rs.ImageName, "");
                    return rs;
                }

                if (saveImageModel.Filestatus != "existed")
                {

                    if (!String.IsNullOrEmpty(saveImageModel.ImageName))
                    {
                        var arrFile = saveImageModel.FilePath.Split('/');
                        rs.ImageName = arrFile[arrFile.Length - 1];
                        rs.FilePath = SystemInfo.PathFileManager + "/" + saveImageModel.FilePath.Replace(saveImageModel.ImageName, "");
                    }
                    else
                    {
                        if (saveImageModel.File != null)
                        {
                        
                           var fileName = _fileHelper.UploadImageModule(saveImageModel.File,saveImageModel.UploadPath,saveImageModel.UploadThumbPath,saveImageModel.Height,saveImageModel.Width);
                            if(!String.IsNullOrEmpty(fileName))
                            {
                                rs.FilePath = saveImageModel.UploadPath;
                                rs.ImageName = fileName;

                            }
                        }

                    }


                }
                else
                {
                    rs.IsExisted = true;
                }
            }
            catch (Exception ex)
            {

            }
            return rs;

        }

    }


}
