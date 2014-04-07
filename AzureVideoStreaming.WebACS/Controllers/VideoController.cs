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
        private VideoRepository _videoRepository;


        public VideoController()
        {
           _videoRepository = new VideoRepository();
        }

        //
        // GET: /Video/
        public ActionResult Index(string videoId)
        {
            return View(_videoRepository.Get(videoId));
        }

        public ActionResult Get(string videoId)
        {
            return Json(_videoRepository.Get(videoId), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAll()
        {
            return Json(_videoRepository.GetAll(),JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetLikes(string videoId, string userId)
        {
            bool userLiked;
            var likeCount = _videoRepository.LikesCount(videoId, userId, out userLiked);
            return Json(new { Count = likeCount, LikedByCurrentUser = userLiked }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetComments(string videoId)
        {
            return Json(_videoRepository.GetComments(videoId), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Upload()
        {
            return View();
        }

	}
}