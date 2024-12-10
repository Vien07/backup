using System;
using System.Linq;
using System.Xml;
using CMS.Services.TranslateServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DTO;
using System.IO;
using System.Text;

namespace CMS.Controllers
{
    public class SharedController : Controller
    {
        private readonly DBContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITranslateServices _translate;
        private string DefaultLang = ConstantStrings.DefaultLang;
        public SharedController(DBContext dBContext, IHttpContextAccessor httpContextAccessor, ITranslateServices translate)
        {
            _dbContext = dBContext;
            _httpContextAccessor = httpContextAccessor;
            _translate = translate;
        }
        public IActionResult Maintenance()
        {
            return View();
        }
        public JsonResult SetSessionLang(string lang)
        {
            try
            {
                _httpContextAccessor.HttpContext.Session.SetString("WebsiteLang", lang);
                return Json(_translate.GetUrl("url.home"));
            }
            catch (Exception ex)
            {
                HttpContext.Session.SetString("WebsiteLang", DefaultLang);
                throw;
            }

        }

        #region sitemap
        [Route("/sitemap.xml")]
        public void Sitemap()
        {
            try
            {
                string host = Request.Scheme + "://" + Request.Host;

                Response.ContentType = "application/xml";

                using (var xml = XmlWriter.Create(Response.Body, new XmlWriterSettings { Indent = true }))
                {
                    xml.WriteStartElement("sitemapindex", "http://www.sitemaps.org/schemas/sitemap/0.9");
                    xml.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
                    xml.WriteAttributeString("xsi", "schemaLocation", null, "http://www.sitemaps.org/schemas/sitemap/0.9 http://www.sitemaps.org/schemas/sitemap/0.9/siteindex.xsd");
                    xml.WriteComment("Powered by BizMaC - www.bizmac.com");

                    xml.WriteStartElement("sitemap");
                    xml.WriteElementString("loc", host + "/home.xml");
                    xml.WriteEndElement();

                    xml.WriteStartElement("sitemap");
                    xml.WriteElementString("loc", host + "/informations.xml");
                    xml.WriteEndElement();

                    xml.WriteStartElement("sitemap");
                    xml.WriteElementString("loc", host + "/feature.xml");
                    xml.WriteEndElement();

                    //xml.WriteStartElement("sitemap");
                    //xml.WriteElementString("loc", host + "/product.xml");
                    //xml.WriteEndElement();

                    xml.WriteStartElement("sitemap");
                    xml.WriteElementString("loc", host + "/news.xml");
                    xml.WriteEndElement();
                }
            }
            catch (Exception ex)
            {

            }

        }
        [Route("/home.xml")]
        public void SitemapHome()
        {
            string host = Request.Scheme + "://" + Request.Host;

            Response.ContentType = "application/xml";

            using (var xml = XmlWriter.Create(Response.Body, new XmlWriterSettings { Indent = true }))
            {
                xml.WriteStartDocument();

                // Write the root element.
                xml.WriteStartElement("urlset", "http://www.google.com/schemas/sitemap/0.9");
                // Write the xmlns:bk="urn:book" namespace declaration.
                xml.WriteAttributeString("xmlns", "xhtml", null, "http://www.w3.org/1999/xhtml");
                xml.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
                xml.WriteAttributeString("xmlns", "image", null, "http://www.google.com/schemas/sitemap-image/1.1");
                xml.WriteAttributeString("xsi", "schemaLocation", null, "http://www.sitemaps.org/schemas/sitemap/0.9 http://www.sitemaps.org/schemas/sitemap/0.9/sitemap.xsd");
                xml.WriteComment("Powered by BizMaC - www.bizmac.com");

                #region url home
                xml.WriteStartElement("url");

                xml.WriteElementString("loc", host + _translate.GetStringWithLang("url.home", DefaultLang));

                xml.WriteStartElement("xhtml", "link", null);
                xml.WriteAttributeString("rel", "alternate");
                xml.WriteAttributeString("href", host + _translate.GetStringWithLang("url.home", "en"));
                xml.WriteAttributeString("hreflang", "en");
                xml.WriteEndElement();

                xml.WriteElementString("priority", "1.0");
                xml.WriteElementString("lastmod", DateTime.Now.ToString("yyyy-MM-dd"));
                xml.WriteEndElement();
                #endregion

                #region url contact
                xml.WriteStartElement("url");

                xml.WriteElementString("loc", host + _translate.GetStringWithLang("url.contact", DefaultLang));

                xml.WriteStartElement("xhtml", "link", null);
                xml.WriteAttributeString("rel", "alternate");
                xml.WriteAttributeString("href", host + _translate.GetStringWithLang("url.contact", "en"));
                xml.WriteAttributeString("hreflang", "en");
                xml.WriteEndElement();

                xml.WriteElementString("priority", "0.9");
                xml.WriteElementString("lastmod", DateTime.Now.ToString("yyyy-MM-dd"));
                xml.WriteEndElement();
                #endregion

                #region url about
                xml.WriteStartElement("url");

                xml.WriteElementString("loc", host + _translate.GetStringWithLang("url.about", DefaultLang));

                xml.WriteStartElement("xhtml", "link", null);
                xml.WriteAttributeString("rel", "alternate");
                xml.WriteAttributeString("href", host + _translate.GetStringWithLang("url.about", "en"));
                xml.WriteAttributeString("hreflang", "en");
                xml.WriteEndElement();

                xml.WriteElementString("priority", "0.8");
                xml.WriteElementString("lastmod", DateTime.Now.ToString("yyyy-MM-dd"));
                xml.WriteEndElement();
                #endregion

                #region url product
                xml.WriteStartElement("url");

                xml.WriteElementString("loc", host + _translate.GetStringWithLang("url.product", DefaultLang));

                xml.WriteStartElement("xhtml", "link", null);
                xml.WriteAttributeString("rel", "alternate");
                xml.WriteAttributeString("href", host + _translate.GetStringWithLang("url.product", "en"));
                xml.WriteAttributeString("hreflang", "en");
                xml.WriteEndElement();

                xml.WriteElementString("priority", "0.9");
                xml.WriteElementString("lastmod", DateTime.Now.ToString("yyyy-MM-dd"));
                xml.WriteEndElement();
                #endregion

                // Write the close tag for the root element.
                xml.WriteEndElement();
            }
        }
        [Route("/informations.xml")]
        public void SitemapAbout()
        {
            string host = Request.Scheme + "://" + Request.Host;

            Response.ContentType = "application/xml";

            using (var xml = XmlWriter.Create(Response.Body, new XmlWriterSettings { Indent = true }))
            {
                xml.WriteStartDocument();

                // Write the root element.
                xml.WriteStartElement("urlset", "http://www.google.com/schemas/sitemap/0.9");

                // Write the xmlns:bk="urn:book" namespace declaration.
                xml.WriteAttributeString("xmlns", "xhtml", null, "http://www.w3.org/1999/xhtml");
                xml.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
                xml.WriteAttributeString("xmlns", "image", null, "http://www.google.com/schemas/sitemap-image/1.1");
                xml.WriteAttributeString("xsi", "schemaLocation", null, "http://www.sitemaps.org/schemas/sitemap/0.9 http://www.sitemaps.org/schemas/sitemap/0.9/sitemap.xsd http://www.w3.org/2002/08/xhtml/xhtml1-strict.xsd");
                xml.WriteComment("Powered by BizMaC - www.bizmac.com");


                var data = _dbContext.AboutDetails.Where(p => p.Enabled == true && p.Deleted == false).ToList();
                foreach (var item in data)
                {
                    xml.WriteStartElement("url");
                    var dataDefault = _dbContext.MultiLang_AboutDetails.Where(p => p.AboutDetailPid == item.Pid && p.LangKey == DefaultLang).FirstOrDefault();
                    xml.WriteElementString("loc", host + _translate.GetStringWithLang("url.about", DefaultLang) + dataDefault.Slug + ".html");
                    var multiLangData = _dbContext.MultiLang_AboutDetails.Where(p => p.AboutDetailPid == item.Pid).ToList();
                    foreach (var itemTemp in multiLangData)
                    {
                        if (itemTemp.LangKey == "en")
                        {
                            xml.WriteStartElement("xhtml", "link", null);
                            xml.WriteAttributeString("rel", "alternate");
                            xml.WriteAttributeString("href", host + _translate.GetStringWithLang("url.about", "en") + itemTemp.Slug + ".html");
                            xml.WriteAttributeString("hreflang", itemTemp.LangKey);
                            xml.WriteEndElement();
                        }
                    }

                    xml.WriteElementString("priority", "0.5");
                    xml.WriteElementString("lastmod", item.UpdateDate.ToString("yyyy-MM-dd"));
                    xml.WriteEndElement();
                }
                // Write the close tag for the root element.
                xml.WriteEndElement();
            }
        }

        [Route("/news.xml")]
        public void SitemapNews()
        {
            string host = Request.Scheme + "://" + Request.Host;

            Response.ContentType = "application/xml";

            using (var xml = XmlWriter.Create(Response.Body, new XmlWriterSettings { Indent = true }))
            {
                xml.WriteStartDocument();

                // Write the root element.
                xml.WriteStartElement("urlset", "http://www.google.com/schemas/sitemap/0.9");

                // Write the xmlns:bk="urn:book" namespace declaration.
                xml.WriteAttributeString("xmlns", "xhtml", null, "http://www.w3.org/1999/xhtml");
                xml.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
                xml.WriteAttributeString("xmlns", "image", null, "http://www.google.com/schemas/sitemap-image/1.1");
                xml.WriteAttributeString("xsi", "schemaLocation", null, "http://www.sitemaps.org/schemas/sitemap/0.9 http://www.sitemaps.org/schemas/sitemap/0.9/sitemap.xsd");
                xml.WriteComment("Powered by BizMaC - www.bizmac.com");

                var data = _dbContext.NewsDetails.Where(p => p.Enabled == true && p.Deleted == false).ToList();
                foreach (var item in data)
                {
                    xml.WriteStartElement("url");
                    var dataDefault = _dbContext.MultiLang_NewsDetails.Where(p => p.NewsDetailPid == item.Pid && p.LangKey == DefaultLang).FirstOrDefault();
                    xml.WriteElementString("loc", host + _translate.GetStringWithLang("url.news", DefaultLang) + dataDefault.Slug + ".html");
                    var multiLangData = _dbContext.MultiLang_NewsDetails.Where(p => p.NewsDetailPid == item.Pid).ToList();
                    foreach (var itemTemp in multiLangData)
                    {
                        if (itemTemp.LangKey == "en")
                        {
                            xml.WriteStartElement("xhtml", "link", null);
                            xml.WriteAttributeString("rel", "alternate");
                            xml.WriteAttributeString("href", host + _translate.GetStringWithLang("url.news", "en") + itemTemp.Slug + ".html");
                            xml.WriteAttributeString("hreflang", itemTemp.LangKey);
                            xml.WriteEndElement();
                        }
                    }

                    xml.WriteElementString("priority", "0.5");
                    xml.WriteElementString("lastmod", item.UpdateDate.ToString("yyyy-MM-dd"));
                    xml.WriteEndElement();
                }
                // Write the close tag for the root element.
                xml.WriteEndElement();
            }
        }
        [Route("/gallery.xml")]
        public void SitemapGallery()
        {
            string host = Request.Scheme + "://" + Request.Host;

            Response.ContentType = "application/xml";

            using (var xml = XmlWriter.Create(Response.Body, new XmlWriterSettings { Indent = true }))
            {
                xml.WriteStartDocument();

                // Write the root element.
                xml.WriteStartElement("urlset", "http://www.google.com/schemas/sitemap/0.9");

                // Write the xmlns:bk="urn:book" namespace declaration.
                xml.WriteAttributeString("xmlns", "xhtml", null, "http://www.w3.org/1999/xhtml");
                xml.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
                xml.WriteAttributeString("xmlns", "image", null, "http://www.google.com/schemas/sitemap-image/1.1");
                xml.WriteAttributeString("xsi", "schemaLocation", null, "http://www.sitemaps.org/schemas/sitemap/0.9 http://www.sitemaps.org/schemas/sitemap/0.9/sitemap.xsd");
                xml.WriteComment("Powered by BizMaC - www.bizmac.com");

                var data = _dbContext.GalleryDetails.Where(p => p.Enabled == true && p.Deleted == false).ToList();
                foreach (var item in data)
                {
                    xml.WriteStartElement("url");
                    var dataDefault = _dbContext.MultiLang_GalleryDetails.Where(p => p.GalleryDetailPid == item.Pid && p.LangKey == DefaultLang).FirstOrDefault();
                    xml.WriteElementString("loc", host + _translate.GetStringWithLang("url.gallery", DefaultLang) + dataDefault.Slug + ".html");
                    var multiLangData = _dbContext.MultiLang_GalleryDetails.Where(p => p.GalleryDetailPid == item.Pid).ToList();
                    foreach (var itemTemp in multiLangData)
                    {
                        if (itemTemp.LangKey == "en")
                        {
                            xml.WriteStartElement("xhtml", "link", null);
                            xml.WriteAttributeString("rel", "alternate");
                            xml.WriteAttributeString("href", host + _translate.GetStringWithLang("url.gallery", "en") + itemTemp.Slug + ".html");
                            xml.WriteAttributeString("hreflang", itemTemp.LangKey);
                            xml.WriteEndElement();
                        }
                    }

                    xml.WriteElementString("priority", "0.5");
                    xml.WriteElementString("lastmod", item.UpdateDate.ToString("yyyy-MM-dd"));
                    xml.WriteEndElement();
                }
                // Write the close tag for the root element.
                xml.WriteEndElement();
            }
        }

        [Route("/feature.xml")]
        public void SitemapFeature()
        {
            string host = Request.Scheme + "://" + Request.Host;

            Response.ContentType = "application/xml";

            using (var xml = XmlWriter.Create(Response.Body, new XmlWriterSettings { Indent = true }))
            {
                xml.WriteStartDocument();

                // Write the root element.
                xml.WriteStartElement("urlset", "http://www.google.com/schemas/sitemap/0.9");

                // Write the xmlns:bk="urn:book" namespace declaration.
                xml.WriteAttributeString("xmlns", "xhtml", null, "http://www.w3.org/1999/xhtml");
                xml.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
                xml.WriteAttributeString("xmlns", "image", null, "http://www.google.com/schemas/sitemap-image/1.1");
                xml.WriteAttributeString("xsi", "schemaLocation", null, "http://www.sitemaps.org/schemas/sitemap/0.9 http://www.sitemaps.org/schemas/sitemap/0.9/sitemap.xsd");
                xml.WriteComment("Powered by BizMaC - www.bizmac.com");

                var data = _dbContext.FeatureDetails.Where(p => p.Enabled == true && p.Deleted == false).ToList();
                foreach (var item in data)
                {
                    xml.WriteStartElement("url");
                    var dataDefault = _dbContext.MultiLang_FeatureDetails.Where(p => p.FeatureDetailPid == item.Pid && p.LangKey == DefaultLang).FirstOrDefault();
                    xml.WriteElementString("loc", host + _translate.GetStringWithLang("url.feature", DefaultLang) + dataDefault.Slug + ".html");
                    var multiLangData = _dbContext.MultiLang_FeatureDetails.Where(p => p.FeatureDetailPid == item.Pid).ToList();
                    foreach (var itemTemp in multiLangData)
                    {
                        if (itemTemp.LangKey == "en")
                        {
                            xml.WriteStartElement("xhtml", "link", null);
                            xml.WriteAttributeString("rel", "alternate");
                            xml.WriteAttributeString("href", host + _translate.GetStringWithLang("url.feature", "en") + itemTemp.Slug + ".html");
                            xml.WriteAttributeString("hreflang", itemTemp.LangKey);
                            xml.WriteEndElement();
                        }
                    }

                    xml.WriteElementString("priority", "0.5");
                    xml.WriteElementString("lastmod", item.UpdateDate.ToString("yyyy-MM-dd"));
                    xml.WriteEndElement();
                }
                // Write the close tag for the root element.
                xml.WriteEndElement();
            }
        }

        //[Route("/product.xml")]
        //public void SitemapProduct()
        //{
        //    string host = Request.Scheme + "://" + Request.Host;

        //    Response.ContentType = "application/xml";

        //    using (var xml = XmlWriter.Create(Response.Body, new XmlWriterSettings { Indent = true }))
        //    {
        //        xml.WriteStartDocument();

        //        // Write the root element.
        //        xml.WriteStartElement("urlset", "http://www.google.com/schemas/sitemap/0.9");

        //        // Write the xmlns:bk="urn:book" namespace declaration.
        //        xml.WriteAttributeString("xmlns", "xhtml", null, "http://www.w3.org/1999/xhtml");
        //        xml.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
        //        xml.WriteAttributeString("xmlns", "image", null, "http://www.google.com/schemas/sitemap-image/1.1");
        //        xml.WriteAttributeString("xsi", "schemaLocation", null, "http://www.sitemaps.org/schemas/sitemap/0.9 http://www.sitemaps.org/schemas/sitemap/0.9/sitemap.xsd");
        //        xml.WriteComment("Powered by BizMaC - www.bizmac.com");

        //        var data = _dbContext.ProductDetails.Where(p => p.Enabled == true && p.Deleted == false).ToList();
        //        foreach (var item in data)
        //        {
        //            xml.WriteStartElement("url");
        //            var dataDefault = _dbContext.MultiLang_ProductDetails.Where(p => p.ProductDetailPid == item.Pid && p.LangKey == DefaultLang).FirstOrDefault();
        //            xml.WriteElementString("loc", host + _translate.GetStringWithLang("url.product", DefaultLang) + dataDefault.Slug + ".html");
        //            var multiLangData = _dbContext.MultiLang_ProductDetails.Where(p => p.ProductDetailPid == item.Pid).ToList();
        //            foreach (var itemTemp in multiLangData)
        //            {
        //                if (itemTemp.LangKey == "en")
        //                {
        //                    xml.WriteStartElement("xhtml", "link", null);
        //                    xml.WriteAttributeString("rel", "alternate");
        //                    xml.WriteAttributeString("href", host + _translate.GetStringWithLang("url.product", "en") + itemTemp.Slug + ".html");
        //                    xml.WriteAttributeString("hreflang", itemTemp.LangKey);
        //                    xml.WriteEndElement();
        //                }
        //            }

        //            xml.WriteElementString("priority", "0.5");
        //            xml.WriteElementString("lastmod", item.UpdateDate.ToString("yyyy-MM-dd"));
        //            xml.WriteEndElement();
        //        }
        //        // Write the close tag for the root element.
        //        xml.WriteEndElement();
        //    }
        //}
        #endregion

        [Route("/robots.txt")]
        public void RobotFeatures()
        {
            try
            {
                var configItem = _dbContext.Configurations.ToList();
                foreach (var item in configItem)
                {
                    if (item.Key == "robots")
                    {
                        string RootDomain = _dbContext.Configurations.Where(p => p.NameKey == "RootDomain").FirstOrDefault().Value;
                        RootDomain = RootDomain.EndsWith("/") ? RootDomain.Remove(RootDomain.Length - 1) : RootDomain;
                        //string DisallowRobots = "User-agent: * \nAllow: /\nSitemap: " + RootDomain + "/sitemap.xml\nDisallow: /b-admin/\nDisallow: /*_escaped_fragment_";
                        string DisallowRobots = "User-agent: * \nAllow: /\nSitemap: " + RootDomain + "/sitemap.xml\nDisallow: /*_escaped_fragment_";
                        using (StreamWriter writer = new StreamWriter(Response.Body, Encoding.UTF8))
                        {
                            if (item.Value == "on")
                            {
                                writer.WriteLine(DisallowRobots);
                            }
                            else
                            {
                                writer.WriteLine(ConstantStrings.AllowRobots);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {

            }
        }

    }
}