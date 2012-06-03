namespace Bobasoft.Cloud
{
    public interface IQueueMessage
    {
        //======================================================
        #region _Properties_

        string Id { get; set; }
        string PopReceipt { get; set; }
        int DequeueCount { get; set; }

        #endregion
    }
}