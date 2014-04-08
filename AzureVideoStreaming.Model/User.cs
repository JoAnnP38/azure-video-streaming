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
        public User(string name, string rowKey)
        {
            PartitionKey = TableStorageConstants.UserPartitionKey;
            RowKey = rowKey;
            Timestamp = DateTime.Now;
            Username = name;
        }

        public string Username { get; set; }
    }
}
