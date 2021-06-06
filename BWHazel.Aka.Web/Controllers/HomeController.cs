using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BWHazel.Aka.Web.ViewModels;

namespace BWHazel.Aka.Web.Controllers
{
    /// <summary>
    /// The home controller.
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger<HomeController> logger;

        /// <summary>
        /// Initialises a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public HomeController(ILogger<HomeController> logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Returns the home page view.
        /// </summary>
        /// <returns>The home page view.</returns>
        public IActionResult Index()
        {
            return this.View();
        }

        /// <summary>
        /// Returns the error view.
        /// </summary>
        /// <returns>The error view.</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}
