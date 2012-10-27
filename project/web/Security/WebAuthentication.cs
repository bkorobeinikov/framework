using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using Bobasoft.Security;
using DotNetOpenAuth.AspNet;
using DotNetOpenAuth.AspNet.Clients;

namespace Bobasoft.Web.Security
{
	/// <summary>
	/// Supports forms and OAuth authentication.
	/// </summary>
	public class WebAuthentication : IAuthentication, IOAuthAuthentication
	{
		//======================================================
		#region _Constructors_

		/// <summary>
		/// 
		/// </summary>
		/// <param name="encryption">To encrypt providername and userid.</param>
		public WebAuthentication(IEncryption<string, string, string> encryption)
		{
			_encryption = encryption;
		}

		#endregion

		//======================================================
		#region _Public properties_

		public bool IsAuthenticated 
		{
			get { return HttpContext.Current.User.Identity.IsAuthenticated; }
		}

		#endregion

		//======================================================
		#region _Public methods_

		public void SetAuthCookie(AuthProvider provider, string userId, bool createPersistantCookie = false)
		{ 
			var userName = _encryption.Encrypt(provider.ToString(), userId);
			FormsAuthentication.SetAuthCookie(userName, createPersistantCookie);
		}

		/// <summary>
		/// Will get provider and userid from httpcontext.
		/// </summary>
		/// <param name="provider"></param>
		/// <param name="userId"></param>
		/// <returns></returns>
		public bool TryGetProviderAndUserId(out AuthProvider provider, out string userId)
		{
			if (IsAuthenticated)
			{
				string providerName;
				if (_encryption.TryDecrypt(HttpContext.Current.User.Identity.Name, out providerName, out userId))
				{
					if (Enum.TryParse(providerName, true, out provider))
						return true;
				}
			}

			provider = AuthProvider.None;
			userId = null;
			return false;
		}

		public void SignOut()
		{
			FormsAuthentication.SignOut();
		}

		public void RequestAuthentication(AuthProvider provider, string returnUrl)
		{
			if (HttpContext.Current == null)
				throw new InvalidOperationException("httpcontext is null");

			var context = new HttpContextWrapper(HttpContext.Current);

			var oauthClient = GetOAuthClient(provider);
			new OpenAuthSecurityManager(context, oauthClient, null).RequestAuthentication(returnUrl);
		}

		public AuthenticationResult VerifyAuthentication(string returnUrl)
		{
			if (HttpContext.Current == null)
				throw new InvalidOperationException("HttpContext is null");

			var context = new HttpContextWrapper(HttpContext.Current);
			var providerName = OpenAuthSecurityManager.GetProviderName(context);

			AuthProvider provider;
			if (!Enum.TryParse(providerName, true, out provider))
				return AuthenticationResult.Failed;

			IAuthenticationClient client;
			if (!TryGetOAuthClient(provider, out client))
				throw new InvalidOperationException("invalid service provider name");

			return new OpenAuthSecurityManager(context, client, null).VerifyAuthentication(returnUrl);
		}

		public void RegisterMicrosoft(string clientId, string clientSecret)
		{
			RegisterClient(new MicrosoftClient(clientId, clientSecret));
		}

		public void RegisterMicrosoft(string clientId, string clientSecret, IDictionary<string, object> extraData)
		{
			RegisterClient(new MicrosoftClient(clientId, clientSecret), extraData);
		}

		public void RegisterGoogle()
		{
			RegisterClient(new GoogleOpenIdClient());
		}

		public void RegisterGoogle(IDictionary<string, object> extraData)
		{
			RegisterClient(new GoogleOpenIdClient(), extraData);
		}

		#endregion

		//======================================================
		#region _Private, protected, internal methods_

		protected virtual void RegisterClient(IAuthenticationClient client, IDictionary<string, object> extraData = null)
		{
			if (client == null)
				throw new ArgumentNullException("client");
			AuthProvider provider;
			if (!Enum.TryParse(client.ProviderName, true, out provider))
				throw new ArgumentException("Unsupported provider name");
			if (_authClients.ContainsKey(provider))
				throw new InvalidOperationException("Specified provider already registered");

			extraData = extraData ?? new Dictionary<string, object>();
			_authClients[provider] = new AuthenticationClientData(client, null, extraData);
		}

		protected IAuthenticationClient GetOAuthClient(AuthProvider provider)
		{
			if (!_authClients.ContainsKey(provider))
				throw new ArgumentException("service provider could not be found");

			return _authClients[provider].AuthenticationClient;
		}

		protected bool TryGetOAuthClient(AuthProvider provider, out IAuthenticationClient client)
		{
			if (_authClients.ContainsKey(provider))
			{
				client = _authClients[provider].AuthenticationClient;
				return true;
			}

			client = null;
			return false;
		}
		
		#endregion

		//======================================================
		#region _Private, protected, internal fields_

		protected IDictionary<AuthProvider, AuthenticationClientData> _authClients = new Dictionary<AuthProvider, AuthenticationClientData>();
		private readonly IEncryption<string, string, string> _encryption;

		#endregion
	}
}