using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RadioStreamer.Common.Utils
{
    public static class Encryption
    {
        public static string HashPassword(string password, string salt)
        {
            HashAlgorithm algorithm = new SHA256Managed();

            string pswdWithSalt = password + salt;

            byte[] pswdByteArray = Encoding.UTF8.GetBytes(pswdWithSalt);

            byte[] hashedPassword = algorithm.ComputeHash(pswdByteArray);

            return Convert.ToBase64String(hashedPassword);
        }

        /// <summary>
        /// Generuje sól do hasła.
        /// </summary>
        public static string generateSalt()
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

            byte[] saltByteArray = new byte[12];
            rng.GetBytes(saltByteArray);

            return Convert.ToBase64String(saltByteArray);
        }


    }
}
