using LazZiya.ImageResize;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DTO;
using CMS.Services.CommonServices;
using DTO.Common;

namespace CMS.Services.FileServices
{
    public class FileServices : IFileServices
    {
        private string minW = "";
        private string maxW = "";
        private string fu = "";
        private string ipre = "";
        private string wtmi = "";
        private string wtmt = "";
        private string wtmp = "";
        private string WatermarkPicThumbActive = "";

        private readonly ICommonServices _common;

        private string Thumb = ConstantStrings.Thumb;
        private string Fullmages = ConstantStrings.Fullmages;
        private string UrlConfigurationImages = ConstantStrings.UrlConfigurationImages;
        public FileServices(ICommonServices common)
        {
            _common = common;
            wtmi = _common.GetConfigValue(ConstantStrings.KeyWatermarkImage);
            wtmt = _common.GetConfigValue(ConstantStrings.KeyWatermarkText);
            ipre = _common.GetConfigValue(ConstantStrings.KeyImagePrefix);
            wtmp = _common.GetConfigValue(ConstantStrings.KeyWatermarkPosition);
            minW = _common.GetConfigValue(ConstantStrings.KeyImageMinWidth);
            maxW = _common.GetConfigValue(ConstantStrings.KeyImageMaxWidth);
            WatermarkPicThumbActive = _common.GetConfigValue(ConstantStrings.KeyWatermarkPicThumbActive);
        }
        public dynamic SaveFileAvatar(IFormFile file, string url, string title)
        {
            try
            {
                var id = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() +
                         DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() +
                         DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
                var entend = file.FileName.Split('.');
                string preFix = ipre;
                var filename = preFix + _common.EncodeTitle(title) + "-" + id + "." + entend[1].ToString();
                var filenameRs = preFix + _common.EncodeTitle(title) + "-" + id + "." + entend[1].ToString();
                var absolutepath = Directory.GetCurrentDirectory();//to get current absolute path
                var kt = 0;
                var filePath = Path.Combine(absolutepath + "\\wwwroot\\" + url, filename);
                decimal height = 0;
                decimal width = 0;
                using (var image = Image.FromStream(file.OpenReadStream()))
                {
                    decimal wPerh = Convert.ToDecimal(image.Width) / Convert.ToDecimal(image.Height);
                    height = image.Height;
                    width = image.Width;

                    if (image.Width > 200)
                    {
                        height = 200 / wPerh;
                        width = 200;
                        kt = 1;
                    }
                    else if (image.Width < 200)
                    {
                        height = 200 / wPerh;
                        width = 200;
                        kt = 1;
                    }
                }

                if (kt == 0)
                {
                    using (var fileStream = new FileStream(filePath, FileMode.OpenOrCreate))
                    {
                        file.CopyTo(fileStream);
                        fileStream.Dispose();
                    }
                }
                else
                {
                    using (var stream = file.OpenReadStream())
                    {

                        var uploadedImage = Image.FromStream(stream);
                        //var img = ImageResize.Scale(uploadedImage, Convert.ToInt32(width), Convert.ToInt32(height));
                        var img = ImageResize.ScaleByWidth(uploadedImage, Convert.ToInt32(width));
                        //img.SaveAs(filePath);
                        Bitmap newBitmap = new Bitmap(img);
                        stream.Dispose();
                        img.Dispose();
                        newBitmap.Save(filePath, System.Drawing.Imaging.ImageFormat.Jpeg);

                    }
                }
                if (System.IO.File.Exists(filePath))
                {
                    return new { isError = false, messError = "", fileName = filenameRs };
                }
                else
                {
                    return new { isError = true, messError = "", fileName = "" };
                }

            }
            catch (Exception ex)
            {

                return new { isError = true, messError = "", fileName = "" };
            }
        }
        public dynamic SaveFileNoBackground(IFormFile file, string url, string title)
        {

            var id = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() +
                        DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() +
                        DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
            var entend = file.FileName.Split('.');
            string preFix = ipre;
            var filename = preFix + _common.EncodeTitle(title) + "-" + id + "." + entend[1].ToString();
            var filenameRs = preFix + _common.EncodeTitle(title) + "-" + id + "." + entend[1].ToString();
            var absolutepath = Directory.GetCurrentDirectory();//to get current absolute path
            var kt = 0;
            var filePath = Path.Combine(absolutepath + "\\wwwroot\\" + url, Fullmages + filename);
            decimal height = 0;
            decimal width = 0;

            try
            {
                using (var image = Image.FromStream(file.OpenReadStream()))
                {
                    decimal wPerh = Convert.ToDecimal(image.Width) / Convert.ToDecimal(image.Height);
                    height = image.Height;
                    width = image.Width;

                    if (image.Width > Convert.ToInt32(maxW))
                    {
                        height = Convert.ToDecimal(maxW) / wPerh;
                        width = Convert.ToDecimal(maxW);
                        kt = 1;
                    }
                    else if (image.Width < Convert.ToInt32(minW))
                    {
                        height = Convert.ToDecimal(minW) / wPerh;
                        width = Convert.ToInt32(minW);
                        kt = 1;
                    }
                }

                if (kt == 0)
                {
                    using (var stream = file.OpenReadStream())
                    {
                        var uploadedImage = Image.FromStream(stream);
                        Bitmap newBitmap = new Bitmap(uploadedImage);
                        newBitmap.MakeTransparent(Color.White);
                        newBitmap.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
                        uploadedImage.Dispose();
                        stream.Dispose();
                    }
                }
                else
                {
                    using (var stream = file.OpenReadStream())
                    {

                        var uploadedImage = Image.FromStream(stream);
                        var img = ImageResize.ScaleByWidth(uploadedImage, Convert.ToInt32(width));
                        Bitmap newBitmap = new Bitmap(img);
                        newBitmap.MakeTransparent(Color.White);
                        stream.Dispose();
                        img.Dispose();
                        newBitmap.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);

                    }
                }
                if (System.IO.File.Exists(filePath))
                {
                    return new { isError = false, messError = "", fileName = filenameRs };
                }
                else
                {
                    return new { isError = true, messError = "", fileName = "" };
                }
            }
            catch (Exception ex)
            {
                if (ex.Message == "A Graphics object cannot be created from an image that has an indexed pixel format. (Parameter 'image')")
                {
                    using (var stream = file.OpenReadStream())
                    {

                        var uploadedImage = Image.FromStream(stream);
                        Bitmap newBitmap = new Bitmap(uploadedImage);
                        stream.Dispose();
                        newBitmap.MakeTransparent(Color.White);
                        newBitmap.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
                    }

                    if (System.IO.File.Exists(filePath))
                    {

                        return new { isError = false, messError = "", fileName = filenameRs };
                    }
                    else
                    {
                        return new { isError = true, messError = "", fileName = "" };
                    }
                }
                else
                {
                    return new { isError = true, messError = "", fileName = "" };

                }
            }
        }

        public dynamic SaveFile(IFormFile file, string url, string title)
        {
            try
            {
                var id = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() +
                         DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() +
                         DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
                //var a = _httpContextAccessor.HttpContext.Session.GetString("Role").ToString();
                var entend = file.FileName.Split('.');
                string preFix = ipre;
                var filename = preFix + _common.EncodeTitle(title) + "-" + id + "." + entend[1].ToString();
                var filenameRs = preFix + _common.EncodeTitle(title) + "-" + id + "." + entend[1].ToString();
                var absolutepath = Directory.GetCurrentDirectory();//to get current absolute path
                //var kt = 0;
                var filePath = Path.Combine(absolutepath + "\\wwwroot\\" + url, Fullmages + filename);
                decimal height = 0;
                decimal width = 0;
                using (var image = Image.FromStream(file.OpenReadStream()))
                {
                    decimal wPerh = Convert.ToDecimal(image.Width) / Convert.ToDecimal(image.Height);
                    height = image.Height;
                    width = image.Width;

                    if (image.Width > Convert.ToInt32(maxW))
                    {
                        height = Convert.ToDecimal(maxW) / wPerh;
                        width = Convert.ToDecimal(maxW);
                        //kt = 1;
                    }
                    else if (image.Width < Convert.ToInt32(minW))
                    {
                        height = Convert.ToDecimal(minW) / wPerh;
                        width = Convert.ToInt32(minW);
                        //kt = 1;
                    }
                }
                using (var stream = file.OpenReadStream())
                {
                    var uploadedImage = Image.FromStream(stream);

                    // Calculate the new width and height based on the desired width (maxW)
                    int newWidth = int.Parse(maxW);
                    int newHeight = Convert.ToInt32(newWidth * uploadedImage.Height / uploadedImage.Width);

                    // Create a new Bitmap with the calculated dimensions and non-indexed format
                    var newBitmap = new Bitmap(newWidth, newHeight, PixelFormat.Format32bppArgb);

                    // Create a Graphics object from the new Bitmap and draw the uploadedImage on it
                    using (var graphics = Graphics.FromImage(newBitmap))
                    {
                        graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        graphics.DrawImage(uploadedImage, 0, 0, newWidth, newHeight);
                    }

                    // Dispose the original uploadedImage as it's no longer needed
                    uploadedImage.Dispose();

                    // Save the resized image to the file
                    newBitmap.Save(filePath, ImageFormat.Jpeg);
                    newBitmap.Dispose();
                }
                if (System.IO.File.Exists(filePath))
                {
                    return new { isError = false, messError = "", fileName = filenameRs };
                }
                else
                {
                    return new { isError = true, messError = "", fileName = "" };
                }

            }
            catch (Exception ex)
            {

                return new { isError = true, messError = "", fileName = "" };
            }
        }
        public dynamic SaveFileNotRename(IFormFile file, string url, string title)
        {
            try
            {
                //var id = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() +
                //         DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() +
                //         DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
                //var a = _httpContextAccessor.HttpContext.Session.GetString("Role").ToString();
                var entend = file.FileName.Split('.');
                //string preFix = ipre;
                var filename = _common.EncodeTitle(title) + "." + entend[1].ToString();
                var filenameRs = _common.EncodeTitle(title) + "." + entend[1].ToString();
                var absolutepath = Directory.GetCurrentDirectory();//to get current absolute path
                var kt = 0;
                var filePath = Path.Combine(absolutepath + "\\wwwroot\\" + url + filename);
                decimal height = 0;
                decimal width = 0;
                using (var image = Image.FromStream(file.OpenReadStream()))
                {
                    decimal wPerh = Convert.ToDecimal(image.Width) / Convert.ToDecimal(image.Height);
                    height = image.Height;
                    width = image.Width;

                    if (image.Width > Convert.ToInt32(maxW))
                    {
                        height = Convert.ToDecimal(maxW) / wPerh;
                        width = Convert.ToDecimal(maxW);
                        kt = 1;
                    }
                    else if (image.Width < Convert.ToInt32(minW))
                    {
                        height = Convert.ToDecimal(minW) / wPerh;
                        width = Convert.ToInt32(minW);
                        kt = 1;
                    }
                }
                //if (kt == 0)
                //{
                //    using (var fileStream = new FileStream(filePath, FileMode.OpenOrCreate))
                //    {
                //        file.CopyTo(fileStream);
                //        fileStream.Dispose();
                //    }
                //}
                //else
                //{
                using (var stream = file.OpenReadStream())
                {
                    var uploadedImage = Image.FromStream(stream);

                    // Calculate the new width and height based on the desired width (maxW)
                    int newWidth = Convert.ToInt32(width);
                    int newHeight = Convert.ToInt32(height);

                    // Create a new Bitmap with the calculated dimensions and non-indexed format
                    var newBitmap = new Bitmap(newWidth, newHeight, PixelFormat.Format32bppArgb);

                    // Create a Graphics object from the new Bitmap and draw the uploadedImage on it
                    using (var graphics = Graphics.FromImage(newBitmap))
                    {
                        graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        graphics.DrawImage(uploadedImage, 0, 0, newWidth, newHeight);
                    }

                    // Dispose the original uploadedImage as it's no longer needed
                    uploadedImage.Dispose();

                    // Save the resized image to the file
                    newBitmap.Save(filePath, ImageFormat.Jpeg);
                    newBitmap.Dispose();
                }
                //}
                if (System.IO.File.Exists(filePath))
                {
                    return new { isError = false, messError = "", fileName = filenameRs };
                }
                else
                {
                    return new { isError = true, messError = "", fileName = "" };
                }

            }
            catch (Exception ex)
            {

                return new { isError = true, messError = "", fileName = "" };
            }
        }
        public dynamic SaveFileWithWatermark(IFormFile file, string url, string title)
        {
            try
            {
                var id = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() +
                         DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() +
                         DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
                //var a = _httpContextAccessor.HttpContext.Session.GetString("Role").ToString();
                var entend = file.FileName.Split('.');
                string preFix = ipre;
                var filename = preFix + _common.EncodeTitle(title) + "-" + id + "." + entend[1].ToString();
                var filenameRs = preFix + _common.EncodeTitle(title) + "-" + id + "." + entend[1].ToString();
                var absolutepath = Directory.GetCurrentDirectory();//to get current absolute path
                var kt = 0;
                var filePath = Path.Combine(absolutepath + "\\wwwroot\\" + url, Fullmages + filename);
                decimal height = 0;
                decimal width = 0;
                using (var image = Image.FromStream(file.OpenReadStream()))
                {
                    decimal wPerh = Convert.ToDecimal(image.Width) / Convert.ToDecimal(image.Height);
                    height = image.Height;
                    width = image.Width;

                    if (image.Width > Convert.ToInt32(maxW))
                    {
                        height = Convert.ToDecimal(maxW) / wPerh;
                        width = Convert.ToDecimal(maxW);
                        kt = 1;
                    }
                    else if (image.Width < Convert.ToInt32(minW))
                    {
                        height = Convert.ToDecimal(minW) / wPerh;
                        width = Convert.ToInt32(minW);
                        kt = 1;
                    }
                }
                if (kt == 0)
                {
                    using var ms = new MemoryStream();
                    file.CopyTo(ms);
                    byte[] byteImage = ms.ToArray();
                    string SigBase64 = Convert.ToBase64String(byteImage);
                    CreateWatermarkImage(SigBase64, filePath);
                }
                else
                {
                    using var stream = file.OpenReadStream();
                    var uploadedImage = Image.FromStream(stream);

                    int newWidth = int.Parse(maxW);
                    int newHeight = Convert.ToInt32(newWidth * uploadedImage.Height / uploadedImage.Width);

                    var newBitmap = new Bitmap(newWidth, newHeight, PixelFormat.Format32bppArgb);

                    using (var graphics = Graphics.FromImage(newBitmap))
                    {
                        graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        graphics.DrawImage(uploadedImage, 0, 0, newWidth, newHeight);
                    }
                    uploadedImage.Dispose();

                    using MemoryStream ms = new();
                    newBitmap.Save(ms, ImageFormat.Jpeg);
                    newBitmap.Dispose();

                    byte[] byteImage = ms.ToArray();
                    string SigBase64 = Convert.ToBase64String(byteImage);

                    CreateWatermarkImage(SigBase64, filePath);

                }
                if (File.Exists(filePath))
                {
                    return new { isError = false, messError = "", fileName = filenameRs };
                }
                else
                {
                    return new { isError = true, messError = "", fileName = "" };
                }

            }
            catch (Exception ex)
            {

                return new { isError = true, messError = "", fileName = "" };
            }
        }
        public dynamic SaveFileNotResize(IFormFile file, string url, string title)
        {
            try
            {
                var id = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() +
                         DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() +
                         DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
                //var a = _httpContextAccessor.HttpContext.Session.GetString("Role").ToString();
                var entend = file.FileName.Split('.');
                string preFix = ipre;
                var filename = Fullmages + preFix + _common.EncodeTitle(title) + "-" + id + "." + entend[1].ToString();
                var filenameRs = preFix + _common.EncodeTitle(title) + "-" + id + "." + entend[1].ToString();
                var absolutepath = Directory.GetCurrentDirectory();//to get current absolute path
                var filePath = Path.Combine(absolutepath + "\\wwwroot\\" + url, filename);
                using (var fileStream = new FileStream(filePath, FileMode.OpenOrCreate))
                {
                    file.CopyTo(fileStream);
                    fileStream.Dispose();
                }
                if (System.IO.File.Exists(filePath))
                {
                    return new { isError = false, messError = "", fileName = filenameRs };
                }
                else
                {
                    return new { isError = true, messError = "", fileName = "" };
                }

            }
            catch (Exception ex)
            {

                return new { isError = true, messError = "", fileName = "" };
            }
        }
        public dynamic SaveFileNotResizeNotRename(IFormFile file, string url, string title)
        {
            try
            {
                //var id = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() +
                //         DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() +
                //         DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
                //var a = _httpContextAccessor.HttpContext.Session.GetString("Role").ToString();
                var entend = file.FileName.Split('.');
                //string preFix = ipre;
                var filename = _common.EncodeTitle(title) + "." + entend[1].ToString();
                var filenameRs = _common.EncodeTitle(title) + "." + entend[1].ToString();
                var absolutepath = Directory.GetCurrentDirectory();//to get current absolute path
                var filePath = Path.Combine(absolutepath + "\\wwwroot\\" + url, filename);
                using (var fileStream = new FileStream(filePath, FileMode.OpenOrCreate))
                {
                    file.CopyTo(fileStream);
                    fileStream.Dispose();
                }
                if (System.IO.File.Exists(filePath))
                {
                    return new { isError = false, messError = "", fileName = filenameRs };
                }
                else
                {
                    return new { isError = true, messError = "", fileName = "" };
                }

            }
            catch (Exception ex)
            {

                return new { isError = true, messError = "", fileName = "" };
            }
        }
        public dynamic SaveFileNotResizeWidth(IFormFile file, string url, string title, int width)
        {
            try
            {
                var id = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() +
                         DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() +
                         DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
                var entend = file.FileName.Split('.');
                string preFix = ipre;
                var filename = Fullmages + preFix + _common.EncodeTitle(title) + "-" + id + "." + entend[1].ToString();
                var filenameRs = preFix + _common.EncodeTitle(title) + "-" + id + "." + entend[1].ToString();
                var absolutepath = Directory.GetCurrentDirectory();//to get current absolute path
                var filePath = Path.Combine(absolutepath + "\\wwwroot\\" + url, filename);

                using (var stream = file.OpenReadStream())
                {
                    var uploadedImage = Image.FromStream(stream);
                    Image img;
                    if (uploadedImage.Width > width)
                    {
                        img = ImageResize.ScaleByWidth(uploadedImage, Convert.ToInt32(width));
                    }
                    else
                    {
                        img = uploadedImage;
                    }
                    Bitmap newBitmap = new Bitmap(img);
                    stream.Dispose();
                    img.Dispose();
                    newBitmap.Save(filePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                    //newBitmap.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
                }

                if (System.IO.File.Exists(filePath))
                {
                    return new { isError = false, messError = "", fileName = filenameRs };
                }
                else
                {
                    return new { isError = true, messError = "", fileName = "" };
                }

            }
            catch (Exception ex)
            {

                return new { isError = true, messError = "", fileName = "" };
            }
        }

        //Lưu PicThumb của Homepage Intro
        public dynamic SaveFileNotResizeWidthNoBackground(IFormFile file, string url, string title, int width)
        {
            try
            {
                var id = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() +
                         DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() +
                         DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
                var entend = file.FileName.Split('.');
                string preFix = ipre;
                var filename = Fullmages + preFix + _common.EncodeTitle(title) + "-" + id + "." + entend[1].ToString();
                var filenameRs = preFix + _common.EncodeTitle(title) + "-" + id + "." + entend[1].ToString();
                var absolutepath = Directory.GetCurrentDirectory();//to get current absolute path
                var filePath = Path.Combine(absolutepath + "\\wwwroot\\" + url, filename);

                using (var stream = file.OpenReadStream())
                {
                    var uploadedImage = Image.FromStream(stream);
                    Image img;
                    if (uploadedImage.Width > width)
                    {
                        img = ImageResize.ScaleByWidth(uploadedImage, Convert.ToInt32(width));
                    }
                    else
                    {
                        img = uploadedImage;
                    }
                    Bitmap newBitmap = new Bitmap(img);
                    stream.Dispose();
                    img.Dispose();
                    //newBitmap.Save(filePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                    newBitmap.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
                }

                if (System.IO.File.Exists(filePath))
                {
                    return new { isError = false, messError = "", fileName = filenameRs };
                }
                else
                {
                    return new { isError = true, messError = "", fileName = "" };
                }

            }
            catch (Exception ex)
            {

                return new { isError = true, messError = "", fileName = "" };
            }
        }
        public dynamic SaveFileOriginal(IFormFile file, string url, string filename)
        {
            try
            {
                var id = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() +
                         DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() +
                         DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
                string ext = Path.GetExtension(file.FileName);
                filename = _common.EncodeTitle(filename) + id + ext;
                var absolutepath = Directory.GetCurrentDirectory();//to get current absolute path
                var filePath = Path.Combine(absolutepath + "\\wwwroot\\" + url, filename);
                using (var fileStream = new FileStream(filePath, FileMode.OpenOrCreate))
                {
                    file.CopyTo(fileStream);
                    fileStream.Dispose();
                }
                if (System.IO.File.Exists(filePath))
                {
                    return new { isError = false, messError = "", fileName = filename };
                }
                else
                {
                    return new { isError = true, messError = "", fileName = "" };
                }

            }
            catch (Exception ex)
            {

                return new { isError = true, messError = "", fileName = "" };
            }

        }
        public dynamic SaveCategoryImage(IFormFile file, string url, string filename)
        {
            try
            {
                var id = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() +
                         DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() +
                         DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();

                string ext = Path.GetExtension(file.FileName);
                filename = _common.EncodeTitle(filename) + id + ext;

                var absolutepath = Directory.GetCurrentDirectory();//to get current absolute path
                var filePath = Path.Combine(absolutepath + "\\wwwroot\\" + url, filename);

                using (var stream = file.OpenReadStream())
                {
                    var uploadedImage = Image.FromStream(stream);
                    if (uploadedImage.Width > Convert.ToInt32(minW))
                    {
                        var img = ImageResize.ScaleByWidth(uploadedImage, Convert.ToInt32(minW));
                        Bitmap newBitmap = new Bitmap(img);
                        stream.Dispose();
                        img.Dispose();
                        newBitmap.MakeTransparent(Color.White);
                        newBitmap.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
                    }
                    else
                    {
                        Bitmap newBitmap = new Bitmap(uploadedImage);
                        stream.Dispose();
                        uploadedImage.Dispose();
                        newBitmap.MakeTransparent(Color.White);
                        newBitmap.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
                    }
                }

                if (System.IO.File.Exists(filePath))
                {
                    return new { isError = false, messError = "", fileName = filename };
                }
                else
                {
                    return new { isError = true, messError = "", fileName = "" };
                }

            }
            catch (Exception ex)
            {

                return new { isError = true, messError = "", fileName = "" };
            }

        }
        public dynamic UpdateFile(IFormFile file, string url, string title)
        {
            try
            {
                var filename = title;
                var absolutepath = Directory.GetCurrentDirectory();//to get current absolute path
                var kt = 0;
                var filePath = Path.Combine(absolutepath + "\\wwwroot\\" + url, filename);
                decimal height = 0;
                decimal width = 0;
                using (var image = Image.FromStream(file.OpenReadStream()))
                {
                    decimal wPerh = Convert.ToDecimal(image.Width) / Convert.ToDecimal(image.Height);
                    height = image.Height;
                    width = image.Width;

                    if (image.Width > Convert.ToInt32(maxW))
                    {
                        height = Convert.ToDecimal(maxW) / wPerh;
                        width = Convert.ToDecimal(maxW);
                        kt = 1;
                    }
                    else if (image.Width < Convert.ToInt32(minW))
                    {
                        height = Convert.ToDecimal(minW) / wPerh;
                        width = Convert.ToInt32(minW);
                        kt = 1;
                    }
                }
                if (kt == 0)
                {
                    using (var fileStream = new FileStream(filePath, FileMode.OpenOrCreate))
                    {
                        file.CopyTo(fileStream);
                    }
                }
                else
                {
                    using (var stream = file.OpenReadStream())
                    {

                        var uploadedImage = Image.FromStream(stream);
                        //var img = ImageResize.Scale(uploadedImage, Convert.ToInt32(width), Convert.ToInt32(height));
                        var img = ImageResize.ScaleByWidth(uploadedImage, Convert.ToInt32(width));
                        img.SaveAs(filePath);
                    }
                }
                if (System.IO.File.Exists(filePath))
                {
                    return new { isError = false, messError = "", fileName = title };
                }
                else
                {
                    return new { isError = true, messError = "", fileName = "" };
                }

            }
            catch (Exception ex)
            {

                return new { isError = true, messError = "", fileName = "" };
            }
        }
        public dynamic DeleteFile(string url, string fileName)
        {
            try
            {

                var absolutepath = Directory.GetCurrentDirectory();//to get current absolute path

                var filePath = Path.Combine(absolutepath + "\\wwwroot\\" + url, fileName);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                if (!File.Exists(filePath))
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
        public void ResizeThumbImageNoBackground(IFormFile Images, string url, string title)
        {
            try
            {

                using (Image image = Image.FromStream(Images.OpenReadStream()))
                {
                    decimal wPerh = Convert.ToDecimal(image.Width) / Convert.ToDecimal(image.Height);
                    decimal height = Convert.ToDecimal(minW) / wPerh;
                    using (var stream = Images.OpenReadStream())
                    {
                        var filename = title;

                        var uploadedImage = Image.FromStream(stream);
                        var absolutepath = Directory.GetCurrentDirectory();//to get current absolute path
                        //returns Image file
                        //var img = ImageResize.Scale(uploadedImage, Convert.ToInt32(ImageMinWidth), Convert.ToInt32(height));
                        Image img = ImageResize.ScaleByWidth(uploadedImage, Convert.ToInt32(minW));

                        var filePath = Path.Combine(absolutepath + "\\wwwroot\\" + url, filename);
                        Bitmap newBitmap = new Bitmap(img);
                        newBitmap.MakeTransparent(Color.White);
                        stream.Dispose();
                        image.Dispose();
                        img.Dispose();

                        newBitmap.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
                    }
                }

            }
            catch (Exception ex)
            {
                if (ex.Message == "A Graphics object cannot be created from an image that has an indexed pixel format. (Parameter 'image')")
                {
                    var absolutepath = Directory.GetCurrentDirectory();//to get current absolute path

                    var filePath = Path.Combine(absolutepath + "\\wwwroot\\" + url, title);

                    using (var stream = Images.OpenReadStream())
                    {

                        var uploadedImage = Image.FromStream(stream);
                        Bitmap newBitmap = new Bitmap(uploadedImage);
                        stream.Dispose();
                        newBitmap.MakeTransparent(Color.White);
                        newBitmap.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
                    }
                }
            }

        }
        public void ResizeThumbImage(IFormFile Images, string url, string title)
        {
            try
            {

                using (Image image = Image.FromStream(Images.OpenReadStream()))
                {
                    decimal wPerh = Convert.ToDecimal(image.Width) / Convert.ToDecimal(image.Height);
                    int newWidth = int.Parse(minW);
                    int newHeight = Convert.ToInt32(newWidth / wPerh);

                    using (var stream = Images.OpenReadStream())
                    {
                        var filename = title;

                        var uploadedImage = Image.FromStream(stream);
                        var img = new Bitmap(uploadedImage, newWidth, newHeight);

                        var absolutepath = Directory.GetCurrentDirectory();
                        var filePath = Path.Combine(absolutepath + "\\wwwroot\\" + url, filename);
                        img.Save(filePath, ImageFormat.Jpeg);

                        stream.Dispose();
                        image.Dispose();
                        img.Dispose();
                    }
                }

            }
            catch (Exception ex)
            {


            }


        }
        public void ResizeThumbImageWithWatermark(IFormFile Images, string url, string title)
        {
            try
            {
                using Image image = Image.FromStream(Images.OpenReadStream());
                decimal wPerh = Convert.ToDecimal(image.Width) / Convert.ToDecimal(image.Height);
                int newWidth = int.Parse(minW);
                int newHeight = Convert.ToInt32(newWidth / wPerh);

                using var stream = Images.OpenReadStream();
                var filename = title;

                var uploadedImage = Image.FromStream(stream);
                var img = new Bitmap(uploadedImage, newWidth, newHeight);

                var absolutepath = Directory.GetCurrentDirectory();
                var filePath = Path.Combine(absolutepath + "\\wwwroot\\" + url, filename);

                stream.Dispose();
                image.Dispose();

                using MemoryStream ms = new();
                img.Save(ms, ImageFormat.Jpeg);
                img.Dispose();
                byte[] byteImage = ms.ToArray();
                string SigBase64 = Convert.ToBase64String(byteImage);

                CreateWatermarkImage(SigBase64, filePath);
            }
            catch (Exception)
            {

            }
        }
        public dynamic WatermarkImage(string url, string fileName)
        {
            try
            {
                var img = Image.FromFile("wwwroot" + url + fileName);

                var watermark = "wwwroot" + UrlConfigurationImages + wtmi;
                var iwm = Image.FromFile(watermark);

                if (wtmp == "left")
                {

                    var iwmOps = new ImageWatermarkOptions
                    {
                        Location = TargetSpot.BottomLeft,

                        // 0 full transparent, 100 full color
                        Opacity = 40,
                        Margin = 10
                    };
                    img.AddImageWatermark(iwm, iwmOps);
                }
                else if (wtmp == "center")
                {
                    var iwmOps = new ImageWatermarkOptions
                    {
                        Location = TargetSpot.Center,

                        // 0 full transparent, 100 full color
                        Opacity = 40,
                        Margin = 10
                    };
                    img.AddImageWatermark(iwm, iwmOps);
                }
                else if (wtmp == "right")
                {
                    var iwmOps = new ImageWatermarkOptions
                    {
                        Location = TargetSpot.BottomRight,

                        // 0 full transparent, 100 full color
                        Opacity = 40,
                        Margin = 10
                    };
                    img.AddImageWatermark(iwm, iwmOps);
                }
                iwm.Dispose();


                System.IO.MemoryStream ms = new MemoryStream();
                img.Save(ms, ImageFormat.Jpeg);
                byte[] byteImage = ms.ToArray();
                var SigBase64 = Convert.ToBase64String(byteImage); // Get Base64
                return SigBase64;
            }
            catch (Exception ex)
            {
                var absolutepath = Directory.GetCurrentDirectory();

                var filePath = Path.Combine(absolutepath + "\\wwwroot\\" + url, fileName);

                var base64 = Convert.ToBase64String(File.ReadAllBytes(filePath));
                return base64;
            }


        }
        public dynamic WatermarkImage(string base64)
        {
            try
            {
                byte[] bytes = Convert.FromBase64String(base64);
                Image img;
                using (MemoryStream ms1 = new(bytes))
                {
                    img = Image.FromStream(ms1);
                }
                var watermark = "wwwroot" + UrlConfigurationImages + wtmi;
                var iwm = Image.FromFile(watermark);
                string watermarkType = _common.GetConfigValue(ConstantStrings.KeyWatermarkType);
                if (watermarkType == "image")
                {
                    int opacity = Convert.ToInt32(_common.GetConfigValue(ConstantStrings.KeyWatermarkOpacity));
                    var iwmOps = new ImageWatermarkOptions
                    {
                        Opacity = opacity,
                        Margin = 10
                    };
                    if (wtmp == "topLeft")
                    {
                        iwmOps.Location = TargetSpot.TopLeft;
                    }
                    else if (wtmp == "topRight")
                    {
                        iwmOps.Location = TargetSpot.TopRight;
                    }
                    else if (wtmp == "center")
                    {
                        iwmOps.Location = TargetSpot.Center;
                    }
                    else if (wtmp == "bottomLeft")
                    {
                        iwmOps.Location = TargetSpot.BottomLeft;
                    }
                    else if (wtmp == "bottomRight")
                    {
                        iwmOps.Location = TargetSpot.BottomRight;
                    }
                    img.AddImageWatermark(iwm, iwmOps);
                }
                else if (watermarkType == "text")
                {
                    var iwmOps = new TextWatermarkOptions
                    {
                        Margin = 10
                    };
                    if (wtmp == "topLeft")
                    {
                        iwmOps.Location = TargetSpot.TopLeft;
                    }
                    else if (wtmp == "topRight")
                    {
                        iwmOps.Location = TargetSpot.TopRight;
                    }
                    else if (wtmp == "center")
                    {
                        iwmOps.Location = TargetSpot.Center;
                    }
                    else if (wtmp == "bottomLeft")
                    {
                        iwmOps.Location = TargetSpot.BottomLeft;
                    }
                    else if (wtmp == "bottomRight")
                    {
                        iwmOps.Location = TargetSpot.BottomRight;
                    }
                    img.AddTextWatermark(wtmt, iwmOps);
                }

                iwm.Dispose();

                MemoryStream ms = new();
                img.Save(ms, ImageFormat.Jpeg);
                img.Dispose();
                byte[] byteImage = ms.ToArray();
                var SigBase64 = Convert.ToBase64String(byteImage); // Get Base64
                return SigBase64;
            }
            catch (Exception ex)
            {
                return base64;
            }
        }
        public dynamic SaveImagesBase64(ImagesModel listImages, string url, string title)
        {
            try
            {
                var id = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() +
                         DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() +
                         DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
                var fileName = Fullmages + ipre + _common.EncodeTitle(title) + "-" + id + ".jpeg";

                var absolutepath = Directory.GetCurrentDirectory();

                var filePath = Path.Combine(absolutepath + "\\wwwroot\\" + url, fileName);
                string base64 = listImages.base64.Split(',')[1];
                byte[] bytes = Convert.FromBase64String(base64);

                using (Image image = Image.FromStream(new MemoryStream(bytes)))
                {
                    var width = image.Width;
                    if (Convert.ToInt32(maxW) < image.Width)
                    {
                        decimal wPerh = Convert.ToDecimal(image.Width) / Convert.ToDecimal(image.Height);
                        decimal height = Convert.ToDecimal(maxW) / wPerh;
                        Image img;
                        try
                        {
                            img = ImageResize.ScaleByWidth(image, Convert.ToInt32(maxW));
                        }
                        catch
                        {
                            img = image;
                        }
                        //img.SaveAs(filePath);
                        Bitmap newBitmap = new Bitmap(img);
                        img.Dispose();

                        image.Dispose();

                        newBitmap.Save(filePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                    }
                    else
                    {
                        image.Save(filePath, System.Drawing.Imaging.ImageFormat.Jpeg);

                    }
                    image.Dispose();

                }
                if (System.IO.File.Exists(filePath))
                {
                    var fileThumpName = ipre + _common.EncodeTitle(title) + "-" + id + ".jpeg";

                    using (var image = Image.FromFile(filePath))
                    {
                        decimal wPerh = Convert.ToDecimal(image.Width) / Convert.ToDecimal(image.Height);
                        decimal height = Convert.ToDecimal(minW) / wPerh;
                        var img = ImageResize.ScaleByWidth(image, Convert.ToInt32(maxW));
                        var filePathThumb = Path.Combine(absolutepath + "\\wwwroot\\" + url, fileThumpName);
                        Bitmap newBitmap = new Bitmap(img);

                        img.Dispose();
                        image.Dispose();

                        newBitmap.Save(filePathThumb, System.Drawing.Imaging.ImageFormat.Jpeg);
                        //img.SaveAs(filePathThumb);                       
                    }
                    return new { isError = true, messError = "", fileName = fileThumpName };
                }

                return new { isError = false, messError = "" };
            }
            catch (Exception ex)
            {

                return new { isError = false, messError = "" };
            }
        }
        public dynamic SaveImagesBase64(string tempBase64, string url, string title)
        {
            try
            {
                var id = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() +
                         DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() +
                         DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
                var fileName = _common.EncodeTitle(title) + "-" + id + ".jpeg";
                var fullName = Fullmages + ipre + fileName;
                var thumbName = ipre + fileName;
                var absolutepath = Directory.GetCurrentDirectory();

                var filePath = Path.Combine(absolutepath + "\\wwwroot\\" + url, fullName);
                var filePathThumb = Path.Combine(absolutepath + "\\wwwroot\\" + url, thumbName);

                string base64 = tempBase64.Split(',')[1];
                byte[] bytes = Convert.FromBase64String(base64);

                Image image;
                using (Image originalImage = Image.FromStream(new MemoryStream(bytes)))
                {
                    // Check if the image has an indexed pixel format
                    if (originalImage.PixelFormat == PixelFormat.Format8bppIndexed ||
                        originalImage.PixelFormat == PixelFormat.Format4bppIndexed ||
                        originalImage.PixelFormat == PixelFormat.Format1bppIndexed)
                    {
                        // Convert the indexed image to a non-indexed format (e.g., 32-bit ARGB)
                        using (Bitmap newBitmap = new Bitmap(originalImage.Width, originalImage.Height, PixelFormat.Format32bppArgb))
                        {
                            // Draw the indexed image onto the new bitmap to convert it
                            using (Graphics graphics = Graphics.FromImage(newBitmap))
                            {
                                graphics.DrawImage(originalImage, new Rectangle(0, 0, originalImage.Width, originalImage.Height));
                            }

                            // Dispose the original indexed image
                            originalImage.Dispose();

                            // Create a new Image instance from the Bitmap
                            image = Image.FromHbitmap(newBitmap.GetHbitmap());
                        }
                    }
                    else
                    {
                        // The image is not indexed, use the original image
                        image = originalImage;
                    }

                    // Perform resizing (if necessary) and save the image and thumbnail
                    if (image.Width > Convert.ToInt32(maxW))
                    {
                        var img = ImageResize.ScaleByWidth(image, Convert.ToInt32(maxW));
                        var thumb = ImageResize.ScaleByWidth(image, Convert.ToInt32(minW));
                        using (Bitmap newBitmap = new Bitmap(img))
                        {
                            using (Bitmap newBitmapThumb = new Bitmap(thumb))
                            {
                                img.Dispose();
                                thumb.Dispose();
                                image.Dispose();
                                newBitmap.Save(filePath, ImageFormat.Jpeg);
                                newBitmapThumb.Save(filePathThumb, ImageFormat.Jpeg);
                            }
                        }
                    }
                    else if (image.Width < Convert.ToInt32(maxW))
                    {
                        var img = ImageResize.ScaleByWidth(image, Convert.ToInt32(maxW));
                        var thumb = ImageResize.ScaleByWidth(image, Convert.ToInt32(minW));
                        using (Bitmap newBitmap = new Bitmap(img))
                        {
                            using (Bitmap newBitmapThumb = new Bitmap(thumb))
                            {
                                img.Dispose();
                                thumb.Dispose();
                                image.Dispose();
                                newBitmap.Save(filePath, ImageFormat.Jpeg);
                                newBitmapThumb.Save(filePathThumb, ImageFormat.Jpeg);
                            }
                        }
                    }
                    else
                    {
                        var thumb = ImageResize.ScaleByWidth(image, Convert.ToInt32(minW));
                        using (Bitmap newBitmapThumb = new Bitmap(thumb))
                        {
                            thumb.Dispose();
                            image.Dispose();
                            newBitmapThumb.Save(filePathThumb, ImageFormat.Jpeg);
                        }
                    }
                }

                // Check if the file exists and return the appropriate result
                if (File.Exists(filePath))
                {
                    return new { isError = true, messError = "", fileName = thumbName };
                }

                return new { isError = false, messError = "" };
            }
            catch (Exception ex)
            {
                // Handle the exception appropriately, e.g., log the error
                return new { isError = true, messError = ex.Message };
            }
        }
        //public dynamic SaveImagesBase64(string tempBase64, string url, string title)
        //{
        //    try
        //    {
        //        var id = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() +
        //                 DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() +
        //                 DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
        //        var fileName = _common.EncodeTitle(title) + "-" + id + ".jpeg";
        //        var fullName = Fullmages + ipre + fileName;
        //        var thumbName = ipre + fileName;
        //        var absolutepath = Directory.GetCurrentDirectory();

        //        var filePath = Path.Combine(absolutepath + "\\wwwroot\\" + url, fullName);
        //        var filePathThumb = Path.Combine(absolutepath + "\\wwwroot\\" + url, thumbName);

        //        string base64 = tempBase64.Split(',')[1];
        //        byte[] bytes = Convert.FromBase64String(base64);

        //        Image image;
        //        using (Image originalImage = Image.FromStream(new MemoryStream(bytes)))
        //        {
        //            // Check if the image has an indexed pixel format
        //            if (originalImage.PixelFormat == PixelFormat.Format8bppIndexed ||
        //                originalImage.PixelFormat == PixelFormat.Format4bppIndexed ||
        //                originalImage.PixelFormat == PixelFormat.Format1bppIndexed)
        //            {
        //                // Convert the indexed image to a non-indexed format (e.g., 32-bit ARGB)
        //                using (Bitmap newBitmap = new Bitmap(originalImage.Width, originalImage.Height, PixelFormat.Format32bppArgb))
        //                {
        //                    // Draw the indexed image onto the new bitmap to convert it
        //                    using (Graphics graphics = Graphics.FromImage(newBitmap))
        //                    {
        //                        graphics.DrawImage(originalImage, new Rectangle(0, 0, originalImage.Width, originalImage.Height));
        //                    }

        //                    // Dispose the original indexed image
        //                    originalImage.Dispose();

        //                    // Create a new Image instance from the Bitmap
        //                    image = Image.FromHbitmap(newBitmap.GetHbitmap());
        //                }
        //            }
        //            else
        //            {
        //                // The image is not indexed, use the original image
        //                image = originalImage;
        //            }

        //            // Perform resizing (if necessary) and save the image and thumbnail
        //            var img = image;
        //            if (image.Width > Convert.ToInt32(maxW))
        //            {
        //                img = ImageResize.ScaleByWidth(image, Convert.ToInt32(maxW));

        //            }
        //            else if (image.Width < Convert.ToInt32(maxW))
        //            {
        //                img = ImageResize.ScaleByWidth(image, Convert.ToInt32(maxW));
        //            }
        //            using (Bitmap newBitmap = new Bitmap(img))
        //            {
        //                img.Dispose();
        //                image.Dispose();
        //                newBitmap.Save(filePath, ImageFormat.Jpeg);
        //            }
        //        }

        //        // Check if the file exists and return the appropriate result
        //        if (File.Exists(filePath))
        //        {
        //            return new { isError = true, messError = "", fileName = thumbName };
        //        }

        //        return new { isError = false, messError = "" };
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle the exception appropriately, e.g., log the error
        //        return new { isError = true, messError = ex.Message };
        //    }
        //}
        public dynamic SaveImagesBase64WithWatermark(string tempBase64, string url, string title)
        {
            try
            {
                var id = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() +
                         DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() +
                         DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
                var fileName = _common.EncodeTitle(title) + "-" + id + ".jpeg";
                var fullName = Fullmages + ipre + fileName;
                var thumbName = ipre + fileName;
                var absolutepath = Directory.GetCurrentDirectory();

                var filePath = Path.Combine(absolutepath + "\\wwwroot\\" + url, fullName);
                var filePathThumb = Path.Combine(absolutepath + "\\wwwroot\\" + url, thumbName);

                string base64 = tempBase64.Split(',')[1];
                byte[] bytes = Convert.FromBase64String(base64);

                Image image;
                using (Image originalImage = Image.FromStream(new MemoryStream(bytes)))
                {
                    // Check if the image has an indexed pixel format
                    if (originalImage.PixelFormat == PixelFormat.Format8bppIndexed ||
                        originalImage.PixelFormat == PixelFormat.Format4bppIndexed ||
                        originalImage.PixelFormat == PixelFormat.Format1bppIndexed)
                    {
                        // Convert the indexed image to a non-indexed format (e.g., 32-bit ARGB)
                        using (Bitmap newBitmap = new Bitmap(originalImage.Width, originalImage.Height, PixelFormat.Format32bppArgb))
                        {
                            // Draw the indexed image onto the new bitmap to convert it
                            using (Graphics graphics = Graphics.FromImage(newBitmap))
                            {
                                graphics.DrawImage(originalImage, new Rectangle(0, 0, originalImage.Width, originalImage.Height));
                            }

                            // Dispose the original indexed image
                            originalImage.Dispose();

                            // Create a new Image instance from the Bitmap
                            image = Image.FromHbitmap(newBitmap.GetHbitmap());
                        }
                    }
                    else
                    {
                        // The image is not indexed, use the original image
                        image = originalImage;
                    }

                    // Perform resizing (if necessary) and save the image and thumbnail
                    var img = image;
                    var thumb = ImageResize.ScaleByWidth(image, Convert.ToInt32(minW));
                    if (image.Width > Convert.ToInt32(maxW))
                    {
                        img = ImageResize.ScaleByWidth(image, Convert.ToInt32(maxW));
                    }
                    else if (image.Width < Convert.ToInt32(minW))
                    {
                        img = ImageResize.ScaleByWidth(image, Convert.ToInt32(minW));
                    }
                    using (Bitmap newBitmap = new(img))
                    {
                        using (Bitmap newBitmapThumb = new(thumb))
                        {
                            img.Dispose();
                            thumb.Dispose();
                            image.Dispose();

                            using (var ms = new MemoryStream())
                            {
                                newBitmap.Save(ms, ImageFormat.Jpeg);
                                byte[] byteImage = ms.ToArray();
                                string SigBase64 = Convert.ToBase64String(byteImage);
                                CreateWatermarkImage(SigBase64, filePath);
                            }

                            if (WatermarkPicThumbActive == "on")
                            {
                                using (var ms = new MemoryStream())
                                {
                                    newBitmapThumb.Save(ms, ImageFormat.Jpeg);
                                    byte[] byteImage = ms.ToArray();
                                    string SigBase64 = Convert.ToBase64String(byteImage);
                                    CreateWatermarkImage(SigBase64, filePathThumb);
                                }
                            }
                            else
                            {
                                newBitmapThumb.Save(filePathThumb, ImageFormat.Jpeg);
                            }
                        }
                    }
                }

                // Check if the file exists and return the appropriate result
                if (File.Exists(filePath))
                {
                    return new { isError = true, messError = "", fileName = thumbName };
                }

                return new { isError = false, messError = "" };
            }
            catch (Exception ex)
            {
                // Handle the exception appropriately, e.g., log the error
                return new { isError = true, messError = ex.Message };
            }
        }
        //public dynamic SaveImagesBase64WithWatermark(string tempBase64, string url, string title)
        //{
        //    try
        //    {
        //        var id = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() +
        //                 DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() +
        //                 DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
        //        var fileName = _common.EncodeTitle(title) + "-" + id + ".jpeg";
        //        var fullName = Fullmages + ipre + fileName;
        //        var thumbName = ipre + fileName;
        //        var absolutepath = Directory.GetCurrentDirectory();

        //        var filePath = Path.Combine(absolutepath + "\\wwwroot\\" + url, fullName);
        //        var filePathThumb = Path.Combine(absolutepath + "\\wwwroot\\" + url, thumbName);

        //        string base64 = tempBase64.Split(',')[1];
        //        byte[] bytes = Convert.FromBase64String(base64);

        //        Image image;
        //        using (Image originalImage = Image.FromStream(new MemoryStream(bytes)))
        //        {
        //            // Check if the image has an indexed pixel format
        //            if (originalImage.PixelFormat == PixelFormat.Format8bppIndexed ||
        //                originalImage.PixelFormat == PixelFormat.Format4bppIndexed ||
        //                originalImage.PixelFormat == PixelFormat.Format1bppIndexed)
        //            {
        //                // Convert the indexed image to a non-indexed format (e.g., 32-bit ARGB)
        //                using (Bitmap newBitmap = new Bitmap(originalImage.Width, originalImage.Height, PixelFormat.Format32bppArgb))
        //                {
        //                    // Draw the indexed image onto the new bitmap to convert it
        //                    using (Graphics graphics = Graphics.FromImage(newBitmap))
        //                    {
        //                        graphics.DrawImage(originalImage, new Rectangle(0, 0, originalImage.Width, originalImage.Height));
        //                    }

        //                    // Dispose the original indexed image
        //                    originalImage.Dispose();

        //                    // Create a new Image instance from the Bitmap
        //                    image = Image.FromHbitmap(newBitmap.GetHbitmap());
        //                }
        //            }
        //            else
        //            {
        //                // The image is not indexed, use the original image
        //                image = originalImage;
        //            }

        //            // Perform resizing (if necessary) and save the image and thumbnail
        //            var img = image;
        //            if (image.Width > Convert.ToInt32(maxW))
        //            {
        //                img = ImageResize.ScaleByWidth(image, Convert.ToInt32(maxW));
        //            }
        //            else if (image.Width < Convert.ToInt32(minW))
        //            {
        //                img = ImageResize.ScaleByWidth(image, Convert.ToInt32(minW));
        //            }
        //            using (Bitmap newBitmap = new(img))
        //            {
        //                img.Dispose();
        //                image.Dispose();

        //                using (var ms = new MemoryStream())
        //                {
        //                    newBitmap.Save(ms, ImageFormat.Jpeg);
        //                    byte[] byteImage = ms.ToArray();
        //                    string SigBase64 = Convert.ToBase64String(byteImage);
        //                    CreateWatermarkImage(SigBase64, filePath);
        //                }
        //            }
        //        }

        //        // Check if the file exists and return the appropriate result
        //        if (File.Exists(filePath))
        //        {
        //            return new { isError = true, messError = "", fileName = thumbName };
        //        }

        //        return new { isError = false, messError = "" };
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle the exception appropriately, e.g., log the error
        //        return new { isError = true, messError = ex.Message };
        //    }
        //}
        public dynamic ConvertIformfileToBase64(IFormFile imagesFile)
        {
            try
            {
                string s = "";
                if (imagesFile.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        imagesFile.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        s = Convert.ToBase64String(fileBytes);
                        // act on the Base64 data
                    }
                }
                return s;
            }
            catch (Exception ex)
            {
                return "";
            }

        }
        public void SaveTxtFile(string url, string content)
        {
            try
            {
                var logPath = System.IO.Path.GetTempFileName();
                using (var writer = File.CreateText(url))
                {
                    writer.WriteLine(content); //or .Write(), if you wish
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public string CreateWatermarkImage(string originPath, string watermarkPath, string fileName)
        {
            try
            {
                var base64Img = WatermarkImage(originPath, Fullmages + fileName);
                byte[] imageBytes = Convert.FromBase64String(base64Img);
                var pathSave = @"wwwroot" + watermarkPath + Fullmages + fileName;
                using (var imageFile = new FileStream(pathSave, FileMode.Create, FileAccess.ReadWrite))
                {
                    imageFile.Write(imageBytes, 0, imageBytes.Length);
                    imageFile.Flush();
                }
                return watermarkPath + Fullmages + fileName;
            }
            catch (Exception ex)
            {
                return originPath + Fullmages + fileName;
            }
        }
        private void CreateWatermarkImage(string base64, string filePath)
        {
            try
            {
                var base64Img = WatermarkImage(base64);
                byte[] imageBytes = Convert.FromBase64String(base64Img);
                using var imageFile = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite);
                imageFile.Write(imageBytes, 0, imageBytes.Length);
                imageFile.Flush();
            }
            catch (Exception)
            {

            }
        }
    }
}
