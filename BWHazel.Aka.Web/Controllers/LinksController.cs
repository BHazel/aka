using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        /// <summary>
        /// Returns the create link view.
        /// </summary>
        /// <returns>The create link view.</returns>
        [HttpGet]
        public IActionResult Create()
        {
            return this.View();
        }

        /// <summary>
        /// Add a new link to the database.
        /// </summary>
        /// <param name="link">The link to add.</param>
        /// <returns>A redirection to the links index page.</returns>
        [HttpPost]
        public async Task<IActionResult> Create(ShortUrl link)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            this.dbContext.ShortUrls
                .Add(link);

            await this.dbContext.SaveChangesAsync();
            return this.RedirectToAction("Index");
        }

        /// <summary>
        /// Returns the open link view.
        /// </summary>
        /// <param name="linkId">The link ID.</param>
        /// <returns>The open link view.</returns>
        public IActionResult Open(string linkId)
        {
            ShortUrl link =
                this.dbContext.ShortUrls
                .FirstOrDefault(s => s.Id == linkId);

            return this.View(link);
        }
    }
}
