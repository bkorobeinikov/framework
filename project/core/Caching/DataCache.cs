using System;

namespace Bobasoft.Caching
{
	public abstract class DataCache
	{
		//======================================================
		#region _Public methods_

		public abstract object Get(string key);
		public abstract T Get<T>(string key);

		public abstract void Put(string key, object obj);
		public abstract void Put(string key, object obj, TimeSpan timeout);

		public abstract void Remove(string key);

		public abstract void Clear();

		#endregion
	}
}