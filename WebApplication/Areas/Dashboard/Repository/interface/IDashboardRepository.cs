using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using CMS.Areas.News.Models;
using System.Collections.Generic;
using System;

namespace CMS.Areas.Dashboard
{
    public interface IDashboardRepository
    {
        dynamic LoadDataNews();
        int CountNews();
        Task<dynamic> GetVisitor();
        dynamic LoadDataProduct();
        int CountProduct();
        Task<dynamic> CountVisitorInMonth(int month, int year);
        Task<dynamic> GetBarChart(int month, int year);
        Task<dynamic> GetBarChartWithDate(DateTime startDate, DateTime endDate);
        Task<dynamic> CountVisitorWithDate(DateTime startDate, DateTime endDate);
    }
}