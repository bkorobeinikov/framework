using System.Collections.Generic;
using System.Net;

namespace Bobasoft.Cloud.Azure.MongoDB
{
    public class ReplicaSetConfiguration
    {
        public class ReplicaSetNodeConfiguration
        {
            public string Id { get; set; }
            public IPEndPoint EndPoint { get; set; }
            public bool Current { get; set; }
        }

        //======================================================
        #region _Public properties_

        /// <summary>
        /// Replica set name.
        /// </summary>
        public string Name { get; set; }

        public List<ReplicaSetNodeConfiguration> Nodes { get; set; }

        #endregion 
    }
}