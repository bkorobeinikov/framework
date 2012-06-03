using System;
using System.Data.Services.Common;
using Bobasoft.Cloud.Azure.AzureStorage;
using MongoDB.Bson.Serialization.Attributes;

namespace Bobasoft.Cloud.Azure
{
    [DataServiceEntity]
    public class TableEntity : ITableStorageIdentifiable
    {
        [BsonIgnore]
        public virtual string PartitionKey { get; set; }
        [BsonIgnore]
        public virtual string RowKey { get; set; }
        [BsonIgnore]
        public DateTime Timestamp { get; set; }
    }
}