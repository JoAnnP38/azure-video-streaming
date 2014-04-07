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
            var rep = new VideoEncodingQueueRepository();

            var queued = rep.GetAll();

            var videoService = new VideoService();

            foreach (var video in queued)
            {
                string mp4Url, vc1Url;
                if (videoService.GetJobOutput(video.MediaServicesJobId, out mp4Url, out vc1Url) == JobState.Finished)
                {
                    // TODO: update video entity and delete entity from videoqueue table
                }
            }
        }

        
    }
}
