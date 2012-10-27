using System.Web.Mvc;

namespace Bobasoft.Web.Security
{
	public class OAuthLoginResult : ActionResult
	{
		//======================================================
		#region _Constructors_

		public OAuthLoginResult(IOAuthAuthentication oAuthAuthentication, AuthProvider provider, string returnUrl)
		{
			_oAuthAuthentication = oAuthAuthentication;

			Provider = provider;
			ReturnUrl = returnUrl;
		}

		#endregion

		//======================================================
		#region _Public properties_

		public AuthProvider Provider { get; private set; }
		public string ReturnUrl { get; private set; }

		#endregion

		//======================================================
		#region _Public methods_

		public override void ExecuteResult(ControllerContext context)
		{
			_oAuthAuthentication.RequestAuthentication(Provider, ReturnUrl);
		}

		#endregion

		//======================================================
		#region _Private, protected, internal fields_

		private readonly IOAuthAuthentication _oAuthAuthentication;

		#endregion
	}
}