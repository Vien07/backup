using Admin.LayoutPage.Database;
using Steam.Core.Base.Models;
using Steam.Core.Common.SteamString;

namespace Admin.LayoutPage
{
    public class FooterPageHelper
    {
        public FooterPageHelper()
        {
        }
        public Response<string> GenerateFooter(
            Database.FooterPage FooterPage,
            List<FooterItem> listFooterItem,
            List<Admin.WebsiteKeys.Database.WebsiteKeys> listKey)
        {
            Response<string> rs = new Response<string>();

            try
            {
                string footer = "";
                footer = FooterPage.FooterBlock;
                footer = footer.Replace("{{FOOTER_PLUGIN}}", FooterPage.FooterPluginBlock);


                string footertemBlock = "";

                foreach (var item in listFooterItem)
                {

                    footer = footer.Replace(item.Key, item.ItemBlock);

                }
                //footer = footer.Replace("{{FOOTER_ITEMS}}", footertemBlock);
                footer = footer.Replace("\\n", "").Replace("\\t", "").Replace("\\\"", "\"");
                rs.Data = footer.AddInfoFromKey(listKey).RemoveATags();
                return rs;

            }
            catch (Exception ex)
            {
                rs.Message = ex.Message;
                rs.IsError = true;
                return rs;
            }
        }


        void SaveFileHeader(string content)
        {
            string fileName = "footer.html"; // Replace with the name of the file you want to update

            var absolutepath = Directory.GetCurrentDirectory();//to get current absolute path
            var filePath = Path.Combine(absolutepath + "\\wwwroot\\layout\\" + fileName);

            if (File.Exists(filePath))
            {

                File.WriteAllText(filePath, content);

            }

        }

    }
}
