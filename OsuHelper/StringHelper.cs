using System;

namespace OsuApiHelper
{
    public static class StringHelper
    {
        public static string Base64Encode(this string plainText) {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        
        public static string Base64Decode(this string base64EncodedData) {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
        
        public static string FormatNumber(this string s)
        {
            return Convert.ToInt64(s).FormatNumber();
        }
        
        public static string FormatNumber(this long s)
        {
            return String.Format("{0:n0}", s);
        }
        
        public static string FormatNumber(this double s)
        {
            return String.Format("{0:n0}", s);
        }
        
        public static string FormatNumber(this int s)
        {
            return String.Format("{0:n0}", s);
        }
    }
}