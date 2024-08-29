using Microsoft.AspNetCore.Identity;
using System;
using System.Text;

namespace PopMedNet.DMCS.Data.Identity
{
    public class PasswordHasher : IPasswordHasher<IdentityUser>
    {
        public string HashPassword(IdentityUser user, string password)
        {
            return ComputeHash(password);
        }

        public PasswordVerificationResult VerifyHashedPassword(IdentityUser user, string hashedPassword, string providedPassword)
        {
            if (string.IsNullOrEmpty(hashedPassword))
            {
                throw new ArgumentNullException(nameof(hashedPassword));
            }
            if (string.IsNullOrEmpty(providedPassword))
            {
                throw new ArgumentNullException(nameof(providedPassword));
            }

            string computedHash = ComputeHash(providedPassword);

            if (string.Equals(hashedPassword, computedHash, StringComparison.Ordinal))
            {
                return PasswordVerificationResult.Success;
            }

            return PasswordVerificationResult.Failed;
        }

        static string ComputeHash(string password)
        {
            using (var sha = new System.Security.Cryptography.SHA256Managed())
            {
                sha.Initialize();
                byte[] hash = null;
                try
                {
                    hash = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
                    return Convert.ToBase64String(hash);
                }
                finally
                {
                    hash = null;
                }
            }
        }
    }
}
