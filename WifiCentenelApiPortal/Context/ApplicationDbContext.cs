using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WifiCentenelApiPortal.Models;

namespace WifiCentenelApiPortal.Context
{
    public class ApplicationDbContext: IdentityDbContext<ApplicationUser>
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Ap> Aps { get; set; }
        public DbSet<Station> Stations { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<LocationAp> LocationAps { get; set; }
        public DbSet<LocationStation> LocationStations { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<InternetAuthorization> InternetAuthorizations { get; set; }
        public DbSet<CoinLog> CoinLogs { get; set; }
        public DbSet<CoinIdentity> CoinIdentities { get; set; }
        public DbSet<Price> Prices { get; set; }
        public DbSet<LegalTerms> LegalTerms { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options) { }
        public ApplicationDbContext() { }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>()
                 .ToTable("CUser");
            builder.Entity<IdentityRole>()
                   .ToTable("CRole");
            builder.Entity<IdentityUserRole<string>>()
                   .ToTable("CUserRole");
            builder.Entity<IdentityUserClaim<string>>()
                   .ToTable("CUserClaim");
            builder.Entity<IdentityUserLogin<string>>()
                   .ToTable("CUserLogin");
            builder.Entity<IdentityRoleClaim<string>>()
                   .ToTable("CRoleClaim");
            builder.Entity<IdentityUserToken<string>>()
                   .ToTable("CUserToken");

            builder
                .Entity<ApplicationUser>()
                .Property(e => e.UserType)
                .HasConversion(
                    v => v.ToString(),
                    v => (UserType)Enum.Parse(typeof(UserType), v));

        }

        public bool Exist()
        {

            return false;

        }
    }
}
