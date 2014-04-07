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
            var table = client.GetTableReference(TableStorageConstants.VideoTableKey);
            table.CreateIfNotExists();
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


    }
}
