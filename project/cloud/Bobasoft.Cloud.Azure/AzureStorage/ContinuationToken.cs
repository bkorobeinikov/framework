namespace Bobasoft.Cloud.Azure.AzureStorage
{
    internal class ContinuationToken
    {
        //======================================================
        #region _Public properties_

        public string PartitionKey { get; set; }
        public string RowKey { get; set; }

        #endregion
    }
}