using System;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Bobasoft.Cloud.Azure
{
    public static class TableKeyHelper
    {
        //======================================================
        #region _Public methods_

        /// <summary>
        /// the table storage system currently does not support the StartsWith() operation in queries. 
        /// As a result we transform s.StartsWith(substring) into   
        /// <code>
        ///     (s.CompareTo(substring) > 0) && (s.CompareTo(NextComparisonString(substring)) "les than" 0)
        /// </code>
        /// we assume that comparison on the service side is as ordinal comparison
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string NextComparisonString(string str)
        {
            if (string.IsNullOrEmpty(str))
                throw new ArgumentException("The string argument must not be null or empty!");

            var last = str[str.Length - 1];

            if (last + 1 > char.MaxValue)
                throw new ArgumentException("Cannot convert the string.");

            // don't use "as" because we want to have an explicit exception here if something goes wrong
            last = (char)(last + 1);

            return str.Substring(0, str.Length - 1) + last;
        }

        /// <summary>
        /// Combines two strings to key.
        /// </summary>
        /// <param name="keyPart1">First part of key.</param>
        /// <param name="keyPart2">Second part of key.</param>
        /// <returns>The key with separted keys part.</returns>
        public static string CombineToKey(string keyPart1, string keyPart2)
        {
            return string.Format("{1}{0}{2}{0}", KeySeparator, Escape(keyPart1), Escape(keyPart2));
        }

        /// <summary>
        /// Combines only one string to key.
        /// </summary>
        /// <param name="keyPart1">First part of key.</param>
        /// <returns>The key with only first part of key. (let asume that second part is empty string)</returns>
        public static string CombineToKey(string keyPart1)
        {
            return string.Format("{1}{0}", KeySeparator, Escape(keyPart1));
        }

        /// <summary>
        /// Combines several strings to key.
        /// </summary>
        /// <param name="keyParts">Key parts.</param>
        /// <returns>The key with several parts.</returns>
        public static string CombineToKey(params string[] keyParts)
        {
            if (keyParts.Length == 0)
                throw new InvalidOperationException("keys part must be >= 1");

            if (keyParts.Length == 1)
                return CombineToKey(keyParts[0]);

            if (keyParts.Length == 2)
                return CombineToKey(keyParts[0], keyParts[1]);

            var key = new StringBuilder();

            //key.Append(Escape(keyParts[0]));
            for (var i = 0; i < keyParts.Length; i++)
            {
                key.Append(Escape(keyParts[i]));
                key.Append(KeySeparator);
            }

            return key.ToString();
        }

        /// <summary>
        /// Retrives the first part (string) from key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The first part of key.</returns>
        public static string GetFirstFromKey(string key)
        {
            var separatorIndex = key.IndexOf(KeySeparator);
            if (separatorIndex == -1)
                throw new InvalidOperationException(string.Format("Invalid key \"{0}\". Key does not contains separator.", key));

            var first = key.Substring(0, separatorIndex);
            return UnEscape(first);
        }

        /// <summary>
        /// Retrives the second part (string) from key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The second part of key.</returns>
        public static string GetSecondFromKey(string key)
        {
            var separatorIndex = key.IndexOf(KeySeparator);
            if (separatorIndex == -1)
                throw new InvalidOperationException(string.Format("Invalid key \"{0}\". Key does not contains separator.", key));
            
            var secondSeparatorIndex = key.IndexOf(KeySeparator, separatorIndex + 1);
            if (secondSeparatorIndex == -1)
                throw new InvalidOperationException(string.Format("Invalid key \"{0}\". Key does not contains second separator.", key));

            var second = key.Substring(separatorIndex + 1, secondSeparatorIndex);
            return UnEscape(second);
        }

        /// <summary>
        /// Retrives all parts (string) from key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>All parts of key.</returns>
        public static string[] GetKeyParts(string key)
        {
            return key.Split(new[] {KeySeparator}, StringSplitOptions.RemoveEmptyEntries).Select(UnEscape).ToArray();
        }

        #endregion

        //======================================================
        #region _Private, protected, internal methods_
        
        // Some characters can cause problems when they are contained in columns 
        // that are included in queries. We are very defensive here and escape a wide range 
        // of characters for key columns (as the key columns are present in most queries)
        internal static bool IsInvalidKeyCharacter(char c)
        {
            return ((c < 32)
                || (c >= 127 && c < 160)
                || (c == '#')
                || (c == '&')
                || (c == '+')
                || (c == '/')
                || (c == '?')
                || (c == ':')
                || (c == '%')
                || (c == '\\')
                );
        }

        internal static string CharToEscapeSequence(char c)
        {
            return EscapeCharacterString + string.Format(CultureInfo.InvariantCulture, "{0:X2}", (int)c);
        }

        internal static string Escape(string s)
        {
            if (string.IsNullOrEmpty(s))
                return s;

            var ret = new StringBuilder();
            foreach (var c in s)
            {
                if (c == EscapeCharacter || c == KeySeparator || IsInvalidKeyCharacter(c))
                    ret.Append(CharToEscapeSequence(c));
                else
                    ret.Append(c);
            }

            return ret.ToString();
        }

        internal static string UnEscape(string s)
        {
            if (string.IsNullOrEmpty(s))
                return s;

            var ret = new StringBuilder();
            for (var i = 0; i < s.Length; i++)
            {
                var c = s[i];
                if (c == EscapeCharacter)
                {
                    if (i + 2 >= s.Length)
                        throw new FormatException("The string " + s + " is not correctly escaped!");

                    var ascii = Convert.ToInt32(s.Substring(i + 1, 2), 16);
                    ret.Append((char)ascii);
                    i += 2;
                }
                else
                {
                    ret.Append(c);
                }
            }

            return ret.ToString();
        }

        #endregion

        //======================================================
        #region _Private, protected, internal fields_

        //private static readonly string KeySeparator = "_";

        // we use a normal character as the separator because of string comparison operations
        // these have to be valid characters
        private const char KeySeparator = 'a';
        private static readonly string KeySeparatorString = new string(KeySeparator, 1);
        private const char EscapeCharacter = 'b';
        private static readonly string EscapeCharacterString = new string(EscapeCharacter, 1);

        #endregion
    }
}