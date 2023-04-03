using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Authentication.MagicLink.Extensions;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAuthenticationMagicLink(this IServiceCollection services, IConfiguration configuration)
        {
            // Register options
            var magicLinkOptions = new MagicLinkOptions();
            configuration.GetSection("MagicLinkOptions").Bind(magicLinkOptions);
            services.AddSingleton(magicLinkOptions);

            // Register services
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IMagicLinkService, MagicLinkService>();
            services.AddSingleton<ITokenGenerator, JwtTokenGenerator>();
            services.AddSingleton<ITokenValidator, JwtTokenValidator>();
            services.AddSingleton<IRenderTemplate, RazorTemplateService>();

            // Configure email services
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

            return services;
        }
    }