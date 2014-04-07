using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AzureVideoStreaming.Core;
using Microsoft.WindowsAzure.MediaServices.Client;

namespace AzureVideoStreaming.WebJob
{
    class Program
    {
        static void Main(string[] args)
        {
            for (;;)
            {
                CheckJobs();
                Thread.Sleep(1000);
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
                string mp4Url, vc1Url;
                if (videoService.GetJobOutput(queuedVideo.MediaServicesJobId, out mp4Url, out vc1Url) == JobState.Finished)
                {
                    Console.WriteLine("Job finished: " + queuedVideo.MediaServicesJobId);
                    var video = videoRepository.Get(queuedVideo.VideoId);
                    video.UrlMp4 = mp4Url;
                    video.UrlVc1 = vc1Url;
                    videoRepository.Update(video);

                    videoEncodingQueueRepository.Remove(queuedVideo.RowKey);
                }
            }
        }

        
    }
}
