using Microsoft.EntityFrameworkCore;
using AMaz.Entity;
using File = AMaz.Entity.File;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace AMaz.DB
{
    public class AMazDbContext : IdentityDbContext<User>
    {
        public DbSet<AcademicYear> AcademicYears { get; set; }
        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<Magazine> Magazines { get; set; }
        public override DbSet<User> Users { get; set; }
        public DbSet<Contribution> Contributions { get; set;}
        public DbSet<File> Files { get; set; }

        public AMazDbContext(DbContextOptions<AMazDbContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Identity
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable(name: "User");
            });
            modelBuilder.Entity<IdentityRole>(entity =>
            {
                entity.ToTable(name: "Role");
            });
            modelBuilder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("UserRoles");
            });

            modelBuilder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("UserClaims");
            });

            modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("UserLogins");
            });

            modelBuilder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("RoleClaims");
            });

            modelBuilder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("UserTokens");
            });
            #endregion

            #region Table relationships
            // If Delete Mangazine ==> set null all the contribution
            modelBuilder.
                Entity<Magazine>().
                HasMany( m => m.Contributions).
                WithOne( c => c.Magazine).
                OnDelete(DeleteBehavior.Cascade);

            // If Delete AcademicYear ==> Just set the AcademicYear in magazine to null
            modelBuilder.
                Entity<AcademicYear>().
                HasMany(m => m.Magazines).
                WithOne(c => c.AcademicYear).
                OnDelete(DeleteBehavior.SetNull);

            //When create user, they are automatically active
            modelBuilder.
                Entity<User>().
                Property(u => u.IsActive).
                HasDefaultValue(true);

            modelBuilder.
                Entity<Contribution>().
                HasMany(c => c.Files).
                WithOne(f => f.Contribution).
                OnDelete(DeleteBehavior.Cascade);
            #endregion

        }
    }
}
