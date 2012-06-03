using System;
using System.Collections.Generic;

namespace Bobasoft.Cloud
{
    public interface ICloudQueue<TMessage> where TMessage : IQueueMessage
    {
        //======================================================
        #region _Properties_

        event EventHandler<QueueMessageEventArgs<TMessage>> AddMessageCompleted;

        #endregion

        //======================================================
        #region _Methods_

        /// <summary>
        /// Ensures the queue exist, if not creates it.
        /// </summary>
        void EnsureExist();

        /// <summary>
        /// Clears the queue.
        /// </summary>
        void Clear();

        /// <summary>
        /// Adds the message to the queue.
        /// </summary>
        /// <param name="message">The message.</param>
        void AddMessage(TMessage message);

        /// <summary>
        /// Adds the message async.
        /// </summary>
        /// <param name="message">The message.</param>
        void AddMessageAsync(TMessage message);

        /// <summary>
        /// Gets the top message from the queue.
        /// </summary>
        /// <returns>The message object.</returns>
        TMessage GetMessage();

        /// <summary>
        /// Gets the top messages from the queue.
        /// </summary>
        /// <param name="maxMessagesToReturn">The max messages to return.</param>
        /// <returns>The message objects.</returns>
        IEnumerable<TMessage> GetMessages(int maxMessagesToReturn);

        /// <summary>
        /// Deletes the message from the queue.
        /// </summary>
        /// <param name="message">The message.</param>
        void DeleteMessage(TMessage message);

        void DeleteMessageAsync(TMessage message);

        void ErrorOutMessage(TMessage message, Exception ex);

        int RetreiveApproximateMessageCount();

        #endregion
    }
}