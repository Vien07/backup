using CMS.Services;
using CMS.Services.CommonServices;
using DTO;
using DTO.Website;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Areas.Dashboard
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly DBContext _dbContext;
        private readonly ICommonServices _common;
        private string UrlNewsImages = ConstantStrings.UrlNewsImages;
        private string UrlProductImages = ConstantStrings.UrlProductImages;
        private string UrlBookImages = ConstantStrings.UrlBookImages;
        private string DefaultLang = ConstantStrings.DefaultLang;
        public DashboardRepository(DBContext dbContext, ICommonServices common)
        {
            _dbContext = dbContext;
            _common = common;
        }
        public dynamic LoadDataNews()
        {
            try
            {
                var data = _dbContext.NewsDetails.Where(p => p.Enabled == true && p.Deleted == false).OrderByDescending(p => p.CounterView).Take(10).ToList();
                List<dynamic> rs = new List<dynamic>();

                foreach (var item in data)
                {
                    var multiData = _dbContext.MultiLang_NewsDetails.Where(p => p.NewsDetailPid == item.Pid && p.LangKey == DefaultLang).FirstOrDefault();
                    dynamic d = new ExpandoObject();
                    d.Pid = item.Pid;
                    d.Title = multiData.Title;
                    d.CounterView = item.CounterView;
                    d.Images = UrlNewsImages + item.PicThumb;
                    rs.Add(d);
                }
                return rs;
            }
            catch (Exception ex)
            {

                return "[]";
            }
        }
        public dynamic LoadDataProduct()
        {
            try
            {
                var data = _dbContext.ProductDetails.Where(p => p.Enabled == true && p.Deleted == false).OrderByDescending(p => p.CounterView).Take(10).ToList();
                List<dynamic> rs = new List<dynamic>();

                foreach (var item in data)
                {
                    var multiData = _dbContext.MultiLang_ProductDetails.Where(p => p.ProductDetailPid == item.Pid && p.LangKey == DefaultLang).FirstOrDefault();
                    dynamic d = new ExpandoObject();
                    d.Pid = item.Pid;
                    d.Title = multiData.Title;
                    d.CounterView = item.CounterView;
                    d.Images = UrlProductImages + item.PicThumb;
                    rs.Add(d);
                }
                return rs;
            }
            catch (Exception ex)
            {

                return "[]";
            }
        }
        public async Task<dynamic> GetVisitor()
        {
            try
            {
                var visitors = await _dbContext.LoadStoredProc("GetVistors").ExecuteStoredProc<VisitorDto>();
                dynamic obj = new ExpandoObject();
                obj.Online = _common.ConvertFormatMoney(visitors[0].Online);
                obj.Total = _common.ConvertFormatMoney(visitors[0].Total);
                return obj;
            }
            catch (Exception ex)
            {
                dynamic obj = new ExpandoObject();
                obj.Online = "0";
                obj.Total = "0";
                return obj;
            }

        }
        public int CountNews()
        {
            try
            {
                var data = _dbContext.NewsDetails.Where(p => p.Enabled == true && p.Deleted == false).ToList();

                return data.Count();
            }
            catch (Exception ex)
            {

                return 0;
            }
        }
        public int CountProduct()
        {
            try
            {
                var data = _dbContext.ProductDetails.Where(p => p.Enabled == true && p.Deleted == false).ToList();

                return data.Count();
            }
            catch (Exception ex)
            {

                return 0;
            }
        }
        public async Task<dynamic> CountVisitorInMonth(int month, int year)
        {
            try
            {
                DateTime startDate = Convert.ToDateTime(month + "/01/" + year);
                DateTime endDate = startDate.AddMonths(1).AddDays(-1);
                var visitors = await _dbContext.LoadStoredProc("GetVistorsInMonth").WithSqlParam("startDate", startDate).WithSqlParam("endDate", endDate).ExecuteStoredProc<VisitorDto>();
                return visitors[0].Total;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public async Task<dynamic> GetBarChart(int month, int year)
        {
            try
            {
                DateTime startDate = Convert.ToDateTime(month + "/01/" + year);
                DateTime endDate = startDate.AddMonths(1).AddDays(-1);
                string jsonData = "[";
                string jsonDay = "[";


                var visitors = await _dbContext.LoadStoredProc("GetBarChart")
                        .WithSqlParam("startDay", startDate.Day)
                        .WithSqlParam("endDay", endDate.Day)
                        .WithSqlParam("month", month)
                        .WithSqlParam("year", year)
                        .ExecuteStoredProc<VisitorDto>();

                var barChartArr = visitors[0].BarChart.Split(",");

                for (int i = 0; i < barChartArr.Length; i++)
                {
                    if (!String.IsNullOrEmpty(barChartArr[i]))
                    {
                        jsonData += barChartArr[i].ToString() + ",";
                        jsonDay += "\"" + i.ToString() + "\"" + ",";
                    }
                }

                jsonData = jsonData.Substring(0, jsonData.Length - 1) + "]";
                jsonDay = jsonDay.Substring(0, jsonDay.Length - 1) + "]";
                dynamic d = new ExpandoObject();
                d.jsData = jsonData;
                d.jsDay = jsonDay;
                return d;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public async Task<dynamic> GetBarChartWithDate(DateTime startDate, DateTime endDate)
        {
            try
            {
                //DateTime startDate = Convert.ToDateTime(month + "/01/" + year);
                //DateTime endDate = startDate.AddMonths(1).AddDays(-1);

                string jsonData = "[";
                string jsonDay = "[";
                for (DateTime i = startDate; i <= endDate; i = i.AddDays(1))
                {
                    var visitors = await _dbContext.LoadStoredProc("GetVistorsInDate").WithSqlParam("date", i.Date).ExecuteStoredProc<VisitorDto>();
                    var data = visitors[0].Total;
                    jsonData += data.ToString() + ",";
                    jsonDay += "\"" + i.Day.ToString() + "\"" + ",";
                }
                jsonData = jsonData.Substring(0, jsonData.Length - 1) + "]";
                jsonDay = jsonDay.Substring(0, jsonDay.Length - 1) + "]";
                dynamic d = new ExpandoObject();
                d.jsData = jsonData;
                d.jsDay = jsonDay;
                return d;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public async Task<dynamic> CountVisitorWithDate(DateTime startDate, DateTime endDate)
        {
            try
            {
                var visitors = await _dbContext.LoadStoredProc("GetVistorsInMonth").WithSqlParam("startDate", startDate).WithSqlParam("endDate", endDate).ExecuteStoredProc<VisitorDto>();
                return visitors[0].Total;
            }
            catch (Exception ex)
            {

                return 0;
            }
        }
    }
}
