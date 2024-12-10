using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using CMS.Areas.Promotion.Models;
using System.Collections.Generic;
using DTO.Common;

namespace CMS.Areas.Promotion
{
    public interface IPromotionCateRepository
    {
        dynamic LoadData(SearchDto search);

        bool Enable(long[] Pid, bool active);
        dynamic Delete(int Pid);
        dynamic DeleteAll(int Pid);
        dynamic Delete(long[] Pid);
        dynamic Edit(long Pid);
        dynamic Insert(PromotionCate newsCate, List<MultiLang_PromotionCate> multiLang_newsCate);
        dynamic Update(PromotionCate newsCate, List<MultiLang_PromotionCate> multiLang_newsCate);
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