/// Written by: Yulia Danilova
/// Creation Date: 30th of July, 2018
/// Purpose: Handles hashing and salting of passwords
#region ========================================================================= USING =====================================================================================
using System;
using System.Security.Cryptography;
#endregion

namespace Leya.Infrastructure.Security
{
    public sealed class PasswordHash
    {
        #region ============================================================== FIELD MEMBERS ================================================================================
        private const int SALT_SIZE = 16;
        private const int HASH_SIZE = 20;
        #endregion

        #region ================================================================= METHODS ===================================================================================        
        /// <summary>
        /// Creates a hash from a string
        /// </summary>
        /// <param name="password">The string representing the password to be hashed</param>
        /// <param name="iterations">The number of iterations for the hashing operation</param>
        /// <returns>The hashed string</returns>
        private static string Hash(string password, int iterations)
        {
            //create salt
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[SALT_SIZE]);
            //create hash
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations);
            byte[] hash = pbkdf2.GetBytes(HASH_SIZE);
            //combine salt and hash
            byte[] hashBytes = new byte[SALT_SIZE + HASH_SIZE];
            Array.Copy(salt, 0, hashBytes, 0, SALT_SIZE);
            Array.Copy(hash, 0, hashBytes, SALT_SIZE, HASH_SIZE);
            //convert to base64
            string base64Hash = Convert.ToBase64String(hashBytes);
            //format hash with extra information
            return string.Format("$MYHASH$V1${0}${1}", iterations, base64Hash);
        }

        /// <summary>
        /// Creates a hash from a string with 10000 iterations
        /// </summary>
        /// <param name="password">The string representing the password to be hashed</param>
        /// <returns>The hashed string</returns>
        public static string Hash(string password)
        {
            return Hash(password, 10000);
        }

        /// <summary>
        /// Checks if a hash is supported
        /// </summary>
        /// <param name="hashString">The hashed string to be checked</param>
        /// <returns>True if the hash is supported, False otherwise</returns>
        private static bool IsHashSupported(string hashString)
        {
            return hashString.Contains("$MYHASH$V1$");
        }

        /// <summary>
        /// Verifies a password against a hash
        /// </summary>
        /// <param name="password">The string representing the password to be checked</param>
        /// <param name="hashedPassword">The hashed representation of the string representing the password to be checked</param>
        /// <returns>True if <paramref name="password"/> and the de-hashed verions of <paramref name="hashedPassword"/> are equal, False otherwise</returns>
        public static bool CheckStringAgainstHash(string password, string hashedPassword)
        {
            // check hash
            if (!IsHashSupported(hashedPassword))
                throw new NotSupportedException("The hashtype is not supported");
            // extract iteration and Base64 string
            string[] splittedHashString = hashedPassword.Replace("$MYHASH$V1$", string.Empty).Split('$');
            int iterations = int.Parse(splittedHashString[0]);
            string base64Hash = splittedHashString[1];
            // get hashbytes
            byte[] hashBytes = Convert.FromBase64String(base64Hash);
            // get salt
            byte[] salt = new byte[SALT_SIZE];
            Array.Copy(hashBytes, 0, salt, 0, SALT_SIZE);
            // create hash with given salt
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations);
            byte[] hash = pbkdf2.GetBytes(HASH_SIZE);
            // get result
            for (var i = 0; i < HASH_SIZE; i++)
                if (hashBytes[i + SALT_SIZE] != hash[i])
                    return false;
            return true;
        }
        #endregion
    }
}
