namespace Bobasoft.Infrastructure
{
    public enum JobOrder
    {
        None = 0,

        /// <summary>
        /// Register general dependencies, etc.
        /// </summary>
        Step1 = 1,

        /// <summary>
        /// Initialization, functionality dependencies, etc.
        /// </summary>
        Step2 = 2,

        /// <summary>
        /// Post initialization.
        /// </summary>
        Step3 = 3,

        /// <summary>
        /// Step, when all initalization are finished. Now can use dependencies, classes without fear of not initialization.
        /// </summary>
        Step4 = 4,
    }
}