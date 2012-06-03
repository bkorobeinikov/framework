namespace Bobasoft.Infrastructure
{
    public abstract class Job
    {
        //======================================================
        #region _Public methods_

        /// <summary>
        /// Register all dependencies. Prepare data for action.
        /// </summary>
        public virtual void PreAction()
        {
        }

        /// <summary>
        /// Initialize all. Prepare data for post action.
        /// </summary>
        public abstract void Action();

        public virtual void PostAction()
        {
        }

        #endregion
    }
}