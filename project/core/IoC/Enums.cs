namespace Bobasoft
{
    /// <summary>
    /// Represent lifetime of registering types.
    /// </summary>
    public enum Lifetime
    {
        /// <summary>
        /// Return each resolve new instance.
        /// </summary>
        Default,

        /// <summary>
        /// Singleton lifetime.
        /// </summary>
        Singleton,

        /// <summary>
        /// Per web request lifetime.
        /// </summary>
        PerWebRequest
    }
}