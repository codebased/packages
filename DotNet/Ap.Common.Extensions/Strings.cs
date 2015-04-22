using System;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace Ap.Common.Extensions
{
    public static class Strings
    {
        public static string RemoveWhiteSpaces(this string source)
        {
            if (String.IsNullOrEmpty(source))
                return source;

            return source.Replace(" ", string.Empty);
        }

        public static Guid ReadOrCreateGuid(this string value)
        {
            Guid result;
            result = Guid.TryParse(value, out result) ? result : Guid.NewGuid();

            return result;
        }

        public static string FormatMessage(this string value, params object[] args)
        {
            return string.Format(value, args);
        }

        /// <summary>
        /// Convert string to integer 16 bits.
        /// </summary>
        /// <param name="value">string value</param>
        /// <returns>Converted short type value</returns>
        public static short ToShort(this string value)
        {
            short result = 0;

            if (value.IsNotNullOrWhiteSpace())
            {
                short.TryParse(value, out result);
            }

            return result;
        }

        /// <summary>
        /// Convert string to integration 32 bits.
        /// </summary>
        /// <param name="value">string value</param>
        /// <returns>Converted integer type value</returns>
        public static int ToInt(this string value)
        {
            var result = 0;

            if (value.IsNotNullOrWhiteSpace())
            {
                int.TryParse(value, out result);
            }

            return result;
        }

        /// <summary>
        /// Convert string to integer 32 bits.
        /// </summary>
        /// <param name="value">string value</param>
        /// <returns>Converted long type value</returns>
        public static long ToLong(this string value)
        {
            long result = 0;

            if (value.IsNotNullOrWhiteSpace())
            {
                long.TryParse(value, out result);
            }

            return result;
        }

        /// <summary>
        /// Check for empty string value. It is the reverse of IsNullOrWhiteSpace...;
        /// </summary>
        /// <param name="value">string value</param>
        /// <returns>Return true if not null otherwise it returns false</returns>
        public static bool IsNotNullOrWhiteSpace(this string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }


        public static bool ValidEmailAddress(this string s)
        {
            return new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,6}$").IsMatch(s);
        }

        public static bool ValidUrl(this string url)
        {
            const string strRegex = "^(https?://)"
                                    + "?(([0-9a-z_!~*'().&=+$%-]+: )?[0-9a-z_!~*'().&=+$%-]+@)?" //user@
                                    + @"(([0-9]{1,3}\.){3}[0-9]{1,3}" // IP- 199.194.52.184
                                    + "|" // allows either IP or domain
                                    + @"([0-9a-z_!~*'()-]+\.)*" // tertiary domain(s)- www.
                                    + @"([0-9a-z][0-9a-z-]{0,61})?[0-9a-z]" // second level domain
                                    + @"(\.[a-z]{2,6})?)" // first level domain- .com or .museum is optional
                                    + "(:[0-9]{1,5})?" // port number- :80
                                    + "((/?)|" // a slash isn't required if there is no file name
                                    + "(/[0-9a-z_!~*'().;?:@&=+$,%#-]+)+/?)$";

            return new Regex(strRegex).IsMatch(url);
        }

        public static bool IsActiveUrl(this string source)
        {
            if (!source.StartsWith("http://", StringComparison.CurrentCultureIgnoreCase) ||
                !source.StartsWith("https://", StringComparison.CurrentCultureIgnoreCase))
                source = "http://" + source;
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(source);
                request.Method = "GET";
                request.ContentType = "application/x-www-form-urlencoded";
                var webresponse = (HttpWebResponse)request.GetResponse();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string QueryStringKeyValue(this string queryString, string paramName)
        {
            if (string.IsNullOrWhiteSpace(queryString) || string.IsNullOrWhiteSpace(paramName)) return string.Empty;

            var query = queryString.Replace("?", "");
            if (!query.Contains("=")) return string.Empty;
            var queryValues = query.Split('&').Select(piQ => piQ.Split('=')).ToDictionary(
                piKey => piKey[0].ToLower().Trim(), piValue => piValue[1]);
            string result;
            var found = queryValues.TryGetValue(paramName.ToLower().Trim(), out result);
            return found ? result : string.Empty;
        }

        public static string Map(this string source, NameValueCollection data)
        {
            return Regex.Replace(source, @"\{(.+?)\}", match => data[match.Groups[1].Value]);
        }
    }
}