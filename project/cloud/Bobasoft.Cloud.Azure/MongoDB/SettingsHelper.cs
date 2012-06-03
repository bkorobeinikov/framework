using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.WindowsAzure.ServiceRuntime;
using MongoDB.Driver;

namespace Bobasoft.Cloud.Azure.MongoDB
{
    public static class SettingsHelper
    {
        //======================================================
        #region _Public methods_

        public static MongoServerSettings GetReplicaSetSettings(string roleName)
        {
            var settings = new MongoServerSettings();
            var replicaSetName = RoleEnvironment.GetConfigurationSettingValue(ReplicaSetNameSetting);
            settings.ReplicaSetName = replicaSetName;

            ReadOnlyCollection<RoleInstance> workerRoleInstances;
            try
            {
                workerRoleInstances = RoleEnvironment.Roles[roleName].Instances;
            }
            catch (KeyNotFoundException ke)
            {
                throw new Exception(
                    string.Format("The MongoDB role should be called {0}", roleName),
                    ke);
            }

            var servers = new List<MongoServerAddress>();
            foreach (var instance in workerRoleInstances)
            {
                var endpoint = AzureRoleHelper.GetEndPoint(instance, MongodPortKey);
                var server = new MongoServerAddress(endpoint.Address.ToString(), endpoint.Port);
                servers.Add(server);
            }

            settings.Servers = servers;
            settings.ConnectionMode = ConnectionMode.ReplicaSet;
            return settings;
        }

        #endregion

        //======================================================
        #region _Public fields_

        public const string MongodPortKey = "Bobasoft.Storage.MongoDb.MongodPort";
        public const string ReplicaSetNameSetting = "Bobasoft.Storage.MongoDb.ReplicaSetName";

        #endregion
    }
}