using System.Text.RegularExpressions;

namespace Bobasoft.Cloud.Azure
{
    internal static class SecurityHelper
    {
        //======================================================
        #region _Public methods_

        public static bool IsValidTableName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return false;

            var reg = new Regex(ValidTableNameRegex);
            return reg.IsMatch(name);
        }

        public static bool IsValidContainerName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return false;

            var reg = new Regex(ValidContainerNameRegex);
            return reg.IsMatch(name);
        }

        #endregion

        //======================================================
        #region _Private, protected, internal fields_

        private const string ValidTableNameRegex = @"^([a-z]|[A-Z]){1}([a-z]|[A-Z]|\d){2,62}$";
        private const string ValidContainerNameRegex = @"^([a-z]|\d){1}([a-z]|-|\d){1,61}([a-z]|\d){1}$";

        #endregion
    }
}