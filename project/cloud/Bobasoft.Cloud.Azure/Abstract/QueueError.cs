using System;
using System.Runtime.Serialization;

namespace Bobasoft.Cloud
{
    [DataContract(Name = "queueError")]
    public class QueueError<TMessage>
    {
        //======================================================
        #region _Public properties_

        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "popReceipt")]
        public string PopReceipt { get; set; }

        [DataMember(Name = "dequeueMessage")]
        public int DequeueCount { get; set; }

        [DataMember(Name = "queueMessage")]
        public TMessage QueueMessage { get; set; }

        [DataMember(Name = "error")]
        public Exception Error { get; set; }

        [DataMember(Name = "instaceId")]
        public string InstaceId { get; set; }

        [DataMember(Name = "timestamp")]
        public DateTime Timestamp { get; set; }

        #endregion
    }
}