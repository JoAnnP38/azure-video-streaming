using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using AzureVideoStreaming.Core;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.MediaServices.Client;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;

namespace AzureVideoStreaming.Worker
{
    public class WorkerRole : RoleEntryPoint
    {
        public override void Run()
        {
            // This is a sample worker implementation. Replace with your logic.
            Trace.TraceInformation("AzureVideoStreaming.Worker entry point called");

            while (true)
            {
                CheckJobs();
                Thread.Sleep(10000);
                
            }
        }

        private static void CheckJobs()
        {
            var videoEncodingQueueRepository = new VideoEncodingQueueRepository();
            var videoRepository = new VideoRepository();

            var queued = videoEncodingQueueRepository.GetAll();

            var videoService = new VideoService();

            foreach (var queuedVideo in queued)
            {
                string mp4Url, vc1Url, thumbnailUrl;
                if (videoService.GetJobOutput(queuedVideo.MediaServicesJobId, out mp4Url, out vc1Url, out thumbnailUrl) == JobState.Finished)
                {
                    Console.WriteLine("Job finished: " + queuedVideo.MediaServicesJobId);
                    var video = videoRepository.Get(queuedVideo.VideoId);
                    video.UrlMp4 = mp4Url;
                    video.UrlVc1 = vc1Url;
                    video.ThumbnailUrl = thumbnailUrl;
                    videoRepository.Update(video);

                    videoEncodingQueueRepository.Remove(queuedVideo.RowKey);
                }
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            return base.OnStart();
        }
    }
}
