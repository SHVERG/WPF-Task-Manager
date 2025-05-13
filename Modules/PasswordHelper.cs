using System.Security.Cryptography;
using System;

namespace WpfTaskManager
{
    public static class PasswordHelper
    {
        public static void CreatePasswordHash(string password, out string hash, out string salt)
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] saltBytes = new byte[16];
                rng.GetBytes(saltBytes);
                salt = Convert.ToBase64String(saltBytes);

                using (var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 100000))
                {
                    hash = Convert.ToBase64String(pbkdf2.GetBytes(32));
                }
            }
        }

        public static bool VerifyPassword(string password, string storedHash, string storedSalt)
        {
            byte[] saltBytes = Convert.FromBase64String(storedSalt);
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 100000))
            {
                string hash = Convert.ToBase64String(pbkdf2.GetBytes(32));
                return hash == storedHash;
            }
        }
    }
}
