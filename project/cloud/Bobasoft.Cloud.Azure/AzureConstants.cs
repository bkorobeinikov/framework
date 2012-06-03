using System;

namespace Bobasoft.Cloud.Azure
{
    public static class AzureConstants
    {
        //======================================================
        #region _Public fields_

        public const int TableQueryMaxEntitiesCount = 1000;

        public const string ContinuationTokenPartitionKey = "x-ms-continuation-NextPartitionKey";
        public const string ContinuationTokenRowKey = "x-ms-continuation-NextRowKey";

        public static readonly DateTime TableMinDateTime = new DateTime(1601, 1, 2);

        #endregion
    }
}