using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BWHazel.Aka.Web.Controllers
{
    /// <summary>
    /// The links controller.
    /// </summary>
    public class LinksController : Controller
    {
        private readonly ILogger<LinksController> logger;

        /// <summary>
        /// Initialises a new instance of the <see cref="LinksController"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public LinksController(ILogger<LinksController> logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Returns the links index view.
        /// </summary>
        /// <returns>The links index view.</returns>
        public IActionResult Index()
        {
            return this.View();
        }
    }
}
