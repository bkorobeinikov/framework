using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Bobasoft.Serialization;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace Bobasoft.Cloud.Azure.AzureStorage
{
    public class AzureTextBlobContainer<TBlob> : ITextCloudBlobContainer<TBlob>
    {
        //======================================================
        #region _Constructors_

        /// <summary>
        /// Creates instance of <see cref="AzureTextBlobContainer{T}"/> with Off public access.
        /// </summary>
        /// <param name="containerName"></param>
        /// <param name="account"></param>
        /// <param name="converter"></param>
        /// <param name="textFormat"></param>
        public AzureTextBlobContainer(string containerName, CloudStorageAccount account, IConverter converter, string textFormat)
            : this(containerName, account, new BlobContainerPermissions{PublicAccess = BlobContainerPublicAccessType.Off}, converter, textFormat)
        {
        }

        public AzureTextBlobContainer(string containerName, CloudStorageAccount account, BlobContainerPermissions containerPermissions, IConverter converter, string textFormat)
        {
            ContainerName = containerName;
            _account = account;
            _converter = converter;
            _textFormat = textFormat;
            ContentType = string.Format("application/{0}", _textFormat);

            var client = account.CreateCloudBlobClient();
            if (client == null)
                throw new InvalidOperationException("Blob client is null.");

            client.RetryPolicy = RetryPolicies.Retry(3, TimeSpan.FromSeconds(5));

            _containerPermissions = containerPermissions;
            _container = client.GetContainerReference(containerName.ToLowerInvariant());
        }

        #endregion

        //======================================================
        #region _Public properties_

        public string ContainerName { get; private set; }
        public string ContentType { get; private set; }
        
        #endregion

        //======================================================
        #region _Public methods_

        public void EnsureExist()
        {
            if (_container.CreateIfNotExist())
                _container.SetPermissions(_containerPermissions);
        }

        public Uri Store(BlobServiceEntity<TBlob> entity)
        {
            var b = _container.GetBlobReference(string.Concat(entity.Name, ".", _textFormat));
            b.Properties.ContentType = ContentType;
            b.UploadText(_converter.Serialize(entity.Blob));
            entity.Uri = b.Uri;
            return b.Uri;
        }

        public Uri StoreAsync(BlobServiceEntity<TBlob> entity)
        {
            var b = _container.GetBlobReference(string.Format("{0}.{1}", entity.Name, _textFormat));
            b.Properties.ContentType = ContentType;
            var text = _converter.Serialize(entity.Blob);
            var bytes = Encoding.UTF8.GetBytes(text);
            var stream = new MemoryStream(bytes);
            b.BeginUploadFromStream(stream,
                                    ar =>
                                    {
                                        var resultBlob = (CloudBlob) ar.AsyncState;
                                        resultBlob.EndUploadFromStream(ar);
                                    },
                                    b);
            entity.Uri = b.Uri;
            return b.Uri;
        }

        public BlobServiceEntity<TBlob> Find(string name)
        {
            var blob = _container.GetBlobReference(string.Format("{0}.{1}", name, _textFormat));
            try
            {
                var bytes = _converter.Deserialize<TBlob>(blob.DownloadText());
                var result = new BlobServiceEntity<TBlob>
                {
                    Name = name,
                    ContentType = blob.Properties.ContentType,
                    Uri = blob.Uri,
                    Blob = bytes,
                };
                result.Metadata.Add(blob.Metadata);
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IEnumerable<BlobServiceEntity<TBlob>> FindAll()
        {
            var blobs = _container.ListBlobs();
            return blobs.Select(blob => Find(blob.Uri.ToString())).ToList();
        }

        public Uri GetUri(string name)
        {
            return new Uri(string.Format("{0}.{1}", name, _textFormat));
        }

        public void Delete(string name)
        {
            var blob = _container.GetBlobReference(string.Format("{0}.{1}", name, _textFormat));
            blob.DeleteIfExists();
        }

        public void DeleteContainer()
        {
            try
            {
                _container.Delete();
            }
            catch (StorageClientException ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                    return;

                throw;
            }
        }

        public string FindAsString(string blobId)
        {
            return _container.GetBlobReference(string.Format("{0}.{1}", blobId, _textFormat)).DownloadText();
        }

        #endregion

        //======================================================
        #region _Private, protected, internal fields_

        private readonly CloudStorageAccount _account;
        private readonly CloudBlobContainer _container;
        private readonly BlobContainerPermissions _containerPermissions;
        private readonly IConverter _converter;
        private readonly string _textFormat;

        #endregion
    }
}