using Microsoft.EntityFrameworkCore;
using AMaz.Entity;

namespace AMaz.DB
{
    public class AMazDbContext : DbContext
    {
        public DbSet<AcademicYear> AcademicYears { get; set; }
        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<Magazine> Magazines { get; set; }
        public DbSet<TermAndCondition> TermAndConditions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Contribution> Contributions { get; set;}

        public AMazDbContext(DbContextOptions<AMazDbContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // If Delete Mangazine ==> Cascade all the contribution
            modelBuilder.
                Entity<Magazine>().
                HasMany( m => m.Contributions).
                WithOne( c => c.Magazine).
                OnDelete(DeleteBehavior.Cascade);

            // If Delete Term ==> Just set the term in contribution to null
            modelBuilder.
                Entity<TermAndCondition>().
                HasMany(m => m.Contributions).
                WithOne(c => c.TermAndCondition).
                OnDelete(DeleteBehavior.SetNull);

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


            base.OnModelCreating(modelBuilder);

        }
    }
}
