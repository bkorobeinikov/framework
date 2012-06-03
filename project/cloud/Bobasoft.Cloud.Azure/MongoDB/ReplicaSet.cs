using System.Net;
using MongoDB.Driver;

namespace Bobasoft.Cloud.Azure.MongoDB
{
    public abstract class ReplicaSet
    {
        //======================================================
        #region _Public properties_

        /// <summary>
        /// Replica set name.
        /// </summary>
        public string Name { get; set; }

        public IPEndPoint PrimaryEndPoint { get; set; }

        public IPEndPoint CurrentEndPoint { get; set; }

        #endregion

        //======================================================
        #region _Public methods_

        public abstract CommandResult GetStatus();

        public abstract bool IsInitialized();

        #endregion
    }
}