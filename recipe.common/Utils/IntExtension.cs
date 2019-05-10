namespace ch.thommenmedia.common.Utils
{
    public static class IntExtension
    {
        /// <summary>
        /// converts an int to a string with leading zeros
        /// </summary>
        /// <param name="number"></param>
        /// <param name="length">number of leading zeros (default = 5)</param>
        /// <returns></returns>
        public static string ToStringWithLeadingZeros(this int number, int length = 5)
        {
            return number.ToString("D" + length);
        }
    }
}
