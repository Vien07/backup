
using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Http;
using Steam.Core.LocalizeManagement.Constants;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using X.PagedList;
using static Steam.Core.LocalizeManagement.Constants.LangItemConstants;

namespace Steam.Core.LocalizeManagement.Models
{
    public class LocalizeCultureModelEdit : Database.LocalizeCulture
    {
        public string SlugKey { get; set; }
        public string LangCode { get; set; }
        public Database.LocalizeCulture GetDatabaseModel()
        {

            Database.LocalizeCulture rs = new Database.LocalizeCulture();
            rs.Pid = this.Pid;
            rs.Order = this.Order;
            rs.LangCode =  this.LangCode;
            rs.SlugKey = this.SlugKey;
            if (!string.IsNullOrEmpty(this.LangCode)) 
            {
                var listLangCode = LangItemConstants.DefaultList;
                LangItem selectedLang = listLangCode.Where(p => p.LanguageCode == this.LangCode).FirstOrDefault();
                if (selectedLang != null) 
                {
                    rs.Name = selectedLang.Name;
                    rs.ShortCode = selectedLang.ShortCode;
                }
            }

            rs.CreateUser = this.CreateUser;
            rs.UpdateUser = this.UpdateUser;
            return rs;
        }

    }
    public class LocalizeCultureViewModel
    {
        public List<MultilangModel> Languages { get; set; }
        public List<Item> Data { get; set; } = new List<Item>();
        public IPagedList<Item> List { get; set; }

        public string ActiveLang { get; set; }
        public class Item
        {
            public int Pid { get; set; }
            public string LangCode { get; set; }
            public string ShortCode { get; set; }
            public string Name { get; set; }
            public string SlugKey { get; set; }
            public double Order { get; set; }
            public bool Enabled { get; set; } = true;
            public bool Deleted { get; set; } = false;
            public DateTime UpdateDate { get; set; }
            public string Lang { get; set; }
        }

    }


    public class LocalizeCultureDetail
    {
        public Database.LocalizeCulture Detail { get; set; } = new Database.LocalizeCulture();

    }

}
