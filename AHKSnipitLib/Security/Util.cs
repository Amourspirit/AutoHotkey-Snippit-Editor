using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Security
{
    /// <summary>
    /// Security Methods and Hash
    /// </summary>
    public sealed class Util
    {
        #region Hash
        /// <summary>
        /// Gets a string hash that has been encoded by SHA 256
        /// </summary>
        /// <param name="text">The String to convert to hash string</param>
        /// <returns>
        /// String hash encoded by SHA 256
        /// </returns>
        /// <remarks>
        /// If <paramref name="text"/> is empty then empty string is returned
        /// </remarks>
        public static string GetStringSha256Hash(string text)
        {
            if (String.IsNullOrEmpty(text))
                return String.Empty;

            using (var sha = new System.Security.Cryptography.SHA256Managed())
            {
                byte[] textData = System.Text.Encoding.UTF8.GetBytes(text);
                byte[] hash = sha.ComputeHash(textData);
                return BitConverter.ToString(hash).Replace("-", String.Empty);
            }
        }

        /// <summary>
        /// Gets A MD5 Hash of the input string.
        /// </summary>
        /// <param name="inputString">The String to hash</param>
        /// <returns>Byte Array</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException">If <paramref name="inputString"/> is null or empty</exception>
        /// <exception cref="EncoderFallbackException"></exception>
        public static byte[] GetHash(string inputString)
        {
            if (string.IsNullOrEmpty(inputString))
            {
                throw new ArgumentNullException("inputString");
            }
            HashAlgorithm algorithm = MD5.Create();  //or use SHA1.Create();
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        /// <summary>
        /// Gets a Md5 Hashed String for input.
        /// </summary>
        /// <param name="inputString">The string to get hash for</param>
        /// <returns>
        /// Returns a string as Hash
        /// </returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException">If <paramref name="inputString"/> is null or empty</exception>
        /// <exception cref="EncoderFallbackException"></exception>
        public static string GetHashString(string inputString)
        {
            if (string.IsNullOrEmpty(inputString))
            {
                throw new ArgumentNullException("inputString");
            }
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }
        #endregion

        #region Base64
        /// <summary>
        /// Converts a string To Base64
        /// </summary>
        /// <param name="str">String to covert to base 64</param>
        /// <returns>
        /// Returns the value of <paramref name="str"/> encoded as base 64
        /// </returns>
        /// <remarks>
        /// String bytes are stored as UTF8 in Base 64 output.
        /// If <paramref name="str"/> is empty then empty string is returned.
        /// </remarks>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="EncoderFallbackException"></exception>
        public static string StringToBase64(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }
            var plainTextBytes = Encoding.UTF8.GetBytes(str);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        /// <summary>
        /// Converts Base 64 string to UTF8 strign
        /// </summary>
        /// <param name="base64EncodedData">The base 64 string to convert</param>
        /// <returns>
        /// UTF8 string converted from base 64
        /// </returns>
        /// <remarks>If <paramref name="base64EncodedData"/> is empty then empty string is returned</remarks>
        public static string StringFromBase64(string base64EncodedData)
        {
            if (string.IsNullOrEmpty(base64EncodedData))
            {
                return string.Empty;
            }
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        /// <summary>
        /// Write file to disk from base 64 string value.
        /// </summary>
        /// <param name="FileName">The full file path to save as</param>
        /// <param name="Base64String">The base 64 data to convert and save</param>
        /// /// <exception cref="ArgumentException"></exception>
        /// /// <exception cref="ArgumentNullException">If <paramref name="FileName"/> or <paramref name="Base64String"/> is null or empty</exception>
        /// <exception cref="PathTooLongException"></exception>
        /// <exception cref="DirectoryNotFoundException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="UnauthorizedAccessException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="System.Security.SecurityException"></exception>
        public static void FileSaveFromBase64(string FileName, string Base64String)
        {
            if (string.IsNullOrEmpty(FileName))
            {
                throw new ArgumentNullException("FileName");
            }
            if (string.IsNullOrEmpty(Base64String))
            {
                throw new ArgumentNullException("Base64String");
            }

            byte[] tempBytes = Convert.FromBase64String(Base64String);
            File.WriteAllBytes(FileName, tempBytes);
        }

        /// <summary>
        /// Gets a file from disk and returns it as Base 64 string
        /// </summary>
        /// <param name="FileName">The file to convert to base 64</param>
        /// <returns>
        /// Returns a string representing <paramref name="FileName"/> as base 64 string.
        /// </returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException">If <paramref name="FileName"/> is null or empty</exception>
        /// <exception cref="FileNotFoundException">If <paramref name="FileName"/> is not found.</exception>
        /// <exception cref="PathTooLongException"></exception>
        /// <exception cref="DirectoryNotFoundException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="UnauthorizedAccessException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="System.Security.SecurityException"></exception>
        public static string FileReadToBase64(string FileName)
        {
            if (string.IsNullOrEmpty(FileName))
            {
                throw new ArgumentNullException("FileName");
            }
            if (File.Exists(FileName) == false)
            {
                throw new FileNotFoundException(Properties.Resources.ErrorFileNotFound, "FileName");
            }
            byte[] AsBytes = File.ReadAllBytes(FileName);
            String AsBase64String = Convert.ToBase64String(AsBytes);
            return AsBase64String;
        }
    }
    #endregion
}
