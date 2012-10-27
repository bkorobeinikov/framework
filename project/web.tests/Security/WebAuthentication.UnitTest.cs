using System;
using Bobasoft.Security;
using Bobasoft.Testing.Web;
using Bobasoft.Web.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Bobasoft.Web.Net40.Tests.Security
{
	[TestClass]
	public class WebAuthenticationUnitTest : HttpUnitTest
	{
		protected Mock<IEncryption<string, string, string>> _encryption;
		protected WebAuthentication _webAuthentication;

		[TestInitialize]
		public void SetUp()
		{
			_encryption = new Mock<IEncryption<string, string, string>>();
			_webAuthentication = new WebAuthentication(_encryption.Object);
		}

		[TestMethod]
		public void RegisterMicrosoftClientShouldSuccess()
		{
			_webAuthentication.RegisterMicrosoft("clientid", "clientSecret");
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void RegisterTwoMicrosoftClientShouldFail()
		{
			_webAuthentication.RegisterMicrosoft("clientid", "clientSecret");
			_webAuthentication.RegisterMicrosoft("clientid", "clientSecret");
		}

		[TestMethod]
		public void RegisterGoogleClientShouldSuccess()
		{
			_webAuthentication.RegisterGoogle();
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void RegisterTwoGoogleClientShouldFail()
		{
			_webAuthentication.RegisterGoogle();
			_webAuthentication.RegisterGoogle();
		}

		[TestMethod]
		public void TryGetProviderAndUserIdShouldUseHttpContextUserIndentity()
		{
			var param1 = "Microsoft";
			var param2 = "userid";
			_encryption.Setup(e => e.TryDecrypt("name", out param1, out param2)).Returns(true);
			_identity.Setup(i => i.Name).Returns("name");
			_identity.Setup(i => i.IsAuthenticated).Returns(true);

			AuthProvider assert1;
			string assert2;
			_webAuthentication.TryGetProviderAndUserId(out assert1, out assert2);

			Assert.AreEqual(AuthProvider.Microsoft, assert1);
			Assert.AreEqual(param2, assert2);
		}
	}
}