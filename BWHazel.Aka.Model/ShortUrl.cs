using System.ComponentModel.DataAnnotations;

namespace BWHazel.Aka.Model;

/// <summary>
/// Represents a short URL.
/// </summary>
public class ShortUrl
{
    /// <summary>
    /// Gets or sets the short URL ID.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the title.
    /// </summary>
    [Required(ErrorMessage = "A title is required.")]
    public string Title { get; set; }

    /// <summary>
    /// Gets or sets the URL.
    /// </summary>
    [Required(ErrorMessage = "A valid URL is required.")]
    [Url]
    public string Url { get; set; }

    /// <summary>
    /// Gets or sets the ID of the user which created the link.
    /// </summary>
    public string UserId { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether a link is public.
    /// </summary>
    public bool IsPublic { get; set; }
}