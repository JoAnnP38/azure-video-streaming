using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureVideoStreaming.Model
{
    public class VideoEncodingQueue : TableEntity
    {
        public VideoEncodingQueue(string videoId, string mediaServiceJobId)
        {
            RowKey = Guid.NewGuid().ToString();
            PartitionKey = "VideoEncodingQueue";
            Timestamp = DateTime.Now;

            VideoId = videoId;
            MediaServicesJobId = mediaServiceJobId;
        }
        public string VideoId { get; set; }
        public string MediaServicesJobId { get; set; }
    }
}
