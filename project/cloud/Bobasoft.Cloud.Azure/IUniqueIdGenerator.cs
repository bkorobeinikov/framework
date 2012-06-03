namespace Bobasoft.Cloud.Azure
{
    public interface IUniqueIdGenerator
    {
        //======================================================
        #region _Public methods_

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scope">The scope name.</param>
        /// <returns></returns>
        string NextId(string scope);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scope">The scope name.</param>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        string NextId(string scope, string format);

        #endregion
    }
}