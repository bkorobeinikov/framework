using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bobasoft
{
    public static class UriHelper
    {
        //======================================================
        #region _Public methods_

        /// <summary>
        /// Builds http url with specified <paramref name="host"/> and <paramref name="port"/>.
        /// </summary>
        /// <param name="host">The host string. </param>
        /// <param name="port">The port number.</param>
        /// <example>host - wwww.contoso.com; port - 8081;</example>
        /// <returns>Returns builded http url. http://contoso.com:8081/
        /// </returns>
        public static string BuildHttpUrl(string host, int port)
        {
            return new UriBuilder("http", host, port).ToString();
        }

        public static IDictionary<string, string> ParseQuery(string query)
        {
            return query.Remove(0, 1).Split('&').Select(s => s.Split('=')).ToDictionary(pair => pair[0], pair => pair[1]);
        }

        public static string BuildQuery(IDictionary<string, string> query)
        {
            var builder = new StringBuilder();

            foreach (var pair in query)
                builder.AppendFormat("{0}={1}&", pair.Key, pair.Value);

            builder.Remove(builder.Length - 1, 1);

            return builder.ToString();
        }

        #endregion
    }
}