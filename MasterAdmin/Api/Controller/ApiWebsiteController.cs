
using Admin.PostsCategory;
using Devsense.PHP.Phar;
using MasterAdmin.Repository;
using Microsoft.AspNetCore.Mvc;

using NLog;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using static MasterAdmin.Models.ApiWebsiteModel;

namespace MasterAdmin.ApiControllers
{
    [ApiController, Route("/api/[controller]")]
    public class ApiWebsiteController : Controller
    {
        //private readonly ILogger _logger;
        private readonly IApiWebsiteRepository _rep;

        public ApiWebsiteController(IApiWebsiteRepository rep)
        {
            _rep = rep;


        }
        [HttpGet("GetSitemap")]
        public ContentResult Sitemap()
        {
            try
            {
                string host = Request.Scheme + "://" + Request.Host;

                XmlDocument xdoc = new XmlDocument();
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.Encoding = Encoding.UTF8;

                UTF8StringWriter builder = new UTF8StringWriter();
                XmlWriter xml = XmlWriter.Create(builder, settings);


                xml.WriteStartDocument();
                xml.WriteStartElement("sitemapindex", "http://www.sitemaps.org/schemas/sitemap/0.9");
                //xml.WriteAttributeString("xmlns", "xsi", null, "http://www.sitemaps.org/schemas/sitemap/0.9");
                //xml.WriteAttributeString("xsi", "schemaLocation", null, "https://www.sitemaps.org/schemas/sitemap/0.9 https://www.sitemaps.org/schemas/sitemap/0.9/sitemap.xsd");

                xml.WriteStartElement("sitemap");
                xml.WriteElementString("loc", host + "/post-sitemap.xml");
                xml.WriteElementString("lastmod", DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ssK"));
                xml.WriteEndElement();

                //xml.WriteStartElement("sitemap");
                //xml.WriteElementString("loc", host + "/page-sitemap.xml");
                //xml.WriteElementString("lastmod", DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ssK"));
                //xml.WriteEndElement();

                xml.WriteStartElement("sitemap");
                xml.WriteElementString("loc", host + "/category-sitemap.xml");
                xml.WriteElementString("lastmod", DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ssK"));
                xml.WriteEndElement();

                xml.WriteStartElement("sitemap");
                xml.WriteElementString("loc", host + "/product-sitemap.xml");
                xml.WriteElementString("lastmod", DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ssK"));
                xml.WriteEndElement();

                xml.WriteStartElement("sitemap");
                xml.WriteElementString("loc", host + "/product_cat-sitemap.xml");
                xml.WriteElementString("lastmod", DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ssK"));
                xml.WriteEndElement();

                xml.WriteStartElement("sitemap");
                xml.WriteElementString("loc", host + "/collection-sitemap.xml");
                xml.WriteElementString("lastmod", DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ssK"));
                xml.WriteEndElement();


                xml.WriteEndElement();


                xml.WriteEndDocument();
                xml.Flush();


                //XmlWriter xml = XmlWriter.Create(S);



                //xdoc.WriteTo(xml);
                return new ContentResult
                {
                    ContentType = "application/xml",
                    Content = builder.ToString(),
                    StatusCode = 200
                };
            }
            catch (Exception ex)
            {
                return null;

            }
        }
        [HttpGet("GetPageSiteMap")]
        public ContentResult PageSitemap()
        {
            try
            {
                string host = Request.Scheme + "://" + Request.Host;

                XmlDocument xdoc = new XmlDocument();
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.Encoding = Encoding.UTF8;

                UTF8StringWriter builder = new UTF8StringWriter();

                using (XmlWriter xml = XmlWriter.Create(builder, settings))
                {
                    xml.WriteStartDocument();
                    xml.WriteStartElement("sitemapindex", "http://www.sitemaps.org/schemas/sitemap/0.9");
                    //xml.WriteAttributeString("xmlns", "xsi", null, "http://www.sitemaps.org/schemas/sitemap/0.9");
                    //xml.WriteAttributeString("xsi", "schemaLocation", null, "https://www.sitemaps.org/schemas/sitemap/0.9 https://www.sitemaps.org/schemas/sitemap/0.9/sitemap.xsd");

                    //xml.WriteStartElement("sitemap");
                    //xml.WriteElementString("loc", host + "/post-sitemap.xml");
                    //xml.WriteElementString("lastmod", DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ssK"));
                    //xml.WriteEndElement();




                    xml.WriteEndElement();


                    xml.WriteEndDocument();
                    xml.Flush();

                }
                //XmlWriter xml = XmlWriter.Create(S);



                //xdoc.WriteTo(xml);
                var a = builder;
                return new ContentResult
                {
                    ContentType = "application/xml",
                    Content = builder.ToString(),
                    StatusCode = 200
                };
            }
            catch (Exception ex)
            {
                return null;

            }
        }
        [HttpGet("GetPostSitemap")]
        public ContentResult PostSitemap()
        {
            try
            {
                List<SiteMapModel> listPost = _rep.GetListPost();
                List<SiteMapModel> listCate = _rep.GetListParentPostCategories();
                string host = Request.Scheme + "://" + Request.Host;

                Response.ContentType = "application/xml";
                XmlDocument xdoc = new XmlDocument();
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.Encoding = Encoding.UTF8;

                UTF8StringWriter builder = new UTF8StringWriter();

                using (XmlWriter writer = XmlWriter.Create(builder, settings))
                {
                    writer.WriteStartDocument();

                    // Add the stylesheet processing instruction

                    // Start the root element with namespaces and schema location
                    writer.WriteStartElement("urlset", "http://www.sitemaps.org/schemas/sitemap/0.9");
                    writer.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
                    writer.WriteAttributeString("xmlns", "image", null, "http://www.google.com/schemas/sitemap-image/1.1");

                    writer.WriteAttributeString("xsi", "schemaLocation", null, "http://www.sitemaps.org/schemas/sitemap/0.9 http://www.sitemaps.org/schemas/sitemap/0.9/sitemap.xsd http://www.google.com/schemas/sitemap-image/1.1 http://www.google.com/schemas/sitemap-image/1.1/sitemap-image.xsd");
                    foreach (var item in listCate)
                    {
                        AddUrlElement(writer, item);

                    }

                    // Add URL elements
                    foreach (var item in listPost)
                    {
                        AddUrlElementWithImages(writer,
                                   item
                               );
                    }


                    // End the root element
                    writer.WriteEndElement();

                    // End the document
                    writer.WriteEndDocument();
                    writer.Flush();

                }



                return new ContentResult
                {
                    ContentType = "application/xml",
                    Content = builder.ToString(),
                    StatusCode = 200
                };
            }
            catch (Exception ex)
            {
                return null;

            }
        }
        [HttpGet("GetPostCategorySitemap")]
        public ContentResult PostCategorySitemap()
        {
            try
            {
                string host = Request.Scheme + "://" + Request.Host;
                List<SiteMapModel> listCate = _rep.GetListPostCategories();

                Response.ContentType = "application/xml";
                XmlDocument xdoc = new XmlDocument();
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.Encoding = Encoding.UTF8;

                UTF8StringWriter builder = new UTF8StringWriter();

                using (XmlWriter writer = XmlWriter.Create(builder, settings))
                {
                    writer.WriteStartDocument();

                    // Add the stylesheet processing instruction

                    // Start the root element with namespaces and schema location
                    writer.WriteStartElement("urlset", "http://www.sitemaps.org/schemas/sitemap/0.9");
                    writer.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
                    writer.WriteAttributeString("xmlns", "image", null, "http://www.google.com/schemas/sitemap-image/1.1");

                    writer.WriteAttributeString("xsi", "schemaLocation", null, "http://www.sitemaps.org/schemas/sitemap/0.9 http://www.sitemaps.org/schemas/sitemap/0.9/sitemap.xsd http://www.google.com/schemas/sitemap-image/1.1 http://www.google.com/schemas/sitemap-image/1.1/sitemap-image.xsd");

                    // Add URL elements
                    foreach (var item in listCate)
                    {
                        AddUrlElement(writer, item);

                    }

                    // End the root element
                    writer.WriteEndElement();

                    // End the document
                    writer.WriteEndDocument();
                    writer.Flush();

                }



                return new ContentResult
                {
                    ContentType = "application/xml",
                    Content = builder.ToString(),
                    StatusCode = 200
                };
            }
            catch (Exception ex)
            {
                return null;

            }
        }
        //[HttpGet("GetListProductsSiteMap")]
        //public ContentResult ProductsSitemap()
        //{
        //    try
        //    {
        //        string host = Request.Scheme + "://" + Request.Host;
        //        List<SiteMapModel> listCate = _rep.ProductsSitemap();

        //        Response.ContentType = "application/xml";
        //        XmlDocument xdoc = new XmlDocument();
        //        XmlWriterSettings settings = new XmlWriterSettings();
        //        settings.Indent = true;
        //        settings.Encoding = Encoding.UTF8;

        //        UTF8StringWriter builder = new UTF8StringWriter();

        //        using (XmlWriter writer = XmlWriter.Create(builder, settings))
        //        {
        //            writer.WriteStartDocument();

        //            // Add the stylesheet processing instruction

        //            // Start the root element with namespaces and schema location
        //            writer.WriteStartElement("urlset", "http://www.sitemaps.org/schemas/sitemap/0.9");
        //            writer.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
        //            writer.WriteAttributeString("xmlns", "image", null, "http://www.google.com/schemas/sitemap-image/1.1");

        //            writer.WriteAttributeString("xsi", "schemaLocation", null, "http://www.sitemaps.org/schemas/sitemap/0.9 http://www.sitemaps.org/schemas/sitemap/0.9/sitemap.xsd http://www.google.com/schemas/sitemap-image/1.1 http://www.google.com/schemas/sitemap-image/1.1/sitemap-image.xsd");

        //            // Add URL elements
        //            foreach (var item in listCate)
        //            {
        //                AddUrlElement(writer, item);

        //            }

        //            // End the root element
        //            writer.WriteEndElement();

        //            // End the document
        //            writer.WriteEndDocument();
        //            writer.Flush();

        //        }



        //        return new ContentResult
        //        {
        //            ContentType = "application/xml",
        //            Content = builder.ToString(),
        //            StatusCode = 200
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;

        //    }
        //}
        //[HttpGet("GetProductCategorySiteMap")]
        //public ContentResult ProductCategorySiteMap()
        //{
        //    try
        //    {
        //        string host = Request.Scheme + "://" + Request.Host;
        //        List<SiteMapModel> listCate = _rep.GetListProductCategories();

        //        Response.ContentType = "application/xml";
        //        XmlDocument xdoc = new XmlDocument();
        //        XmlWriterSettings settings = new XmlWriterSettings();
        //        settings.Indent = true;
        //        settings.Encoding = Encoding.UTF8;

        //        UTF8StringWriter builder = new UTF8StringWriter();

        //        using (XmlWriter writer = XmlWriter.Create(builder, settings))
        //        {
        //            writer.WriteStartDocument();

        //            // Add the stylesheet processing instruction

        //            // Start the root element with namespaces and schema location
        //            writer.WriteStartElement("urlset", "http://www.sitemaps.org/schemas/sitemap/0.9");
        //            writer.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
        //            writer.WriteAttributeString("xmlns", "image", null, "http://www.google.com/schemas/sitemap-image/1.1");

        //            writer.WriteAttributeString("xsi", "schemaLocation", null, "http://www.sitemaps.org/schemas/sitemap/0.9 http://www.sitemaps.org/schemas/sitemap/0.9/sitemap.xsd http://www.google.com/schemas/sitemap-image/1.1 http://www.google.com/schemas/sitemap-image/1.1/sitemap-image.xsd");

        //            // Add URL elements
        //            foreach (var item in listCate)
        //            {
        //                AddUrlElement(writer, item);

        //            }

        //            // End the root element
        //            writer.WriteEndElement();

        //            // End the document
        //            writer.WriteEndDocument();
        //            writer.Flush();

        //        }



        //        return new ContentResult
        //        {
        //            ContentType = "application/xml",
        //            Content = builder.ToString(),
        //            StatusCode = 200
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;

        //    }
        //}
        //[HttpGet("GetCollectionSitemap")]
        //public ContentResult CollectionSitemap()
        //{
        //    try
        //    {
        //        string host = Request.Scheme + "://" + Request.Host;
        //        List<SiteMapModel> listCate = _rep.CollectionSitemap();

        //        Response.ContentType = "application/xml";
        //        XmlDocument xdoc = new XmlDocument();
        //        XmlWriterSettings settings = new XmlWriterSettings();
        //        settings.Indent = true;
        //        settings.Encoding = Encoding.UTF8;

        //        UTF8StringWriter builder = new UTF8StringWriter();

        //        using (XmlWriter writer = XmlWriter.Create(builder, settings))
        //        {
        //            writer.WriteStartDocument();

        //            writer.WriteStartElement("urlset", "http://www.sitemaps.org/schemas/sitemap/0.9");
        //            writer.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
        //            writer.WriteAttributeString("xmlns", "image", null, "http://www.google.com/schemas/sitemap-image/1.1");

        //            writer.WriteAttributeString("xsi", "schemaLocation", null, "http://www.sitemaps.org/schemas/sitemap/0.9 http://www.sitemaps.org/schemas/sitemap/0.9/sitemap.xsd http://www.google.com/schemas/sitemap-image/1.1 http://www.google.com/schemas/sitemap-image/1.1/sitemap-image.xsd");

        //            // thêm danh sách url
        //            foreach (var item in listCate)
        //            {
        //                AddUrlElement(writer, item);

        //            }


        //            writer.WriteEndElement();

        //            writer.WriteEndDocument();
        //            writer.Flush();

        //        }



        //        return new ContentResult
        //        {
        //            ContentType = "application/xml",
        //            Content = builder.ToString(),
        //            StatusCode = 200
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;

        //    }
        //}

        static void AddUrlElement(XmlWriter writer, SiteMapModel data)
        {

            writer.WriteStartElement("url");

            writer.WriteElementString("loc", data.loc);

            writer.WriteElementString("lastmod", data.lastmod);

            writer.WriteEndElement();
        }

        static void AddUrlElementWithImages(XmlWriter writer, SiteMapModel data)
        {
            // bắt đầu thẻ url
            writer.WriteStartElement("url");

            // thêm thẻ con loc
            writer.WriteElementString("loc", data.loc);

            // thêm thẻ con lastmod
            writer.WriteElementString("lastmod", data.lastmod);

            // Add image:image elements
            //foreach (string imageUrl in imageUrls)
            //{
            if (!string.IsNullOrEmpty(data.image_loc))
            {


                writer.WriteStartElement("image", "http://www.google.com/schemas/sitemap-image/1.1");

                writer.WriteElementString("image", "loc", null, data.image_loc);
                //writer.WriteElementString("image","title",null,"","");
                if (!String.IsNullOrEmpty(data.image_title))
                {
                    writer.WriteStartElement("image", "title", null);

                    writer.WriteCData(data.image_title);
                    writer.WriteEndElement();

                }
                if (!String.IsNullOrEmpty(data.image_caption))
                {
                    writer.WriteStartElement("image", "caption", null);

                    writer.WriteCData(data.image_caption);
                    writer.WriteEndElement();
                }
            }
            // You can add more image elements here if needed

            writer.WriteEndElement();
            //}

            // End URL element
            writer.WriteEndElement();
        }

    }
    public static class ApiWebsiteControllerHelper
    {
        public static string SerializeObject<T>(this T value)
        {
            var serializer = new XmlSerializer(typeof(T));
            var settings = new XmlWriterSettings
            {
                Encoding = new UTF8Encoding(true),
                Indent = false,
                OmitXmlDeclaration = false,
                NewLineHandling = NewLineHandling.None
            };

            using (var stringWriter = new UTF8StringWriter())
            {
                using (var xmlWriter = XmlWriter.Create(stringWriter, settings))
                {
                    serializer.Serialize(xmlWriter, value);
                }

                return stringWriter.ToString();
            }
        }


    }
    public class UTF8StringWriter : StringWriter
    {
        public override Encoding Encoding
        {
            get
            {
                return Encoding.UTF8;
            }
        }
    }


}
