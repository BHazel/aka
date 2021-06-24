using Microsoft.AspNetCore.Mvc;

namespace BWHazel.Aka.Web.Controllers
{
    /// <summary>
    /// The account controller.
    /// </summary>
    public class AccountController : Controller
    {
        /// <summary>
        /// Returns the access denied view.
        /// </summary>
        /// <returns>The access denied view.</returns>
        public IActionResult AccessDenied()
        {
            return this.View();
        }
    }
}
