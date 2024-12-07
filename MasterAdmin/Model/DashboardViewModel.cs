
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MasterAdmin.Models.DashboardViewModel
{
    public class Product_Item : Admin.ProductManagement.Database.Product
    {


        public string Slug { get; set; }
        public string Cate { get; set; }
        public string CateSlug { get; set; }
        public string ImagePath { get; set; }
        public int CountView { get; set; }


    }
    public class ReportTraffic
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public void ToValue(List<ReportTraffic> listData)
        {
            string rs = "[]";
            try
            {
                var temp = listData.Select(item => item.Value).ToArray();
                 rs = JsonConvert.SerializeObject(temp);

            }
            catch (Exception)
            {

                this.Name = rs;
            }
            this.Value= rs;
        }
        public void ToLabel(List<ReportTraffic> listData)
        {
            string rs = "[]";

            try
            {
                var temp = listData.Select(item => item.Name).ToArray();
                rs = JsonConvert.SerializeObject(temp);
            }
            catch (Exception)
            {

                this.Name = rs;

            }
            this.Name = rs;
        }


    }
    public class StatisticsInfo
    {


        public string OnlineUser { get; set; }
        public string SizeMedia { get; set; }
        public string CountPost { get; set; }
        public string CountTraffic { get; set; }

    }
}
