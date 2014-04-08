using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureVideoStreaming.Model
{
    public class Comment : TableEntity
    {
        public Comment()
        {
            
        }
        public Comment(string videoId, string authorId, string text, string authorName)
        {
            PartitionKey = videoId;
            RowKey = Guid.NewGuid().ToString();
            Timestamp = DateTime.Now;

            VideoId = videoId;
            AuthorUserId = authorId;
            CommentText = text;
            AuthorName = authorName;
        }

        public string VideoId { get; set; }
        public string CommentText { get; set; }
        public string AuthorUserId { get; set; }

        public string AuthorName { get; set; }
    }
}
