using System;
using System.Linq;
using System.Net;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Bobasoft.Cloud.Azure.MongoDB
{
    public class AzureReplicaSetNode : IReplicaSetNode
    {
        //======================================================
        #region _Constructors_

        public AzureReplicaSetNode(ReplicaSetConfiguration config)
        {
            _config = config;
            EndPoint = _config.Nodes.First(n => n.Current).EndPoint;
            ReplicaSet = new AzureReplicaSet
            {
                Name = _config.Name,
                CurrentEndPoint = EndPoint,
            };
        }

        #endregion

        //======================================================
        #region _Public properties_

        public ReplicaSet ReplicaSet { get; protected set; }

        public bool IsPrimary
        {
            get { return ReplicaSet.PrimaryEndPoint == EndPoint; }
        }

        public IPEndPoint EndPoint { get; protected set; }

        #endregion

        //======================================================
        #region _Public methods_

        public void Configure(ReplicaSetConfiguration config, bool cleanup = false)
        {
            if (!IsPrimary)
                throw new InvalidOperationException("Reconfigure only on primary node");

            _config = config;

            Configure(true, cleanup);
        }

        public void Initialize(bool cleanup = false)
        {
            if (ReplicaSet.IsInitialized())
                throw new InvalidOperationException("replica set already initialized");

            Configure(cleanup);
        }

        public void StepdownIfNeeded()
        {
            var server = GetLocalConnection(true);
            if (server.State == MongoServerState.Disconnected)
            {
                server.Connect();
            }

            if (server.Instance.IsPrimary)
            {
                var stepDownCommand = new CommandDocument
                                      {
                                          {"replSetStepDown", 1}
                                      };

                server.GetDatabase("admin").RunCommand(stepDownCommand);
            }
        }

        public MongoServer GetLocalConnection(bool slaveOk = false)
        {
            return MongoDBHelper.GetConnection(EndPoint, slaveOk);
        }

        public void Ping()
        {
            var nodeDocument = new BsonDocument();
            foreach (var node in _config.Nodes)
            {
                nodeDocument.Add(
                    string.Format(NodeName, node.Id),
                    string.Format(node.EndPoint.ToString()));
            }

            var commandDocument = new BsonDocument
                                  {
                                      {"cloud", 1},
                                      {"nodes", nodeDocument},
                                      {"me", string.Format(NodeName, _config.Nodes.First(n => n.Current).Id)}
                                  };

            var cloudCommand = new CommandDocument(commandDocument);

            var server = GetLocalConnection(true);
            server.GetDatabase("admin").RunCommand(cloudCommand);
        }

        #endregion

        //======================================================
        #region _Private, protected, internal methods_

        protected void Configure(bool reconfig = false, bool cleanup = false)
        {
            var membersDocument = new BsonArray();

            foreach (var node in _config.Nodes)
            {
                membersDocument.Add(new BsonDocument
                                        {
                                            {"_id", int.Parse(node.Id)},
                                            {"host", string.Format(NodeName, node.Id)}
                                        });
            }

            var cfg = new BsonDocument
                          {
                              {"_id", _config.Name},
                              {"members", membersDocument}
                          };
            var initCommand = new CommandDocument
                                  {
                                      {reconfig ? "replSetReconfig" : "replSetInitiate", cfg}
                                  };

            var server = GetLocalConnection(true);

            if (cleanup)
            {
                // clear oplog collections
                var localdb = server.GetDatabase("local");
                localdb.GetCollection(string.Format("oplog.{0}", _config.Name)).Drop();
                localdb.GetCollection("system.replset").FindAndRemove(new QueryDocument("_id", _config.Name), null);
            }

            server.GetDatabase("admin").RunCommand(initCommand);
        }

        #endregion

        //======================================================
        #region _Private, protected, internal fields_

        private ReplicaSetConfiguration _config;

        private const string NodeName = "#d{0}";
        //var nodeAddress = "{0}:{1}";

        #endregion
    }
}