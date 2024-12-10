using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using CMS.Areas.FAQ.Models;
using System.Collections.Generic;
using DTO.Common;

namespace CMS.Areas.FAQ
{
    public interface IFAQCateRepository
    {
        dynamic LoadData(SearchDto search);

        bool Enable(long[] Pid, bool active);
        dynamic Delete(int Pid);
        dynamic DeleteAll(int Pid);
        dynamic Delete(long[] Pid);
        dynamic Edit(long Pid);
        dynamic Insert(FAQCate faqCate, List<MultiLang_FAQCate> multiLang_faqCate);
        dynamic Update(FAQCate faqCate, List<MultiLang_FAQCate> multiLang_faqCate);
        bool Count(int code);
        string Search(SearchDto searchData);
        bool Coppy(long[] Pid);
        dynamic Validation(dynamic data);
        bool Up(long Pid);
        bool Down(long Pid);
        bool MoveRow(long from, long to);
        bool UpdateOrder(long Pid, int order);
    }
}