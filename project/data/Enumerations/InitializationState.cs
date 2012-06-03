namespace Bobasoft.Data
{
    /// <summary>
    /// Initialization states.
    /// </summary>
    public enum InitializationState
    {
        /// <summary>
        /// Not initialized.
        /// </summary>
        NotInitialized,

        /// <summary>
        /// Initialization in progress.
        /// </summary>
        Initializing,

        /// <summary>
        /// Initialized.
        /// </summary>
        Initialized,

        /// <summary>
        /// Initialization failed.
        /// </summary>
        Failed,


        /// <summary>
        /// Initialization canceled.
        /// </summary>
        Canceled,
    }
}