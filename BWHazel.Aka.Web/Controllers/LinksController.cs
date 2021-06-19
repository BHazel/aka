using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BWHazel.Aka.Data;
using BWHazel.Aka.Model;

namespace BWHazel.Aka.Web.Controllers
{
    /// <summary>
    /// The links controller.
    /// </summary>
    public class LinksController : Controller
    {
        private readonly ILogger<LinksController> logger;
        private readonly AkaDbContext dbContext;

        /// <summary>
        /// Initialises a new instance of the <see cref="LinksController"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="dbContext">The database context.</param>
        public LinksController(ILogger<LinksController> logger, AkaDbContext dbContext)
        {
            this.logger = logger;
            this.dbContext = dbContext;
        }

        /// <summary>
        /// Returns the links index view.
        /// </summary>
        /// <returns>The links index view.</returns>
        public IActionResult Index()
        {
            List<ShortUrl> links =
                this.dbContext.ShortUrls
                    .ToList();

            return this.View(links);
        }
    }
}
