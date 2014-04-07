using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AzureVideoStreaming.Core;
using AzureVideoStreaming.Model;

namespace AzureVideoStreaming.Web.Controllers
{
    public class VideoController : Controller
    {
        //
        // GET: /Video/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Get(string videoId)
        {
            return Json(new Video("author", "title", "desc", "mp4", "vc1", "thumb", DateTime.Now), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAll()
        {
            return Json(new List<Video>() { new Video("author", "title", "desc", "mp4", "vc1", "thumb", DateTime.Now), new Video("author", "title", "desc", "mp4", "vc1", "thumb", DateTime.Now) }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetLikes(string videoId)
        {
            return Json(new { Count = 4, LikedByCurrentUser = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public string Upload(HttpPostedFileBase file)
        {
            if (file.ContentLength > 0)
            {
                var fileName = Guid.NewGuid().ToString();
                var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
                Directory.CreateDirectory(Server.MapPath("~/App_Data/uploads"));
                file.SaveAs(path);

                var videoService = new VideoService();
                var asset = videoService.CreateAssetAndUploadSingleFile(path);
                videoService.CreateEncodingJob(asset, path);

                return path;
            }
            return "error";
            
        }
	}
}