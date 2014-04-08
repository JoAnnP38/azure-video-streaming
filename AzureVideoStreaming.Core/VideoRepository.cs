using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AzureVideoStreaming.Model;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureVideoStreaming.Core
{
    public class VideoRepository 
    {
        private readonly CloudStorageAccount _storageAccount;

        public VideoRepository()
        {
            // Retrieve the storage account from the connection string.
            _storageAccount = CloudStorageAccount.Parse(
                ConfigurationManager.AppSettings["StorageConnectionString"]);

            var client = _storageAccount.CreateCloudTableClient();
            client.GetTableReference(TableStorageConstants.VideoTableKey).CreateIfNotExists();
            client.GetTableReference(TableStorageConstants.LikeTableKey).CreateIfNotExists();
            client.GetTableReference(TableStorageConstants.CommentTableKey).CreateIfNotExists();
        }

        public IList<Video> GetActive()
        {
            return
                GetAll()
                    .Where(
                        v =>
                            !string.IsNullOrEmpty(v.ThumbnailUrl) && !string.IsNullOrEmpty(v.UrlMp4) &&
                            !string.IsNullOrEmpty(v.UrlVc1))
                    .ToList();
        }

      

        public Video Get(string videoId)
        {
            var client = _storageAccount.CreateCloudTableClient();
            var table = client.GetTableReference(TableStorageConstants.VideoTableKey);
            var retrieveOperation = TableOperation.Retrieve<Video>(TableStorageConstants.VideoPartitionKey, videoId);

            var retrievedResult = table.Execute(retrieveOperation);

            var entity = retrievedResult.Result as Video;
            return entity;
        }


        public Video Add(Video video)
        {
            var client = _storageAccount.CreateCloudTableClient();
            var table = client.GetTableReference(TableStorageConstants.VideoTableKey);

            var insertOperation = TableOperation.Insert(video);
            table.Execute(insertOperation);

            return video;
        }

        public Video Update(Video video)
        {
            var client = _storageAccount.CreateCloudTableClient();
            var table = client.GetTableReference(TableStorageConstants.VideoTableKey);

            var replaceOperation = TableOperation.InsertOrReplace(video);
            table.Execute(replaceOperation);
            return video;
        }

        public IList<Video> GetAll()
        {
            var client = _storageAccount.CreateCloudTableClient();
            var table = client.GetTableReference(TableStorageConstants.VideoTableKey);

            TableQuery<Video> query = new TableQuery<Video>();
            return table.ExecuteQuery(query).ToList();
            
        }

        public int LikesCount(string videoId, string userId, out bool userLiked)
        {
            var client = _storageAccount.CreateCloudTableClient();
            var table = client.GetTableReference(TableStorageConstants.LikeTableKey);

            TableQuery<Like> query = new TableQuery<Like>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, videoId));

            var results = query.Execute().ToList();
            userLiked = results.Any(l => l.AuthorId == userId);
            
            return results.Count();

        }

        public IList<Comment> GetComments(string videoId)
        {
            var client = _storageAccount.CreateCloudTableClient();
            var table = client.GetTableReference(TableStorageConstants.CommentTableKey);

            TableQuery<Comment> query = new TableQuery<Comment>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, videoId));

            return query.Execute().ToList();
        }

        public Comment AddComment(Comment comment)
        {
            var client = _storageAccount.CreateCloudTableClient();
            var table = client.GetTableReference(TableStorageConstants.CommentTableKey);

            var insertOperation = TableOperation.Insert(comment);
            table.Execute(insertOperation);

            return comment;

        }

        public Like AddLike(Like like)
        {
            var client = _storageAccount.CreateCloudTableClient();
            var table = client.GetTableReference(TableStorageConstants.LikeTableKey);

            var insertOperation = TableOperation.Insert(like);
            table.Execute(insertOperation);

            return like;

        }
    }
}
