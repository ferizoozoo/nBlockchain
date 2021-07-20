using System.Text;
using System.Security.Cryptography;
using System;

namespace Blockchain.Utilities
{
    public static class Hash
    {
        public static string CreateHash(string data)
        {
            StringBuilder Sb = new StringBuilder();

            using (var hash = SHA256.Create())            
            {
                Encoding enc = Encoding.UTF8;
                Byte[] result = hash.ComputeHash(enc.GetBytes(data));

                foreach (Byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();

        }
    }
}
