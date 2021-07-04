using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using BWHazel.Aka.Data;
using BWHazel.Aka.Model;
using BWHazel.Aka.Web.Services;

namespace BWHazel.Aka.Web.Controllers
{
    /// <summary>
    /// The links controller.
    /// </summary>
    [Authorize(Policy = "Aka.BWHazel/Administrator")]
    public class LinksController : Controller
    {
        private readonly ILogger<LinksController> logger;
        private readonly IMemoryCache memoryCache;
        private readonly AkaDbContext dbContext;
        private readonly IdentityService identityService;
        private readonly ShortUrlService shortUrlService;
        private readonly DataService dataService;

        /// <summary>
        /// Initialises a new instance of the <see cref="LinksController"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="memoryCache">The memory cache.</param>
        /// <param name="dbContext">The database context.</param>
        /// <param name="identityService">The identity service.</param>
        /// <param name="shortUrlService">The short URL service.</param>
        public LinksController(
            ILogger<LinksController> logger,
            IMemoryCache memoryCache,
            AkaDbContext dbContext,
            IdentityService identityService,
            ShortUrlService shortUrlService,
            DataService dataService)
        {
            this.logger = logger;
            this.memoryCache = memoryCache;
            this.dbContext = dbContext;
            this.identityService = identityService;
            this.shortUrlService = shortUrlService;
            this.dataService = dataService;
        }

        /// <summary>
        /// Returns the links index view.
        /// </summary>
        /// <returns>The links index view.</returns>
        [AllowAnonymous]
        public IActionResult Index()
        {
            List<ShortUrl> links = this.dataService.GetAllShortUrls();
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

            if (string.IsNullOrWhiteSpace(link.Id))
            {
                link.Id = this.shortUrlService.GenerateShortUrlCode();
            }

            link.UserId = this.identityService.GetUserId(this.User);
            await this.dataService.AddShortUrl(link);

            return this.RedirectToAction("Index");
        }

        /// <summary>
        /// Returns the edit link view.
        /// </summary>
        /// <param name="linkId"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Edit(string linkId)
        {
            ShortUrl link = this.dataService.GetShortUrl(linkId);
            if (link == null)
            {
                return this.NotFound();
            }

            return this.View(link);
        }

        /// <summary>
        /// Edits a link in the database.
        /// </summary>
        /// <param name="link">The link to edit.</param>
        /// <returns>A redirection to the links index page.</returns>
        [HttpPost]
        public IActionResult Edit(ShortUrl link)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(link);
            }

            this.dbContext.ShortUrls
                .Update(link);

            this.dbContext.SaveChanges();
            return this.RedirectToAction("Index");
        }

        /// <summary>
        /// Returns the delete link view.
        /// </summary>
        /// <param name="linkId">The link ID.</param>
        /// <returns>The delete link view.</returns>
        [HttpGet]
        public IActionResult Delete(string linkId)
        {
            ShortUrl link = this.dataService.GetShortUrl(linkId);
            if (link == null)
            {
                return this.NotFound();
            }

            return this.View(link);
        }

        /// <summary>
        /// Deletes a link from the database.
        /// </summary>
        /// <param name="link">The link to delete.</param>
        /// <returns>A redirection to the links index page.</returns>
        [HttpPost]
        public IActionResult Delete(ShortUrl link)
        {
            this.dbContext.ShortUrls
                .Remove(link);

            this.dbContext.SaveChanges();
            return this.RedirectToAction("Index");
        }

        /// <summary>
        /// Returns the open link view.
        /// </summary>
        /// <param name="linkId">The link ID.</param>
        /// <returns>The open link view.</returns>
        [AllowAnonymous]
        public IActionResult Open(string linkId)
        {
            ShortUrl link = this.dataService.GetShortUrl(linkId);
            return this.View(link);
        }
    }
}
