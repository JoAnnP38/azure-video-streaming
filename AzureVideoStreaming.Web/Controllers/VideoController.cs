using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
            return Json(new Video("author", "title", "desc", "mp4", "vc1", "thumb", DateTime.Now));
        }

        public ActionResult GetAll()
        {
            return Json(new List<Video>(){new Video("author", "title", "desc", "mp4", "vc1", "thumb", DateTime.Now), new Video("author", "title", "desc", "mp4", "vc1", "thumb", DateTime.Now)});
        }

        public ActionResult GetLikes(string videoId)
        {
            return Json(new {Count = 4, LikedByCurrentUser = true});
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
                file.SaveAs(path);
                return path;

            }
            return "error";
            
        }
	}
}