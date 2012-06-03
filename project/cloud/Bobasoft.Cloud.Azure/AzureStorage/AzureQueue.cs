using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Bobasoft.Serialization;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;

namespace Bobasoft.Cloud.Azure.AzureStorage
{
    public class AzureQueue<TMessage> : ICloudQueue<TMessage> where TMessage : IQueueMessage
    {
        //======================================================
        #region _Constructors_

        public AzureQueue(string queueName, CloudStorageAccount account, IConverter converter)
            : this(queueName, account, TimeSpan.FromMinutes(5), converter)
        {
        }

        public AzureQueue(string queueName, CloudStorageAccount account, TimeSpan visibilityTimeout, IConverter converter)
        {
            QueueName = queueName;
            VisibilityTimeout = visibilityTimeout;
            _account = account;
            _converter = converter;

            var client = _account.CreateCloudQueueClient();
            if (client == null)
                throw new InvalidOperationException("Queue client is null.");

            _queue = client.GetQueueReference(QueueName.ToLowerInvariant());
            _errorContainer = new AzureTextBlobContainer<QueueError<TMessage>>(string.Format("queue-errors-{0}", QueueName.ToLowerInvariant()),
                                                                               account, converter, "srl");
        }

        #endregion

        //======================================================
        #region _Public properties_

        public string QueueName { get; private set; }
        public TimeSpan VisibilityTimeout { get; private set; }

        public event EventHandler<QueueMessageEventArgs<TMessage>> AddMessageCompleted;

        #endregion

        //======================================================
        #region _Public methods_

        public void EnsureExist()
        {
            if (RoleEnvironment.IsAvailable)
                _queue.CreateIfNotExist();

            _errorContainer.EnsureExist();
        }

        public void Clear()
        {
            _queue.Clear();
        }

        public void AddMessage(TMessage message)
        {
            var m = _converter.Serialize(message);
            _queue.AddMessage(new CloudQueueMessage(m));
        }

        public void AddMessageAsync(TMessage message)
        {
            var m = _converter.Serialize(message);
            _queue.BeginAddMessage(new CloudQueueMessage(m),
                                   ar =>
                                   {
                                       var q = (CloudQueue) ar.AsyncState;
                                       Exception error = null;
                                       var success = false;
                                       if (q == null)
                                           error = new NullReferenceException("AsyncState");

                                       try
                                       {
                                           q.EndAddMessage(ar);
                                           success = true;
                                       }
                                       catch (Exception ex)
                                       {
                                           success = false;
                                           error = ex;
                                       }

                                       if (AddMessageCompleted != null)
                                       {
                                           var args = new QueueMessageEventArgs<TMessage>()
                                                      {
                                                          Error = error,
                                                          Message = message,
                                                          Success = success,
                                                      };
                                           AddMessageCompleted(this, args);
                                       }
                                   },
                                   _queue);
        }

        public TMessage GetMessage()
        {
            var m = _queue.GetMessage(VisibilityTimeout);
            if (m == null)
                return default(TMessage);

            return GetDeserializedMessage(m);
        }

        public IEnumerable<TMessage> GetMessages(int maxMessagesToReturn)
        {
            var messages = _queue.GetMessages(maxMessagesToReturn, VisibilityTimeout);
            return messages.Select(GetDeserializedMessage);
        }

        public void DeleteMessage(TMessage message)
        {
            try
            {
                _queue.DeleteMessage(message.Id, message.PopReceipt);
            }
            catch (StorageClientException ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                    return;
                
                throw;
            }
        }

        public void DeleteMessageAsync(TMessage message)
        {
            _queue.BeginDeleteMessage(message.Id, message.PopReceipt,
                                      ar => _queue.EndDeleteMessage(ar), null);
        }

        public void ErrorOutMessage(TMessage message, Exception ex)
        {
            string instanceId = null;

            try
            {
                var currentRole = RoleEnvironment.CurrentRoleInstance;
                if (currentRole != null)
                    instanceId = currentRole.Id;
            }
            catch
            {
            }

            var error = new QueueError<TMessage>
                        {
                            Error = ex,
                            InstaceId = instanceId,
                            QueueMessage = message,
                            Timestamp = DateTime.UtcNow,
                            DequeueCount = message.DequeueCount,
                            Id = message.Id,
                            PopReceipt = message.PopReceipt
                        };

            var entity = new BlobServiceEntity<QueueError<TMessage>>
                         {
                             Name = string.Format("{0}_{1}", error.Timestamp.GetFormatedTicks(), message.Id),
                             Blob = error,
                             ContentType = ErrorContentType,
                         };
            _errorContainer.Store(entity);
        }

        public int RetreiveApproximateMessageCount()
        {
            return _queue.RetrieveApproximateMessageCount();
        }

        #endregion

        //======================================================
        #region _Private, protected, internal methods_

        private TMessage GetDeserializedMessage(CloudQueueMessage message)
        {
            var m = _converter.Deserialize<TMessage>(message.AsString);
            m.Id = message.Id;
            m.PopReceipt = message.PopReceipt;
            m.DequeueCount = message.DequeueCount;

            return m;
        }

        #endregion

        //======================================================
        #region _Private, protected, internal fields_

        private readonly CloudStorageAccount _account;

        private readonly CloudQueue _queue;
        private readonly ITextCloudBlobContainer<QueueError<TMessage>> _errorContainer;
        private readonly IConverter _converter;

        internal const string ErrorContentType = "application/slr";

        #endregion
    }
}