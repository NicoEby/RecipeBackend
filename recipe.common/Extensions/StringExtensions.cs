using System.Text;

namespace ch.thommenmedia.common.Extensions
{
    public static class StringExtensions
    {
        public static string Apply(this string orin, params object[] args)
        {
            return string.Format(orin, args);
        }

        /// <summary>
        ///     Converts a String to a ByteArray
        /// </summary>
        /// <param name="orin">input string</param>
        /// <param name="encoding">The Encoding to Use, default ist Systemdefault</param>
        /// <returns></returns>
        public static byte[] ToByteArray(this string orin, Encoding encoding = null)
        {
            return encoding == null ? Encoding.Default.GetBytes(orin) : encoding.GetBytes(orin);
        }
    }
}