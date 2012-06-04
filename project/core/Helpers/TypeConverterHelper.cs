using System;

namespace Bobasoft
{
    public class TypeConverterHelper
    {
        //======================================================
        #region _Public methods_

        public static T Convert<T>(object value)
        {
            return value is string ? ConvertFromString<T>(value as string) : (T) value;
        }

        public static T ConvertFromString<T>(string value)
        {
            return (T)ConvertFromString(typeof (T), value);
        }

        public static object ConvertFromString(Type type, string value)
        {
            if (string.IsNullOrEmpty(value) || value == "null")
                return null;
			
#if !WinRT
            if (typeof(string).IsAssignableFrom(type))
                return value;

            if (typeof(Enum).IsAssignableFrom(type))
                return Enum.Parse(type, value, true);

            if (typeof(bool).IsAssignableFrom(type))
                return bool.Parse(value);

            if (typeof(int).IsAssignableFrom(type))
                return int.Parse(value);

            if (typeof(double).IsAssignableFrom(type))
                return double.Parse(value);

            if (typeof(Guid).IsAssignableFrom(type))
                return new Guid(value);

            if (typeof(long).IsAssignableFrom(type))
                return long.Parse(value);

            // ------------------ not often used ----

            if (typeof(uint).IsAssignableFrom(type))
                return uint.Parse(value);

            if (typeof(ulong).IsAssignableFrom(type))
                return ulong.Parse(value);

            if (typeof(float).IsAssignableFrom(type))
                return float.Parse(value);

            if (typeof(byte).IsAssignableFrom(type))
                return byte.Parse(value);

            if (typeof(sbyte).IsAssignableFrom(type))
                return sbyte.Parse(value);
#endif

            return null;
        }

        #endregion
    }
}