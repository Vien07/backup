using Org.BouncyCastle.Crypto.Modes;
using Steam.Core.Base.Models;

using System.Xml.Linq;

namespace Admin.ProductManagement.Helpers
{
    public class MisaProductCategoryHelper
    {
        public MisaProductCategoryHelper()
        {
        }

        public Response<string> GenerateXMLRewriteUrl(string preSlugCate, string preSlugDetail, string catePage
            , string detailPage, string parram
                        )
        {
            //string preSlugCate = "danh-muc";
            Response<string> rs = new Response<string>();
            var rules = new List<dynamic>();
            try
            {
                List<RuleData> ruleDataList = new List<RuleData>();



                if (!string.IsNullOrEmpty(catePage))
                {
                    var tempPar = parram ?? "";
                    //var tempPar = "page:PageIndex:1;sex:sex:1";

                    var arrPar = tempPar.Split(";");
                    string paramDefault = "";
                    string paramRewrite = "";
                    string listParams = "";
                    var n = 1;
                    if (!String.IsNullOrEmpty(tempPar))
                    {
                        foreach (var part in arrPar)
                        {
                            if(!string.IsNullOrEmpty(part))
                            {
                                var parPart = part.Split(":");
                                if (parPart.Length == 3)
                                {
                                    paramDefault += string.Format("&{0}={1}", parPart[1].ToString(), parPart[2].ToString());
                                    paramRewrite += string.Format("{{QUERY_STRING}}:{0}=(.*)", parPart[0].ToString()) + ";";
                                    listParams += string.Format("{0}={{C:{1}}}", parPart[1].ToString(), n) + "&";
                                }
                                n++;
                            }    
                        
                        }
                    }

                    ruleDataList.Add(
                     new RuleData(
                       string.Format("Rewrite {0} Cate List With Detail Prefix - No Params", preSlugDetail),
                     string.Format("^({0}|{0}/)$",  preSlugDetail),
                     string.Format("{0}?rootSlug={1}{2}", catePage, preSlugDetail, paramDefault),
                      ""));
                    ruleDataList.Add(
                       new RuleData(
                         string.Format("Rewrite {0} Cate List With Detail Prefix - No Params", preSlugDetail),
                       string.Format("^({0}|{0}/)$", preSlugCate),
                       string.Format("{0}?rootSlug={1}{2}", catePage, preSlugCate, paramDefault),
                        ""));
                    ruleDataList.Add(
              new RuleData(
        string.Format("Rewrite {0} Cate - No Params", preSlugCate),
           string.Format("^{0}/([a-zA-Z0-9\\-+']+)/$", preSlugCate),
          string.Format("{0}?cateSlug={{R:1}}{1}", catePage, paramDefault),
            ""));
           //         ruleDataList.Add(
           //             new RuleData(
           //       string.Format("Rewrite {0} Cate", preSlugCate),
           //string.Format("^{0}/([a-zA-Z0-9\\-+']+)/?$", preSlugCate),
           //         string.Format("{0}?cateSlug={{R:1}}{1}", catePage, listParams),
           //           paramRewrite));
                }
                if (!string.IsNullOrEmpty(detailPage))
                {

                    ruleDataList.Add(
                        new RuleData(
                            string.Format("Rewrite {0} Detail", preSlugDetail),
                         string.Format("^{0}/([a-zA-Z0-9-+']+)$", preSlugDetail),
                            string.Format("{0}?slug={{R:1}}", detailPage),
                            "{REQUEST_FILENAME}:IsFile:true"));  
                    ruleDataList.Add(
                        new RuleData(
                            string.Format("Rewrite {0} Detail", preSlugDetail),
                         string.Format("^{0}/([a-zA-Z0-9-+']+)/$", preSlugDetail),
                            string.Format("{0}?slug={{R:1}}", detailPage),
                            "{REQUEST_FILENAME}:IsFile:true"));
                }


                XDocument xmlDocument = GenerateXmlFromRuleData(ruleDataList);




                rs.Data = xmlDocument.ToString();
                return rs;

            }
            catch (Exception ex)
            {
                rs.Message = ex.Message;
                rs.IsError = true;
                return rs;
            }
        }

        static XElement CreateRule(string name, string matchUrl, string actionUrl, string paramconditions)
        {
            string[] conditions = paramconditions.Split(";");
            var ruleElement = new XElement("rule",
                new XAttribute("name", name),
                new XAttribute("stopProcessing", "true"),
                new XElement("match", new XAttribute("url", matchUrl)),
                new XElement("action", new XAttribute("type", "Rewrite"), new XAttribute("url", actionUrl))
            );


            if (conditions.Length > 0)
            {
                var conditionsElement = new XElement("conditions");
                foreach (var condition in conditions)
                {
                    var conditionParts = condition.Split(':');
                    if (conditionParts[0] == "{QUERY_STRING}")
                    {
                        conditionsElement.Add(new XElement("add",
        new XAttribute("input", conditionParts[0]),
        new XAttribute("pattern", conditionParts[1])
    ));
                    }
                    else if (conditionParts[0] == "{REQUEST_FILENAME}")
                    {
                        conditionsElement.Add(new XElement("add",
                    new XAttribute("input", conditionParts[0]),
                    new XAttribute("matchType", conditionParts[1]),
                    new XAttribute("negate", conditionParts[2])
                    ));
                    }


                }
                ruleElement.Add(conditionsElement);
            }

            return ruleElement;
        }
        static XDocument GenerateXmlFromRuleData(List<RuleData> ruleDataList)
        {
            XDocument xmlDocument = new XDocument(
                new XElement("rewrite",
                    new XElement("rules",
                        ruleDataList.Select(ruleData =>
                            CreateRule(ruleData.Name, ruleData.MatchUrl, ruleData.ActionUrl, ruleData.Conditions)
                        )
                    )
                )
            );
            return xmlDocument;
        }
        class RuleData
        {
            public string Name { get; }
            public string MatchUrl { get; }
            public string ActionUrl { get; }
            public string Conditions { get; }

            public RuleData(string name, string matchUrl, string actionUrl, string conditions)
            {
                Name = name;
                MatchUrl = matchUrl;
                ActionUrl = actionUrl;
                Conditions = conditions;
            }
        }
    }
}
