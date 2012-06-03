using System;

namespace Bobasoft.Cloud
{
    public class QueueMessageEventArgs<TMessage> : EventArgs where TMessage : IQueueMessage
    {
        //======================================================
        #region _Public properties_

        public TMessage Message { get; set; }
        public Exception Error { get; set; }
        public bool Success { get; set; }

        #endregion
    }
}