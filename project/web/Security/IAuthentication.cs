namespace Bobasoft.Web.Security
{
	public interface IAuthentication
	{
		//======================================================
		#region _Properties_

		bool IsAuthenticated { get; }

		#endregion

		//======================================================
		#region _Methods_

		void SetAuthCookie(AuthProvider provider, string userId, bool createPersistantCookie = false);
		bool TryGetProviderAndUserId(out AuthProvider provider, out string userId);

		void SignOut();

		#endregion
	}
}