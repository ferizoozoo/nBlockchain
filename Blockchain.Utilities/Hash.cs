using System.Text;
using System.Security.Cryptography;
using System;

namespace Blockchain.Utilities
{
    public static class Hash
    {
        public static string CreateHash(string data)
        {
            if (!string.IsNullOrEmpty(data))
                return string.Empty;

            var dataBytes = Encoding.Unicode.GetBytes(data);

            var sha256 = SHA256.Create();
            return sha256.ComputeHash(dataBytes).ToString();
        }
    }
}
