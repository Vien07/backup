using Admin.LayoutPage.Database;
using Steam.Core.Base.Models;
using Steam.Core.Common.SteamString;

namespace Admin.LayoutPage
{
    public class QuickToolBarHelper
    {
        string Domain = "http://w3bdemo.com:9010/";
        public QuickToolBarHelper()
        {
        }
        public Response<string> GenerateQuickToolBar(QuickToolBar QuickToolBar, List<QuickToolBarItem> listQuickToolBar, List<Admin.WebsiteKeys.Database.WebsiteKeys> listKey)
        {
            Response<string> rs = new Response<string>();

            try
            {
                string QuickToolBarHtml = "";
                QuickToolBarHtml = QuickToolBar.QuickToolBarBlock;

                string QuickToolBarItemHtml = "";

                foreach (var item in listQuickToolBar)
                {
                    QuickToolBarHtml = QuickToolBarHtml.Replace(item.Key, item.ItemBlock);


                }
            
                QuickToolBarHtml = QuickToolBarHtml.ToRemoveBreakSympol();
                rs.Data = QuickToolBarHtml.AddInfoFromKey(listKey).RemoveATags();

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
            string fileName = "QuickToolBar.html"; // Replace with the name of the file you want to update

            var absolutepath = Directory.GetCurrentDirectory();//to get current absolute path
            var filePath = Path.Combine(absolutepath + "\\wwwroot\\layout\\" + fileName);

            if (File.Exists(filePath))
            {

                File.WriteAllText(filePath, content);

            }

        }

    }
}
