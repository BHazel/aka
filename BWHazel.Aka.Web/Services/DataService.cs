using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using BWHazel.Aka.Data;
using BWHazel.Aka.Model;

namespace BWHazel.Aka.Web.Services
{
    /// <summary>
    /// Works with data.
    /// </summary>
    public class DataService
    {
        private const string DataCacheKey = "Site:DataCacheKey";

        private readonly IConfiguration configuration;
        private readonly IMemoryCache memoryCache;
        private readonly AkaDbContext dbContext;

        /// <summary>
        /// Initialises a new instance of the <see cref="AkaDbContext"/> class.
        /// </summary>
        /// <param name="memoryCache">The memory cache.</param>
        /// <param name="dbContext">The database context.</param>
        public DataService(IConfiguration configuration, IMemoryCache memoryCache, AkaDbContext dbContext)
        {
            this.configuration = configuration;
            this.memoryCache = memoryCache;
            this.dbContext = dbContext;
        }

        /// <summary>
        /// Gets all short URLs.
        /// </summary>
        /// <remarks>
        /// Data retrieval is first attempted from the cache.  If not present then retrieval is from
        /// the database and the cache is set.
        /// </remarks>
        /// <returns>A collection of all short URLs.</returns>
        public List<ShortUrl> GetAllShortUrls()
        {
            string dataCacheKey = this.configuration[DataCacheKey];

            List<ShortUrl> links;
            this.memoryCache.TryGetValue(dataCacheKey, out links);

            if (links == null)
            {
                links =
                    this.dbContext.ShortUrls
                        .ToList();

                this.memoryCache.Set(dataCacheKey, links);
            }

            return links;
        }

        /// <summary>
        /// Get a single short URL from its ID.
        /// </summary>
        /// <param name="linkId">The link ID.</param>
        /// <returns>A short URL.</returns>
        public ShortUrl GetShortUrl(string linkId)
        {
            string dataCacheKey = this.configuration[DataCacheKey];

            ShortUrl link =
                this.GetAllShortUrls()
                .FirstOrDefault(s => s.Id == linkId);

            return link;
        }

        /// <summary>
        /// Adds a new short URL.
        /// </summary>
        /// <remarks>
        /// Once the database is updated the cache is reset with the updated collection of short URLs.
        /// </remarks>
        /// <param name="link">The short URL to add.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task AddShortUrl(ShortUrl link)
        {
            string dataCacheKey = this.configuration[DataCacheKey];

            this.dbContext.ShortUrls
                .Add(link);

            await this.dbContext.SaveChangesAsync();
            this.memoryCache.Set(dataCacheKey, this.dbContext.ShortUrls.ToList());
        }
    }
}
