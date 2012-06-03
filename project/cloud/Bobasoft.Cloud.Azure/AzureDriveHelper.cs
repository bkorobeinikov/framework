using System;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;

namespace Bobasoft.Cloud.Azure
{
    public static class AzureDriveHelper
    {
        //======================================================
        #region _Public methods_

        public static string GetMountedPathFromBlob(IStorageConfiguration config,
            string localCachePath,
            string cloudDir,
            string containerName,
            string blobName,
            int driveSize,
            out CloudDrive drive)
        {

            Diagnostics.TraceInformation(string.Format("In mounting cloud drive for dir {0} on {1} with {2}",
                                                       cloudDir,
                                                       containerName,
                                                       blobName));

            var blobClient = config.Account.CreateCloudBlobClient();

            Diagnostics.TraceInformation("Get container");
            // this should be the name of your replset
            var driveContainer = blobClient.GetContainerReference(containerName);

            // create blob container (it has to exist before creating the cloud drive)
            try
            {
                driveContainer.CreateIfNotExist();
            }
            catch (Exception e)
            {
                Diagnostics.Trace("Exception when creating container", e);
            }

            var blobUri = blobClient.GetContainerReference(containerName).GetPageBlobReference(blobName).Uri.ToString();
            Diagnostics.TraceInformation(string.Format("Blob uri obtained {0}", blobUri));

            // create the cloud drive
            drive = config.Account.CreateCloudDrive(blobUri);
            try { drive.Create(driveSize); } catch {}
            Diagnostics.TraceInformation("Initialize cache");
            var localStorage = RoleEnvironment.GetLocalResource(localCachePath);

            CloudDrive.InitializeCache(localStorage.RootPath.TrimEnd('\\'),
                localStorage.MaximumSizeInMegabytes);

            // mount the drive and get the root path of the drive it's mounted as
            Diagnostics.TraceInformation(string.Format("Trying to mount blob as azure drive on {0}",
                                                       RoleEnvironment.CurrentRoleInstance.Id));
            var driveLetter = drive.Mount(localStorage.MaximumSizeInMegabytes,
                                          DriveMountOptions.None);
            Diagnostics.TraceInformation(string.Format("Write lock acquired on azure drive, mounted as {0}, on role instance {1}",
                                                       driveLetter, RoleEnvironment.CurrentRoleInstance.Id));
            return driveLetter;
        }

        #endregion
    }
}