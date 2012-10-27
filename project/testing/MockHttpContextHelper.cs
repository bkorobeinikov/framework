using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;

namespace Bobasoft.Testing
{
	public static class MockHttpContextHelper
	{
		public static HttpContextBase FakeHttpContext()
		{
			var context = new Mock<HttpContextBase>();
			var request = new Mock<HttpRequestBase>();
			var response = new Mock<HttpResponseBase>();
			var session = new Mock<HttpSessionStateBase>();
			var server = new Mock<HttpServerUtilityBase>();

			context.Setup(ctx => ctx.Request).Returns(request.Object);
			context.Setup(ctx => ctx.Response).Returns(response.Object);
			context.Setup(ctx => ctx.Session).Returns(session.Object);
			context.Setup(ctx => ctx.Server).Returns(server.Object);

			return context.Object;
		}

		public static HttpContextBase FakeHttpContext(string url)
		{
			var context = FakeHttpContext();
			context.Request.SetupRequestUrl(url);
			return context;
		}

		public static void SetFakeControllerContext(this Controller controller)
		{
			var httpContext = FakeHttpContext();
			var context = new ControllerContext(new RequestContext(httpContext, new RouteData()), controller);
			controller.ControllerContext = context;
		}

		static string GetUrlFileName(string url)
		{
			return url.Contains("?") ? url.Substring(0, url.IndexOf("?", StringComparison.InvariantCulture)) : url;
		}

		static NameValueCollection GetQueryStringParameters(string url)
		{
			if (url.Contains("?"))
			{
				var parameters = new NameValueCollection();

				var parts = url.Split("?".ToCharArray());
				var keys = parts[1].Split("&".ToCharArray());

				foreach (string key in keys)
				{
					var part = key.Split("=".ToCharArray());
					parameters.Add(part[0], part[1]);
				}

				return parameters;
			}

			return null;
		}

		public static void SetHttpMethodResult(this HttpRequestBase request, string httpMethod)
		{
			Mock.Get(request).Setup(req => req.HttpMethod).Returns(httpMethod);
		}

		public static void SetupRequestUrl(this HttpRequestBase request, string url)
		{
			if (url == null)
				throw new ArgumentNullException("url");

			var mock = Mock.Get(request);

			mock.Setup(req => req.Url).Returns(new Uri(url));
			mock.Setup(req => req.QueryString).Returns(GetQueryStringParameters(url));
		}

		public static void SetupExecutionPath(this HttpRequestBase request, string url)
		{
			if (url == null)
				throw new ArgumentNullException("url");

			if (!url.StartsWith("~"))
				throw new ArgumentException("Url should start with '~'", "url");

			var mock = Mock.Get(request);

			mock.Setup(req => req.AppRelativeCurrentExecutionFilePath).Returns(url);
			mock.Setup(req => req.PathInfo).Returns(string.Empty);
		}
	}
}