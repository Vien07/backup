using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using CMS.Areas.FAQ.Models;
using System.Collections.Generic;
using DTO.Common;

namespace CMS.Areas.FAQ
{
    public interface IFAQRepository
    {
        dynamic LoadData(SearchDto search);

        bool Enable(long[] Pid, bool active);
        bool Delete(int Pid);
        dynamic Delete(long[] Pid);
        dynamic Edit(int Pid);
        dynamic Insert(FAQDetail faqDetail, List<MultiLang_FAQDetail> multiLangFAQDetail,
                           IFormFile Images, List<Temp_Images> listImagesFAQ,
                           List<Temp_MultiLang_Images> listLangImagesFAQ, string listCates);
        dynamic Update(FAQDetail faqDetail,
            List<MultiLang_FAQDetail> multiLangFAQDetail,
            IFormFile Images, List<Temp_Images> listDeleteImages, List<Temp_Images> listImagesFAQ, List<Temp_MultiLang_Images> listLangImagesFAQ, string listCates);
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