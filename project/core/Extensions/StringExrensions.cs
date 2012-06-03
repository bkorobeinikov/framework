using System;

namespace Bobasoft
{
    public static class StringExrensions
    {
        //======================================================
        #region _Public methods_

        public static bool Contains(this string original, string value, StringComparison comparisionType)
        {
            return original.IndexOf(value, comparisionType) >= 0;
        }

        public static string With(this string s, params object[] args)
        {
            return string.Format(s, args);
        }

        #endregion
    }
}