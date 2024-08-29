using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PopMedNet.DMCS.Data.Identity
{
    public class DMCSUserManager : UserManager<IdentityUser>
    {
        public DMCSUserManager(IUserStore<IdentityUser> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<IdentityUser> passwordHasher,
            IEnumerable<IUserValidator<IdentityUser>> userValidators,
            IEnumerable<IPasswordValidator<IdentityUser>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<DMCSUserManager> logger) : base(
                store,
                optionsAccessor,
                passwordHasher,
                userValidators,
                passwordValidators,
                keyNormalizer,
                errors,
                services,
                logger
                )
        {
        }

        /// <summary>
        /// Sets the password hash for the user, and then updates the security stamp.
        /// </summary>
        /// <param name="user">The user to update the password hash for.</param>
        /// <param name="passwordHash">The new password hash.</param>
        /// <returns></returns>
        public async Task UpdatePasswordHash(IdentityUser user, string passwordHash)
        {
            await ((IUserPasswordStore<IdentityUser>)Store).SetPasswordHashAsync(user, passwordHash, CancellationToken);
            await UpdateSecurityStampAsync(user);
        }
    }
}
