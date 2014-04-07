using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureVideoStreaming.Phone.Models
{
    public class Video
    {
        public string VideoId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string UrlMp4 { get; set; }
        public string UrlVc1 { get; set; }
        public string ThumbnailUrl { get; set; }
        public DateTime DateUploaded { get; set; }
        public string AuthorUserId { get; set; }
    }
}
