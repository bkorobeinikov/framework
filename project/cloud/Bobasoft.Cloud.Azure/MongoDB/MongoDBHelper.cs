using System.Net;
using MongoDB.Driver;

namespace Bobasoft.Cloud.Azure.MongoDB
{
    public static class MongoDBHelper
    {
        public static MongoServer GetConnection(IPEndPoint endPoint, bool slaveOk = false)
        {
            const string connectionString = "mongodb://{0}/";
            const string connectionStringSlaveOk = "mongodb://{0}/?slaveOk=true";
            return MongoServer.Create(slaveOk ? string.Format(connectionStringSlaveOk, endPoint) : string.Format(connectionString, endPoint));
        } 
    }
}