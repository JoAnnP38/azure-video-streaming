using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureVideoStreaming.Phone.Models
{
    public class Comment
    {
        public Comment()
        {
            
        }

        public string RowKey { get; set; }
        public string VideoId { get; set; }
        public string CommentText { get; set; }
        public string AuthorUserId { get; set; }

        public string AuthorName { get; set; }
    }
}
