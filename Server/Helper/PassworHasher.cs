using System.Security.Cryptography;
using System.Text;

namespace ExScheduler_Server.Helper
{
    public class PassworHasher
    {
        public static string HashPassword( string password, out string salt)
        {
            using (var sha512  = SHA512.Create())
            {
                byte[] saltBytes = new byte[32];
                
                using (var randomNumberGenerator = RandomNumberGenerator.Create())
                {
                    randomNumberGenerator.GetBytes(saltBytes);
                }
                salt = Convert.ToBase64String(saltBytes);

                byte[] saltedPasswordBytes = Encoding.UTF8.GetBytes(password + salt);
                byte[] hashBytes = sha512.ComputeHash(saltedPasswordBytes);

                return Convert.ToBase64String(hashBytes);
            }
        }
        public static bool VerifyPassword(string password, string storedHash, string storedSalt)
        {
            byte[] saltBytes = Convert.FromBase64String(storedSalt);
            byte[] storedHashBytes = Convert.FromBase64String(storedHash);

            using (var sha512 = SHA512.Create())
            {
                byte[] saltedPasswordBytes = Encoding.UTF8.GetBytes(password + storedSalt);
                byte[] hashBytes = sha512.ComputeHash(saltedPasswordBytes);

                // Compare the computed hash with the stored hash
                return storedHashBytes.SequenceEqual(hashBytes);
            }
        }
    }
}
