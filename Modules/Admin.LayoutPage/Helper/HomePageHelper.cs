using Admin.LayoutPage.Database;
using Newtonsoft.Json.Linq;
using Steam.Core.Base.Models;
using Steam.Core.Common.SteamString;
using Steam.Core.Utilities.STeamHelper;
using static Admin.LayoutPage.HomePageHelperModel;

namespace Admin.LayoutPage
{
    public class HomePageHelper
    {
        private readonly IRestHelper _restHelper;
        public HomePageHelper(IRestHelper restHelper)
        {
            _restHelper = restHelper;
        }

        public Response<string> GenerateHomePage(List<HomePage> listSection, List<Admin.WebsiteKeys.Database.WebsiteKeys> listKey)
        {
            Response<string> rs = new Response<string>();

            var listSectionHTML = "";

            try
            {
                //var dataa = GetListDataFromApi("");

                foreach (var item in listSection)
                {


                    if (item.TypeView == "list")
                    {
                        GenHtmlList(ref listSectionHTML, item);
                    }
                    else if (item.TypeView == "tab")
                    {
                        var listTabHtml = "";
                        GenHtmlTab(ref listTabHtml, item);
                        listSectionHTML += item.Section.Replace("{{SECTION_TABS}}", listTabHtml);
                    }

                }
                listSectionHTML = listSectionHTML.ToRemoveBreakSympol();
                rs.Data = listSectionHTML.AddInfoFromKey(listKey).RemoveATags();

                return rs;
            }
            catch (Exception ex)
            {

                rs.Message = ex.Message;
                rs.IsError = true;
                return rs;
            }
        }
        void GenHtmlList(ref string listSectionHTML, dynamic item)
        {
            try
            {
                var data = GetListDataFromApi(item.SourceData);
              var isError = data.IsError;
                var listItemHTML = "";
                if (!isError)
                {

                    foreach (var ele in data.Data)
                    {

                        var itemHTML = item.ListItemHtml.ToString();
                        foreach (var i in ele)
                        {
                            itemHTML = itemHTML.Replace(i.Key, i.Value);

                        }
                        listItemHTML += itemHTML;

                    }
                    var sectionHTML = item.Section.Replace("{{SECTION_ITEMS}}", listItemHTML);
                    listSectionHTML += sectionHTML;

                }
            }
            catch (Exception ex)
            {

            }


        }
        void GenHtmlTab(ref string listTabHtml, dynamic item)
        {
            try
            {
                listTabHtml = "";
                var data = GetListDataFromApiTab(item.SourceData);
                foreach (var ele in data.Data)
                {
                    var tabName = ele.TabName.ToString();
                    var image = ele.ImagePath.ToString();
                    var tabId = ele.Pid.ToString();
                    var image_alt = ele.Images_Alt != null ? ele.Images_Alt : "";
                    listTabHtml += item.ListTabHtml.ToString().Replace("{{SECTION_TABS_NAME}}", tabName).Replace("{{SECTION_TABS_ID}}", tabId)
                        .Replace("{{SECTION_TABS_IMAGE}}", image)
                        .Replace("{{SECTION_TABS_IMAGE_ALT}}", image_alt);
                    var itemChildHtml = "";
                    foreach (var eleChild in ele.ListItems)
                    {
                        //var temp = "";
                        //var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(eleChild);
                        var values = JObject.FromObject(eleChild).ToObject<Dictionary<string, string>>();

                        var temp = item.ListItemChildHtml.ToString();
                        foreach (var i in values)
                        {
                            temp = temp.Replace(i.Key, i.Value);

                        }
                        itemChildHtml += temp;
                    }
                    listTabHtml = listTabHtml.Replace("{{SECTIONITEM_CHILD}}", itemChildHtml);
                }
                listTabHtml = listTabHtml.ToRemoveBreakSympol(); ;

                //listTabNameHtml = "abc";

            }
            catch (Exception ex)
            {

            }
        }
        public Response<List<Dictionary<string, string>>> GetListDataFromApi(string DOMAIN)
        {
            Response<List<Dictionary<string, string>>> rs = new Response<List<Dictionary<string, string>>>();

            //return GenSampleDataFromApi();
            try
            {
                var headers = new Dictionary<string, string>();
                headers.Add("Content-Type", "application/json");
                var data = _restHelper.Get<Response<List<Dictionary<string, string>>>>(DOMAIN, requestHeaders: headers);
                if (data != null)
                {
                    rs = data;
                }
                else
                {
                    rs.IsError = true;
                }

                ////var options = new RestClientOptions("https://localhost:50181")
                //var options = new RestClientOptions(DOMAIN)
                //{
                //    MaxTimeout = -1,
                //};
                //var client = new RestClient(options);
                //var request = new RestRequest("", Method.Get);

                //request.AddHeader("Content-Type", "application/json");
                ////request.AddHeader("Authorization", "Bearer" + " " + access_token);
                //request.AddParameter("application/json", body, ParameterType.RequestBody);

                //RestResponse response = client.Execute(request);

                //if (response.StatusCode == System.Net.HttpStatusCode.OK)
                //{
                //    rs = JsonConvert.DeserializeObject<Response<List<Dictionary<string, string>>>>(response.Content);
                //}
                //else
                //{
                //    rs.isError = true;
                //}
                return rs;

            }
            catch (Exception ex)
            {
                rs.IsError = true;
                return rs;

            }
        }


        public Response<List<GetListDataFromApiTab>> GetListDataFromApiTab(string DOMAIN)
        {
            Response<List<GetListDataFromApiTab>> rs = new Response<List<GetListDataFromApiTab>>();

            //return GenSampleDataFromApi();
            try
            {
                var headers = new Dictionary<string, string>();
                headers.Add("Content-Type", "application/json");
                var data = _restHelper.Get<Response<List<GetListDataFromApiTab>>>(DOMAIN, requestHeaders: headers);
                if (data != null)
                {
                    rs = data;
                }
                else
                {
                    rs.IsError = true;
                }

                return rs;

            }
            catch (Exception ex)
            {
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
    public class HomePageHelperModel
    {
        public class GetListDataFromApiTab
        {
            public string? TabName { get; set; }
            public string? Images_Alt { get; set; }
            public string? ImagePath { get; set; }
            public long Pid { get; set; }
            public List<Dictionary<string, string>> ListItems { get; set; }
        }
    }
}
