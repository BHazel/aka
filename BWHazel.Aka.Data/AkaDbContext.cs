using Microsoft.EntityFrameworkCore;
using BWHazel.Aka.Model;

namespace BWHazel.Aka.Data
{
    /// <summary>
    /// Aka short URL database context.
    /// </summary>
    public class AkaDbContext : DbContext
    {
        private const string CosmosDbContainerName = "AkaShortUrls";

        /// <summary>
        /// Initialise a new instance of the <see cref="AkaDbContext"/> class.
        /// </summary>
        /// <param name="options">The database context options.</param>
        public AkaDbContext(DbContextOptions<AkaDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Initialises and configures the model.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ShortUrl>()
                .ToContainer(CosmosDbContainerName)
                .HasPartitionKey(s => s.Url)
                .HasNoDiscriminator();

            modelBuilder.Entity<ShortUrl>()
                .Property(s => s.Id)
                .ToJsonProperty("id");

            modelBuilder.Entity<ShortUrl>()
                .Property(s => s.Title)
                .ToJsonProperty("title");

            modelBuilder.Entity<ShortUrl>()
                .Property(s => s.Url)
                .ToJsonProperty("url");

            modelBuilder.Entity<ShortUrl>()
                .Property(s => s.UserId)
                .ToJsonProperty("userId");

            modelBuilder.Entity<ShortUrl>()
                .Property(s => s.IsPublic)
                .ToJsonProperty("isPublic");
        }

        /// <summary>
        /// Gets or sets the short URLs.
        /// </summary>
        public DbSet<ShortUrl> ShortUrls { get; set; }
    }
}
