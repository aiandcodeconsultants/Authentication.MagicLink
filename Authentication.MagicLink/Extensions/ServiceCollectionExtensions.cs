using System.Reflection;
using Authentication.MagicLink.Tests.Services;
using Microsoft.AspNetCore.Mvc.Razor.Extensions;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;

namespace Authentication.MagicLink.Extensions;

public static class ServiceCollectionExtensions
{
    public const string JwtOrCookiePolicy = "JWT_OR_COOKIE";
    public static IServiceCollection AddAuthenticationMagicLink(this IServiceCollection services, IConfiguration configuration, bool addRazorTemplateService = true, bool addHttpContextAccessor = true, string policyName = JwtOrCookiePolicy)
    {
        var magicLinkSection = configuration.GetSection("MagicLink");
        //var magicLinkOptions = configuration.GetValue<MagicLinkSettings>("MagicLink");
        var magicLinkOptions = new MagicLinkSettings();

        // Register options
        services.Configure<MagicLinkSettings>(magicLinkSection);

        var magicLinkSchema = magicLinkOptions.Schema;
        var bearerSchema = magicLinkOptions.BearerSchema;
        var jwtIssuer = magicLinkOptions.Issuer;
        var jwtAudience = magicLinkOptions.Audience;
        var jwtSecretKey = magicLinkOptions.SecretKey;

        // Register services
        services.AddSingleton<IUserService, UserService>();
        services.AddSingleton<IMagicLinkService, MagicLinkService>();
        services.AddSingleton<ITokenGenerator, JwtTokenGenerator>();
        services.AddSingleton<ITokenValidator, JwtTokenValidator>();
        //services.AddSingleton<IRenderTemplate, RazorTemplateService>();
        if(addRazorTemplateService) services.AddRazorTemplateService();
        if (addHttpContextAccessor) services.AddHttpContextAccessor();
        
        // Configure email services
        configuration.GetSection("MagicLink").Bind(magicLinkOptions);
        var emailServiceType = magicLinkOptions.EmailServiceType;
        switch (emailServiceType)
        {
            case EmailServiceType.Smtp:
                services.AddSingleton<IEmailService, SmtpEmailService>();
                break;
            case EmailServiceType.SendGrid:
                services.AddSingleton<IEmailService, SendGridEmailService>();
                break;
            case EmailServiceType.MailKit:
                services.AddSingleton<IEmailService, MailKitEmailService>();
                break;
            default:
                throw new ArgumentException($"Invalid email service type: {emailServiceType}");
        }

        // Configure distributed cache
        if (magicLinkOptions.UseDistributedCache)
        {
            services.AddDistributedMemoryCache(); // Use distributed memory cache for testing purposes
            // Replace the above line with the appropriate distributed cache implementation (e.g., Redis)
        }
        else
        {
            services.AddMemoryCache();
        }

        // Register RazorProjectEngine
        services.AddSingleton(provider =>
        {
            var fileSystem = RazorProjectFileSystem.Create(".");
            var projectEngineBuilder = RazorProjectEngine.Create(RazorConfiguration.Default, fileSystem, builder =>
            {
                RazorExtensions.Register(builder);

                // Additional customization of the builder, if needed
            });

            return projectEngineBuilder;
        });

        services.AddAuthorization(options => {
            // See https://learn.microsoft.com/en-us/aspnet/core/security/authorization/policies?view=aspnetcore-7.0
            options.AddPolicy("AdminOnly", policy => {
                policy.RequireRole("Administrator");
            });
            options.AddPolicy("Authenticated", policy => policy.RequireAuthenticatedUser());
        });

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = magicLinkSchema;
            options.DefaultSignInScheme = magicLinkSchema;
            //options.DefaultChallengeScheme = magicLinkSchema;
            options.DefaultScheme = policyName;
            options.DefaultChallengeScheme = policyName;
        })
        .AddCookie(magicLinkSchema, options =>
        {
            options.Cookie.Name = "MagicLink.Auth";
            options.LoginPath = "/login";
            options.LogoutPath = "/logout";
            //options.ClaimsIssuer = magicLinkOptions.Issuer;
        })
        .AddJwtBearer(bearerSchema, options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtIssuer,
                ValidAudience = jwtAudience,
                IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(jwtSecretKey)),
            };
        })
        // this is the key piece for cookies + jwt/bearer - See https://weblog.west-wind.com/posts/2022/Mar/29/Combining-Bearer-Token-and-Cookie-Auth-in-ASPNET
        .AddPolicyScheme(policyName, "MagicLink JWT or Cookie", options =>
        {
            // runs on each request
            options.ForwardDefaultSelector = context =>
            {
                // filter by auth type
                string? authorization = context.Request.Headers[HeaderNames.Authorization];
                if (!string.IsNullOrEmpty(authorization) && authorization.StartsWith("Bearer "))
                    return bearerSchema;

                // otherwise always check for cookie auth
                return magicLinkSchema;
            };
        })
        ;

        return services;
    }

    public static IServiceCollection AddRazorTemplateService(this IServiceCollection services)
    {
        var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        services.AddSingleton<IRenderTemplate>(new RazorTemplateService(basePath!));
        return services;
    }
}