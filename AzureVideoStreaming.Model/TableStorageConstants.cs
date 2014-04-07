using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureVideoStreaming.Model
{
    public class TableStorageConstants
    {
        public const string UserPartitionKey = "users";
        public const string VideoPartitionKey = "videos";
        public const string VideoEncodingQueuePartitionKey = "videoEncodingQueues";

    }
}
