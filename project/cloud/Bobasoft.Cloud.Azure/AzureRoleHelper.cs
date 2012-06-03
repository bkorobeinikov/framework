using System.Net;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace Bobasoft.Cloud.Azure
{
    public class AzureRoleHelper
    {
        //======================================================
        #region _Public methods_

        public static string GetConfigValue(string key)
        {
            return RoleEnvironment.GetConfigurationSettingValue(key);
        }

        public static int ParseNodeInstanceId(string id)
        {
            return int.Parse(id.Substring(id.LastIndexOf("_") + 1));
        }

        public static IPEndPoint GetEndPoint(RoleInstance roleInstance, string endPointName)
        {
            var endPoint = roleInstance.InstanceEndpoints[endPointName].IPEndpoint;
            return RoleEnvironment.IsEmulated
                       ? new IPEndPoint(Dns.GetHostAddresses("localhost")[0],
                                        endPoint.Port + ParseNodeInstanceId(roleInstance.Id))
                       : endPoint;
        }

        #endregion 
    }
}