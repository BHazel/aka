using System;
using Microsoft.Extensions.Configuration;
using DotNext;

namespace BWHazel.Aka.Web.Services;

/// <summary>
/// Performs short URL operations.
/// </summary>
public class ShortUrlService
{
    private const string ShortUrlCharactersKey = "Site:ShortCodeCharacters";
    private const string ShortUrlLengthKey = "Site:ShortCodeLength";

    private readonly IConfiguration configuration;

    /// <summary>
    /// Initialises a new instance of the <see cref="ShortUrlService"/> class.
    /// </summary>
    /// <param name="configuration">The application configuration.</param>
    public ShortUrlService(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    /// <summary>
    /// Generates a short URL code based on application configuration.
    /// </summary>
    /// <returns>A short URL code.</returns>
    public string GenerateShortUrlCode()
    {
        string shortUrlCharacters = this.configuration[ShortUrlCharactersKey];
        int shortUrlLength = int.Parse(this.configuration[ShortUrlLengthKey]);

        Random random = new();
        return random.NextString(shortUrlCharacters, shortUrlLength);
    }
}