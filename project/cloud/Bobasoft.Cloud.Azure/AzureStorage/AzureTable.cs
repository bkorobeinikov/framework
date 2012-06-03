using System.Collections.Generic;
using System.Data.Services.Client;
using System.Diagnostics;
using System.Linq;
using Bobasoft.Serialization;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace Bobasoft.Cloud.Azure.AzureStorage
{
    public class AzureTable<TEntity> : ICloudTable<TEntity> where TEntity : class, ITableStorageIdentifiable
    {
        //======================================================
        #region _Constructors_

        public AzureTable(string tableName, CloudStorageAccount account)
        {
            TableName = tableName;
            _account = account;
        }

        public AzureTable(string tableName, CloudStorageAccount account, IConverter converter)
        {
            TableName = tableName;
            _account = account;
            _converter = converter;
        }

        #endregion

        //======================================================
        #region _Public properties_

        public string TableName { get; protected set; }

        public virtual IQueryable<TEntity> Query
        {
            get
            {
                var context = GetContext();
                return context.CreateQuery<TEntity>(TableName).AsTableServiceQuery();
            }
        }

        public bool IgnoreEntityAleadyExists { get; set; }

        #endregion

        //======================================================
        #region _Public methods_

        public virtual void EnsureExist()
        {
            var tableClient = new CloudTableClient(_account.TableEndpoint.ToString(), _account.Credentials);
            tableClient.CreateTableIfNotExist(TableName);
        }

        public virtual void Add(TEntity entity)
        {
            Add(new[] {entity});
        }

        public virtual void Add(IEnumerable<TEntity> entities)
        {
            var context = GetContext();

            foreach (var entity in entities)
                context.AddObject(TableName, entity);

            try
            {
                context.SaveChangesWithRetries(SaveChangesOptions.Batch);
            }
            catch (DataServiceRequestException ex)
            {
                if (ex.InnerException is DataServiceClientException)
                {
                    var innerException = (DataServiceClientException)ex.InnerException;
                    if (innerException.StatusCode == 409 && IgnoreEntityAleadyExists)
                    {
                        Debug.WriteLine("The specified entity already exists.");
                        Debug.WriteLine("Message:\n" + innerException.Message);
                        return;
                    }
                }

                throw;
            }
        }

        public virtual void Update(TEntity entity)
        {
            Update(new[] {entity});
        }

        public virtual void Update(IEnumerable<TEntity> entities)
        {
            var context = GetContext();

            foreach (var entity in entities)
            {
                context.AttachTo(TableName, entity, "*");
                context.UpdateObject(entity);
            }

            context.SaveChangesWithRetries(SaveChangesOptions.Batch);
        }

        public virtual void Replace(TEntity entity)
        {
            Replace(new[] {entity});
        }

        public virtual void Replace(IEnumerable<TEntity> entities)
        {
            var context = GetContext();

            foreach (var entity in entities)
            {
                context.AttachTo(TableName, entity, "*");
                context.UpdateObject(entity);
            }

            context.SaveChangesWithRetries(SaveChangesOptions.ReplaceOnUpdate);
        }

        public virtual void Delete(TEntity entity)
        {
            Delete(new[] {entity});
        }

        public virtual void Delete(IEnumerable<TEntity> entities)
        {
            var context = GetContext();

            foreach (var entity in entities)
            {
                context.AttachTo(TableName, entity, "*");
                context.DeleteObject(entity);
            }

            context.SaveChangesWithRetries();
        }

        #endregion

        //======================================================
        #region _Private, protected, internal methods_

        protected virtual TableServiceContext GetContext()
        {
            return new AzureTableServiceContext(_account.TableEndpoint.ToString(), _account.Credentials, _converter)
                   {
                       IgnoreMissingProperties = true,
                       IgnoreResourceNotFoundException = true,
                   };
        }

        #endregion

        //======================================================
        #region _Private, protected, internal fields_

        protected CloudStorageAccount _account;
        private readonly IConverter _converter;

        #endregion
    }
}