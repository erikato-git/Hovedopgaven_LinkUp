using Microsoft.EntityFrameworkCore;
using REST_API.Models;

namespace REST_API.Data
{
    public class MssqlContext : DbContext
    {
        /*
         * DbContextOptions<T> enables me to configure settings for DataContext outside the class in Program.cs
         */
        public MssqlContext(DbContextOptions<MssqlContext> options): base(options) 
        { 
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Hovedopgave;Integrated Security=SSPI;Trust Server Certificate=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Account to Profile (One-to-many relationship)
            modelBuilder.Entity<Account>()
                .HasMany(a => a.Profiles)
                .WithOne(p => p.Account)
                .HasForeignKey(p => p.AccountId)
                .OnDelete(DeleteBehavior.Cascade);  // Cascade deletion for composition

            // Profile to Keyword (One-to-one relationship)
            modelBuilder.Entity<Profile>()
                .HasOne(p => p.Keyword)
                .WithOne(k => k.Profile)
                .HasForeignKey<Keyword>(k => k.ProfileId)
                .OnDelete(DeleteBehavior.Cascade);  // Cascade deletion for composition

            // Profile to Portfolio (One-to-one relationship)
            modelBuilder.Entity<Profile>()
                .HasOne(p => p.Portfolio)
                .WithOne(po => po.Profile)
                .HasForeignKey<Portfolio>(po => po.ProfileId)
                .OnDelete(DeleteBehavior.Cascade);  // Cascade deletion for composition

            // Profile to Pitch (One-to-many relationship)
            modelBuilder.Entity<Profile>()
                .HasMany(p => p.Pitches)
                .WithOne(pi => pi.Profile)
                .HasForeignKey(pi => pi.ProfileId)
                .OnDelete(DeleteBehavior.SetNull);  // Pitches not destroyed when the profile is destroyed

            // Profile to AudienceSpecification (One-to-one relationship)
            modelBuilder.Entity<Profile>()
                .HasOne(p => p.AudienceSpecification)
                .WithOne(a => a.Profile)
                .HasForeignKey<AudienceSpecification>(a => a.ProfileId)
                .OnDelete(DeleteBehavior.Cascade);  // Cascade deletion for composition

            // Keyword to Education (One-to-one relationship)
            modelBuilder.Entity<Keyword>()
                .HasOne(e => e.Education)
                .WithOne(k => k.Keyword)
                .HasForeignKey<Keyword>(k => k.EducationId)
                .OnDelete(DeleteBehavior.Cascade);  // Cascade deletion for composition
        }


        public DbSet<Account> Accounts { get; set; }
        public DbSet<AudienceSpecification> AudienceSpecifications { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<Keyword> Keywords { get; set; }
        public DbSet<PersonInformation> PersonInformation { get; set; }
        public DbSet<Pitch> Pitches { get; set; }
        public DbSet<Portfolio> PortfolioItems { get; set; }
        public DbSet<Profile> Profiles { get; set; }
    }
}
