using LinkUp_REST_API.Models.Pending;
using LinkUp_REST_API.Models;
using Microsoft.EntityFrameworkCore;

namespace LinkUp_REST_API.Data.DbContextConnections
{
    /*
     * TODO: Consider to use another name than 'DataContext' that indicates a generic dbcontext class
     */

    public class DataContext : DbContext
    {
        /*
         * Constructor required by test-container, I cannot configure connection-string from DI, it doesn't work with test-containers in xUnit > WebApplicationFactory
         */
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        /*
         * Guide for configuring to MSSQL
         * Create database
         * 1. Tools > NuGet Package Manager > Package Manager Console
         * 2. type: add-migration [migration-name]
         * 3. check migration file
         * 4. type: update-database
         * Open in SSMS (Microsoft SQL Server Management Studio)
         * 5. Server type: Database engine, Server name: (localdb)\MSSQLLocalDB, Authentication: Windows Authentication
         * 6. No username or password
         * 7. Click "Connect"
         * 8. Navigate to: Databases > Hovedopgave
         */
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Hovedopgave;Integrated Security=SSPI;Trust Server Certificate=True");
        }


        public DbSet<Account> Accounts { get; set; }
        public DbSet<AudienceSpecification> AudienceSpecifications { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<Keyword> Keywords { get; set; }
        public DbSet<PersonInformation> PersonInformation { get; set; }
        public DbSet<Pitch> Pitches { get; set; }
        public DbSet<Portfolio> PortfolioItems { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Media> Medias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Account to Profile (One-to-many relationship)
            modelBuilder.Entity<Account>()
                .HasMany(a => a.Profiles)
                .WithOne(p => p.Account)
                .HasForeignKey(p => p.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            // Account to PerfonInformation (One-to-one relationship)
            modelBuilder.Entity<Account>()
                .HasOne(p => p.PersonInformation)
                .WithOne()
                .HasForeignKey<PersonInformation>(p => p.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            // Profile to Keyword (One-to-one relationship)
            modelBuilder.Entity<Profile>()
                .HasOne(p => p.Keyword)
                .WithOne(k => k.Profile)
                .HasForeignKey<Keyword>(k => k.ProfileId)
                .OnDelete(DeleteBehavior.Cascade);  

            // Profile to Portfolio (One-to-one relationship)
            modelBuilder.Entity<Profile>()
                .HasOne(p => p.Portfolio)
                .WithOne(po => po.Profile)
                .HasForeignKey<Portfolio>(po => po.ProfileId)
                .OnDelete(DeleteBehavior.Cascade);  

            // Profile to Pitch (One-to-many relationship)
            modelBuilder.Entity<Profile>()
                .HasMany(p => p.Pitches)
                .WithOne(pi => pi.Profile)
                .HasForeignKey(pi => pi.ProfileId)
                .OnDelete(DeleteBehavior.Cascade);  

            // Profile to AudienceSpecification (One-to-one relationship)
            modelBuilder.Entity<Profile>()
                .HasOne(p => p.AudienceSpecification)
                .WithOne(a => a.Profile)
                .HasForeignKey<AudienceSpecification>(a => a.ProfileId)
                .OnDelete(DeleteBehavior.Cascade);  

            modelBuilder.Entity<Education>()
                .HasOne(e => e.Keyword)
                .WithOne(k => k.Education)
                .HasForeignKey<Education>(e => e.KeywordId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
