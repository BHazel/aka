namespace BWHazel.Aka.Web.ViewModels
{
    /// <summary>
    /// Represents an error on a view.
    /// </summary>
    public class ErrorViewModel
    {
        /// <summary>
        /// Gets or sets the request ID.
        /// </summary>
        public string RequestId { get; set; }

        /// <summary>
        /// Gets value indicating whether the request ID should be shown.
        /// </summary>
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
