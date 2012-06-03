using System.Net;
using MongoDB.Driver;

namespace Bobasoft.Cloud.Azure.MongoDB
{
    public interface IReplicaSetNode
    {
        //======================================================
        #region _Public properties_

        bool IsPrimary { get; }
        IPEndPoint EndPoint { get; }

        ReplicaSet ReplicaSet { get; }

        #endregion 

        //======================================================
        #region _Public methods_

        /// <summary>
        /// Initialize replica set
        /// </summary>
        void Initialize(bool cleanup = false);

        void Configure(ReplicaSetConfiguration config, bool cleanup = false);

        void StepdownIfNeeded();

        void Ping();

        MongoServer GetLocalConnection(bool slaveOk = false);

        #endregion
    }
}