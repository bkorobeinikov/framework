namespace Bobasoft.Data
{
    /// <summary>
    /// Transaction statuses
    /// </summary>
    public enum TransactionStatus
    {
        /// <summary>
        /// Transaction not strated.
        /// </summary>
        None = 0,

        /// <summary>
        /// Transaction in process.
        /// </summary>
        Processing = 1,

        /// <summary>
        /// Transaction is done.
        /// </summary>
        Done = 2,

        /// <summary>
        /// Transaction is failed.
        /// </summary>
        Failed = 3,

        /// <summary>
        /// Transaction canceled.
        /// </summary>
        Canceled = 4,
    }
}