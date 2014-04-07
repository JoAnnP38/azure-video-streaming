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
    public class VideoEncodingQueueRepository
    {
        private CloudStorageAccount _storageAccount;

        public VideoEncodingQueueRepository()
        {
            // Retrieve the storage account from the connection string.
           _storageAccount = CloudStorageAccount.Parse(
               ConfigurationManager.AppSettings["StorageConnectionString"]);

            var client = _storageAccount.CreateCloudTableClient();
            var table = client.GetTableReference(TableStorageConstants.VideoEncodingQueueTableKey);
            table.CreateIfNotExists();

        }


        public VideoEncodingQueue Add(VideoEncodingQueue queue)
        {
            var client = _storageAccount.CreateCloudTableClient();
            var table = client.GetTableReference(TableStorageConstants.VideoEncodingQueueTableKey);

            var insertOperation = TableOperation.Insert(queue);
            table.Execute(insertOperation);

            return queue;
        }

      

        public IList<VideoEncodingQueue> GetAll()
        {
            var client = _storageAccount.CreateCloudTableClient();
            var table = client.GetTableReference(TableStorageConstants.VideoEncodingQueueTableKey);

            TableQuery<VideoEncodingQueue> query = new TableQuery<VideoEncodingQueue>();
            return table.ExecuteQuery(query).ToList();

        } 
    }
}
