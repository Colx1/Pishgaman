using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pishgaman.Areas.Identity.Data;
using Pishgaman.Models;

namespace Pishgaman.Data
{
    public class PishgamanDB : IdentityDbContext<ApplicationUser>
    {
        public PishgamanDB(DbContextOptions<PishgamanDB> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<Person> Tbl_Persons { get; set; }
        public DbSet<IpAddressBlacklist> Tbl_IpAddressBlacklists { get; set; }
        public DbSet<JwtStoredToken> Tbl_JwtStoredTokens { get; set; }
    }
}
