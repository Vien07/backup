using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Dynamic;
using X.PagedList;
using System.Globalization;
using CMS.Areas.Comment.Models;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using DTO;
using CMS.Services.FileServices;
using CMS.Services.CommonServices;
using DTO.Comment;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using static CMS.Services.ExtensionServices;

namespace CMS.Repository
{
    public class Comment_Repository : IComment_Repository
    {
        private string DateFormat = "";
        private string WatermarkActive = "";
        private string PageLimit = "";

        private readonly ICommonServices _common;

        private readonly DBContext _dbContext;
        private readonly IFileServices _fileServices;

        private string KeyPageLimit = ConstantStrings.KeyPageLimit;
        private string DefaultLang = ConstantStrings.DefaultLang;
        private string KeyDateFormat = ConstantStrings.KeyDateFormat;
        private string UrlCommentImages = ConstantStrings.UrlCommentImages;
        private string Thumb = ConstantStrings.Thumb;
        private string Fullmages = ConstantStrings.Fullmages;
        private string formatBase64 = ConstantStrings.formatBase64;
        private string UrlPreviewImages = ConstantStrings.UrlPreviewImages;

        public Comment_Repository(DBContext dbContext, ICommonServices common)
        {
            _dbContext = dbContext;
            _common = common;
            DateFormat = _common.GetConfigValue(KeyDateFormat);
            PageLimit = _common.GetConfigValue(KeyPageLimit);
        }
        public async Task<List<CommentDto>> GetList(string lang, int page)
        {
            try
            {
                CultureInfo vi = new CultureInfo(lang);

                var model = await (from a in _dbContext.Comments
                                   join b in _dbContext.MultiLang_Comments on a.Pid equals b.CommentPid
                                   where (!a.Deleted && a.Enabled) && (b.LangKey == lang)
                                   orderby a.Order descending
                                   select new CommentDto
                                   {
                                       Title = b.Name,
                                       TitleAlt = b.Name.Replace("\"", "").Replace("'", ""),
                                       Star = a.Star,
                                       Description = b.Description,
                                       PicThumb = UrlCommentImages + Thumb + a.PicThumb,
                                       Image = UrlCommentImages + Thumb + a.Image,
                                   }).ToListAsync();

                return model;
            }
            catch (Exception ex)
            {
                return new List<CommentDto>();
            }
        }
    }
}
