namespace Bobasoft.Cloud
{
    public interface ITextCloudBlobContainer<TBlob> : ICloudBlobContainer<TBlob>
    {
        //======================================================
        #region _Methods_

        /// <summary>
        /// Finds specified object.
        /// </summary>
        /// <param name="blobId">The object id.</param>
        /// <returns>The object in serialized format.</returns>
        string FindAsString(string blobId);

        #endregion
    }
}