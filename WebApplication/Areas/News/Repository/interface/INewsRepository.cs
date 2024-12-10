using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using CMS.Areas.News.Models;
using System.Collections.Generic;
using DTO.Common;

namespace CMS.Areas.News
{
    public interface INewsRepository
    {
        dynamic LoadData(SearchDto search);

        bool Enable(long[] Pid, bool active);
        bool Delete(int Pid);
        dynamic Delete(long[] Pid);
        dynamic Edit(int Pid);
        dynamic Insert(NewsDetail newsDetail, List<MultiLang_NewsDetail> multiLangNewsDetail,
                           IFormFile Images, List<Temp_Images> listImagesNews,
                           List<Temp_MultiLang_Images> listLangImagesNews, string listCates);
        dynamic Update(NewsDetail newsDetail,
            List<MultiLang_NewsDetail> multiLangNewsDetail,
            IFormFile Images, List<Temp_Images> listDeleteImages, List<Temp_Images> listImagesNews, List<Temp_MultiLang_Images> listLangImagesNews, string listCates);
        bool Count(int code);
        string Search(SearchDto searchData);
        bool Coppy(long[] Pid);
        dynamic Validation(dynamic data);
        bool Preview(string obj, string objDetail, IFormFile PicThumb);
        bool MoveRow(long from, long to);
        bool UpdateOrder(long Pid, int order);
        bool SaveStatus(long pid, bool value, string type);

    }
}