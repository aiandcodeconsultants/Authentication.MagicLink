using System.Security.Claims;

namespace Authentication.MagicLink.Extensions;

public static class UserExtensions
{
    public static ClaimsPrincipal ToClaimsPrincipal(this User user, string schema = "MagicLink")
        => new ClaimsPrincipal(
            new ClaimsIdentity(new Claim[] { 
                new Claim(ClaimTypes.Role, "Administrator"), 
                //new Claim(ClaimTypes.NameIdentifier, UserValidationFacade.GetUsername(),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Authentication, schema),
            }
        , ClaimTypes.Authentication, ClaimTypes.NameIdentifier, ClaimTypes.Role));
}
