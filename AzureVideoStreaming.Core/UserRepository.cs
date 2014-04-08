using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AzureVideoStreaming.Model;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureVideoStreaming.Core
{
    public class UserRepository
    {
        private CloudStorageAccount _storageAccount;

        public UserRepository()
        {


            // Retrieve the storage account from the connection string.
            _storageAccount = CloudStorageAccount.Parse(
                ConfigurationManager.AppSettings["StorageConnectionString"]);

            var client = _storageAccount.CreateCloudTableClient();
            client.GetTableReference(TableStorageConstants.UserTableKey).CreateIfNotExists();
        }

        
        public User Get(string userId)
        {
            var client = _storageAccount.CreateCloudTableClient();
            var table = client.GetTableReference(TableStorageConstants.UserTableKey);
            var retrieveOperation = TableOperation.Retrieve<User>(TableStorageConstants.UserPartitionKey, userId);

           
                var retrievedResult = table.Execute(retrieveOperation);
                var entity = retrievedResult.Result as User;
                return entity;

       
        }

        public IList<User> GetAll()
        {
            var client = _storageAccount.CreateCloudTableClient();
            var table = client.GetTableReference(TableStorageConstants.UserTableKey);

            var query = new TableQuery<User>();
            return table.ExecuteQuery(query).ToList();

        }


        public User Add(User user)
        {
            var client = _storageAccount.CreateCloudTableClient();
            var table = client.GetTableReference(TableStorageConstants.UserTableKey);

            var insertOperation = TableOperation.Insert(user);
            table.Execute(insertOperation);

            return user;
        }


    }
}
