using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using AzureVideoStreaming.Core;
using AzureVideoStreaming.Model;

namespace AzureVideoStreaming.WebACS.Content
{
    public class HomeController : Controller
    {
        private VideoRepository _videoRepository;
        //
        // GET: /Home/
        public HomeController()
        {
            _videoRepository = new VideoRepository();

        }
        public ActionResult Index()
        {

            return View(_videoRepository.GetActive());
        }

        public ActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Upload(string title, string description, HttpPostedFileBase file)
        {
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
    }
}
