using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureVideoStreaming.Phone.Models
{
    public class Like
    {
        public Like()
        {

        }

        public int Count { get; set; }
        public bool LikedByCurrentUser { get; set; }
    }
}
