using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureVideoStreaming.Model
{
    public class Like : TableEntity
    {
        public Like(string videoId, string authorId)
        {
            PartitionKey = videoId;
            RowKey = Guid.NewGuid().ToString();
            Timestamp = DateTime.Now;

            AuthorId = authorId;
            VideoId = videoId;
        }

        public string AuthorId { get; set; }
        public string VideoId { get; set; }
    }
}
