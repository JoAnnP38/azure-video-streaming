using System;
using System.IO;
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
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Get(string videoId)
        {
            return Json(_videoRepository.Get(videoId), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAll()
        {
            return Json(_videoRepository.GetAll(), JsonRequestBehavior.AllowGet);
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

        [HttpPost]
        public string Upload(string title, string description, HttpPostedFileBase file)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                return "Must enter title";
            }
            else if (file == null || string.IsNullOrWhiteSpace(file.FileName))
            {
                return "Must select file";
            }

            if (file.ContentLength > 0)
            {
                var fileName = Guid.NewGuid() + "-" + Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
                Directory.CreateDirectory(Server.MapPath("~/App_Data/uploads"));
                file.SaveAs(path);

                var videoRep = new VideoRepository();
                var video = new Video("test", title, description, null, null, null, DateTime.Now);
                videoRep.Add(video);

                var videoService = new VideoService();
                var asset = videoService.CreateAssetAndUploadSingleFile(path);
                var job = videoService.CreateEncodingJob(asset, path);

                var videoQueueRep = new VideoEncodingQueueRepository();
                videoQueueRep.Add(new VideoEncodingQueue(video.RowKey, job.Id));

                return path;
            }

            return "error";
        }
    }
}