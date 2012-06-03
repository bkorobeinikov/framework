using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Services.Client;
using Bobasoft.Cloud.Azure;
using Bobasoft.Cloud.Azure.AzureStorage;

namespace System.Linq
{
    public static class LinqHelper
    {
        //======================================================
        #region _Public methods_

        public static T AzureFirstOrDefault<T>(this IQueryable<T> query, string partitionKey, string rowKey) where T : TableEntity
        {
            return query.Where(e => e.PartitionKey == partitionKey && e.RowKey == rowKey).FirstOrDefault();
        }

        public static IQueryable<T> AzureWhere<T>(this IQueryable<T> query, string partitionKey) where T : TableEntity
        {
            return query.Where(e => e.PartitionKey == partitionKey);
        }

        public static IQueryable<T> AzureWherePartitionPrefix<T>(this IQueryable<T> query, string partitionKeyPrefix) where T : TableEntity
        {
            return query.Where(e => e.PartitionKey.CompareTo(partitionKeyPrefix) >= 0 &&
                                        e.PartitionKey.CompareTo(TableKeyHelper.NextComparisonString(partitionKeyPrefix)) < 0);
        }

        public static IQueryable<T> AzureWherePartitionPrefix<T>(this IQueryable<T> query, string partitionKeyPrefix, string rowKey) where T : TableEntity
        {
            return query.Where(e => (e.PartitionKey.CompareTo(partitionKeyPrefix) >= 0 &&
                                     e.PartitionKey.CompareTo(TableKeyHelper.NextComparisonString(partitionKeyPrefix)) < 0) &&
                                    e.RowKey == rowKey);
        }

        public static IQueryable<T> AzureWhereRowPrefix<T>(this IQueryable<T> query, string partitionKey, string rowKeyPrefix) where T : TableEntity
        {
            return query.Where(e => e.PartitionKey == partitionKey &&
                                    (e.RowKey.CompareTo(rowKeyPrefix) >= 0 &&
                                     e.RowKey.CompareTo(TableKeyHelper.NextComparisonString(rowKeyPrefix)) < 0));
        }

        public static IQueryable<T> AzureWherePrefix<T>(this IQueryable<T> query, string partitionKeyPrefix, string rowKeyPrefix) where T : TableEntity
        {
            return query.Where(e => (e.PartitionKey.CompareTo(partitionKeyPrefix) >= 0 &&
                                     e.PartitionKey.CompareTo(TableKeyHelper.NextComparisonString(partitionKeyPrefix)) < 0) &&
                                    (e.RowKey.CompareTo(rowKeyPrefix) >= 0 && e.RowKey.CompareTo(TableKeyHelper.NextComparisonString(rowKeyPrefix)) < 0));
        }

        /// <summary>
        /// Only use, if needed load more than 1000 entities at once.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public static IEnumerable<T> AzureAsEnumerable<T>(this IQueryable<T> query)
        {
            const int take = AzureConstants.TableQueryMaxEntitiesCount;
            ContinuationToken token = null;

            var entities = new Collection<T>();
            do
            {
                var q = (DataServiceQuery<T>)query.Take(take);

                if (token != null)
                {
                    q = q.AddQueryOption("NextPartitionKey", token.PartitionKey);
                    if (token.RowKey != null)
                        q = q.AddQueryOption("NextRowKey", token.RowKey);
                }

                var response = (QueryOperationResponse)q.Execute();
                if (response != null)
                {
                    foreach (T entity in response)
                        entities.Add(entity);
                }

                if (response.Headers.ContainsKey(AzureConstants.ContinuationTokenPartitionKey))
                {
                    token = new ContinuationToken();
                    token.PartitionKey = response.Headers[AzureConstants.ContinuationTokenPartitionKey];
                    if (response.Headers.ContainsKey(AzureConstants.ContinuationTokenRowKey))
                        token.RowKey = response.Headers[AzureConstants.ContinuationTokenRowKey];
                }
                else
                    token = null;

            } while (token != null);

            return entities;
        }

        #endregion
    }
}