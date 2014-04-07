using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureVideoStreaming.Model
{
    public class User : TableEntity
    {
        public User()
        {
            
        }
        public User(string name)
        {
            PartitionKey = TableStorageConstants.UserPartitionKey;
            //RowKey = Guid.NewGuid().ToString();
            Timestamp = DateTime.Now;
            Username = name;
        }

        public string Username { get; set; }
    }
}
