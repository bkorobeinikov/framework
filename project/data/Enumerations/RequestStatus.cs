namespace Bobasoft.Data
{
    /// <summary>
    /// Request statuses
    /// </summary>
    public enum RequestStatus
    {
        /// <summary>
        /// Default request status.
        /// </summary>
        None = 0,

        /// <summary>
        /// Request is in pending status.
        /// </summary>
        Pending = 1,

        /// <summary>
        /// Request accepted.
        /// </summary>
        Accepted = 2,

        /// <summary>
        /// Request rejected. 
        /// </summary>
        Rejected = 3,

        /// <summary>
        /// Request canceled
        /// </summary>
        Canceled = 4,
    }
}