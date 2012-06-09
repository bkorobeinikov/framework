using System;
using System.Reflection;

namespace Bobasoft
{
	public static class TypeExtensions
	{
		//======================================================
		#region _Public methods_

#if WinRT
		public static bool IsAssignableFrom(this Type type, Type type2)
		{
			return type.GetTypeInfo().IsAssignableFrom(type2.GetTypeInfo());
		}
#endif

		#endregion
	}
}