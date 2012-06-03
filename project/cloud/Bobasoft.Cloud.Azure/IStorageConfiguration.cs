using Microsoft.WindowsAzure;

namespace Bobasoft.Cloud.Azure
{
    public interface IStorageConfiguration
    {
        //======================================================
        #region _Properties_

        CloudStorageAccount Account { get; }

        #endregion
    }
}