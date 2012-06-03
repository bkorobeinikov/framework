using MongoDB.Bson;
using MongoDB.Driver;

namespace Bobasoft.Cloud.Azure.MongoDB
{
    public class AzureReplicaSet : ReplicaSet
    {
        //======================================================
        #region _Public methods_

        public override bool IsInitialized()
        {
            return IsReplicaSetInitialized();
        }

        public override CommandResult GetStatus()
        {
            var server = MongoDBHelper.GetConnection(CurrentEndPoint, true);
            var result = server.GetDatabase("admin").RunCommand("replSetGetStatus");
            return result;
        }

        #endregion

        //======================================================
        #region _Private, protected, internal methods_
        
        public bool IsReplicaSetInitialized()
        {
            try
            {
                //var server = MongoDBHelper.GetConnection(CurrentEndPoint, true);
                //return server.ReplicaSetName != null && server.Primary != null;
                var result = GetStatus();
                BsonValue startupStatus;
                result.Response.TryGetValue("startupStatus", out startupStatus);
                if (startupStatus != null)
                {
                    if (startupStatus == 3)
                    {
                        return false;
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion 
    }
}