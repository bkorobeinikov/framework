using System;
using System.Web;
using Bobasoft.Caching;

namespace Bobasoft.Web.Caching
{
	public class RequestDataCache : DataCache
	{
		protected const string CacheKeyFormat = "cache:{0}";

		public override object Get(string key)
		{
			var k = CacheKeyFormat.With(key);
			return HttpContext.Current.Items[k];
		}

		public override T Get<T>(string key)
		{
			var k = CacheKeyFormat.With(key);
			var obj = HttpContext.Current.Items[k];

			return obj != null ? (T) obj : default(T);
		}

		public override void Put(string key, object obj)
		{
			var k = CacheKeyFormat.With(key);
			HttpContext.Current.Items[k] = obj;
		}

		public override void Put(string key, object obj, TimeSpan timeout)
		{
			throw new NotSupportedException("Timeout currently not supported for request cache");
		}

		public override void Remove(string key)
		{
			var k = CacheKeyFormat.With(key);
			HttpContext.Current.Items.Remove(k);
		}

		public override void Clear()
		{
			HttpContext.Current.Items.Clear();
		}
	}
}