using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace Bobasoft.Cloud.Azure.AzureStorage
{
    public class AzureBinaryBlobContainer : ICloudBlobContainer<byte[]>
    {
        //======================================================
        #region _Constructors_

        /// <summary>
        /// Creates instance of <see cref="AzureBinaryBlobContainer"/> with Off public access.
        /// </summary>
        /// <param name="containerName"></param>
        /// <param name="account"></param>
        public AzureBinaryBlobContainer(string containerName, CloudStorageAccount account)
            : this(containerName, account, new BlobContainerPermissions{PublicAccess = BlobContainerPublicAccessType.Off})
        {
        }

        public AzureBinaryBlobContainer(string containerName, CloudStorageAccount account, BlobContainerPermissions containerPermissions)
        {
            ContainerName = containerName;
            _account = account;

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

        #endregion

        //======================================================
        #region _Public methods_

        public void EnsureExist()
        {
            if (_container.CreateIfNotExist())
                _container.SetPermissions(_containerPermissions);
        }

        public Uri Store(BlobServiceEntity<byte[]> entity)
        {
            var b = _container.GetBlobReference(entity.Name);
            b.Properties.ContentType = entity.ContentType;
            b.Properties.CacheControl = entity.CacheControl;
            //if (entity.ContentEncoding != null)
                b.Properties.ContentEncoding = entity.ContentEncoding;
            //if (entity.ContentLanguage != null)
                b.Properties.ContentLanguage = entity.ContentLanguage;
            b.Metadata.Add(entity.Metadata);
            b.UploadByteArray(entity.Blob);
            entity.Uri = b.Uri;
            return b.Uri;
        }

        public Uri StoreAsync(BlobServiceEntity<byte[]> entity)
        {
            var b = _container.GetBlobReference(entity.Name);
            b.Properties.ContentType = entity.ContentType;
            b.Properties.CacheControl = entity.CacheControl;
            b.Properties.ContentEncoding = entity.ContentEncoding;
            b.Properties.ContentLanguage = entity.ContentLanguage;
            b.Metadata.Add(entity.Metadata);
            var stream = new MemoryStream(entity.Blob);
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

        public BlobServiceEntity<byte[]> Find(string name)
        {
            var blob = _container.GetBlobReference(name);
            try
            {
                var bytes = blob.DownloadByteArray();
                var result = new BlobServiceEntity<byte[]>
                             {
                                 Name = name,
                                 ContentType = blob.Properties.ContentType,
                                 Uri = blob.Uri,
                                 Blob = bytes,
                             };
                result.Metadata.Add(blob.Metadata);
                result.ContentType = blob.Properties.ContentType;
                result.CacheControl = blob.Properties.CacheControl;
                result.ContentEncoding = blob.Properties.ContentEncoding;
                result.ContentLanguage = blob.Properties.ContentLanguage;
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public IEnumerable<BlobServiceEntity<byte[]>> FindAll()
        {
            return _container.ListBlobs().Select(blob => Find(blob.Uri.ToString())).ToList();
        }

        public Uri GetUri(string name)
        {
            return _container.GetBlobReference(name).Uri;
        }

        public void Delete(string name)
        {
            var blob = _container.GetBlobReference(name);
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

        #endregion

        //======================================================
        #region _Private, protected, internal fields_

        private readonly CloudStorageAccount _account;
        private readonly CloudBlobContainer _container;
        private readonly BlobContainerPermissions _containerPermissions;

        #endregion
    }
}