using DotNetOpenAuth.AspNet;
using System;
using System.Collections.Generic;

namespace Bobasoft.Web.Security
{
	public class AuthenticationClientData
	{
		public IAuthenticationClient AuthenticationClient { get; private set; }

		public string DisplayName { get; private set; }

		public IDictionary<string, object> ExtraData { get; private set; }

		public AuthenticationClientData(IAuthenticationClient authenticationClient, string displayName, IDictionary<string, object> extraData)
		{
			if (authenticationClient == null)
				throw new ArgumentNullException("authenticationClient");
			AuthenticationClient = authenticationClient;
			DisplayName = displayName;
			ExtraData = extraData;
		}
	}
}
