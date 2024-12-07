using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Steam.Core.Base.Constant;
using Steam.Core.Utilities.SteamModels;
using Steam.Core.Common.SteamString;
using Svg;
using System.Drawing;
using ImageProcessor;
using ImageProcessor.Plugins.WebP.Imaging.Formats;
using Newtonsoft.Json;
using RestSharp;
using Steam.Core.Common;

namespace Steam.Core.Utilities.STeamHelper
{
    public class FileServerHelper : IFileHelper
    {
        string VirtualFolder = "";
        string ApiUpload = "";
        string ApiUploadBase64 = "";
        string AccessKey = "";
        private readonly IConfiguration _configuration;

        public FileServerHelper(IConfiguration configuration)
        {
            _configuration = configuration;
            ApiUpload = _configuration["FileManager:Api_Upload"];
            ApiUploadBase64 = _configuration["FileManager:Api_UploadBase64"];
            AccessKey = _configuration["FileManager:access_keys"];
        }
        public string UploadImageModule(IFormFile File,  string SavePath, string SavePathThumb, int height, int width)
        {
            try
            {
                var fileName = Path.GetFileNameWithoutExtension(File.FileName) + "-" + string.Empty.ToUniqeID() + Path.GetExtension(File.FileName);

                var absolutePath = Directory.GetCurrentDirectory();
                //string path = Path.Combine(absolutePath + "/wwwroot", "\\FileStorage\\Storage\\PostsCategory\\img");


                var absolutepath = Directory.GetCurrentDirectory();//to get current absolute path
                //var filePath = Path.Combine(absolutepath + "\\wwwroot\\FileStorage\\Storage\\" + SavePath);
                var filePath = Path.Combine($"{absolutepath}/{SystemInfo.PathFileManager}/{SavePath}", fileName);
                string filePathThumb = Path.Combine($"{absolutepath}/{SystemInfo.PathFileManager}/{SavePath}", fileName);
                var ext = File.FileName.GetExtension();
                if (ext == "svg")
                {
                    SaveSvg(File, filePathThumb);
                    SaveSvg(File, filePathThumb);
                }
                else if (ext == "webp")
                {
                    SaveWebp(File, SavePath, height, width);
                    //SaveWebp(imageInfo.File, filePathThumb, imageInfo.height,  width);
                }
                else
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        File.CopyTo(memoryStream);
                        using (var img = System.Drawing.Image.FromStream(memoryStream))
                        {

                            System.Drawing.Image saveImg = img.Resize(img.Width, img.Height);

                            SaveThumbImage(img, filePathThumb);


                        }
                    }
                }
                if (System.IO.File.Exists(filePath))
                {
                    return fileName;

                }
                return fileName;
            }
            catch (Exception ex)
            {
                return String.Empty;
            }
        }
        public void SaveSvg(IFormFile svgFile, string filePathName)
        {
            try
            {
                if (svgFile != null && svgFile.Length > 0)
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        using (Stream fileStream = svgFile.OpenReadStream())
                        {
                            fileStream.CopyTo(memoryStream);
                        }

                        memoryStream.Position = 0;

                        var svgDocument = SvgDocument.Open<SvgDocument>(memoryStream);

                        // Convert SVG to bitmap
                        var bitmap = svgDocument.Draw();
                        bitmap.Save(filePathName);
                    }
                }

            }
            catch (Exception ex)
            {


            }

        }
        public void SaveWebp(IFormFile webpFile, string pathFileName, int height, int width)
        {
            try
            {
                using (var memoryStream = webpFile.OpenReadStream())
                {

                    var webpEncoder = new WebPFormat();

                    using (var imageFactory = new ImageFactory())
                    {
                        if (height > 0 && width > 0)
                        {
                            imageFactory.Load(memoryStream)
                           .Format(webpEncoder)
                           .Resize(new Size(width, height))
                           .Save(pathFileName);
                        }
                        else
                        {
                            imageFactory.Load(memoryStream)
                           .Format(webpEncoder)
                           .Save(pathFileName);

                        }

                    }
                }

            }
            catch (Exception ex)
            {


            }

        }

        public string UploadImageNotResize(IFormFile file, string dirPath)
        {
            try
            {
                var absolutepath = Directory.GetCurrentDirectory();
                var filePath = Path.Combine(absolutepath + "\\wwwroot" + dirPath);

                string filePathName = Path.Combine(filePath, file.FileName);
                var ext = file.FileName.GetExtension();
                if (ext == "svg")
                {
                    SaveSvg(file, filePathName);
                }
                else if (ext == "webp")
                {
                    SaveWebp(file, filePathName, 0, 0);
                }
                else
                {

                    using (var memoryStream = new MemoryStream())
                    {
                        file.CopyTo(memoryStream);

                        using (var img = System.Drawing.Image.FromStream(memoryStream))
                        {
                            System.Drawing.Image saveImg = img;
                            saveImg.Save(filePathName);
                        }


                    }
                }
                return file.FileName;

            }
            catch (Exception ex)
            {
                return String.Empty;
            }
        }
        public ResponeSaveFile UploadImagesBase64(UploadImageBase64Info imageinfo)
        {
            try
            {
                var absolutepath = Directory.GetCurrentDirectory();
                var filePath = Path.Combine(absolutepath + "\\wwwroot\\" + imageinfo.Path);
                var fileName = imageinfo.FileName + "-" + string.Empty.ToUniqeID() + ".jpeg";

                string imagesUpload = Path.Combine(filePath, fileName);

                string base64 = imageinfo.Base64.Split(',')[1];
                byte[] bytes = Convert.FromBase64String(base64);

                using (System.Drawing.Image image = System.Drawing.Image.FromStream(new MemoryStream(bytes)))
                {

                    image.Resize(imageinfo.Width, imageinfo.Height);


                    image.Save(imagesUpload, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
                if (System.IO.File.Exists(imagesUpload))
                {
                    return new ResponeSaveFile { isError = false, MessError = "", FileName = fileName };

                }

                return new ResponeSaveFile { isError = true, MessError = "" };
            }
            catch (Exception ex)
            {

                return new ResponeSaveFile { isError = true, MessError = "" };
            }
        }
        public void SaveThumbImage(System.Drawing.Image img, string path)
        {
            try
            {
                System.Drawing.Image saveImg = img.Resize(122, 91);
                saveImg.Save(path);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public ResponeSaveFile UploadImageModule(UploadImageInfo imageInfo)
        {
            try
            {
                var absolutepath = imageInfo.Absolutepath;//Directory.GetCurrentDirectory();//to get current absolute path
                var filePath = Path.Combine(absolutepath + "\\wwwroot\\" + imageInfo.Path);
                var fileName = Path.GetFileNameWithoutExtension(imageInfo.File.FileName) + "-" + string.Empty.ToUniqeID() + Path.GetExtension(imageInfo.File.FileName);
                string filePathThumb = Path.Combine(Path.Combine(absolutepath + "\\wwwroot\\" + imageInfo.PathThumb) + fileName);
                string filePathFull = Path.Combine(filePath + fileName);
                var ext = imageInfo.File.FileName.GetExtension();
                if (ext == "svg")
                {
                    SaveSvg(imageInfo.File, filePathFull);
                    SaveSvg(imageInfo.File, filePathThumb);
                }
                else if (ext == "webp")
                {
                    SaveWebp(imageInfo.File, filePathFull, imageInfo.Height, imageInfo.Width);
                    //SaveWebp(imageInfo.File, filePathThumb, imageInfo.height,  width);
                }
                else
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        imageInfo.File.CopyTo(memoryStream);
                        using (var img = System.Drawing.Image.FromStream(memoryStream))
                        {


                            if (img.Width > imageInfo.Width)
                            {
                                System.Drawing.Image saveImg = img.ResizeWidth(imageInfo.Width);
                                saveImg.Save(filePathFull);
                            }
                            else
                            {
                                System.Drawing.Image saveImg = img.Resize(img.Width, img.Height);
                                saveImg.Save(filePathFull);
                            }
                            SaveThumbImage(img, filePathThumb);


                        }
                    }
                }
                if (System.IO.File.Exists(filePathFull))
                {
                    return new ResponeSaveFile { isError = false, MessError = "", FileName = fileName };

                }
                return new ResponeSaveFile { isError = true, MessError = "" };


            }
            catch (Exception ex)
            {
                return new ResponeSaveFile { isError = true, MessError = ex.ToString(), FileName = "" };

            }
        }
        public ResponeSaveFile UploadFileModule(UploadFileInfo fileInfo)
        {
            try
            {
                var absolutepath = Directory.GetCurrentDirectory();//to get current absolute path
                var filePath = Path.Combine(absolutepath + "\\wwwroot\\" + fileInfo.Path);
                var fileName = Path.GetFileNameWithoutExtension(fileInfo.File.FileName) + "-" + string.Empty.ToUniqeID() + Path.GetExtension(fileInfo.File.FileName);
                string filePathFull = Path.Combine(filePath, fileName);
                using (var memoryStream = new MemoryStream())
                {
                    fileInfo.File.CopyTo(memoryStream);
                    //System.IO.File.Create(filePathFull, memoryStream);
                    //memoryStream.Sav
                }
                if (System.IO.File.Exists(filePathFull))
                {
                    return new ResponeSaveFile { isError = false, MessError = "", FileName = fileName };

                }
                return new ResponeSaveFile { isError = true, MessError = "" };


            }
            catch (Exception ex)
            {
                return new ResponeSaveFile { isError = true, MessError = ex.ToString(), FileName = "" };

            }
        }
        public ResponeSaveFile UploadImage(IFormFile file)
        {
            try
            {
                int? width = 400; int? height = 300;

                var absolutepath = Directory.GetCurrentDirectory();//to get current absolute path
                var filePath = Path.Combine(absolutepath + "\\wwwroot\\FileStorage\\Storage");
                var filePathThumb = Path.Combine(absolutepath + "\\wwwroot\\FileStorage\\thumbs");

                string filePathName = Path.Combine(filePath, file.FileName);
                using (var memoryStream = new MemoryStream())
                {
                    file.CopyTo(memoryStream);
                    using (var img = System.Drawing.Image.FromStream(memoryStream))
                    {
                        System.Drawing.Image saveImg = img.Resize(width.Value, height.Value);
                        saveImg.Save(filePathName);
                        SaveThumbImage(img, filePathThumb);

                    }
                }
                if (System.IO.File.Exists(filePathName))
                {
                    return new ResponeSaveFile { isError = false, MessError = "", FileName = file.FileName };

                }
                return new ResponeSaveFile { isError = true, MessError = "" };

            }
            catch (Exception ex)
            {
                return new ResponeSaveFile { isError = true, MessError = ex.ToString(), FileName = "" };
            }
        }

        public ResponeSaveFile UploadImageNotResize(UploadImageInfo imageInfo)
        {
            try
            {
                var absolutepath = Directory.GetCurrentDirectory();
                var filePath = Path.Combine(absolutepath + "\\wwwroot" + imageInfo.Path);
                var filePathThumb = Path.Combine(absolutepath + "\\wwwroot" + imageInfo.PathThumb);

                string filePathName = Path.Combine(filePath, imageInfo.File.FileName);
                using (var memoryStream = new MemoryStream())
                {
                    imageInfo.File.CopyTo(memoryStream);
                    using (var img = System.Drawing.Image.FromStream(memoryStream))
                    {
                        System.Drawing.Image saveImg = img;
                        saveImg.Save(filePathName);
                        SaveThumbImage(img, filePathThumb);

                    }
                }
                if (System.IO.File.Exists(filePathName))
                {
                    return new ResponeSaveFile { isError = false, MessError = "", FileName = imageInfo.File.FileName };

                }
                return new ResponeSaveFile { isError = true, MessError = "" };

            }
            catch (Exception ex)
            {
                return new ResponeSaveFile { isError = true, MessError = ex.ToString(), FileName = "" };
            }
        }
        public void DeleteFile(string dirPath, string fileName)
        {
            var absolutepath = Directory.GetCurrentDirectory();//to get current absolute path
            var filePath = Path.Combine(absolutepath + "\\wwwroot\\" + dirPath + fileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
        public void CreateFolder(string pathFolder)
        {
            var absolutePath = Directory.GetCurrentDirectory();
            // Determine whether the directory exists.
            if (Directory.Exists(absolutePath + pathFolder))
            {
                Console.WriteLine("That path exists already.");
            }
            else
            {
                Directory.CreateDirectory(absolutePath + pathFolder);

            }
        }
        public async Task<string> UploadImagesBase64ToServer(UploadImageBase64Info imageinfo)
        {
            string responseObj = "";
            try
            {

                var client = new RestClient(ApiUploadBase64);
                var request = new RestRequest();
                request.AddHeader("AccessKey", AccessKey);
                request.AddJsonBody(JsonConvert.SerializeObject((object)imageinfo));
                var response = await client.ExecutePostAsync(request);
                if (response.IsSuccessful)
                {
                    return response.Content.ToString();

                }
                else
                {
                    return responseObj = "-1";
                }
            }
            catch (Exception ex)
            {

                return "-1";
            }
        }
        public async Task<string> UploadImageToServer(UploadImageInfo model)
        {
            using (HttpClient httpClient = new HttpClient())
            using (MultipartFormDataContent form = new MultipartFormDataContent())
            {
                form.Add(new StringContent(model.Height.ToString()), "Height");
                form.Add(new StringContent(model.Width.ToString()), "Width");
                form.Add(new StringContent(model.FileName), "FileName");
                form.Add(new StringContent(model.Path), "Path");
                form.Add(new StringContent(model.PathThumb), "PathThumb");
                string contentType = model.File.ContentType;

                form.Add(new StreamContent(model.File.OpenReadStream())
                {
                    Headers =
                {
                    ContentLength = model.File.Length,
                      ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType)                }
                }, "File", model.File.FileName); ;

                HttpResponseMessage response = await httpClient.PostAsync(ApiUpload, form);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    return "-1";
                }
            }
        }
    }
}
