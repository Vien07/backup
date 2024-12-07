
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Admin.SEO.Models
{
    public class ParamSearch
    {

        public string KeySearch { get; set; }=String.Empty;
        public string Cate { get; set; } = "0";
        public string Type { get; set; } = "0";
        public string Module { get; set; } = "";
        public bool? isEnable { get; set; }
        public string Active { get; set; } = "";

        public int PageIndex { get; set; } = 1;
        public ParamSearch Init()
        {
            var rs = new ParamSearch();
            try
            {
                rs.KeySearch = this.KeySearch;
                rs.Module = this.Module;
                if (this.Active == "0")
                {
                    rs.isEnable = false;
                }
                else if (this.Active == "1")
                {
                    rs.isEnable = true;
                }
                else
                {
                    rs.isEnable = null;
                }
                rs.PageIndex = this.PageIndex;



            }
            catch (Exception ex)
            {

                throw;
            }
            return rs;
        }
    }
}
