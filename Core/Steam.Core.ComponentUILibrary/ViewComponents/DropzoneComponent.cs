using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using Steam.Core.Utilities.STeamHelper;
using Steam.Core.Utilities.SteamModels;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

using Microsoft.Extensions.DependencyInjection;
using static ComponentUILibrary.Models.DropzoneModel;

namespace ComponentUILibrary.ViewComponents
{
    public class DropzoneComponent : ViewComponent
    {
        public IViewComponentResult Invoke(DropzoneModel model)
        {
            return View(model);
        }
        public static async Task<IHtmlContent> Render(ViewContext viewContext, DropzoneModel model)
        {
            var viewComponentHelper = (IViewComponentHelper)viewContext.HttpContext.RequestServices.GetRequiredService(typeof(IViewComponentHelper));
            (viewComponentHelper as IViewContextAware)?.Contextualize(viewContext);
            return await viewComponentHelper.InvokeAsync(nameof(DropzoneComponent), new { model = model });
        }
        public List<DropzoneModel.File> SaveListFile(SaveImageModel imageModel, string listFIlesString)
        {
            FileHelper _fileHelper = new FileHelper();

            List<FileInfoModel> listFIles = JsonConvert.DeserializeObject<List<FileInfoModel>>(listFIlesString);
            List<DropzoneModel.File> listFiles = new List<DropzoneModel.File>();
            try
            {

                var absolutepath = Directory.GetCurrentDirectory();

                foreach (var item in listFIles)
                {
                    if (item.status == "new")
                    {
                        var saveFile = _fileHelper.UploadImagesBase64(new UploadImageBase64Info
                        {
                            Base64 = item.dataUrl,
                            Height = imageModel.Height,
                            Width = imageModel.Width ,
                            FileName = imageModel.ImageName,
                            Path = imageModel.UploadPath
                        });
                        if (!saveFile.isError)
                        {
                            listFiles.Add(new DropzoneModel.File
                            {
                                Status = "new",
                                FilePath = imageModel.UploadPath,
                                FileName= saveFile.FileName,
                                Pid = 0,
                                FileInfo = new FileInfoModel
                                {
                                    alt=item.alt,
                                    caption=item.caption,
                                    description=item.description,
                                    order=item.order,
                                }


                            });

                        }


                    }
                    else if (item.status == "delete")
                    {
     
                        listFiles.Add(new DropzoneModel.File
                        {
                            Status = "delete",
                            Pid = Convert.ToInt32(item.id)             
                        });

                    }
                    else if (item.status == "edit")
                    {
             
                        listFiles.Add(new DropzoneModel.File
                        {
                            Status = "edit",
                            FilePath ="",
                            FileName ="",
                            Pid = Convert.ToInt32(item.id),
                            FileInfo = new FileInfoModel
                            {
                                alt = item.alt,
                                caption = item.caption,
                                description = item.description,
                                order = item.order,
                            }
                        });
                    }
                    if (item.status == "server")
                    {
                        var arrFile = item.dataUrl.Split('/');
                        var img = arrFile[arrFile.Length - 1];
                        var filePath = item.dataUrl.Replace(img, "");
                        listFiles.Add(new DropzoneModel.File
                        {
                            Status = "new",
                            FilePath = filePath,
                            FileName = img,
                            Pid = 0,
                            FileInfo = new FileInfoModel
                            {
                                alt = item.alt,
                                caption = item.caption,
                                description = item.description,
                                order = item.order,
                            }
                        });


                    }


                }


            }
            catch (Exception ex)
            {

            }
            return listFiles;
        }
    }


}
