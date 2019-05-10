using System;
using System.Globalization;

namespace ch.thommenmedia.common.Utils
{
    public static class FileSizeHelper
    {
        public static string GetHumanReadableSize(long bytes)
        {
            if (bytes <= 0)
                return "0B";
            string[] suf = {"B", "KB", "MB", "GB", "TB", "PB"};
            var place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            var num = Math.Round(bytes / Math.Pow(1024, place), 1);
            var readable = num.ToString(CultureInfo.InvariantCulture) + suf[place];
            return readable;
        }
    }
}