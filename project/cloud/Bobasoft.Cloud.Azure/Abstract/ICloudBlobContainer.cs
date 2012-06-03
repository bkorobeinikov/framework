using System;
using System.Collections.Generic;

namespace Bobasoft.Cloud
{
    public interface ICloudBlobContainer<TBlob>
    {
        //======================================================
        #region _Methods_

        /// <summary>
        /// Ensures the blob container exist, if not creates it.
        /// </summary>
        void EnsureExist();

        /// <summary>
        /// Saves the specified object.
        /// </summary>
        Uri Store(BlobServiceEntity<TBlob> entity);

        /// <summary>
        /// Saves the specified object asyncronously.
        /// </summary>
        Uri StoreAsync(BlobServiceEntity<TBlob> entity);

        /// <summary>
        /// Finds the specified object.
        /// </summary>
        /// <param name="name">The object id.</param>
        /// <returns>The blob entity.</returns>
        BlobServiceEntity<TBlob> Find(string name);

        /// <summary>
        /// Finds all objects.
        /// </summary>
        /// <returns>The objects.</returns>
        IEnumerable<BlobServiceEntity<TBlob>> FindAll();

        /// <summary>
        /// Gets the URI of the blob container.
        /// </summary>
        /// <param name="name">The object id.</param>
        /// <returns>The URI of the blob container.</returns>
        Uri GetUri(string name);

        /// <summary>
        /// Deletes the specified object.
        /// </summary>
        /// <param name="name">The object id.</param>
        void Delete(string name);

        /// <summary>
        /// Deletes the container.
        /// </summary>
        void DeleteContainer();

        #endregion
    }
}