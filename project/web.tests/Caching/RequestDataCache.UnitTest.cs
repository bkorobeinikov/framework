using System.Web;
using Bobasoft.Testing.Web;
using Bobasoft.Web.Caching;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bobasoft.Web.Net40.Tests.Caching
{
	[TestClass]
	public class RequestDataCacheUnitTest : HttpUnitTest
	{
		protected RequestDataCache _cache;

		[TestInitialize]
		public void SetUp()
		{
			_cache = new RequestDataCache();
		}

		[TestMethod]
		public void GetShouldGetObjectFromHttpContextItemsAndKeyShouldContainsCachePrefix()
		{
			var key = "cache:key1";
			var assertObj = new object();
			HttpContext.Current.Items[key] = assertObj;

			var obj = _cache.Get("key1");

			Assert.AreEqual(assertObj, obj);
		}

		[TestMethod]
		public void GenericGetShouldGetObjectFromHttpContextItemsAndKeyShouldContainsCachePrefix()
		{
			var key = "cache:key1";
			var assertObj = new object();
			HttpContext.Current.Items[key] = assertObj;

			var obj = _cache.Get<object>("key1");

			Assert.AreEqual(assertObj, obj);
		}

		[TestMethod]
		public void GetShouldNotFailIfThereAreNoObjectInCache()
		{
			Assert.IsNull(_cache.Get("key1"));
		}

		[TestMethod]
		public void GenericGetShouldNotFailIfThereAreNoObjectInCache()
		{
			Assert.AreEqual(default(int), _cache.Get<int>("key1"));
		}

		[TestMethod]
		public void GenericGetShouldNotFailIfThereAreNoObjectInCache1()
		{
			Assert.IsNull(_cache.Get<object>("key1"));
		}

		[TestMethod]
		public void PutShouldPutObjectToHttpContextItemsAndKeySHoulContainsCachePrefix()
		{
			var assert = new object();

			_cache.Put("key1", assert);

			Assert.AreEqual(assert, HttpContext.Current.Items["cache:key1"]);
		}

		[TestMethod]
		public void RemoveShouldRemoveObjectFromHttpContextItems()
		{
			var assertObj = new object();
			HttpContext.Current.Items["cache:key1"] = assertObj;

			_cache.Remove("key1");

			Assert.IsNull(HttpContext.Current.Items["cache:key1"]);
		}

		[TestMethod]
		public void ClearShouldClearAllHttpContextItems()
		{
			var assertObj = new object();
			HttpContext.Current.Items["cache:key1"] = assertObj;

			_cache.Clear();

			Assert.AreEqual(0, HttpContext.Current.Items.Count);
		}
	}
}