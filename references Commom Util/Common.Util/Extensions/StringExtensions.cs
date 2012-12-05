using System;
using System.Text.RegularExpressions;

namespace Common.Util
{
    public static class StringExtensions
    {
        public static string UtilMySqlCleanString(this string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                return str.Replace("\\", "\\\\").Replace("'", "\\'");
            }
            return str;
        }

        public static Match Regex_Matches(this string str, string regExpression)
        {
            if (!string.IsNullOrEmpty(str))
            {
                return Regex.Match(str, regExpression);
            }
            return null;
        }

        public static decimal? GetDecimal(this string str)
        {
            decimal? retval = null;
            decimal val;

            if (decimal.TryParse(str, out val))
            {
                retval = val;
            }

            return retval;
        }

        public static DateTime? GetDateTime(this string str)
        {
            DateTime? retval = null;
            DateTime val;

            if (DateTime.TryParse(str, out val))
            {
                retval = val;
            }

            return retval;
        }


        public static string UtilCleanUrlString(this string plainText)
        {
            if(!string.IsNullOrEmpty(plainText))
            {
            string regex = @"((www\.|(http|https|ftp|news|file)+\:\/\/)[&#95;.a-z0-9-]+\.[a-z0-9\/&#95;:@=.+?,##%&~-]*[^.|\'|\# |!|\(|?|,| |>|<|;|\)])";
            Regex r = new Regex(regex, RegexOptions.IgnoreCase);
            string replaceString = r.Replace(plainText, String.Empty);
            string withoutAngerString = Regex.Replace(replaceString, "<.*?>", string.Empty);
            return Regex.Replace(withoutAngerString, @"[\\\/:\*\?#<>|&]", String.Empty).Replace(".", string.Empty).Replace("-", string.Empty).Replace("_", string.Empty).Replace("'", string.Empty);
            }
            return plainText;
        }

        public static string UtilAuthorString(this string authorName)
        {
            string returnString = string.Empty;
            if (!string.IsNullOrEmpty(authorName))
            {
                returnString = authorName.Replace(" ", "-");
            }
            return returnString;
        }
        public static string RemoveHTMLTagAndASCII(this string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                //str = Regex.Replace(str, @"<(.|\n)*?>", "");
                str = Regex.Replace(str, @"</?table[^>]*>|</?tr[^>]*>|</?td[^>]*>|</?thead[^>]*>|</?tbody[^>]*>", "").Replace(" ", "").Replace("\n", "<br>");
                str = Regex.Replace(str, @"[^\u0000-\u007F]", "");
            }
            return str;
        }


    }
}
