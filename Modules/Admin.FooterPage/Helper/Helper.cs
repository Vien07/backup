using Admin.FooterPage.Database;
using Microsoft.AspNetCore.Http;
using Steam.Core.Common.Models;
using Steam.Core.Common.SteamModels;
using Steam.Core.Common.SteamString;
using Steam.Core.Utilities.SteamModels;
using System.Drawing;

namespace Admin.FooterPage
{
    public class FooterPageHelper
    {
        public FooterPageHelper()
        {
        }
        public string GenerateHeader(Database.FooterPage FooterPage,List<FooterItem> listFooterItem)
        {
            try
            {
                string footer = "";
                footer = FooterPage.FooterBlock;
                footer = footer.Replace("{{FOOTER_PLUGIN}}", FooterPage.FooterPluginBlock);


                string footertemBlock = "";

                foreach (var item in listFooterItem)
                {

                    footertemBlock += item.ItemBlock;

                }
                footer = footer.Replace("{{FOOTER_ITEM}}", footertemBlock);
  
                return footer;

            }
            catch (Exception ex)
            {
                return "----Có lỗi trong quá trình tạo header----";
            }
        }


        void SaveFileHeader(string content)
        {
            string fileName = "header.html"; // Replace with the name of the file you want to update

            var absolutepath = Directory.GetCurrentDirectory();//to get current absolute path
            var filePath = Path.Combine(absolutepath + "\\wwwroot\\layout\\" + fileName);

            if (File.Exists(filePath))
            {

                File.WriteAllText(filePath, content);

            }

        }

    }
}
