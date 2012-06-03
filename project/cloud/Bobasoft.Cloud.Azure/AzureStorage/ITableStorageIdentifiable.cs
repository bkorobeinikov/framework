using System;

namespace Bobasoft.Cloud.Azure.AzureStorage
{
    public interface ITableStorageIdentifiable
    {
        //======================================================
        #region _Properties_

        string PartitionKey { get; set; }
        string RowKey { get; set; }
        DateTime Timestamp { get; set; }

        #endregion 
    }
}