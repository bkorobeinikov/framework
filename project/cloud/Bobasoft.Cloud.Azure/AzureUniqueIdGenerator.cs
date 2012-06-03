using SnowMaker;

namespace Bobasoft.Cloud.Azure
{
    public class AzureUniqueIdGenerator : IUniqueIdGenerator
    {
        //======================================================
        #region _Constructors_

        public AzureUniqueIdGenerator(IStorageConfiguration configuration, string containerName, int batchSize)
        {
            _configuration = configuration;
            _containerName = containerName;
            _batchSize = batchSize;
        }

        #endregion

        //======================================================
        #region _Private, protected, internal properties_

        private SnowMaker.IUniqueIdGenerator _idGenerator;
        private SnowMaker.IUniqueIdGenerator IdGenerator
        {
            get
            {
                 return _idGenerator ?? (_idGenerator = new UniqueIdGenerator(new BlobOptimisticDataStore(_configuration.Account, _containerName))
                                                       {
                                                           BatchSize = _batchSize,
                                                       });
            }
        }

        #endregion

        //======================================================
        #region _Public methods_

        public string NextId(string scope)
        {
            return IdGenerator.NextId(scope).ToString();
        }

        public string NextId(string scope, string format)
        {
            return IdGenerator.NextId(scope).ToString(format);
        }

        #endregion

        //======================================================
        #region _Private, protected, internal fields_

        private readonly IStorageConfiguration _configuration;
        private readonly string _containerName;
        private readonly int _batchSize;

        #endregion
    }
}