using Org.BouncyCastle.Crypto.Modes;
using Steam.Core.Base.Models;

using System.Xml.Linq;

namespace Admin.PostsCategory
{
    public class PostsCategoryHelper
    {
        public PostsCategoryHelper()
        {
        }
        public Response<string> GenerateXMLRewriteUrl_old(string preSlugCate, List<Database.PostsCategory> listCate)
        {
            //string preSlugCate = "danh-muc";
            Response<string> rs = new Response<string>();
            var rules = new List<dynamic>();
            try
            {
                foreach (var item in listCate)
                {
                    if (!string.IsNullOrEmpty(item.WebsiteCatePage))
                    {
                        var ruleCateNoParam =
                                new
                                {
                                    Name = "Rewrite " + item.Slug + " Cate NoParam",
                                    StopProcessing = "true",
                                    MatchUrl = "^" + preSlugCate + "/" + item.Slug + "/$",
                                    ConditionsInput = "{REQUEST_FILENAME}",
                                    ConditionsMatchType = "IsFile",
                                    ConditionsNegate = "true",
                                    ActionType = "Rewrite",
                                    ActionUrl = item.WebsiteCatePage + "?cateSlug={R:1}&rootSlug=" + item.Slug
                                };
                        var ruleCate =
                                new
                                {
                                    Name = "Rewrite " + item.Slug + " Cate",
                                    StopProcessing = "true",
                                    MatchUrl = "^" + preSlugCate + "/" + item.Slug + "?/([a-zA-Z0-9-+']+)?$",
                                    ConditionsInput = "{REQUEST_FILENAME}",
                                    ConditionsMatchType = "IsFile",
                                    ConditionsNegate = "true",
                                    ActionType = "Rewrite",
                                    ActionUrl = item.WebsiteCatePage + "?cateSlug={R:1}&rootSlug=" + item.Slug
                                };
                        rules.Add(ruleCate);

                    }
                    if (!string.IsNullOrEmpty(item.WebsiteDetailPage))
                    {

                        var ruleDetail =
                        new
                        {
                            Name = "Rewrite " + item.Slug + " Detail",
                            StopProcessing = "true",
                            MatchUrl = "^" + item.Slug + "/([a-zA-Z0-9-+']+)?/?$",
                            ConditionsInput = "{REQUEST_FILENAME}",
                            ConditionsMatchType = "IsFile",
                            ConditionsNegate = "true",
                            ActionType = "Rewrite",
                            ActionUrl = item.WebsiteDetailPage + "?slug={R:1}",

                        };

                        rules.Add(ruleDetail);
                    }


                }

                var rewriteElement = new XElement("rewrite",
                    new XElement("rules",
                        from rule in rules
                        select new XElement("rule",
                            new XAttribute("name", rule.Name),
                            new XAttribute("stopProcessing", rule.StopProcessing),
                            new XElement("match", new XAttribute("url", rule.MatchUrl)),
                            new XElement("conditions",
                                new XElement("add",
                                    new XAttribute("input", rule.ConditionsInput),
                                    new XAttribute("matchType", rule.ConditionsMatchType),
                                    new XAttribute("negate", rule.ConditionsNegate)
                                )
                            ),
                            new XElement("action",
                                new XAttribute("type", rule.ActionType),
                                new XAttribute("url", rule.ActionUrl)
                            )
                        )
                    )
                );
                rs.Data = rewriteElement.ToString();
                return rs;

            }
            catch (Exception ex)
            {
                rs.Message = ex.Message;
                rs.IsError = true;
                return rs;
            }
        }

        public Response<string> GenerateXMLRewriteUrl(string preSlugCate,
            List<Database.PostsCategory> listCate 
            ,List<Admin.TemplatePage.Database.TemplatePage> listPageTemplate)
        {
            //string preSlugCate = "danh-muc";
            Response<string> rs = new Response<string>();
            var rules = new List<dynamic>();
            try
            {
                List<RuleData> ruleDataList = new List<RuleData>();
                foreach (var item in listCate)
                {

                    if (!string.IsNullOrEmpty(item.WebsiteCatePage))
                    {
                        var tempPage = listPageTemplate.Where(p => p.Url.ToLower().Trim() == item.WebsiteCatePage.ToLower().Trim()).FirstOrDefault();
                        var tempPar = tempPage.Parameters ?? "";
                       //var tempPar = "page:PageIndex:1;sex:sex:1";

                        var arrPar = tempPar.Split(";");
                        string paramDefault = "";
                        string paramRewrite = "";
                        string listParams = "";
                        var n = 1;
                        if(!String.IsNullOrEmpty(tempPar))
                        {
                            foreach (var part in arrPar)
                            {
                                var parPart = part.Split(":");
                                if(parPart.Length == 3) { 
                                paramDefault += string.Format("&{0}={1}", parPart[1].ToString(), parPart[2].ToString());
                                paramRewrite += string.Format("{{QUERY_STRING}}:{0}=(.*)", parPart[0].ToString()) + ";";
                                listParams += string.Format("{0}={{C:{1}}}", parPart[1].ToString(), n) + "&";
                                }
                                n++;
                            }
                        }    
       
                        ruleDataList.Add(
                         new RuleData(
                           string.Format("Rewrite {0} Cate - No Category No Params", item.Slug),
                         string.Format("^({0}/{1}/|{0}/{1})$", preSlugCate, item.Slug),
                         string.Format("{0}?cateSlug={1}&rootSlug={1}{2}", item.WebsiteCatePage, item.Slug, paramDefault),
                          ""));
                        ruleDataList.Add(
                            new RuleData(
                      string.Format("Rewrite {0} Cate - No Category", item.Slug),
                         string.Format("^{0}/{1}/$", preSlugCate, item.Slug),
                        string.Format("{0}?cateSlug={1}&rootSlug={1}&{2}", item.WebsiteCatePage, item.Slug, listParams),
                          paramRewrite));

                          ruleDataList.Add(
                           new RuleData(
                            string.Format("Rewrite {0} Cate", item.Slug),
                        string.Format("^({0}/{1}/([a-zA-Z0-9-+']+)/|{0}/{1}/([a-zA-Z0-9-+']+))$", preSlugCate, item.Slug),
                        string.Format("{0}?cateSlug={{R:1}}&rootSlug={1}{2}", item.WebsiteCatePage, item.Slug, paramDefault),
                          ""));
                        ruleDataList.Add(
                           new RuleData(
                            string.Format("Rewrite {0} Cate", item.Slug),
                        string.Format("^{0}/{1}/([a-zA-Z0-9-+']+)/$", preSlugCate, item.Slug),
                        string.Format("{0}?cateSlug={{R:1}}&rootSlug={1}&{2}", item.WebsiteCatePage, item.Slug, listParams),
                          paramRewrite));
                    }
                    if (!string.IsNullOrEmpty(item.WebsiteDetailPage))
                    {

                        ruleDataList.Add(
                            new RuleData(
                                string.Format("Rewrite {0} Detail", item.Slug),
                                string.Format("^{0}/([a-zA-Z0-9-+']+)?/?$", item.Slug),
                                string.Format("{0}?slug={{R:1}}&rootSlug={1}", item.WebsiteDetailPage,item.Slug),
                                "{REQUEST_FILENAME}:IsFile:true"));
                    }

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
