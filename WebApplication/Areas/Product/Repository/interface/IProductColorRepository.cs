using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using CMS.Areas.Product.Models;
using System.Collections.Generic;
using DTO.Common;

namespace CMS.Areas.Product
{
    public interface IProductColorRepository
    {
        dynamic LoadData(SearchDto search);
        bool Enable(long[] Pid, bool active);
        dynamic Delete(int Pid);
        dynamic DeleteAll(int Pid);
        dynamic Delete(long[] Pid);
        dynamic Edit(long Pid);
        dynamic Insert(ProductColor ProductColor, List<MultiLang_ProductColor> multiLang_ProductColor, IFormFile PicThumb);
        dynamic Update(ProductColor ProductColor, List<MultiLang_ProductColor> multiLang_ProductColor, IFormFile PicThumb);
        bool Count(int code);
        string Search(SearchDto searchData);
        bool Coppy(long[] Pid);
        dynamic Validation(dynamic data);
        bool Up(long Pid);
        bool Down(long Pid);
        bool MoveRow(long from, long to);
        bool SaveShowHome(long Pid, bool value);
        bool UpdateOrder(long Pid, int order);

    }
}