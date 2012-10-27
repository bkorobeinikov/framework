using System.IO;
using System.Net.Http;
using System.Security.Principal;
using System.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Bobasoft.Testing.Web
{
	[TestClass]
	public class HttpUnitTest
	{
		protected HttpContextBase _context;
		protected Mock<IIdentity> _identity;

		[TestInitialize]
		public void SetupHttp()
		{
			IoC.Initialize(new DependencyResolver());
			InitializeHttpContext();
		}

		protected virtual void InitializeHttpContext()
		{
			_context = MockHttpContextHelper.FakeHttpContext();
			Mock.Get(_context).SetupAllProperties();

			// initialize context user
			_identity = new Mock<IIdentity>();
			_identity.Setup(i => i.IsAuthenticated).Returns(false);
			var user = new GenericPrincipal(_identity.Object, new string[0]);
			_context.User = user;

			HttpContext.Current = new HttpContext(
				new HttpRequest("", "http://tempuri.org", ""),
				new HttpResponse(new StringWriter())) {User = user};
		}

		[TestCleanup]
		public void CleanupHttp()
		{
		}

		protected virtual void SetRequestUrl(string url)
		{
			_context.Request.SetupRequestUrl(url);
		}

		protected virtual void SetExecutionPath(string path)
		{
			_context.Request.SetupExecutionPath(path);
		}

		protected virtual void SetHttpMethod(HttpMethod httpMethod)
		{
			_context.Request.SetHttpMethodResult(httpMethod.Method);
		}
	}
}