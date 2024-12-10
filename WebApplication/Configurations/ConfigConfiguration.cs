using CMS.Areas.Configurations.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace CMS.Configurations
{
    public class ConfigConfiguration : IEntityTypeConfiguration<Configuration>
    {
        public void Configure(EntityTypeBuilder<Configuration> builder)
        {
            builder.HasData(
                new Configuration { Key = "website-Name", NameKey = "WebsiteName", Group = "Infomation", Type = "text" },
                new Configuration { Key = "email-Admin", NameKey = "EmailAdmin", Group = "Infomation", Type = "text", Value = "thanh.nc@bizmac.com.vn" },
                new Configuration { Key = "meta-Keywords", NameKey = "MetaKeywords", Group = "Infomation", Type = "text" },
                new Configuration { Key = "meta-Description", NameKey = "MetaDescription", Group = "Infomation", Type = "text" },
                new Configuration { Key = "code-Header", NameKey = "CodeHeader", Group = "Infomation", Type = "text" },
                new Configuration { Key = "code-Body", NameKey = "CodeBody", Group = "Infomation", Type = "text" },
                new Configuration { Key = "share-facebook", NameKey = "ShareFacebook", Group = "Infomation", Type = "text" },
                new Configuration { Key = "share-whatsapp", NameKey = "ShareWhatsApp", Group = "Infomation", Type = "text" },
                new Configuration { Key = "share-skype", NameKey = "ShareSkype", Group = "Infomation", Type = "text" },
                new Configuration { Key = "share-viber", NameKey = "ShareViber", Group = "Infomation", Type = "text" },
                new Configuration { Key = "share-twitter", NameKey = "ShareTwitter", Group = "Infomation", Type = "text" },
                new Configuration { Key = "share-youtube", NameKey = "ShareYoutube", Group = "Infomation", Type = "text" },
                new Configuration { Key = "share-telegram", NameKey = "ShareTelegram", Group = "Infomation", Type = "text" },
                new Configuration { Key = "share-linkedin", NameKey = "ShareLinkedin", Group = "Infomation", Type = "text" },
                new Configuration { Key = "share-instagram", NameKey = "ShareInstagram", Group = "Infomation", Type = "text" },
                new Configuration { Key = "share-pinterest", NameKey = "SharePinterest", Group = "Infomation", Type = "text" },
                new Configuration { Key = "share-podcast", NameKey = "SharePodcast", Group = "Infomation", Type = "text" },
                new Configuration { Key = "link-policy", NameKey = "LinkPolicy", Group = "Infomation", Type = "text" },
                new Configuration { Key = "share-zalo", NameKey = "ShareZalo", Group = "Infomation", Type = "text" },
                new Configuration { Key = "share-tiktok", NameKey = "ShareTiktok", Group = "Infomation", Type = "text" },
                new Configuration { Key = "link-bct", NameKey = "LinkBCT", Group = "Infomation", Type = "text" },
                new Configuration { Key = "link-certificate", NameKey = "LinkCertificate", Group = "Infomation", Type = "text" },
                new Configuration { Key = "root-domain", NameKey = "RootDomain", Group = "Infomation", Type = "text" });

            builder.HasData(
                new Configuration { Key = "images-upload-prefix", NameKey = "ImagePrefix", Group = "Images", Type = "text", Value = "bizmac_" },
                new Configuration { Key = "image-upload-min-width", NameKey = "ImageMinWidth", Group = "Images", Value = "600", Type = "text" },
                new Configuration { Key = "image-upload-max-width", NameKey = "ImageMaxWidth", Group = "Images", Value = "1366", Type = "text" },
                new Configuration { Key = "banner-slide-upload-width", NameKey = "BannerSlideWidth", Group = "Images", Value = "1920", Type = "text" },
                new Configuration { Key = "logo", NameKey = "Logo", Group = "Images", Type = "file", Value = "bizmac.png" },
                new Configuration { Key = "logo-footer", NameKey = "LogoFooter", Group = "Images", Type = "file", Value = "bizmac.png" },
                new Configuration { Key = "favicon", NameKey = "Favicon", Group = "Images", Type = "file" },
                new Configuration { Key = "default-og-image", NameKey = "DefaultOgImage", Group = "Images", Type = "file" },
                new Configuration { Key = "faq-image", NameKey = "FAQImage", Group = "Images", Type = "file" },
                new Configuration { Key = "home-image", NameKey = "HomeImage", Group = "Images", Type = "file" },
                new Configuration { Key = "home-image-mobile", NameKey = "HomeImageMobile", Group = "Images", Type = "file" },
                new Configuration { Key = "feature-image", NameKey = "FeatureImage", Group = "Images", Type = "file" },
                new Configuration { Key = "bct-image", NameKey = "BctImage", Group = "Images", Type = "file" },
                new Configuration { Key = "certificate-image", NameKey = "CertificateImage", Group = "Images", Type = "file" },
                new Configuration { Key = "popup-delay-time", NameKey = "PopupDelayTime", Group = "Images", Value = "2", Type = "text" },
                new Configuration { Key = "watermark-image", NameKey = "WatermarkImage", Group = "Images", Type = "file" },
                new Configuration { Key = "watermark-text", NameKey = "WatermarkText", Group = "Images", Type = "text" },
                new Configuration { Key = "watermark-opacity", NameKey = "WatermarkOpacity", Group = "Images", Type = "text", Value = "40" },
                new Configuration { Key = "watermark-type", NameKey = "WatermarkType", Group = "Images", Type = "radio", Value = "image" },
                new Configuration { Key = "how-image-list-show", NameKey = "HowImageListShow", Group = "Images", Type = "radio", Value = "slide" },
                new Configuration { Key = "position-image-list-show", NameKey = "PositionImageListShow", Group = "Images", Type = "radio", Value = "top" },
                new Configuration { Key = "watermark-position", NameKey = "WatermarkPosition", Group = "Images", Type = "radio", Value = "bottomRight" });

            builder.HasData(
                new Configuration { Key = "reCaptcha-site-key", NameKey = "reCapchaSiteKey", Group = "Other", Type = "text" },
                new Configuration { Key = "reCaptcha-secret-key", NameKey = "reCatchaSecretKey", Group = "Other", Type = "text" },
                new Configuration { Key = "google-signin-key", NameKey = "GoogleSignInKey", Group = "Infomation", Type = "text" },
                new Configuration { Key = "facebook-appid", NameKey = "FacebookAppId", Group = "Infomation", Type = "text" },
                new Configuration { Key = "zalo-oaid", NameKey = "ZaloOAId", Group = "Infomation", Type = "text", Value = "1" },

                new Configuration { Key = "hot-faq-limit", NameKey = "HotFAQLimit", Group = "Other", Type = "text", Value = "5" },
                new Configuration { Key = "hot-news-limit", NameKey = "HotNewsLimit", Group = "Other", Type = "text", Value = "5" },
                new Configuration { Key = "hot-product-limit", NameKey = "HotProductLimit", Group = "Other", Type = "text", Value = "5" },
                new Configuration { Key = "hot-project-limit", NameKey = "HotProjectLimit", Group = "Other", Type = "text", Value = "5" },
                new Configuration { Key = "hot-feature-limit", NameKey = "HotFeatureLimit", Group = "Other", Type = "text", Value = "5" },
                new Configuration { Key = "hot-gallery-limit", NameKey = "HotGalleryLimit", Group = "Other", Type = "text", Value = "5" },
                new Configuration { Key = "relate-faq-limit", NameKey = "RelateFAQLimit", Group = "Other", Type = "text", Value = "5" },
                new Configuration { Key = "relate-news-limit", NameKey = "RelateNewsLimit", Group = "Other", Type = "text", Value = "5" },
                new Configuration { Key = "relate-product-limit", NameKey = "RelateProductLimit", Group = "Other", Type = "text", Value = "5" },
                new Configuration { Key = "relate-project-limit", NameKey = "RelateProjectLimit", Group = "Other", Type = "text", Value = "5" },
                new Configuration { Key = "relate-feature-limit", NameKey = "RelateFeatureLimit", Group = "Other", Type = "text", Value = "5" },
                new Configuration { Key = "relate-gallery-limit", NameKey = "RelateGalleryLimit", Group = "Other", Type = "text", Value = "5" },
                new Configuration { Key = "date-format", NameKey = "DateFormat", Group = "Other", Type = "select", Value = "DD/MM/YYYY" },
                new Configuration { Key = "page-limit", NameKey = "PageLimit", Group = "Other", Type = "text", Value = "12" },
                new Configuration { Key = "page-limit-detail", NameKey = "PageLimitDetail", Group = "Other", Type = "text", Value = "6" },
                new Configuration { Key = "robots", NameKey = "Robots", Group = "Other", Type = "check" },
                new Configuration { Key = "maintenance", NameKey = "Maintenance", Group = "Other", Type = "check" },
                new Configuration { Key = "recaptcha", NameKey = "Recaptcha", Group = "Other", Type = "check" },
                new Configuration { Key = "page-limit-admin", NameKey = "PageLimitAdmin", Group = "Other", Type = "text", Value = "12" },
                new Configuration { Key = "watermark-active", NameKey = "WatermarkActive", Group = "Other", Type = "check" },
                new Configuration { Key = "watermark-picThumb-active", NameKey = "WatermarkPicThumbActive", Group = "Other", Type = "check" },
                new Configuration { Key = "google-login", NameKey = "GoogleLogin", Group = "Other", Type = "check" },
                new Configuration { Key = "facebook-login", NameKey = "FacebookLogin", Group = "Other", Type = "check" },
                new Configuration { Key = "seo-config", NameKey = "SEOConfig", Group = "Other", Type = "check", Value = "off" },
                new Configuration { Key = "product-code", NameKey = "ProductCode", Group = "Other", Type = "text" },
                new Configuration { Key = "money-format", NameKey = "FormatMoney", Group = "Other", Type = "select", Value = "." });

            builder.HasData(
                new Configuration { Key = "email-fromName", NameKey = "EmailFromName", Group = "Email", Type = "text", Value = "bizmac.com" },
                new Configuration { Key = "email-fromEmail", NameKey = "EmailFromEmail", Group = "Email", Type = "text", Value = "thanh.nc@bizmac.com.vn" },
                new Configuration { Key = "email-SMTPServer", NameKey = "EmailSMTPServer", Group = "Email", Type = "text", Value = "smtp.gmail.com" },
                new Configuration { Key = "email-SMTPUser", NameKey = "EmailSMTPUser", Group = "Email", Type = "text", Value = "noreply.smtp.web@gmail.com" },
                new Configuration { Key = "email-SMTPPassword", NameKey = "EmailSMTPPassword", Group = "Email", Type = "text", Value = "tjthlgoblzekmpud" },
                new Configuration { Key = "email-port", NameKey = "EmailPort", Group = "Email", Type = "text", Value = "587" },
                new Configuration { Key = "email-SSLTLSEncryption", NameKey = "EmailEncryption", Group = "Email", Type = "select", Value = "TLS" },
                new Configuration { Key = "email-GlobalEmailHeader", NameKey = "EmailGlobalEmailHeader", Group = "Email", Type = "text", Value = "" },
                new Configuration { Key = "email-GlobalEmailFooter", NameKey = "EmailGlobalEmailFooter", Group = "Email", Type = "text", Value = "" });


            builder.HasData(
               new Configuration { Key = "tax-code-company", NameKey = "TaxCodeCompany", Group = "Infomation", Type = "text", Value = "" },
               new Configuration { Key = "display-ibanking", NameKey = "DisplayiBanking", Group = "Infomation", Type = "check" },
               new Configuration { Key = "ibanking-name", NameKey = "iBankingName", Group = "Infomation", Type = "text", Value = "" },
               new Configuration { Key = "ibanking-info", NameKey = "iBankingInfo", Group = "Infomation", Type = "text", Value = "" },
               new Configuration { Key = "display-momo", NameKey = "DisplayMomo", Group = "Infomation", Type = "check" },
               new Configuration { Key = "momo-api", NameKey = "MomoApi", Group = "Infomation", Type = "text", Value = "" },
               new Configuration { Key = "momo-partner-code", NameKey = "MomoPartnerCode", Group = "Infomation", Type = "select", Value = "" },
               new Configuration { Key = "momo-access-key", NameKey = "MomoAccessKey", Group = "Infomation", Type = "text", Value = "" },
               new Configuration { Key = "momo-secrect-key", NameKey = "MomoSecrectKey", Group = "Infomation", Type = "text", Value = "" });
        }
    }
}
