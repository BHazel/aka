using Microsoft.EntityFrameworkCore;

namespace BWHazel.Aka.Data
{
    public class AkaDbContext : DbContext
    {
        /// <summary>
        /// Initialise a new instance of the <see cref="AkaDbContext"/> class.
        /// </summary>
        /// <param name="options">The database context options.</param>
        public AkaDbContext(DbContextOptions<AkaDbContext> options)
            : base(options)
        {
        }
    }
}
