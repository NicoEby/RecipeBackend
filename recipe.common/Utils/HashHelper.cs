using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ch.thommenmedia.common.Utils
{
    public class HashHelper
    {
        private const int MaxBufferSize = 1024 * 1024 * 100; // 100 MB

        public static string GetMD5HashFromFile(string fileName)
        {
            string retVal;

            using (var stream = new BufferedStream(File.OpenRead(fileName), 1200000))
            {
                MD5 md5 = new MD5CryptoServiceProvider();
                var hash = md5.ComputeHash(stream);
                retVal = BitConverter.ToString(hash).Replace("-", "");
            }

            return retVal;
        }

        public static string CreateMd5ForFolder(string path, bool bufferedFileRead = true)
        {
            // assuming you want to include nested folders
            var files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories)
                .OrderBy(p => p).ToList();

            var md5 = MD5.Create();

            // Haben wir überhaupt subdirectories oder files, ansonsten hashwert für den Ordnernamen zurück geben
            if (files.Count == 0)
                md5.ComputeHash(Encoding.UTF8.GetBytes(path.ToLower()));
            else
            {
                for (var i = 0; i < files.Count; i++)
                {
                    var file = files[i];

                    // hash path
                    var relativePath = file.Substring(path.Length + 1);
                    var pathBytes = Encoding.UTF8.GetBytes(relativePath.ToLower());
                    md5.TransformBlock(pathBytes, 0, pathBytes.Length, pathBytes, 0);

                    // hash content
                    if (!bufferedFileRead)
                    {
                        var contentBytes = File.ReadAllBytes(file);

                        if (i == files.Count - 1)
                            md5.TransformFinalBlock(contentBytes, 0, contentBytes.Length);
                        else
                            md5.TransformBlock(contentBytes, 0, contentBytes.Length, contentBytes, 0);
                    }
                    else
                        HashFileChunk(file, md5, i == files.Count - 1);
                }
            }

            return BitConverter.ToString(md5.Hash).Replace("-", "");
        }

        private static void HashFileChunk(string fileName, ICryptoTransform hash, bool isFinal)
        {
            var bufferSize = MaxBufferSize;

            // Ist die Datei kleiner als die max. Buffergrösse
            var size = new FileInfo(fileName).Length;

            if (size < MaxBufferSize)
                bufferSize = (int) size;

            using (
                var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read,
                    FileShare.None, bufferSize, FileOptions.SequentialScan))
            {
                fs.Position = 0;
                var ioBuffer = new byte[bufferSize];

                while (fs.Read(ioBuffer, 0, bufferSize) > 0)
                {
                    if (isFinal && fs.Position >= fs.Length)
                        hash.TransformFinalBlock(ioBuffer, 0, ioBuffer.Length);
                    else
                        hash.TransformBlock(ioBuffer, 0, ioBuffer.Length, ioBuffer, 0);

                    // Buffer an noch verfügbare Grösse anpassen
                    if (fs.Position + bufferSize > fs.Length)
                    {
                        bufferSize = (int) (fs.Length - fs.Position);
                        // Neuen Buffer mit entsprechender Grösse erstellen
                        ioBuffer = new byte[bufferSize];
                    }
                }
            }
        }

        /// <summary>
        ///     Berechnet einen MD5-Hash mit dem angegebenen String
        /// </summary>
        /// <param name="input">Der zu konventirende String</param>
        /// <param name="encoding">Kodierung des Strings (Default = ASCII)</param>
        /// <returns>MD5-Hash</returns>
        public static string CalculateMD5HashFromString(string input, Encoding encoding = null)
        {
            // step 1, calculate MD5 hash from input
            var md5 = MD5.Create();

            var inputBytes = encoding == null ? Encoding.ASCII.GetBytes(input) : encoding.GetBytes(input);
            var hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            var sb = new StringBuilder();

            for (var i = 0; i < hash.Length; i++)
                sb.Append(hash[i].ToString("x2"));

            return sb.ToString();
        }
    }
}