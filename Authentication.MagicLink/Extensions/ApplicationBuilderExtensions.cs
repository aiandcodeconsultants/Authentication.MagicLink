using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Net.Http.Headers;
using System.Security.Claims;
namespace Authentication.MagicLink.Extensions;

/// <summary>A class providing extension methods for <see cref="IApplicationBuilder"/> instances.</summary>
public static class ApplicationBuilderExtensions
{
    public static IEndpointRouteBuilder MapMagicLink(
        this IEndpointRouteBuilder app,
        string loginPath = "/login",
        string logoutPath = "/logout",
        string userInfoPath = "/user/me",
        string magicLinkPath = "/magiclink")
    {
        // Login GET request
        app.MapGet(loginPath, () =>
        {
            string html = @"
                <html>
                    <head>
                        <title>Login</title>
                    </head>
                    <body>
                        <form method=""post"" action=""/login"">
                            <label for=""email"">Email:</label>
                            <input type=""email"" id=""email"" name=""email"" required>
                            <button type=""submit"">Send Magic Link</button>
                        </form>
                    </body>
                </html>";
            return Results.Content(html, MediaTypeHeaderValue.Parse("text/html"));
        });

        // Login POST request
        app.MapPost(loginPath, async ([FromServices] IHttpContextAccessor contextAccessor, [FromServices] IMagicLinkService magicLinkService) =>
        {
            var form = contextAccessor.HttpContext!.Request.Form;
            var email = form["Email"];
            // Use IMagicLinkService to generate magic link
            string magicLink = await magicLinkService.GenerateMagicLinkAsync(email);

            return Results.Content($"Magic link sent to {email}<br/><div><a href=\"/\">Home</a></div>", "text/html");
        });

        app.MapGet(logoutPath, async context =>
        {
            await context.SignOutAsync();
            context.Response.Redirect("/");
        });

        app.MapGet(userInfoPath, async ([FromServices] IUserService userService, [FromServices]IHttpContextAccessor httpContextAccessor) =>
        {
            var claimsPrincipal = httpContextAccessor.HttpContext.User;
            var user = await userService.GetUserByIdAsync(claimsPrincipal.Identity.Name);
            var userByEmail = await userService.GetUserByEmailAsync(claimsPrincipal.Claims.First(c => c.Type == ClaimTypes.Email).Value);
            return Results.Content(System.Text.Json.JsonSerializer.Serialize(user ?? userByEmail), "application/json");
        }).RequireAuthorization("Authenticated");

        // Magic link GET request
        app.MapGet(magicLinkPath, async ([FromServices] IMagicLinkService magicLinkService, [FromServices] IHttpContextAccessor httpContextAccessor, [FromQuery] string token, [FromQuery] string redirectUrl = "/") =>
        {
            var context = httpContextAccessor.HttpContext;
            if (!string.IsNullOrEmpty(token))
            {
                bool isValid = await magicLinkService.ValidateMagicLinkAsync(token);

                if (isValid)
                {
                    await context.SignInAsync(context.User);
                    context.Response.Redirect(redirectUrl);
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Invalid magic link token");
                }
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync("Token is missing");
            }
        });

        return app;
    }
}
