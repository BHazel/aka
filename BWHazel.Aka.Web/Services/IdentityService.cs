using System.Linq;
using System.Security.Claims;

namespace BWHazel.Aka.Web.Services;

/// <summary>
/// Works with identities and claims.
/// </summary>
public class IdentityService
{
    private const string UserIdClaimType = "http://schemas.microsoft.com/identity/claims/objectidentifier";
    private const string UserNameClaimType = "name";

    /// <summary>
    /// Gets the user ID.
    /// </summary>
    /// <param name="user">The user.</param>
    /// <returns>The user ID.</returns>
    public string GetUserId(ClaimsPrincipal user)
    {
        string userId = this.GetClaimValue(user, UserIdClaimType);
        return userId;
    }

    /// <summary>
    /// Gets the user name.
    /// </summary>
    /// <param name="user">The user.</param>
    /// <returns>The user name.</returns>
    public string GetUserName(ClaimsPrincipal user)
    {
        string userName = this.GetClaimValue(user, UserNameClaimType);
        return userName;
    }

    /// <summary>
    /// Gets a claim value for a user.
    /// </summary>
    /// <param name="user">The user.</param>
    /// <param name="claim">The claim.</param>
    /// <returns>The claim value.</returns>
    public string GetClaimValue(ClaimsPrincipal user, string claim)
    {
        string claimValue =
            user.Claims
                .Where(c => c.Type == claim)
                .Select(c => c.Value)
                .First();

        return claimValue;
    }
}