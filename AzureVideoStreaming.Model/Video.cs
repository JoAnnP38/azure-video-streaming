using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureVideoStreaming.Model
{
    public class Video : TableEntity
    {
        public Video()
        {
        }

        public Video(string authorId, string title, string description, string urlMp4, string urlVc1, string thumbnailUrl, DateTime dateUploaded, string authorName = "anonymous")
        {
            PartitionKey = TableStorageConstants.VideoPartitionKey;
            RowKey = Guid.NewGuid().ToString();
            Timestamp = DateTime.Now;

            AuthorUserId = authorId;
            Title = title;
            Description = description;
            UrlMp4 = urlMp4;
            UrlVc1 = urlVc1;
            ThumbnailUrl = thumbnailUrl;
            DateUploaded = dateUploaded;
            AuthorName = authorName;
        }

        public string Title { get; set; }
        public string Description { get; set; }
        public string UrlMp4 { get; set; }
        public string UrlVc1 { get; set; }
        public string ThumbnailUrl { get; set; }
        public DateTime DateUploaded { get; set; }
        public string AuthorUserId { get; set; }

        public string AuthorName { get; set; }
    }
}
