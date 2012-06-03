using System.Collections.Generic;
using System.Net;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace Bobasoft.Cloud.Azure.MongoDB
{
    public class AzureReplicaSetConfiguration : ReplicaSetConfiguration
    {
        //======================================================
        #region _Constructors_

        public AzureReplicaSetConfiguration(RoleInstance roleInstance, string replicaSetName, IPEndPoint endPoint)
        {
            Name = replicaSetName;
            Nodes = new List<ReplicaSetNodeConfiguration>();
            
            var roleName = roleInstance.Role.Name;

            foreach (var instance in RoleEnvironment.Roles[roleName].Instances)
            {
                var ip = AzureRoleHelper.GetEndPoint(instance, SettingsHelper.MongodPortKey);
                var id = AzureRoleHelper.ParseNodeInstanceId(instance.Id);
                Nodes.Add(new ReplicaSetNodeConfiguration
                              {
                                  Id = id.ToString(),
                                  EndPoint = ip,
                                  Current = ip.Equals(endPoint),
                              });
            }
        }

        #endregion
    }
}