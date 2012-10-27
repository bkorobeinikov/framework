using System.Collections.Generic;
using DotNetOpenAuth.AspNet;

namespace Bobasoft.Web.Security
{
	public interface IOAuthAuthentication
	{
		//======================================================
		#region _Methods_

		void RequestAuthentication(AuthProvider provider, string returnUrl);
		AuthenticationResult VerifyAuthentication(string returnUrl);
		
		void RegisterMicrosoft(string clientId, string clientSecret);
		void RegisterMicrosoft(string clientId, string clientSecret, IDictionary<string, object> extraData);

		void RegisterGoogle();
		void RegisterGoogle(IDictionary<string, object> extraData);

		#endregion
	}
}