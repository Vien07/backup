using Admin.LayoutPage.Database;
using Steam.Core.Base.Constant;
using Steam.Core.Base.Models;
using Steam.Core.Common.SteamString;

namespace Admin.LayoutPage
{
    public class SliderHelper
    {
        public SliderHelper()
        {
        }
        public Response<string> GenerateSlider(Slider slider, List<SliderItem> listSlider, List<Admin.WebsiteKeys.Database.WebsiteKeys> listKey)
        {
            Response<string> rs = new Response<string>();

            try
            {
                string sliderHtml = "";
                sliderHtml = slider.SliderBlock;

                string sliderItemHtml = "";

                foreach (var item in listSlider)
                {
     
                    if (item.TypeMedia == "image")
                    {
                        sliderItemHtml += item.ItemBlock.Replace("{{SLIDERITEM_MEDIA}}", SystemInfo.MedidaFileServer + item.FilePath + item.Images)
                                     .Replace("{{SLIDERITEM_TITLE}}", item.Title)
                                     .Replace("{{SLIDERITEM_DESCRIPTION}}", item.Description);
                    }
                    else if (item.TypeMedia == "video")
                    {
                        sliderItemHtml += item.ItemBlock.Replace("{{SLIDERITEM_MEDIA}}", item.VideoLink)
                                     .Replace("{{SLIDERITEM_TITLE}}", item.Title)
                                     .Replace("{{SLIDERITEM_DESCRIPTION}}", item.Description);
                    }

                }
                sliderHtml = sliderHtml.Replace("{{SLIDER_ITEMS}}", sliderItemHtml);
                sliderHtml = sliderHtml.ToRemoveBreakSympol();
                rs.Data = sliderHtml.AddInfoFromKey(listKey).RemoveATags();

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
            string fileName = "slider.html"; // Replace with the name of the file you want to update

            var absolutepath = Directory.GetCurrentDirectory();//to get current absolute path
            var filePath = Path.Combine(absolutepath + "\\wwwroot\\layout\\" + fileName);

            if (File.Exists(filePath))
            {

                File.WriteAllText(filePath, content);

            }

        }

    }
}
