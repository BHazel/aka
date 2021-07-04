using System.Collections.Generic;
using System.Linq;
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
    }
}
