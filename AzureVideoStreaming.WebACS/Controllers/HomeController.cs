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
using AzureVideoStreaming.WebACS.Helpers;

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
            if (!IdentityHelper.IsUserRegistered())
                return RedirectToAction("Register");
            return View(_videoRepository.GetActive());
        }

        public ActionResult Upload()
        {
            if (!IdentityHelper.IsUserRegistered())
                return RedirectToAction("Register");
            return View();
        }

        [HttpPost]
        public ActionResult Upload(string title, string description, HttpPostedFileBase file)
        {
            if (!IdentityHelper.IsUserRegistered())
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
            if (IdentityHelper.IsUserRegistered())
                return RedirectToAction("Index");
            return View();
        }

        [HttpPost]
        public ActionResult Register(HomeRegisterVM model)
        {
            if (IdentityHelper.IsUserRegistered())
                return RedirectToAction("Index");

            if (!ModelState.IsValid)
                return View();

            var token = IdentityHelper.GetUserToken();
            if (string.IsNullOrEmpty(token))
                return View();
            
            var savedUser = this.userRepository.Add(new User(model.Username, token));
            if(savedUser!=null)
            {
                return RedirectToAction("Index");
            }
            else
                return View();
        }

        [HttpGet]
        public ActionResult Details(string videoId)
        {
            var video = this._videoRepository.Get(videoId);
            if (video == null)
                return new HttpNotFoundResult();

            var model = new HomeDetailsVM();
            model.Video = video;
            model.Comments = this._videoRepository.GetComments(videoId).ToList();
            return View(model);
        }
        [HttpPost]
        public ActionResult Details(HomeDetailsVM model)
        {
            if(string.IsNullOrEmpty(model.Comment))
                return RedirectToAction("Details", new { videoId = model.VideoId });

            var user = IdentityHelper.GetUserFromIdentity();
            var comment = new Comment(model.VideoId, IdentityHelper.GetUserToken(), model.Comment, user.Username);
            this._videoRepository.AddComment(comment);
            return RedirectToAction("Details", new { videoId = model.VideoId });
        }
    }
}
