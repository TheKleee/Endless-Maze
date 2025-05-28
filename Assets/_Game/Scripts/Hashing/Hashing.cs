using System.Security.Cryptography;
using System.Text;

namespace EM_Hashing
{
    public static class Hashing
    {
        public static string Hash(this string data)
        {
            byte[] textToBytes = Encoding.UTF8.GetBytes(data);
            SHA256 mySHA256 = SHA256.Create();

            byte[] hashValue = mySHA256.ComputeHash(textToBytes);

            return GetHexStringFromHash(hashValue);
        }

        static string GetHexStringFromHash(byte[] hash)
        {
            string hexString = string.Empty;

            foreach (byte b in hash)
                hexString += b.ToString("x2");

            return hexString;
        }
    }
}