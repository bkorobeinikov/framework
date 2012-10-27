namespace Bobasoft.Caching
{
	public interface IDataCacheFactory
	{
		//======================================================
		#region _Methods_

		DataCache GetOrCreateCache(string cacheName);

		#endregion
	}
}