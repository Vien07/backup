using System.Diagnostics;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Steam.Core.FileManager.Model;

namespace Steam.Core.FileManager.Controllers
{
    public class HomeController : Controller
    {
        public AdminLogin adminLogin { get; }
        public HomeController(IOptions<AdminLogin> adminLogin)
        {
            this.adminLogin = adminLogin.Value;
        }


        [Route("")]
        public ActionResult Index(string akey)
        {
            return View();

            //if (akey == this.adminLogin.AccessKey)
            //{
            //    return View();

            //}
            //else
            //{
            //    return Redirect("/error");
            //}
        }

        [Route("ckeditor")]
        public ActionResult CKEditor(string akey)
        {
            if (akey == this.adminLogin.AccessKey)
            {
                return View();

            }
            else
            {
                return Redirect("/error");
            }
        }

        [Route("tiny-mce-4")]
        public ActionResult TinyMCE4(string akey)
        {
            if (akey == this.adminLogin.AccessKey)
            {
                return View();

            }
            else
            {
                return Redirect("/error");
            }
        }

        [Route("tiny-mce-5")]
        public ActionResult TinyMCE5(string akey)
        {
            if (akey == this.adminLogin.AccessKey)
            {
                return View();

            }
            else
            {
                return Redirect("/error");
            }
        }

        [Route("iframe")]
        public ActionResult IFrame(string akey)
        {
            if (akey == this.adminLogin.AccessKey)
            {
                return View();

            }
            else
            {
                return Redirect("/error");
            }
        }

        [Route("standalone")]
        public ActionResult Standalone(string akey)
        {
            if (akey == this.adminLogin.AccessKey)
            {
                return View();

            }
            else
            {
                return Redirect("/error");
            }
        }

        [Route("error")]
        public IActionResult Error()
        {
            return View();
        }
    }
}