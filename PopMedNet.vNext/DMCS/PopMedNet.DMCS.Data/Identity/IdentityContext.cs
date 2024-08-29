using Microsoft.EntityFrameworkCore;
using System;

namespace PopMedNet.DMCS.Data.Identity
{
    public class IdentityContext : Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityDbContext<IdentityUser, Microsoft.AspNetCore.Identity.IdentityRole<Guid>, Guid>
    {
        public IdentityContext(Microsoft.EntityFrameworkCore.DbContextOptions<IdentityContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
