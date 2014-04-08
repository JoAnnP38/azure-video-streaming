using AzureVideoStreaming.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using AzureVideoStreaming.Model;
using AzureVideoStreaming.WebACS.Models;
using System.Web.Security;

namespace AzureVideoStreaming.WebACS.Content
{
    public class HomeController : Controller
    {
        private VideoRepository _videoRepository;
        private UserRepository userRepository = null;

        //
        // GET: /Home/
        public HomeController()
        {
            _videoRepository = new VideoRepository();
            this.userRepository = new UserRepository();

        }
       

        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated && string.IsNullOrEmpty(User.Identity.Name))
                return RedirectToAction("Register");
            return View(_videoRepository.GetActive());
        }

        public ActionResult Upload()
        {
            if (User.Identity.IsAuthenticated && string.IsNullOrEmpty(User.Identity.Name))
                return RedirectToAction("Register");
            return View();
        }

        [HttpPost]
        public ActionResult Upload(string title, string description, HttpPostedFileBase file)
        {
            if (User.Identity.IsAuthenticated && string.IsNullOrEmpty(User.Identity.Name))
                return RedirectToAction("Register");
            try
            {
                if (file.ContentLength > 0)
                {
                    var fileName = Guid.NewGuid() + "-" + Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
                    Directory.CreateDirectory(Server.MapPath("~/App_Data/uploads"));
                    file.SaveAs(path);

                    var videoRep = new VideoRepository();
                    var video = new Video("test", "Foo", "Bar", null, null, null, DateTime.Now);
                    videoRep.Add(video);

                    var videoService = new VideoService();
                    var asset = videoService.CreateAssetAndUploadSingleFile(path);
                    var job = videoService.CreateEncodingJob(asset, path);

                    var videoQueueRep = new VideoEncodingQueueRepository();
                    videoQueueRep.Add(new VideoEncodingQueue(video.RowKey, job.Id));

                }
            }
            catch (Exception e)
            {
                // notify
            }

            return RedirectToAction("Index");
        }


        public ActionResult Claims()
        {
            ViewBag.ClaimsIdentity = Thread.CurrentPrincipal.Identity;
            return View();
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(HomeRegisterVM model)
        {
            if (!ModelState.IsValid)
                return View();

            var identity = Thread.CurrentPrincipal.Identity as System.Security.Claims.ClaimsIdentity;
            var claim = identity.Claims.FirstOrDefault(t => t.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
            string token = "";
            if (claim != null)
                token = claim.Value;
            if (string.IsNullOrEmpty(token))
                return View();

            // Azure table storage ne voli forward slasheve
            token = token.Replace('/', '_');

            var user = this.userRepository.Get(token);
            if (user != null)
                return View();//user registered

            var savedUser = this.userRepository.Add(new User(model.Username, token));
            if(savedUser!=null)
            {
                FormsAuthentication.SetAuthCookie(model.Username, true);
                return RedirectToAction("Index");
            }
            else
                return View();
        }
    }
}
