using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TucaAPI.src.Common;
using TucaAPI.src.Models;

namespace TucaAPI.src.Data
{
    public class ApplicationDBContext : IdentityDbContext<AppUser>
    {
        public ApplicationDBContext(DbContextOptions dbContextOptions)
            : base(dbContextOptions) { }

        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Portfolio> Portfolios { get; set; }
        public DbSet<PasswordHistory> PasswordHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Portfolio>(i => i.HasKey(p => new { p.AppUserId, p.StockId }));

            builder.Entity<PasswordHistory>(i => i.HasKey(p => new { p.AppUserId, p.Id }));

            builder
                .Entity<PasswordHistory>()
                .HasOne(i => i.AppUser)
                .WithMany(i => i.PasswordHistory)
                .HasForeignKey(i => i.AppUserId);

            builder
                .Entity<Portfolio>()
                .HasOne(i => i.AppUser)
                .WithMany(i => i.Portfolios)
                .HasForeignKey(i => i.AppUserId);

            builder
                .Entity<Portfolio>()
                .HasOne(i => i.Stock)
                .WithMany(i => i.Portfolios)
                .HasForeignKey(i => i.StockId);

            List<IdentityRole> roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Name = PermissionRoles.ADM,
                    NormalizedName = PermissionRoles.ADM.Normalize()
                },
                new IdentityRole
                {
                    Name = PermissionRoles.USER,
                    NormalizedName = PermissionRoles.USER.Normalize()
                }
            };

            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
