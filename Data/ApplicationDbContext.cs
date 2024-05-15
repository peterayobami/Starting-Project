using Microsoft.EntityFrameworkCore;

namespace Starting_Project
{
    /// <summary>
    /// The database representational model for the application
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="options"></param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        #endregion

        #region Db Sets

        /// <summary>
        /// The program database container
        /// </summary>
        public DbSet<ProgramDataModel> Programs { get; set; }

        /// <summary>
        /// The applicant database container
        /// </summary>
        public DbSet<ApplicationDataModel> Applications { get; set; }

        /// <summary>
        /// The system logs database container
        /// </summary>
        public DbSet<SystemLogDataModel> SystemLogs { get; set; }

        #endregion

        public override int SaveChanges()
        {
            // For each entry...
            foreach (var entry in ChangeTracker.Entries<BaseDataModel>())
            {
                // If data is being created...
                if (entry.State == EntityState.Added)
                {
                    // Set date created
                    entry.Entity.DateCreated = DateTime.UtcNow;

                    // Set date modified
                    entry.Entity.DateModified = DateTime.UtcNow;
                }

                // If data is being modified...
                if (entry.State == EntityState.Modified)
                {
                    // Set date modified
                    entry.Entity.DateModified = DateTime.UtcNow;
                }
            }

            return base.SaveChanges();
        }

        public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // For each entry...
            foreach (var entry in ChangeTracker.Entries<BaseDataModel>())
            {
                // If data is being created...
                if (entry.State == EntityState.Added)
                {
                    // Set date created
                    entry.Entity.DateCreated = DateTime.Now;

                    // Set date modified
                    entry.Entity.DateModified = DateTime.Now;
                }

                // If data is being modified...
                if (entry.State == EntityState.Modified)
                {
                    // Set date modified
                    entry.Entity.DateModified = DateTime.Now;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Map containers and set partition key
            modelBuilder.Entity<ProgramDataModel>().ToContainer(nameof(Programs)).HasPartitionKey(e => e.ProgramId).HasNoDiscriminator();
            modelBuilder.Entity<ApplicationDataModel>().ToContainer(nameof(Applications)).HasPartitionKey(e => e.ProgramId).HasNoDiscriminator();
            modelBuilder.Entity<SystemLogDataModel>().ToContainer(nameof(SystemLogs)).HasPartitionKey(e => e.ProgramId).HasNoDiscriminator();

            base.OnModelCreating(modelBuilder);
        }
    }
}