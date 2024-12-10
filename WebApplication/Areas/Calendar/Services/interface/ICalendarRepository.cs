using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using CMS.Areas.Calendar.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using DTO.Common;

namespace CMS.Areas.Calendar
{
    public interface ICalendarRepository
    {
        dynamic LoadData();
        bool Enable(long[] Pid, bool active);
        bool Delete(int Pid);
        dynamic Delete(long[] Pid);
        dynamic Edit(long Pid);
        dynamic Insert(Models.Calendar calendar, List<MultiLang_Calendar> multiLang_Calendar);
        dynamic Update(Models.Calendar calendar, List<MultiLang_Calendar> multiLang_Calendar);
        bool Count(int code);
        string Search(SearchDto searchData);
        dynamic Validation(dynamic data);
        bool Up(long Pid);
        bool Down(long Pid);
        bool UpdateOrder(long Pid, int order);
        bool MoveRow(long from, long to);
    }
}