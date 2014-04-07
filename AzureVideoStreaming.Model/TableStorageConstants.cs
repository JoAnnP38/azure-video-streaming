using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureVideoStreaming.Model
{
    public class TableStorageConstants
    {
        public const string UserPartitionKey = "usersPartition";
        public const string VideoPartitionKey = "videosPartition";
        public const string VideoEncodingQueuePartitionKey = "videoEncodingQueuesPartition";

        public const string UserTableKey = "userTable";
        public const string VideoTableKey = "videoTable";
        public const string LikeTableKey = "likeTable";
        public const string VideoEncodingQueueTableKey = "videoEncodingQueueTable";
        public const string CommentTableKey = "commentTable";
    }
}
