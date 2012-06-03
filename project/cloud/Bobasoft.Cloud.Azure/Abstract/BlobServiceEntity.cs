using System;
using System.Collections.Specialized;

namespace Bobasoft.Cloud
{
    public class BlobServiceEntity<T>
    {
        //======================================================
        #region _Constructors_

        public BlobServiceEntity()
        {
            Metadata = new NameValueCollection();
        }

        #endregion

        //======================================================
        #region _Public properties_

        /// <summary>
        /// The name of the blob.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The blob content itself.
        /// </summary>
        public T Blob { get; set; }

        /// <summary>
        /// The blob content type.
        /// </summary>
        public string ContentType { get; set; }

        public string CacheControl { get; set; }
        public string ContentEncoding { get; set; }
        public string ContentLanguage { get; set; }

        /// <summary>
        /// The blob metadata.
        /// </summary>
        public NameValueCollection Metadata { get; private set; }

        /// <summary>
        /// Gets or sets uri of blob.
        /// </summary>
        public Uri Uri { get; set; }

        #endregion
    }
}