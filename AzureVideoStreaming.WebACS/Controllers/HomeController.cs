using AzureVideoStreaming.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace AzureVideoStreaming.WebACS.Content
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        private UserRepository userRepository = null;

        public HomeController()
        {
            this.userRepository = new UserRepository();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Upload()
        {
            return View();
        }

        public ActionResult Claims()
        {
            ViewBag.ClaimsIdentity = Thread.CurrentPrincipal.Identity;
            return View();
        }

        public ActionResult Register()
        {
        }
    }
}
